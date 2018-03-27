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
            return null;
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

            // return eventID
            return null;
        }

        public async Task<bool> UpdateEvent(string eventID, string eventName, DateTime startTime, DateTime endTime)
        {
            return true;
        }

        public async Task<bool> DeleteEvent(string eventID)
        {
            return true;
        }

        public async Task<Event> GetEventByID(string eventID)
        {
            Event e = new Event();
        
            return e;
        }

        //controller verify that userID = ownerID
        // check if user is subscribe to that calendar
    }
}
