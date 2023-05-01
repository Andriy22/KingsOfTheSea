using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RefreshToken.Queries.GetRefreshTokenByTokenQuery;

public class GetRefreshTokenByTokenQueryHandler : IRequestHandler<GetRefreshTokenByTokenQuery, Domain.RefreshToken>
{
    private readonly IDataContext _dataContext;

    public GetRefreshTokenByTokenQueryHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Domain.RefreshToken> Handle(GetRefreshTokenByTokenQuery request,
        CancellationToken cancellationToken)
    {
        return await _dataContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.Token, cancellationToken);
    }
}