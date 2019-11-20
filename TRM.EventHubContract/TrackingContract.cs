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
        /// EventHub szolgáltatás csatorna prefixe
        /// </summary>
        public const string CHANNEL_PREFIX = nameof(TrackingContract);

        /// <summary>
        /// Általános válasz (~ void visszatérésnek felel meg)
        /// </summary>
        public class Response
        {
        }
    }
}
