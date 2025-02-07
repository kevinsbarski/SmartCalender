using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;

namespace SmartCalender.API.Services.ParsingSevice;

public interface IParsingService
{
    public EventResponse ParseEventFromText(string text);
}

