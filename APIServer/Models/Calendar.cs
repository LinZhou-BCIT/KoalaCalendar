using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Models
{
    public class Calendar
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid CalendarID { get; set; }
        public string Name { get; set; }
        public string AccessCode { get; set; }
        public string OwnerID { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
