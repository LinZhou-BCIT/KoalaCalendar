using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;

namespace CalendarJson
{
    public class JSONParsing
    {
        public void Test()
        {
            string testString = 
@"{
	'type':'C_GET',
	'CK':'1','Auth':
	'<something to do with auth>',
	'Events': 
		[
			{
				'Event_ID':'1',
				'EVENT_DAY':'20',
				'EVENT_NAME':'test',
				'EVENT_DESC':'generic',
				'EVENT_START':'datetime',
				'EVENT_END':'datetime'
			}
		]
}";
            Calendar test = JsonConvert.DeserializeObject<Calendar>(testString);
            
            Console.WriteLine(test.CalendarEvents);

            

        }
    }

    public class CalendarEvent
    {
        public int eventID          { get; set; }
        public int eventDay         { get; set; }
        public string eventName     { get; set; }
        public string eventDesc     { get; set; }
        public string eventStart    { get; set; }
        public string eventEnd      { get; set; }
    }

    public class Calendar
    {
        public string type { get; set; }
        public string calendarKey { get; set; }
        public string Auth { get; set; }
        public CalendarEvent[] CalendarEvents { get; set; }
    }
}
