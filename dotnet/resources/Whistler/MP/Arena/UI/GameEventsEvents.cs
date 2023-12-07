using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MP.Arena.Racing;
using Whistler.MP.Arena.UI.DTO;

namespace Whistler.MP.Arena.UI
{
    internal class GameEventsEvents : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(GameEventsEvents));
        public static List<ExtPlayer> Subscribers = new List<ExtPlayer>();
        
        public GameEventsEvents()
        {
            Main.OnPlayerReady += SendDefaultRacingData;
            Main.PlayerPreDisconnect += p =>
            {
                if (Subscribers.Contains(p))
                    Subscribers.Remove(p);
            };
        }

        private static void SendDefaultRacingData(ExtPlayer player)
        {
            var mapper = MapperManager.Get();
            
            player.TriggerCefEvent("events/setEvents", 
                JsonConvert.SerializeObject(mapper.Map<IEnumerable<RacingEventDto>>(RacingManager.Events.Events.ToList())));
        }

        [RemoteEvent("ge:close")]
        public void OnEventsMenuClosed(ExtPlayer player)
        {
            try
            {
                if (Subscribers.Contains(player)) Subscribers.Remove(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception at {nameof(OnEventsMenuClosed)}: {ex}");
            }
        }
        
        [RemoteEvent("ge:reg")]
        public void OnPlayerRegisteredOnEvent(ExtPlayer player)
        {
            try
            {
                DialogUI.Open(player, "racing_2".Translate(RacingSettings.RegistrationPayment), 
                    new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "gui_727",
                        Icon = "confirm",
                        Action = RegisterAndUpdateHud
                    },
                    new DialogUI.ButtonSetting
                    {
                        Name = "gui_728",
                        Icon = "cancel",
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception at {nameof(OnPlayerRegisteredOnEvent)}: {ex}");
            }
        }

        private static void RegisterAndUpdateHud(ExtPlayer player)
        {
            RacingManager.CurrentMap.RegisterPlayer(player);

            player.TriggerCefEvent("events/setRegistered", JsonConvert.SerializeObject(new
            {
                id = (int) RacingManager.CurrentMap.RacingName,
                registered = true
            }));
                
            Subscribers.ForEach(s => s.TriggerCefEvent("events/setPlayersCount", 
                JsonConvert.SerializeObject(new
                {
                    id = (int) RacingManager.CurrentMap.RacingName,
                    players = RacingManager.CurrentMap.Players.Count
                })));
        }
        
        [RemoteEvent("ge:unreg")]
        public void OnPlayerUnregisteredOnEvent(ExtPlayer player)
        {
            try
            {
                RacingManager.CurrentMap.LeavePlayer(player);

                player.TriggerCefEvent("events/setRegistered", JsonConvert.SerializeObject(new
                {
                    id = (int) RacingManager.CurrentMap.RacingName,
                    registered = false
                }));
                
                Subscribers.ForEach(s => s.TriggerCefEvent("events/setPlayersCount", 
                    JsonConvert.SerializeObject(new
                    {
                        id = (int) RacingManager.CurrentMap.RacingName,
                        players = RacingManager.CurrentMap.Players.Count
                    })));
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception at {nameof(OnPlayerUnregisteredOnEvent)}: {ex}");
            }
        }
        
        [RemoteEvent("exitrace")]
        public void OnPlayerExitRace(ExtPlayer player)
        {
            try
            {
                RacingManager.CurrentMap.LeavePlayer(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Exception at {nameof(OnPlayerExitRace)}: {ex}");
            }
        }
    }
}