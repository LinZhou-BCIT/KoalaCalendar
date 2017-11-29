using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoalaCalendar.Models.ViewModels
{
    public class EventVM
    {
        public string EventID { get; set; }
        [DisplayName("Title")]
        public string EventTitle { get; set; }
        [DisplayName("Start Time")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }
        [DisplayName("End Time")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
        [DisplayName("Calendar")]
        public string CalendarName { get; set; }
    }
}
