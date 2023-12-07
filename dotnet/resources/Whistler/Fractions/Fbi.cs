using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using System;
using System.Linq;
using Whistler.ClothesCustom;
using Whistler.Helpers;
using Whistler.GUI.Lifts;
using Whistler.Core.QuestPeds;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class Fbi : Script
    {
        private static Dictionary<int, ColShape> Cols = new Dictionary<int, ColShape>();
        private static List<Vector3> fbiCheckpoints = new List<Vector3>()
        {
            new Vector3(124.0121, -769.8059, 241.3), // duty          0
            new Vector3(136.1821, -761.7615, 241.1518), // 49 floor       1
            new Vector3(130.9762, -762.3011, 241.1518), // 49 floor to 53          2
            new Vector3(156.81, -735.0605, 257.17), // 53 floor          3
            new Vector3(141.3403, -735.0605, 261.8509), // roof          4 stockfibiskrisha   Coords: new Vector3(141.3403, -735.0605, 261.7315),    JSON: {"x":141.340332,"y":-735.060547,"z":261.731476} 
            new Vector3(118.9617, -729.1614, 241.1518), // gun menu           5
            new Vector3(136.0578, -761.8408, 44.75204), // 1 floor        6
            new Vector3(123.9827, -740.5386, 32.13463), // 0 floor       7
        };
        public static bool warg_mode = false;

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Fbi));

        public static QuestPedParamModel fibPed = new QuestPedParamModel(PedHash.KarenDaniels, new Vector3(114.93268, -747.7851, 45.751575), "Molly Clark", "Сотрудник FIB", 76, 0, 2);



        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                #region cols
                InteractShape.Create(fbiCheckpoints[0], 1, 2, 0)
                    .AddDefaultMarker()
                    .AddInteraction(ChangeDutyStatus, "interact_3");

                InteractShape.Create(fbiCheckpoints[2], 1, 2, 0)
                    .AddDefaultMarker()
                    .AddInteraction(ChangePos49, "interact_0");

                InteractShape.Create(fbiCheckpoints[3], 1, 2, 0)
                    .AddDefaultMarker()
                    .AddInteraction(ChangePos53, "interact_0");

                #endregion
                Lift.Create(CheckAccessLift)
                    .AddFloor("client_54", fbiCheckpoints[7])
                    .AddFloor("client_55", fbiCheckpoints[6])
                    .AddFloor("client_56", fbiCheckpoints[1])
                    .AddFloor("client_57", fbiCheckpoints[4]);

                var ped = new QuestPed(fibPed);
                ped.PlayerInteracted += (player, ped) =>
                {
                    try
                    {
                        var introPage = new DialogPage("", ped.Name, ped.Role);
                        introPage.AddAnswer("Позвать сотрудника FIB", callFibs);
                        introPage.AddCloseAnswer("Досвидания");
                        introPage.OpenForPlayer(player);
                    }
                    catch (Exception e)
                    {
                        _logger.WriteError("fibPed: " + e.ToString());
                    }
                };
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }

        public static void callFibs(ExtPlayer player)
        {
            try
            {
                DateTime access = DateTime.Now.AddMinutes(-1);
                if (player.HasData("playerSendFraction"))
                    access = player.GetData<DateTime>("playerSendFraction");
                if (DateTime.Now > access)
                {
                    Chat.SendFractionMessage(9, "Поступил вызов на ресепшен от " + player.Name.Replace('_', ' '), true);
                    Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "Вы вызвали сотрудника, ожидайте", 3000);
                    SafeTrigger.SetData(player, "playerSendFraction", DateTime.Now.AddMinutes(1));
                }
                else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Нельзя вызывать сотрудников так часто", 3000);
            }
            catch (Exception ex) { _logger.WriteError("callFibs: " + ex.ToString()); }
        }
        private static void ChangeDutyStatus(ExtPlayer player)
        {
            if (player.Character.FractionID == 9)
                SkinManager.OpenSkinMenu(player);
            else 
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_165", 3000);
        }

        private static bool CheckAccessLift(ExtPlayer player)
        {
            DateTime access = DateTime.Now.AddMinutes(-1);
            if (player.HasData("accessFibLiftTime"))
                access = player.GetData<DateTime>("accessFibLiftTime");
            if (player.Character.FractionID != 9 && !player.IsAdmin() && DateTime.Now > access)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_542", 3000);
                return false;
            }
            return true;
        }

        private static void ChangePos49(ExtPlayer player)
        {
            player.ChangePosition(fbiCheckpoints[3] + new Vector3(0, 0, 1.12));
        }

        private static void ChangePos53(ExtPlayer player)
        {
            player.ChangePosition(fbiCheckpoints[2] + new Vector3(0, 0, 1.12));
        }

        [Command("accesslift")]
        public static void Command_GiveAccessToLift(ExtPlayer player, int id)
        {
            if (!Manager.CanUseCommand(player, "accesslift"))
                return;
            var target = Trigger.GetPlayerByUuid(id);
            if (target == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_342", 3000);
                return;
            }
            SafeTrigger.SetData(target, "accessFibLiftTime", DateTime.Now.AddMinutes(1));
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_543".Translate(target.Character.UUID), 3000);
        }
    }
}