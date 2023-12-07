using AutoMapper.Configuration.Conventions;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Helpers;
using Whistler.Jobs.Hunting.Dtos;
using Newtonsoft.Json;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Inventory.Enums;
using Whistler.Inventory;
using Whistler.Entities;

namespace Whistler.Jobs.Hunting
{
    internal class Animal
    {
        private readonly static Random Random = new Random();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Animal));
        private readonly static List<int> AvailableModels = new List<int>()
        {
            -832573324,
            1682622302,
            -664053099
        };
        private static int LastAnimalId = 1;

        public int ID { get; }
        public int HP { get; private set; } = 100;
        public Vector3 Position { get => new Vector3(_currentPosition.X, _currentPosition.Y, _currentPosition.Z); }

        public event Action<Animal> Destroy;
        
        private readonly int _model;
        private readonly HuntingGround _huntingGround;

        private List<ExtPlayer> _clients = new List<ExtPlayer>();
        private ExtPlayer _controller;
        private Vector3 _currentPosition;
        private State _state = State.Spawn;
        private DateTime _spawnDate = DateTime.Now;

        public Animal(HuntingGround huntingGround, List<ExtPlayer> clients)
        {
            ID = LastAnimalId++;
            _model = AvailableModels[Random.Next(0, AvailableModels.Count)];

            _huntingGround = huntingGround;
            _currentPosition = GetSpawnPosition(huntingGround.CenterPosition, huntingGround.Range);

            _huntingGround.PlayerEnterGround += LoadForPlayer;
            _huntingGround.PlayerExitGround += UnloadForPlayer;

            Work.AnimalUpdatePosition += HandleUpdate;
            Work.AnimalSetPosition += HandleSetPosition;
            Work.AnimalStateUpdate += HandleUpdateState;
            Work.AnimalShootedFromPosition += HandleShootFromPosition;
            Work.AnimalDecreaseHp += HandleDecreaseHp;
            Work.AnimalHautByPlayer += HandleHaut;
            Work.AnimalAntiBug += CheckBugged;

            clients.ForEach(c => LoadForPlayer(c));
        }

        private void CheckBugged()
        {
            if ((DateTime.Now - _spawnDate).TotalMinutes >= 10)
            {
                UnsubscribeFromEvents();
                UnloadForAll();
                Destroy?.Invoke(this);
            }
        }

        private void UnsubscribeFromEvents()
        {
            _huntingGround.PlayerEnterGround -= LoadForPlayer;
            _huntingGround.PlayerExitGround -= UnloadForPlayer;

            Work.AnimalUpdatePosition -= HandleUpdate;
            Work.AnimalSetPosition -= HandleSetPosition;
            Work.AnimalStateUpdate -= HandleUpdateState;
            Work.AnimalShootedFromPosition -= HandleShootFromPosition;
            Work.AnimalDecreaseHp -= HandleDecreaseHp;
            Work.AnimalHautByPlayer -= HandleHaut;
            Work.AnimalAntiBug -= CheckBugged;
        }

        private void LoadForPlayer(ExtPlayer player)
        {
            if (_controller is null)
            {
                _controller = player;
            }
            else
            {
                _clients.Add(player);
            }

            var animalDto = new AnimalDto
            {
                ID = ID,
                Model = _model,
                Position = _currentPosition,
                State = _state,
                IsController = _controller == player
            };

            player.TriggerEventSafe("hunting:loadAnimal", JsonConvert.SerializeObject(animalDto));
        }

        private void UnloadForPlayer(ExtPlayer player)
        {
            if (player == null) return;
            if (_controller == player)
            {
                _controller = null;
                SetNewController();
            }
            else
            {
                _clients.Remove(player);
            }

            player.TriggerEventSafe("hunting:unloadAnimal", ID);
        }

        private void SetNewController()
        {
            var client = _clients.FirstOrDefault();

            if (client != null)
            {
                _clients.Remove(client);
                _controller = client;

                SafeTrigger.ClientEvent(client, "hunting:setController", ID);
            }
        }

        private Vector3 GetSpawnPosition(Vector3 centerPosition, float range)
        {
            var randomX = Random.Next(0, (int)range);
            var randomY = Random.Next(0, (int)range);

            return centerPosition + new Vector3(randomX, randomY, 0);
        }

        private void UnloadForAll()
        {
            _clients.ToList().ForEach(c => UnloadForPlayer(c));
            UnloadForPlayer(_controller);
        }

        private void HandleHaut(ExtPlayer player, int animalId)
        {
            try
            {
                if (animalId != ID) return;

                if (!player.GetEquip().IsCurrent(ItemNames.Knife))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "hunting_11", 3000);
                    return;
                }

                UnsubscribeFromEvents();

                Main.OnAntiAnim(player);
                player.PlayAnimation("amb@world_human_gardener_plant@male@base", "base", 39);

                NAPI.Task.Run(() =>
                {
                    Main.OffAntiAnim(player);
                    player.StopAnimation();

                    if (!player.GetInventory().AddItem(ItemsFabric.CreateOther(ItemNames.AnimalSkin, 1, false)))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "Core_185", 3000);
                        return;
                    }

                    Notify.Send(player, NotifyType.Success, NotifyPosition.Bottom, "hunting_12", 3000);

                    UnloadForAll();
                }, 3000);

                Destroy?.Invoke(this);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
            
        }

        private void HandleDecreaseHp(int animalId, int hpToTake)
        {
            if (animalId != ID) return;

            HP -= hpToTake;
            //Console.WriteLine($"Hp decrease on {hpToTake}. Now {HP}");
            if (HP <= 0)
            {
                SafeTrigger.ClientEvent(_controller, "hunting:handleAnimalDeath", ID);
            }
        }

        private void HandleShootFromPosition(int animalId, Vector3 position)
        {
            if (animalId != ID) return;

            _controller.TriggerEventSafe("hunting:handleShoot", ID, position);
        }

        private void HandleSetPosition(int animalId, Vector3 newCoords)
        {
            if (animalId != ID) return;

            _currentPosition = newCoords;

            SafeTrigger.ClientEventToPlayers(_clients.ToArray(), "hunting:setAnimalPosition", ID, newCoords);
        }

        private void HandleUpdateState(int animalId, int newState)
        {
            if (animalId != ID) return;

            _state = (State)newState;

            SafeTrigger.ClientEventToPlayers(_clients.ToArray(), "hunting:setState", ID, newState);
        }

        private void HandleUpdate(int animalId, Vector3 newCoords)
        {
            if (animalId != ID) return;

            _currentPosition = newCoords;

            SafeTrigger.ClientEventToPlayers(_clients.ToArray(), "hunting:updateAnimal", ID, newCoords);
        }

        internal enum State
        {
            Spawn = -1,
            Stand,
            Walk,
            Dead
        }
    }
}