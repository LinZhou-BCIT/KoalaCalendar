using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIServer.Models.CalendarViewModels;
using APIServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Produces("application/json")]
    [Route("api/CalendarAPI")]
    [Authorize]
    public class CalendarAPI : Controller
    {
        private readonly CalendarRepo _calendarRepo;
        private readonly EventRepo _eventRepo;

        //injection
        public CalendarAPI(
        CalendarRepo calendarRepo,
        EventRepo eventRepo)
        {
            _calendarRepo = calendarRepo;
            _eventRepo = eventRepo;
        }

        //[Authorize (Roles = "Professor")]
        [HttpPost]
        public async Task<object> CreateCalendar([FromBody] CalendarVM model)
        {
            return Ok("Calendar has been created successfully.");
        }

        [HttpPost]
        public async Task<object> SubscribeCalendar(string accesCode)
        {
            return Ok("You are now subscribed to the calendar.");
        }

        [HttpGet]
        public async Task<object> GetCalendar(string searchInput)
        {
            var listOfCalendars = await _calendarRepo.GetAllCalendars();

            if (searchInput == null)
            {
                return listOfCalendars;
            }
            
            return _calendarRepo.SearchCalendar(searchInput);
        }

        // Get any events of any calendar by ID, starttime and endtime
        [HttpPost]
        public async Task<object> GetEventsByCalendarIDs([FromBody] EventRequestVM model)
        {
            List<IEnumerable<EventVM>> listOfEventLists = new List<IEnumerable<EventVM>>();

            foreach(string calendarID in model.CalendarIDs)
            {
                var events = await _eventRepo.GetEvents(calendarID, model.StartTime, model.EndTime);
                listOfEventLists.Add(events);
            }

            return listOfEventLists;
        }

        [HttpDelete]
        public async Task<object> DeleteCalendar([FromBody] CalendarVM model)
        {
            bool result = await _calendarRepo.RemoveCalendar(model.CalendarID);

            if (result)
            {
                string message = "Calendar has been deleted successfully.";
                return Ok(message);
            }

            return BadRequest("The calendar cannot be deleted.");
        }

        [HttpPost]
        public async Task<object> UnsubscribeCalendar([FromBody] CalendarVM model)
        {
            string userID = "sampleid";
           
            bool result = await _calendarRepo.UnassignCalendar(userID, model.CalendarID);

            if (result)
            {
                return Ok("You have successfully unsubscribe to the calendar");
            }

            return BadRequest("The request to unsubscribe to the calendar is unsuccessful.");
        }

        [HttpPost]
        public async Task<string> AddEvent([FromBody] EventVM model)
        {
            return await _eventRepo.CreateEvent(model.CalendarID, model.EventName, model.StartTime, model.EndTime);
        }

        [HttpPut]
        public async Task<bool> UpdateEvent([FromBody] EventVM model)
        {
            return await _eventRepo.UpdateEvent(model.EventID, model.EventName, model.StartTime, model.EndTime);
        }

        [HttpDelete]
        public async Task<bool> DeleteEvent([FromBody] EventVM model)
        {
            return await _eventRepo.DeleteEvent(model.EventID);
        }

        [HttpPost]
        public async Task<object> GetEventByID([FromBody] EventVM model)
        {
            return await _eventRepo.GetEventByID(model.EventID);
        }
    }
}