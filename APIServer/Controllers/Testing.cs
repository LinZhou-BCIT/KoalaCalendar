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
using Newtonsoft.Json;
//FOR TESTING
namespace APIServer.Controllers
{
    [Route("testingliam/[action]")]
    public class Testing : Controller
    {
        private readonly ICalendarRepo _calendarRepo;
        private readonly IEventRepo _eventRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        private const string DevUser = "a3ca0059-2788-40a7-aa23-ac6082d2d696";

        //injection
        public Testing(
            UserManager<ApplicationUser> userManager,
            ICalendarRepo calendarRepo,
            IEventRepo eventRepo)
        {
            _userManager = userManager;
            _calendarRepo = calendarRepo;
            _eventRepo = eventRepo;
        }

        [HttpPost]
        public async Task<object> test_createCal([FromForm] CalendarVM model)
        {
            string returnId = await _calendarRepo.CreateCalendar(model.Name, DevUser);
            return await Task.FromResult(Ok("Calendar Id : " + returnId));
        }

        [HttpPost]
        public async Task<object> test_GetAllCalendarsForUser()
        {
            var result = await _calendarRepo.GetOwnedCalendars(DevUser);
            var settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented, // Just for humans
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return await Task.FromResult(Ok(JsonConvert.SerializeObject(result, settings))); //Important
        }
        [HttpPost]
        public async Task<object> test_SearchCalendar([FromForm] string input)
        {
            string userID = HttpContext.User.Claims.ElementAt(2).Value;
            var result = await _calendarRepo.SearchCalendar(userID, input); //stop sql injection
            var settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented, // Just for humans
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return await Task.FromResult(Ok(JsonConvert.SerializeObject(result, settings))); //Important
        }
        [HttpPost]
        public async Task<object> test_UpdateCalendar()
        {
            var result = await _calendarRepo.UpdateCalendar(Guid.Parse("0eec9477-4ac1-4530-862b-db88b3b322cd"), "Test Calendar NEW");
            return await Task.FromResult(Ok(result));
        }
        [HttpPost]
        public async Task<object> test_genAcc()
        {
            var result = await _calendarRepo.GenerateAccessCode(Guid.Parse("0eec9477-4ac1-4530-862b-db88b3b322cd"));
            return await Task.FromResult(Ok(result));
        }
        [HttpPost]
        public async Task<object> test_deleteCal()
        {
            var result = await _calendarRepo.RemoveCalendar(Guid.Parse("0eec9477-4ac1-4530-862b-db88b3b322cd"));
            return await Task.FromResult(Ok(result));
        }
        //Event
        [HttpPost]
        public async Task<object> test_createEvent([FromForm] EventVM model)
        {
            string result = await _eventRepo.CreateEvent(Guid.Parse(model.CalendarID), model.Name, model.StartTime, model.EndTime);
            return await Task.FromResult(Ok("Event Created with ID: " + result));
        }

        [HttpPost]
        public async Task<object> test_getEventsTimeSpan([FromForm] string calID, DateTime start, DateTime end)
        {
            var result = await _eventRepo.GetEvents(Guid.Parse(calID), start, end);

            return await Task.FromResult(Ok(result));
        }
        [HttpPost]
        public async Task<object> test_getEvents([FromForm] string calID)
        {
            var result = await _eventRepo.GetEvents(Guid.Parse(calID));
            return await Task.FromResult(Ok(result));
        }
        [HttpPost]
        public async Task<object> test_updateEvent([FromForm] Event model, string eventID)
        {
            var result = await _eventRepo.UpdateEvent(model.EventID, model.Name, model.StartTime, model.EndTime);
            return await Task.FromResult(Ok(result));
        }
        [HttpPost]
        public async Task<object> test_deleteEvent([FromForm] string eventID)
        {
            var result = await _eventRepo.DeleteEvent(Guid.Parse(eventID));
            return await Task.FromResult(Ok(result));
        }
        [HttpPost]
        public async Task<object> text_getEvent([FromForm] string eventID)
        {
            var result = await _eventRepo.GetEventByID(Guid.Parse(eventID));
            return await Task.FromResult(Ok(result));
        }
    }
}