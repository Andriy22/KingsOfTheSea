using MediatR;

namespace Application.Stats.Commands.CreateStatsCommand;

public class CreateStatsCommand : IRequest<int>
{
    public string PlayerId { get; set; }
    public bool IsWin { get; set; }
    public int EloDelta { get; set; }
    public DateTime Date { get; set; }
}