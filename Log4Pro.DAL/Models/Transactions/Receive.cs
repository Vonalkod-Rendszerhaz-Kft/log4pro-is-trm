using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.DAL.Models.Transactions
{
    /// <summary>
    /// Shipping Unit
    /// </summary>
    [Table("Receives")]
    public class Receive : Transaction
    {
        public virtual Location Location { get; set; }
    }
}
