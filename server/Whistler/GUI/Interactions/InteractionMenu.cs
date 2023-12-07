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
            var documentsInteractionsWithPlayer = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("cef_80", "driver-license", Docs.Licenses))
                .AddItem(new InteractionMenuPageItem("cef_90", "passport-card", Docs.Passport));

            var familyInteractionsWithPlayer = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("cef_98", "thief-fam", FractionCommands.RobberyTarget),
                    p => (p.GetFamily()?.OrgActiveType ?? OrgActivityType.Unknown) == OrgActivityType.Crime)
                .AddItem(new InteractionMenuPageItem("intMenu_12", "balaclava-take-fam", Selecting.TakeMask),
                    p => (p.GetFamily()?.OrgActiveType ?? OrgActivityType.Unknown) == OrgActivityType.Crime)
                .AddItem(new InteractionMenuPageItem("intMenu_11", "family-invite", Selecting.InvitePlayerToFamily),
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
                    p => new [] { 1, 2, 3, 4, 5, 10, 11, 12, 13, 16 }.ToList().Contains(p.GetCharacter().FractionID))
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
                .AddItem(new InteractionMenuPageItem("intMenu_10".Translate("A"), "take-A-lic", (PlayerGo player, PlayerGo target) => FractionCommands.TakeDriveLic(player, target, LicenseName.Moto)), 
                    p => Manager.CanUseCommand(p, "takedrivelic", false))
                .AddItem(new InteractionMenuPageItem("intMenu_10".Translate("B"), "take-B-lic", (PlayerGo player, PlayerGo target) => FractionCommands.TakeDriveLic(player, target, LicenseName.Auto)), 
                    p => Manager.CanUseCommand(p, "takedrivelic", false))
                .AddItem(new InteractionMenuPageItem("intMenu_10".Translate("C"), "take-C-lic", (PlayerGo player, PlayerGo target) => FractionCommands.TakeDriveLic(player, target, LicenseName.Truck)), 
                    p => Manager.CanUseCommand(p, "takedrivelic", false))
                .AddItem(new InteractionMenuPageItem("intMenu_4", "give-gun-lic", Selecting.GiveGunLicense), 
                    p => Manager.CanUseCommand(p, "givegunlic", false))
                .AddItem(new InteractionMenuPageItem("intMenu_6", "invite", FractionCommands.InviteToFraction), 
                    p => Manager.CanUseCommand(p, "invite", false));


            var interactionsWithPlayer = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("cef_79", "handshake", Selecting.playerHandshakeTarget))
                .AddItem(new InteractionMenuPageItem("cef_90_1", "document", documentsInteractionsWithPlayer))
                .AddItem(new InteractionMenuPageItem("cef_85", "give-money", Selecting.OpenMoneyTransferMenu))
                //.AddItem(new InteractionMenuPageItem("cef_84", "exchange", Selecting.SuggestOffer))
                .AddItem(new InteractionMenuPageItem("cef_83", "community", fractionInteractionWithPlayer))
                .AddItem(new InteractionMenuPageItem("cef_83_1", "family", familyInteractionsWithPlayer))
                .AddItem(new InteractionMenuPageItem("cef_86", "heal", Ems.PlayerHealTarget))
                .AddItem(new InteractionMenuPageItem("intMenu_1", "tocar", FractionCommands.playerInCar),
                    p => Manager.CanUseCommand(p, "incar", false))
                .AddItem(new InteractionMenuPageItem("cef_86_2", "house-sell", (PlayerGo player, PlayerGo target) =>
                {
                    player.SetData("SELECTEDPLAYER", target);
                    player.OpenInput("Core1_26", "Core1_29", 100, "house_sell");
                }), p => HouseManager.GetHouse(p, true) != null);

            _playersInteractionPages.Add(documentsInteractionsWithPlayer);
            _playersInteractionPages.Add(fractionInteractionWithPlayer);
            //_playersInteractionPages.Add(familyInteractionsWithPlayer);
            _playersInteractionPages.Add(interactionsWithPlayer);

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
                    VehicleStreaming.SetFreezePosition(v, v.GetVehicleGo().IsFreezed);
                }), (v) => v.Class == 14)
                .AddItem(new InteractionMenuPageItem("Продать автомобиль", "car-fee", (p, v) =>
                {
                    //vehGo.Data.CanAccessVehicle(player, AccessType.Tuning)
                    if (v.GetVehicleGo().Data.CanAccessVehicle(p, AccessType.SellZero) || v.GetVehicleGo().Data.CanAccessVehicle(p, AccessType.SellDollars))
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
                }), (v) => v.GetVehicleGo().IsWearable())
                .AddItem(new InteractionMenuPageItem("circle_veh_7", "carpass", VehicleManager.ViewVehicleTechnicalCertificate), (v) => v.GetVehicleGo().IsWearable())
                .AddItem(new InteractionMenuPageItem("circle_veh_8", "carbox", VehicleManager.SetContainerToVehicle), (p) => p.IsPlayerHaveContainer())
                .AddItem(new InteractionMenuPageItem("circle_veh_9", "repair-car", VehicleManager.UseRepairKit));

            _pedInteractionPage = new InteractionMenuPage()
                .AddItem(new InteractionMenuPageItem("circle_pet_0", "pet_0"))
                .AddItem(new InteractionMenuPageItem("circle_pet_1", "pet_1"))
                .AddItem(new InteractionMenuPageItem("circle_pet_2", "pet_2"))
                .AddItem(new InteractionMenuPageItem("circle_pet_3", "pet_3"));
        }

        [RemoteEvent("intMenu:selected")]
        public static void OnPlayerSelectedAnswer(PlayerGo player, params object[] arguments)
        {
            try
            {
                if (player.IsInVehicle) return;
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
                        player.SetData("SELECTEDPLAYER", target);
                        selectedItem.CallbackWithPlayers?.Invoke(player, target as PlayerGo);
                        break;
                    case InteractionType.WithVehicle:
                        selectedItem.CallbackWithVehicles?.Invoke(player, target as Vehicle);
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
        public void OnPlayerOpenedInteractionMenu(PlayerGo player, params object[] arguments)
        {
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
                        _vehiclesInteractionPage.OpenForPlayerWithVehicle(player, target as Vehicle);
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
