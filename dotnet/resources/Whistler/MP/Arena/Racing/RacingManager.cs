using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MP.Arena.Locations;
using Whistler.SDK;

namespace Whistler.MP.Arena.Racing
{
    internal class RacingManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RacingManager));
        public static RacingMap CurrentMap { get; set; }
        public static GameEventsList Events { get; private set; }
        
        public RacingManager()
        {
            Events = new GameEventsList();
            Events.LoadMaps();
            Timers.Start(RacingSettings.CreationDelayTime, StartNextRace);
        }

        public static void StartNextRace()
        {
            CurrentMap?.StopRace();
            CurrentMap = Events.NextEvent();
            CurrentMap.StartRegistration();
        }
        
        [RemoteEvent("race:enteredCP")]
        private static void OnPlayerEnteredCheckpoint(ExtPlayer player)
        {
            try
            {
                CurrentMap.Players[player].SetNextCheckPoint();
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(OnPlayerEnteredCheckpoint)}: {ex}");
            }
        }
        
        [RemoteEvent("race:enteredFinish")]
        private static void OnPlayerFinished(ExtPlayer player, int raceTimeInSeconds)
        {
            try
            {
                var racer = CurrentMap.Players[player]; 
                racer.OnRacerFinished(raceTimeInSeconds);
                CurrentMap.OnPlayerFinished(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(OnPlayerFinished)}: {ex}");
            }
        }
        
        [RemoteEvent("race:openleaveDialog")]
        private static void OnPlayerLeaved(ExtPlayer player)
        {
            try
            {
                DialogUI.Open(player, "events_18", new List<DialogUI.ButtonSetting>
                {
                    new DialogUI.ButtonSetting
                    {
                        Name = "gui_727",// Да
                        Icon = "confirm",
                        Action = p => CurrentMap?.LeavePlayer(p)
                    },
                    new DialogUI.ButtonSetting
                    {
                        Name = "gui_728",// Нет
                        Icon = "cancel",
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Unhandled exception catched at {nameof(OnPlayerEnteredCheckpoint)}: {ex}");
            }
        }
        
        [ServerEvent(Event.PlayerDisconnected)]
        public void PlayerDisconnectedHandler(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                DeletePlayerFromCurrentMap(player);
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled error catched on {nameof(OnPlayerEnteredCheckpoint)} {e}");
            }
        }
        
        public static void OnPlayerDeadOnRace(ExtPlayer player)
        {
            try
            {
                DeletePlayerFromCurrentMap(player);
                player.ChangePosition(null);
                NAPI.Player.SpawnPlayer(player, player.Position);
                player.Health = 100;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled error catched on {nameof(OnPlayerDeadOnRace)} {e}");
            }
        }

        public static void DeletePlayerFromCurrentMap(ExtPlayer player)
        {
            if (CurrentMap == null) return;
            if (CurrentMap.Players.ContainsKey(player)) CurrentMap.LeavePlayer(player);
        }
    }
}