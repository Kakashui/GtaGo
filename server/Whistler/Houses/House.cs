using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Whistler.Core;
using Whistler.SDK;
using System.Linq;
using Whistler.Families;
using Whistler.Houses.DTOs;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.Houses.Furnitures;
using Whistler.ParkingSystem;
using Whistler.Possessions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Whistler.NewDonateShop;
using Whistler.MoneySystem.Models;
using Whistler.MoneySystem;
using System.Data;
using Whistler.Common;
using Whistler.Common.Interfaces;
using Whistler.Houses.Models;
using Whistler.Entities;

namespace Whistler.Houses
{
    internal class House : IWhistlerProperty
    {
        public int ID { get; }
        public int OwnerID { get; private set; }
        public HouseTypes Type { get; private set; }
        public Vector3 Position { get; private set; }
        public int Price { get; private set; }
        public bool Locked { get; private set; }
        public int GarageID { get; private set; }
        public int BankNew { get; private set; }
        public OwnerType OwnerType { get; private set; }
        public int RentCost { get; private set; }
        private List<Roommate> _roommates  = new List<Roommate>();
        public List<Furniture> Furnitures;
        public bool Pledged { get; set; }

        [JsonIgnore] public uint Dimension { get; set; }

        [JsonIgnore]
        public Blip blip;
        [JsonIgnore]
        private TextLabel label;

        [JsonIgnore]
        private InteractShape intshape;

        [JsonIgnore]
        public CheckingAccount BankModel
        {
            get
            {
                return BankManager.GetAccount(BankNew);
            }
        }
        [JsonIgnore]
        public int HouseTax
        {
            get
            {
                return Convert.ToInt32(Price * MoneyConstants.PayTaxCoeffForHour);
            }
        }

        [JsonIgnore]
        public List<Player> PlayersInside = new List<Player>();

        [JsonIgnore]
        public Garage HouseGarage => GarageManager.Garages.GetValueOrDefault(GarageID);
        [JsonIgnore]
        public HouseType TypeHouse => HouseManager.HouseTypeList.GetValueOrDefault(Type);

        public PropertyType PropertyType => PropertyType.House;
        public int CurrentPrice => Price;
        public string PropertyName => TypeHouse?.Name;
        public House(int id, int ownerID, HouseTypes type, Vector3 position, int price, bool locked, int garageID, List<Roommate> roommates, int rentCost, List<Furniture> furnitures = null, OwnerType typeOwner = 0)
        {
            ID = id;
            RentCost = rentCost;
            OwnerID = ownerID;
            Type = type;
            Position = position;
            Price = price;
            Locked = locked;
            GarageID = garageID;
            BankNew = BankManager.CreateAccount(TypeBankAccount.House).ID;
            _roommates = roommates;
            OwnerType = typeOwner;
            Furnitures = furnitures;
            Dimension = (uint)(HouseManager.DimensionID + ID);

            UpdateBlip();

            intshape = InteractShape.Create(Position, 1, 2)
                .AddOnEnterColshapeExtraAction((c, player) =>
                {
                    player.SetData("HOUSEID", ID);
                })
                .AddOnExitColshapeExtraAction((c, player) =>
                {
                    player.ResetData("HOUSEID");
                })
                .AddInteraction(OpenHousePanel, "interact_19");

            FurnitureService.InitializeForHouse(this);
            label = NAPI.TextLabel.CreateTextLabel("House", Position + new Vector3(0, 0, 1.5), 5f, 0.4f, 0, new Color(255, 255, 255), false, 0);
            MySQL.Query("INSERT INTO `houses`" +
                "(`id`,`owneruuid`,`type`,`position`,`price`,`locked`,`garage`,`banknew`,`typeowner`, `furnitures`, `owner`, `occupiers`) VALUES " +
                "(@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10, @prop11)",
                ID, OwnerID, (int)Type, JsonConvert.SerializeObject(Position), Price, Locked, GarageID, BankNew, (int)OwnerType, JsonConvert.SerializeObject(Furnitures), "", JsonConvert.SerializeObject(new List<Roommate>()));
        }
        
