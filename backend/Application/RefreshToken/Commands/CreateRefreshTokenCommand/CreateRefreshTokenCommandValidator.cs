﻿using FluentValidation;

namespace Application.RefreshToken.Commands.CreateRefreshTokenCommand;

public class CreateRefreshTokenCommandValidator : AbstractValidator<CreateRefreshTokenCommand>
{
    public CreateRefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.CreatedAt).NotEmpty().WithMessage("CreatedAt is required.");
        RuleFor(x => x.ToLife).NotEmpty().WithMessage("ToLife is required.");
    }
}