using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log4Pro.IS.TRM.EventHubContract;
using Vrh.EventHub.Core;
using Vrh.EventHub.Protocols.RedisPubSub;
using Log4Pro.IS.TRM.DAL;
using System.Xml.Linq;

namespace Log4Pro.IS.TRM.KanbanModule
{
    internal class KanbanService : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public KanbanService()
        {
            EventHubCore.RegisterHandler<RedisPubSubChannel,
                TrackingContract.KanbanModule.SuccessStoreIn>(EventHubChannelName, StoreIn);
            EventHubCore.RegisterHandler<RedisPubSubChannel,
                TrackingContract.KanbanModule.SuccessStoreOut>(EventHubChannelName, StoreOut);
            EventHubCore.RegisterHandler<RedisPubSubChannel,
                TrackingContract.KanbanModule.ChangeEvent>(EventHubChannelName, KanbanStoreChange);
        }

        /// <summary>
        /// Betárolás kanbanálványra
        /// </summary>
        /// <param name="request"></param>
        private void StoreIn(TrackingContract.KanbanModule.SuccessStoreIn request)
        {
            try
            {
                using (var dbc = new ISTRMContext())
                {
                    var packagingUnit = dbc.PackagingUnits.FirstOrDefault(x => x.Active
                                                                   && x.PackageUnitId == request.PackageUnitId
                                                                   && x.Status == PackagingUnitStatus.PutOut);
                    if (packagingUnit == null)
                    {
                        throw new Exception($"Unknown packaging unit id: {request.PackageUnitId}");
                    }
                    else
                    {
                        dbc.KanbanStoreIn(packagingUnit);
                        var xml = GetMonitorData(packagingUnit, $"Kanban stroring.");
                        dbc.AddMonitorData(WorkstationType.Kanban, InstanceName, xml);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var xml = MonitorDataContentHelper.GetMonitorDataForErrors(ex);
                    using (var dbc = new ISTRMContext())
                    {
                        dbc.AddMonitorData(WorkstationType.Kanban, InstanceName, xml);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Kitárolás kanbanállványról
        /// </summary>
        /// <param name="request"></param>
        private void StoreOut(TrackingContract.KanbanModule.SuccessStoreOut request)
        {
            try
            {
                using (var dbc = new ISTRMContext())
                {
                    var packagingUnit = dbc.PackagingUnits.FirstOrDefault(x => x.Active
                                                                   && x.PackageUnitId == request.PackageUnitId
                                                                   && x.Status == PackagingUnitStatus.OnKanban);
                    if (packagingUnit == null)
                    {
                        throw new Exception($"Unknown packaging unit id: {request.PackageUnitId}");
                    }
                    else
                    {
                        dbc.KanbanStoreOut(packagingUnit);
                        var xml = GetMonitorData(packagingUnit, $"Kanban removing.");
                        dbc.AddMonitorData(WorkstationType.Kanban, InstanceName, xml);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var xml = MonitorDataContentHelper.GetMonitorDataForErrors(ex);
                    using (var dbc = new ISTRMContext())
                    {
                        dbc.AddMonitorData(WorkstationType.Kanban, InstanceName, xml);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Kanban tárolóterület megváltozása esemény
        /// </summary>
        /// <param name="request"></param>
        private void KanbanStoreChange(TrackingContract.KanbanModule.ChangeEvent request)
        {
            try
            {
                using (var dbc = new ISTRMContext())
                {
                    foreach (var location in request.LocationMap)
                    {
                        var packagingUnit = dbc.PackagingUnits.FirstOrDefault(x => x.Active 
                                                                                    && x.PackageUnitId == location.LoadedPackageUnitId 
                                                                                    && x.Status == PackagingUnitStatus.OnKanban);
                        var xml = GetMonitorMapData(packagingUnit, location);
                        dbc.AddMonitorData(WorkstationType.KanbanMap, $"{InstanceName}:{location.Location}", xml);
                    }                    
                }
                
            }
            catch (Exception ex)
            {
                try
                {
                    var xml = MonitorDataContentHelper.GetMonitorDataForErrors(ex);
                    using (var dbc = new ISTRMContext())
                    {
                        dbc.AddMonitorData(WorkstationType.Kanban, InstanceName, xml);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Munkahely instance neve TODO: itt iplmentáld, ha később az konfigurálható kell legyen, vagy valami logika lapján kell képződjön!!!
        /// </summary>
        private string InstanceName { get => "demo"; }

        /// <summary>
        /// Eventhub csatorna neve
        /// </summary>
        private string EventHubChannelName { get => $"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.KanbanModule.MODULE_PREFIX}:{InstanceName}"; }

        private XElement GetMonitorMapData(PackagingUnit packagingUnit, TrackingContract.KanbanModule.LocationMap locationMap)
        {
            var monitorDataList = new Dictionary<string, string>()
            {
                { "LocationStatus", locationMap.Status.ToString() },
                { "ExpectedPackagingUnit", locationMap.ExpectedPackagingUnit },
                { "LoadedPackagingUnit", locationMap.LoadedPackageUnitId },
                { "LoadError", locationMap.IsError.ToString() },
                { "PartNumber", packagingUnit != null ? packagingUnit.Part.PartNumber : "" },
                { "PartDescription", packagingUnit != null ? packagingUnit.Part.Description : "" },
                { "Qty", packagingUnit != null ? packagingUnit.Quantity.ToString() : "" },
            };
            return MonitorDataContentHelper.CreateContent(monitorDataList);
        }

        /// <summary>
        /// Visszadja a Monitorrekordba irandó adatokat
        /// </summary>
        /// <param name="packagingUnit"></param>
        /// <returns></returns>
        private XElement GetMonitorData(PackagingUnit packagingUnit, string message)
        {
            var monitorDataList = new Dictionary<string, string>()
            {
                { "Message", message },
                { "PackagingUnitId", packagingUnit.PackageUnitId },
                { "PartNumber", packagingUnit.Part.PartNumber },
                { "PartDescription", packagingUnit.Part.Description },
                { "Qty", packagingUnit.Quantity.ToString() },                
            };
            return MonitorDataContentHelper.CreateContent(monitorDataList);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    EventHubCore.DropHandler<RedisPubSubChannel,
                        TrackingContract.KanbanModule.SuccessStoreIn>(EventHubChannelName, StoreIn);
                    EventHubCore.DropHandler<RedisPubSubChannel,
                        TrackingContract.KanbanModule.SuccessStoreOut>(EventHubChannelName, StoreOut);
                    EventHubCore.RegisterHandler<RedisPubSubChannel,
                        TrackingContract.KanbanModule.ChangeEvent>(EventHubChannelName, KanbanStoreChange);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~KanbanService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
