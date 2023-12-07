
using GTANetworkAPI;
using System.Collections.Generic;
using System;
using Whistler.SDK;
using Whistler.Fractions;
using Whistler.Core;
using Whistler.MoneySystem;
using Whistler.Helpers;
using Whistler.Entities;
using Whistler.Core.QuestPeds;

namespace Whistler.Marriage
{
    public class Marriage : Script
    {
        private static int _marriageDuty = 100000;
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Marriage));


        private List<Vector3> PriestsPoint = new List<Vector3>()
        {
            new Vector3(-777.173, 20.87795, 39.52997)
        };

        private List<Priest> PriestsShape;


        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                PriestsShape = new List<Priest>();
                for (int id = 0; id < PriestsPoint.Count; id++)
                {
                    PriestsShape.Add(new Priest(id, PriestsPoint[id]));
                }
            }
            catch (Exception ex) { _logger.WriteError("onResourceStart: " + ex.ToString()); }

        }

        [RemoteEvent("marriage:callbackInvite")]
        public static void Marriage_callbackInvite(ExtPlayer player, string name)
        {
            try
            {
                ExtPlayer target = Trigger.GetPlayerByName(name);
                if (target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_63", 3000);
                    return;
                }
                if (!target.HasData("PRIEST_ID") || !player.HasData("PRIEST_ID") || target.GetData<int>("PRIEST_ID") != player.GetData<int>("PRIEST_ID"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_64", 3000);
                    return;
                }
                if (target.GetGender())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_65", 3000);
                    return;
                }
                if (target.Character.Partner != -1)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_66", 3000);
                    return;
                }
                SafeTrigger.SetData(target, "Marriage:partner", player);
                SafeTrigger.ClientEvent(target, "marriage:proposal", player.Name.Replace('_',' '));
            }
            catch (Exception ex) { _logger.WriteError("Marriage_callbackInvite: " + ex.ToString()); }

        }

        [RemoteEvent("marriage:callbackProposal")]
        public static void Marriage_callbackProposal(ExtPlayer target, bool assent)
        {
            try
            {
                if (!target.HasData("Marriage:partner"))
                {
                    Notify.Send(target, NotifyType.Error, NotifyPosition.BottomCenter, "local_67", 3000);
                    return;
                }
                ExtPlayer player = target.GetData<ExtPlayer>("Marriage:partner");
                if (!player.IsLogged())
                {
                    Notify.Send(target, NotifyType.Error, NotifyPosition.BottomCenter, "local_68", 3000);
                    return;
                }
                if (!assent)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_69", 3000);
                    return;
                }
                if (!target.HasData("PRIEST_ID") || !player.HasData("PRIEST_ID") || target.GetData<int>("PRIEST_ID") != player.GetData<int>("PRIEST_ID"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_70", 3000);
                    Notify.Send(target, NotifyType.Error, NotifyPosition.BottomCenter, "local_70", 3000);
                    return;
                }
                if (!Wallet.TransferMoney(player.Character, Manager.GetFraction(6), _marriageDuty, 0, "Оплата за свадьбу"))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_71", 3000);
                    return;
                }
                player.Character.SetPartner(player, target);
                target.Character.SetPartner(target, player);
                player.CreatePlayerAction(PersonalEvents.PlayerActions.Mariage, 1);
                target.CreatePlayerAction(PersonalEvents.PlayerActions.Mariage, 1);
                Chat.AdminToAll( "Com_143".Translate( player.Name.Replace('_', ' '), target.Name.Replace('_', ' ')));
            }
            catch (Exception ex) { _logger.WriteError("Marriage_callbackInvite: " + ex.ToString()); }

        }

        public static void InteractMerriage(ExtPlayer player)
        {
            try
            {
                if (!player.GetGender())
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_72", 3000);
                    return;
                }
                if (player.Character.Partner != -1)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_73", 3000);
                    return;
                }
                if (player.Character.Money < _marriageDuty)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_74", 3000);
                    return;
                }
                SafeTrigger.ClientEvent(player, "marriage:invite");
            }
            catch (Exception ex) { _logger.WriteError("interactPressed: " + ex.ToString()); }
        }

        [Command("admdivorce")]
        public static void AdminDivorce(ExtPlayer player, int targetID)
        {
            if (Group.CanUseAdminCommand(player, "admdivorce"))
                return;
            ExtPlayer target = Trigger.GetPlayerByUuid(targetID);
            if (target.IsLogged())
                Divorce(target);
        }

        public static void Divorce(ExtPlayer player)
        {
            try
            {
                int partner = player.Character.Partner;
                if (partner == -1)
                    return;
                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, "local_75".Translate( Main.PlayerNames[partner]), 3000);
                player.Character.Partner = -1; 
                ExtPlayer target = Trigger.GetPlayerByUuid(partner);
                if (target != null)
                {
                    target.Character.Partner = -1;
                    Notify.Send(target, NotifyType.Info, NotifyPosition.BottomCenter, "local_76".Translate(player.Name), 3000);
                }
                MySQL.Query("UPDATE characters SET partner = @prop1 WHERE uuid = @prop0", partner, -1);      
                MySQL.Query("UPDATE characters SET partner = @prop1 WHERE uuid = @prop0", player.Character.UUID, -1);          
            }
            catch (Exception ex) { _logger.WriteError("interactPressed: " + ex.ToString()); }
        }


    }

    internal class Priest
    {
        private int id;
        private InteractShape shape;
        public static QuestPedParamModel divorcePed = new QuestPedParamModel(PedHash.Business03AFY, new Vector3(-774.23035, 32.036896, 40.646904), "Erin Bennett", "Незнакомка", 160, 0, 2);
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Priest));

        public Priest(int id, Vector3 pos)
        {
            this.id = id;

            shape = InteractShape.Create(pos, 5, 3)
                .AddOnEnterColshapeExtraAction((c, p) => p.SetData("PRIEST_ID", id))
                .AddOnExitColshapeExtraAction((c, p) => p.ResetData("PRIEST_ID"))
                .AddInteraction((player) =>
                {
                    Marriage.InteractMerriage(player);
                });

            NAPI.Blip.CreateBlip(489, pos, 1, 255, "ЗАГС", 255, 100, true, 0, 0);

            var ped = new QuestPed(divorcePed);
            ped.PlayerInteracted += (player, ped) =>
            {
                try
                {
                    var introPage = new DialogPage("", ped.Name, ped.Role);
                    introPage.AddAnswer("Хочу развестись", Marriage.Divorce);
                    introPage.AddCloseAnswer("Досвидания");
                    introPage.OpenForPlayer(player);
                }
                catch (Exception e)
                {
                    _logger.WriteError("divorcePed: " + e.ToString());
                }
            };
        }
    }
}