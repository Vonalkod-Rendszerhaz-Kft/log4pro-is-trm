using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM.DAL
{
    /// <summary>
    /// Beszállító
    /// </summary>
    public class Supplier
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Megnevezés
        /// </summary>
        [MaxLength(100)]
        [Index(IsClustered =false, IsUnique = true)]
        [Required]
        public string Description { get; set; }
    }
}
