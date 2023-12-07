using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.SDK.CustomColShape;
using Whistler.MoneySystem;
using Whistler.Inventory;
using Whistler.SDK.StockSystem;
using Whistler.Common;
using Whistler.Entities;
using static Whistler.Fractions.GangsCapture;

namespace Whistler.Fractions
{
    class GangsCapture : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(GangsCapture));
        public static readonly Dictionary<int, GangPoint> gangPoints = new Dictionary<int, GangPoint>();

        public static Dictionary<int, CaptConfig> CaptList = new Dictionary<int, CaptConfig>();
        public static Dictionary<int, WaitingConfig> WaitingCapt = new Dictionary<int, WaitingConfig>();

        private static int _winCaptureMoney = 300;

        private static int _minPlayerForCapt = 3;

        private static int _maxCaptTogether = 1;

        private static readonly int _maxHour = 23;
        private static readonly int _minHour = 13;

        private static readonly int waitMessageInterval = 20;

        private static readonly int timeForWaitCapt = 900;

        private static readonly int timeForCapt = 900;

        public static Dictionary<int, bool> gangIsCapt = new Dictionary<int, bool>
        {
            { 1, false }, // families
            { 2, false }, // ballas
            { 3, false }, // vagos
            { 4, false }, // marabunta
            { 5, false }, // blood street
        };

        public static Dictionary<int, int> gangPointsColor = new Dictionary<int, int>
        {
            { 1, 52 }, // families
            { 2, 58 }, // ballas
            { 3, 70 }, // vagos
            { 4, 77 }, // marabunta
            { 5, 59 }, // blood street
        };
        private static Dictionary<int, string> pictureNotif = new Dictionary<int, string>
        {
            { 1, "CHAR_MP_FAM_BOSS" }, // families
            { 2, "CHAR_MP_GERALD" }, // ballas
            { 3, "CHAR_ORTEGA" }, // vagos
            { 4, "CHAR_MP_ROBERTO" }, // marabunta
            { 5, "CHAR_MP_SNITCH" }, // blood street
        };
        private static Dictionary<int, DateTime> nextCaptDate = new Dictionary<int, DateTime>
        {
            { 1, DateTime.Now },
            { 2, DateTime.Now },
            { 3, DateTime.Now },
            { 4, DateTime.Now },
            { 5, DateTime.Now },
        };
        private static Dictionary<int, DateTime> protectDate = new Dictionary<int, DateTime>
        {
            { 1, DateTime.Now },
            { 2, DateTime.Now },
            { 3, DateTime.Now },
            { 4, DateTime.Now },
            { 5, DateTime.Now },
        };

        private static Dictionary<int, List<ExtPlayer>> _playerInTeamMenu = new Dictionary<int, List<ExtPlayer>>
        {
            { 1, new List<ExtPlayer>() },
            { 2, new List<ExtPlayer>() },
            { 3, new List<ExtPlayer>() },
            { 4, new List<ExtPlayer>() },
            { 5, new List<ExtPlayer>() },
        };

        private static List<GangZone> _zones = new List<GangZone>
        {
            new GangZone(44, 130, new Vector3(319.8332, -2185.118, 90.95729), 
                new List<List<int>>
                    {
                        new List<int>{0, 1, 2},
                        new List<int>{0, 1, 2, 3},
                        new List<int>{0, 1, 2, 3, 4},
                        new List<int>{0, 1, 2, 3, 4},
                        new List<int>{0, 1, 2, 3, 4},
                        new List<int>{0, 1, 2, 3},
                        new List<int>{   1, 2},
                    }),
            new GangZone(-5, 130, new Vector3(734.3699, -2488.151, 90.95729), 
                new List<List<int>>
                    {
                        new List<int>{0, 1, 2 },
                        new List<int>{0, 1, 2, 3},
                        new List<int>{0, 1, 2, 3},
                        new List<int>{0, 1, 2, 3},
                        new List<int>{0, 1, 2},
                        new List<int>{0, 1, 2},
                        new List<int>{0, 1},
                        new List<int>{0, 1},
                    }),
            new GangZone(-1F, 130, new Vector3(1557, -2493, 90.95729), 
                new List<List<int>>
                    {
                        new List<int>{        -1, 0},
                        new List<int>{        -1, 0},
                        new List<int>{        -1, 0},
                        new List<int>{        -1, 0},
                        new List<int>{    -2, -1, 0},
                        new List<int>{    -2, -1, 0},
                        new List<int>{-3, -2, -1, 0},
                        new List<int>{-3, -2, -1, 0},
                    }),
        };

        private static List<ShapeZone> gangZones = new List<ShapeZone>();

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                foreach (var zone in _zones)
                {
                    gangZones.AddRange(zone.GetPoints());
                }
                //using (var w = new StreamWriter("ParsedConfigs/gangzones.js"))
                //{
                //    w.WriteLine($"global.gangZones = JSON.parse(`{JsonConvert.SerializeObject(gangZones.Select(item => new { center = item.Center, width = item.Width, rot = item.Rotation, color = 1}))}`);");
                //}
                var result = MySQL.QueryRead("SELECT * FROM gangspoints");
                if (result == null || result.Rows.Count == 0)
                {
                    var index = 0;
                    foreach (var zone in gangZones)
                    {
                        MySQL.Query("INSERT INTO gangspoints (`id`, `gangid`) VALUES (@prop0, 1)", index);
                        index++;
                    }

                    result = MySQL.QueryRead("SELECT * FROM gangspoints");
                }

                foreach (DataRow Row in result.Rows)
                {
                    var data = new GangPoint();
                    data.ID = Convert.ToInt32(Row["id"]);
                    data.GangOwner = Convert.ToInt32(Row["gangid"]);
                    data.IsCapture = false;

                    if (data.ID >= gangZones.Count) break;
                    gangPoints.Add(data.ID, data);
                }

                foreach (var gangpoint in gangPoints.Values)
                {
                    var colShape = ShapeManager.CreateSquareColShape(gangZones[gangpoint.ID].Center, gangZones[gangpoint.ID].Width, gangZones[gangpoint.ID].Rotation, NAPI.GlobalDimension);
                    colShape.OnEntityEnterColShape += (ExtPlayer player) => onPlayerEnterGangPoint(player, gangpoint.ID);
                    colShape.OnEntityExitColShape += (ExtPlayer player) => onPlayerExitGangPoint(player, gangpoint.ID);
                }

                Main.OnPlayerReady += HandlePlayerReady;
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"FRACTIONS_CAPTURE\":\n" + e.ToString());
            }
        }

        private void HandlePlayerReady(ExtPlayer player)
        {
            var fractionId = player.Character.FractionID;
            var waitingConfig = WaitingCapt.Values.FirstOrDefault(conf => conf.AttackersFracId == fractionId || conf.DefendersFracId == fractionId);

            if (waitingConfig != null)
            {
                var currentTime = (int)(DateTime.Now - waitingConfig.StartWaitingAt).TotalSeconds;
                CaptureUI.SendUntilCaptureTimer(player, waitingConfig.WaitingTime, currentTime, "timer_to_capture_0");
            }
        }

        [Command("fixcapt")]
        public static void CMD_FixCapt(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (!Group.CanUseAdminCommand(player, "fixcapt")) return;
                foreach (var waitCapt in WaitingCapt)
                {
                    if (waitCapt.Value.TimerWait != null)
                    {
                        Timers.Stop(waitCapt.Value.TimerWait);
                        waitCapt.Value.TimerWait = null;
                    }
                    var region = gangPoints[waitCapt.Key];
                    region.IsCapture = false;
                    NAPI.Task.Run(() =>
                    {
                        SafeTrigger.ClientEventForAll("setZoneFlash", region.ID, false);
                        SafeTrigger.ClientEventForAll("setZoneColor", region.ID, gangPointsColor[region.GangOwner]);
                    });
                }
                WaitingCapt.Clear();

                foreach (var capt in CaptList)
                {
                    if (capt.Value.TimerCapt != null)
                    {
                        Timers.Stop(capt.Value.TimerCapt);
                        capt.Value.TimerCapt = null;
                    }
                    var region = gangPoints[capt.Key];
                    region.IsCapture = false;
                    NAPI.Task.Run(() =>
                    {
                        SafeTrigger.ClientEventForAll("setZoneFlash", region.ID, false);
                        SafeTrigger.ClientEventForAll("setZoneColor", region.ID, gangPointsColor[region.GangOwner]);
                    });
                }
                CaptList.Clear();

                for (int i = 0; i < protectDate.Count; i++)
                    protectDate[i] = DateTime.Now;
                for (int i = 0; i < nextCaptDate.Count; i++)
                    nextCaptDate[i] = DateTime.Now;
                for (int i = 0; i < gangIsCapt.Count; i++)
                    gangIsCapt[i] = false;
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"CMD_FixCapt\":\n" + e.ToString());
            }

        }

        [RemoteEvent("mmenu:captteam")]
        public static void CMD_OpenCaptTeamMenu(ExtPlayer player)
        {

            try
            {
                if (!player.IsLogged())
                    return;
                var fraction = Manager.GetFraction(player);
                if (!_playerInTeamMenu.ContainsKey(fraction?.Id ?? -1))
                    return;

                int enemyFracID = 0;
                var point = WaitingCapt.FirstOrDefault(item => gangPoints[item.Key].GangOwner == player.Character.FractionID);
                bool attackStatus = true;
                if (point.Value != null)
                {
                    enemyFracID = point.Value.AttackersFracId;
                    attackStatus = false;
                }
                else if (player.HasData("GANGPOINT") && player.GetData<int>("GANGPOINT") != -1 && gangPoints.ContainsKey(player.GetData<int>("GANGPOINT")))
                    enemyFracID = gangPoints[player.GetData<int>("GANGPOINT")].GangOwner;


                //Собираем игроков
                var members = fraction.OnlineMembers.Values.ToList();
                List<MemberDTO> listMembers = new List<MemberDTO>();
                foreach (var client in members)
                {
                    int state = 0;
                    if (client.HasData("CAPTTEAM") && client.GetData<bool>("CAPTTEAM") == true)
                        state = 2;
                    else if (client.HasData("IS_REQUESTED") && client.GetData<bool>("IS_REQUESTED") && client.HasData("REQUEST") && client.GetData<string>("REQUEST") == "INVITECAPT")
                        state = 1;
                    listMembers.Add(new MemberDTO(client.Name, fraction.GetRank(client).RankName, state));
                }

                //Отправляем данные
                SafeTrigger.ClientEvent(player, "capt:openTeamMenu", JsonConvert.SerializeObject(listMembers), fraction.Id, enemyFracID, attackStatus);
                _playerInTeamMenu[fraction.Id].Add(player);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"CMD_OpenCaptTeamMenu\":\n" + e.ToString());
            }
        }

        [RemoteEvent("capt:srvInviteMember")]
        public static void Event_InviteMember(ExtPlayer player, string name)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.CanUseCommand(player, "capture"))
                    return;
                int fracID = player.Character.FractionID;
                if (WaitingCapt.FirstOrDefault(item => item.Value.AttackersFracId == fracID).Value != null)
                    return;
                if (player.Name == name)
                {
                    SetMemberInGangTeam(player, 2);
                    return;
                }
                ExtPlayer target = Trigger.GetPlayerByName(name);
                if (target == null)
                    return;
                if (target.Character.FractionID != fracID)
                    return;
                SetMemberInGangTeam(target, 1);
                SafeTrigger.ClientEvent(target, "openDialog", "INVITECAPT", "Frac_525".Translate());
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"Event_InviteMember\":\n" + e.ToString());
            }
        }

        [RemoteEvent("capt:srvKickMember")]
        public static void Event_KickMember(ExtPlayer player, string name)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                if (!Manager.CanUseCommand(player, "capture"))
                    return;
                int fracID = player.Character.FractionID;
                if (WaitingCapt.FirstOrDefault(item => item.Value.AttackersFracId == fracID).Value != null)
                    return;
                ExtPlayer target = Trigger.GetPlayerByName(name);
                if (target == null)
                    return;
                if (target.Character.FractionID != fracID)
                    return;
                SetMemberInGangTeam(target, 0);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"Event_KickMember\":\n" + e.ToString());
            }
        }

        [RemoteEvent("capt:exitTeamMenu")]
        public static void Event_ExitTeamMenu(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                int fracID = player.Character.FractionID;
                if (!_playerInTeamMenu.ContainsKey(fracID) || !_playerInTeamMenu[fracID].Contains(player))
                    return;
                _playerInTeamMenu[fracID].Remove(player);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"Event_ExitTeamMenu\":\n" + e.ToString());
            }
        }

        [RemoteEvent("capt:attackToEnemy")]
        public static void Event_AttackToEnemy(ExtPlayer player)
        {
            CMD_startCapture(player);
        }

        [RemoteEvent("capt:deffendRegion")]
        public static void Event_DeffendRegion(ExtPlayer player)
        {
            CMD_DefendersReady(player);
        }

        public static void SetMemberInGangTeam(ExtPlayer player, int state)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                var fraction = Manager.GetFraction(player);
                if (fraction == null || !_playerInTeamMenu.ContainsKey(fraction.Id))
                    return;
                if (state == 2)
                    SafeTrigger.SetData(player, "CAPTTEAM", true);
                else
                    player.ResetData("CAPTTEAM");
                SafeTrigger.ClientEventToPlayers(_playerInTeamMenu[fraction.Id].ToArray(), "capt:setMember", player.Name, fraction.GetRank(player).RankName, state);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"SetMemberInGangTeam\":\n" + e.ToString());
            }
        }

        public static void CMD_startCapture(ExtPlayer player)
        {
            try
            {
                if (!Manager.CanUseCommand(player, "capture")) return;
                if (player.GetData<int>("GANGPOINT") == -1)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_399", 6000);
                    return;
                }
                GangPoint region = gangPoints[player.GetData<int>("GANGPOINT")];
                int attackFrack = player.Character.FractionID;

                if (region.GangOwner == attackFrack)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_400", 6000);
                    return;
                }
                
                if (Manager.GetFraction(region.GangOwner)?.OnlineMembers.FirstOrDefault(item => Manager.CanUseCommand(item.Value, "capture", false) == true).Key == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_526", 6000);
                    return;
                }
                if (DateTime.Now.Hour < _minHour || DateTime.Now.Hour > _maxHour)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_401", 6000);
                    return;
                }
                if (DateTime.Now < nextCaptDate[attackFrack])
                {
                    DateTime g = new DateTime((nextCaptDate[attackFrack] - DateTime.Now).Ticks);
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_402".Translate( g.Minute, g.Second), 6000);
                    return;
                }

                if (DateTime.Now < protectDate[region.GangOwner])
                {
                    DateTime g = new DateTime((protectDate[region.GangOwner] - DateTime.Now).Ticks);
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_403".Translate( g.Minute, g.Second), 6000);
                    return;
                }

                if (CaptList.Count() >= _maxCaptTogether)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_527", 6000);
                    return;
                }

                if (gangIsCapt[attackFrack])
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_528", 6000);
                    return;
                }
                if (gangIsCapt[region.GangOwner])
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_529", 6000);
                    return;
                }

                int attackersCount = Manager.GetFraction(attackFrack).OnlineMembers.Count(item => item.Value.HasData("CAPTTEAM") && item.Value.GetData<bool>("CAPTTEAM"));
                if (attackersCount < _minPlayerForCapt)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Недостаточно игроков в команде для атаки. Для начала захвата территории требуется минимум {_minPlayerForCapt} игрока.", 6000);
                    return;
                }

                if (Manager.countOfFractionMembers(region.GangOwner) < attackersCount)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_404", 6000);
                    return;
                }

                var waitingConfig = new WaitingConfig(attackFrack, region.GangOwner, Timers.Start(waitMessageInterval * 1000, () => WaitingUpdateTimer(region.ID)), timeForWaitCapt);

                WaitingCapt.Add(region.ID, waitingConfig);

                gangIsCapt[attackFrack] = true;
                gangIsCapt[region.GangOwner] = true;

                NAPI.Pools.GetAllPlayers().ForEach(player =>
                {
                    if (!(player is ExtPlayer p)) return;

                    if (p.IsLogged())
                    {
                        if(p.Character.FractionID == attackFrack)
                            CaptureUI.SendUntilCaptureTimer(p, waitingConfig.WaitingTime, 0, "timer_to_capture_0");
                        else if(p.Character.FractionID == region.GangOwner)
                        {
                            CaptureUI.SendUntilCaptureTimer(p, waitingConfig.WaitingTime, 0, "timer_to_capture_0");
                            if (Manager.CanUseCommand(p, "capture", false))
                                SafeTrigger.ClientEvent(p, "mmenu:frac:capt:attack");
                        }
                    }
                });

                region.IsCapture = true;

                SafeTrigger.ClientEventForAll("setZoneFlash", region.ID, true, gangPointsColor[region.GangOwner]);

                Chat.SendFractionMessage(region.GangOwner, "Frac_534".Translate((int)WaitingCapt[region.ID].TimeLeft.Subtract(DateTime.Now).TotalSeconds), false);
                Chat.SendFractionMessage(attackFrack, "Frac_535".Translate(Manager.getName(region.GangOwner)), false);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"CMD_startCapture\":\n" + e.ToString());
            }
        }

        public static void CMD_DefendersReady(ExtPlayer player)
        {
            try
            {
                var point = WaitingCapt.FirstOrDefault(item => gangPoints[item.Key].GangOwner == player.Character.FractionID);
                if (point.Value == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_531", 6000);
                    return;
                }

                int attackId = point.Value.AttackersFracId;
                int defendId = gangPoints[point.Key].GangOwner;
                Dictionary<ExtPlayer, bool> attackers = new Dictionary<ExtPlayer, bool>();
                Dictionary<ExtPlayer, bool> defenders = new Dictionary<ExtPlayer, bool>();

                foreach (var play in Manager.GetFraction(attackId).OnlineMembers.Values)
                {
                    if (play.IsLogged() && play.HasData("CAPTTEAM") && play.GetData<bool>("CAPTTEAM") == true)
                        attackers.Add(play, play.HasData("GANGPOINT") && play.GetData<int>("GANGPOINT") == point.Key);
                }
                foreach (var play in Manager.GetFraction(defendId).OnlineMembers.Values)
                {
                    if (play.IsLogged() && play.HasData("CAPTTEAM") && play.GetData<bool>("CAPTTEAM") == true)
                        defenders.Add(play, play.HasData("GANGPOINT") && play.GetData<int>("GANGPOINT") == point.Key);
                }

                if (defenders.Count() < attackers.Count() && defenders.Count() < Manager.countOfFractionMembers(defendId))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_532", 6000);
                    return;
                }

                if (defenders.Count() > attackers.Count())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_533".Translate( attackers.Count()), 6000);
                    return;
                }
                StartGangCapture(attackers, defenders, point.Key);
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"CMD_DefendersReady\":\n" + e.ToString());
            }
        }

        public static void StartGangCapture(Dictionary<ExtPlayer, bool> attackers, Dictionary<ExtPlayer, bool> defenders, int idGangPoint)
        {
            try
            {
                if (!WaitingCapt.ContainsKey(idGangPoint))
                    return;
                if (CaptList.ContainsKey(idGangPoint))
                    return;
                var region = gangPoints[idGangPoint];
                int attackFrack = WaitingCapt[idGangPoint].AttackersFracId;
                if (WaitingCapt[idGangPoint].TimerWait != null)
                {
                    Timers.Stop(WaitingCapt[idGangPoint].TimerWait);
                    WaitingCapt[idGangPoint].TimerWait = null;
                }
                WaitingCapt.Remove(idGangPoint);
                List<ExtPlayer> playerInPoint = Main.GetExtPlayersListByPredicate((player) => player.HasData("GANGPOINT") && player.GetData<int>("GANGPOINT") == idGangPoint && (player.Character.FractionID == attackFrack || player.Character.FractionID == region.GangOwner));
                CaptList.Add(idGangPoint, new CaptConfig(defenders, attackers, playerInPoint, region.GangOwner, attackFrack, Timers.StartOnce(timeForCapt * 1000, () => CaptCloseTimer(idGangPoint)), timeForCapt));

                foreach (var p in defenders)
                {
                    if (p.Value)
                        SafeTrigger.UpdateDimension(p.Key, CaptList[idGangPoint].Dimension);
                }

                foreach (var p in attackers)
                {
                    if (p.Value)
                        SafeTrigger.UpdateDimension(p.Key, CaptList[idGangPoint].Dimension);
                }

                foreach (var p in playerInPoint)
                {
                    ShowHud(p, true, true, attackFrack, region.GangOwner, 0, attackers.Count(), defenders.Count());
                }

                //CaptList[idGangPoint].TimerCapt = Timers.Start(1000, () => timerUpdate(idGangPoint));

                Chat.SendFractionMessage(region.GangOwner, "Frac_408".Translate(Manager.getName(attackFrack)), false);
                Chat.SendFractionMessage(attackFrack, "Frac_409", false);


                UpdateCaptInfo(idGangPoint);

                var gangsters = Trigger.GetAllPlayers()
                    .Where(p => p.IsLogged())
                    .Where(p => p.Character.FractionID == attackFrack || p.Character.FractionID == region.GangOwner)
                    .ToList();

                foreach (var p in gangsters)
                {
                    CaptureUI.DisableUntilCaptureTimer(p);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"StartGangCapture\":\n" + e.ToString());
            }
        }

        public static void EndWaitCapture(int gangPointID, bool defendIsLost = true)
        {
            try
            {
                if (!WaitingCapt.ContainsKey(gangPointID))
                    return;
                if (WaitingCapt[gangPointID].TimerWait != null)
                {
                    Timers.Stop(WaitingCapt[gangPointID].TimerWait);
                    WaitingCapt[gangPointID].TimerWait = null;
                }

                int attackersID = WaitingCapt[gangPointID].AttackersFracId;
                int defendersID = WaitingCapt[gangPointID].DefendersFracId;

                WaitingCapt.Remove(gangPointID);

                var region = gangPoints[gangPointID];
                region.IsCapture = false;

                gangIsCapt[defendersID] = false;
                gangIsCapt[attackersID] = false;

                protectDate[defendersID] = DateTime.Now.AddMinutes(20);
                protectDate[attackersID] = DateTime.Now.AddMinutes(20);

                Chat.SendFractionMessage(defendersID, "Frac_412", false);
                Chat.SendFractionMessage(attackersID, "Frac_413", false);
                region.GangOwner = attackersID;
                foreach (var player in Manager.GetFraction(attackersID).OnlineMembers.Values)
                {
                    if (player.IsLogged())
                    {
                        Wallet.MoneyAdd(player.Character, _winCaptureMoney, "Награда за победу в войне банд");
                    }
                }
                nextCaptDate[attackersID] = DateTime.Now.AddMinutes(30);
                NAPI.Task.Run(() =>
                {
                    SafeTrigger.ClientEventForAll("setZoneFlash", region.ID, false);
                    SafeTrigger.ClientEventForAll("setZoneColor", region.ID, gangPointsColor[region.GangOwner]);
                });
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"EndWaitCapture\":\n" + e.ToString());
            }
        }

        public static void EndGangCapture(int idPoint)
        {
            try
            {
                if (!CaptList.ContainsKey(idPoint))
                    return;
                var capt = CaptList[idPoint];
                if (capt.TimerCapt != null)
                {
                    Timers.Stop(capt.TimerCapt);
                    capt.TimerCapt = null;
                }

                var region = gangPoints[idPoint];
                region.IsCapture = false;

                gangIsCapt[capt.DefendersFracId] = false;
                gangIsCapt[capt.AttackersFracId] = false;

                protectDate[capt.DefendersFracId] = DateTime.Now.AddMinutes(20);
                protectDate[capt.AttackersFracId] = DateTime.Now.AddMinutes(20);

                foreach (var player in capt.InsideZone)
                {
                    if (!player.IsLogged()) continue;
                    ShowHud(player, false, true);
                }

                foreach (var p in capt.Attackers)
                {
                    if (p.Key.Dimension == capt.Dimension) SafeTrigger.UpdateDimension(p.Key, 0);
                }
                foreach (var p in capt.Defenders)
                {
                    if (p.Key.Dimension == capt.Dimension) SafeTrigger.UpdateDimension(p.Key, 0);
                }

                if (capt.Attackers.Count(item => item.Value == true) <= capt.Defenders.Count(item => item.Value == true))
                {
                    Chat.SendFractionMessage(capt.DefendersFracId, "Frac_410", false);
                    Chat.SendFractionMessage(capt.AttackersFracId, "Frac_411", false);
                    foreach (var player in Manager.GetFraction(capt.DefendersFracId).OnlineMembers.Values)
                    {
                        if (player.IsLogged())
                        {
                            Wallet.MoneyAdd(player.Character, _winCaptureMoney, "Награда за победу в войне банд");
                        }
                    }
                }
                else
                {
                    Chat.SendFractionMessage(capt.DefendersFracId, "Frac_412", false);
                    Chat.SendFractionMessage(capt.AttackersFracId, "Frac_413", false);
                    region.GangOwner = capt.AttackersFracId;
                    foreach (var player in Manager.GetFraction(capt.AttackersFracId).OnlineMembers.Values)
                    {
                        if (player.IsLogged())
                        {
                            Wallet.MoneyAdd(player.Character, _winCaptureMoney, "Награда за победу в войне банд");
                        }
                    }
                }
                nextCaptDate[capt.AttackersFracId] = DateTime.Now.AddMinutes(30);
                SendItemsToWinnerStock(capt.Dimension, region.GangOwner);
                NAPI.Task.Run(() =>
                {
                    SafeTrigger.ClientEventForAll("setZoneFlash", region.ID, false);
                    SafeTrigger.ClientEventForAll("setZoneColor", region.ID, gangPointsColor[region.GangOwner]);
                });
                CaptList.Remove(idPoint);
                SavingRegions();
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"EndGangCapture\":\n" + e.ToString());
            }
        }

        private static void SendItemsToWinnerStock(uint dimension, int winFrac)
        {
            var items = DropSystem.ClearItemsInDimension(dimension);
            var stock = StockManager.GetStockByPredicate(item => item.OwnerId == winFrac && item.TypeOwner == OwnerType.Fraction);
            if (stock == null)
                return;
            var inventory = InventoryService.GetById(stock.InventoryId);
            foreach (var item in items)
            {
                if (item != null)
                    inventory.AddItem(item, log: Inventory.Enums.LogAction.Move);
            }
        }


        private static void WaitingUpdateTimer(int gangPointID)
        {
            try
            {
                if (!WaitingCapt.ContainsKey(gangPointID))
                    return;

                if (WaitingCapt[gangPointID].TimeLeft <= DateTime.Now)
                    EndWaitCapture(gangPointID);
            }
            catch (Exception e) { _logger.WriteError("WaitingUpdateTimer: " + e.ToString()); }
        }

        private static void CaptCloseTimer(int gangPointID)
        {
            try
            {
                EndGangCapture(gangPointID);
            }
            catch (Exception e) { _logger.WriteError("CaptCloseTimer: " + e.ToString()); }
        }


        public static void UpdateCaptInfo(int idPoint)
        {
            try
            {
                if (!CaptList.ContainsKey(idPoint))
                    return;
                var capt = CaptList[idPoint];
                foreach (var player in capt.InsideZone)
                {
                    CaptureUI.SetCaptureStats(player, capt.Attackers.Count(), capt.Defenders.Count(), (int)capt.TimeLeft.Subtract(DateTime.Now).TotalSeconds);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"UpdateCaptInfo\":\n" + e.ToString());
            }
        }

        
        public static void ShowHud(ExtPlayer player, bool show, bool resetKillLog, int firstTeam = 0, int secondTeam = 0, int captTime = 0, int firstTeamCnt = 0, int secondTeamCnt = 0)
        {
            try
            {
                if (show)
                {
                    CaptureUI.EnableCaptureUI(player, firstTeam, secondTeam, captTime, firstTeamCnt, secondTeamCnt);
                    CaptureUI.EnableKillLog(player);
                }
                else
                {
                    CaptureUI.DisableCaptureUI(player);
                    CaptureUI.DisableKillog(player, resetKillLog);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"ShowHud\":\n" + e.ToString());
            }
        }

        private static void onPlayerEnterGangPoint(ExtPlayer player, int idPoint)
        {
            if (!player.IsLogged()) return;
            SafeTrigger.SetData(player, "GANGPOINT", idPoint);

            if (CaptList.ContainsKey(idPoint) && (player.Character.FractionID == CaptList[idPoint].AttackersFracId || player.Character.FractionID == CaptList[idPoint].DefendersFracId))
            {
                CaptList[idPoint].InsideZone.Add(player);
                ShowHud(player, true, true, CaptList[idPoint].AttackersFracId, CaptList[idPoint].DefendersFracId);
                UpdateCaptInfo(idPoint);
                if (CaptList[idPoint].Attackers.ContainsKey(player))
                {
                    CaptList[idPoint].Attackers[player] = true;
                    if (player.Dimension != CaptList[idPoint].Dimension)
                        SafeTrigger.UpdateDimension(player,  CaptList[idPoint].Dimension);
                }
                else if (CaptList[idPoint].Defenders.ContainsKey(player))
                {
                    CaptList[idPoint].Defenders[player] = true;
                    if (player.Dimension != CaptList[idPoint].Dimension)
                        SafeTrigger.UpdateDimension(player,  CaptList[idPoint].Dimension);
                }
            }
        }

        private static void onPlayerExitGangPoint(ExtPlayer player, int idPoint)
        {
            if (!player.IsLogged()) return;

            if (idPoint == player.GetData<int>("GANGPOINT"))
            {
                SafeTrigger.SetData(player, "GANGPOINT", -1);
            }
            if (CaptList.ContainsKey(idPoint) && CaptList[idPoint].InsideZone.Contains(player))
            {
                CaptList[idPoint].InsideZone.Remove(player);
                ShowHud(player, false, false);
                UpdateCaptInfo(idPoint);
                if (CaptList[idPoint].Attackers.ContainsKey(player))
                    CaptList[idPoint].Attackers[player] = false;
                else if (CaptList[idPoint].Defenders.ContainsKey(player))
                    CaptList[idPoint].Defenders[player] = false;
            }
        }

        public static bool Event_PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            try
            {
                if (!CaptList.Any()) return false;

                KeyValuePair<int, CaptConfig> capt = CaptList.FirstOrDefault(item => item.Value != null && (item.Value.Attackers.ContainsKey(player) || item.Value.Defenders.ContainsKey(player)));
                if (capt.Value == null) return false;

                if (capt.Value.Attackers != null && capt.Value.Attackers.ContainsKey(player)) capt.Value.Attackers.Remove(player);
                else if (capt.Value.Defenders != null && capt.Value.Defenders.ContainsKey(player)) capt.Value.Defenders.Remove(player);

                foreach (var target in capt.Value.InsideZone)
                {
                    CaptureUI.AddKillogItem(target, killer, player, weapon);
                }
                if (capt.Value.Attackers.ContainsKey(killer) || capt.Value.Defenders.ContainsKey(killer))
                {
                    killer.CreatePlayerAction(PersonalEvents.PlayerActions.KillOnCaptAndBizwar, 1);
                }
                UpdateCaptInfo(capt.Key);
                if (capt.Value.Attackers.Count() == 0 || capt.Value.Defenders.Count() == 0)
                    EndGangCapture(capt.Key);
                return true;
            }
            catch (Exception e) 
            { 
                _logger.WriteError("PlayerDeath: " + e.ToString());
                return false;
            }
        }

        public static void SavingRegions()
        {
            try
            {
                foreach (var region in gangPoints.Values)
                    MySQL.Query("UPDATE gangspoints SET gangid = @prop0 WHERE id = @prop1", region.GangOwner, region.ID);
            }
            catch (Exception e)
            {
                _logger.WriteError($"SavingRegions:\n{e}");
            }
        }

        public static void LoadBlips(ExtPlayer player)
        {
            try
            {
                var colors = new List<int>();
                foreach (var g in gangPoints.Values)
                    colors.Add(gangPointsColor[g.GangOwner]);

                SafeTrigger.ClientEvent(player, "loadCaptureBlips", Newtonsoft.Json.JsonConvert.SerializeObject(colors));

                foreach (var zone in CaptList)
                    SafeTrigger.ClientEvent(player, "setZoneFlash", zone.Key, true, gangPointsColor[zone.Value.DefendersFracId]);
                foreach (var zone in WaitingCapt)
                    SafeTrigger.ClientEvent(player, "setZoneFlash", zone.Key, true, gangPointsColor[gangPoints[zone.Key].GangOwner]);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"GangCapture.LoadBlips: {ex.ToString()}");
            }
        }

        //[ServerEvent(Event.ResourceStop)]
        //public void OnResourceStop()
        //{
        //    try
        //    {
        //        SavingRegions();
        //    }
        //    catch (Exception e) { _logger.WriteError("ResourceStop: " + e.ToString()); }
        //}

        class ShapeZone
        {
            public Vector3 Center { get; }
            public bool IsSquare { get; set; }
            public float Width { get; set; }
            public float Rotation { get; set; }

            public ShapeZone(Vector3 center, float width, float rotation)
            {
                Center = center;
                Width = width;
                Rotation = rotation;
                IsSquare = true;
            }
        }

        class GangZone
        {
            private float _rotation { get; }
            private float _width { get; }
            private List<List<int>> _offsets { get; }
            private Vector3 _start { get; }
            public GangZone(float rotation, float width, Vector3 start, List<List<int>> offsets)
            {
                _rotation = rotation;
                _width = width;
                _start = start;
                _offsets = offsets;
            }
            public List<ShapeZone> GetPoints()
            {
                var cos = Math.Cos(_rotation / 180 * Math.PI);
                var sin = Math.Sin(_rotation / 180 * Math.PI);
                var start = _start;
                List<ShapeZone> zones = new List<ShapeZone>();
                foreach (var line in _offsets)
                {
                    foreach (var item in line)
                    {
                        zones.Add(new ShapeZone(start + new Vector3(_width * cos * item, _width * sin * item, 0), _width, _rotation));
                    }
                    start += new Vector3(-_width * sin, _width * cos, 0);
                }
                return zones;
            }
        }
        public class GangPoint
        {
            public int ID { get; set; }
            public int GangOwner { get; set; }
            public bool IsCapture { get; set; }
        }

        public class CaptConfig
        {
            private static uint _dim = 666666;
            public Dictionary<ExtPlayer, bool> Defenders { get; set; }
            public Dictionary<ExtPlayer, bool> Attackers { get; set; }
            public List<ExtPlayer> InsideZone { get; set; }
            public int AttackersFracId { get; set; }
            public int DefendersFracId { get; set; }
            public string TimerCapt { get; set; }
            public DateTime TimeLeft { get; set; }
            public uint Dimension { get; private set; }
            public CaptConfig(Dictionary<ExtPlayer, bool> defenders, Dictionary<ExtPlayer, bool> attackers, List<ExtPlayer> insideZone, int defendersFracId, int attackersFracId, string timerCapt, int timeForCapt)
            {
                Defenders = defenders;
                Attackers = attackers;
                InsideZone = insideZone;
                AttackersFracId = attackersFracId;
                DefendersFracId = defendersFracId;
                TimerCapt = timerCapt;
                TimeLeft = DateTime.Now.AddSeconds(timeForCapt);
                Dimension = _dim++;
            }
        }

        public class WaitingConfig
        {
            public int AttackersFracId { get; set; }
            public int DefendersFracId { get; set; }
            public string TimerWait { get; set; }
            public DateTime TimeLeft { get; set; }
            public DateTime StartWaitingAt { get; set; }
            public int WaitingTime { get; set; }
            public WaitingConfig(int attackersFracId, int defendersFracId, string timerWait, int timeLeft)
            {
                AttackersFracId = attackersFracId;
                DefendersFracId = defendersFracId;
                TimerWait = timerWait;
                TimeLeft = DateTime.Now.AddSeconds(timeLeft);
                WaitingTime = timeLeft;

                StartWaitingAt = DateTime.Now;
            }
        }

        public class MemberDTO
        {
            public string Name { get; set; }
            public string Rank { get; set; }
            public int State { get; set; }
            public MemberDTO(string name, string rank, int state)
            {
                Name = name;
                Rank = rank;
                State = state;
            }
        }
    }
}
