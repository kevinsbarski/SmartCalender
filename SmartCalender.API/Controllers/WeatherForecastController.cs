using Microsoft.AspNetCore.Mvc;
using SmartCalender.API.Models;
using System;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SmartCalender.API.Controllers
{
    [ApiController]
    [Route("[controller]")] // Base route from plan (adjust if needed)
    public class WeatherForecastController : ControllerBase
    {
        // --- Add Logger ---
        // NOTE: Plan assumes logger injection setup in Program.cs/Startup.cs
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        // --- End Logger ---


        // Placeholder method to simulate retrieving forecast data.
        // In a real application, this would involve accessing a data source or service.
        private WeatherForecast? GetForecastForDate(DateTime date)
        {
            // Example placeholder logic: Return a fixed forecast for demonstration.
            // Replace this with your actual forecast retrieval logic.
            // Consider adding randomness or fetching from a mock service if needed.
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            // Simple example: return null if date is too far in the future for this placeholder
            if ((date.Date - DateTime.Today).TotalDays > 14)
            { // Compare Date part only
                return null;
            }

            // Seed Random with Date parts for some pseudo-consistency if needed
            var rng = new Random(date.DayOfYear + date.Year);

            return new WeatherForecast
            {
                Date = date, // Use the input date
                TemperatureC = rng.Next(-20, 55), // Example temperature
                Summary = summaries[rng.Next(summaries.Length)]  // Example summary
            };
        }

        // New GET endpoint as requested
        [HttpGet("forecast")] // Route: /WeatherForecast/forecast?daysFromNow=X
        [ProducesResponseType(typeof(WeatherForecast), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Added for completeness
        public IActionResult GetWeatherForecast([FromQuery] int daysFromNow)
        {
            // Input Validation
            if (daysFromNow < 0)
            {
                _logger?.LogWarning("Invalid input: daysFromNow ({Days}) cannot be negative.", daysFromNow); // Use logger safely
                return BadRequest("daysFromNow must be a non-negative integer.");
            }

            try
            {
                // Use UtcNow or Today depending on desired timezone behavior
                DateTime targetDate = DateTime.Today.AddDays(daysFromNow);
                _logger?.LogInformation("Requesting forecast for {TargetDate}", targetDate.ToShortDateString());

                // Retrieve the forecast using the placeholder logic
                // RECOMMENDATION: Make GetForecastForDate async Task<WeatherForecast?> and await here
                var forecast = GetForecastForDate(targetDate);

                // Handle case where no forecast is found
                if (forecast == null)
                {
                    _logger?.LogWarning("No forecast found for {TargetDate}", targetDate.ToShortDateString());
                    return NotFound($"No weather forecast available for {targetDate:yyyy-MM-dd}.");
                }

                _logger?.LogInformation("Returning forecast for {TargetDate}", targetDate.ToShortDateString());
                // Return the found forecast
                return Ok(forecast);
            }
            catch (ArgumentOutOfRangeException ex) // Catch specific date calculation error
            {
                _logger?.LogError(ex, "Date calculation error for daysFromNow = {Days}", daysFromNow);
                return BadRequest("The value for 'daysFromNow' resulted in an invalid date calculation.");
            }
            catch (Exception ex)
            {
                // Log the exception details (using a proper logging framework is recommended)
                _logger?.LogError(ex, "Error retrieving forecast for daysFromNow = {Days}", daysFromNow);
                // Return a generic server error response
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        // Add this method inside the WeatherForecastController class
        [HttpGet("{date}")] // Define the route parameter for the date
        public ActionResult<WeatherForecast> GetSpecificDate(DateTime date)
        {
            // Simulate generating forecasts (same logic as the original Get)
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            // Find the forecast for the specific date requested
            // Note: Comparing DateOnly with just the Date part of DateTime
            var specificForecast = forecasts.FirstOrDefault(f => f.Date == DateOnly.FromDateTime(date.Date));

            if (specificForecast == null)
            {
                // Return 404 Not Found if no forecast matches the date
                return NotFound($"No forecast found for date: {date.ToShortDateString()}");
            }

            // Return the specific forecast
            return Ok(specificForecast);
        }

        // --- Keep any other existing methods in this controller if it already existed ---
        // (Context did not provide existing methods, so none are included here)

[HttpGet("monthlyReport")]
public async Task<IActionResult> GetMonthlyReport()
{
    _logger.LogInformation("Getting monthly weather report");
    try 
    {
        var report = new List<WeatherForecast>();
        for (int i = 0; i < 30; i++) 
        {
            report.Add(GetForecastForDate(DateTime.Today.AddDays(i)));
        }
        return Ok(report);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating monthly report");
        return StatusCode(500, "An error occurred");
    }
}
    }
}