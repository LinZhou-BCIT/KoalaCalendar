using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Models.CalendarViewModels
{
    public class CalendarVM
    {
        public Guid CalendarID { get; set; }
        public string Name { get; set; }
        public string AccessCode { get; set; }
        public string OwnerID { get; set; }
        public string OwnerEmail { get; set; }
    }
}
