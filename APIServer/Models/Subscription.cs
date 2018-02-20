using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Models
{
    public class Subscription
    {
        [Key, Column(Order = 0)]
        public string UserID { get; set; }
        [Key, Column(Order = 1)]
        public Guid CalendarID { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Calendar Calendar { get; set; }
    }
}
