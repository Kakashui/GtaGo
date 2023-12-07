using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using Whistler.Common;
using Whistler.Common.Interfaces;
using Whistler.Core;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.NewDonateShop;
using Whistler.PriceSystem;
using Whistler.SDK;

namespace Whistler.VehicleSystem.Models.VehiclesData
{
    abstract public class PersonalBaseVehicle : VehicleBase, IWhistlerProperty
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PersonalBaseVehicle));
        public int KeyNum { get; set; }
        public PropBuyStatus PropertyBuyStatus { get; protected set; }
        public float EngineHealth { get; set; } = 1000;
        public double Mileage { get; set; } = 0;
        public double MileageOilChange { get; set; } = 0;
        public double MileageBrakePadsChange { get; set; } = 0;
        public double MileageTransmissionService { get; set; } = 0;
        public WantedLevel WantedLVL { get; set; } = null;
        public Vector3 SavePosition { get; set; } = null;
        public Vector3 SaveRotation { get; set; } = null;
        public int TradePoint { get; set; } = -1;
        public bool Pledged { get; set; }

        public PropertyType PropertyType => PropertyType.Vehicle;
        public int CurrentPrice 
        {
            get
            {
                switch (PropertyBuyStatus)
                {
                    case PropBuyStatus.Bought:
                    case PropBuyStatus.Roulette:
                        return PriceManager.GetPriceInDollars(TypePrice.Car, ModelName, 0);
                    case PropBuyStatus.Donate:
                    case PropBuyStatus.Given:
                    default:
                        return 0;
                }
            }
        }
        public string PropertyName => Config?.DisplayName;
        public PersonalBaseVehicle(DataRow row, OwnerType typeOwner) : base(row)
        {
            Price = Convert.ToInt32(row["price"]);
            KeyNum = Convert.ToInt32(row["keynum"]);
            PropertyBuyStatus = (PropBuyStatus)Convert.ToInt32(row["buytype"]);
            if (PropertyBuyStatus == PropBuyStatus.Unknown)
            {
                var isStreamerCar = Convert.ToBoolean(row["streamer"] == DBNull.Value ? false : row["streamer"]);
                var donateCar = Convert.ToBoolean(row["donatecar"]);
                if (!isStreamerCar && !donateCar)
                    SetBuyStatus(PropBuyStatus.Bought);
                else if (isStreamerCar && !donateCar)
                    SetBuyStatus(PropBuyStatus.Given);
                else if (!isStreamerCar && donateCar)
                    SetBuyStatus(PropBuyStatus.Donate);
                else
                    SetBuyStatus(PropBuyStatus.Roulette);
            }
            IsDeath = Convert.ToBoolean(row["isdeath"]);
            Fuel = Convert.ToInt32(row["fuel"]);
            Dirt = Convert.ToSingle(row["dirt"]);
            DirtClear = row["dirtclear"] == DBNull.Value ? DateTime.UtcNow : Convert.ToDateTime(row["dirtclear"]);

            EngineHealth = Convert.ToSingle(row["enginehealth"]);
            DoorBreak = Convert.ToInt32(row["doorbreak"]);
            Mileage = Convert.ToDouble(row["mileage"]);
            MileageOilChange = Convert.ToDouble(row["mileageoilchange"]);
            MileageBrakePadsChange = Convert.ToDouble(row["mileagebrakepadschange"]);
            MileageTransmissionService = Convert.ToDouble(row["mileagetransmissionservice"]);
            WantedLVL = row["wantedlevel"] == DBNull.Value ? null : JsonConvert.DeserializeObject<WantedLevel>(row["wantedlevel"].ToString());
            SavePosition = JsonConvert.DeserializeObject<Vector3>(row["saveposition"].ToString());
            SaveRotation = JsonConvert.DeserializeObject<Vector3>(row["saverotation"].ToString());
            TradePoint = Convert.ToInt32(row["tradepoint"]);
            Pledged = Convert.ToBoolean(row["pledged"]);

            OwnerType = typeOwner;

        }
        public PersonalBaseVehicle(int holderId, string model, Color color1, Color color2, int fuel, int price, PropBuyStatus buyStatus, OwnerType typeOwner)
        {
            OwnerID = holderId;
            ModelName = model;
            Position = null;
            Rotation = null;
            VehCustomization = new CustomizationVehicleModel();
            Fuel = fuel;
            Dirt = 0;
            DirtClear = DateTime.UtcNow;
            EnginePower = 0;
            EngineTorque = 1;
            Price = price;
            PropertyBuyStatus = buyStatus;
            VehCustomization.PrimColor = color1;
            VehCustomization.SecColor = color2;
            OwnerType = typeOwner;
            Number = VehicleManager.GenerateNumber();
            TradePoint = -1;
            CreateNewInventory();
            var dataQuery = MySQL.QueryRead("INSERT INTO `vehicles`(`number`, `holderuuid`, `model`, `fuel`, `price`, `componentsnew`, `power`, `torque`, `buytype`, `typeowner`, `enginehealth`, `inventoryId`, `wantedlevel`) " +
                "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10, @prop11, @prop12); SELECT @@identity;",
                Number, OwnerID, ModelName, Fuel, Price, JsonConvert.SerializeObject(VehCustomization), EnginePower, EngineTorque, PropertyBuyStatus, (int)OwnerType, EngineHealth, InventoryId, JsonConvert.SerializeObject(WantedLVL));
            ID = Convert.ToInt32(dataQuery.Rows[0][0]);
            VehicleManager.Vehicles.Add(ID, this);
            VehicleManager.NewVehicleToHolder?.Invoke(this);
        }
        public PersonalBaseVehicle()
        {
        }
        protected override ExtVehicle SpawnCar()
        {
            return Spawn(Position, Rotation, 0);
        }

        protected override ExtVehicle SpawnCar(Vector3 position, Vector3 rotation, uint dimension)
        {
            if (Thread.CurrentThread.Name != "Main") return null;

            uint model = NAPI.Util.GetHashKey(ModelName);
            ExtVehicle vehicle = NAPI.Vehicle.CreateVehicle(model, position, rotation.Z, 0, 0, dimension: dimension) as ExtVehicle;
            if (vehicle == null || !vehicle.Exists) return null;

            vehicle.Initialize(this);
            vehicle.InitializeLicense(vehicle.Class);
            ushort vehicleId = vehicle.Id;
            vehicle.Session.Id = vehicle.Id;
            vehicle.Session.Dimension = dimension;
            if (Main.AllVehicles.ContainsKey(vehicleId)) Main.AllVehicles[vehicleId] = vehicle;
            else Main.AllVehicles.Add(vehicleId, vehicle);
            NAPI.Vehicle.SetVehicleNumberPlate(vehicle, Number);
            vehicle.Dimension = dimension;
            VehicleStreaming.SetEngineState(vehicle, false);
            VehicleStreaming.SetLockStatus(vehicle, true);

            VehicleManager.ApplyVehicleState(vehicle);
            if (Fuel < 0) Fuel = 0;
            VehicleStreaming.SetVehicleFuel(vehicle, Fuel);

            return vehicle;
        }

        public override void Save()
        {
            MySQL.Query("UPDATE `vehicles` SET `holderuuid`=@prop0, `model`=@prop1, `typeowner`=@prop2, `componentsnew`=@prop3, `position`=@prop4, `rotation`=@prop5," +
                  " `dirt`=@prop6, `number`=@prop7, `power`=@prop8, `torque`=@prop9,`fuel`=@prop10, `keynum`=@prop11, `enginehealth`=@prop12, `doorbreak`=@prop13, " +
                  "`mileage`=@prop14, `mileageoilchange`=@prop15, `mileagebrakepadschange`=@prop16, `mileagetransmissionservice`=@prop17, `wantedlevel` = @prop18, " +
                  "`saveposition` = @prop19, `saverotation` = @prop20, `dirtclear` = @prop21  WHERE `idkey`=@prop22",
                    OwnerID,
                    ModelName,
                    (int)OwnerType,
                    JsonConvert.SerializeObject(VehCustomization),
                    JsonConvert.SerializeObject(Position),
                    JsonConvert.SerializeObject(Rotation),
                    Dirt == float.NaN ? 0.0f : Dirt,
                    Number,
                    EnginePower,
                    EngineTorque,
                    Fuel,
                    KeyNum,
                    EngineHealth,
                    DoorBreak,
                    (int)Mileage,
                    (int)MileageOilChange,
                    (int)MileageBrakePadsChange,
                    (int)MileageTransmissionService,
                    JsonConvert.SerializeObject(WantedLVL),
                    JsonConvert.SerializeObject(SavePosition),
                    JsonConvert.SerializeObject(SaveRotation),
                    MySQL.ConvertTime(DirtClear),
                    ID
            );
        }

        public void SetTradePoint(int id)
        {
            TradePoint = id;
            MySQL.Query("UPDATE `vehicles` SET `tradepoint` = @prop0 WHERE `idkey` = @prop1",
                    TradePoint,
                    ID
            );
        }
        public void SetBuyStatus(PropBuyStatus status)
        {
            PropertyBuyStatus = status;
            MySQL.Query("UPDATE `vehicles` SET `buytype` = @prop0 WHERE `idkey` = @prop1",
                    (int)PropertyBuyStatus,
                    ID
            );
        }

        public override void VehicleDeath(ExtVehicle vehicle)
        {
            if (OwnerType == OwnerType.Personal)
            {
                string ownerName = GetHolderName();
                ExtPlayer owner = Trigger.GetPlayerByName(ownerName);
                if (owner != null)
                    Notify.Send(owner, NotifyType.Alert, NotifyPosition.BottomCenter, "Core1_74".Translate(), 3000);
            }

            IsDeath = true;

            vehicle.CustomDelete();
        }
        public override void DeleteVehicle(ExtVehicle vehicle = null)
        {
            DestroyVehicle();
            VehicleManager.RemoveVehicleFromHolder?.Invoke(this);
            VehicleManager.Vehicles.Remove(ID);
            MySQL.Query("UPDATE `vehicles` SET `isdeleted` = 1 WHERE `idkey` = @prop0", ID);
        }
        public override void RespawnVehicle()
        {
            Houses.GarageManager.SendVehicleIntoGarage(ID);
        }

        public void SetSavePosition(Vector3 position, Vector3 rotation)
        {
            SavePosition = position;
            SaveRotation = rotation;
            Save();
        }
        protected override void PlayerEnterVehicle(ExtPlayer player, ExtVehicle vehicle, sbyte seatid)
        {
            if (TradePoint > 0)
            {
                if (CanAccessVehicle(player, AccessType.SellDollars))
                {
                    DialogUI.Open(player, "veh:trade:1", new List<DialogUI.ButtonSetting>
                    {
                        new DialogUI.ButtonSetting
                        {
                            Name = "House_58",
                            Icon = "confirm",
                            Action = p => VehicleTrading.TradeManager.CancelSellVehicle(TradePoint)
                        },

                        new DialogUI.ButtonSetting
                        {
                            Name = "House_59",
                            Icon = "cancel"
                        }
                    });
                }
                else
                    VehicleTrading.TradeManager.OpenBuyMenu(player, TradePoint);
            }
        }

        protected override void PlayerExitVehicle(ExtPlayer player, ExtVehicle vehicle)
        {
        }

        public void SetPledged(bool status)
        {
            Pledged = status;
            MySQL.Query("UPDATE `vehicles` SET `pledged` = @prop0 WHERE `idkey` = @prop1",
                Pledged,
                ID
            );
        }
        public void DeletePropertyFromMember() => VehicleManager.Remove(ID);
    }
}