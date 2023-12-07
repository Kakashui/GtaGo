using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Whistler.Core.Pets
{
    public class Pet
    {
        public int ID { get; }
        public int Model { get; }

        private Player _controller;
        private List<Player> _clients = new List<Player>();

        private State _currentState = State.Sit;
        private Vector3 _currentPosition;

        public Pet(int id, Player owner, int model)
        {
            ID = id;
            Model = model;
            _controller = owner;
            _currentPosition = owner.Position;

            owner.SetSharedData("pets:id", ID);
            owner.TriggerEvent("pets:setControlledPet", JsonConvert.SerializeObject(GetDto()));

            var players = NAPI.Pools.GetAllPlayers().Where(c => c != owner && c.Dimension == owner.Dimension && c.Position.DistanceTo(owner.Position) < 250);
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
            _controller.TriggerEvent("pets:unloadControlledPed");
            _controller.ResetSharedData("pets:id");

            Trigger.ClientEventToPlayers(_clients.ToArray(), "pets:unloadPet", ID);
            _clients.Clear();
        }

        private void ChangeState(State state)
        {
            _currentState = state;
            Trigger.ClientEventToPlayers(_clients.ToArray(), "pets:setState", ID, state);
        }

        private void MoveToPosition(Vector3 position, float speed)
        {
            _currentPosition = position;
            Trigger.ClientEventToPlayers(_clients.ToArray(), "pets:move", ID, position, speed);
        }

        private void SetPosition(Vector3 position)
        {
            _currentPosition = position;
            Trigger.ClientEventToPlayers(_clients.ToArray(), "pets:setPosition", ID, position);
        }

        private void UnloadForPlayer(Player player, bool notifyClient = false)
        {
            _clients.Remove(player);

            if (notifyClient)
            {
                player.TriggerEvent("pets:unloadPet", ID);
            }
        }

        private void LoadForPlayer(Player player)
        {
            if (!_clients.Contains(player))
            {
                player.TriggerEvent("pets:loadPet", JsonConvert.SerializeObject(GetDto()));
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
