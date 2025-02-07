namespace SmartCalender.API.Models
{
    public class EventDetails
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public EventDateTime? Start {  get; set; }
        public EventDateTime? End { get; set; } 
        public string Location { get; set; }
        
    }
}
