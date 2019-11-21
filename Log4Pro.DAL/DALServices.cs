using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Log4Pro.IS.TRM.DAL
{
    /// <summary>
    /// Általános DAL szolgáltatások a context bővitőmetodusaiként megvalósítva
    /// </summary>
    public static class DALServices
    {
        /// <summary>
        /// Érkeztet (betárol) egy szállítási egységet
        /// </summary>
        /// <param name="dbc">adatbázis context</param>
        /// <param name="tekedInShippingUnit">ezt a beérkeztetett szállítási efgységet tárolja be</param>
        /// <param name="storeLocation">erre a tároló lokációra</param>
        /// <returns>A létrehozott betárolt beszállítói egység</returns>
        public static ShippingUnit Receive(this ISTRMContext dbc, ShippingUnit tekedInShippingUnit, string storeLocation)
        {
            var transaction = new Transaction()
            {
                Timestamp = DateTime.Now,
                Type = TransactionType.SUReceive,
            };
            tekedInShippingUnit.Active = false;
            tekedInShippingUnit.CloserTransaction = transaction;
            var receivedShippingUnit = new ShippingUnit()
            {
                Active = true,
                CreaterTransaction = transaction,
                ExternalShippingUnitId = tekedInShippingUnit.ExternalShippingUnitId,
                Part = tekedInShippingUnit.Part,
                Quantity = tekedInShippingUnit.Quantity,
                ShippingUnitId = tekedInShippingUnit.ShippingUnitId,
                Status = ShippingUnitStatus.Received,
                StoreLocation = storeLocation,
            };
            dbc.Transactions.Add(transaction);
            dbc.ShippingUnits.Add(receivedShippingUnit);
            dbc.SaveChanges();
            return receivedShippingUnit;
        }

        /// <summary>
        /// Átcsomagolás művelet
        /// </summary>
        /// <param name="dbc">adatbázis context</param>
        /// <param name="shippingUnit">beszállítói egység, ahonnan az átcsomagolás történik</param>
        /// <param name="packagingUnitId">a csomagolási egység, ahová átrakunk</param>
        /// <param name="qty">az átrakott mennyiség</param>
        /// <returns>a létrejött csomagolási egység entítás</returns>
        public static PackagingUnit RePack(this ISTRMContext dbc, ShippingUnit shippingUnit, string packagingUnitId, int qty)
        {
            var transaction = new Transaction()
            {
                Timestamp = DateTime.Now,
                Type = TransactionType.PULoad,
            };
            if (shippingUnit.Quantity < qty)
            {
                throw new Exception($"This shipping unit ({shippingUnit.ShippingUnitId}) is not contain enought qty ({qty})!");
            }
            var packagingUnit = dbc.PackagingUnits.FirstOrDefault(x => x.PackageUnitId == packagingUnitId
                                                                        && x.Active &&
                                                                        x.Status == PackagingUnitStatus.Created);
            int existingQty = 0;
            if (packagingUnit != null)
            {
                existingQty = packagingUnit.Quantity;
                if (shippingUnit.PartId != packagingUnit.PartId)
                {
                    throw new Exception($"This package unit ({packagingUnitId}) contains another partnumber! Repacking not enabled!");
                }
                packagingUnit.Active = false;
                packagingUnit.CloserTransaction = transaction;
            }
            var newPackagingUnit = new PackagingUnit()
            {
                    Status = PackagingUnitStatus.Created,
                    Active = true,
                    CreaterTransaction = transaction,
                    PackageUnitId = packagingUnitId,
                    Part = shippingUnit.Part,
                    Quantity = existingQty + qty,                    
            };
            shippingUnit.Active = false;
            shippingUnit.CloserTransaction = transaction;
            var newShippingUnit = new ShippingUnit()
            {
                Active = true,
                CreaterTransaction = transaction,
                ExternalShippingUnitId = shippingUnit.ExternalShippingUnitId,
                Part = shippingUnit.Part,
                Quantity = shippingUnit.Quantity - qty,
                ShippingUnitId = shippingUnit.ShippingUnitId,
                Status = ShippingUnitStatus.Received,
                StoreLocation = shippingUnit.StoreLocation,                
            };
            dbc.Transactions.Add(transaction);
            dbc.ShippingUnits.Add(newShippingUnit);
            dbc.PackagingUnits.Add(newPackagingUnit);
            dbc.SaveChanges();
            return newPackagingUnit;
        }

        /// <summary>
        /// Kitárol egy csomagolási egységet a raktárból
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="storedPackagingUnit"></param>
        /// <returns></returns>
        public static PackagingUnit PutOut(this ISTRMContext dbc, PackagingUnit storedPackagingUnit)
        {
            var transaction = new Transaction()
            {
                Timestamp = DateTime.Now,
                Type = TransactionType.PUPutOut,
            };
            storedPackagingUnit.Active = false;
            storedPackagingUnit.CloserTransaction = transaction;
            var putOutPackagingUnit = new PackagingUnit()
            {
                Active = true,
                CreaterTransaction = transaction,
                Part = storedPackagingUnit.Part,
                Quantity = storedPackagingUnit.Quantity,
                PackageUnitId = storedPackagingUnit.PackageUnitId,
                Status = PackagingUnitStatus.PutOut,                
            };
            dbc.Transactions.Add(transaction);
            dbc.PackagingUnits.Add(putOutPackagingUnit);
            dbc.SaveChanges();
            return putOutPackagingUnit;
        }

        /// <summary>
        /// Betárolás kanban állványra
        /// </summary>
        /// <param name="dbc">adatbázis context</param>
        /// <param name="storedPackagingUnit">kanban állványra betárolt csomagolási egység azonosítója</param>
        public static void KanbanStoreIn(this ISTRMContext dbc, PackagingUnit packagingUnit)
        {
            var transaction = new Transaction()
            {
                Timestamp = DateTime.Now,
                Type = TransactionType.PUKanbanIn,
            };
            packagingUnit.Active = false;
            packagingUnit.CloserTransaction = transaction;
            var newPackagingUnit = new PackagingUnit()
            {
                Active = true,
                CreaterTransaction = transaction,
                PackageUnitId = packagingUnit.PackageUnitId,
                Part = packagingUnit.Part,
                Quantity = packagingUnit.Quantity,
                Status = PackagingUnitStatus.OnKanban,
            };
            dbc.Transactions.Add(transaction);
            dbc.PackagingUnits.Add(newPackagingUnit);
            dbc.SaveChanges();
        }

        /// <summary>
        /// Kitárolás kanban állványról a termelésbe
        /// </summary>
        /// <param name="dbc">adatbázis context</param>
        /// <param name="storedPackagingUnit">kanban állványra betárolt csomagolási egység azonosítója</param>
        public static void KanbanStoreOut(this ISTRMContext dbc, PackagingUnit packagingUnit)
        {
            var transaction = new Transaction()
            {
                Timestamp = DateTime.Now,
                Type = TransactionType.PUKanbanOut,
            };
            packagingUnit.Active = false;
            packagingUnit.CloserTransaction = transaction;
            var newPackagingUnit = new PackagingUnit()
            {
                Active = true,
                CreaterTransaction = transaction,
                PackageUnitId = packagingUnit.PackageUnitId,
                Part = packagingUnit.Part,
                Quantity = packagingUnit.Quantity,
                Status = PackagingUnitStatus.InProduction,
            };
            dbc.Transactions.Add(transaction);
            dbc.PackagingUnits.Add(newPackagingUnit);
            dbc.SaveChanges();
        }

        /// <summary>
        /// Kiír egy monitor rekordot az adatbázisba
        /// </summary>
        /// <param name="dbc">adatbázis context</param>
        /// <param name="type">A Monitor rekord típusa</param>
        /// <param name="instanceName">a monitor rekord példány azonosítója</param>
        /// <param name="xmlContent">a monitor rekord adattartalma</param>
        public static void AddMonitorData(this ISTRMContext dbc, WorkstationType type, string instanceName, XElement xmlContent)
        {
            var monitorRecord = dbc.MonitorDatas.FirstOrDefault(x => x.Type == type && x.Instance == instanceName);
            if (monitorRecord == null)
            {
                monitorRecord = new MonitorData()
                {
                    Instance = instanceName,
                    Type = WorkstationType.Receiving,
                };
                dbc.MonitorDatas.Add(monitorRecord);
            }
            else
            {
                monitorRecord.ContentXml = xmlContent;
                monitorRecord.Timestamp = DateTime.Now;
            }
            dbc.SaveChanges();
        }
    }
}
