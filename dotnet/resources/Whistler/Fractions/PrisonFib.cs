using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Data;
using Whistler.GUI;
using Whistler.SDK;
using Whistler.Fractions.PDA;
using Whistler.MoneySystem;
using Whistler.Core;
using Whistler.Core.Character;
using Whistler.Helpers;
using Whistler.MoneySystem.Interface;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class Camera
    {
        public Camera(Vector3 pos)
        {
            Position = pos;
            Member1 = false;
            Member2 = false;
        }
        public Vector3 Position { get; set; }
        public bool Member1 { get; set; }
        public bool Member2 { get; set; }
    }
    class PrisonFib
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PrisonFib));
        public static List<Camera> PrisonPoints = new List<Camera>()
        {
        //   new Camera( new Vector3(1715.676, 2586.962, 44.46871)), // 0 -
          new Camera( new Vector3(1715.697, 2587.101, 50.90438)), // 1
          new Camera( new Vector3(1707.271, 2586.981, 47.74722)), // 2
          new Camera( new Vector3(1715.035, 2586.899, 47.76767)), // 3
          new Camera( new Vector3(1706.6, 2583.783, 47.75203)), // 4
          new Camera( new Vector3(1715.908, 2583.587, 47.76764)), // 5
          new Camera( new Vector3(1707.181, 2580.386, 47.74729)), // 6
          new Camera( new Vector3(1714.601, 2579.427, 47.77143)), // 7
          new Camera( new Vector3(1707.387, 2576.11, 47.74798)), // 8
          new Camera( new Vector3(1715.445, 2575.752, 47.77144)), // 9
        //   new Camera( new Vector3(1705.748, 2575.918, 44.46521)), // 10 -
        //   new Camera( new Vector3(1715.77, 2576.264, 44.46438)), // 11 - 
        //   new Camera( new Vector3(1707.366, 2579.838, 44.45911)), // 12 -
        //   new Camera( new Vector3(1716.032, 2579.51, 44.46613)), // 13 -
        //   new Camera( new Vector3(1706.94, 2583.531, 44.46509)), // 14 -
        //   new Camera( new Vector3(1715.246, 2583.726, 44.46679)), // 15 -
        //   new Camera( new Vector3(1707.487, 2587.029, 44.46593)), // 16 -
        //   new Camera( new Vector3(1674.291, 2587.05, 47.75146)), // 17 -
        //   new Camera( new Vector3(1674.702, 2586.976, 44.46871)), // 18 - 
        //   new Camera( new Vector3(1666.347, 2587.141, 44.46875)), // 19 -
        //   new Camera( new Vector3(1674.313, 2583.49, 44.46875)), // 20 -
        //   new Camera( new Vector3(1665.959, 2583.458, 44.4687)), // 21 -
        //   new Camera( new Vector3(1674.343, 2579.817, 44.4687)), // 22 - 
        //   new Camera( new Vector3(1666.052, 2579.738, 44.46871)), // 23 - 
        //   new Camera( new Vector3(1674.685, 2575.972, 44.46874)), // 24 -
        //   new Camera( new Vector3(1666.271, 2576.147, 44.46875)), // 25 -
          new Camera( new Vector3(1707.743, 2576.13, 50.8656)), // 26 
          new Camera( new Vector3(1707.601, 2579.892, 50.8656)), // 27 
          new Camera( new Vector3(1706.292, 2583.327, 50.86555)), // 28 
          new Camera( new Vector3(1705.184, 2587.066, 50.8656)), // 29
          new Camera( new Vector3(1715.91, 2576.13, 50.90438)), // 30
          new Camera( new Vector3(1715.716, 2579.748, 50.90437)), // 31
          new Camera( new Vector3(1717.055, 2583.361, 50.90438)), // 32
          new Camera( new Vector3(1666.522, 2575.697, 50.90437)), // 33
          new Camera( new Vector3(1666.467, 2579.915, 50.90438)), // 34
          new Camera( new Vector3(1666.445, 2583.582, 50.90437)), // 35
          new Camera( new Vector3(1666.448, 2586.699, 50.90437)), // 36
          new Camera( new Vector3(1674.599, 2575.974, 50.8656)), // 37
          new Camera( new Vector3(1674.102, 2579.543, 50.86563)), // 38
          new Camera( new Vector3(1673.899, 2582.775, 50.86561)), // 39
          new Camera( new Vector3(1675.145, 2586.719, 50.8656)), // 40
          new Camera( new Vector3(1674.339, 2575.541, 47.75204)), // 41
          new Camera( new Vector3(1674.729, 2579.914, 47.75209)), // 42
          new Camera( new Vector3(1674.797, 2583.786, 47.74777)), // 43
          new Camera( new Vector3(1664.898, 2576.19, 47.77139)), // 44
          new Camera( new Vector3(1666.62, 2579.86, 47.77143)), // 45
          new Camera( new Vector3(1665.501, 2583.529, 47.77145)), // 46
          new Camera( new Vector3(1665.822, 2586.9, 47.77129)), //47
        };

        public static Vector3 randomPrisonpointFib()
        {
            Vector3 pos = null;
            foreach (var item in PrisonPoints)
            {
                if (item == null || item.Member1) continue;

                item.Member1 = true;
                pos = item.Position;
            }
            if (pos == null)
            {
                foreach (var item in PrisonPoints)
                {
                    if (item == null || item.Member2) continue;
                    item.Member2 = true;
                    pos = item.Position;
                }
            }
            if (pos == null) pos = PrisonPoints[0].Position;
            return pos;
        }

        public static Vector3 checkPrison(int id)
        {
            Vector3 pos = null;
            pos = PrisonPoints[id].Position;
            return pos;
        }

        public static Vector3 EnterPoint = new Vector3(1711.0012, 2581.5393, 45.588715);
        public static Vector3 ExitPoint = new Vector3(1849.8461, 2600.6648, 45.61652);
        public static bool CanUsePrisonFib(ExtPlayer sender, bool notify = true)
        {
            if (sender.Character.FractionID == 7 || sender.Character.FractionID == 9)
                return true;
            if (notify)
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_326".Translate(), 3000);
            return false;
        }
        public static void ToPrison(ExtPlayer sender, ExtPlayer target, int time)
        {
            try
            {

            if (sender == target)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Невозможно применить на себе", 3000);
                return;
            }
            if (!sender.Character.OnDuty)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны начать рабочий день", 3000);
                return;
            }
            if (sender.Position.DistanceTo(target.Position) > 2)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок слишком далеко", 3000);
                return;
            }
            if (!sender.GetData<bool>("IS_IN_ARREST_AREA"))
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны быть возле камеры", 3000);
                return;
            }
            if (target.Character.CourtTime > 0)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок уже в тюрьме", 3000);
                return;
            }
            if (target.Character.WantedLVL == null)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не в розыске", 3000);
                return;
            }
            if (target.Character.WantedLVL.Level < 3 && target.Character.WantedLVL.Level > 0)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Этого игрока нужно определить в КПЗ", 3000);
                return;
            }
            if (!target.Character.Cuffed)
            {
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Игрок не в наручниках", 3000);
                return;
            }


                // if (!sender.Character.OnDuty)
                // {
                //     Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Вы должны начать рабочий день", 3000);
                //     return;
                // }
                // if (!sender.HasData("PrisFib") || !target.HasData("PrisFib") || sender.GetData<string>("PrisFib") != target.GetData<string>("PrisFib")) return;
                // if (time > 1000)
                // {
                //     Chat.SendTo(sender, "Frac_488".Translate(1000));
                //     return;
                // }

                target.ChangePosition(randomPrisonpointFib());
                target.UnCuffed();
                Weapons.RemoveAll(target, true);
                WantedSystem.SetPlayerWantedLevel(target, null, 0, null);  

                SafeTrigger.SetData(target, "ARREST_TIMER", Timers.StartTask(1000, () => timer_prisFib(target)));
                int fixedTime = target.Character.IsPrimeActive() ? Convert.ToInt32(time/2) : time;
                target.Character.CourtTime = fixedTime * 60;
                target.Character.ArrestID = sender.Character.FractionID;
                Chat.SendTo(target, $"{sender.Name} посадил Вас в тюрьму на {fixedTime} минут");
                Chat.SendTo(sender, $"Вы посадили в тюрьму {target.Name} на {time} минут");

                Chat.SendFractionMessage(7, "Frac_250".Translate(sender.Name, target.Name, target.Character.WantedLVL.Reason), true );
                Chat.SendFractionMessage(9, "Frac_250".Translate(sender.Name, target.Name, target.Character.WantedLVL.Reason), true );

                PDA.PoliceArrests.NewArrest(sender, target, target.Character.WantedLVL.Reason);
                GameLog.Arrest(sender.Character.UUID, target.Character.UUID, target.Character.WantedLVL.Reason, target.Character.WantedLVL.Level, sender.Name, target.Name);
           

                SafeTrigger.ClientEvent(target, "Client_CheckIsInJail");
                //Client_CheckIsInJail
            }
            catch (Exception e) { _logger.WriteError("ToPrison: " + e.ToString()); }
        }

        public static void SellZek(ExtPlayer target, int price, ExtPlayer lawyer)
        {
            try
            {
                if (!Wallet.TransferMoney(target.Character, new List<(IMoneyOwner, int)>
                {
                    (Manager.GetFraction(6), Convert.ToInt32(price * 0.8)),
                    (lawyer.Character, Convert.ToInt32(price * 0.2)),
                }, "Money_SellZek".Translate(lawyer.Character.UUID)))
                {
                    Notify.Send(target, NotifyType.Alert, NotifyPosition.BottomCenter, "Frac_491".Translate(), 5000);
                    Notify.Send(lawyer, NotifyType.Alert, NotifyPosition.BottomCenter, "Biz_56".Translate(), 5000);
                    return;
                }
                unPrisonFib(target, lawyer);
            }
            catch (Exception e) { _logger.WriteError("unPrisonFib: " + e.ToString()); }
        }
        public static void unPrisonFib(ExtPlayer target, ExtPlayer sender = null)
        {
            try
            {
                NAPI.Task.Run(() =>
                {
                    target.ChangePosition(ExitPoint);
                    target.Character.ArrestID = 0;
                    target.Character.CourtTime = 0;
                    SafeTrigger.ClientEvent(target, "Client_CheckIsInJailDestroy");
                    

                    if (target.HasData("ARREST_TIMER"))
                    {
                        Timers.Stop(target.GetData<string>("ARREST_TIMER"));
                        target.ResetData("ARREST_TIMER");
                    }

                    if (sender != null)
                        Chat.SendTo(sender, "Frac_492".Translate(target.Name));
                    Chat.SendTo(target, "Frac_493".Translate());
                });
            }
            catch (Exception e) { _logger.WriteError("unPrisonFib: " + e.ToString()); }
        }

        public static void timer_prisFib(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.Character.CourtTime <= 0)
                {
                    if (player.Character.ArrestID == 0) return;
                    unPrisonFib(player);
                    return;
                }

                player.Character.CourtTime--;
            }
            catch (Exception e)
            {
                _logger.WriteError("FIB PRISON TIME: " + e.ToString());
            }
        }

        public static void StartWork()
        {
            try
            {
                ColShape colShape = NAPI.ColShape.CreateCylinderColShape(new Vector3(1674.599, 2575.974, 50.8656), 50, 20, 0);
                colShape.OnEntityExitColShape += (s, e) =>
                {
                    if (e == null) return;
                    if (!(e is ExtPlayer player)) return;
                    if (player.Character == null) return;
                    if (player.Character.CourtTime <= 0) return;

                    player.ChangePosition(randomPrisonpointFib());
                };


                var col = NAPI.ColShape.CreateCylinderColShape(new Vector3(0, 0, 0), 1.2f, 2);
                NAPI.Blip.CreateBlip(188, EnterPoint, 1, 49, "Тюрьма", 255, 100, true, 0, 0);
                NAPI.Marker.CreateMarker(1, EnterPoint - new Vector3(0, 0, 1.5), new Vector3(), new Vector3(), 2, new Color(255, 255, 255, 80), false, NAPI.GlobalDimension);
                col = NAPI.ColShape.CreateCylinderColShape(EnterPoint, 1.2f, 2, NAPI.GlobalDimension);
                col.OnEntityEnterColShape += (c, p) =>
                {
                    try
                    {
                        if (p == null) return;

                        p.SetData("PrisFib", 9);
                    }
                    catch (Exception e) { _logger.WriteError("EXCEPTION AT \"StartWork_onEntityEnterColShape\":\n" + e.ToString()); }
                };
                col.OnEntityExitColShape += (c, p) =>
                {
                    try
                    {
                        if (p == null) return;

                        p.ResetData("PrisFib");
                    }
                    catch (Exception e) { _logger.WriteError("EXCEPTION AT \"StartWork_onEntityExitColShape\":\n" + e.ToString()); }
                };
            }
            catch (Exception e) { _logger.WriteError("StartWork: " + e.ToString()); }
        }
    }
}