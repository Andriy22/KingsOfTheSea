using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string Nickname { get; set; }
        public DateTime LastLogIn { get; set; }
        public DateTime CreationTime { get; set; }
        public Language Language { get; set; }
    }
}