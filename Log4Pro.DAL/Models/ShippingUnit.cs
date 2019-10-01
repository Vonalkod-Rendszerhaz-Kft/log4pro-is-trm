using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.DAL.Models
{
    public class ShippingUnit
    {
        public int Id { get; set; }
        public string ShippingUnitId { get; set; }
        public string ExternalShippingUnitId { get; set; }
        public virtual Part Part { get; set; }
        public int Quantity { get; set; }
        public DateTime Create { get; set; }
        public DateTime? Recieved { get; set; }
        public virtual Location Location { get; set; }

    }
}
