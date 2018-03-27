using APIServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public interface ICalendarRepo
    {
        Task<string> CreateCalendar(string calendarName, string ownerID);
        Task<IEnumerable<Calendar>> GetAllCalendarsForUser(string userID);
        Task<IEnumerable<Calendar>> SearchCalendar(string searchInput);
        Task<bool> UpdateCalendar(string calendarID, string calendarName);
        Task<string> GenerateAccessCode(Guid calendarID);
        Task<bool> RemoveCalendar(Guid calendarID);
        Task<bool> UnsubUserFromCalendar(string userID, Guid calendarID);
    }
}
