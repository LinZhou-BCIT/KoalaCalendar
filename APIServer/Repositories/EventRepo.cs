using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIServer.Data;
using APIServer.Models;

namespace APIServer.Repositories
{
    public class EventRepo: IEventRepo
    {
        ApplicationDbContext _context;
        public EventRepo(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Event>> GetEvents(Guid calendarID, DateTime startTime, DateTime endTime)
        {
            var result = _context.Events.Where(c => c.CalendarID == calendarID && c.EndTime <= endTime && c.StartTime >= startTime).AsEnumerable<Event>();
            return await Task.FromResult(result);
        }

        public async Task<string> CreateEvent(Guid calendarID, string eventName, DateTime startTime, DateTime endTime)
        {
            Event newEvent = new Event() {
                EventID = Guid.NewGuid(),
                Name = eventName,
                StartTime = startTime,
                EndTime = endTime,
                CalendarID = calendarID
            };

            _context.Events.Add(newEvent);
            _context.SaveChanges();
            // return eventID
            return await Task.FromResult(newEvent.EventID.ToString());
        }

        public async Task<bool> UpdateEvent(Guid eventID, string eventName, DateTime newStartTime, DateTime newEndTime)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteEvent(Guid eventID)
        {
            return await Task.FromResult(true);
        }

        public async Task<Event> GetEventByID(Guid eventID)
        {
            Event e = new Event();
        
            return await Task.FromResult(e);
        }

        //controller verify that userID = ownerID
        // check if user is subscribe to that calendar
    }
}
