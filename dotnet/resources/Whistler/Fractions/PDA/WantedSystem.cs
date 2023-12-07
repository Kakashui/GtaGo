using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.GUI.Documents;
using Whistler.GUI.Documents.Models;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.Fractions.PDA
{
    static class WantedSystem
    {
        public static void UpdatePlayerWantedLevel(ExtPlayer player, int level)
        {
            if (!player.IsLogged())
                return;
            SafeTrigger.ClientEvent(player, "setWanted", level);
        }
        public static void SetPlayerWantedLevel(ExtPlayer player, ExtPlayer whoGive, int lvl, string reason)
        {
            if (!player.IsLogged()) return;

            Character character = player.Character;
            if (lvl <= 0) 
            {
                lvl = 0;
                character.WantedLVL = null;
            }
            else
            {
                if (character.WantedLVL == null)
                {
                    character.WantedLVL = new WantedLevel(lvl, whoGive == null ? "none" : whoGive.Name, DateTime.Now, reason);
                }    
                else
                {
                    character.WantedLVL.Level = lvl;
                    character.WantedLVL.Reason = reason;
                    character.WantedLVL.WhoGive = whoGive == null ? "none" : whoGive.Name;
                }
            }
            UpdatePlayerWantedLevel(player, lvl);
            SubscribeToPda.TriggerEventToSubscribers(PDAConstants.UPDATE_PLAYER_WANTED_LEVEL, GetSearchPlayer(character));
        }
        public static void SetVehicleWantedLevel(PersonalBaseVehicle vData, Player whoGive, int lvl, string reason)
        {
            if (lvl <= 0)
            {
                lvl = 0;
                vData.WantedLVL = null;
            }
            else
            {
                if (vData.WantedLVL == null)
                    vData.WantedLVL = new WantedLevel(lvl, whoGive.Name, DateTime.Now, reason);
                else
                {
                    vData.WantedLVL.Level = lvl;
                    vData.WantedLVL.Reason = reason;
                    vData.WantedLVL.WhoGive = whoGive.Name;
                }
            }
            SubscribeToPda.TriggerEventToSubscribers(PDAConstants.UPDATE_VEHICLE_WANTED_LEVEL, GetSearchVehicle(vData));
        }
        public static string GetLicenses(List<License> licList)
        {
            var lic = "";
            foreach (var item in licList.Where(item => item.IsActive))
            {
                lic += $"{DocumentConfigs.GetLicenseWord(item.Name)} ";
            }
            if (lic == "") lic = "Gui_33";
            return lic;
        }
        public static string GetJsonWantedPlayers()
        {
            var list = Main.GetExtPlayersListByPredicate((player) => player.Character.WantedLVL != null);
            return JsonConvert.SerializeObject(list.Select(item => new { name = item.Name, passport = item.Character.UUID, sex = item.Character.Customization.Gender, number = item.GetPhone()?.Phone?.SimCard.Number, wantedLevel = item.Character.WantedLVL.Level, licenses = GetLicenses(item.Character.Licenses)}));
        }

        public static string GetJsonWantedVehicle()
        {
            var vehicles = VehicleManager.Vehicles.Where(veh => veh.Value is PersonalBaseVehicle && (veh.Value as PersonalBaseVehicle).WantedLVL != null);
            return JsonConvert.SerializeObject(vehicles.Select(item => new { name = item.Value.GetHolderName(), number = item.Value.Number, carModel = item.Value.ModelName, wantedLevel = (item.Value as PersonalBaseVehicle).WantedLVL.Level, key = item.Value.ID }));
        }

        public static string GetSearchPlayer(Character character)
        {
            return JsonConvert.SerializeObject(new { name = character.FullName, passport = character.UUID, sex =character.Customization.Gender, number = character.PhoneTemporary.Phone.SimCard.Number, wantedLevel = character.WantedLVL?.Level ?? 0, licenses = GetLicenses(character.Licenses) });
        }
        public static string GetSearchVehicle(PersonalBaseVehicle vData)
        {
            return JsonConvert.SerializeObject(new { name = vData.GetHolderName(), number = vData.Number, carModel = vData.ModelName, wantedLevel = vData.WantedLVL?.Level ?? 0, key = vData.ID });
        }
    }
}
