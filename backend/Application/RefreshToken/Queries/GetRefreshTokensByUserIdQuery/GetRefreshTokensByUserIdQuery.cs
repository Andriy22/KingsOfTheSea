using MediatR;

namespace Application.RefreshToken.Queries.GetRefreshTokensByUserIdQuery;

public class GetRefreshTokensByUserIdQuery : IRequest<List<Domain.RefreshToken>>
{
    public string UserId { get; set; }
}