using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarData
{
    public class CalendarEvent
    {
        public int EVENT_ID { get; set; }
        public int EVENT_DAY { get; set; }
        public string EVENT_NAME { get; set; }
        public string EVENT_DESC { get; set; }
        public string EVENT_START { get; set; }
        public string EVENT_END { get; set; }
    }

    public class Calendar
    {
        public string TYPE { get; set; }
        public string CALENDAR_KEY { get; set; }
        public string AUTH { get; set; }
        public CalendarEvent[] CalendarEvents { get; set; }
    }
}
