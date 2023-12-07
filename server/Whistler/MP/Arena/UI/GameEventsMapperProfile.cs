using System;
using System.Globalization;
using AutoMapper;
using Newtonsoft.Json;
using Whistler.MP.Arena.Racing;
using Whistler.MP.Arena.UI.DTO;

namespace Whistler.MP.Arena.UI
{
    public class GameEventsMapperProfile : Profile
    {
        public GameEventsMapperProfile()
        {
            CreateMap<RacingMap, RacingEventDto>()
                .ForMember(dto => dto.Id, dto
                    => dto.MapFrom(source => (int) source.RacingName))
                .ForMember(dto => dto.Description, dto
                    => dto.MapFrom(source => source.Description))
                .ForMember(dto => dto.Name, dto
                    => dto.MapFrom(source => source.Name))
                .ForMember(dto => dto.MaxPlayers, dto
                    => dto.MapFrom(source => 15))
                .ForMember(dto => dto.LocationName, dto
                    => dto.MapFrom(source => source.LocationName));

            CreateMap<RacingMap, CurrentRacingDto>()
                .ForMember(dto => dto.Id, dto
                    => dto.MapFrom(source => (int) source.RacingName))
                .ForMember(dto => dto.StartTime, dto
                    => dto.MapFrom(source => source.StartTime.ToString("HH:mm")))
                .ForMember(dto => dto.CountStartTime, dto
                    => dto.MapFrom(source => Convert.ToInt32(source.StartTime.Subtract(DateTime.Now).TotalMinutes) + " min"))
                .ForMember(dto => dto.TotalPlayers, dto
                    => dto.MapFrom(source => source.Players.Count))
                .ForMember(dto => dto.BestTime, dto
                    => dto.MapFrom(source =>
                        $"{TimeSpan.FromSeconds(RacingSettings.BestDailyTimes[source.RacingName]).Minutes}:{TimeSpan.FromSeconds(RacingSettings.BestDailyTimes[source.RacingName]).Seconds}"));
        }
    }
}