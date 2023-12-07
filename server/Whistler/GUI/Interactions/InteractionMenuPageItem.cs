using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.VehicleSystem.Models;

namespace Whistler.GUI.Interactions
{
    internal class InteractionMenuPageItem
    {
        public static Dictionary<string, InteractionMenuPageItem> AllInteractionMenuPageItems =
            new Dictionary<string, InteractionMenuPageItem>();

        [JsonProperty("title")]
        public string NameKey { get; }

        [JsonProperty("key")]
        public string Key{ get; }

        [JsonIgnore]
        public Action<PlayerGo, PlayerGo> CallbackWithPlayers { get; }

        [JsonIgnore]
        public Action<PlayerGo, Vehicle> CallbackWithVehicles { get; }

        public InteractionMenuPageItem(string nameKey, string key, Action<PlayerGo, PlayerGo> callback)
        {
            NameKey = nameKey;
            Key = key;
            CallbackWithPlayers = callback;
            AllInteractionMenuPageItems.Add(key, this);
        }

        public InteractionMenuPageItem(string nameKey, string key)
        {
            NameKey = nameKey;
            Key = key;
            AllInteractionMenuPageItems.Add(key, this);
        }

        public InteractionMenuPageItem(string nameKey, string key, Action<PlayerGo, Vehicle> callback)
        {
            NameKey = nameKey;
            Key = key;
            CallbackWithVehicles = callback;
            AllInteractionMenuPageItems.Add(key, this);
        }

        public InteractionMenuPageItem(string nameKey, string key, InteractionMenuPage pageToRedirect)
        {
            NameKey = nameKey;
            Key = key;
            CallbackWithPlayers = (actingPlayer, targetPlayer) => pageToRedirect.OpenForPlayer(actingPlayer);
            AllInteractionMenuPageItems.Add(key, this);
        }
    }
}
