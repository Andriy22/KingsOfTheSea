using FluentValidation;

namespace Application.RefreshToken.Commands.UpdateRefreshTokenCommand;

public class UpdateRefreshTokenCommandValidator : AbstractValidator<UpdateRefreshTokenCommand>
{
    public UpdateRefreshTokenCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}