using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [Produces("application/json")]
    [Route("api/CalendarAPI")]
    public class CalendarAPIController : Controller
    {
    }
}