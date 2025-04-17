namespace SmartCalender.API.Models.Auth
{
    public class UserRegistrationRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
