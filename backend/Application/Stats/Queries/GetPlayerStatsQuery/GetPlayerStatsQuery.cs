using MediatR;

namespace Application.Stats.Queries.GetPlayerStatsQuery;

public class GetPlayerStatsQuery : IRequest<List<StatsDto>>
{
    public string PlayerId { get; set; }
}