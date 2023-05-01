using MediatR;

namespace Application.RefreshToken.Queries.GetRefreshTokenByTokenQuery;

public class GetRefreshTokenByTokenQuery : IRequest<Domain.RefreshToken>
{
    public Guid Token { get; set; }
}