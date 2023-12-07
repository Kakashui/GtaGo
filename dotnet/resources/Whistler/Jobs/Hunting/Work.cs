using GTANetworkAPI;
using Whistler.SDK;
using Whistler.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Jobs.Hunting
{
    internal delegate void UpdateAnimalPositionHandler(int animalId, Vector3 newCoords);
    internal delegate void UpdateAnimalStateHandler(int animalId, int newState);

    internal partial class Work : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Work));
        private const int SpawnPeriodicityInMinutes = 1;

        public static event Action AnimalSpawnTick;
        public static event UpdateAnimalPositionHandler AnimalUpdatePosition;
        public static event UpdateAnimalPositionHandler AnimalSetPosition;
        public static event UpdateAnimalStateHandler AnimalStateUpdate;
        public static event Action<ExtPlayer> PlayerDisconnected;
        public static event Action<int, Vector3> AnimalShootedFromPosition;
        public static event Action<int, int> AnimalDecreaseHp;
        public static event Action<ExtPlayer, int> AnimalHautByPlayer;
        public static event Action AnimalAntiBug;

        private readonly static List<HuntingGround> _huntingGrounds = new List<HuntingGround>();
        private Shop _shop;

        public static HuntingGround GetHuntingGroundWithPlayer(ExtPlayer player)
        {
            return _huntingGrounds.Find(h => h.IsPlayerOnGround(player));
        }

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStartHandler()
        {
            // _huntingGrounds.Add(new HuntingGround(new Vector3(-1497.328, 4578.897, 35.26891), 200, true));
            // _huntingGrounds.Add(new HuntingGround(new Vector3(-512, 5170, 90), 200, true));
            // _huntingGrounds.Add(new HuntingGround(new Vector3(1119, 4723, 97), 200, false));

            // _shop = new Shop(new Vector3(-2190.417, 4276.241, 48.1743));

            // Timers.Start(SpawnPeriodicityInMinutes * 60 * 1000, HandleAnimalSpawWhistlerTimer);
            // Timers.Start(60 * 1000, HandleCheckIfBuggedAnimal);
        }

        public static List<Vector3> GetHuntingGroundsPositions()
        {
            return _huntingGrounds.Select(h => h.CenterPosition).ToList();
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void PlayerDisconnectedHandler(ExtPlayer player, DisconnectionType type, string reason)
        {
            try
            {
                PlayerDisconnected?.Invoke(player);
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on 'Event.PlayerDisconnected' {e}"); }
        }

        public void HandleCheckIfBuggedAnimal()
        {
            AnimalAntiBug?.Invoke();
        }

        public void HandleAnimalSpawWhistlerTimer()
        {
            AnimalSpawnTick?.Invoke();
        }

        [RemoteEvent("hunting:hautAnimal")]
        public void HautAnimal(ExtPlayer player, int animalId)
        {
            try
            {
                AnimalHautByPlayer?.Invoke(player, animalId);
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on 'hunting:hautAnimal' {e}"); }
        }

        [RemoteEvent("hunting:decreaseHp")]
        public void DecreaseHpHandler(ExtPlayer player, int animalId, int hpToTakeAway)
        {
            try
            {
                AnimalDecreaseHp?.Invoke(animalId, hpToTakeAway);
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on 'hunting:decreaseHp' {e}"); }
        }

        [RemoteEvent("hunting:shootAnimal")]
        public void ShootAnimalHandler(ExtPlayer player, int animalId, float x, float y, float z)
        {
            try
            {
                AnimalShootedFromPosition?.Invoke(animalId, new Vector3(x, y, z));
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on 'hunting:shootAnimal' {e}"); }
        }

        [RemoteEvent("hunting:setAnimalPosition")]
        public void SetAnimalPositionHandler(ExtPlayer player, int animalId, float x, float y, float z)
        {
            try
            {
                AnimalSetPosition?.Invoke(animalId, new Vector3(x, y, z));
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on 'hunting:setAnimalPosition' {e}"); }
        }

        [RemoteEvent("hunting:updateAnimal")]
        public void UpdateAnimalHandler(ExtPlayer player, int animalId, float x, float y, float z)
        {
            try
            {
                AnimalUpdatePosition?.Invoke(animalId, new Vector3(x, y, z));
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on 'hunting:updateAnimal' {e}"); }
        }

        [RemoteEvent("hunting:updateAnimalState")]
        public void UpdateAnimalStateHandler(ExtPlayer player, int animalId, int newState)
        {
            try
            {
                AnimalStateUpdate?.Invoke(animalId, newState);
            }
            catch (Exception e) { _logger.WriteError($"Unhandled error catched on 'hunting:updateAnimal' {e}"); }
        }
    }
}
