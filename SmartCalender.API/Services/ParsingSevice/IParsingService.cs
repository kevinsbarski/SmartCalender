using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;

namespace SmartCalender.API.Services.ParsingSevice;

public interface IParsingService
{
    Task<EventResponse> ParseEventFromTextAsync(string text);
}

