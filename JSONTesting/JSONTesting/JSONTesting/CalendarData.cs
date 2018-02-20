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
        public CalendarEvent(int e_id, int e_day, string e_name, string e_desc, string e_start, string e_end)
        {
            EVENT_ID = e_id;
            EVENT_DAY = e_day;
            EVENT_NAME = e_name;
            EVENT_DESC = e_desc;
            EVENT_START = e_start;
            EVENT_END = e_end;
        }
    }

    public class Calendar
    {
        public string TYPE { get; set; } //The type of action. C_GET, C_SET, C_SEND
        public int CALENDAR_KEY { get; set; } //The id for calendar.
        public string AUTH { get; set; } //Some sort authentication token.
        public List<CalendarEvent> CalendarEvents { get; set; } //Array of CalendarEvent objects.
        public Calendar(string c_type, int c_key, string c_auth, List<CalendarEvent> c_events)
        {
            TYPE = c_type;
            CALENDAR_KEY = c_key;
            AUTH = c_auth;
            CalendarEvents = c_events;
        }
    }
}