        public House(DataRow row)
        {

            ID = Convert.ToInt32(row["id"].ToString());
            OwnerID = Convert.ToInt32(row["owneruuid"]);
            var type = Convert.ToInt32(row["type"]);
            Type = (HouseTypes)Convert.ToInt32(row["type"]);
            Position = JsonConvert.DeserializeObject<Vector3>(row["position"].ToString());
            Price = Convert.ToInt32(row["price"]);
            Locked = Convert.ToBoolean(row["locked"]);
            Pledged = Convert.ToBoolean(row["pledged"]);
            GarageID = Convert.ToInt32(row["garage"]);
            BankNew = Convert.ToInt32(row["banknew"]);
            if (BankModel == null)
            {
                BankNew = BankManager.CreateAccount(TypeBankAccount.House).ID;
                MySQL.Query("UPDATE `houses` SET banknew = @prop1 WHERE id = @prop0", ID, BankNew);
            }
            Furnitures = JsonConvert.DeserializeObject<List<Furniture>>(row["furnitures"].ToString());
            RentCost = Convert.ToInt32(row["rentCost"]);
            _roommates = JsonConvert.DeserializeObject<List<Roommate>>(row["occupiers"].ToString()) ?? new List<Roommate>();

            OwnerType = (OwnerType)Convert.ToByte(row["typeowner"]);

            if (OwnerID <= 0 && OwnerType > OwnerType.Personal)
                OwnerType = 0;
            Dimension = (uint)(HouseManager.DimensionID + ID);

            UpdateBlip();

            intshape = InteractShape.Create(Position, 1, 2)
                .AddOnEnterColshapeExtraAction((c, player) =>
                {
                    player.SetData("HOUSEID", ID);
                })
                .AddOnExitColshapeExtraAction((c, player) =>
                {
                    player.ResetData("HOUSEID");
                })
                .AddInteraction(OpenHousePanel, "interact_19");

            FurnitureService.InitializeForHouse(this);
            label = NAPI.TextLabel.CreateTextLabel("House", Position + new Vector3(0, 0, 1.5), 5f, 0.4f, 0, new Color(255, 255, 255), false, 0);
        }


        public void OpenHousePanel(PlayerGo player)
        {
            if (player.IsInVehicle) return;
            player.TriggerEvent("houses:openInfoPanel", JsonConvert.SerializeObject(GetInfoDTO(player)));
        }

        public string GetHouseOwnerName()
        {
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    if (Main.PlayerNames.ContainsKey(OwnerID))
                        return Main.PlayerNames[OwnerID];
                    break;
                case OwnerType.Family:
                    return FamilyManager.GetFamilyName(OwnerID);
            }
            return "Unknown";
        }
        private HouseInfoDTO GetInfoDTO(PlayerGo player)
        {
            var canPlayerEnter = player.GetCharacter().UUID == OwnerID || !Locked || OwnerType == OwnerType.Family;
            return new HouseInfoDTO
            {
                ID = ID,
                Owner = GetHouseOwnerName(),
                Class = HouseManager.HouseTypeList[Type].Name,
                Roommates = HouseManager.MaxRoommates[Type],
                GarageSpace = HouseGarage.GarageConfig.MaxCars,
                Price = Price,
                IsSelled = OwnerID != -1,
                IsLocked = Locked,
                IsTarget = player.Character.HouseTarget == ID,
                CanEnter = canPlayerEnter
            };
        }

        public void OnOwnerInteracted(Player player)
        {
            if (OwnerID == -1) return;
            var dto = new HouseOwnerMenuDTO(player, this, _roommates);
            player.TriggerEvent("house::ownerInteracted", JsonConvert.SerializeObject(dto));
        }

