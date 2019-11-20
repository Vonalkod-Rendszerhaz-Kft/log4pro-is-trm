using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM
{
    /// <summary>
    /// Lehetséges beavatkozások
    /// </summary>
    public enum PosibleRequestIds
    {
        /// <summary>
        /// Ismeretlen (csak a TryParse out miatt, hogy ne adjon létezőt...)
        /// </summary>
        Unknown,
        /// <summary>
        /// Bevételezés lekérdezés
        /// </summary>
        TakeInQuery,
        /// <summary>
        /// Bevéteélezés
        /// </summary>
        TakleIn,
        /// <summary>
        /// Betárolás
        /// </summary>
        Receiving,
    }
}
