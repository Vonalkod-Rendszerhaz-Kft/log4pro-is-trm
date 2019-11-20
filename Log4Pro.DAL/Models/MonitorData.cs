using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VRH.Common;

namespace Log4Pro.IS.TRM.DAL
{    
    /// <summary>
    /// Monitor adatok
    /// </summary>
    public class MonitorData
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Munkahely típusa
        /// </summary>
        [MaxLength(50)]
        [Index(IsClustered = false, IsUnique = false)]
        public string WorkstationType
        {
            get => Type.ToString(); 
            set => Type = value.ToEnum<WorkstationType>();
        }

        /// <summary>
        /// Munkahely típusa
        /// </summary>
        [NotMapped]
        public WorkstationType Type { get; set; }

        /// <summary>
        /// Példány
        /// </summary>
        [MaxLength(50)]
        [Index(IsClustered = false, IsUnique = false)]
        public string Instance { get; set; }

        /// <summary>
        /// Időbélyeg (utolsó változás)
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Monitor adattartalom
        /// </summary>
        [Column(TypeName = "xml")]
        public string Content { get; set; }

        /// <summary>
        /// Monitor adattartalom
        /// </summary>
        [NotMapped]
        public XElement ContentXml { get => XElement.Parse(Content); set => Content=value.ToString(); }
    }
}
