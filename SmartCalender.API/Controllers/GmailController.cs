using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Services.EventService;
using SmartCalender.API.Services.MailSevice;

namespace SmartCalender.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GmailController : ControllerBase
    {
        private readonly IGmailService _gmailService;
        private readonly IEventService _eventService;

        public GmailController(IGmailService gmailService, IEventService eventService)
        {
            _gmailService = gmailService;
            _eventService = eventService;
        }

        /// <summary>
        /// Fetch a list of emails. Optional query can be passed (e.g. "is:unread").
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> ListEmails([FromQuery] string query = null)
        {
            var messages = await _gmailService.GetEmailAsync(query);
            return Ok(messages);
        }

        [HttpGet("Detailed list")]
        public async Task<IActionResult> DetailedListEmails([FromQuery] string query = null)
        {
            var messages = await _gmailService.ListAndFetchFullAsync(query);
            return Ok(messages);
        }


        /// <summary>
        /// Fetch a single email by ID, parse it with OpenAI, and create a Google Calendar event.
        /// </summary>
        [HttpPost("parse-and-create/{emailId}")]
        public async Task<IActionResult> ParseAndCreateEventFromEmail(string emailId)
        {
            // 1) Get the full email body
            var emailBody = await _gmailService.GetEmailBodyAsync(emailId);

            // 2) Parse it with your existing OpenAI logic
            var parsedEvent = await _eventService.ParseEventFromText(emailBody);

            // 3) Insert the event into Google Calendar
            var created = await _eventService.CreateCalendarEvent(parsedEvent);

            return Ok(created);
        }
    }
}
