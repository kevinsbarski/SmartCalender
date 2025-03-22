# SmartCalendar 🧠📅

**SmartCalendar** is a C# .NET 8 Web API side project that automates calendar event creation by integrating Google Calendar, Gmail, and OpenAI. It parses free text (including email content) and intelligently schedules events in your Google Calendar.

> ⚠️ **Note:** This is a personal side project and still a work in progress. Many features are under development, and there’s a lot more to build and improve.

---

## ✨ Features

- Parse free-form text into structured calendar events using OpenAI (GPT-4o)
- Read and analyze Gmail messages to extract event details
- Automatically create events in Google Calendar
- Understand relative dates (e.g., "next Thursday")

---

## 🛠️ Technologies Used

- ASP.NET Core 8.0
- Google Calendar API
- Gmail API
- OpenAI API (GPT-4o)
- Swagger (Swashbuckle)
- OAuth2 (Google authentication)
- Newtonsoft.Json

---

## ⚙️ How It Works

1. **Gmail Integration**  
   Retrieves emails from Gmail using OAuth2, including subject, body, and metadata.

2. **AI-Powered Parsing**  
   Sends text to OpenAI to extract event details like title, date, time, location, and description.

3. **Google Calendar Integration**  
   Creates the event in your Google Calendar using the parsed details.

---

## 📁 Project Structure
- `SmartCalender.API/`
  - `Controllers/` – API endpoints (Gmail, Event, Calendar, OpenAI)
  - `Models/` – DTOs and config models
    - `Configuration/` – Google API settings
  - `Services/` – Business logic layers
    - `CalendarService/` – Google Calendar integration
    - `MailService/` – Gmail API integration
    - `ParsingService/` – OpenAI-powered text parser
  - `Program.cs` – Application entry point
  - `appsettings.json` – API keys and configuration


---

## 🚀 Getting Started

> **Requirements:** .NET 8 SDK, Google Cloud credentials, OpenAI API key

1. Clone the repository:

git clone https://github.com/kevinsbarski/SmartCalendar.git

2. Configure your `appsettings.json` with Google and OpenAI API credentials.

3. Run the project:
dotnet run

4. Open Swagger UI in your browser:  
`https://localhost:{PORT}/swagger`

---

## 🔌 API Endpoints

- `GET /api/gmail/list`  
List Gmail messages.

- `POST /api/gmail/parse-and-create/{emailId}`  
Parse an email and create a calendar event.

- `POST /api/openai/parse`  
Parse a raw text prompt into an event.

- `POST /api/event/create`  
Create an event manually with structured data.

---

## 🚧 Work in Progress

Some areas still being developed:

- Error handling and input validation
- UI (currently backend only)
- Calendar conflict handling
- Authentication flow refinement

---

## 📄 License

Licensed under the [MIT License](LICENSE).

---
