using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand>
{
    private readonly IDataContext _dataContext;

    public UpdateRatingCommandHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Unit> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        var user = _dataContext.Users.FirstOrDefault(x => x.Id == request.UserId);

        if (user == null) throw new CustomHttpException("User not found!");

        user.EloRating = request.NewEloRating;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}