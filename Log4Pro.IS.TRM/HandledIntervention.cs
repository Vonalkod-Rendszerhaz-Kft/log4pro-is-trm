using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM
{
    /// <summary>
    /// A szolgáltatás által kezelt beavatkozások
    /// </summary>
    public enum HandledInterventions
    {
        /// <summary>
        /// Bevételezés
        /// </summary>
        TakeIn,
        /// <summary>
        /// Betárolás
        /// </summary>
        Receive,
        /// <summary>
        /// Átcsomagolás
        /// </summary>
        Repack,
        /// <summary>
        /// Kitárolás
        /// </summary>
        PutOut,
        /// <summary>
        /// Érkeztetés kanban állványra
        /// </summary>
        KanbanIn,
        /// <summary>
        /// Kitárolás kanban állványról
        /// </summary>
        KanbanOut,
    }
}
