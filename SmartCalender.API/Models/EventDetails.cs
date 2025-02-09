namespace SmartCalender.API.Models
{
    public class EventDetails
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; } 
        public string Location { get; set; }
        
    }
}
