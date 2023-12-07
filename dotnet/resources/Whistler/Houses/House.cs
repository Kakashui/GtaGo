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
        public Vector3 CamPosition { get; private set; }

        [JsonIgnore]
        public int RobberyItemsCount { get; set; }

        [JsonIgnore] 
        public uint Dimension { get; set; }

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
                return Convert.ToInt32(Price * MoneyConstants.PayTaxCoeffHouseForHour);
            }
        }

        [JsonIgnore]
        public List<ExtPlayer> PlayersInside = new List<ExtPlayer>();

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
            CamPosition = new Vector3(0,0,0);
            Price = price;
            Locked = locked;
            GarageID = garageID;
            BankNew = BankManager.CreateAccount(TypeBankAccount.House).ID;
            _roommates = roommates;
            OwnerType = typeOwner;
            Furnitures = furnitures;
            Dimension = (uint)(HouseManager.DimensionID + ID);
            RobberyItemsCount = Main.rnd.Next(1, 4);
            UpdateBlip();

            intshape = InteractShape.Create(Position, 1, 2)
                .AddOnEnterColshapeExtraAction((c, player) =>
                {
                    SafeTrigger.SetData(player, "HOUSEID", ID);
                })
                .AddOnExitColshapeExtraAction((c, player) =>
                {
                    player.ResetData("HOUSEID");
                })
                .AddInteraction(OpenHousePanel, "interact_19");

            FurnitureService.InitializeForHouse(this);
            label = NAPI.TextLabel.CreateTextLabel("House", Position + new Vector3(0, 0, 1.5), 5f, 0.4f, 0, new Color(255, 255, 255), false, 0);
            MySQL.Query("INSERT INTO `houses`" +
                "(`id`,`owneruuid`,`type`,`position`,`price`,`locked`,`garage`,`banknew`,`typeowner`, `furnitures`, `owner`, `roommates`,`camposition`) VALUES " +
                "(@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7, @prop8, @prop9, @prop10, @prop11, @prop12)",
                ID, OwnerID, (int)Type, JsonConvert.SerializeObject(Position), Price, Locked, GarageID, BankNew, (int)OwnerType, JsonConvert.SerializeObject(Furnitures), "", JsonConvert.SerializeObject(new List<Roommate>()), JsonConvert.SerializeObject(CamPosition));
        }
        
        public House(DataRow row)
        {

            ID = Convert.ToInt32(row["id"].ToString());
            OwnerID = Convert.ToInt32(row["owneruuid"]);
            var type = Convert.ToInt32(row["type"]);
            Type = (HouseTypes)Convert.ToInt32(row["type"]);
            Position = JsonConvert.DeserializeObject<Vector3>(row["position"].ToString());
            CamPosition = JsonConvert.DeserializeObject<Vector3>(row["camposition"].ToString());
            Price = Convert.ToInt32(row["price"]);
            Locked = OwnerID == -1 ? true : Convert.ToBoolean(row["locked"]);
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
            _roommates = JsonConvert.DeserializeObject<List<Roommate>>(row["roommates"].ToString()) ?? new List<Roommate>();

            OwnerType = (OwnerType)Convert.ToByte(row["typeowner"]);

            if (OwnerID <= 0 && OwnerType > OwnerType.Personal)
                OwnerType = 0;
            Dimension = (uint)(HouseManager.DimensionID + ID);
            RobberyItemsCount = Main.rnd.Next(1, 4);

            UpdateBlip();

            intshape = InteractShape.Create(Position, 1, 2)
                .AddOnEnterColshapeExtraAction((c, player) =>
                {
                    SafeTrigger.SetData(player, "HOUSEID", ID);
                })
                .AddOnExitColshapeExtraAction((c, player) =>
                {
                    player.ResetData("HOUSEID");
                })
                .AddInteraction(OpenHousePanel, "interact_19");

            FurnitureService.InitializeForHouse(this);
            label = NAPI.TextLabel.CreateTextLabel("House", Position + new Vector3(0, 0, 1.5), 5f, 0.4f, 0, new Color(255, 255, 255), false, 0);
        }


        public void OpenHousePanel(ExtPlayer player)
        {
            if (player.IsInVehicle) return;

            SafeTrigger.ClientEvent(player,"houses:openInfoPanel", 
                JsonConvert.SerializeObject(GetInfoDTO(player)),
                JsonConvert.SerializeObject(new { money = player.GetMoneyPayment(PaymentsType.Cash).IMoneyBalance, bank = player.GetMoneyPayment(PaymentsType.Card).IMoneyBalance })
            );
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
        private HouseInfoDTO GetInfoDTO(ExtPlayer player)
        {
            bool canPlayerEnter = !Locked;
            if (OwnerType == OwnerType.Family)
            {
                Families.Models.Family family = player.GetFamily();
                if (family != null && OwnerID == family.Id) canPlayerEnter = true;
            }
            else if (OwnerType == OwnerType.Personal && OwnerID == player.Character.UUID) canPlayerEnter = true;

            return new HouseInfoDTO
            {
                ID = ID,
                Owner = GetHouseOwnerName(),
                Class = HouseManager.HouseTypeList.ContainsKey(Type) ? HouseManager.HouseTypeList[Type].Name : "Unknown",
                Roommates = HouseManager.MaxRoommates.ContainsKey(Type) ? HouseManager.MaxRoommates[Type] : 0,
                GarageSpace = HouseGarage == null ? 0 : HouseGarage.GarageConfig.MaxCars,
                Price = Price,
                IsSelled = OwnerID != -1,
                IsLocked = Locked,
                IsTarget = player.Character.HouseTarget == ID,
                CanEnter = canPlayerEnter,
                CamPositionX = CamPosition != null ? CamPosition.X : 0f,
                CamPositionY = CamPosition != null ? CamPosition.Y : 0f,
                CamPositionZ = CamPosition != null ? CamPosition.Z : 0f
            };
        }

        public void OnOwnerInteracted(ExtPlayer player, bool owner = true, bool reopen = true)
        {
            if (OwnerID == -1) return;

            HouseOwnerMenuDTO dto = new HouseOwnerMenuDTO(player, this, _roommates, owner);
            if (reopen) 
            {
                SafeTrigger.ClientEvent(player,"house::ownerInteracted", JsonConvert.SerializeObject(dto));
                return;
            }
            player.TriggerCefEvent("homeMenu/setFullState", JsonConvert.SerializeObject(dto));
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

        public void ProcessRentCostForOccupiers(DateTime time)
        {
            if (OwnerID < 0) return;
            if (time.Hour != HouseManager.HourTimeToProceedRoommatesRent) return;
            if (_roommates == null || !_roommates.Any()) return;
            if (RentCost <= 0) return;

            CheckingAccount ownerBankData = BankManager.GetAccountByUUID(OwnerID);
            if (ownerBankData == null) return;

            CheckingAccount bankData;
            ExtPlayer target;
            bool updateDb = false;
            List<Roommate> rommatesCopy = _roommates.ToList();
            foreach (Roommate roommate in rommatesCopy)
            {
                if (roommate == null) continue;

                target = roommate.Character;
                bankData = target == null ? BankManager.GetAccountByUUID(roommate.CharacterUUID) : target.Character.BankModel;
                if (MoneySystem.Wallet.TransferMoney(bankData, ownerBankData, RentCost, 0, "Оплата аренды")) continue;

                RemoveRoommate(roommate, false);
                updateDb = true;
            }
            if (!updateDb) return;
            
            UpdateRoommates();
        }

        public Roommate GetRoommate(int uuid)
        {
            return _roommates.FirstOrDefault(item => item.CharacterUUID == uuid);
        }

        public bool RoomatesAny()
        {
            return _roommates.Any();
        }

        public List<ExtPlayer> GetOnlineRoommates()
        {
            return _roommates.Select(item => Trigger.GetPlayerByUuid(item.CharacterUUID)).Where(item => item != null).ToList();
        }

        public void AddRoommate(Roommate roommate, bool updateDb = true)
        {
            if (_roommates.Contains(roommate)) return;

            _roommates.Add(roommate);
            if (updateDb) UpdateRoommates();
            CreateRoommateBlipAndMarker(roommate.CharacterUUID);
        }

        public void RemoveRoommate(ExtPlayer player, bool updateDb = true)
        {
            Roommate roommate = _roommates.FirstOrDefault(item => item.Character == player);
            if (roommate == null) return;

            RemoveRoommate(roommate, updateDb);
        }

        public void RemoveRoommate(int uuid, bool updateDb = true)
        {
            Roommate roommate = _roommates.FirstOrDefault(item => item.CharacterUUID == uuid);
            if (roommate == null) return;

            RemoveRoommate(roommate, updateDb);
        }

        private void RemoveRoommate(Roommate roommate, bool updateDb = true)
        {
            if (!_roommates.Contains(roommate)) return;
            
            _roommates.Remove(roommate);
            if (updateDb) UpdateRoommates();
            ExtPlayer player = roommate.Character == null ? Trigger.GetPlayerByUuid(roommate.CharacterUUID) : roommate.Character;
            if (player == null) return;

            DeleteRoommateBlipAndMarker(player);
            RemovePlayer(player);
            Notify.SendAlert(player, "newHouses_2");
        }
        public void RemoveRoommates()
        {
            if (!_roommates.Any()) return;

            ExtPlayer target;
            foreach (Roommate roommate in _roommates)
            {
                target = roommate.Character == null ? Trigger.GetPlayerByUuid(roommate.CharacterUUID) : roommate.Character;
                if (target == null) continue;

                DeleteRoommateBlipAndMarker(target);
                RemovePlayer(target);
                Notify.SendAlert(target, "newHouses_2");
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
            MySQL.Query("UPDATE `houses` SET `roommates` = @prop0 WHERE `id` = @prop1", JsonConvert.SerializeObject(_roommates), ID);
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

        public void SetCam(Vector3 campos)
        {
            CamPosition = campos;
            MySQL.Query("UPDATE `houses` SET `camposition` = @prop0 WHERE `id` = @prop1", JsonConvert.SerializeObject(CamPosition), ID);
        }
        #endregion

        #region CheckAccess
        public bool GetAccess(ExtPlayer player, FamilyHouseAccess access)
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
                            return OwnerID == player.Character.UUID || GetRoommate(player.Character.UUID) != null;
                        case FamilyHouseAccess.UpgradeGarage:
                        case FamilyHouseAccess.FullAccess:
                            return OwnerID == player.Character.UUID;
                        default:
                            break;
                    }
                    return OwnerID == player.Character.UUID || GetRoommate(player.Character.UUID) != null;
                case OwnerType.Family:
                    return FamilyManager.CanAccessToHouse(player, OwnerID, access);
            }
            return false;
        }
        public bool GetAccessFurniture(ExtPlayer player, FamilyFurnitureAccess access)
        {
            if (OwnerID <= 0)
                return false;
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    return OwnerID == player.Character.UUID;
                case OwnerType.Family:
                    return FamilyManager.CanAccessToFurniture(player, OwnerID, access);
            }
            return false;
        }

        public bool GetAccessToGarage(ExtPlayer player)
        {
            if (OwnerID <= 0)
                return false;
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    return OwnerID == player.Character.UUID || GetRoommate(player.Character.UUID) != null;
                case OwnerType.Family:
                    return OwnerID == player.Character.FamilyID;
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

            if (ownerId != -1)
            {
                OwnerType = ownerType;
                if (OwnerType == OwnerType.Family)
                {
                    var obj = new
                    {
                        type = TypeHouse.Name,
                        address = ""
                    };
                    string json = JsonConvert.SerializeObject(obj);
                    foreach (ExtPlayer member in FamilyManager.GetFamilyMembers(ownerId))
                    {
                        member.TriggerCefEvent("familyMenu/updateHouseData", json);
                    }
                }
            }
            else
            {
                if (OwnerID != -1 && OwnerType == OwnerType.Family)
                {
                    var obj = new
                    {
                        type = string.Empty,
                        address = ""
                    };
                    string json = JsonConvert.SerializeObject(obj);
                    foreach (ExtPlayer member in FamilyManager.GetFamilyMembers(OwnerID))
                    {
                        member.TriggerCefEvent("familyMenu/updateHouseData", json);
                    }
                }
                OwnerType = OwnerType.Personal;
            }
            OwnerID = ownerId;

            CreateClientBlipAndMarker();

            if (OwnerID != -1)
            {
                if (OwnerType == OwnerType.Personal)
                {
                    var player = Trigger.GetPlayerByUuid(OwnerID);
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
            Roommate roommateData = GetRoommate(uuid);
            if (roommateData != null) roommateData.SetCharacter(null);

            List<int> allPlayers = _roommates.Select(item => item.CharacterUUID).ToList();
            allPlayers.Add(OwnerID);
            if (allPlayers.Contains(uuid)) allPlayers.Remove(uuid);
            foreach (int roommate in allPlayers)
            {
                if (Trigger.GetPlayerByUuid(roommate) != null) return;
            }

            if (HouseGarage == null) return;

            HouseGarage.DestroyCars();
        }
        public void ConnectedPlayer()
        {
            NAPI.Task.Run(() =>
            {
                if (HouseGarage == null) return;

                HouseGarage.RespawnCars();
            }, 7000);
        }

        public void SendPlayer(ExtPlayer player)
        {
            player.ChangePosition(HouseManager.HouseTypeList[Type].Position + new Vector3(0, 0, 1.12));
            SafeTrigger.UpdateDimension(player,  Convert.ToUInt32(Dimension));
            player.Character.InsideHouseID = ID;
            if (!PlayersInside.Contains(player)) 
                PlayersInside.Add(player);
            HouseManager.PlayerEntered?.Invoke(player, this);
        }
        public void RemovePlayer(ExtPlayer player, bool exit = true)
        {
            if (player.Character.InsideHouseID != ID)
                return;
            if (exit)
            {
                player.ChangePosition(Position + new Vector3(0, 0, 1.12));
                SafeTrigger.UpdateDimension(player,  0);
            }
            player.Character.InsideHouseID = -1;

            RemoveFromList(player);
            HouseManager.PlayerLeaved?.Invoke(player);
        }

        public void RemoveFromList(ExtPlayer player)
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
                    SafeTrigger.UpdateDimension(player,  0);

                    player.Character.InsideHouseID = -1;
                    HouseManager.PlayerLeaved?.Invoke(player);
                }
            }
            PlayersInside = new List<ExtPlayer>();
        }

        #region Create Player Blips and Markers
        public void CreateClientBlipAndMarker()
        {
            if (OwnerID == -1)
                return;
            switch (OwnerType)
            {
                case OwnerType.Personal:
                    var player = Trigger.GetPlayerByUuid(OwnerID);
                    if (player != null)
                    {
                        player.CreateClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID, 40, "House", Position, 1, 73, 0);
                        player.CreateClientMarker(333, 42, HouseGarage?.Position ?? new Vector3() - new Vector3(0, 0, 0.5), 2, NAPI.GlobalDimension, new Color(182, 211, 0), new Vector3(90, 90, 90));
                        SafeTrigger.ClientEvent(player, "createGarageBlip", HouseGarage?.Position ?? new Vector3());
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
            CreateRoommateBlipAndMarker(Trigger.GetPlayerByUuid(uuid));
        }

        public void CreateRoommateBlipAndMarker(ExtPlayer player)
        {
            if (player == null) return;

            player.CreateClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID, 40, "House", Position, 1, 73, 0);
            player.CreateClientMarker(333, 42, HouseGarage?.Position ?? new Vector3() - new Vector3(0, 0, 0.5), 2, NAPI.GlobalDimension, new Color(182, 211, 0), new Vector3(90, 90, 90));
            SafeTrigger.ClientEvent(player, "createGarageBlip", HouseGarage?.Position ?? new Vector3());
        }

        public void CreateFamilyMemberMarker(ExtPlayer player)
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
                    var player = Trigger.GetPlayerByUuid(OwnerID);
                    if (player != null)
                    {
                        player.DeleteClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID);
                        player.DeleteClientMarker(333);
                        SafeTrigger.ClientEvent(player,"deleteGarageBlip");
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
        public void DeleteRoommateBlipAndMarker(ExtPlayer player)
        {            
            player.DeleteClientBlip(HouseManager.PERSONAL_HOUSE_BLIP_ID);
            player.DeleteClientMarker(333);
            SafeTrigger.ClientEvent(player,"deleteGarageBlip");            
        }
        public void DeleteFamilyMemberMarker(ExtPlayer player)
        {
            player.DeleteClientMarker(334);
        }

        #endregion
    }
}