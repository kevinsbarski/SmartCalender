using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using SmartCalender.API.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartCalender.API.Services.ParsingSevice
{
    public class OpenAIParsingService : IParsingService
    {
        private readonly string _openAiApiKey;
        public OpenAIParsingService(IConfiguration configuration)
        {
            _openAiApiKey = configuration.GetSection("OpenAI:ApiKey").Value;
        }

        public async Task<EventResponse> ParseEventFromText(string text)
        {

            ChatClient client = new(model: "gpt-4o", _openAiApiKey);
            var eventDetails = new EventDetails();

            System.DateTime present = System.DateTime.UtcNow;

            string eventDetailsJson = JsonSerializer.Serialize(eventDetails);


            string prompt =
                    $"Today is {present}" +
                    $"Extract the event details from the following text and return an object in this format without adding any comments: {eventDetailsJson}\n\n" +
                    $"If the event date is referred to using relative terms such as 'next Wednesday' or 'this Thursday', interpret them correctly to the nearest upcoming date." +
                    $"if there is no DATE and words like tommorow or after tommorow then calculate the current date and add to it the days please" +
                    $"Also if the text is in Hebrew then use hebrew calendar days not english , you have to check if its hebrew or english to know in which" +
                    $"Language to fill the Json fields." +
                    $"If no date is provided, assume that the event is on the closest future date mentioned, based on the context of the text.\n\n" +
                    $"Also relate to Asia/Jerusalem timezone please, GMT+2 Israel time." +
                    $"Text: \"{text}\"";
            var completion =  await client.CompleteChatAsync(prompt);
            EventDetails? result = JsonSerializer.Deserialize<EventDetails>(completion.Value.Content[0].Text);


            return new EventResponse
            {
                ParsedEvent = result,
                Message = "please review the event before confirming"
            };
        }
        }
    }

