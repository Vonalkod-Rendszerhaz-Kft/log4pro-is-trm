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
    /// Szállítási egységek
    /// </summary>
    [Table("ShippingUnits")]
    public class ShippingUnit : StoreUnit
    {
        /// <summary>
        /// Szállítási egység azonosító
        /// </summary>
        [MaxLength(50)]
        [Index(IsClustered = false, IsUnique = false)]
        public string ShippingUnitId { get; set; }

        /// <summary>
        /// Külső szállítói egység azonosító
        /// </summary>
        [MaxLength(50)]
        [Index(IsClustered = false, IsUnique = false)]
        public string ExternalShippingUnitId { get; set; }

        /// <summary>
        /// Beszálítói egység státusza (Ne használd kód oldalról!!!)
        /// </summary>
        [MaxLength(30)]
        [Index(IsUnique = false, IsClustered = false)]
        public string ShippingUnitStatus
        {
            get => Status.ToString();
            set => Status = value.ToEnum<ShippingUnitStatus>();
        }

        /// <summary>
        /// Beszálítói egység státusza
        /// </summary>
        [NotMapped] 
        public ShippingUnitStatus Status { get; set;}

        /// <summary>
        /// Tárolóhely, ahol a beszálítói egység található
        /// </summary>
        [MaxLength(30)]
        [Index(IsUnique = false, IsClustered = false)]
        public string StoreLocation { get; set; }
    }
}
