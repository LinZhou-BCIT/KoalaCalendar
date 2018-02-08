using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace JSONTesting
{
    class JSONParsing
    {

        private string testString = 
            "{'type':'C_GET','CK':'1','Auth':'<something to do with auth>'," +
            "'Event_ID':{'EVENT_DAY':'20','EVENT_NAME':'test','EVENT_DESC':'generic','EVENT_START':'datetime','EVENT_END':'datetime'}}";


        public IDictionary<string, string> Parse(string jsonString)
        {
            IDictionary<string, string> JsonMap = new Dictionary<string, string>();
            Json.Decode


            return JsonMap;
        }
    }
}
