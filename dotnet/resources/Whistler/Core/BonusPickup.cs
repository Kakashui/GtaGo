using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;
using Object = GTANetworkAPI.Object;

namespace Whistler.Core
{
    public class BonusPickup
    {
        public static Dictionary<int, (ColShape, Object, Marker)> Pickups = new Dictionary<int, (ColShape, Object, Marker)>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BonusPickup));
        public static int Counter;

        public static void Create(Vector3 position, int money)
        {
            var obj = NAPI.Object.CreateObject(1452661060, position, new Vector3());
            var shape = NAPI.ColShape.CreatCircleColShape(position.X, position.Y, 2);
            var marker = NAPI.Marker.CreateMarker(29, position + new Vector3(0, 0, 1), new Vector3(), new Vector3(),
                1.5f, InteractShape.DefaultMarkerColor);
            var id = Counter;
            Pickups.Add(id, (shape, obj, marker));
            Counter++;
            NAPI.Task.Run(() =>
            {
                shape.OnEntityEnterColShape += (s, p) =>
                {
                    try
                    {
                        if (!(p is ExtPlayer player)) return;

                        Notify.Send(player, NotifyType.Success, NotifyPosition.Top, $"+{money}$", 3000);
                        Wallet.MoneyAdd(player.Character, money, "Кейс с бонусом");
                        Entered(player, id);
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteError($"Bonus Pickup entered exception: {ex}");
                    }
                };
            }, 5000);
        }

        private static void Entered(ExtPlayer player, int id)
        {
            if (!Pickups.ContainsKey(id)) return;
            var (interactShape, obj, marker) = Pickups[id];
            interactShape.Delete();
            marker.Delete();
            obj.Delete();
            Pickups.Remove(id);
        }
    }
}