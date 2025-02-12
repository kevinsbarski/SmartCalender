using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;
using SmartCalender.API.Services.CalenderService;
using SmartCalender.API.Services.ParsingSevice;

namespace SmartCalender.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : Controller
    {
        private readonly ICalendarService _googleCalendarService;
        public CalendarController(ICalendarService googleCalendarService)
        {
            _googleCalendarService = googleCalendarService;
        }

        [HttpPost("create event")]
        public async Task<ActionResult<EventResponse>> ParseEventFromText([FromBody] CalendarEvent request)
        {
            var parsedEvent = await _googleCalendarService.CreateEvent(request);
            return Ok();
        }
    }
}
