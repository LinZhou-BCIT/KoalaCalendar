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
        public async Task<object> AddCalendar([FromBody] CalendarVM model)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<object> SubscribeCalendar(string accesCode)
        {
            return Ok();
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
        public async Task<object> DeleteCalendar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEvent()
        {
            return View();
        }

        [HttpPut]
        public IActionResult UpdateEvent()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult DeleteEvent()
        {
            return View();
        }
    }
}