using Application.Common.Mappings;
using AutoMapper;
using Domain;

namespace Application.User.Queries.GetTopPlayersQuery;

public class PlayerDto : IMapWith<Player>
{
    public string Id { get; set; }
    public string NickName { get; set; }
    public int EloRating { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Player, PlayerDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.NickName, opt => opt.MapFrom(s => s.Nickname))
            .ForMember(d => d.EloRating, opt => opt.MapFrom(s => s.EloRating));
    }
}