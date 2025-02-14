
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using SmartCalender.API.Models.Configuration;
using SmartCalender.API.Services.CalenderService;
using SmartCalender.API.Services.EventService;
using SmartCalender.API.Services.ParsingSevice;
using Microsoft.AspNetCore.Authentication.Google;


var builder = WebApplication.CreateBuilder(args);

var googleApiSettings = builder.Configuration.GetSection("GoogleApiSettings");
builder.Services.AddScoped<IGoogleApiSettings>(sp =>
    sp.GetRequiredService<IOptions<GoogleApiSettings>>().Value);

builder.Services.Configure<GoogleApiSettings>(builder.Configuration.GetSection(nameof(GoogleApiSettings)));
builder.Services.AddScoped<IParsingService, OpenAIParsingService>();
builder.Services.AddScoped<ICalendarService, GoogleCalendarService>();
builder.Services.AddScoped<IEventService, EventService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = googleApiSettings["ClientId"]!;
    options.ClientSecret = googleApiSettings["ClientSecret"]!;
    options.CallbackPath = "/signin-google";
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
