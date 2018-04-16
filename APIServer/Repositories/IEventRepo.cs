using APIServer.Models;
using APIServer.Models.CalendarViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public interface IEventRepo
    {
        Task<IEnumerable<EventVM>> GetEvents(Guid calendarID, DateTime startTime, DateTime endTime);
        Task<IEnumerable<Event>> GetEvents(Guid CalendarID);
        Task<string> CreateEvent(Guid calendarID, string eventName, DateTime startTime, DateTime endTime);
        Task<bool> UpdateEvent(Guid eventID, string eventName, DateTime startTime, DateTime endTime);
        Task<bool> DeleteEvent(Guid eventID);
        Task<Event> GetEventByID(Guid eventID);
        EventVM ConvertToVM(Event ev);
    }
}
