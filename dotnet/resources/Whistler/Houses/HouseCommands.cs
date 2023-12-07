using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Houses.Furnitures;
using Whistler.Possessions;
using Whistler.SDK;

namespace Whistler.Houses
{
    class HouseCommands : Script
    {
        [Command("cleargarages")]
        public static void CMD_ClearGarages(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "cleargarages")) return;

            var list = new List<int>();
            lock (GarageManager.Garages)
            {
                foreach (var g in GarageManager.Garages)
                {
                    if (g.Value.GarageHouse == null)
                        list.Add(g.Key);
                }
            }

            foreach (var id in list)
            {
                GarageManager.Garages.Remove(id);
                MySQL.Query("DELETE FROM `garages` WHERE `id` = @prop0", id);
            }
        }

        [Command("createhouse")]
        public static void CMD_CreateHouse(ExtPlayer player, int type, int price)
        {
            lock (HouseManager.Houses)
            {

                if (!Group.CanUseAdminCommand(player, "createhouse")) return;
                if (!Enum.IsDefined(typeof(HouseTypes), type))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_112", 3000);
                        return;
                    }
                House newHouse = new House(HouseManager.Houses.LastOrDefault()?.ID + 1 ?? 0, -1, (HouseTypes)type,
                    player.Position - new Vector3(0, 0, 1.12), price, false, 0, new List<Roommate>(), 0, new List<Furniture>(), OwnerType.Personal);

                HouseManager.Houses.Add(newHouse);
                Chat.SendTo(player, "House_113".Translate(newHouse.ID));
            }
        }

        [Command("removehouse")]
        public static void CMD_RemoveHouse(ExtPlayer player, int id)
        {
            if (!Group.CanUseAdminCommand(player, "removehouse")) return;

            House house = HouseManager.GetHouseById(id);
            if (house == null) return;

            if (GarageManager.Garages.ContainsKey(house.GarageID))
            {
                var garage = GarageManager.Garages[house.GarageID];
                garage.Destroy();
                GarageManager.Garages.Remove(house.GarageID);
                MySQL.Query("DELETE FROM garages WHERE id= @prop0", garage.ID);
            }

            house.Destroy();
            HouseManager.Houses.Remove(house);
            MySQL.Query("DELETE FROM houses WHERE id = @prop0", house.ID);
        }

        [Command("houseis")]
        public static void CMD_HouseIs(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "houseis")) return;
            if (!player.HasData("HOUSEID"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_114", 3000);
                return;
            }
            House house = HouseManager.GetHouseById(player.GetData<int>("HOUSEID"));
            if (house == null) return;

            Chat.SendTo(player, $"{player.GetData<int>("HOUSEID")}");
        }

        [Command("housenewprice")]
        public static void CMD_setHouseNewPrice(ExtPlayer player, int price)
        {
            if (!Group.CanUseAdminCommand(player, "housenewprice")) return;
            if (!player.HasData("HOUSEID"))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "House_114", 3000);
                return;
            }

            HouseManager.GetHouseById(player.GetData<int>("HOUSEID"))?.SetPrice(price);
        }


        [Command("hcam")]
        public static void CMD_hcam(ExtPlayer player, int houseid)
        {
            if (!Group.CanUseAdminCommand(player, "housenewprice")) return;

            HouseManager.GetHouseById(houseid)?.SetCam(player.Position);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Камера установлена", 3000);
        }
    }
}
