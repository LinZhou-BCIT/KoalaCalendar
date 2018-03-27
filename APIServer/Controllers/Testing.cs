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
//FOR TESTING
namespace APIServer.Controllers
{
    //[Produces("application/json")]
    [Route("testingliam/[action]")]
    public class Testing : Controller
    {
        private readonly ICalendarRepo _calendarRepo;
        private readonly IEventRepo _eventRepo;
        private readonly UserManager<ApplicationUser> _userManager;

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
        public async Task<object> test_createCal()
        {
            string returnId = await _calendarRepo.CreateCalendar("Test Calendar", "5ff3d103-a4aa-45d4-b288-7f55f77a3d4f");
            return Ok(returnId);
        }
        [HttpPost]
        public async Task<object> test_GetAllCalendarsForUser()
        {
            var result = await _calendarRepo.GetAllCalendarsForUser("5ff3d103-a4aa-45d4-b288-7f55f77a3d4f");
            return Ok(result.First());
        }
        [HttpPost]
        public async Task<object> test_SearchCalendar()
        {
            var result = await _calendarRepo.SearchCalendar("Test");
            return Ok(result.First());
        }
        [HttpPost]
        public async Task<object> test_UpdateCalendar()
        {
            var result = await _calendarRepo.UpdateCalendar(Guid.Parse("0eec9477-4ac1-4530-862b-db88b3b322cd"),"Test Calendar NEW");
            return Ok(result);
        }
        [HttpPost]
        public async Task<object> test_genAcc()
        {
            var result = await _calendarRepo.GenerateAccessCode(Guid.Parse("0eec9477-4ac1-4530-862b-db88b3b322cd"));
            return Ok(result);
        }
        [HttpPost]
        public async Task<object> test_deleteCal()
        {
            var result = await _calendarRepo.RemoveCalendar(Guid.Parse("0eec9477-4ac1-4530-862b-db88b3b322cd"));
            return Ok(result);
        }
    }
}