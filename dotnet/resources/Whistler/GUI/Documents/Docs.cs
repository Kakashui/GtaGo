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
using Whistler.Entities;

namespace Whistler.GUI.Documents
{
    class Docs : Script
    {
        [Command("showlic")]
        public static void ShowLic(ExtPlayer player)
        {
            AcceptLicenses(player, player);
        }
        public static void Passport(ExtPlayer from, ExtPlayer to)
        {
            Vector3 pos = to.Position;
            if (from.Position.DistanceTo(pos) > 2)
            {
                Notify.Send(from, NotifyType.Error, NotifyPosition.BottomCenter, "Gui_37".Translate(), 3000);
                return;
            }
            DialogUI.Open(to, "Gui_38".Translate(from.Character.UUID), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",// yes
                    Icon = "confirm",
                    Action = (p) =>
                    {
                        Notify.Send(p, NotifyType.Info, NotifyPosition.BottomCenter, "Gui_44".Translate(from.Character.UUID), 5000);
                        Notify.Send(from, NotifyType.Info, NotifyPosition.BottomCenter, "Gui_45".Translate(p.Character.UUID), 5000);
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

        public static void Licenses(ExtPlayer from, ExtPlayer to)
        {
            Vector3 pos = to.Position;
            if (from.Position.DistanceTo(pos) > 2)
            {
                Notify.Send(from, NotifyType.Error, NotifyPosition.BottomCenter, "Gui_37".Translate(), 3000);
                return;
            }
            DialogUI.Open(to, "Gui_39".Translate(from.Character.UUID), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",// yes
                    Icon = "confirm",
                    Action = (p) =>
                    {
                        Notify.Send(p, NotifyType.Info, NotifyPosition.BottomCenter, "Gui_47".Translate(from.Character.UUID), 5000);
                        Notify.Send(from, NotifyType.Info, NotifyPosition.BottomCenter, "Gui_48".Translate(p.Character.UUID), 5000);
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

        public static void ShowCertificates(ExtPlayer from, ExtPlayer to)
        {
            if (from.Position.DistanceTo(to.Position) > 2)
            {
                Notify.Send(from, NotifyType.Error, NotifyPosition.BottomCenter, "Gui_37".Translate(), 3000);
                return;
            }
            DialogUI.Open(to, "gui_792".Translate(from.Character.UUID), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "gui_727",// you
                    Icon = "confirm",
                    Action = (p) =>
                    {
                        if (!from.IsLogged()) return;
                        var character = from.Character;
                        SafeTrigger.ClientEvent(p, "certificates:show", JsonConvert.SerializeObject(new { frac = character.FractionID, firstName = character.FirstName, lastName = character.LastName, id = character.UUID, post = Manager.getNickname(character.FractionID, character.FractionLVL) }));
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "gui_728",// no
                    Icon = "cancel",
                }
            });
        }
        public static void AcceptPasport(ExtPlayer player, ExtPlayer from)
        {
            var acc = from.Character;
            string gender = (acc.Customization.Gender) ? "Мужчина" : "Женщина";
            string fraction = ((acc.FractionID >= 6 && acc.FractionID <= 9) || (acc.FractionID >= 14 && acc.FractionID <= 17)) ? Configs.GetConfigOrDefault(acc.FractionID).Name : "Нет";
            string work = (acc.WorkID > 0) ? Jobs.WorkManager.JobStats[acc.WorkID-1] : "Безработный";
            string partner = (acc.Partner != -1) ? Main.PlayerNames[acc.Partner] : (acc.Customization.Gender ? "Холост" : "Не замужем");
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
            SafeTrigger.ClientEvent(player, "passport", json);
            SafeTrigger.ClientEvent(player, "newPassport", from, acc.UUID);
        }
        public static void AcceptLicenses(ExtPlayer player, ExtPlayer from)
        {
            var acc = from.Character;

            SafeTrigger.ClientEvent(player, "licenses", JsonConvert.SerializeObject(acc.Licenses.Where(item => item.IsActive), new JsonSerializerSettings { DateFormatString = "dd-MM-yyyy" }), from.Name, acc.Customization.Gender, acc.CreateDate.ToString("dd.MM.yyyy"));
        }
    }
}
