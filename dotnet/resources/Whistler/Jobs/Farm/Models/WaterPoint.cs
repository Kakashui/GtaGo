using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Models;

namespace Whistler.Jobs.Farm.Models
{
    class WaterPoint
    {
        public Vector3 Position { get; set; }
        private InteractShape Shape { get; set; }
        private Blip Blip { get; set; }
        public WaterPoint(Vector3 position)
        {
            Position = position;
            CreateInteract();
            CreateBlip();
        }

        private void CreateInteract()
        {
            Shape = InteractShape.Create(Position, 2, 2)
                .AddDefaultMarker()
                .AddInteraction(Interact, "interact_42");
        }
        private void CreateBlip()
        {
            //Blip = NAPI.Blip.CreateBlip(615, Position, 1, 40, "Water", 255, 0, true, 0, 0);
        }

        private void Interact(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            var inventory = player.GetInventory();
            int countWatering = 0;
            foreach (var item in inventory.Items)
            {
                if (item is WateringCan)
                {
                    var watering = (WateringCan)item;
                    int needWater = watering.Config.MaxWater - watering.Water;
                    if (needWater == 0 || inventory.CurrentWeight + needWater > inventory.MaxWeight)
                        continue;
                    watering.PourWater();
                    inventory.UpdateItemData(watering.Index);
                    countWatering++;
                }
            }
            if (countWatering > 0)
            {
                player.CreatePlayerAction(PersonalEvents.PlayerActions.FilledTheWateringCan, countWatering);
                SDK.Notify.SendSuccess(player, "farmHouse_7".Translate(countWatering));
            }
            else
                SDK.Notify.SendError(player, "farmHouse_8");

        }

        public void Destroy()
        {
            Shape?.Destroy();
            Blip?.Delete();
        }
    }
}
