using APIServer.Models;
using APIServer.Models.CalendarViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public interface ICalendarRepo
    {
        Task<string> CreateCalendar(string calendarName, string ownerID);
        Task<Calendar> GetCalendarByID(Guid calendarID);
        Task<IEnumerable<CalendarVM>> GetOwnedCalendars(string userID);
        Task<IEnumerable<CalendarVM>> GetSubbedCalendars(string userID);
        Task<IEnumerable<Calendar>> SearchCalendar(string userID, string searchInput);
        Task<bool> UpdateCalendar(Guid calendarID, string calendarName);
        Task<string> GenerateAccessCode(Guid calendarID);
        Task<bool> RemoveCalendar(Guid calendarID);
        Task<bool> SubToCalendar(string userID, string accessCode);
        Task<bool> UnsubUserFromCalendar(string userID, Guid calendarID);
        CalendarVM ConvertToVM(Calendar calendar);
        Task<bool> VerifyCalendarOwnner(string userID, Guid calendarID);
    }
}
