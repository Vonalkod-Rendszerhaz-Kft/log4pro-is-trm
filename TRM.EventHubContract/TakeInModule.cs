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
        /// Bevételezés modul
        /// </summary>
        public class TakeInModule
        {
            /// <summary>
            /// EventHub szolgáltatáscsatorna modul prefixe
            /// </summary>
            public const string MODULE_PREFIX = nameof(TakeInModule);

            /// <summary>
            /// Bevételezés lekérdezés (azonosítás beszállítói cikkszám alapján)
            /// </summary>
            public class TakeInQueryRequest
            {
                public string SupplierShippingUnitId { get; set; }
            }

            /// <summary>
            /// Bevételezés lekérdezésre adott válasz
            /// </summary>
            public class TakeInQueryResponse : Response
            {
                /// <summary>
                /// A bevételezett cikkhez tartozó adatok
                /// </summary>
                public Dictionary<string, string> Datas { get; set; }
            }

            /// <summary>
            /// Bevételezés
            /// </summary>
            public class TakeInRequest
            {
                /// <summary>
                /// Belső szállítási egység azonosító
                /// </summary>
                public string InternalShippingUnitId { get; set; }

                /// <summary>
                /// Beszállítói szállítási egység azonosító
                /// </summary>
                public string ExternalShippingUnitId { get; set; }

                /// <summary>
                /// Cikkszám
                /// </summary>
                public string PartNumber { get; set; }

                /// <summary>
                /// Mennyiség
                /// </summary>
                public int Qty { get; set; }
            }
        }
    }
}
