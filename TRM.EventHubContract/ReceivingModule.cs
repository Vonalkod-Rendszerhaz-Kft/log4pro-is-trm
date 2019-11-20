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
        /// Betároló modul
        /// </summary>
        public class ReceivingModule
        {
            /// <summary>
            /// EventHub szolgáltatáscsatorna modul prefixe
            /// </summary>
            public const string MODULE_PREFIX = nameof(ReceivingModule);

            /// <summary>
            /// Betárolás kérés
            /// </summary>
            public class ReceiveRequest
            {
                /// <summary>
                /// Szállítási egység azonosító
                /// </summary>
                public string ShippingUnitId { get; set; }
            }

            /// <summary>
            /// Betárolás válasz
            /// </summary>
            public class ReceiveResponse : Response
            {
                /// <summary>
                /// Tárolóhely, ahová betárolásra került
                /// </summary>
                public string StoreLocation { get; set; }
            }
        }
    }
}
