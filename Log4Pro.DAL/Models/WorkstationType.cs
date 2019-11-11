using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM.DAL
{
    /// <summary>
    /// Munkahely típus
    /// </summary>
    public enum WorkstationType 
    { 
        /// <summary>
        /// Betároló
        /// </summary>
        Receiving, 
        /// <summary>
        /// Átcsomagoló
        /// </summary>
        Repacking, 
        /// <summary>
        /// Kitárroló
        /// </summary>
        Putout, 
        /// <summary>
        /// Kanban állvány
        /// </summary>
        Kanban,
    }
}
