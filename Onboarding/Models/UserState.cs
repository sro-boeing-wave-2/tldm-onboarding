namespace Onboarding.Models
{
    public class UserState
    {
        public int Id { get; set; }
        public string EmailId { get; set; }
        public bool IsJoined { get; set; }
        public string Otp { get; set; }
    }
}