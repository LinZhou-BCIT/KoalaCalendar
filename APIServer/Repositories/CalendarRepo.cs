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
            return await Task.FromResult(newCal.CalendarID.ToString());
        }

        public async Task<IEnumerable<Calendar>> GetAllCalendarsForUser(string userID) //Get subbed cal too
        {
            // query for user
            var result = _context.Calendars.Where(c => c.OwnerID == userID).AsEnumerable<Calendar>();
            return await Task.FromResult(result); //return list
        }

        public async Task<IEnumerable<Calendar>> SearchCalendar(string searchInput) //Get subbed cal too
        {
            var results = _context.Calendars.Where(c => c.Name.Contains(searchInput)).AsEnumerable<Calendar>();
            return await Task.FromResult(results);
        }
        
        /*  Update Calendar */
        public async Task<bool> UpdateCalendar(Guid calendarID, string calendarName)
        {
            var result = _context.Calendars.First(c => c.CalendarID == calendarID); //Find Calendar

            if (result != null)
            {
                result.Name = calendarName; //Update name
                _context.SaveChanges();     //Save changes
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
        /*  End Update Calendar */

        /*  Access Code Generation  */
        public async Task<string> GenerateAccessCode(Guid calendarID)
        {
            int codeLen = 5;            //Access code length
            String acCode = "";         //Access code string
            Random ran = new Random();  //Random generator instance
            String calendarIdString = calendarID.ToString();
            while (acCode == "")
            {
                for (int i = 0; i < codeLen; i++)
                {
                    acCode += calendarIdString[ran.Next(0, calendarIdString.Length)];   //  Get random character from calendar's id and add that to the access code
                }

                if (_context.Calendars.Any(c => c.AccessCode == acCode)) //Test if in database
                {
                    acCode = "";    //If it is, loop
                }
            }

            var result = _context.Calendars.Where(c => c.CalendarID == calendarID).First(); //Get calendar to update access code

            if (result != null)
            {
                result.AccessCode = acCode; //Access code prop is updated
                _context.SaveChanges();     //Save changed
            }else
            {
                throw new System.ArgumentException(String.Format("{0} is an invalid calendar id, calendar id does not exist", calendarID));
                //Calendar id that was fed in was not in database
            }

            return await Task.FromResult(acCode);  //Returns access code
        }
        /*  End Access Code Generation  */

        /*  Remove Calendar */
        public async Task<bool> RemoveCalendar(Guid calendarID)
        {
            var result = _context.Calendars.First(c => c.CalendarID == calendarID);  //Find Calendar

            if (result != null) //Check if null
            {
                _context.Calendars.Remove(result);  //Delete Result
                _context.SaveChanges(); //Save Changes
                return await Task.FromResult(true);
            }else
            {
                return await Task.FromResult(false);   //Return false if calendar was not found
            }
        }
        /*  End Remove Calendar */

        public async Task<bool> UnsubUserFromCalendar(string userID, Guid calendarID)
        {
            var result = _context.Subscriptions.Where(c => c.UserID == userID && c.CalendarID == calendarID).FirstOrDefault();
            if (result != null)
            {
                _context.Subscriptions.Remove(result);
                _context.SaveChanges();
                return await Task.FromResult(true);
            }else
            {
                return await Task.FromResult(false);
            }
        }

    }
}
