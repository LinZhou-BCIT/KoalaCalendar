using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIServer.Data;
using APIServer.Models.CalendarViewModels;

namespace APIServer.Repositories
{
    public class EventRepo
    {
        ApplicationDbContext _context;
        public EventRepo(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<EventVM>> GetEvents(string calendarID, DateTime startTime, DateTime endTime)
        {
            return null;
        }

        public async Task<string> CreateEvent(string calendarID, string eventName, DateTime startTime, DateTime endTime)
        {
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

        public async Task<EventVM> GetEventByID(string eventID)
        {
            EventVM e = new EventVM();
        
            return e;
        }

        //controller verify that userID = ownerID
        // check if user is subscribe to that calendar
    }
}
