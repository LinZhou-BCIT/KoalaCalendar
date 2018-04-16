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
                string newCalendarID = await _calendarRepo.CreateCalendar(model.Name, userID);
                // remove if not generating accessCode on create 
                await _calendarRepo.GenerateAccessCode(Guid.Parse(newCalendarID));
                return Ok(new { calendarID = newCalendarID });
            }
            return StatusCode(403, new { Message = "Only Professors can create custom calendars." });
        }

        [HttpGet]
        public async Task<object> GetCalendar(Guid calendarID)
        {
            Calendar calendar = await _calendarRepo.GetCalendarByID(calendarID);
            if (calendar != null)
            {
                return Ok(new { calendar = _calendarRepo.ConvertToVM(calendar) });
            }
            else
            {
                return StatusCode(400, new { Message = "Calendar not found." });
            }
        }

        [HttpGet]
        public async Task<object> GetOwnedCalendars()
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            var calendars = await _calendarRepo.GetOwnedCalendars(userID);
            return Ok(new { calendars });
        }

        [HttpGet]
        public async Task<object> GetSubbedCalendars()
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            var calendars = await _calendarRepo.GetSubbedCalendars(userID);
            return Ok(new { calendars });
        }


        [HttpPut]
        public async Task<object> UpdateCalendar([FromBody] CalendarVM model)
        {
            // validate if user is owner of calendar here ***********************************
            bool success = await _calendarRepo.UpdateCalendar(model.CalendarID, model.Name);
            if (success)
            {
                return Ok(new { Message = "Update successful." });
            }
            else
            {
                return StatusCode(400, new { Message = "Calendar not found." });
            }
        }

        [HttpDelete]
        public async Task<object> DeleteCalendar(Guid calendarID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            // validate if user is owner of calendar here ***********************************
            bool result = await _calendarRepo.RemoveCalendar(calendarID);

            if (result)
            {
                return Ok(new { message = "Calendar has been deleted successfully." });
            }
            else
            {
                return StatusCode(400, new { Message = "Deletion failed." });
            }
        }

        [HttpGet]
        public async Task<object> SubscribeToCalendar(string accessCode)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            bool success = await _calendarRepo.SubToCalendar(userID, accessCode);
            if (success)
            {
                return Ok(new { Message = "Subscription successful." });
            }
            else
            {
                return StatusCode(400, new { Message = "Subscription failed." });
            }
        }


        [HttpGet]
        public async Task<object> UnsubscribeFromCalendar(string calendarID)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            bool result = await _calendarRepo.UnsubUserFromCalendar(userID, Guid.Parse(calendarID));
            if (result)
            {
                return Ok(new { Message = "You have successfully unsubscribe to the calendar" });
            } else
            {
                return StatusCode(400, new { Message = "Unsub failed." });
            }
        }

        // This functionality will not be included for now
        //[HttpGet]
        //public async Task<object> SearchCalendar(string searchInput)
        //{
        //    //var listOfCalendars = await _calendarRepo.GetAllCalendarsForUser();

        //    //if (searchInput == null)
        //    //{
        //    //    return listOfCalendars;
        //    //}

        //    //return _calendarRepo.SearchCalendar(searchInput);
        //    return null;
        //}

        // Get any events of any calendar by ID, starttime and endtime
        [HttpPost]
        public async Task<object> GetEventsOfTimeRange([FromBody] EventRequestVM model)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            List<EventVM> listOfEvents = new List<EventVM>();
            List<string> listOfCalendarIDs = new List<string>();
            if (model.CalendarIDs == null || model.CalendarIDs.Length == 0)
            {
                IEnumerable<CalendarVM> ownedCalendars = await _calendarRepo.GetOwnedCalendars(userID);
                IEnumerable<CalendarVM> subbedCalendars = await _calendarRepo.GetSubbedCalendars(userID);
                listOfCalendarIDs.AddRange(ownedCalendars.Select(c => c.CalendarID.ToString()));
                listOfCalendarIDs.AddRange(subbedCalendars.Select(c => c.CalendarID.ToString()));
            } else
            {
                listOfCalendarIDs = model.CalendarIDs.ToList();
            }

            foreach(string calendarID in listOfCalendarIDs)
            {
                var events = await _eventRepo.GetEvents(Guid.Parse(calendarID), model.StartTime, model.EndTime);
                listOfEvents.AddRange(events);
            }
            listOfEvents = listOfEvents.OrderBy(e => e.StartTime).ToList();
            return Ok(new { events = listOfEvents });
        }

        [HttpGet]
        public async Task<object> GetEventByID(string eventID)
        {
            Event ev = await _eventRepo.GetEventByID(Guid.Parse(eventID));
            if (ev != null)
            {
                return Ok(new { Event = _eventRepo.ConvertToVM(ev) });
            }
            return StatusCode(400, new { Message = "Get event failed." });
        }

        [HttpPost]
        public async Task<string> AddEvent([FromBody] EventVM model)
        {
            // validate if user is owner of calendar here ***********************************
            return await _eventRepo.CreateEvent(Guid.Parse(model.CalendarID), model.Name, model.StartTime, model.EndTime);
        }

        [HttpPut]
        public async Task<bool> UpdateEvent([FromBody] EventVM model)
        {
            // validate if user is owner of calendar here ***********************************
            return await _eventRepo.UpdateEvent(Guid.Parse(model.EventID), model.Name, model.StartTime, model.EndTime);
        }

        [HttpDelete]
        public async Task<bool> DeleteEvent([FromBody] EventVM model)
        {
            // validate if user is owner of calendar here ***********************************
            return await _eventRepo.DeleteEvent(Guid.Parse(model.EventID));
        }

    }
}