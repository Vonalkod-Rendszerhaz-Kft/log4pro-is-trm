using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM.EventHubContract
{
    /// <summary>
    /// A TRM modul EventHub Contractja
    /// </summary>
    public partial class TrackingContract
    {
        /// <summary>
        /// Átcsomagoló modul
        /// </summary>
        public class RepackingModule
        {
            /// <summary>
            /// EventHub szolgáltatáscsatorna modul prefixe
            /// </summary>
            public const string MODULE_PREFIX = nameof(RepackingModule);

            /// <summary>
            /// Átcsomagolásra elérhető mennyiség lekérdezése kérés
            /// </summary>
            public class AvailableQtyRequest
            {
                /// <summary>
                /// Beszállítói egység azonosítója
                /// </summary>
                public string ShippingUnitId { get; set; }
            }

            /// <summary>
            /// Átcsomagolásra elérhető mennyiség lekérdezése válasz
            /// </summary>
            public class AvailableQtyResponse
            {
                /// <summary>
                /// Beszállítói egységben elérhető mennyiség
                /// </summary>
                public int AvailableQty { get; set; }
            }

            /// <summary>
            /// Átcsomagolás kérés
            /// </summary>
            public class RepackRequest
            {
                /// <summary>
                /// Beszállítói egység azonosítója
                /// </summary>
                public string ShippingUnitId { get; set; }

                /// <summary>
                /// Tárolóegység azonosítója
                /// </summary>
                public string PackagingUnitId { get; set; }

                /// <summary>
                /// Átrakott mennyiség
                /// </summary>
                public int Qty { get; set; }
            }
        }
    }
}
