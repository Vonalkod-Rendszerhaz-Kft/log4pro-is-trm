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
        /// Kitárolás modul egyezménye
        /// </summary>
        public class PutOutModule
        {
            /// <summary>
            /// EventHub szolgáltatáscsatorna modul prefixe
            /// </summary>
            public const string MODULE_PREFIX = nameof(PutOutModule);

            /// <summary>
            /// Kitárolás kérés
            /// </summary>
            public class PutOutRequest
            {
                public string PackagingUnitId { get; set; }
            }
        }
    }
}
