using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.MP.Arena.Locations;
using Whistler.MP.Arena.UI;
using Whistler.SDK;
using Chat = Whistler.Core.Chat;

namespace Whistler.MP.Arena.Racing
{
    public class RacingMap
    {
        public List<ArenaLocationSpawn> SpawnPoints { get; }

        public List<Vector3> Checkpoints { get; }

        public string Name { get; }

        public RacingName RacingName { get; set; }

        public string LocationName { get; set; }
        
        public string Description { get; set; }

        public uint VehicleHash { get; }

        public bool Started { get; set; }

        public Dictionary<ExtPlayer, Racer> Players => _players.ToDictionary(key => key.Player);

        public DateTime StartTime { get; set; }
        
        private int _totalContribution;
        private List<Racer> _players = new List<Racer>();
        private int _playersFinishedCount;
        private const double BestTimeRewardMultiplier = 1.3;
        private static uint _dimension = Dimensions.RequestPrivateDimension("race");
        
        public RacingMap(DateTime startTime, RacingName id, string name, uint vehicleHash, string description, string locationName, int circles, List<ArenaLocationSpawn> spawnPoints, List<Vector3> checkpoints)
        {
            RacingName = id;
            StartTime = startTime;
            Description = description;
            Name = name;
            LocationName = locationName;
            SpawnPoints = spawnPoints;
            Checkpoints = new List<Vector3>();

            for (var i = 0; i < circles; i++) Checkpoints.AddRange(checkpoints);
            
            VehicleHash = vehicleHash;
        }

        public void StartRegistration()
        {
            GameEventsEvents.Subscribers.ForEach(s => s.TriggerCefEvent("events/setCurrentEventId", (int) RacingName));
            Chat.AdminToAll("racing:1".Translate(Name, RacingSettings.RegistrationTime / 1000 / 60));
            Timers.StartOnce(RacingSettings.RegistrationTime, StartRace);
        }

        private void StartRace()
        {
            Started = true;
            RacingManager.Events.InsertFromQueue();
            
            _players.ForEach(p=>
            {
                p.Spawn(_players.IndexOf(p), _dimension);
                // Отправляем ID-шники врагов
                SafeTrigger.ClientEvent(p.Player,"arena:tracking:start", 
                    JsonConvert.SerializeObject(_players
                        .Where(e => e != p)
                        .Select(t => t.Player.Value)));
            });
        }

        public void RegisterPlayer(ExtPlayer player)
        {
            if (Players.ContainsKey(player)) return;
            var spawn = SpawnPoints.FirstOrDefault(s => s.IsFree);
            if (spawn == null)
            {
                Notify.SendError(player, "racing_0");
                return;
            }

            if (!Wallet.MoneySub(player.Character, RacingSettings.RegistrationPayment, "Money_Registration"))
            {
                Notify.SendError(player, "racing_1");
                return;
            }

            _totalContribution += RacingSettings.RegistrationPayment;
            
            spawn.IsFree = false;
            _players.Add(new Racer(player, VehicleHash, spawn, Checkpoints));
        }

        public void OnPlayerFinished(ExtPlayer player)
        {
            _playersFinishedCount++;
            
            var earnedMoneyShare = _playersFinishedCount switch
            {
                1 => 0.5,// 50% банка за 1 место
                2 => 0.3,
                3 => 0.2,
                _ => 0
            };
            var winning = Convert.ToInt32(_totalContribution * earnedMoneyShare);

            if (_playersFinishedCount == 1)// Если приехал первым
            {
                Chat.AdminToAll("racing:2".Translate(player.Name, Name));
                player.CreatePlayerAction(PersonalEvents.PlayerActions.WinRace, 1);
            }
            
            Notify.Send(player, NotifyType.Info, NotifyPosition.Top,
                earnedMoneyShare == 0
                    ? "events_15".Translate(_playersFinishedCount, winning)
                    : "events_16".Translate(_playersFinishedCount), 
                5000);
            
            if (RacingSettings.BestDailyTimes[RacingName] == 0 || Players[player].TotalSeconds < RacingSettings.BestDailyTimes[RacingName] && _playersFinishedCount == 1)
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.Top,
                    "events_17".Translate(Players[player].TotalSeconds),
                    5000);
                RacingSettings.BestDailyTimes[RacingName] = Players[player].TotalSeconds;
            }
            
            Wallet.MoneyAdd(player.Character, Convert.ToInt32(winning * BestTimeRewardMultiplier), "Призовые за участие в гоночных соревнованиях");

            if (_playersFinishedCount == Players.Count)
            {
                StopRace();
                RacingManager.CurrentMap = null;
            }
        }
        
        public void LeavePlayer(ExtPlayer player)
        {
            if (player == null) return;
            if (!Players.ContainsKey(player)) return;
            Players[player].Delete();
            _players.Remove(_players.FirstOrDefault(r => r.Player.Value == player.Value));
        }

        public void StopRace()
        {
            foreach (var (client, racer) in Players)
                racer.Delete();
            RacingManager.Events.AddEventToList(RacingName);
            Players.Clear();
        }
    }
}