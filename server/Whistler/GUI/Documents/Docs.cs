using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.Fractions;
using System.Linq;

namespace Whistler.GUI.Documents
{
    class Docs : Script
    {
        [Command("showlic")]
        public static void ShowLic(Player player)
        {
            AcceptLicenses(player, player);
        }
        public static void Passport(Player from, Player to)
        {
            Vector3 pos = to.Position;
            if (from.Position.DistanceTo(pos) > 2)
            {
                Notify.Send(from, NotifyType.Error, NotifyPosition.BottomCenter, "Gui_37".Translate(), 3000);
                return;
            }
            DialogUI.Open(to, "Gui_38".Translate(from.Value), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",// yes
                    Icon = "confirm",
                    Action = (p) =>
                    {
                        Notify.Send(p, NotifyType.Info, NotifyPosition.BottomCenter, "Gui_44".Translate( from.Value), 5000);
                        Notify.Send(from, NotifyType.Info, NotifyPosition.BottomCenter, "Gui_45".Translate(p.Value), 5000);
                        AcceptPasport(p, from);
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "gui_728",// no
                    Icon = "cancel",
                }
            });
        }

        public static void Licenses(Player from, Player to)
        {
            Vector3 pos = to.Position;
            if (from.Position.DistanceTo(pos) > 2)
            {
                Notify.Send(from, NotifyType.Error, NotifyPosition.BottomCenter, "Gui_37".Translate(), 3000);
                return;
            }
            DialogUI.Open(to, "Gui_39".Translate(from.Value), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",// yes
                    Icon = "confirm",
                    Action = (p) =>
                    {
                        Notify.Send(p, NotifyType.Info, NotifyPosition.BottomCenter, "Gui_47".Translate( from.Value), 5000);
                        Notify.Send(from, NotifyType.Info, NotifyPosition.BottomCenter, "Gui_48".Translate(p.Value), 5000);
                        AcceptLicenses(p, from);
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "gui_728",// no
                    Icon = "cancel",
                }
            });
        }

        public static void ShowCertificates(Player from, Player to)
        {
            if (from.Position.DistanceTo(to.Position) > 2)
            {
                Notify.Send(from, NotifyType.Error, NotifyPosition.BottomCenter, "Gui_37".Translate(), 3000);
                return;
            }
            DialogUI.Open(to, "gui_792".Translate(from.Value), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",// you
                    Icon = "confirm",
                    Action = (p) =>
                    {
                        if (!from.IsLogged()) return;
                        var character = from.GetCharacter();
                        p.TriggerEvent("certificates:show", JsonConvert.SerializeObject(new { frac = character.FractionID, firstName = character.FirstName, lastName = character.LastName, id = character.UUID, post = Manager.getNickname(character.FractionID, character.FractionLVL) }));
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "gui_728",// no
                    Icon = "cancel",
                }
            });
        }
        public static void AcceptPasport(Player player, Player from)
        {
            var acc = from.GetCharacter();
            string gender = (acc.Customization.Gender) ? "Gui_40" : "Gui_41";
            string fraction = (acc.FractionID > 0 && acc.FractionID != 9) ? Configs.GetConfigOrDefault(acc.FractionID).Name : "Gui_42";
            string work = (acc.WorkID > 0) ? Jobs.WorkManager.JobStats[acc.WorkID] : "Gui_43";
            string partner = (acc.Partner != -1) ? Main.PlayerNames[acc.Partner] : (acc.Customization.Gender ? "Core_333" : "Core_334");
            List<object> data = new List<object>
                    {
                        acc.UUID,
                        acc.FirstName,
                        acc.LastName,
                        acc.CreateDate.ToString("dd.MM.yyyy"),
                        gender,
                        fraction,
                        work,
                        partner
                    };
            string json = JsonConvert.SerializeObject(data);
            Trigger.ClientEvent(player, "passport", json);
            Trigger.ClientEvent(player, "newPassport", from, acc.UUID);
        }
        public static void AcceptLicenses(Player player, Player from)
        {
            var acc = from.GetCharacter();

            Trigger.ClientEvent(player, "licenses", JsonConvert.SerializeObject(acc.Licenses.Where(item => item.IsActive), new JsonSerializerSettings { DateFormatString = "dd-MM-yyyy" }), from.Name, acc.Customization.Gender, acc.CreateDate.ToString("dd.MM.yyyy"));
        }
    }
}
