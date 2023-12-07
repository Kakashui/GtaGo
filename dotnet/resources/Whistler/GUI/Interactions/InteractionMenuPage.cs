using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;

namespace Whistler.GUI.Interactions
{
    internal class InteractionMenuPage
    {
        private readonly Dictionary<string, InteractionMenuPageItem> _items = new Dictionary<string, InteractionMenuPageItem>();
        private readonly Dictionary<string, Predicate<ExtPlayer>> _itemsPredicates = new Dictionary<string, Predicate<ExtPlayer>>();
        private readonly Dictionary<string, Predicate<ExtVehicle>> _itemVehiclePredicates = new Dictionary<string, Predicate<ExtVehicle>>();
        public IReadOnlyDictionary<string, InteractionMenuPageItem> Items => _items;

        public InteractionMenuPage AddItem(InteractionMenuPageItem item)
        {
            _items.Add(item.Key, item);
            return this;
        }

        public InteractionMenuPage AddItem(InteractionMenuPageItem item, Predicate<ExtPlayer> predicate)
        {
            _itemsPredicates.Add(item.Key, predicate);
            _items.Add(item.Key, item);
            return this;
        }

        public InteractionMenuPage AddItem(InteractionMenuPageItem item, Predicate<ExtVehicle> predicate)
        {
            _itemVehiclePredicates.Add(item.Key, predicate);
            _items.Add(item.Key, item);
            return this;
        }

        public void OpenForPlayer(ExtPlayer player)
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
            SafeTrigger.ClientEvent(player,"intMenu:open", JsonConvert.SerializeObject(itemsToShow));
        }
        public void OpenForPlayerWithVehicle(ExtPlayer player, ExtVehicle vehicle)
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
            SafeTrigger.ClientEvent(player,"intMenu:open", JsonConvert.SerializeObject(itemsToShow));
        }
    }
}
