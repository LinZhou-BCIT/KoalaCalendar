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
	'TYPE':'C_GET',
	'CALENDAR_KEY':'1','AUTH':
	'<something to do with auth>',
	'CalendarEvents': 
		[
			{
				'EVENT_ID':'1',
				'EVENT_DAY':'20',
				'EVENT_NAME':'test',
				'EVENT_DESC':'generic',
				'EVENT_START':'datetime',
				'EVENT_END':'datetime'
			},
            {
				'EVENT_ID':'2',
				'EVENT_DAY':'21',
				'EVENT_NAME':'test1',
				'EVENT_DESC':'generic1',
				'EVENT_START':'datetime1',
				'EVENT_END':'datetime1'
			}
		]
}";
            Calendar test = JsonConvert.DeserializeObject<Calendar>(testString);
            
            Console.WriteLine(test.CalendarEvents[0].EVENT_ID);

            

        }
    }

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
