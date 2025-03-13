namespace SmartCalender.API.Models
{
    public class EmailDto
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string DateSent { get; set; }
        public string Body { get; set; }

    }
}
