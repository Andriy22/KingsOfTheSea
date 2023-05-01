using Application.Common.DTOs.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthorizationVM> AuthorizationAsync(AuthorizationDTO model);
    Task<AuthorizationVM> RefreshAsync(RefreshTokenDTO model);
}