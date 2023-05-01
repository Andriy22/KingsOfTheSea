using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.User.Queries.GetTopPlayersQuery;

public class GetTopPlayersQueryHandler : IRequestHandler<GetTopPlayersQuery, List<PlayerDto>>
{
    private readonly IDataContext _context;
    private readonly IMapper _mapper;

    public GetTopPlayersQueryHandler(IDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PlayerDto>> Handle(GetTopPlayersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .OrderByDescending(x => x.EloRating)
            .Take(request.Limit).ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}