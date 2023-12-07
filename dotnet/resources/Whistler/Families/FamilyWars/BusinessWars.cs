using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using Whistler.Fractions;
using Whistler.MoneySystem;
using Whistler.Helpers;
using Whistler.Families.Models;
using System.Linq;
using Whistler.Entities;

namespace Whistler.Families
{
    class BusinessWar
    {
        public bool warIsGoing = false;
        public bool warStarting = false;
        private string warTimer;
        private string toStartWarTimer;
        private Family attackersFamily { get; set; }
        private Family defendersFamily { get; set; }
        private int timerCount
        {
            get
            {
                return (int)(DateTime.Now - _startCapt).TotalSeconds;
            }
        }
        private DateTime _startCapt = DateTime.Now;

        private int _winBizMoney = 300;
        private BattleModel _battleModel = null;


        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BusinessWar));
        public BattleLocation Id { get; set; }
        public Blip WarBlip { get; set; }
        public ColShape WarColShape { get; set; }
        public event Action<ExtPlayer, BattleLocation> PlayerEnterBattleLocation;
        public event Action<ExtPlayer, BattleLocation> PlayerExitBattleLocation;
        public BusinessWar(BattleLocation id, BattleZoneModel zone)
        {
            Id = id;
            WarColShape = NAPI.ColShape.CreateCylinderColShape(zone.Position + new Vector3(0, 0, -500), zone.Range, 1000, zone.Dimension);
            WarColShape.OnEntityEnterColShape += onPlayerEnterBizWar;
            WarColShape.OnEntityExitColShape += onPlayerExitBizWar;
            if (zone.Enable)
                WarBlip = NAPI.Blip.CreateBlip(zone.BlipType, zone.Position, 1, zone.BlipColor, Main.StringToU16("Война за бизнес"), 255, 0, true, 0, zone.Dimension);
        }
        private void onPlayerEnterBizWar(ColShape shape, Player client)
        {
            try
            {
                if (!(client is ExtPlayer player)) return;

                if (!player.IsLogged())
                    return;
                player.Character.WarZone = Id;
                PlayerEnterBattleLocation?.Invoke(player, Id);
                if (warIsGoing && (player.Character.FamilyID == attackersFamily?.Id || player.Character.FamilyID == defendersFamily?.Id || player.IsAdmin()))
                {
                    UpdateCaptureUi(player, true);
                }
            }
            catch (Exception ex) { _logger.WriteError("onPlayerEnterBizWar: " + ex.Message); }
        }

        private void onPlayerExitBizWar(ColShape shape, Player client)
        {
            try
            {

                if (!(client is ExtPlayer player)) return;

                if (!player.IsLogged())
                    return;
                player.Character.WarZone = BattleLocation.None;
                PlayerExitBattleLocation?.Invoke(player, Id);
                if (warIsGoing)
                {
                    UpdateCaptureUi(player, false);
                }
            }
            catch (Exception ex) { _logger.WriteError("onPlayerExitBizWar: " + ex.Message); }
        }

        public bool CMD_startBizwar(Family familyAttack, Family familyDefend, BattleModel battleModel)
        {
            if (warIsGoing || warStarting)
            {
                return false;
            }
            WarBlip.Color = 49;
            Chat.SendFamilyMessage(familyDefend.Id, $"Внимание! У нас есть 10 минут на сборы! {familyAttack.Name} решили захватить наш бизнес", false);
            Chat.SendFamilyMessage(familyAttack.Id, "Стреляй! Забирай! Примерно через 10 минут приедут противники", false);

            _startCapt = DateTime.Now;

            attackersFamily = familyAttack;
            defendersFamily = familyDefend;
            familyAttack.NextBizWarTime = DateTime.Now.AddMinutes(60); // NEXT BIZWAR

            SendCaptureData((ExtPlayer p) =>
            {
                CaptureUI.SendUntilCaptureTimer(p, FamilyWars.WarManager.TimeToCapt, 0, "До начала битвы");
            }, true);

            _battleModel = battleModel;
            toStartWarTimer = Timers.StartOnce(FamilyWars.WarManager.TimeToCapt * 1000, () => timerStart());
            warStarting = true;
            return true;
        }

        private void timerStart()
        {
            var attackers = attackersFamily?.GetMembersInBattle(Id) ?? 0;
            var defenders = defendersFamily?.GetMembersInBattle(Id) ?? 0;

            SendCaptureData((ExtPlayer p) =>
            {
                CaptureUI.EnableCaptureUI(p, 100, 200, 0, attackers, defenders, false);
                CaptureUI.EnableKillLog(p);
            });
            if (warTimer != null)
                Timers.Stop(warTimer);
            warTimer = Timers.StartOnce(FamilyWars.WarManager.BizwarLength * 1000, EndCapture);
            warStarting = false;
            warIsGoing = true;

            Chat.SendFamilyMessage(defendersFamily.Id, $"Внимание! На нас напали! {attackersFamily.Name} решили захватить наш бизнес", false);
            Chat.SendFamilyMessage(attackersFamily.Id, "Стреляй! Забирай! Вы начали войну за бизнес", false);
        }
        
        private void CheckEndCapture(int attackers, int defenders)
        {
            if (attackers == 0 || defenders == 0)
            {
                EndCapture();
                if (warTimer != null)
                    Timers.Stop(warTimer);
            }
        }

        private void EndCapture()
        {
            try
            {
                if (_battleModel == null)
                    return;
                var attackers = attackersFamily?.GetMembersInBattle(Id) ?? 0;
                var defenders = defendersFamily?.GetMembersInBattle(Id) ?? 0;
                warTimer = null;

                SendCaptureData((ExtPlayer p) =>
                {
                    CaptureUI.DisableCaptureUI(p);
                    CaptureUI.DisableKillog(p, true);
                });

                if (defendersFamily != null)
                    defendersFamily.ProtectBizWarDate = DateTime.Now.AddMinutes(20);

                if (attackersFamily != null)
                    attackersFamily.ProtectBizWarDate = DateTime.Now.AddMinutes(20);
                var familyWinner = attackers <= defenders ? defendersFamily : attackersFamily;
                var familyLose = attackers <= defenders ?  attackersFamily : defendersFamily;
                _battleModel.FinishBattle(BattleStatus.FightIsOver, familyWinner.Id);
                if (attackers <= defenders)
                {
                    Chat.SendFamilyMessage(defendersFamily.Id, "Вы отстояли свой бизнес", false);
                    Chat.SendFamilyMessage(attackersFamily.Id, "Вы проиграли! Враги были сильнее! Вы не смогли захватить бизнес", false);
                }
                else if (attackers > defenders)
                {
                    Chat.SendFamilyMessage(defendersFamily.Id, "Вы потеряли бизнес..", false);
                    Chat.SendFamilyMessage(attackersFamily.Id, "Вы захватили бизнес!", false);
                }
                foreach (var member in FamilyManager.GetFamilyMembers(familyWinner.Id))
                {
                    Wallet.MoneyAdd(member.Character, _winBizMoney, "Захват бизнеса");
                }
                warIsGoing = false;
                WarBlip.Color = 40;
            }
            catch (Exception e) { _logger.WriteError($"EndMafiaWar: " + e.ToString()); }
        }

        private void SendCaptureData(Action<ExtPlayer> action, bool timerToCapt = false)
        {
            if (attackersFamily != null)
                foreach (var member in attackersFamily.OnlineMembers)
                {
                    if (member.Value.IsLogged() && (member.Value.Character.WarZone == Id || timerToCapt))
                    {
                        action.Invoke(member.Value);
                    }
                }
            if (defendersFamily != null)
                foreach (var member in defendersFamily.OnlineMembers)
                {
                    if (member.Value.IsLogged() && (member.Value.Character.WarZone == Id || timerToCapt))
                    {
                        action.Invoke(member.Value);
                    }
                }
            if (!timerToCapt)
                foreach (var admin in Core.ReportSystem.ReportManager.Admins)
                {
                    if (admin.IsLogged() && admin.Character.WarZone == Id)
                    {
                        action.Invoke(admin);
                    }
                }
        }

        public void UpdateCaptureUi(ExtPlayer newPlayer = null, bool enable = true)
        {
            var attackers = attackersFamily?.GetMembersInBattle(Id) ?? 0;
            var defenders = defendersFamily?.GetMembersInBattle(Id) ?? 0;

            if (newPlayer != null)
            {
                if (enable)
                {
                    CaptureUI.EnableCaptureUI(newPlayer, 100, 200, timerCount, attackers, defenders, false);
                    CaptureUI.EnableKillLog(newPlayer);
                }
                else
                {
                    CaptureUI.DisableCaptureUI(newPlayer);
                    CaptureUI.DisableKillog(newPlayer, true);
                }
            }
            SendCaptureData((ExtPlayer p) =>
            {
                CaptureUI.SetCaptureStats(p, attackers, defenders, timerCount);
            });
            CheckEndCapture(attackers, defenders);
        }

        public bool Event_PlayerDeath(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            if (!warIsGoing) 
                return false;
            if (!player.IsLogged())
                return false;

            if (player.Character.FamilyID != defendersFamily.Id && player.Character.FamilyID != attackersFamily.Id) 
                return false;

            SendCaptureData((ExtPlayer p) =>
            {
                CaptureUI.AddKillogItem(p, killer, player, weapon);
            });
            if (killer.Character.FamilyID == defendersFamily.Id || killer.Character.FamilyID == attackersFamily.Id)
            {
                killer.CreatePlayerAction(PersonalEvents.PlayerActions.KillOnCaptAndBizwar, 1);
            }
            UpdateCaptureUi();
            return true;
        }
    }
}
