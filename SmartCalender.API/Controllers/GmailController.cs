using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;
using SmartCalender.API.Services.EventService;
using SmartCalender.API.Services.MailSevice;
using System.Text;

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

        [HttpGet("list")]
        public async Task<IActionResult> ListEmails([FromQuery] string query = null)
        {
            var messages = await _gmailService.GetEmailListAsync(query);
            return Ok(messages);
        }

        [HttpPost("parse-and-create/{emailId}")]
        public async Task<IActionResult> ParseAndCreateEventFromEmail(string emailId)
        {
            var emailBody = await _gmailService.GetEmailDtoAsync(emailId);

            var combinedText = ConvertEmailDtoToString(emailBody);

            var parsedEvent = await _eventService.ParseEventFromText(combinedText);

            var created = await _eventService.CreateCalendarEvent(parsedEvent);

            return Ok(created);
        }
        private static string ConvertEmailDtoToString(EmailDto dto)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Subject: {dto.Subject}");
            sb.AppendLine($"From: {dto.From}");
            sb.AppendLine($"DateSent: {dto.DateSent}");
            sb.AppendLine();
            sb.AppendLine("---- BODY ----");
            sb.AppendLine(dto.Body);
            return sb.ToString();
        }

    }
}
