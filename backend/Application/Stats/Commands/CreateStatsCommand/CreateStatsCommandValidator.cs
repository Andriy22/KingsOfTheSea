using Application.Interfaces;
using FluentValidation;

namespace Application.Stats.Commands.CreateStatsCommand;

public class CreateStatsCommandValidator : AbstractValidator<CreateStatsCommand>
{
    private readonly IDataContext _context;

    public CreateStatsCommandValidator(IDataContext context)
    {
        _context = context;

        RuleFor(x => x.PlayerId)
            .NotEmpty().WithMessage("Player ID is required.");

        RuleFor(x => x.EloDelta)
            .NotEqual(0).WithMessage("Elo delta cannot be zero.");
    }
}