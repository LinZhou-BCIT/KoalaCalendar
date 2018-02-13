using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class CalendarRepo
    {
        public async Task<string> CreateCalendar(string calendarName)
        {
            //return calendarID once it is created

            return null;
        }

        public async Task<IEnumerable<string>> GetAllCalendars()
        {
            return null;
        }

        public async Task<IEnumerable<string>> SearchCalendar(string searchInput)
        {
            return null;
        }

        public async Task<bool> UpdateCalendar(string calendarID, string calendarName)
        {
            return true;
        }

        public async Task<string> GenerateAccessCode(string calendarID)
        {
            // calendarID will be used to generate the accessCode
            //return access code
            return null;
        }

        public async Task<bool> RemoveCalendar(string calendarID)
        {
            return true;
        }

    }
}