        public void UpdateBlip()
        {
            if (blip != null && blip.Exists)
                blip.Delete();

            blip = NAPI.Blip.CreateBlip(Position);

            if (OwnerType == OwnerType.Family)
            {
                var hBlip = new HouseBlipType(40, 5, 1F, "Семейный дом");

                blip.Sprite = hBlip.Sprite;
                blip.Color = hBlip.Color;
                blip.Scale = hBlip.Scale;
                blip.ShortRange = true;
                blip.Name = hBlip.Name;
            }
            else
            {
                var hBlip = new HouseBlipType(374, ((OwnerID == -1) ? 52 : 59), 0.6F, ((OwnerID == -1) ? "Дом" : "Дом"));

                blip.Sprite = hBlip.Sprite;
                blip.Color = hBlip.Color;
                blip.Scale = hBlip.Scale;
                blip.ShortRange = true;
                blip.Name = hBlip.Name;
            }

        }
        public void Destroy()
        {
            blip?.Delete();
            intshape?.Destroy();
            label?.Delete();
            RemoveAllPlayers();
        }

        #region Roommates

        //private void ProcessRentCostForOccupiers()
        //{
        //    int tax = 0;
        //    foreach (var roommate in _roommates)
        //    {
        //        var onlinePlayer = Main.GetPlayerByUUID(roommate.CharacterUUID);
        //        if (!roommate.ChangeBalance(-RentCost))
        //            RemoveRoommate(roommate.CharacterUUID);
        //        else
        //            tax += RentCost;
        //    }
        //    GameLog.Money($"houseroommates({ID})", "server", tax, "rentHouse");
        //    UpdateRoommates();
        //}

        public Roommate GetRoommate(int uuid)
        {
            return _roommates.FirstOrDefault(item => item.CharacterUUID == uuid);
        }

        public List<PlayerGo> GetOnlineRoommates()
        {
            return _roommates.Select(item => Main.GetPlayerByUUID(item.CharacterUUID)).Where(item => item != null).ToList();
        }

        public void AddRoommate(Roommate roommate)
        {
            _roommates.Add(roommate);
            UpdateRoommates();
            CreateRoommateBlipAndMarker(roommate.CharacterUUID);
        }

        public void RemoveRoommate(int uuid)
        {
            var roommate = _roommates.FirstOrDefault(item => item.CharacterUUID == uuid);
            if (roommate != null)
                RemoveRoommate(roommate);
        }
        private void RemoveRoommate(Roommate roommate)
        {
            _roommates.Remove(roommate);
            UpdateRoommates();
            var player = Main.GetPlayerByUUID(roommate.CharacterUUID);
            if (player != null)
            {
                DeleteRoommateBlipAndMarker(player);
                RemovePlayer(player);
                Notify.SendAlert(player, "newHouses_2");
            }
        }
        public void RemoveRoommates()
        {
            foreach (var roommate in _roommates)
            {
                var player = Main.GetPlayerByUUID(roommate.CharacterUUID);
                if (player != null)
                {
                    DeleteRoommateBlipAndMarker(player);
                    RemovePlayer(player);
                    Notify.SendAlert(player, "newHouses_2");
                }
            }
            _roommates = new List<Roommate>();
            UpdateRoommates();
        }
        #endregion

        #region Save DB

        public void SetLock(bool locked)
        {
            Locked = locked; 
            MySQL.Query("UPDATE `houses` SET `locked` = @prop0 WHERE `id` = @prop1", Locked, ID);
        }

        public void SetGarageId(int garageId)
        {
            GarageID = garageId;
            MySQL.Query("UPDATE `houses` SET `garage`=@prop0 WHERE `id`=@prop1", GarageID, ID);
        }

        public void SetPrice(int price)
        {
            Price = price;
            MySQL.Query("UPDATE `houses` SET `price` = @prop0 WHERE `id` = @prop1", Price, ID);

        }
        public void SetRentPrice(int price)
        {
            RentCost = price;
            MySQL.Query("UPDATE `houses` SET `rentCost` = @prop0 WHERE `id` = @prop1", RentCost, ID);
        }
        public void UpdateRoommates()
        {
            MySQL.Query("UPDATE `houses` SET `occupiers` = @prop0 WHERE `id` = @prop1", JsonConvert.SerializeObject(_roommates), ID);
        }
        public void UpdateFurnitures()
        {
            MySQL.Query("UPDATE `houses` SET `furnitures` = @prop0 WHERE `id` = @prop1", JsonConvert.SerializeObject(Furnitures), ID);
        }
        public void UpdateOwner()
        {
            MySQL.Query("UPDATE `houses` SET `owneruuid` = @prop0, `typeowner`=@prop1 WHERE `id` = @prop2", OwnerID, (int)OwnerType, ID);
        }
        public void SetPledged(bool status)
        {
            Pledged = status;
            MySQL.Query("UPDATE `houses` SET `pledged` = @prop0 WHERE `id` = @prop1", Pledged, ID);
        }

