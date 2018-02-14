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
            string testString = @"{ 'test' : 1 }";
            Calendar test = JsonConvert.DeserializeObject<Calendar>(testString);
            
            Console.WriteLine(test.type);

            

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
