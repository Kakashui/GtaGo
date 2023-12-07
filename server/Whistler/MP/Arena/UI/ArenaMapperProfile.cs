using System.Linq;
using AutoMapper;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Helpers;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Lobbies;
using Whistler.MP.Arena.UI.DTO;

namespace Whistler.MP.Arena.UI
{
    public class ArenaMapperProfile : Profile
    {
        public ArenaMapperProfile()
        {
            CreateMap<StrikeLobby, StrikeLobbyDTO>()
                .ForMember(dto => dto.Id, dto
                    => dto.MapFrom(source => source.Id))
                .ForMember(dto => dto.Title, dto
                    => dto.MapFrom(source => source.Settings.LocationName.ToString()))
                .ForMember(dto => dto.Type, dto
                    => dto.MapFrom(source => ArenaLobbyHelper.GetModeName(source.Settings.BattleMode)))
                .ForMember(dto => dto.EntryBet, dto
                    => dto.MapFrom(source => source.Settings.EntryBet))
                .ForMember(dto => dto.IsStarted, dto
                    => dto.MapFrom(source => source.CurrentState == LobbyState.InGame))
                .ForMember(dto => dto.MaxPlayers, dto
                    => dto.MapFrom(source => source.Settings.MaxPlayers))
                .ForMember(dto => dto.OwnerName, dto
                    => dto.MapFrom(source => source.Leader.Player.Name))
                .ForMember(dto => dto.TotalRounds, dto
                    => dto.MapFrom(source => source.Settings.TotalRounds))
                .ForMember(dto => dto.WeaponName, dto
                    => dto.MapFrom(source => source.Settings.AvailableWeapon))
                .ForMember(dto => dto.IsMapChanging, dto
                    => dto.MapFrom(source => source.CurrentState == LobbyState.MapChanging))
                .ForMember(dto => dto.GreenTeamPlayerNames, dto
                    => dto.MapFrom(source => source.Members.Where(m => m.Team == TeamName.Green)
                        .Select(s => s.Player.Name)))
                .ForMember(dto => dto.RedTeamPlayerNames, dto
                    => dto.MapFrom(source => source.Members.Where(m => m.Team == TeamName.Red)
                        .Select(s => s.Player.Name)));

            CreateMap<IArenaLobbyMember, LobbyMemberDTO>()
                .ForMember(dto => dto.TeamName, dto
                    => dto.MapFrom(source => source.Team.ToString().ToUpper()))
                .ForMember(dto => dto.LobbyId, dto
                    => dto.MapFrom(source => source.CurrentLobby.Id))
                .ForMember(dto => dto.PlayerName, dto
                    => dto.MapFrom(source => source.Player.Name));

            CreateMap<IBattleMember, BattleMemberDTO>()
                .ForMember(dto => dto.Id, dto
                    => dto.MapFrom(source => source.Player.Value))
                .ForMember(dto => dto.Deaths, dto
                    => dto.MapFrom(source => source.Deaths))
                .ForMember(dto => dto.Kills, dto
                    => dto.MapFrom(source => source.Kills))
                .ForMember(dto => dto.Name, dto
                    => dto.MapFrom(source => source.Player.Name));

            CreateMap<RatingModel, ArenaRatingItemDTO>()
                .ForMember(dto => dto.Name, dto
                    => dto.MapFrom(source => source.FullName))
                .ForMember(dto => dto.Place, dto
                    => dto.MapFrom(source => ArenaRatingManager.Ratings.IndexOf(source) + 1))
                .ForMember(dto => dto.TotalPoints, dto
                    => dto.MapFrom(source => source.Points));
        }
    }
}