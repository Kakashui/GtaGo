using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper.Internal;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Families;
using Whistler.Families.Models;
using Whistler.Fractions.GOV.Config;
using Whistler.Fractions.GOV.DTO;
using Whistler.Fractions.GOV.Models;
using Whistler.GUI;
using Whistler.GUI.Documents;
using Whistler.GUI.Documents.Enums;
using Whistler.GUI.Lifts;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.Fractions.GOV
{
    class Government : Script
    {
        public static Vector3 BlipPosition = new Vector3(2475, -384, 95);

        private static List<Vector3> _clothesPoints = new List<Vector3>
        {
            new Vector3(2526.769, -436.2271, 105.7386),
            new Vector3(2501.637, -411.1296, 105.7387),
            new Vector3(2501.518, -357.3382, 105.7362),
            new Vector3(2526.576, -332.1549, 105.7362),
        };
        private static List<InteractShape> _shapes = new List<InteractShape>();
        private static List<Complaint> _complaints = new List<Complaint>();
        private static int _nameChangePrice = 500000;
        private static int _createFamilyPrice = 1000000;

        private static List<Philanthropist> _philanthropists = new List<Philanthropist>();
        public Government()
        {
            foreach (var pos in Consignments.VotePositions)
            {
                _shapes.Add(InteractShape.Create(pos, 1, 2, 0)
                    .AddInteraction(OpenGovernmentMenu, "interact_16"));
            }
            var leftLift = Lift.Create()
                .AddFloor("input", LiftsConfig.LeftInputLift, marker: false, exit: false)
                .AddFloor("1 floor", LiftsConfig.LeftExitLift, LiftsConfig.LeftExitLiftRot, marker: false, input: false)
                .AddFloor("input", LiftsConfig.LeftInputLiftUp, dimension: NAPI.GlobalDimension, marker: false, exit: false);
            LiftsConfig.LeftExitLiftUp.ForEach(model =>
            {
                leftLift.AddFloor(model.Name, model.Position, model.Rotation, dimension: model.Dimension, marker: false, exitPredicate: (ExtPlayer p) => CheckExitAccess(p, model.Access), input: false);
            });
            var rightLift = Lift.Create()
                .AddFloor("input", LiftsConfig.RightInputLift, marker: false, exit: false)
                .AddFloor("1 floor", LiftsConfig.RightExitLift, LiftsConfig.RightExitLiftRot, marker: false, input: false)
                .AddFloor("input", LiftsConfig.RightInputLiftUp, dimension: NAPI.GlobalDimension, marker: false, exit: false);
            LiftsConfig.RightExitLiftUp.ForEach(model =>
            {
                rightLift.AddFloor(model.Name, model.Position, model.Rotation, dimension: model.Dimension, marker: false, exitPredicate: (ExtPlayer p) => CheckExitAccess(p, model.Access), input: false);
            });


            foreach (var point in _clothesPoints)
            {
                InteractShape.Create(point, 1, 2, NAPI.GlobalDimension)
                    .AddDefaultMarker(Configs.GetConfigOrDefault(17).FracColor)
                    .AddInteraction((player) =>
                    {
                        interactPressedClothes(player);
                    }, "interact_3");                
            }

            LoadPhilanthropists();
        }

        public static void SendGovernmentNotify(ExtPlayer player, string title, string message)
        {
            SafeTrigger.ClientEvent(player,"government:sendNotify", title, message);
        }

        private static void LoadPhilanthropists()
        {

            DataTable result = MySQL.QueryRead("SELECT * FROM `philanthropists` WHERE `time` > @prop0", MySQL.ConvertTime(DateTime.Now.AddMonths(-1)));
            if (result == null || result.Rows.Count == 0)
            {
                return;
            }
            foreach (DataRow row in result.Rows)
            {
                int uuid = Convert.ToInt32(row["uuid"]);
                int amount = Convert.ToInt32(row["amount"]);
                var philanthrop = _philanthropists.FirstOrDefault(item => item.Uuid == uuid);
                if (philanthrop != null)
                    philanthrop.Amount += amount;
                else
                    _philanthropists.Add(new Philanthropist(uuid, amount));
            }
            _philanthropists.Sort(new CompPhilanthropist<Philanthropist>());
        }

        private static void LoadTopPhilanthropists(ExtPlayer player, int countBest)
        {
            if (countBest <= 0)
                return;
            var philanthropists = _philanthropists
                .Where(item => item.Amount > (_philanthropists.Count >= countBest ? _philanthropists[countBest - 1].Amount : 0))
                .Select(item => new { name = Main.PlayerNames.GetOrDefault(item.Uuid), donate = item.Amount.ToString() });
            SafeTrigger.ClientEvent(player,"government:loadPhilanthropistsList", JsonConvert.SerializeObject(philanthropists));
        }

        private static void interactPressedClothes(ExtPlayer player)
        {
            if (player.Character.FractionID == 17)
                SkinManager.OpenSkinMenu(player);
            else
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_522", 3000);
        }

        private static bool CheckExitAccess(ExtPlayer player, int access)
        {
            return player.Character.FractionID == 17 || player.IsAdmin();
        }

        public static void OpenGovernmentMenu(Whistler.Entities.ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            List<SocialStatusDTO> socialStatus = new List<SocialStatusDTO>
            {
                new SocialStatusDTO()
                {
                    desc = "cityHall_1",
                    values = new List<string>
                    {
                        "cityHall_1",
                        player.Character.GetPartnerName()
                    }
                },
                new SocialStatusDTO()
                {
                    desc = "cityHall_3",
                    values = player.GetLicensesList().Select(item => "cityHall_4".Translate(item)).ToList()
                }
            };
            if (player.Character.QuestStage == StartQuest.StartQuestNames.Stage4InspectTheDisplay && player.StartQuestTempParam == 0)
            {
                player.StartQuestTempParam = 1;
                player.DeleteClientMarker(781);
                player.DeleteClientMarker(780);
            }
            SafeTrigger.ClientEvent(player,"government:openVoteMenu",
                player.Character.UUID,
                JsonConvert.SerializeObject(new 
                { 
                    voteDone = DateTime.Now > Consignments.FinishVoteDate, 
                    currentVote = player.Character.LastVote, 
                    finishDate = Consignments.FinishVoteDate.ToString("F"), 
                    partiesList = Consignments.ConsignmentVotes.Select(item => item.Value.GetConsignmentVoteDTO()) 
                }),
                JsonConvert.SerializeObject(socialStatus));
            LoadTopPhilanthropists(player, 10);
        }
        [RemoteEvent("government:sendVote")]
        public static void RemoteEvent_SendVote(ExtPlayer player, int vote)
        {
            if (!player.IsLogged())
                return;
            if (Consignments.ConsignmentVotes.ContainsKey(vote))
            {
                MySQL.Query("UPDATE `characters` SET `lastvote` = @prop0 WHERE `uuid` = @prop1", vote, player.Character.UUID);
                player.Character.LastVote = vote;
                SafeTrigger.ClientEvent(player,"government:setVote", player.Character.LastVote);
            }
        }
        [RemoteEvent("government:createComplaint")]
        public static void RemoteEvent_CreateComplaint(ExtPlayer player, int fraction, string name, string text)
        {
            if (!player.IsLogged())
                return;
            var target = Main.PlayerNames.FirstOrDefault(item => item.Value == name);
            if (target.Value == null)
            {
                SendGovernmentNotify(player, "cityHall_5", "cityHall_6");
                return;
            }
            _complaints.Add(new Complaint(player.Character.UUID, fraction, target.Key, text));
            SendGovernmentNotify(player, "cityHall_7", "cityHall_8");
        }
        [RemoteEvent("government:createRecord")]
        public static void RemoteEvent_CreateRecord(ExtPlayer player, string employee, string text)
        {
            if (!player.IsLogged())
                return;

        }
        [RemoteEvent("government:changeName")]
        public static void RemoteEvent_ChangeName(ExtPlayer player, string newName, int paymentType)
        {
            if (!player.IsLogged())
                return;

            if (!Wallet.TryChange(player.GetMoneyPayment((PaymentsType)paymentType), -_nameChangePrice))
            {
                SendGovernmentNotify(player, "cityHall_5", "cityHall_9");
                return;
            }
            var result = Character.ChangeName(player.Character.FullName, newName);
            switch (result)
            {
                case ChangeNameResult.Success:
                    Wallet.TransferMoney(player.GetMoneyPayment((PaymentsType)paymentType), Manager.GetFraction(6), _nameChangePrice, 0, "Money_ChangeName");
                    break;
                case ChangeNameResult.BadCurrentName:
                    break;
                case ChangeNameResult.IncorrectNewName:
                    SendGovernmentNotify(player, "cityHall_5", "cityHall_10");
                    break;
                case ChangeNameResult.NewNameIsExist:
                    SendGovernmentNotify(player, "cityHall_5", "cityHall_11");
                    break;
            }
        }
        [RemoteEvent("government:createFamily")]
        public static void RemoteEvent_CreateFamily(ExtPlayer player, string familyName, int paymentType, string leaders, string nation)
        {
            if (!player.IsLogged())
                return;
            if (!player.CheckInviteToFamily(null))
            {
                SendGovernmentNotify(player, "cityHall_5", "Fam_45");
                return;
            }
            if (FamilyManager.CreateFamily(player, familyName, _createFamilyPrice, (PaymentsType)paymentType, 
                (ExtPlayer player, bool success, string message) => 
                {
                    if (success)
                        SendGovernmentNotify(player, "cityHall_12", message);
                    else
                        SendGovernmentNotify(player, "cityHall_5", message);
                }))
            {
                Family family = player.GetFamily();
                family.ChangeNation(nation);
                List<string> leadersList = JsonConvert.DeserializeObject<List<string>>(leaders);
                foreach (var item in leadersList)
                {
                    ExtPlayer target = Trigger.GetPlayerByName(item) as ExtPlayer;
                    if (!target.IsLogged())
                        continue;
                    if (!target.CheckInviteToFamily(family))
                        continue;
                    FamilyManager.InvitePlayerToFamily(target, family, 0);
                }
            }
        }
        [RemoteEvent("government:donateToGov")]
        public static void RemoteEvent_DonateToGov(ExtPlayer player, int amount, int type, bool menuGov)
        {
            if (!player.IsLogged())
                return;
            if (Wallet.MoneySub(player.Character, amount, "Money_Philanthrop"))
            {
                int uuid = player.Character.UUID;
                MySQL.Query("INSERT INTO `philanthropists` (`uuid`,`amount`,`time`, `target`) VALUES (@prop0, @prop1, @prop2, @prop3)", uuid, amount, MySQL.ConvertTime(DateTime.Now), type);
                var philanthrop = _philanthropists.FirstOrDefault(item => item.Uuid == uuid);
                if (philanthrop != null)
                    philanthrop.Amount += amount;
                else
                    _philanthropists.Add(new Philanthropist(uuid, amount));
                _philanthropists.Sort(new CompPhilanthropist<Philanthropist>());
                LoadTopPhilanthropists(player, 10);
                if (menuGov)
                {
                    SendGovernmentNotify(player, "cityHall_12", "cityHall_13".Translate(amount));
                    player.CreatePlayerAction(PersonalEvents.PlayerActions.DonateInGOV, amount);
                }
                else
                {
                    Notify.SendSuccess(player, "cityHall_13".Translate(amount));
                    player.CreatePlayerAction(PersonalEvents.PlayerActions.DonateInBank, amount);
                }
            }
        }
        [RemoteEvent("government:buyLicense")]
        public static void RemoteEvent_BuyLicense(ExtPlayer player, int typeLic, int typePay)
        {
            if (!player.IsLogged())
                return;
            if (typeLic < 9)
                return;
            if (player.Character.Mulct > 0)
            {
                SendGovernmentNotify(player, "cityHall_5", "cityHall_17");
                return;
            }
            if (!Enum.IsDefined(typeof(LicenseName), typeLic)) return;
            LicenseName license = (LicenseName)typeLic;
            var price = DocumentConfigs.GetLicensePrice(license);
            if (player.CheckLic(license))
            {
                SendGovernmentNotify(player, "cityHall_5", "cityHall_14");
                return;
            }
            if (Wallet.TransferMoney(player.GetMoneyPayment((PaymentsType)typePay), Manager.GetFraction(6), price, 0, "Money_BuyLic".Translate(typeLic)))
            {
                player.GiveLic(license);
                MainMenu.SendStats(player);
                SendGovernmentNotify(player, "cityHall_12", "cityHall_15");
            }
            else
            {
                SendGovernmentNotify(player, "cityHall_5", "cityHall_16");
            }
        }
    }

}
