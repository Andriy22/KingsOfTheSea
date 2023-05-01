using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RefreshToken.Queries.GetRefreshTokensByUserIdQuery;

public class
    GetRefreshTokensByUserIdQueryHandler : IRequestHandler<GetRefreshTokensByUserIdQuery, List<Domain.RefreshToken>>
{
    private readonly IDataContext _dataContext;

    public GetRefreshTokensByUserIdQueryHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Domain.RefreshToken>> Handle(GetRefreshTokensByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _dataContext.RefreshTokens
            .Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken);
    }
}