using System;
using System.Collections.Generic;
using System.Data;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using Whistler.Core.Character;
using Newtonsoft.Json;
using Whistler.ClothesCustom;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem;
using System.Linq;
using Whistler.Helpers;
using Whistler.Fractions.Models;
using Whistler.Fractions.PDA;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Inventory.Enums;

namespace Whistler.Fractions
{
    class Police : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Police));

        public static QuestPedParamModel policePed = new QuestPedParamModel(PedHash.Cop01SFY, new Vector3(612.3222, 13.368966, 82.74041), "Claire Williams", "Сотрудник Полиции", -107, 0, 4);


        private static Dictionary<int, ColShape> Cols = new Dictionary<int, ColShape>();
        public static List<Vector3> policeCheckpoints = new List<Vector3>()
        {
            new Vector3(564.3243, 10.62936, 81.74), // shape, where player can arrest suspect       0
            new Vector3(619.8245, 9.741503, 81.74), // blip     1
            new Vector3(593.828, -13.14343, 81.74), // dressing room     2
            new Vector3(452.2765, -993.3061, 29.68959), // special checkpoint     3
            new Vector3(567.4738, 18.01583, 82.9), // prison room      4 
            new Vector3(562.7616, 8.54627, 82.9), // place of release from prison     5
            new Vector3(441.9336, -981.5965, 29.6896), // buy gun licence     6
            new Vector3(439.8187, -981.6356, 29.5696), // surrender bags with drill and money     7
            new Vector3(480.6758, -990.0038, 23.79472),  // open stock     8
            new Vector3(1711.0012, 2581.5393, 45.588715),  // prison shape  9
            new Vector3(1670.4656, 2581.5393, 45.588715),  // prison shape  10
        };

        public static List<ItemTypes> IllegalItems = new List<ItemTypes>()
        {
            ItemTypes.Narcotic,
        };

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                //NAPI.World.DeleteWorldProp(NAPI.Util.GetHashKey("v_ilev_arm_secdoor"), new Vector3(453.0793, -983.1894, 30.83926), 30f);

                Cols.Add(0, NAPI.ColShape.CreateCylinderColShape(policeCheckpoints[0], 6, 3, 0));
                Cols.Add(1, NAPI.ColShape.CreateCylinderColShape(policeCheckpoints[9], 6, 3, 0));
                Cols.Add(2, NAPI.ColShape.CreateCylinderColShape(policeCheckpoints[10], 6, 3, 0));
                Cols[0].OnEntityEnterColShape += arrestShape_onEntityEnterColShape;
                Cols[0].OnEntityExitColShape += arrestShape_onEntityExitColShape;
                Cols[1].OnEntityEnterColShape += arrestShape_onEntityEnterColShape;
                Cols[1].OnEntityExitColShape += arrestShape_onEntityExitColShape;
                Cols[2].OnEntityEnterColShape += arrestShape_onEntityEnterColShape;
                Cols[2].OnEntityExitColShape += arrestShape_onEntityExitColShape;

                ColShape colshape = NAPI.ColShape.CreateCylinderColShape(policeCheckpoints[4], 25, 5, 0);
                colshape.OnEntityExitColShape += (s, e) =>
                {
                    if (!(e is ExtPlayer extPlayer)) return;
                    if (!extPlayer.IsLogged()) return;

                    if (extPlayer.Character.ArrestDate <= DateTime.Now) return;
                    extPlayer.ChangePosition(policeCheckpoints[4]);
                };


                InteractShape.Create(policeCheckpoints[2], 1, 2)
                    .AddDefaultMarker()
                    .AddInteraction(ChangeDutyStatus, "interact_3");
                PoliceArrests.LoadArrests();

                var ped = new QuestPed(policePed);
                ped.PlayerInteracted += (player, ped) =>
                {
                    try
                    {
                        var introPage = new DialogPage("", ped.Name, ped.Role);
                        introPage.AddAnswer("Позвать сотрудника Полиции", callPolice);
                        introPage.AddCloseAnswer("Досвидания");
                        introPage.OpenForPlayer(player);
                    }
                    catch (Exception e)
                    {
                        _logger.WriteError("policePed: " + e.ToString());
                    }
                };

            } catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        public static void callPolice(ExtPlayer player)
        {
            try
            {
                DateTime access = DateTime.Now.AddMinutes(-1);
                if (player.HasData("playerSendFraction"))
                    access = player.GetData<DateTime>("playerSendFraction");
                if (DateTime.Now > access)
                {
                    Chat.SendFractionMessage(7, "Поступил вызов на ресепшен от " + player.Name.Replace('_', ' '), true);
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Вы вызвали сотрудника, ожидайте", 3000);
                    SafeTrigger.SetData(player, "playerSendFraction", DateTime.Now.AddMinutes(1));
                }
                else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Нельзя вызывать сотрудников так часто", 3000);
            }
            catch (Exception ex) { _logger.WriteError("callFibs: " + ex.ToString()); }
        }

        private static void ChangeDutyStatus(ExtPlayer player)
        {
            if (player.Character.FractionID == 7)
                SkinManager.OpenSkinMenu(player);
            else 
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_171", 3000);
        }



        #region shapes
        private void arrestShape_onEntityEnterColShape(ColShape shape, Player entity)
        {
            try
            {
                entity.SetData("IS_IN_ARREST_AREA", true);
            }
            catch (Exception ex) { _logger.WriteError("arrestShape_onEntityEnterColShape: " + ex.Message); }
        }

        private void arrestShape_onEntityExitColShape(ColShape shape, Player player)
        {
            try
            {
                if (!(player is ExtPlayer extPlayer)) return;
                if (!extPlayer.IsLogged()) return;

                SafeTrigger.SetData(extPlayer, "IS_IN_ARREST_AREA", false);
            }
            catch (Exception ex) { _logger.WriteError("arrestShape_onEntityExitColShape: " + ex.Message); }
        }
        #endregion

        public static void onPlayerDisconnectedhandler(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                if (player.HasData("ARREST_TIMER"))
                {
                    Timers.Stop(player.GetData<string>("ARREST_TIMER"));
                }

                if (player.Character.Following != null)
                {
                    ExtPlayer target = player.Character.Following;
                    target.Character.Follower = null;
                }
                else if (player.Character.Follower != null)
                {
                    ExtPlayer target = player.Character.Follower;
                    target.Character.Following = null;
                    SafeTrigger.ClientEvent(target, "setFollow", false);
                }                
            }
            catch (Exception e) { _logger.WriteError("PlayerDisconnected: " + e.ToString()); }
        }

    }
}