using System.Net;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.RefreshToken.Commands.UpdateRefreshTokenCommand;

public class UpdateRefreshTokenCommandHandler : IRequestHandler<UpdateRefreshTokenCommand>
{
    private readonly IDataContext _dataContext;

    public UpdateRefreshTokenCommandHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Unit> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken =
            await _dataContext.RefreshTokens.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (refreshToken == null)
            throw new CustomHttpException($"Refresh token with id [${request.Id}] not found", HttpStatusCode.NotFound);

        refreshToken.IsExpired = request.IsExpired;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}