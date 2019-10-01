using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Log4Pro.DAL.Models
{
    public enum Type { Receiving, Repacking, Putout, Kanban}
    public class MonitorData
    {
        public int Id { get; set; }
        public Type Instance { get; set; }
        public DateTime Timestamp { get; set; }

        [Column(TypeName = "xml")]
        public string Content { get; set; }

        [NotMapped]
        public XElement ContentXml { get => XElement.Parse(Content); set => Content=value.ToString(); }
    }
}
