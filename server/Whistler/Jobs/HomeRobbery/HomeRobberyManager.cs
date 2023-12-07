using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.Jobs.HomeRobbery.Models;
using Whistler.SDK;

namespace Whistler.Jobs.HomeRobbery
{
    class HomeRobberyManager : Script
    {
        private static Random _rnd = new Random();
        private static int _profitInOneItem = 3000;
        private const int HomeRobberyWorkID = 20;
        private static Vector3 _endPosition = new Vector3(-127.2, 2793, 52.207754);
        public static QuestPedParamModel WorkPed = new QuestPedParamModel(PedHash.Chip, new Vector3(-110.98062, 2810.8464, 53.15873), "Jason", "homeRobbery_1", 160, 0, 2);

        public HomeRobberyManager()
        {
            NAPI.Blip.CreateBlip(763, WorkPed.Position, 1, 6, Main.StringToU16("Ограбление домов"), 255, 0, true, 0, 0);
            var ped = new QuestPed(WorkPed);
            ped.PlayerInteracted += WorkPed_PlayerInteracted;
            InteractShape.Create(_endPosition, 3, 2, 0)
                .AddMarker(27, _endPosition, 3, InteractShape.DefaultMarkerColor)
                .AddInteraction(SellRobberyItems, "interact_58", Key.VK_E);
        }

        private static void SellRobberyItems(PlayerGo player)
        {
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "options_program_27");
                return;
            }
            var vehGo = player.Vehicle.GetVehicleGo();

            int itemCount = vehGo.Data.GetRobberyAbstractItem(int.MaxValue);
            if (itemCount > 0)
            {
                MoneySystem.Wallet.MoneyAdd(player.Character, itemCount * _profitInOneItem, "Money_SellRobberyItem");
                Notify.SendSuccess(player, "home:robbery:1".Translate(itemCount, itemCount * _profitInOneItem));
                DeletePrevBlipAndMarker(player);
                EndWork(player);
            }
            else
                Notify.SendSuccess(player, "home:robbery:2");
        }

        private void WorkPed_PlayerInteracted(PlayerGo player, QuestPed ped)
        {
            var level = player.Character.QuestStage;
            DialogPage startPage;
            if ((player.GetFraction()?.OrgActiveType ?? Common.OrgActivityType.Invalid) == Common.OrgActivityType.Government || (player.GetFamily()?.OrgActiveType ?? Common.OrgActivityType.Invalid) == Common.OrgActivityType.Government)
            {
                startPage = new DialogPage("home:robbery:17",
                    ped.Name,
                    "home:robbery:18")
                    .AddCloseAnswer("home:robbery:9");
            }
            else
            {
                switch (player.Character.WorkID)
                {
                    case 0:
                        if (player.Character.HouseTarget >= 0)
                        {
                            DeletePrevBlipAndMarker(player);
                            EndWork(player);
                        }
                        var nextPage = new DialogPage("home:robbery:11",
                            ped.Name,
                            ped.Role)
                            .AddAnswer("home:robbery:5", WorkPed_CallBack)
                            .AddCloseAnswer("home:robbery:3");
                        startPage = new DialogPage("home:robbery:4",
                            ped.Name,
                            ped.Role)
                            .AddAnswer("home:robbery:10", nextPage)
                            .AddAnswer("home:robbery:5", WorkPed_CallBack)
                            .AddCloseAnswer("home:robbery:3");
                        break;
                    case HomeRobberyWorkID:
                        if (player.Character.HouseTarget < 0)
                        {
                            nextPage = new DialogPage("home:robbery:11",
                                ped.Name,
                                ped.Role)
                                .AddAnswer("home:robbery:5", WorkPed_CallBack)
                                .AddCloseAnswer("home:robbery:3");
                            startPage = new DialogPage("home:robbery:4",
                                ped.Name,
                                ped.Role)
                                .AddAnswer("home:robbery:10", nextPage)
                                .AddAnswer("home:robbery:5", WorkPed_CallBack)
                                .AddCloseAnswer("home:robbery:3");
                        }
                        else
                        {
                            startPage = new DialogPage("home:robbery:6",
                                ped.Name,
                                ped.Role)
                                .AddAnswer("home:robbery:7", EndWork)
                                .AddCloseAnswer("home:robbery:3");
                        }
                        break;
                    default:
                        startPage = new DialogPage("home:robbery:8",
                            ped.Name,
                            ped.Role)
                            .AddCloseAnswer("home:robbery:9");
                        break;
                }
            }
            startPage.OpenForPlayer(player);
        }
        private void WorkPed_CallBack(PlayerGo player)
        {
            if (player.Character.WorkID != 0 && player.Character.WorkID != HomeRobberyWorkID)
            {
                Notify.SendError(player, "Jobs_64");
                return;
            }
            if (player.Character.HouseTarget >= 0)
            {
                return;
            }
            GetNextHome(player, player.Position);
        }

        private static void EndWork(PlayerGo player)
        {
            player.Character.WorkID = 0;
            player.Character.HouseTarget = -1;
        }

        public static void GiveContainer(PlayerGo player)
        {
            RobberyItem item = new RobberyItem(PersonalEvents.Contracts.AbstractItemNames.Robbery, 1);
            player.GiveContainerToPlayer(item, AttachId.RobberyBox);

            CallPolice(HouseManager.GetHouseById(player.Character.HouseTarget), false);
            if (_rnd.NextDouble() < 0.33)
            {
                Notify.SendSuccess(player, "home:robbery:12");
                DeletePrevBlipAndMarker(player);
                GetNextHome(player, HouseManager.GetHouseById(player.Character.HouseTarget)?.Position ?? player.Position);
            }
        }

        private static void DeletePrevBlipAndMarker(PlayerGo player)
        {
            player.DeleteClientMarker(900);
            player.DeleteClientBlip(900);
        }
        private static void GetNextHome(PlayerGo player, Vector3 myPosition)
        {
            var house = GetNextRobberyHouse(player, myPosition);
            player.Character.HouseTarget = house.ID;
            player.CreateClientBlip(900, 1, "Target", house.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateWaypoint(house.Position);
            player.CreateClientMarker(900, 0, house.HouseGarage.GarageConfig.CoordsModel.RobberyPos + new Vector3(0, 0, 1.5), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
        }

        private static House GetNextRobberyHouse(PlayerGo player, Vector3 myPosition)
        {
            return HouseManager.Houses.Where(item => item.HouseGarage != null && item.Position.DistanceTo2D(myPosition) > 2000).GetRandomElement();
        }
        public static bool CheckGiveContainer(PlayerGo player)
        {
            return player.Character.HouseTarget > 0 && (GarageManager.Garages.GetValueOrDefault(player.Character.InsideGarageID)?.GarageHouse?.ID ?? -2) == player.Character.HouseTarget;
        }

        public static void CallPolice(House house, bool breaking)
        {
            if (breaking)
                Chat.SendFractionMessage(7, (p) => "home:robbery:13".Translate(house.ID, (int)p.Position.DistanceTo(house.Position), house.ID), true);
            else
                Chat.SendFractionMessage(7, (p) => "home:robbery:14".Translate(house.ID, (int)p.Position.DistanceTo(house.Position), house.ID), true);
        }
        [Command("homerob")]
        public static void GetCall(PlayerGo player, int houseID)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.FractionID != 7)
            {
                Notify.SendError(player, "home:robbery:15");
                return;
            }
            House house = HouseManager.GetHouseById(houseID);
            if (house == null)
            {
                Notify.SendError(player, "home:robbery:16");
                return;
            }
            player.CreateWaypoint(house.Position);
        }
    }
}
