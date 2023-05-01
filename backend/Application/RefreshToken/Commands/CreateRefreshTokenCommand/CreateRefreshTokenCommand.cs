using MediatR;

namespace Application.RefreshToken.Commands.CreateRefreshTokenCommand;

public class CreateRefreshTokenCommand : IRequest<Domain.RefreshToken>
{
    public Guid Token { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ToLife { get; set; }
    public bool IsExpired { get; set; }
    public string IpAddress { get; set; }
}