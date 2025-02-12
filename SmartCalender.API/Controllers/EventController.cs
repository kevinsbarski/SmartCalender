using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;
using SmartCalender.API.Services.EventService;
using SmartCalender.API.Services.ParsingSevice;

namespace SmartCalender.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost("parse")]
    public async Task<ActionResult<EventResponse>> ParseEvent([FromBody] EventRequest request)
    {
        var parsedEvent = await _eventService.ParseEventFromText(request.EventAsText);
        return Ok(parsedEvent);
    }
    [HttpPost("create")]
    public async Task<ActionResult<EventResponse>> CreateEvent ([FromBody] CalendarEvent eventDetails)
    {
        var createdEvent = await _eventService.CreateCalendarEvent(eventDetails);
        return Ok(createdEvent);
    }
}

