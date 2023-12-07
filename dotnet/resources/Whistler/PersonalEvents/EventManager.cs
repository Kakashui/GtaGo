using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Families.FamilyMenu;
using Whistler.Families.Models;
using Whistler.Helpers;
using Whistler.PersonalEvents.Achievements;
using Whistler.PersonalEvents.Contracts;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.PersonalEvents.DTO;
using Whistler.PersonalEvents.Models;
using Whistler.SDK;

namespace Whistler.PersonalEvents
{
    class EventManager : Script
    {
        private static readonly Dictionary<PlayerActions, Action<ExtPlayer, int>> _subscribes = new Dictionary<PlayerActions, Action<ExtPlayer, int>>();
        public EventManager()
        {
            Main.OnPlayerReady += OnPlayerReady;
            FamilyMenuManager.FamilyLoad += FamilyLoad;
        }

        public static EventModel EventModelCreateOrLoad(int uuid)
        {
            var result = MySQL.QueryRead("SELECT * FROM `achievements` WHERE `uuid` = @prop0", uuid);
            if (result == null || result.Rows.Count == 0)
                return new EventModel(new Dictionary<AchieveNames, Achievements.AchieveModels.AchieveProgress>(), ContractLoadOrCreate(uuid, ContractTypes.Single));
            else
                return new EventModel(result.Rows, ContractLoadOrCreate(uuid, ContractTypes.Single));
        }

        public static ContractInfo ContractLoadOrCreate(int id, ContractTypes contractType)
        {
            var result = MySQL.QueryRead("SELECT * FROM `contracts` WHERE `ownerid` = @prop0 AND `ownerType` = @prop1", id, (int)contractType);
            if (result == null || result.Rows.Count == 0)
                return new ContractInfo();
            else
                return new ContractInfo(result.Rows);
        }

        private void FamilyLoad(ExtPlayer player, Family family)
        {
            SafeTrigger.ClientEvent(player,"personalEvents:loadFamilyContracts", family.FamilyContracts.GetSerializeDTOString());
        }

        private void OnPlayerReady(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"personalEvents:load", player.Character.EventModel.GetSerializeDTOString());
            SafeTrigger.ClientEvent(player,"personalEvents:loadMyContracts", player.Character.EventModel.Contracts.GetSerializeDTOString());
        }

        public static void AddEvent(PlayerActions action, Action<ExtPlayer, int> handler)
        {
            if (!_subscribes.ContainsKey(action))
                _subscribes.Add(action, handler);
            else
                _subscribes[action] += handler;
        }
        public static void RemoveEvent(PlayerActions action, Action<ExtPlayer, int> handler)
        {
            if (_subscribes.TryGetValue(action, out Action<ExtPlayer, int> d))
            {
                if (d != null)
                    d -= handler;
            }
        }

        public static void InvokeEvent(PlayerActions action, ExtPlayer player, int progress)
        {
            _subscribes.TryGetValue(action, out Action<ExtPlayer, int> d);
            d?.Invoke(player, progress);
        }

        [Command("testevent")]
        public static void CreatePlayerAction(ExtPlayer player, int eventName, int progress)
        {
            player.CreatePlayerAction((PlayerActions)eventName, progress);
        }

        [RemoteEvent("personalEvents:syncClientEvents")]
        public static void SyncClientEvents(ExtPlayer player, string eventName, int points)
        {
            if (Enum.TryParse(eventName, true, out PlayerActions actionName))
            {
                player.CreatePlayerAction(actionName, points);
            }
        }

        [RemoteEvent("personalEvents:acceptReward")]
        public static void AcceptReward(ExtPlayer player, int eventName)
        {
            AchieveSettings.AchievesSettings.GetValueOrDefault((AchieveNames)eventName)?.GiveReward(player);
        }

        [RemoteEvent("personalEvents:acceptContract")]
        public static void AcceptContract(ExtPlayer player, int contractName)
        {
            ContractSettings.ContractsSettings.GetValueOrDefault((ContractNames)contractName).AcceptContract(player);
        }
    }
}
