using APIServer.Data;
using APIServer.Models;
using APIServer.Models.CalendarViewModels;
using Microsoft.EntityFrameworkCore;
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
            _context = context; //Import context
        }

        /* Created Calendar */
        public async Task<string> CreateCalendar(string calendarName, string ownerID) //Creates a new calendar
        {
            Calendar newCal = new Calendar() //Create instance of new calendar and set properties 
            {
                Name = calendarName, 
                CalendarID = Guid.NewGuid(),
                OwnerID = ownerID
            };

            await _context.Calendars.AddAsync(newCal); //Add new calendar to database
            await _context.SaveChangesAsync(); //Save changes

            return newCal.CalendarID.ToString(); //Return newly created calendars ID
        }
        /* End Create Calendar */

        /* Get Calendar By ID */
        public async Task<Calendar> GetCalendarByID(Guid calendarID)
        {
            Calendar result = await _context.Calendars.Where(c => c.CalendarID == calendarID).Include(c => c.Owner).FirstOrDefaultAsync();
            return result;
        }
        /* End Get Calendar By ID */

        /* Get Owned Calendars */
        public async Task<IEnumerable<CalendarVM>> GetOwnedCalendars(string userID) //Get all calendars user has made
        {
            //var result = _context.Calendars.Where(c => c.OwnerID == userID).AsEnumerable<Calendar>(); //query for user
            //return await Task.FromResult(result); //return list
            IQueryable<CalendarVM> ownedCalendars = _context.Calendars.Where(c => c.OwnerID == userID).Include(c => c.Owner)
                .Select(c => ConvertToVM(c));
            return await ownedCalendars.ToListAsync();
        }
        /* End Get Owned Calendars */

        /* Get Subbed Calendars */
        public async Task<IEnumerable<CalendarVM>> GetSubbedCalendars(string userID) //Get all calendars user has made
        {
            IQueryable<Calendar> sharedCalendars = _context.Calendars
                .Join(_context.Subscriptions,
                        c => c.CalendarID,
                        s => s.CalendarID,
                        (c, s) => new { c, s })
                        .Where(joined => joined.s.UserID == userID)
                        .Select(j => j.c);
            IQueryable<CalendarVM> sharedCalendarVMs = sharedCalendars.Include(c => c.Owner).Select(c => ConvertToVM(c));
            return await sharedCalendarVMs.ToListAsync();
        }
        /* End Get Subbed Calendars */

        /* Search Calendar */
        public async Task<IEnumerable<Calendar>> SearchCalendar(string userID, string searchInput)
        {
            var results = _context.Calendars.Include(b => b.Events)
                .Where(c => c.Name.Contains(searchInput) && c.OwnerID == userID).AsEnumerable<Calendar>(); //Get all calendars with name that matches input

            return await Task.FromResult(results); //Return IEnumerable
        }
        /* End Search Calendar */


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
                    acCode += calendarIdString[ran.Next(0, calendarIdString.Length)];   //Get random character from calendar's id and add that to the access code
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
                _context.SaveChanges();     //Save changes
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

        public async Task<bool> SubToCalendar(string userID, string accessCode)
        {
            Calendar calendar = await _context.Calendars.Where(c => c.AccessCode == accessCode)
                .FirstOrDefaultAsync();
            if (calendar != null)
            {
                Subscription newSub = new Subscription()
                {
                    UserID = userID,
                    CalendarID = calendar.CalendarID
                };
                Subscription checkExistSub = await _context.Subscriptions
                    .Where(s => s.UserID == userID && s.CalendarID == newSub.CalendarID).FirstOrDefaultAsync();
                if (checkExistSub == null)
                {
                    // only insert of not already subbed
                    await _context.Subscriptions.AddAsync(newSub);
                    await _context.SaveChangesAsync();
                }
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<bool> UnsubUserFromCalendar(string userID, Guid calendarID)
        {
            var result = _context.Subscriptions.Where(c => c.UserID == userID && c.CalendarID == calendarID).FirstOrDefault(); //Find sub to remove

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

        public CalendarVM ConvertToVM(Calendar calendar)
        {
            CalendarVM vm = new CalendarVM()
            {
                CalendarID = calendar.CalendarID,
                Name = calendar.Name,
                AccessCode = calendar.AccessCode,
                OwnerID = calendar.OwnerID,
                OwnerEmail = calendar.Owner.Email
            };
            return vm;
        }

        public async Task<bool> VerifyCalendarOwnder(string userID, Guid calendarID)
        {
            var result = await _context.Calendars.Where(c => c.OwnerID == userID).Select(b => b.CalendarID).ToListAsync();
            if (result.Contains(calendarID)) {
                return true;
            }else
            {
                return false;
            }
        }
    } 
}
