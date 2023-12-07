using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Data;
using Whistler.GUI;
using Whistler.SDK;
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
          new Camera( new Vector3(1715.676, 2586.962, 44.46871)),
          new Camera( new Vector3(1715.697, 2587.101, 50.90438)),
          new Camera( new Vector3(1707.271, 2586.981, 47.74722)),
          new Camera( new Vector3(1715.035, 2586.899, 47.76767)),
          new Camera( new Vector3(1706.6, 2583.783, 47.75203)),
          new Camera( new Vector3(1715.908, 2583.587, 47.76764)),
          new Camera( new Vector3(1707.181, 2580.386, 47.74729)),
          new Camera( new Vector3(1714.601, 2579.427, 47.77143)),
          new Camera( new Vector3(1707.387, 2576.11, 47.74798)),
          new Camera( new Vector3(1715.445, 2575.752, 47.77144)),
          new Camera( new Vector3(1705.748, 2575.918, 44.46521)),
          new Camera( new Vector3(1715.77, 2576.264, 44.46438)),
          new Camera( new Vector3(1707.366, 2579.838, 44.45911)),
          new Camera( new Vector3(1716.032, 2579.51, 44.46613)),
          new Camera( new Vector3(1706.94, 2583.531, 44.46509)),
          new Camera( new Vector3(1715.246, 2583.726, 44.46679)),
          new Camera( new Vector3(1707.487, 2587.029, 44.46593)),
          new Camera( new Vector3(1674.291, 2587.05, 47.75146)),
          new Camera( new Vector3(1674.702, 2586.976, 44.46871)),
          new Camera( new Vector3(1666.347, 2587.141, 44.46875)),
          new Camera( new Vector3(1674.313, 2583.49, 44.46875)),
          new Camera( new Vector3(1665.959, 2583.458, 44.4687)),
          new Camera( new Vector3(1674.343, 2579.817, 44.4687)),
          new Camera( new Vector3(1666.052, 2579.738, 44.46871)),
          new Camera( new Vector3(1674.685, 2575.972, 44.46874)),
          new Camera( new Vector3(1666.271, 2576.147, 44.46875)),
          new Camera( new Vector3(1707.743, 2576.13, 50.8656)),
          new Camera( new Vector3(1707.601, 2579.892, 50.8656)),
          new Camera( new Vector3(1706.292, 2583.327, 50.86555)),
          new Camera( new Vector3(1705.184, 2587.066, 50.8656)),
          new Camera( new Vector3(1715.91, 2576.13, 50.90438)),
          new Camera( new Vector3(1715.716, 2579.748, 50.90437)),
          new Camera( new Vector3(1717.055, 2583.361, 50.90438)),
          new Camera( new Vector3(1666.522, 2575.697, 50.90437)),
          new Camera( new Vector3(1666.467, 2579.915, 50.90438)),
          new Camera( new Vector3(1666.445, 2583.582, 50.90437)),
          new Camera( new Vector3(1666.448, 2586.699, 50.90437)),
          new Camera( new Vector3(1674.599, 2575.974, 50.8656)),
          new Camera( new Vector3(1674.102, 2579.543, 50.86563)),
          new Camera( new Vector3(1673.899, 2582.775, 50.86561)),
          new Camera( new Vector3(1675.145, 2586.719, 50.8656)),
          new Camera( new Vector3(1674.339, 2575.541, 47.75204)),
          new Camera( new Vector3(1674.729, 2579.914, 47.75209)),
          new Camera( new Vector3(1674.797, 2583.786, 47.74777)),
          new Camera( new Vector3(1664.898, 2576.19, 47.77139)),
          new Camera( new Vector3(1666.62, 2579.86, 47.77143)),
          new Camera( new Vector3(1665.501, 2583.529, 47.77145)),
          new Camera( new Vector3(1665.822, 2586.9, 47.77129)),
        };

        public static Vector3 randomPrisonpointFib()
        {
            Vector3 pos = null;
            foreach (var item in PrisonPoints)
            {
                if (item.Member1) continue;
                item.Member1 = true;
                pos = item.Position;
            }
            if (pos == null)
            {
                foreach (var item in PrisonPoints)
                {
                    if (item.Member2) continue;
                    item.Member2 = true;
                    pos = item.Position;
                }
            }
            if (pos == null) pos = PrisonPoints[0].Position;
            return pos;
        }

        public static Vector3 EnterPoint = new Vector3(1696.589, 2570.917, 44.46875);
        public static Vector3 ExitPoint = new Vector3(1823.519, 2594.261, 44.77098);
        public static bool CanUsePrisonFib(PlayerGo sender, bool notify = true)
        {
            if (sender.GetCharacter().FractionID == 7 || sender.GetCharacter().FractionID == 9)
                return true;
            if (notify)
                Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_326".Translate(), 3000);
            return false;
        }
        public static void ToPrison(PlayerGo sender, PlayerGo target, int time)
        {
            try
            {
                if (!sender.GetCharacter().OnDuty)
                {
                    Notify.Send(sender, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_167".Translate(), 3000);
                    return;
                }
                if (!sender.HasData("PrisFib") || !target.HasData("PrisFib") || sender.GetData<string>("PrisFib") != target.GetData<string>("PrisFib")) return;
                if (time > 1000)
                {
                    Chat.SendTo(sender, "Frac_488".Translate(1000));
                    return;
                }
                target.ChangePosition(randomPrisonpointFib());
                target.UnCuffed();
                Weapons.RemoveAll(target, true);
                target.ResetData("putprison");

                target.SetData("ARREST_TIMER", Timers.StartTask(1000, () => timer_prisFib(target)));
                target.GetCharacter().CourtTime = time * 60;
                target.GetCharacter().ArrestID = sender.GetCharacter().FractionID;
                Chat.SendTo(target, "Frac_489".Translate( sender.Name, time));
                Chat.SendTo(sender, "Frac_490".Translate(target.Name, time));
            }
            catch (Exception e) { _logger.WriteError("ToPrison: " + e.ToString()); }
        }

        public static void SellZek(PlayerGo target, int price, PlayerGo lawyer)
        {
            try
            {
                if (!Wallet.TransferMoney(target.GetCharacter(), new List<(IMoneyOwner, int)>
                {
                    (Manager.GetFraction(6), Convert.ToInt32(price * 0.8)),
                    (lawyer.GetCharacter(), Convert.ToInt32(price * 0.2)),
                }, "Money_SellZek".Translate(lawyer.GetCharacter().UUID)))
                {
                    Notify.Send(target, NotifyType.Alert, NotifyPosition.BottomCenter, "Frac_491".Translate(), 5000);
                    Notify.Send(lawyer, NotifyType.Alert, NotifyPosition.BottomCenter, "Biz_56".Translate(), 5000);
                    return;
                }
                unPrisonFib(target, lawyer);
            }
            catch (Exception e) { _logger.WriteError("unPrisonFib: " + e.ToString()); }
        }
        public static void unPrisonFib(PlayerGo target, PlayerGo sender = null)
        {
            try
            {
                WhistlerTask.Run(() =>
                {
                    target.ChangePosition(ExitPoint);
                    target.GetCharacter().ArrestID = 0;
                    target.GetCharacter().CourtTime = 0;

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

        public static void timer_prisFib(PlayerGo player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.GetCharacter().CourtTime <= 0)
                {
                    if (player.GetCharacter().ArrestID == 0) return;
                    unPrisonFib(player);
                    return;
                }
                player.GetCharacter().CourtTime--;
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
                var col = NAPI.ColShape.CreateCylinderColShape(new Vector3(0, 0, 0), 1.2f, 2);
                NAPI.Blip.CreateBlip(188, EnterPoint, 1, 49, "ׂ‏נüלא", 255, 100, true, 0, 0);
                NAPI.Marker.CreateMarker(1, EnterPoint - new Vector3(0, 0, 1.5), new Vector3(), new Vector3(), 2, new Color(255, 255, 255, 80), false, NAPI.GlobalDimension);
                col = NAPI.ColShape.CreateCylinderColShape(EnterPoint, 1.2f, 2, NAPI.GlobalDimension);
                col.OnEntityEnterColShape += (c, p) =>
                {
                    try
                    {
                        p.SetData("PrisFib", 9);
                    }
                    catch (Exception e) { _logger.WriteError("EXCEPTION AT \"StartWork_onEntityEnterColShape\":\n" + e.ToString()); }
                };
                col.OnEntityExitColShape += (c, p) =>
                {
                    try
                    {
                        p.ResetData("PrisFib");
                    }
                    catch (Exception e) { _logger.WriteError("EXCEPTION AT \"StartWork_onEntityExitColShape\":\n" + e.ToString()); }
                };
            }
            catch (Exception e) { _logger.WriteError("StartWork: " + e.ToString()); }
        }
    }
}