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
    /// Cikkszám
    /// </summary>
    public class Part
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Belső cikkszám
        /// </summary>
        [MaxLength(50)]
        [Index(IsClustered = false, IsUnique = true)]
        [Required]
        public string PartNumber { get; set; }

        /// <summary>
        /// Beszálítói cikkszám
        /// </summary>        
        [MaxLength(50)]
        [Index(IsClustered = false, IsUnique = true)] 
        [Required]
        public string ExternalPartNumber { get; set; }

        /// <summary>
        /// Besszálíztó csomagolási egységének a mennyisége
        /// </summary>
        public int SupplierShippingUnitQty { get; set; }

        /// <summary>
        /// FK Beszállító
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// NP Beszállító
        /// </summary>
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// Megnevezés
        /// </summary>
        public string Description { get; set; }
    }
}

