using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using Whistler.Core.QuestPeds;
using Whistler.Fractions.Models;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.MoneySystem;
using Whistler.Common;
using Whistler.Inventory.Enums;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class Gangs : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Gangs));

        public static Dictionary<int, Vector3> EnterPoints = new Dictionary<int, Vector3>()
        {
            //{ 1, new Vector3(-223.069, -1616.918, 33.74932) },
            //{ 2, new Vector3(85.79318, -1958.851, 20.0017) },
            //{ 3, new Vector3(1408.579, -1486.897, 59.53736) },
            //{ 4, new Vector3(892.2407, -2172.888, 31.16626) },
            //{ 5, new Vector3(485.3334, -1528.692, 28.18008) },
        };
        public static Dictionary<int, Vector3> ExitPoints = new Dictionary<int, Vector3>()
        {
            //{ 1, new Vector3(-201.7147, -1627.962, -38.664788) },
            //{ 2, new Vector3(82.57095, -1958.607, -23.41236) },
            //{ 3, new Vector3(1420.487, -1497.264, -107.8639) },
            //{ 4, new Vector3(892.4592, -2168.068, 0.921189) },
           // { 5, new Vector3(484.9963, -1536.083, -33.22089) },
        };

        private static List<QuestPedParamModel> _armorPeds = new List<QuestPedParamModel>
        {
            new QuestPedParamModel(PedHash.G, new Vector3(-16.512, -1310.426, 29.26), "G", "frac_549", 13, 0, 2),
            new QuestPedParamModel(PedHash.LamarDavis, new Vector3(944.9, -1722, 30.57075), "Lamar Davis", "frac_549", 266, 0, 2),
            new QuestPedParamModel(PedHash.Claypain, new Vector3(480.9, -1531.9, 29.30612), "Clay Pain", "frac_549", 16, 0, 2),
            
        };
        private static List<QuestPed> _questPeds = new List<QuestPed>();

        private static int _narkoPrice = 1500;
        private static int _armorPrice = 2000;

        [ServerEvent(Event.ResourceStart)]
        public void Event_OnResourceStart()
        {
            try
            {
                foreach (var point in EnterPoints)
                {
                    var fraction = point.Key;

                    InteractShape.Create(point.Value, 1.2f, 2)
                        .AddDefaultMarker()
                        .AddInteraction((player) =>
                        {
                            Manager.enterInterier(player, fraction);
                        }, "interact_28");
                }

                foreach (var point in ExitPoints)
                {
                    var fraction = point.Key;

                    InteractShape.Create(point.Value, 1.2f, 2)
                        .AddDefaultMarker()
                        .AddInteraction((player) =>
                        {
                            Manager.exitInterier(player, fraction);
                        }, "interact_29");
                }
                foreach (var pedConf in _armorPeds)
                {
                    var ped = new QuestPed(pedConf);
                    ped.PlayerInteracted += Ped_PlayerInteracted;
                    _questPeds.Add(ped);
                }
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        private void Ped_PlayerInteracted(ExtPlayer player, QuestPed ped)
        {
            var fraction = player.GetFraction();
            DialogPage startPage;

            switch (fraction?.OrgActiveType ?? OrgActivityType.Invalid)
            {
                case OrgActivityType.Crime:
                    startPage = new DialogPage("frac_550",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("frac_551", GiveArmor)
                        .AddAnswer("frac_552", GiveNarko);
                    break;
                case OrgActivityType.Invalid:
                case OrgActivityType.Mafia:
                case OrgActivityType.Government:
                default:
                    startPage = new DialogPage("frac_553",
                        ped.Name,
                        "frac_557")
                        .AddCloseAnswer("frac_554");
                    break;
            }

            startPage.OpenForPlayer(player);
        }

        private static void GiveArmor(ExtPlayer player)
        {
            if (!Wallet.MoneySub(player.Character, _armorPrice, "Money_PedArmor"))
            {
                Notify.SendError(player, "frac_556");
                return;
            }
            var armor = ItemsFabric.CreateClothes(ItemNames.BodyArmor, true, 0, 0, false);
            if (armor != null)
            {
                armor.Armor = 50;
                player.GetInventory()?.AddItem(armor);
            }
        }

        private static void GiveNarko(ExtPlayer player)
        {
            if (!Wallet.MoneySub(player.Character, _narkoPrice, "Money_PedNarko"))
            {
                Notify.SendError(player, "frac_556");
                return;
            }
            var narko = ItemsFabric.CreateNarcotic(ItemNames.Marijuana, 5, false);
            if (narko != null)
            {
                player.GetInventory()?.AddItem(narko);
            }
        }
    }
}
