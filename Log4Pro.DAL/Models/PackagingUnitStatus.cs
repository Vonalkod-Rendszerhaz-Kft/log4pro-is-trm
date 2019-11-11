using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM.DAL
{
    /// <summary>
    /// Csomagolási egység státusz
    /// </summary>
    public enum PackagingUnitStatus
    {
        /// <summary>
        /// Létrehozva
        /// </summary>
        Created,
        /// <summary>
        /// Kitárolva
        /// </summary>
        PutOut,
        /// <summary>
        /// Kanbanállványon
        /// </summary>
        OnKanban,
        /// <summary>
        /// Termelésben
        /// </summary>
        InProduction,
    }
}
