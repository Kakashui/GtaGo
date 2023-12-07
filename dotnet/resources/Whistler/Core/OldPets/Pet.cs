using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Core.OldPets
{
    public class Pet
    {
        public int ID { get; }
        public int Model { get; }

        private ExtPlayer _controller;
        private List<ExtPlayer> _clients = new List<ExtPlayer>();

        private State _currentState = State.Sit;
        private Vector3 _currentPosition;

        public Pet(int id, ExtPlayer owner, int model)
        {
            ID = id;
            Model = model;
            _controller = owner;
            _currentPosition = owner.Position;

            owner.SetSharedData("pets:id", ID);
            SafeTrigger.ClientEvent(owner, "pets:setControlledPet", JsonConvert.SerializeObject(GetDto()));

            var players = NAPI.Pools.GetAllPlayers().Where(c => c != owner && c.Dimension == owner.Dimension && c.Position.DistanceTo(owner.Position) < 250).Cast<ExtPlayer>().ToList();
            foreach (var player in players)
            {
                LoadForPlayer(player);
            }

            PetsManager.PetLoad += (petId, p) => { if (petId == ID) LoadForPlayer(p); };
            PetsManager.PetUnload += (petId, p) => { if (petId == ID) UnloadForPlayer(p); };
            PetsManager.PetMoveToPosition += (petId, pos, speed) => { if (petId == ID) MoveToPosition(pos, speed); };
            PetsManager.PetChangeState += (petId, state) => { if (petId == ID) ChangeState(state); };
            PetsManager.PetSetPosition += (petId, pos) => { if (petId == ID) SetPosition(pos); };

            Main.PlayerPreDisconnect += (player) => {
                UnloadForPlayer(player);
            };
        }

        public void Destroy()
        {
            SafeTrigger.ClientEvent(_controller, "pets:unloadControlledPed");
            _controller.ResetSharedData("pets:id");

            SafeTrigger.ClientEventToPlayers(_clients.ToArray(), "pets:unloadPet", ID);
            _clients.Clear();
        }

        private void ChangeState(State state)
        {
            _currentState = state;
            SafeTrigger.ClientEventToPlayers(_clients.ToArray(), "pets:setState", ID, state);
        }

        private void MoveToPosition(Vector3 position, float speed)
        {
            _currentPosition = position;
            SafeTrigger.ClientEventToPlayers(_clients.ToArray(), "pets:move", ID, position, speed);
        }

        private void SetPosition(Vector3 position)
        {
            _currentPosition = position;
            SafeTrigger.ClientEventToPlayers(_clients.ToArray(), "pets:setPosition", ID, position);
        }

        private void UnloadForPlayer(ExtPlayer player, bool notifyClient = false)
        {
            _clients.Remove(player);

            if (notifyClient)
            {
                SafeTrigger.ClientEvent(player,"pets:unloadPet", ID);
            }
        }

        private void LoadForPlayer(ExtPlayer player)
        {
            if (!_clients.Contains(player))
            {
                SafeTrigger.ClientEvent(player,"pets:loadPet", JsonConvert.SerializeObject(GetDto()));
                _clients.Add(player);
            }
        }

        public enum State
        {
            Sit,
            Stay,
            Follow,
            Hunt
        }

        public PedDto GetDto() => new PedDto
        {
            ID = ID,
            Model = Model,
            Position = _currentPosition,
            ControllerId = _controller.Value,
            State = (int)_currentState
        };
    }
}
