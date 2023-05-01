using Application.Common.Mappings;
using AutoMapper;

namespace Application.Stats.Queries.GetPlayerStatsQuery;

public class StatsDto : IMapWith<Domain.Stats>
{
    public int Id { get; set; }
    public string PlayerId { get; set; }
    public string PlayerName { get; set; }
    public bool IsWin { get; set; }
    public int EloDelta { get; set; }
    public string Date { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Stats, StatsDto>()
            .ForMember(d => d.PlayerId, opt => opt.MapFrom(s => s.Player.Id))
            .ForMember(d => d.PlayerName, opt => opt.MapFrom(s => s.Player.Nickname))
            .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date.ToString("dd/MM/yyyy HH:mm:ss")));
    }
}