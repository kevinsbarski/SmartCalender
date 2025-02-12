using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;
using SmartCalender.API.Services.ParsingSevice;

namespace SmartCalender.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAiController : Controller
    {
        private readonly IParsingService _parsingService;
        public OpenAiController(IParsingService parsingService)
        {
            _parsingService = parsingService;
        }

        [HttpPost("parse")]
        public async Task<ActionResult<EventResponse>> ParseEventFromText([FromBody] EventRequest request)
        {
            var parsedEvent = await _parsingService.ParseEventFromTextAsync(request.EventAsText);
            return Ok(parsedEvent);
        }
    }
}
