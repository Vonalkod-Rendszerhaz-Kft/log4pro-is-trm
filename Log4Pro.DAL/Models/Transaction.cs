using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRH.Common;

namespace Log4Pro.IS.TRM.DAL
{
    /// <summary>
    /// Tranzakció (Művelet)
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Azonosító
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Időbélyeg
        /// </summary>
        [Index(IsClustered = false, IsUnique = false)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Tranzakció típusa (Ne használd kódoldalon!!!)
        /// </summary>
        [MaxLength(50)]
        [Index(IsClustered = false, IsUnique = false)]
        public string TransactionType 
        {
            get
            {
                return Type.ToString();
            }
            set
            {
                Type = value.ToEnum<TransactionType>();
            } 
        }
                
        /// <summary>
        /// Tranzakció típusa
        /// </summary>
        [NotMapped]
        public TransactionType Type { get; set; }
    }
}
