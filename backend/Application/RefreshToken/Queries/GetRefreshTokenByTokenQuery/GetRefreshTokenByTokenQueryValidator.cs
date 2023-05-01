using FluentValidation;

namespace Application.RefreshToken.Queries.GetRefreshTokenByTokenQuery;

public class GetRefreshTokenByTokenQueryValidator : AbstractValidator<GetRefreshTokenByTokenQuery>
{
    public GetRefreshTokenByTokenQueryValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
    }
}