        public void SetType(HouseTypes type)
        {
            Type = type;
            MySQL.Query("UPDATE `houses` SET `type` = @prop0 WHERE `id` = @prop1", Type, ID);
        }
        #endregion

        #region CheckAccess
        public bool GetAccess(Player player, FamilyHouseAccess access)
        {
            if (OwnerID <= 0)
                return false;
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    switch (access)
                    {
                        case FamilyHouseAccess.EnterHouse:
                        case FamilyHouseAccess.OpenDoors:
                            return OwnerID == player.GetCharacter().UUID || GetRoommate(player.GetCharacter().UUID) != null;
                        case FamilyHouseAccess.UpgradeGarage:
                        case FamilyHouseAccess.FullAccess:
                            return OwnerID == player.GetCharacter().UUID;
                        default:
                            break;
                    }
                    return OwnerID == player.GetCharacter().UUID || GetRoommate(player.GetCharacter().UUID) != null;
                case OwnerType.Family:
                    return FamilyManager.CanAccessToHouse(player, OwnerID, access);
            }
            return false;
        }
        public bool GetAccessFurniture(Player player, FamilyFurnitureAccess access)
        {
            if (OwnerID <= 0)
                return false;
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    return OwnerID == player.GetCharacter().UUID;
                case OwnerType.Family:
                    return FamilyManager.CanAccessToFurniture(player, OwnerID, access);
            }
            return false;
        }

        public bool GetAccessToGarage(Player player)
        {
            if (OwnerID <= 0)
                return false;
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    return OwnerID == player.GetCharacter().UUID || GetRoommate(player.GetCharacter().UUID) != null;
                case OwnerType.Family:
                    return OwnerID == player.GetCharacter().FamilyID;
            }
            return false;
        }
        #endregion

        public void SetOwner(int ownerId, OwnerType ownerType)
        {
            HouseGarage?.DestroyCars();

            RemoveAllPlayers();
            DeleteClientBlipAndMarker();
            RemoveRoommates();
            BankModel.UnSubscribe();

            OwnerID = ownerId;
            if (OwnerID != -1)
                OwnerType = ownerType;
            else
                OwnerType = OwnerType.Personal;

            CreateClientBlipAndMarker();

            if (OwnerID != -1)
            {
                if (OwnerType == OwnerType.Personal)
                {
                    var player = Main.GetPlayerByUUID(OwnerID);
                    if (player.IsLogged())
                    {
                        ParkingManager.DeleteParkingVehicle(player);
                        BankModel.Subscribe(player);
                    }
                }
            }
            UpdateBlip();
            UpdateOwner();
        }
        public void DeletePropertyFromMember() => SetOwner(-1, OwnerType.Personal);

        public void DisconnectedPlayer(int uuid)
        {
            var allPlayers = _roommates.Select(item => item.CharacterUUID).ToList();
            allPlayers.Add(OwnerID);
            if (allPlayers.Contains(uuid))
                allPlayers.Remove(uuid);
            foreach (var roommate in allPlayers)
            {
                if (Main.GetPlayerByUUID(roommate) != null)
                    return;
            }
            HouseGarage.DestroyCars();
        }
        public void ConnectedPlayer()
        {
            WhistlerTask.Run(() => HouseGarage.RespawnCars(), 7000);
        }

        public void SendPlayer(Player player)
        {
            player.ChangePosition(HouseManager.HouseTypeList[Type].Position + new Vector3(0, 0, 1.12));
            player.Dimension = Convert.ToUInt32(Dimension);
            player.GetCharacter().InsideHouseID = ID;
            if (!PlayersInside.Contains(player)) 
                PlayersInside.Add(player);
            HouseManager.PlayerEntered?.Invoke(player, this);
        }
        public void RemovePlayer(Player player, bool exit = true)
        {
            if (player.GetCharacter().InsideHouseID != ID)
                return;
            if (exit)
            {
                player.ChangePosition(Position + new Vector3(0, 0, 1.12));
                player.Dimension = 0;
            }
            player.GetCharacter().InsideHouseID = -1;

            RemoveFromList(player);
            HouseManager.PlayerLeaved?.Invoke(player);
        }

        public void RemoveFromList(Player player)
        {
            if (PlayersInside.Contains(player)) 
                PlayersInside.Remove(player);
        }

        public void RemoveAllPlayers()
        {
            foreach (var player in PlayersInside)
            {
                if (player != null)
                {
                    player.ChangePosition(Position + new Vector3(0, 0, 1.12));
                    player.Dimension = 0;

                    player.GetCharacter().InsideHouseID = -1;
                    HouseManager.PlayerLeaved?.Invoke(player);
                }
            }
            PlayersInside = new List<Player>();
        }

        #region Create Player Blips and Markers
        public void CreateClientBlipAndMarker()
        {
            if (OwnerID == -1)
                return;
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    var player = Main.GetPlayerByUUID(OwnerID);
                    if (player != null)
                    {
                        player.CreateClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID, 40, "House", Position, 1, 73, 0);
                        player.CreateClientMarker(333, 42, HouseGarage?.Position ?? new Vector3() - new Vector3(0, 0, 0.5), 2, NAPI.GlobalDimension, new Color(182, 211, 0), new Vector3(90, 90, 90));
                        Trigger.ClientEvent(player, "createGarageBlip", HouseGarage?.Position ?? new Vector3());
                    }
                    break;
                case OwnerType.Family:
                    foreach (var member in FamilyManager.GetFamilyMembers(OwnerID))
                    {
                        CreateFamilyMemberMarker(member);
                    }
                    break;
            }
        }
        public void CreateRoommateBlipAndMarker(int uuid)
        {
            var player = Main.GetPlayerByUUID(uuid);
            if (player != null)
            {
                player.CreateClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID, 40, "House", Position, 1, 73, 0);
                player.CreateClientMarker(333, 42, HouseGarage?.Position ?? new Vector3() - new Vector3(0, 0, 0.5), 2, NAPI.GlobalDimension, new Color(182, 211, 0), new Vector3(90, 90, 90));
                Trigger.ClientEvent(player, "createGarageBlip", HouseGarage?.Position ?? new Vector3());
            }
        }
        public void CreateFamilyMemberMarker(Player player)
        {
            player.CreateClientMarker(334, 42, HouseGarage?.Position ?? new Vector3() - new Vector3(0, 0, 0.5), 2, NAPI.GlobalDimension, new Color(220, 220, 0), new Vector3(90, 90, 90));
        }
        public void DeleteClientBlipAndMarker()
        {
            if (OwnerID == -1)
                return;
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    var player = Main.GetPlayerByUUID(OwnerID);
                    if (player != null)
                    {
                        player.DeleteClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID);
                        player.DeleteClientMarker(333);
                        player.TriggerEvent("deleteGarageBlip");
                    }
                    break;
                case OwnerType.Family:
                    foreach (var member in FamilyManager.GetFamilyMembers(OwnerID))
                    {
                        DeleteFamilyMemberMarker(member);
                    }
                    break;
            }
        }
        public void DeleteRoommateBlipAndMarker(Player player)
        {            
            player.DeleteClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID);
            player.DeleteClientMarker(333);
            player.TriggerEvent("deleteGarageBlip");            
        }
        public void DeleteFamilyMemberMarker(Player player)
        {
            player.DeleteClientMarker(334);
        }

        #endregion
    }
}