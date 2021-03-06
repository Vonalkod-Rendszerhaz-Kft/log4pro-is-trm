using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log4Pro.IS.TRM.DAL;
using VRH.Common;

namespace Log4Pro.IS.TRM.TakeInModule
{
    /// <summary>
    /// Egy azonosítóval azonosított homogén beszállítói egységcsomag
    /// </summary>
    internal class SupplierShippingUnit
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="supplierShippingUnitId">beszállítói csomag azonosító</param>
        public SupplierShippingUnit(string supplierShippingUnitId)
        {
            SupplierShippingUnitId = supplierShippingUnitId;
            if (supplierShippingUnitId.Length < 8)
            {
                SupplierPartNumber = supplierShippingUnitId;
            }
            else
            {
                SupplierPartNumber = supplierShippingUnitId.Substring(0, 8);
            }
            if (supplierShippingUnitId.Length > 12)
            {
                if (int.TryParse(supplierShippingUnitId.Substring(supplierShippingUnitId.Length - 5), out int qty))
                {
                    SupplierQty = qty;
                }
            }
            GetMyDataFromDb();
			FVS = "565243303030" + GetFVS(); 
			MTSId = GetRandomString(11);
            Slot = GetRandomString(8);
        }       

        /// <summary>
        /// Belső cikkszám
        /// </summary>
        public string PartNumber { get; private set; }

        /// <summary>
        /// Cikk megnevezése 
        /// </summary>
        public string PartDescription { get; private set; }
        
        /// <summary>
        /// Beszállító megnevezése
        /// </summary>
        public string SupplierName { get; private set; }

        /// <summary>
        /// Mennyiség
        /// </summary>
        public int Qty
        {
            get => SupplierQty > 0 ? SupplierQty : DefaultSupplierQty;
        }

        /// <summary>
        /// Beszállítói egység egyedi azonosítója
        /// </summary>
        public string SupplierShippingUnitId { get; private set; }

        /// <summary>
        /// Beszállítói cikkszám
        /// </summary>
        public string SupplierPartNumber { get; private set; }

        /// <summary>
        /// beszállítói csomagegység mennyisége
        /// </summary>
        public int SupplierQty { get; private set; } = 0;

        /// <summary>
        /// Alapértelmetzett beszállítói mennyiség
        /// </summary>
        public int DefaultSupplierQty { get; private set; }

        /// <summary>
        /// belső FVS azonosító
        /// </summary>
        public string FVS { get; private set; }

        /// <summary>
        /// MTS azonosító
        /// </summary>
        public string MTSId { get; private set; }

        /// <summary>
        /// Tároló azomositó
        /// </summary>
        public string Slot { get; private set; }

        /// <summary>
        /// Bélyeg
        /// </summary>
        public readonly DateTime TimeStamp = DateTime.Now;

        /// <summary>
        /// Visszaad a specifikált hoszban egy random stringet
        /// </summary>
        /// <param name="length">random string hossza</param>
        /// <returns>random string</returns>
        private string GetRandomString(int? length = null)
        {
            if (length == null)
            {
                return Guid.NewGuid().ToString().Replace("-", "");
            }
            else
            {
                string randomString = Guid.NewGuid().ToString().Replace("-", "");
                return randomString.Substring(0, length.HasValue ? length.Value : randomString.Length);
            }
        }

		private string GetFVS()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < 6; i++)
			{
				sb.Append(GetRandomNumberCode());
			}
			return sb.ToString();
		}

		private string GetRandomNumberCode()
		{
			var i = _rnd.Next(0, 9);
			return ((int)i.ToString()[0]).ToString("X2");
		}

		/// <summary>
		/// BEállítja a cikk adatait az adatbázisból
		/// </summary>
		private void GetMyDataFromDb()
        {
            using (var dbc = new ISTRMContext())
            {
                var part = dbc.Parts.FirstOrDefault(X => X.ExternalPartNumber == SupplierPartNumber);
                if (part != null)
                {
                    PartNumber = part.PartNumber;
                    PartDescription = part.Description;
                    SupplierName = part.Supplier.Description;
                    DefaultSupplierQty = part.SupplierShippingUnitQty;
                }
                else
                {
                    throw new Exception($"Unknown External PartNumber: {SupplierPartNumber}");
                }
            }
        }

		private Random _rnd = new Random();
    }
}
