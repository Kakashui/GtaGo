using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using WebSocketSharp;
using Whistler.Core.Pets.Models;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.Core.Pets
{
    internal class Controller : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Controller));
        public static readonly Dictionary<int, PetData> AllPets = new Dictionary<int, PetData>();

        [ServerEvent(Event.ResourceStart)]
        public void Event_ResourceStart()
        {
            Config.Initialize();

            DataTable result = MySQL.QueryRead("SELECT * FROM `pets`");
            if (result != null && result.Rows.Count != 0)
            {
                int ownerUuid;
                string name;
                uint model;
                int health;
                string positionData;
                uint dimension;
                Vector3 position;
                bool freeRename;
                PetData newPet;
                foreach (DataRow row in result.Rows)
                {
                    ownerUuid = (int)row["OwnerUuid"];
                    if (AllPets.ContainsKey(ownerUuid)) continue;

                    name = row.IsNull("Name") ? null : (string)row["Name"];
                    model = (uint)row["Model"];
                    health = (int)row["Health"];
                    positionData = row.IsNull("Position") ? null : row["Position"].ToString();
                    position = string.IsNullOrEmpty(positionData) ? null : JsonConvert.DeserializeObject<Vector3>(positionData);
                    dimension = (uint)row["Dimension"];
                    freeRename = (bool)row["FreeRename"];
                    newPet = new PetData(ownerUuid, name, model, health, position, dimension, freeRename);
                    AllPets.Add(ownerUuid, newPet);
                }
            }
            _logger.WriteInfo($"Pets ({AllPets.Count}) successfully initialized.");
        }

        [Command("givepet", GreedyArg = true)]
        public static void CMD_AdminGunsWithComponents(ExtPlayer player, int id, string petName)
        {
            if (!Group.CanUseAdminCommand(player, "givepet")) return;

            ExtPlayer target = Trigger.GetPlayerByUuid(id);
            if (target == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок с таким ID не найден", 3000);
                return;
            }

            if (!Config.PetsConfig.Any())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Животных в конфиге не найдено.", 3000);
                return;
            }

            PetConfig config = Config.PetsConfig.Values.FirstOrDefault(x => x.Name == petName);
            if (config == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Животного с таким именем не найдено, попробуйте иначе.", 3000);
                return;
            }

            PetData pet = GetPet(target);
            if (pet != null)
            {
                UnloadPlayerPet(target, true, true);
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Старое животное игрока было успешно удалено.", 3000);
            }

            CreatePet(target, config, true);
            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Новое животное было успешно выдано игроку.", 3000);
        }

        public static void InitializePlayerPet(ExtPlayer player, Vector3 position = null)
        {
            PetData pet = GetPet(player);
            if (pet == null) return;
            if (pet.Dead) return;

            ExtPed ped = NAPI.Ped.CreatePed(pet.Model, position ?? pet.Position ?? player.Position, 0f, dynamic: true, invincible: true, controlLocked: true, dimension: pet.Dimension) as ExtPed;
            if (ped == null) return;

            ped.SetSharedData("pet:isPet", true);
            ped.SetSharedData("pet:name", $"{pet.Name} ({player.Character.UUID})");
            ped.Controller = player;
            ped.OwnerUuid = player.Character.UUID;
            pet.Controller = player;
            pet.Pet = ped;
            SafeTrigger.ClientEvent(player, "client::initPet", ped, pet.Name, pet.Model);
            if (pet.Config == null) return;

            if (pet.Config.Heal > 0)
            {
                if (pet.Timers.ContainsKey("HealOwner")) Timers.Stop(pet.Timers["HealOwner"]);
                pet.Timers.Add("HealOwner", Timers.Start(3000, () =>
                {
                    Pet_HealOwner(player, pet);
                }));
            }
        }
        [RemoteEvent("server::pet:buyPet")]
        private static void Pet_BuyPet(ExtPlayer player, string petName)
        {
            if (player == null || !player.IsLogged()) return;
            if (!Config.PetsConfig.Any()) return;

            PetConfig config = Config.PetsConfig.Values.FirstOrDefault(x => x.Name == petName);
            if (config == null) return;

            PetData pet = GetPet(player);
            if (pet != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Одновременно можно владеть только одним питомцем.", 3000);
                return;
            }

            BuyPet(player, config);
        }

        public static bool BuyPet(ExtPlayer player, PetConfig petConfig)
        {
            if (DoesPetExist(player)) return false;
            if (petConfig.Price <= 0) return false;

            if (!player.SubMCoins(petConfig.Price))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "К сожалению, у Вас не хватает донат.валюты.", 3000);
                return false;
            }

            CreatePet(player, petConfig, true);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы успешно купили питомца {petConfig.Name} за {petConfig.Price} донат.валюты.", 5000);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Для взаимодействия с ним нажмите кнопку F5 вблизи от него и дайте команду. Ветеринар находится в больнице.", 10000);
            return true;
        }

        public static void CreatePet(ExtPlayer owner, PetConfig petConfig, bool initialize = false)
        {
            if (DoesPetExist(owner)) return;

            AllPets.Add(owner.Character.UUID, new PetData(owner.Character.UUID, petConfig.Name, petConfig.Model, petConfig.MaxHealth, owner.Position));
            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "INSERT INTO `pets` (`OwnerUuid`, `Name`, `Model`, `Health`, `Position`) VALUES (@owner, @name, @model, @health, @pos)"
            };
            cmd.Parameters.AddWithValue("@owner", owner.Character.UUID);
            cmd.Parameters.AddWithValue("@name", petConfig.Name);
            cmd.Parameters.AddWithValue("@model", petConfig.Model);
            cmd.Parameters.AddWithValue("@health", petConfig.MaxHealth);
            cmd.Parameters.AddWithValue("@pos", JsonConvert.SerializeObject(owner.Position));
            MySQL.Query(cmd);
            if (!initialize) return;
            
            InitializePlayerPet(owner, owner.Position);
        }

        private static void SavePet(PetData pet)
        {
            Vector3 petPosition = pet.Pet?.Position;
            pet.Position = petPosition;
            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "UPDATE `pets` SET `Name`=@name, `Model`=@model, `Health`=@health, `Position`=@position, `Dimension`=@dimension, `FreeRename`=@rename WHERE `OwnerUuid`=@uuid"
            };
            cmd.Parameters.AddWithValue("@name", pet.Name);
            cmd.Parameters.AddWithValue("@model", pet.Model);
            cmd.Parameters.AddWithValue("@health", pet.Health);
            cmd.Parameters.AddWithValue("@position", petPosition == null ? null : JsonConvert.SerializeObject(petPosition));
            cmd.Parameters.AddWithValue("@dimension", pet.Dimension);
            cmd.Parameters.AddWithValue("@rename", pet.FreeRename);
            cmd.Parameters.AddWithValue("@uuid", pet.OwnerUuid);
            MySQL.Query(cmd);
        }

        public static bool DeletePet(int ownerUuid)
        {
            if (!AllPets.ContainsKey(ownerUuid)) return false;

            MySqlCommand cmd = new MySqlCommand
            {
                CommandText = "DELETE FROM `pets` WHERE `OwnerUuid`=@uuid"
            };
            cmd.Parameters.AddWithValue("@uuid", ownerUuid);
            MySQL.Query(cmd);
            AllPets.Remove(ownerUuid);
            return true;
        }

        public static bool DoesPetExist(ExtPlayer owner)
        {
            if (owner == null || !owner.IsLogged()) return false;

            return DoesPetExist(owner.Character.UUID);
        }

        public static bool DoesPetExist(int ownerUuid)
        {
            return AllPets.ContainsKey(ownerUuid);
        }

        public static PetData GetPet(ExtPlayer owner)
        {
            if (!DoesPetExist(owner)) return null;

            return AllPets[owner.Character.UUID];
        }

        public static PetData GetPet(int ownerUuid)
        {
            if (!DoesPetExist(ownerUuid)) return null;

            return AllPets[ownerUuid];
        }

        public static void UnloadPlayerPet(ExtPlayer player, bool delete = false, bool stayInGame = false)
        {
            PetData pet = GetPet(player);
            if (pet == null) return;

            if (pet.Pet != null)
            {
                pet.Pet.Delete();
                pet.Pet = null;
            }
            if (pet.Timers.Any())
            {
                foreach (string timer in pet.Timers.Values)
                {
                    if (string.IsNullOrEmpty(timer)) continue;

                    Timers.Stop(timer);
                }
                pet.Timers.Clear();
            }
            pet.Controller = null;

            if (delete) DeletePet(pet.OwnerUuid);
            else SavePet(pet);

            if (stayInGame) SafeTrigger.ClientEvent(player, "client::unloadPet");
        }

        [RemoteEvent("server::pet:getBall")]
        private static void Pet_GetBall(ExtPlayer player, ExtPed ped)
        {
            ped?.SetSharedData("attachmentsData", JsonConvert.SerializeObject(new List<uint>()));

            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Ваш питомец принёс Вам мячик.", 3000);
        }

        private static void DoChat(ExtPlayer owner, string msg)
        {
            if (owner == null || !owner.IsLogged()) return;

            foreach (ExtPlayer p in owner.GetPlayersInRange(10, true))
            {
                SafeTrigger.ClientEvent(p, "chat:api:action", ChatType.Do, msg, owner.Value);
            }
        }

        [RemoteEvent("server::pet:sniffResult")]
        private static void Pet_SniffTarget(ExtPlayer player, ExtPlayer target)
        {
            if (player == null || target == null) return;
            if (!player.IsLogged() || !target.IsLogged()) return;

            Inventory.Models.InventoryModel inventory = target.GetInventory();
            if (inventory == null || !inventory.Items.Any())
            {
                DoChat(player, $"Питомец обнюхал {target.Name}");
                return;
            }

            foreach (Inventory.Models.BaseItem item in inventory.Items)
            {
                if (item == null) continue;
                if (!Police.IllegalItems.Contains(item.Type)) continue;

                DoChat(player, $"Питомец обнюхал {target.Name} и что-то учуял");
                return;
            }
            DoChat(player, $"Питомец обнюхал {target.Name}");
        }

        [RemoteEvent("server::pet:teleport")]
        private static void Pet_TeleportToPlayer(ExtPlayer player)
        {
            PetData pet = GetPet(player);
            if (pet == null || pet.Pet == null) return;

            pet.Pet.Position = player.IsInVehicle ? player.Position.Around(4f) : player.Position;
        }

        [RemoteEvent("server::pet:deleteBall")]
        private static void Pet_DeleteBall(ExtPlayer player, ExtPed ped, float xPos, float yPos, float zPos)
        {
            uint hash = NAPI.Util.GetHashKey("ball");
            ped?.SetSharedData("attachmentsData", JsonConvert.SerializeObject(new List<uint> { hash }));

            SafeTrigger.ClientEventInRange(new Vector3(xPos, yPos, zPos), 200f, "client::pet:deleteBall", xPos, yPos, zPos);
        }

        public static void Pet_UpdateDimension(ExtPlayer player, uint dimension)
        {
            PetData pet = GetPet(player);
            if (pet == null || pet.Pet == null) return;

            pet.Pet.Dimension = dimension;
            pet.Pet.Position = player.Position;
            //pet.Pet.Controller = player;
            pet.Dimension = dimension;
            SafeTrigger.ClientEvent(player, "client::pet:unfreeze");
        }

        public static void Pet_UpdateName(ExtPlayer owner, string name)
        {
            PetData pet = GetPet(owner);
            if (pet == null) return;

            if (string.IsNullOrEmpty(name) && pet.Config != null) name = pet.Config.Name;
            if (string.IsNullOrEmpty(name) || name.Length > 50 || !Regex.IsMatch(name, @"^[a-zA-Z\p{IsCyrillic} ]+$"))
            {
                Notify.Send(owner, NotifyType.Error, NotifyPosition.BottomCenter, "Неверный формат имени, пожалуйста, используйте только русские/английские символы и пробел. Длина имени не должна превышать 50 символов", 10000);
                return;
            }

            if (!pet.FreeRename && !Wallet.TransferMoney(owner.Character, Manager.GetFraction(8), Ems.RenamePetByBotPrice, 0, "Оплата за переименование питомца (NPC)"))
            {
                Notify.SendError(owner, "У Вас недостаточно денег с собой.");
                return;
            }
            pet.FreeRename = false;
            pet.Name = name;
            pet.Pet?.SetSharedData("pet:name", $"{name} ({owner.Character.UUID})");
            SafeTrigger.ClientEvent(owner, "client::pet:rename", name);
            Notify.Send(owner, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно изменили имя питомца.", 3000);
            SavePet(pet);
        }

        public static void Pet_ToggleFreeze(ExtPlayer owner, bool freeze = true)
        {
            PetData pet = GetPet(owner);
            if (pet == null || pet.Pet == null || pet.Dead) return;

            SafeTrigger.ClientEvent(owner, freeze ? "client::pet:freeze" : "client::pet:unfreeze");
        }

        public static void Pet_Sit(ExtPlayer owner)
        {
            PetData pet = GetPet(owner);
            if (pet == null || pet.Pet == null || pet.Dead) return;

            SafeTrigger.ClientEvent(owner, "client::pet:sit");
        }

        public static void Pet_Sleep(ExtPlayer owner)
        {
            PetData pet = GetPet(owner);
            if (pet == null || pet.Pet == null || pet.Dead) return;

            SafeTrigger.ClientEvent(owner, "client::pet:sleep");
        }

        [RemoteEvent("server::pet:attackPlayer")]
        private static void Pet_AttackPlayerByUuid(ExtPlayer owner, int targetUuid)
        {
            ExtPlayer target = Trigger.GetPlayerByUuid(targetUuid);
            if (target == null) return;

            Pet_AttackPlayer(owner, target.Value);
        }

        public static void Pet_AttackPlayer(ExtPlayer owner, int targetId, bool withNotify = true)
        {
            PetData pet = GetPet(owner);
            if (pet == null || pet.Pet == null || pet.Dead) return;

            SafeTrigger.ClientEvent(owner, "client::pet:attackPlayer", targetId, withNotify);
        }

        [RemoteEvent("server::pet:attackPet")]
        private static void Pet_AttackPetByUuid(ExtPlayer owner, int targetUuid)
        {
            ExtPlayer target = Trigger.GetPlayerByUuid(targetUuid);
            if (target == null) return;

            PetData pet = GetPet(target);
            if (pet == null || pet.Pet == null || pet.Dead) return;

            Pet_AttackPet(owner, pet.Pet.Value);
        }

        public static void Pet_AttackPet(ExtPlayer owner, int targetId, bool withNotify = true)
        {
            PetData pet = GetPet(owner);
            if (pet == null || pet.Pet == null || pet.Dead) return;

            SafeTrigger.ClientEvent(owner, "client::pet:attackPet", targetId, withNotify);
        }

        [RemoteEvent("server::pet:sniffTarget")]
        private static void Pet_SniffPlayerByUuid(ExtPlayer owner, int targetUuid)
        {
            ExtPlayer target = Trigger.GetPlayerByUuid(targetUuid);
            if (target == null) return;

            Pet_SniffPlayer(owner, target.Value);
        }

        public static void Pet_SniffPlayer(ExtPlayer owner, int targetId)
        {
            PetData pet = GetPet(owner);
            if (pet == null || pet.Pet == null || pet.Dead) return;

            SafeTrigger.ClientEvent(owner, "client::pet:sniff", targetId);
        }

        public static void Pet_FollowPlayer(ExtPlayer owner, int targetId)
        {
            PetData pet = GetPet(owner);
            if (pet == null || pet.Pet == null || pet.Dead) return;

            SafeTrigger.ClientEvent(owner, "client::pet:followTarget", targetId);
        }

        public static bool Pet_IsFarmBuff(ExtPlayer owner)
        {
            PetData pet = GetPet(owner);
            if (pet == null || pet.Pet == null) return false;
            if (pet.Dead) return false;

            return pet.Model == (uint)PedHash.Pig || pet.Model == (uint)PedHash.Boar;
        }

        [RemoteEvent("server::pet:dmgPetToPlayer")]
        private static void Pet_DamagePetToPlayer(ExtPlayer player, ExtPlayer target, ExtPed damager)
        {
            if (target == null || damager == null) return;

            PetData damagerPet = GetPet(damager.OwnerUuid);
            if (damagerPet == null || damagerPet.Dead || damagerPet.Config == null) return;
            if (damagerPet.Config.Damage <= 0) return;
            if (target.HasData("AGM") && target.GetData<bool>("AGM")) return;

            Pet_AttackPlayer(target, damager.Value, false);

            int tempHealth = target.Health;
            if (tempHealth <= 0) return;

            bool isInDeath = false;
            if (target.HasSharedData("InDeath")) isInDeath = target.GetSharedData<bool>("InDeath");
            if (isInDeath) return;

            target.Health = Math.Max(0, tempHealth - damagerPet.Config.Damage);
        }

        [RemoteEvent("server::pet:dmgPetToPet")]
        private static void Pet_DamagePetToPet(ExtPlayer player, ExtPed target, ExtPed damager)
        {
            if (target == null || damager == null) return;
            if (target == damager) return;

            PetData damagedPet = GetPet(target.OwnerUuid);
            if (damagedPet == null || damagedPet.Dead) return;

            PetData damagerPet = GetPet(damager.OwnerUuid);
            if (damagerPet == null || damagerPet.Dead || damagerPet.Config == null) return;
            if (damagerPet.Config.Damage <= 0) return;

            Pet_AttackPet(damagedPet.Controller, damager.Value, false);
            Pet_Damage(damagedPet.Controller, damagedPet, damagerPet.Config.Damage);
        }

        [RemoteEvent("server::pet:dmgPlayerToPet")]
        private static void Pet_DamagePlayerToPet(ExtPlayer player, ExtPed target)
        {
            if (target == null) return;

            PetData damagedPet = GetPet(target.OwnerUuid);
            if (damagedPet == null) return;
            if (damagedPet.Dead) return;
            if (damagedPet.Controller == null || damagedPet.Controller == player) return;
            if (damagedPet.Config.ImmuneToPlayerDamage) return;

            Pet_AttackPlayer(damagedPet.Controller, player.Value, false);
            Pet_Damage(damagedPet.Controller, damagedPet, 5);
        }

        private static void Pet_Damage(ExtPlayer owner, PetData pet, int damage)
        {
            if (pet.Dead) return;

            pet.Health -= damage;
            if (pet.Health > 0) return;

            pet.Health = 0;
            DoChat(owner, $"Питомец {pet.Name} мёртв");
            UnloadPlayerPet(owner, stayInGame: true);
            SafeTrigger.ClientEvent(owner, "client::pet:deathStatus", true);
            Notify.SendError(owner, "Ваш питомец был критически ранен и доставлен к ветеринару в больницу.");
        }

        private static void Pet_HealOwner(ExtPlayer owner, PetData pet)
        {
            if (owner == null || !owner.IsLogged() || pet.Dead || pet.Config == null || pet.Config.Heal <= 0)
            {
                if (pet.Timers.ContainsKey("HealOwner")) 
                {
                    Timers.Stop(pet.Timers["HealOwner"]);
                    pet.Timers.Remove("HealOwner");
                }
                return;
            }

            int tempHealth = owner.Health;
            if (tempHealth <= 0 || tempHealth >= 100) return;

            bool isInDeath = false;
            if (owner.HasSharedData("InDeath")) isInDeath = owner.GetSharedData<bool>("InDeath");
            if (isInDeath) return;

            owner.Health = Math.Min(100, tempHealth + pet.Config.Heal);
        }

        public static void Pet_Rename(ExtPlayer owner)
        {
            if (owner == null || !owner.IsLogged()) return;

            SafeTrigger.ClientEvent(owner, "openInput", "Переименовать питомца", "Введите новое имя", 50, "petRename");
        }

        public static void Pet_BuyToy(ExtPlayer owner)
        {
            if (owner == null || !owner.IsLogged()) return;

            Inventory.Models.InventoryModel inventory = owner.GetInventory();
            if (inventory == null) return;

            Inventory.Models.BaseItem item = inventory.GetItemLink(ItemNames.Ball);
            if (item != null)
            {
                Notify.SendError(owner, "У Вас уже есть мячик с собой");
                return;
            }

            item = ItemsFabric.CreateWeapon(ItemNames.Ball, false);
            if (item == null) return;

            if (!Wallet.TransferMoney(owner.Character, Manager.GetFraction(8), Ems.ToyPetByBotPrice, 0, "Оплата за игрушку питомца (NPC)"))
            {
                Notify.SendError(owner, "У Вас недостаточно денег с собой.");
                return;
            }

            inventory.AddItem(item);
            Notify.SendSuccess(owner, "Вы успешно купили мячик для собаки.");
        }

        public static void Pet_Revive(ExtPlayer owner)
        {
            if (owner == null || !owner.IsLogged()) return;

            PetData pet = GetPet(owner.Character.UUID);
            if (pet == null) return;
            if (pet.Config == null) return;

            if (pet.Health >= pet.Config.MaxHealth)
            {
                Notify.SendError(owner, "Ваш питомец не нуждается в лечении.");
                return;
            }
            if (!Wallet.TransferMoney(owner.Character, Manager.GetFraction(8), Ems.HealPetByBotPrice, 0, "Оплата за лечение питомца (NPC)"))
            {
                Notify.SendError(owner, "У Вас недостаточно денег с собой.");
                return;
            }

            bool deadStatus = pet.Dead;
            pet.Health = pet.Config.MaxHealth;
            if (!deadStatus)
            {
                Notify.SendSuccess(owner, "Питомец успешно вылечен.");
                return;
            }

            DoChat(owner, $"Питомец {pet.Name} жив");
            InitializePlayerPet(owner, new Vector3(302.33572, -596.3623, 43.129753).Around(2f));
            SafeTrigger.ClientEvent(owner, "client::pet:deathStatus", false);
            Notify.SendSuccess(owner, "Питомец успешно вылечен, он ожидает новой команды.");
            SavePet(pet);
        }
    }
}
