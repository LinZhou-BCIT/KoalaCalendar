using APIServer.Data;
using APIServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class CalendarRepo: ICalendarRepo
    {
        ApplicationDbContext _context;
        public CalendarRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateCalendar(string calendarName, string ownerID)
        {
            Calendar newCal = new Calendar()
            {
                Name = calendarName,
                CalendarID = Guid.NewGuid(),
                OwnerID = ownerID
            };
            _context.Calendars.Add(newCal);

            _context.SaveChanges();
            //return calendarID once it is created
            return newCal.CalendarID.ToString();
        }

        public async Task<IEnumerable<Calendar>> GetAllCalendarsForUser(string userID)
        {
            // query for user
            return _context.Calendars.ToList();
        }

        public async Task<IEnumerable<Calendar>> SearchCalendar(string searchInput)
        {
            return null;
        }

        public async Task<bool> UpdateCalendar(string calendarID, string calendarName)
        {
            return true;
        }

        public async Task<string> GenerateAccessCode(string calendarID)
        {
            int codeLen = 5;
            String acCode = "";
            for (int i = 0; i < codeLen; i++)
            {
                Random ran = new Random();
                acCode += calendarID[ran.Next(0,calendarID.Length)];
            }
            // calendarID will be used to generate the accessCode
            //return access code
            return null;
        }

        public async Task<bool> RemoveCalendar(string calendarID)
        {
            return true;
        }

        public async Task<bool> UnsubUserFromCalendar(string userID, string calendarID)
        {
            return true;
        }

    }
}
