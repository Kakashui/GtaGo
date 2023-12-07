using GTANetworkAPI;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Whistler.Inventory;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Models;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Core.OldPets
{
    internal delegate void PetHandler(int petId, ExtPlayer player);
    internal delegate void PetChangeStateHandler(int petId, Pet.State state);
    internal delegate void PetMoveToPositionHandler(int petId, Vector3 position, float speed);
    internal delegate void PetSetPositionHandler(int petId, Vector3 position);

    internal class PetsManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PetsManager));
        private readonly static Dictionary<int, Pet> _pets = new Dictionary<int, Pet>();

        private static int _lastPetID = 1;

        public static event PetHandler PetLoad;
        public static event PetHandler PetUnload;
        public static event PetMoveToPositionHandler PetMoveToPosition;
        public static event PetSetPositionHandler PetSetPosition;
        public static event PetChangeStateHandler PetChangeState;

        [RemoteEvent("pets:loadPet")]
        public void LoadPetForPlayer(ExtPlayer player, int petId)
        {
            try
            {
                PetLoad?.Invoke(petId, player);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception cougth on pets:loadPet: " + e.ToString()); }
        }

        [RemoteEvent("pets:unloadPet")]
        public void UnloadPetForPlayer(ExtPlayer player, int petId)
        {
            try
            {
                PetUnload?.Invoke(petId, player);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception cougth on pets:unloadPet: " + e.ToString()); }
        }

        [RemoteEvent("pets:moveToPosition")]
        public void MovePetPosition(ExtPlayer player, int petId, float x, float y, float z, float speed)
        {
            try
            {
                PetMoveToPosition?.Invoke(petId, new Vector3(x, y, z), speed);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception cougth on pets:syncPosition: " + e.ToString()); }
        }

        [RemoteEvent("pets:setPosition")]
        public void SetPetPosition(ExtPlayer player, int petId, float x, float y, float z)
        {
            try
            {
                PetSetPosition?.Invoke(petId, new Vector3(x, y, z));
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception cougth on pets:syncPosition: " + e.ToString()); }
        }

        [RemoteEvent("pets:syncState")]
        public void SyncPetState(ExtPlayer player, int petId, int newState)
        {
            try
            {
                PetChangeState?.Invoke(petId, (Pet.State)newState);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception cougth on pets:syncState: " + e.ToString()); }
        }

        [RemoteEvent("pets:destroyPet")]
        public void DestoryPet(ExtPlayer player)
        {
            try
            {
                UnloadPlayerPet(player);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception cougth on pets:syncState: " + e.ToString()); }
        }

        [ServerEvent(Event.ResourceStart)]
        public void HandleResourceStart()
        {
            try
            {
                Main.PlayerPreDisconnect += UnloadPlayerPet;
                InventoryService.OnUseAnimal += UsePet;
                //Items.ItemUse += UsePet;
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception cougth on ResourceStart: " + e.ToString()); }
        }

        private void UsePet(ExtPlayer player, Animal item)
        {
            if (item.IsActive)
            {
                UnloadPlayerPet(player);
            }
            else
            {
                var activePetItem = player.GetInventory().Items.FirstOrDefault(it => it.Type == Inventory.Enums.ItemTypes.Animal && (it as Animal).IsActive);
                if (activePetItem != null)
                {
                    UnloadPlayerPet(player);
                }
                int model = item.PedHash;
                item.IsActive = true;
                SpawnPet(player, model);
            }
        }

        private void UnloadPlayerPet(ExtPlayer player)
        {
            try
            {
                var findItem = player.GetInventory().Items.FirstOrDefault(it => it.Type == Inventory.Enums.ItemTypes.Animal && (it as Animal).IsActive);
                if (findItem != null)
                {
                    (findItem as Animal).IsActive = false;
                }

                if (player.HasSharedData("pets:id"))
                {
                    int id = player.GetSharedData<int>("pets:id");
                    UnloadPet(id);
                }
            }
            catch (Exception e) { _logger.WriteError("UnloadPlayerPet: " + e.ToString()); }
        }

        private void UnloadPet(int petId)
        {
            if (_pets.ContainsKey(petId))
            {
                var pet = _pets[petId];
                pet.Destroy();

                _pets.Remove(petId);
            }
        }

        public static void SpawnPet(ExtPlayer owner, int petModel)
        {
            var pet = new Pet(_lastPetID++, owner, petModel);
            _pets.Add(pet.ID, pet);
        }
    }
}
