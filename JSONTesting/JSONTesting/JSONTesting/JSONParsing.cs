using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace JSONTesting
{
    class JSONParsing
    {
        internal class CalendarEvent
        {
            internal int date;

            internal string name;

            internal string description;

            internal string eventStart;

            internal string eventEnd;
        }

        internal class CalendarEventList
        {
            internal CalendarEvent calenderEvents;
        }

        [DataContract]
        internal class CalendarData
        {
            [DataMember]
            internal string CallType;

            [DataMember]
            internal int CalKey;

            [DataMember]
            internal string Auth;

            [DataMember]
            internal CalendarEventList EventList;
        }

        public void Test()
        {
            CalendarData c = new CalendarData();
            c.CallType = "C_GET";
            c.CalKey = 1;
            c.Auth = "aaaa";
            //c.Events = "";
        }

        public IDictionary<string, string> Parse(string jsonString)
        {
            IDictionary<string, string> JsonMap = new Dictionary<string, string>();




            return JsonMap;
        }
    }
}
