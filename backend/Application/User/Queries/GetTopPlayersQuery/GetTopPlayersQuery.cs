using MediatR;

namespace Application.User.Queries.GetTopPlayersQuery;

public class GetTopPlayersQuery : IRequest<List<PlayerDto>>
{
    public int Limit { get; set; }
}