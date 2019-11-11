using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM.DAL
{
    /// <summary>
    /// Lehetséges műveletek
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// SU Bevételezve
        /// </summary>
        SUTakeIn,
        /// <summary>
        /// SU Betárolva
        /// </summary>
        SUReceive,
        /// <summary>
        /// PU létrehozva
        /// </summary>
        PUCreate,
        /// <summary>
        /// PU kitárolva
        /// </summary>
        PUPutOut,
        /// <summary>
        /// PU kanbanállványon
        /// </summary>
        PUKanbanIn,
        /// <summary>
        /// PU termelésben
        /// </summary>
        PUKanbanOut,
    }
}
