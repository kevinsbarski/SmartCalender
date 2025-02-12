using Google.Apis.Calendar.v3;
using OpenAI;
using SmartCalender.API.Models.Configuration;
using SmartCalender.API.Services.CalenderService;
using SmartCalender.API.Services.EventService;
using SmartCalender.API.Services.ParsingSevice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IParsingService, OpenAIParsingService>();
//builder.Services.AddScoped<ICalendarService, GoogleCalendarService>();
builder.Services.Configure<GoogleApiSettings>(builder.Configuration.GetSection(nameof(GoogleApiSettings)));
builder.Services.AddSingleton<ICalendarService>(provider =>
{   ICalendarService service = GoogleCalendarService
        .CreateAsync(provider.GetRequiredService<IConfiguration>())
        .GetAwaiter()
        .GetResult();
    return service;
});
builder.Services.AddScoped<IEventService, EventService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
