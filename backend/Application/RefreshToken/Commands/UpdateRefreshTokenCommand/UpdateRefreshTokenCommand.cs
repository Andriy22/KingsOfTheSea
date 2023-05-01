using MediatR;

namespace Application.RefreshToken.Commands.UpdateRefreshTokenCommand;

public class UpdateRefreshTokenCommand : IRequest
{
    public int Id { get; set; }
    public bool IsExpired { get; set; }
}