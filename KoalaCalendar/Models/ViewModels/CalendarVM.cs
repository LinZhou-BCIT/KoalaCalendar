using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KoalaCalendar.Models.ViewModels
{
    public class CalendarVM
    {
        [DisplayName("Calendar Name")]
        public string Title { get; set; }
        [DisplayName("Key")]
        public string Key { get; set; }
    }
}
