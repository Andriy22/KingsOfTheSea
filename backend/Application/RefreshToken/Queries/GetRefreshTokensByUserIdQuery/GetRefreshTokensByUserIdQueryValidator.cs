using FluentValidation;

namespace Application.RefreshToken.Queries.GetRefreshTokensByUserIdQuery;

public class GetRefreshTokensByUserIdQueryValidator : AbstractValidator<GetRefreshTokensByUserIdQuery>
{
    public GetRefreshTokensByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required.");
    }
}