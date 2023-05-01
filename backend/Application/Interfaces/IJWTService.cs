using Domain;

namespace Application.Interfaces;

public interface IJWTService
{
    Task<string> CreateRefreshTokenAsync(Player user);
    string CreateToken(Player user);
}