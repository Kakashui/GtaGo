using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.VehicleSystem.Models;
using Whistler.Core;

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
        public Action<ExtPlayer, ExtPlayer> CallbackWithPlayers { get; }

        [JsonIgnore]
        public Action<ExtPlayer, ExtVehicle> CallbackWithVehicles { get; }

        public InteractionMenuPageItem(string nameKey, string key, Action<ExtPlayer, ExtPlayer> callback)
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

        public InteractionMenuPageItem(string nameKey, string key, Action<ExtPlayer, ExtVehicle> callback)
        {
            NameKey = nameKey;
            Key = key;
            CallbackWithVehicles = callback;
            AllInteractionMenuPageItems.Add(key, this);
        }

        public InteractionMenuPageItem(string nameKey, string key, Action<ExtPlayer, ExtVehicle> callback, bool rename)
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

        public InteractionMenuPageItem(string nameKey, string key, InteractionMenuPage pageToRedirect, bool car)
        {
            NameKey = nameKey;
            Key = key;
            CallbackWithVehicles = (actingPlayer, actingVehicle) => pageToRedirect.OpenForPlayerWithVehicle(actingPlayer, actingVehicle);
            AllInteractionMenuPageItems.Add(key, this);
        }
    }
}
