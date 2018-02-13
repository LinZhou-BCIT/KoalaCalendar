using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIServer.Models.CalendarViewModels;

namespace APIServer.Repositories
{
    public class EventRepo
    {
        public async Task<IEnumerable<EventVM>> GetEvents(string calendarID, DateTime startTime, DateTime endTime)
        {

            return null;
        }

        //Create Event
        // pass in calendarID as a parameter

        //Update Event

        //Delete Event

        //controller verify that userID = ownerID
        // check if user is subscribe to that calendar
    }
}
