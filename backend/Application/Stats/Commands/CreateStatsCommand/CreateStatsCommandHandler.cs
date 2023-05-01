using Application.Common.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Stats.Commands.CreateStatsCommand;

public class CreateStatsCommandHandler : IRequestHandler<CreateStatsCommand, int>
{
    private readonly IDataContext _context;

    public CreateStatsCommandHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateStatsCommand request, CancellationToken cancellationToken)
    {
        var player = await _context.Users.FindAsync(request.PlayerId);
        if (player == null) throw new NotFoundException(nameof(Player), request.PlayerId);

        var stats = new Domain.Stats
        {
            Player = player,
            IsWin = request.IsWin,
            EloDelta = request.EloDelta,
            Date = request.Date
        };

        _context.Stats.Add(stats);
        await _context.SaveChangesAsync(cancellationToken);

        return stats.Id;
    }
}