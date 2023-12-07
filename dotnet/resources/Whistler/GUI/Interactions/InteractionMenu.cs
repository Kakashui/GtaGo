using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.GUI.Documents;
using Whistler.GUI.Documents.Enums;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.GUI.Interactions
{
    internal class InteractionMenu : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(InteractionMenu));
        private List<InteractionMenuPage> _playersInteractionPages = new List<InteractionMenuPage>();
        
        private InteractionMenuPage _vehiclesInteractionPage;
        private InteractionMenuPage _pedInteractionPage;
        private static Vector3 SellPosition = new Vector3(144.1654, -3001.976, 6.061024);

        public InteractionMenu()
        {

            InteractionMenuPage documentsInteractionsWithPlayer = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("cef_80", "driver-license", Docs.Licenses))
                .AddItem(new InteractionMenuPageItem("cef_90", "passport-card", Docs.Passport));

            var familyInteractionsWithPlayer = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("cef_98", "thief-fam", FractionCommands.RobberyTarget),
                    p => (p.GetFamily()?.OrgActiveType ?? OrgActivityType.Unknown) == OrgActivityType.Crime)
                .AddItem(new InteractionMenuPageItem("intMenu_12", "balaclava-take-fam", Selecting.TakeMask),
                    p => (p.GetFamily()?.OrgActiveType ?? OrgActivityType.Unknown) == OrgActivityType.Crime)
                .AddItem(new InteractionMenuPageItem("Пригласить в семью", "family-invite", Selecting.InvitePlayerToFamily),
                    p => Families.FamilyManager.CanAccessToMemberManagement(p));

            var fractionInteractionWithPlayer = new InteractionMenuPage()
                //.AddItem(new InteractionMenuPageItem("intMenu_14", "plastic-surgery", Ems.SendCreator),
                //    p => Manager.canUseCommand(p, "plasticsurgery", false))
                .AddItem(new InteractionMenuPageItem("intMenu_13", "certificates", Docs.ShowCertificates),
                    p => Manager.IsGovFraction(p))
                .AddItem(new InteractionMenuPageItem("cef_78", "show-driver-license", Docs.AcceptLicenses),
                    p => Manager.IsSilovic(p))
                .AddItem(new InteractionMenuPageItem("cef_77", "show-passport-card", Docs.AcceptPasport),
                    p => Manager.IsSilovic(p))
                .AddItem(new InteractionMenuPageItem("intMenu_7", "to-kpz", FractionCommands.arrestTarget),
                    p => Manager.CanUseCommand(p, "arrest", false))
                .AddItem(new InteractionMenuPageItem("intMenu_8", "release-kpz", FractionCommands.releasePlayerFromPrison),
                    p => Manager.CanUseCommand(p, "rfp", false))
                .AddItem(new InteractionMenuPageItem("cef_88", "arrest", FractionCommands.targetFollowPlayer),
                    p => Manager.CanUseCommand(p, "follow", false))
                .AddItem(new InteractionMenuPageItem("intMenu_12", "balaclava-take", Selecting.TakeMask),
                    p => Manager.IsSilovic(p) || Manager.IsGang(p))
                .AddItem(new InteractionMenuPageItem("cef_98", "thief", FractionCommands.RobberyTarget),                    
                    p => new [] { 1, 2, 3, 4, 5, 10, 11, 12, 13, 16 }.ToList().Contains(p.Character.FractionID))
                .AddItem(new InteractionMenuPageItem("cef_98_1", "unarrest", Selecting.Unarrest),
                    p => Manager.CanUseCommand(p, "uncuff", false))
                .AddItem(new InteractionMenuPageItem("intMenu_9", "frisk", Weapons.OpenFriskMenu),
                    p => Manager.CanUseCommand(p, "takeguns", false))
                .AddItem(new InteractionMenuPageItem("cef_92", "sell-drug", Selecting.OfferSellMedKit),
                    p => Manager.IsMedic(p))
                .AddItem(new InteractionMenuPageItem("cef_440", "overheal", Selecting.OfferHealTarget),
                    p => Manager.IsMedic(p))                
                .AddItem(new InteractionMenuPageItem("cef_101", "tax", Selecting.MakePenalty),
                    p => Manager.CanUseCommand(p, "ticket", false))
                .AddItem(new InteractionMenuPageItem("cef_104", "cuff", Selecting.ToPrison),
                    p => PrisonFib.CanUsePrisonFib(p, false))
                .AddItem(new InteractionMenuPageItem("intMenu_3", "take-gun-lic", FractionCommands.TakeGunLic), 
                    p => Manager.CanUseCommand(p, "takegunlic", false))
                .AddItem(new InteractionMenuPageItem("intMenu_10".Translate("A"), "take-A-lic", (ExtPlayer player, ExtPlayer target) => FractionCommands.TakeDriveLic(player, target, LicenseName.Moto)), 
                    p => Manager.CanUseCommand(p, "takedrivelic", false))
                .AddItem(new InteractionMenuPageItem("intMenu_10".Translate("B"), "take-B-lic", (ExtPlayer player, ExtPlayer target) => FractionCommands.TakeDriveLic(player, target, LicenseName.Auto)), 
                    p => Manager.CanUseCommand(p, "takedrivelic", false))
                .AddItem(new InteractionMenuPageItem("intMenu_10".Translate("C"), "take-C-lic", (ExtPlayer player, ExtPlayer target) => FractionCommands.TakeDriveLic(player, target, LicenseName.Truck)), 
                    p => Manager.CanUseCommand(p, "takedrivelic", false))
                .AddItem(new InteractionMenuPageItem("intMenu_4", "give-gun-lic", Selecting.GiveGunLicense), 
                    p => Manager.CanUseCommand(p, "givegunlic", false))
                .AddItem(new InteractionMenuPageItem("Пригласить в организацию", "invite", FractionCommands.InviteToFraction), 
                    p => Manager.CanUseCommand(p, "invite", false));


            var interactionsWithPlayer = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("cef_79", "handshake", Selecting.playerHandshakeTarget))
                .AddItem(new InteractionMenuPageItem("cef_90_1", "document", documentsInteractionsWithPlayer))
                .AddItem(new InteractionMenuPageItem("cef_85", "give-money", Selecting.OpenMoneyTransferMenu))
                //.AddItem(new InteractionMenuPageItem("cef_84", "exchange", Selecting.SuggestOffer))
                .AddItem(new InteractionMenuPageItem("Организация", "community", fractionInteractionWithPlayer))
                .AddItem(new InteractionMenuPageItem("Семья", "family", familyInteractionsWithPlayer))
                .AddItem(new InteractionMenuPageItem("cef_86", "heal", Ems.PlayerHealTarget))
                .AddItem(new InteractionMenuPageItem("intMenu_1", "tocar", FractionCommands.playerInCar),
                    p => Manager.CanUseCommand(p, "incar", false))
                .AddItem(new InteractionMenuPageItem("cef_86_2", "house-sell", (ExtPlayer player, ExtPlayer target) =>
                {
                    SafeTrigger.SetData(player, "SELECTEDPLAYER", target);

                    List<DialogUI.ButtonSetting> buttons = new List<DialogUI.ButtonSetting>();
                    if (HouseManager.GetHouse(player, true) != null)
                    {
                        buttons.Add(new DialogUI.ButtonSetting
                        {
                            Name = "Личный",
                            Icon = null,
                            Action = (p) => player.OpenInput("Core1_26", "Core1_29", 100, "house_sell"),
                        });
                    }
                    if (HouseManager.GetHouseFamily(player) != null)
                    {
                        buttons.Add(new DialogUI.ButtonSetting
                        {
                            Name = "Семейный",
                            Icon = null,
                            Action = (p) => player.OpenInput("Core1_26", "Core1_29", 100, "familyhouse_sell"),
                        });
                    }
                    buttons.Add(new DialogUI.ButtonSetting
                    {
                        Name = "Отмена",
                        Icon = null,
                        Action = (p) => { },
                    });
                    DialogUI.Open(player, "Выберите, какой дом хотите продать.", buttons);
                    
                }), p => HouseManager.GetHouse(p, true) != null || HouseManager.GetHouseFamily(p) != null);

            _playersInteractionPages.Add(documentsInteractionsWithPlayer);
            _playersInteractionPages.Add(fractionInteractionWithPlayer);
            _playersInteractionPages.Add(interactionsWithPlayer);

            InteractionMenuPage carIntercationsPlayers = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("Сбоку от водителя", "carpassengers_1", (p, v) =>
                    VehicleManager.EjectPassengers(p, v, 1)))

                .AddItem(new InteractionMenuPageItem("За водителем", "carpassengers_2", (p, v) =>
                    VehicleManager.EjectPassengers(p, v, 2)))

                .AddItem(new InteractionMenuPageItem("За соседним пассажиром", "carpassengers_", (p, v) =>
                    VehicleManager.EjectPassengers(p, v, 3)));


            _vehiclesInteractionPage = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("client_78", "hoodsvg", (p, v) =>
                    VehicleManager.ChangeVehicleDoorOpen(p, v, DoorID.DoorHood)))
                .AddItem(new InteractionMenuPageItem("Core1_77", "trunk-open", (p, v) =>
                    VehicleManager.ChangeVehicleDoorOpen(p, v, DoorID.DoorTrunk)))
                .AddItem(new InteractionMenuPageItem("cef_82", "car-doors", VehicleManager.ChangeVehicleDoors))
                .AddItem(new InteractionMenuPageItem("cef_81", "searching-car", Selecting.OpenCarStock), (v) => v.Class != 13 && v.Class != 8)
                .AddItem(new InteractionMenuPageItem("circle_veh_6", "anchor", (p, v) =>
                {
                    if (v.Class != 14)
                    {
                        Notify.SendError(p, "interr:menu:1");
                        return;
                    }
                    VehicleStreaming.SetFreezePosition(v, v.IsFreezed);
                }), (v) => v.Class == 14)
                .AddItem(new InteractionMenuPageItem("Продать автомобиль", "car-fee", (p, v) =>
                {
                    //extVehicle.Data.CanAccessVehicle(player, AccessType.Tuning)
                    if (v.Data.CanAccessVehicle(p, AccessType.SellZero) || v.Data.CanAccessVehicle(p, AccessType.SellDollars))
                    {
                        p.CreateWaypoint(SellPosition);
                        Notify.Send(p, NotifyType.Success, NotifyPosition.BottomCenter, "Место продажи автомобиля отмечено на карте", 3000);
                    }
                    else
                    {
                        Notify.Send(p, NotifyType.Error, NotifyPosition.BottomCenter, "Вы не можете продать данный транспорт", 3000);
                    }
                    //p.CreateWaypoint(SellPosition);
                    //Notify.Send(p, NotifyType.Success, NotifyPosition.BottomCenter, "Место продажи автомобиля отмечено на карте", 3000);
                    return;
                }), (v) => v.IsWearable())
                .AddItem(new InteractionMenuPageItem("circle_veh_7", "carpass", VehicleManager.ViewVehicleTechnicalCertificate), (v) => v.IsWearable())
                .AddItem(new InteractionMenuPageItem("circle_veh_8", "carbox", VehicleManager.SetContainerToVehicle), (p) => p.IsPlayerHaveContainer())
                .AddItem(new InteractionMenuPageItem("circle_veh_9", "repair-car", VehicleManager.UseRepairKit))
                .AddItem(new InteractionMenuPageItem("Высадить пассажира", "carpassengers", carIntercationsPlayers, true));

            _pedInteractionPage = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("circle_pet_0", "pet_0"))
                .AddItem(new InteractionMenuPageItem("circle_pet_1", "pet_1"))
                .AddItem(new InteractionMenuPageItem("circle_pet_2", "pet_2"))
                .AddItem(new InteractionMenuPageItem("circle_pet_3", "pet_3"));
        }

        [RemoteEvent("intMenu:selected")]
        public static void OnPlayerSelectedAnswer(ExtPlayer player, params object[] arguments)
        {
            try
            {
                // if (player.IsInVehicle) return;
                var target = (Entity) arguments[0];
                var answerKey = arguments[1].ToString();
                var interactionTypeIndex = Convert.ToInt32(arguments[2]);
                var interactionType = (InteractionType) interactionTypeIndex;
                if (!InteractionMenuPageItem.AllInteractionMenuPageItems.TryGetValue(answerKey, out var selectedItem))
                {
                    _logger.WriteError("interr:menu:2");
                    return;
                }
                
                switch (interactionType)
                {
                    case InteractionType.WithPlayer:
                        SafeTrigger.SetData(player, "SELECTEDPLAYER", target);
                        selectedItem.CallbackWithPlayers?.Invoke(player, target as ExtPlayer);
                        break;
                    case InteractionType.WithVehicle:
                        selectedItem.CallbackWithVehicles?.Invoke(player, target as ExtVehicle);
                        break;
                    case InteractionType.WithPed:
                        break;
                    case InteractionType.WithObject:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(interactionType), interactionType, 
                            "Interaction type was out of server enum");
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError("intMenu:selected: " + ex);
            }
        }

        [RemoteEvent("intMenu:opened")]
        public void OnPlayerOpenedInteractionMenu(ExtPlayer player, params object[] arguments)
        {
            if(player.DATA_INTERACT_ID == -1){
                var interactionType = (InteractionType)arguments[0];
                Entity target = (Entity)arguments[1];
                try
                {
                    switch (interactionType)
                    {
                        case InteractionType.WithPlayer:
                            _playersInteractionPages[2].OpenForPlayer(player);
                            break;
                        case InteractionType.WithVehicle:
                            _vehiclesInteractionPage.OpenForPlayerWithVehicle(player, target as ExtVehicle);
                            break;
                        case InteractionType.WithPed:
                            _pedInteractionPage.OpenForPlayer(player);
                            break;
                        case InteractionType.WithObject:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(interactionType), interactionType, null);
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteError("intMenu:opened: " + ex);
                }  
            }
        }
    }
}
