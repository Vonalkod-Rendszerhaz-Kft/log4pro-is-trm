using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.DAL.Models.Transactions
{
    [Table("KanbanLoadings")]
    public class KanbanLoading : Transaction
    {
        public virtual KanbanLocation Location { get; set; }
    }
}
