using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.DAL.Models
{
    public class Part
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public string ExternalProductNumber { get; set; }
        public virtual Supplier Supplier { get; set; }
        public string Description { get; set; }
    }
}

