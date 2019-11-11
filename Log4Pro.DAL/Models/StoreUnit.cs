using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM.DAL
{
    /// <summary>
    /// Tárolási egységek közös őse
    /// </summary>
    public class StoreUnit
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// KF: Cikk
        /// </summary>
        public int PartId { get; set; }

        /// <summary>
        /// NP:  Cikk
        /// </summary>
        public virtual Part Part { get; set; }

        /// <summary>
        /// Mennyiség
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Aktív készlet rekord (true)
        /// </summary>
        [Index(IsUnique = false, IsClustered = false)]
        public bool Active { get; set; }

        /// <summary>
        /// FK: A rekordot létrehozó tranzakció azonosítója
        /// </summary>
        public int CreaterTransactionId { get; set; }

        /// <summary>
        /// NP: A rekordot létrehozó tranzakció
        /// </summary>
        public virtual Transaction CreaterTransaction { get; set; }

        /// <summary>
        /// FK: A rekordot lezáró tranzakció azonosítója
        /// </summary>
        public int? CloserTransactionId { get; set; }

        /// <summary>
        /// NP: A rekordot lezáró tranzakció azonosítója
        /// </summary>
        public Transaction CloserTranzaction { get; set; }
    }
}
