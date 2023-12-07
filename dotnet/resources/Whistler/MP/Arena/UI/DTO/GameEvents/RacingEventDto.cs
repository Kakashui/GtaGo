using Newtonsoft.Json;

namespace Whistler.MP.Arena.UI.DTO
{
    public class RacingEventDto
    {
        /*eventId: 0,
        name: "Каскадерские гонки",
        locationName: "Парк на окраине",
        players: 200,
        maxPlayers: 400,
        startTime: '19:00',//время старта
        description: "Равным  начало повседневной работы по формированию позиции представляет собой интересный эксперимент проверки существенных финансовых и административных условий. Повседневная практика показывает, что реализация намеченных плановых заданий требуют определения и уточнения системы обучения кадров, соответствует насущным потребностям. отношении поставленных задач.",
        bestTime: '07:32',//лучшее время за день в сек
        registered: false, 
        countStartTime: '20 мин'*/
        [JsonProperty("eventId")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}