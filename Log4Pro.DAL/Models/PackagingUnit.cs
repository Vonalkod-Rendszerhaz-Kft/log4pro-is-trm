using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.DAL.Models
{
    public class PackagingUnit
    {
        public int Id { get; set; }
        public string PackageUnitId { get; set; }
        public virtual Part Part { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; }
    }
}
