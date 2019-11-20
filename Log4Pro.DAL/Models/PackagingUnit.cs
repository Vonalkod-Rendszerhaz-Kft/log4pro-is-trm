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
    /// Csomagolási (belső tárolási egységek)
    /// </summary>
    [Table("PackagingUnits")]
    public class PackagingUnit : StoreUnit
    {
        /// <summary>
        /// Szálítási egység azonosító
        /// </summary>
        [MaxLength(128)]
        [Index(IsUnique = false, IsClustered = false)]
        public string PackageUnitId { get; set; }

        /// <summary>
        /// Csomagolási egység státusza (Ne nhasználd kódoldalon!!!)
        /// </summary>
        [MaxLength(30)]
        [Index(IsUnique = false, IsClustered = false)]
        public string PackagingUnitStatus
        {
            get => Status.ToString();
            set => Status = value.ToEnum<PackagingUnitStatus>();
        }

        /// <summary>
        /// Csomagolói egység státusza
        /// </summary>
        [NotMapped]
        public PackagingUnitStatus Status { get; set; }
    }
}
