using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Helpers;
using Whistler.SDK;
using Newtonsoft.Json;
using Whistler.Customization.Models;
using Whistler.Core;
using Whistler.Entities;

namespace Whistler.Customization
{
    class CustomizationEvents : Script
    {
        private WhistlerLogger _logger = new WhistlerLogger(typeof(CustomizationEvents));
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            CustomizationService.InitDB();
            CustomizationService.ParseTattoo();
        }

        [Command("applycustomiz")]
        public void ApplayCharacter(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "develop")) return;
            player.Character.Customization.Apply(player);
        }

        [RemoteEvent("customization:save")]
        public void ClientEvent_saveCharacter(
            ExtPlayer player,
            int slot,
            string firstName,
            string lastName,
            bool gender,
            int eyeColor,
            string parentsJson,
            string hairsJson,
            string featuresJson,
            string overlayJson,
            string clothesJson
        )
        {
            try
            {
                if (player == null) return;
                string name="";
                if(slot >= 0)
                {
                    if (!Character.NameIsCorrect(firstName, lastName))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Имя/фамилия должны иметь от 3 до 25 символов A-Z, a-z", 3000);
                        return;
                    }

                    firstName = char.ToUpper(firstName[0]) + firstName.Substring(1).ToLower();
                    lastName = char.ToUpper(lastName[0]) + lastName.Substring(1).ToLower();
                    name = $"{firstName}_{lastName}";
                    if (Main.PlayerNames.ContainsValue(name))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Данное имя уже занято", 3000);
                        return;
                    }
                }
                
                var parents = JsonConvert.DeserializeObject<ParentsDTO>(parentsJson);
                var hairs = JsonConvert.DeserializeObject<HairsDTO>(hairsJson);
                var faceFeatures = JsonConvert.DeserializeObject<List<FaceFeatureDTO>>(featuresJson);
                var headOverlay = JsonConvert.DeserializeObject <Dictionary<int, HeadOverlayDTO>>(overlayJson);
                var clothes = JsonConvert.DeserializeObject<ClothesDTO>(clothesJson);
                if (slot < 0)
                {
                    CustomizationService.ChangeCharacterCustomization(player, gender, (byte)eyeColor, parents, faceFeatures, headOverlay, hairs);
                } else
                    CustomizationService.CreateNewCharacter(player, slot, firstName, lastName, gender, (byte)eyeColor, parents, faceFeatures, headOverlay, hairs, new List<Decoration>(), clothes);
            }catch(Exception e)
            {
                _logger.WriteError($"SaveCharactre:\n{e}");
            }
        }
    }
}
