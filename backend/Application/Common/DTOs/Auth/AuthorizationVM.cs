namespace Application.Common.DTOs.Auth;

public class AuthorizationVM
{
    public AuthorizationVM()
    {
        Roles = new List<string>();
    }

    public string UserId { get; set; }
    public string Access_token { get; set; }
    public string Refresh_token { get; set; }
    public string UserName { get; set; }
    public List<string> Roles { get; set; }
}