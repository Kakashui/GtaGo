using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;

namespace Whistler.GUI.Interactions
{
    internal class InteractionMenuPage
    {
        private readonly Dictionary<string, InteractionMenuPageItem> _items = new Dictionary<string, InteractionMenuPageItem>();
        private readonly Dictionary<string, Predicate<PlayerGo>> _itemsPredicates = new Dictionary<string, Predicate<PlayerGo>>();
        private readonly Dictionary<string, Predicate<Vehicle>> _itemVehiclePredicates = new Dictionary<string, Predicate<Vehicle>>();
        public IReadOnlyDictionary<string, InteractionMenuPageItem> Items => _items;

        public InteractionMenuPage AddItem(InteractionMenuPageItem item)
        {
            _items.Add(item.Key, item);
            return this;
        }

        public InteractionMenuPage AddItem(InteractionMenuPageItem item, Predicate<PlayerGo> predicate)
        {
            _itemsPredicates.Add(item.Key, predicate);
            _items.Add(item.Key, item);
            return this;
        }

        public InteractionMenuPage AddItem(InteractionMenuPageItem item, Predicate<Vehicle> predicate)
        {
            _itemVehiclePredicates.Add(item.Key, predicate);
            _items.Add(item.Key, item);
            return this;
        }

        public void OpenForPlayer(PlayerGo player)
        {
            var itemsToShow = new List<InteractionMenuPageItem>();
            foreach (var (key, page) in _items)
            {
                if (_itemsPredicates.TryGetValue(key, out var predicate))
                {
                    if (predicate(player)) itemsToShow.Add(page);
                }
                else itemsToShow.Add(page);
            }
            player.TriggerEvent("intMenu:open", JsonConvert.SerializeObject(itemsToShow));
        }
        public void OpenForPlayerWithVehicle(PlayerGo player, Vehicle vehicle)
        {
            var itemsToShow = new List<InteractionMenuPageItem>();
            foreach (var (key, page) in _items)
            {
                if (_itemVehiclePredicates.TryGetValue(key, out var predicateVehicle) && vehicle != null)
                {
                    if (predicateVehicle(vehicle)) itemsToShow.Add(page);
                }
                else if (_itemsPredicates.TryGetValue(key, out var predicate))
                {
                    if (predicate(player)) itemsToShow.Add(page);
                }
                else itemsToShow.Add(page);
            }
            player.TriggerEvent("intMenu:open", JsonConvert.SerializeObject(itemsToShow));
        }
    }
}
