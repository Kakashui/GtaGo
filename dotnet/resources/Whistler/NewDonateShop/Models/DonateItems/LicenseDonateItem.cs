using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.GUI.Documents;
using Whistler.GUI.Documents.Enums;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class LicenseDonateItem: BaseDonateItem
    {
        public LicenseDonateItem(params LicenseName[] licenses)
        {
            Licenses = licenses.ToList();
        }
        public List<LicenseName> Licenses { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var character = player.Character;
            var lic = "";
            Licenses.ForEach(l =>
            {
                if (player.GiveLic(l))
                    lic += DocumentConfigs.GetLicenseWord(l) + " ";
            });
            if(lic == "")
            {
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:license:err".Translate(), 3000);
                return false;
            }
            else character.Save();
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:license:ok".Translate(lic), 3000);
            return true;
        }
    }
}
