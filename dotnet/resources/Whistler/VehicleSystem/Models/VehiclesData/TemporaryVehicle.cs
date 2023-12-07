using GTANetworkAPI;
using System.Collections.Generic;
using Whistler.Businesses;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Gangs.WeedFarm;
using Whistler.GarbageCollector;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.Jobs;
using Whistler.Jobs.TruckersJob;
using Whistler.MoneySystem;
using Whistler.Phone.Taxi.Service;
using Whistler.SDK;
using Whistler.VehicleRent.Configs;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    class TemporaryVehicle : VehicleBase
    {
        private static int _id = -1;
        public VehicleAccess Access { get; set; }
        public ExtPlayer Driver { get; set; } = null;
        public TemporaryVehicle(uint modelName) : base()
        {
            ID = _id--;
            Access = VehicleAccess.Admin; 
            var config = VehicleConfiguration.GetConfig(modelName);
            ModelName = config.ModelName;
            var inventory = new InventoryModel(config.MaxWeight, config.Slots, InventoryTypes.Vehicle, true);
            InventoryId = inventory.Id;
        }
        public TemporaryVehicle(uint modelName, string numberPlate, VehicleAccess accessType, ExtPlayer driver) : base()
        {
            ID = _id--;
            Access = accessType;
            Number = numberPlate;
            Driver = driver; 
            var config = VehicleConfiguration.GetConfig(modelName);
            ModelName = config.ModelName;
            var inventory = new InventoryModel(config.MaxWeight, config.Slots, InventoryTypes.Vehicle, true);
            InventoryId = inventory.Id;
        }

        protected override ExtVehicle SpawnCar(Vector3 position, Vector3 rotation, uint dimension)
        {
            return null;
        }
        protected override ExtVehicle SpawnCar()
        {
            return null;
        }

        public override bool CanAccessVehicle(ExtPlayer player, AccessType access)
        {
            if (!player.IsLogged())
                return false;
            if ((int)access >= (int)AccessType.Tuning)
                return false;
            switch (Access)
            {
                case VehicleAccess.Admin:
                case VehicleAccess.MedKits:
                    if (player.Character.AdminLVL >= 1)
                        return true;
                    if (player.Character.MediaHelper > 0)
                        return true;
                    return false;
                case VehicleAccess.MatWars:
                case VehicleAccess.RoyalBattle:
                    if (access <= AccessType.Inventory)
                        return true;
                    break;
                case VehicleAccess.WorkCarThief:
                    return false;
                case VehicleAccess.WorkTruck:
                case VehicleAccess.WorkBus:
                case VehicleAccess.DockLoader:
                    switch (access)
                    {
                        case AccessType.Inventory:
                            return false;
                    }
                    break;
                case VehicleAccess.WeedFarm:
                    return WeedFarmService.HasVehicleKey(player, OwnerID);
            }
            if (Driver == player)
                return true;
            return false;
        }

        public override string GetHolderName()
        {
            return $"{Access.ToString().ToUpper()} {Driver?.Name}";
        }

        public override void Save()
        {
            return;
        }

        public override void VehicleDeath(ExtVehicle vehicle)
        {
            return;
        }


        public override void DeleteVehicle(ExtVehicle vehicle = null)
        {
            return;
        }

        public override void DestroyVehicle()
        {
            Vehicle?.CustomDelete();
            Driver?.RemoveTempVehicle(Access);
        }

        public override void RespawnVehicle()
        {
            switch (Access)
            {
                case VehicleAccess.WorkBus:
                    DestroyVehicle();
                    if (Driver.IsLogged())
                    {
                        Driver.Session.OnWork = false;
                        Driver.Character.WorkID = 0;
                        Notify.Send(Driver, NotifyType.Info, NotifyPosition.BottomCenter, $"Jobs_165", 3000);
                        SafeTrigger.ClientEvent(Driver, "bus:clear");
                        Driver.SetData("PAYMENT", 0);
                        MainMenu.SendStats(Driver);
                    }
                    break;
                case VehicleAccess.School:
                    if (Driver.IsLogged() && Driver?.GetTempVehicle(VehicleAccess.School) == Vehicle)
                    {
                        NAPI.ClientEvent.TriggerClientEvent(Driver, "school:finishPractic", false);
                        Notify.Send(Driver, NotifyType.Warning, NotifyPosition.BottomCenter, $"Core_305", 3000);
                    }
                    else
                        DestroyVehicle();
                    break;
                case VehicleAccess.StartQuest:
                case VehicleAccess.Admin:
                case VehicleAccess.Work:
                case VehicleAccess.MatWars:
                case VehicleAccess.MedKits:
                case VehicleAccess.RoyalBattle:
                case VehicleAccess.WorkTruck:
                case VehicleAccess.Rent:
                default:
                    DestroyVehicle();
                    break;
            }
        }

        protected override void PlayerEnterVehicle(ExtPlayer player, ExtVehicle vehicle, sbyte seatid)
        {
            if (Access == VehicleAccess.ShowRoom)
            {
                //VehicleManager.WarpPlayerOutOfVehicle(player);
                NAPI.Player.KickPlayer(player, "Вы были кикнуты за попытку обойти систему.");
                return;
            }
            if (player.VehicleSeat == VehicleConstants.DriverSeat)
            {
                if (Driver == player)
                    GarbageManager.Remove(vehicle);
                switch (Access)
                {
                    case VehicleAccess.MatWars:
                        var fracid = player.Character.FractionID;
                        if ((fracid < 1 || fracid > 5) && player.GetFamily() == null)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Core1_80", 3000);
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        }
                        break;
                    case VehicleAccess.MedKits:
                        if (player.Character.AdminLVL < 1)
                        {
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        }
                        break;
                    case VehicleAccess.StartQuest:
                        if (Driver != player)
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                        player.SendTip("tip_enter_vehicle");
                        break;
                    case VehicleAccess.WorkTruck:
                        if (Driver != player)
                        {
                            Notify.Send(player, NotifyType.Warning, NotifyPosition.Top, "Truckers_1".Translate(), 3000);
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        }
                        SafeTrigger.ClientEvent(player,"truckers:destroyMarker");
                        TruckRentPark.RentParks[vehicle.GetData<int>("RENTPARK_ID")].OnPlayerEnterVehicle(player);
                        TruckRentPark.RentParks[vehicle.GetData<int>("RENTPARK_ID")].ProcessWaitingTimer(player);
                        break;
                    case VehicleAccess.WorkBus:
                        if (!player.CheckLic(GUI.Documents.Enums.LicenseName.Truck))
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_166", 3000);
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        }
                        if (player.Character.WorkID != 4)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_171", 3000);
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                        }
                        if (Driver != player)
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_170", 3000);
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                            return;
                        }
                        SafeTrigger.ClientEvent(player,"bus:destroyMarker");
                        if (player.TempVehicleIsExist(VehicleAccess.WorkBus))
                            player.Session.InWorkCar = true;
                        else
                            Bus.OnStartWork(player);
                        break;
                    case VehicleAccess.DockLoader:
                        SafeTrigger.ClientEvent(player,"dockLoader:destroyMarker");
                        break;
                    case VehicleAccess.Admin:
                    case VehicleAccess.Work:
                    case VehicleAccess.School:
                    case VehicleAccess.RoyalBattle:
                    default:
                        break;
                }
            }
            else
            {
                switch (Access)
                {
                    case VehicleAccess.WorkBus:
                        if (Driver == null) return;

                        if (Driver.Session.InWorkCar == true)
                        {
                            var price = 30;
                            if (Wallet.TransferMoney(player.Character, Manager.GetFraction(6), price, 0, "Оплата за проезд (автобус)"))
                            {
                                Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Jobs_172".Translate(price), 3000);
                            }
                            else
                            {
                                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_173", 3000);
                                VehicleManager.WarpPlayerOutOfVehicle(player);
                            }
                        }
                        else
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Jobs_174", 3000);
                            VehicleManager.WarpPlayerOutOfVehicle(player);
                        }
                        break;
                    case VehicleAccess.Admin:
                    case VehicleAccess.Work:
                    case VehicleAccess.MatWars:
                    case VehicleAccess.MedKits:
                    case VehicleAccess.School:
                    case VehicleAccess.StartQuest:
                    case VehicleAccess.RoyalBattle:
                    case VehicleAccess.WorkTruck:
                    default:
                        break;
                }
            }
        }

        protected override void PlayerExitVehicle(ExtPlayer player, ExtVehicle vehicle)
        {

            if (Driver == player)
            {
                switch (Access)
                {
                    case VehicleAccess.Rent:

                        DriverData taxiDriverData = TaxiService.GetDriverData(player);
                        if (taxiDriverData != null)
                        {
                            DialogUI.Open(player, "Хотите ли вы закончить работу в такси?", new List<DialogUI.ButtonSetting>
                            {
                                new DialogUI.ButtonSetting
                                {
                                    Name = "House_58",// yes
                                    Icon = "confirm",
                                    Action = Phone.Taxi.Job.EndWorkDayHandler.HandleEndWork
                                },

                                new DialogUI.ButtonSetting
                                {
                                    Name = "House_59",// no
                                    Icon = "cancel",
                                    Action = Phone.Taxi.Job.EndWorkDayHandler.HandleEndWorkDelayed
                                }
                            });
                            return;
                        }

                        Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, "Biz_144".Translate(RentVehicleConfig.RespawnTime), 3000);
                        GarbageManager.Add(vehicle, RentVehicleConfig.RespawnTime);
                        break;
                    case VehicleAccess.Admin:
                        break;
                    case VehicleAccess.Work:
                        break;
                    case VehicleAccess.MatWars:
                        break;
                    case VehicleAccess.MedKits:
                        break;
                    case VehicleAccess.School:
                        GarbageManager.Add(vehicle, 1);
                        //Notify.Send(player, NotifyType.Warning, NotifyPosition.BottomCenter, $"Core_304", 3000);
                        break;
                    case VehicleAccess.StartQuest:
                        GarbageManager.Add(vehicle, RentVehicleConfig.RespawnTime);
                        break;
                    case VehicleAccess.RoyalBattle:
                        break;
                    case VehicleAccess.WorkTruck:
                        DialogUI.Open(player, "Truckers_4", new List<DialogUI.ButtonSetting>
                        {
                            new DialogUI.ButtonSetting
                            {
                                Name = "House_58",// Да
                                Icon = "confirm",
                                Action = p => TruckRentPark.RentParks[(int) vehicle.GetData<int>("RENTPARK_ID")].StopRentForPlayer(p),
                            },

                            new DialogUI.ButtonSetting
                            {
                                Name = "House_59",// Нет
                                Icon = "cancel",
                                Action = p => TruckRentPark.RentParks[(int) vehicle.GetData<int>("RENTPARK_ID")].StartWaitingTimer(p),
                            }
                        });
                        
                        break;
                    case VehicleAccess.WorkBus:
                        if (player.Character.WorkID == 4 && player.Session.OnWork)
                        {
                            DialogUI.Open(player, "Хотите ли вы отменить аренду автобуса?", new List<DialogUI.ButtonSetting>
                            {
                                new DialogUI.ButtonSetting
                                {
                                    Name = "House_58",// yes
                                    Icon = "confirm",
                                    Action = Bus.StopWork
                                },

                                new DialogUI.ButtonSetting
                                {
                                    Name = "House_59",// no
                                    Icon = "cancel",
                                    Action = Bus.StopWorkDelayed
                                }
                            });
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
