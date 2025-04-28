using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDashboard.Shared.Models
{
    public class OneOffChore
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }   // make this nullable
        public bool Completed { get; set; }
    }
}
