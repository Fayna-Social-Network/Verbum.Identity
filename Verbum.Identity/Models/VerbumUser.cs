namespace Verbum.Identity.Models
{
    public class VerbumUser
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }  
        public bool IsOnline { get; set; }
        public DateTime UserRegistrationDate { get; set; }
    }
}
