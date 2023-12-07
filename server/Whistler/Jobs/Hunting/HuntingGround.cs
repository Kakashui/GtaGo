using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Whistler.Jobs.Hunting
{
    internal class HuntingGround
    {
        private const int MaxAnimalsOnHuntingGround = 5;
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(HuntingGround));

        public Vector3 CenterPosition { get; }
        public float Range { get; }

        public event Action<Player> PlayerEnterGround;
        public event Action<Player> PlayerExitGround;

        private ColShape _colshape;
        private List<Animal> _animals = new List<Animal>();
        private List<Player> _clientsOnGround = new List<Player>();

        public HuntingGround(Vector3 centerPosition, float range, bool blip)
        {
            CenterPosition = centerPosition;
            Range = range;
            if (blip)
                NAPI.Blip.CreateBlip(141, centerPosition, 2, 56, "Охота", shortRange: true);
            _colshape = NAPI.ColShape.CreatCircleColShape(CenterPosition.X, CenterPosition.Y, Range + 100, 0);
            _colshape.OnEntityEnterColShape += (s, c) => EnterHuntingGround(c);
            _colshape.OnEntityExitColShape += (s, c) => ExitHuntingGround(c);

            Work.AnimalSpawnTick += SpawnAnimal;
            Work.PlayerDisconnected += ExitHuntingGround;
        }

        public List<Vector3> GetAnimalsPositions()
        {
            return _animals.ToList().Select(a => a.Position).ToList();
        }

        public bool IsPlayerOnGround(Player player)
        {
            return _clientsOnGround.Contains(player);
        }

        private void SpawnAnimal()
        {
            if (_animals.Count >= MaxAnimalsOnHuntingGround)
            {
                return;
            }

            var animal = new Animal(this, _clientsOnGround.ToList());
            animal.Destroy += HandleAnimalDestroy;

            _animals.Add(animal);
        }

        private void HandleAnimalDestroy(Animal animal)
        {
            if (_animals.Contains(animal))
            {
                _animals.Remove(animal);
            }
        }

        private void EnterHuntingGround(Player client)
        {
            try
            {
                if (!client.IsLogged())
                {
                    return;
                }

                PlayerEnterGround?.Invoke(client);
                _clientsOnGround.AddIfNotExists(client);

                client.TriggerEvent("hunting:setOnHuntingGround", true, CenterPosition, Range);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception caught on 'EnterHuntingGround': " + e.ToString()); }
        }

        private void ExitHuntingGround(Player client)
        {
            try
            {
                PlayerExitGround?.Invoke(client);
                _clientsOnGround.Remove(client);

                client.TriggerEvent("hunting:setOnHuntingGround", false);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception caught on 'ExitHuntingGround': " + e.ToString()); }
        }
    }
}
