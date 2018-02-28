using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIServer.Models;
using APIServer.Models.CalendarViewModels;
using APIServer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace APIServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CalendarAPI : Controller
    {
        private readonly ICalendarRepo _calendarRepo;
        private readonly IEventRepo _eventRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        //injection
        public CalendarAPI(
            UserManager<ApplicationUser> userManager,
            ICalendarRepo calendarRepo,
            IEventRepo eventRepo)
        {
            _userManager = userManager;
            _calendarRepo = calendarRepo;
            _eventRepo = eventRepo;
        }

        //[Authorize (Roles = "Professor")]
        [HttpPost]
        public async Task<object> CreateCalendar([FromBody] CalendarVM model)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            bool isProf = await _userManager.IsInRoleAsync(user, "PROFESSOR");
            if (isProf)
            {
                string newCalendarID = await _calendarRepo.CreateCalendar(model.CalendarName, userID);
                return Ok(new { calendarID = newCalendarID });
            }
            return StatusCode(403, new { Message = "Only Professors can create custom calendars." });
        }

        [HttpPost]
        public async Task<object> SubscribeCalendar(string accesCode)
        {
            return Ok("You are now subscribed to the calendar.");
        }

        [HttpGet]
        public async Task<object> GetCalendar(string searchInput)
        {
            //var listOfCalendars = await _calendarRepo.GetAllCalendarsForUser();

            //if (searchInput == null)
            //{
            //    return listOfCalendars;
            //}

            //return _calendarRepo.SearchCalendar(searchInput);
            return null;
        }

        // Get any events of any calendar by ID, starttime and endtime
        [HttpPost]
        public async Task<object> GetEventsByCalendarIDs([FromBody] EventRequestVM model)
        {
            List<IEnumerable<Event>> listOfEventLists = new List<IEnumerable<Event>>();

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
           
            bool result = await _calendarRepo.UnsubUserFromCalendar(userID, model.CalendarID);

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