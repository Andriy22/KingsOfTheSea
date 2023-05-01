using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Stats.Queries.GetPlayerStatsQuery;

public class GetPlayerStatsQueryHandler : IRequestHandler<GetPlayerStatsQuery, List<StatsDto>>
{
    private readonly IDataContext _context;
    private readonly IMapper _mapper;

    public GetPlayerStatsQueryHandler(IDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<StatsDto>> Handle(GetPlayerStatsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Stats
            .Include(x => x.Player)
            .Where(x => x.Player.Id == request.PlayerId).OrderByDescending(x => x.Date).ProjectTo<StatsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}