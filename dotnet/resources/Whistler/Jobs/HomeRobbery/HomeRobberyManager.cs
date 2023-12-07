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
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.Jobs.HomeRobbery.Models;
using Whistler.SDK;
using Whistler.VehicleSystem.Models;

namespace Whistler.Jobs.HomeRobbery
{
    class HomeRobberyManager : Script
    {
        private static Random _rnd = new Random();
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

        private static void SellRobberyItems(ExtPlayer player)
        {
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "options_program_27");
                return;
            }
            ExtVehicle extVehicle = player.Vehicle as ExtVehicle;

            int itemCount = extVehicle.Data.GetRobberyAbstractItem(int.MaxValue);
            if (itemCount > 0)
            {
                int price = GetPriceForItems(itemCount);
                MoneySystem.Wallet.MoneyAdd(player.Character, price, $"Продажа награбленных вещей ({itemCount})");
                Notify.SendSuccess(player, "home:robbery:1".Translate(itemCount, price));
                DeletePrevBlipAndMarker(player);
                EndWork(player);
                player.Session.NextRobbery = DateTime.Now;
            }
            else
                Notify.SendSuccess(player, "home:robbery:2");
        }

        private static int GetPriceForItems(int count)
        {
            if (count <= 0) return 0;

            int totalPrice = 0;
            for (int i = 0; i < count; i++) totalPrice += _rnd.Next(2500, 5001);
            return totalPrice;
        }

        private void WorkPed_PlayerInteracted(ExtPlayer player, QuestPed ped)
        {
            var level = player.Character.QuestStage;
            DialogPage startPage;
            Fraction fraction = player.GetFraction();
            if (fraction == null || fraction.OrgActiveType != Common.OrgActivityType.Crime)
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
                        if (player.Character.HouseTarget >= 0) return;

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
                        if (player.Character.HouseTarget == -1)
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
                                .AddAnswer("Пока нет, хочу закончить, попробую в другой раз", EndWork)
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
        private void WorkPed_CallBack(ExtPlayer player)
        {
            DateTime now = DateTime.Now;
            if (player.Character.WorkID != 0 && player.Character.WorkID != HomeRobberyWorkID)
            {
                Notify.SendError(player, "Jobs_64");
                return;
            }
            if (player.Session.NextRobbery > now)
            {
                TimeSpan expiredIn = player.Session.NextRobbery - now;
                Notify.SendError(player, $"Вы недавно уже брали задание на ограбление, попробуйте еще раз через {expiredIn.Minutes}:{expiredIn.Seconds}.", 7000);
                return;
            }
            if (player.Character.HouseTarget >= 0)
            {
                Notify.SendError(player, $"У Вас всё еще есть задание. Сперва закончите его.", 7000);
                return;
            }

            player.Session.NextRobbery = now.AddMinutes(5);
            player.Character.WorkID = HomeRobberyWorkID;
            GetNextHome(player, player.Position);
        }

        private static void EndWork(ExtPlayer player)
        {
            player.Character.WorkID = 0;
            player.Character.HouseTarget = -1;
            //MainMenu.SendStats(player);
        }

        public static void GiveContainer(ExtPlayer player)
        {
            House house = HouseManager.GetHouseById(player.Character.HouseTarget);
            if (house == null) return;

            if (house.RobberyItemsCount == 0)
            {
                Notify.SendAlert(player, "В этом доме уже нечего красть, отправляйтесь в следующий или сдайте накраденное.");
                DeletePrevBlipAndMarker(player);
                GetNextHome(player, house.Position ?? player.Position);
                return;
            }

            house.RobberyItemsCount--;
            RobberyItem item = new RobberyItem(PersonalEvents.Contracts.AbstractItemNames.Robbery, 1);
            player.GiveContainerToPlayer(item, AttachId.RobberyBox);
            CallPolice(house, false);
            if (house.RobberyItemsCount != 0) return;

            Notify.SendAlert(player, "В этом доме больше нечего красть, отправляйтесь в следующий или сдайте накраденное.");
            DeletePrevBlipAndMarker(player);
            GetNextHome(player, house.Position ?? player.Position);
        }

        private static void DeletePrevBlipAndMarker(ExtPlayer player)
        {
            player.DeleteClientMarker(900);
            player.DeleteClientBlip(900);
        }
        private static void GetNextHome(ExtPlayer player, Vector3 myPosition)
        {
            House house = GetNextRobberyHouse(player, myPosition);
            if (house == null)
            {
                EndWork(player);
                Notify.SendAlert(player, "В данный момент нет домов для ограбления, попробуйте позже.");
                return;
            }
            player.Character.HouseTarget = house.ID;
            player.CreateClientBlip(900, 1, "Цель", house.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateWaypoint(house.Position);
            player.CreateClientMarker(900, 0, house.HouseGarage.GarageConfig.CoordsModel.RobberyPos + new Vector3(0, 0, 1.5), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
        }

        private static House GetNextRobberyHouse(ExtPlayer player, Vector3 myPosition)
        {
            IEnumerable<House> houses = HouseManager.Houses.Where(item => item.HouseGarage != null && item.RobberyItemsCount > 0 && item.Position.DistanceTo2D(myPosition) > 2000);
            if (!houses.Any()) return null;

            return houses.GetRandomElement();
        }
        public static bool CheckGiveContainer(ExtPlayer player)
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
        public static void GetCall(ExtPlayer player, int houseID)
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
