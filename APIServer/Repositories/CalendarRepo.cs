using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class CalendarRepo
    {
        public async Task<bool> CreateCalendar(string calendarName, string accessCode)
        {
            return true;
        }

        public async Task<IEnumerable<string>> GetAllCalendars()
        {
            return null;
        }

        public async Task<IEnumerable<string>> SearchCalendar(string searchInput)
        {
            return null;
        }

    }
}
