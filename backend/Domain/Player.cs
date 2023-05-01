using Microsoft.AspNetCore.Identity;

namespace Domain;

public class Player : IdentityUser
{
    public string Nickname { get; set; }
    public DateTime LastLogIn { get; set; }
    public DateTime CreationTime { get; set; }

    public int EloRating { get; set; }
}