using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;
using SmartCalender.API.Services.EventService;
using SmartCalender.API.Services.ParsingSevice;

namespace SmartCalender.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IParsingService _OpenAIParsingServicervice;
    public EventController(IParsingService OpenAIParsingService)
    {
        _OpenAIParsingServicervice = OpenAIParsingService;
    }

    [HttpPost("parse")]
    public ActionResult ParseEvent([FromBody] EventRequest request)
    {
        var parsedEvent = _OpenAIParsingServicervice.ParseEventFromText(request.EventAsText);
        return Ok(parsedEvent);
    }
}

