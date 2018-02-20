using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using CalendarData;

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
            
            Console.WriteLine(test.CalendarEvents.Length);
        }
    }
}
