using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIServer.Data;
using APIServer.Models;
using APIServer.Models.CalendarViewModels;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Repositories
{
    public class EventRepo: IEventRepo
    {
        ApplicationDbContext _context;
        public EventRepo(ApplicationDbContext context)
        {
            _context = context;

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
            if (newEvent.Name == null) newEvent.Name = "Event";

            _context.Events.Add(newEvent);
            _context.SaveChanges();
            // return eventID
            return await Task.FromResult(newEvent.EventID.ToString());
        }

        public async Task<IEnumerable<Event>> GetEvents(Guid calendarID)
        {
            var result = _context.Events.Where(c => c.CalendarID == calendarID).AsEnumerable<Event>();
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<EventVM>> GetEvents(Guid calendarID, DateTime startTime, DateTime endTime)
        {
            //var result = _context.Events.Where(c => c.CalendarID == calendarID && c.EndTime <= endTime && c.StartTime >= startTime).AsEnumerable<Event>();
            // get all overlapping events
            IQueryable<EventVM> result = _context.Events
                .Where(c => c.CalendarID == calendarID && c.EndTime >= startTime && c.StartTime <= endTime)
                .Include(e => e.Calendar).Select(e => ConvertToVM(e));
            return await result.ToListAsync();
        }

        public async Task<bool> UpdateEvent(Guid eventID, string newEventName, DateTime newStartTime, DateTime newEndTime)
        {
            var result = _context.Events.Where(c => c.EventID == eventID).First();

            if (result != null)
            {
                if (newEventName != null)result.Name         = newEventName;
                if (newStartTime != null)result.StartTime    = newStartTime;
                if (newEndTime   != null)result.EndTime      = newEndTime;
                _context.SaveChanges();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteEvent(Guid eventID)
        {
            var result = _context.Events.Where(c => c.EventID == eventID).FirstOrDefault();

            if (result != null)
            {
                _context.Events.Remove(result);
                _context.SaveChanges();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Event> GetEventByID(Guid eventID)
        {
            var result = await _context.Events.Where(c => c.EventID == eventID).Include(e => e.Calendar).FirstOrDefaultAsync();
            return result;
        }

        public EventVM ConvertToVM(Event ev)
        {
            EventVM vm = new EventVM()
            {
                EventID = ev.EventID.ToString(),
                CalendarID = ev.CalendarID.ToString(),
                Name = ev.Name,
                StartTime = ev.StartTime,
                EndTime = ev.EndTime,
                CalendarName = ev.Calendar.Name
            };
            return vm;
        }

        // controller verify that userID = ownerID
        // check if user is subscribe to that calendar
    }
}
