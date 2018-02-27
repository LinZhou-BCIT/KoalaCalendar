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

        /*  Access Code Generation  */
        public async Task<string> GenerateAccessCode(string calendarID)
        {
            int codeLen = 5;            //Access code length
            String acCode = "";         //Access code string
            Random ran = new Random();  //Random generator instance

            while (acCode == "")
            {
                for (int i = 0; i < codeLen; i++)
                {
                    acCode += calendarID[ran.Next(0, calendarID.Length)];   //  Get random character from calendar's id and add that to the access code
                }

                var acTest = _context.Calendars.First(c => c.AccessCode == acCode); //Test if access code is already being used

                if (acTest != null)
                {
                    acCode = "";    //If it is, loop
                }
            }

            if (acCode == "")
            {
                throw new System.Exception("Invalid access code generated");   
                /*  
                    This should never be triggered
                */
            }

            var result = _context.Calendars.First(c => c.CalendarID.ToString() == calendarID); //Get calendar to update access code

            if (result != null)
            {
                result.AccessCode = acCode; //Access code prop is updated
                _context.SaveChanges();     //Save changed
            }else
            {
                throw new System.ArgumentException(String.Format("{0} is an invalid calendar id, calendar id does not exist", calendarID),"calendarID");
                //Calendar id that was fed in was not in database
            }

            return acCode;  //Returns access code
        }
        /*  End Access Code Generation  */

        public async Task<bool> RemoveCalendar(string calendarID)
        {
            var result = _context.Calendars.First(c => c.CalendarID.ToString() == calendarID);
            return true;
        }

        public async Task<bool> UnsubUserFromCalendar(string userID, string calendarID)
        {
            return true;
        }

    }
}
