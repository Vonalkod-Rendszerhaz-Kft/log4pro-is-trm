using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.DAL.Models.Transactions
{
    [Table("Packagings")]
    public class Packaging : Transaction
    {
        public virtual ShippingUnit ShippingUnit { get; set; }
        public virtual PackagingUnit PackagingUnit { get; set; }
        public int Quantity { get; set; }
    }
}
