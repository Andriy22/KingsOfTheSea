using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.DTOs.Auth;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.RefreshToken.Commands.UpdateRefreshTokenCommand;
using Application.RefreshToken.Queries.GetRefreshTokenByTokenQuery;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IJWTService _jwtService;
    private readonly IMediator _mediator;
    private readonly UserManager<Player> _userManager;

    public AuthService(UserManager<Player> userManager, IMediator mediator, IJWTService jwtService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _mediator = mediator;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<AuthorizationVM> AuthorizationAsync(AuthorizationDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null) throw new CustomHttpException("Email or password is invalid!");

        var isPassword = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!isPassword) throw new CustomHttpException("Email or password is invalid!");

        return new AuthorizationVM
        {
            Access_token = _jwtService.CreateToken(user),
            Refresh_token = await _jwtService.CreateRefreshTokenAsync(user),
            UserName = model.Email,
            Roles = (await _userManager.GetRolesAsync(user)).ToList(),
            UserId = user.Id
        };
    }

    public async Task<AuthorizationVM> RefreshAsync(RefreshTokenDTO model)
    {
        var token = await _mediator.Send(new GetRefreshTokenByTokenQuery
        {
            Token = Guid.Parse(model.RefreshToken)
        });

        var refresh_time = _configuration.GetSection("JWT").GetValue<int>("REFRESH_LIFETIME");

        if (token == null) throw new CustomHttpException("We can't find your token...");

        if (token.IsExpired) throw new CustomHttpException("Refresh token is expired...");

        if (token.ToLife.AddMinutes(refresh_time) <= DateTime.Now)
            throw new CustomHttpException("Refresh token is expired...");

        var handler = new JwtSecurityTokenHandler();
        var decrypt_token = handler.ReadJwtToken(model.AccessToken);

        if (decrypt_token.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value !=
            token.UserId) throw new CustomHttpException("Looks like a stolen token!");

        await _mediator.Send(new UpdateRefreshTokenCommand
        {
            Id = token.Id,
            IsExpired = true
        });

        var user = await _userManager.FindByIdAsync(token.UserId);

        return new AuthorizationVM
        {
            Access_token = _jwtService.CreateToken(user),
            Refresh_token = await _jwtService.CreateRefreshTokenAsync(user),
            UserName = user.Email,
            Roles = (await _userManager.GetRolesAsync(user)).ToList(),
            UserId = user.Id
        };
    }
}