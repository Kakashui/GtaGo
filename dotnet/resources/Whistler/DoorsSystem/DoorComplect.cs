using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.DoorsSystem
{
    public class DoorComplect
    {
        public DoorComplect(string name, Vector3 actionPoint, List<Door> doors, bool locked, bool iterract = false)
        {
            Name = name;
            ActionPoint = actionPoint;
            Doors = doors;
            Locked = locked;
            DefaultState = locked;
            Hash = (int)NAPI.Util.GetHashKey(name);
            Interract = iterract;
        }
        public string Name { get; set; }
        public int Hash { get; set; }
        public bool Locked { get; set; }
        public bool DefaultState { get; set; }
        public bool Interract { get; set; }
        public Vector3 ActionPoint { get; set; }
        public List<Door> Doors { get; set; }
        public string Access { get; set; }

        public void SetState(bool state)
        {
            Locked = state;
            NAPI.ClientEvent.TriggerClientEventForAll("doors:state:set", Hash, Locked);
        }
        public void UpdateStateForPlayer(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"doors:state:set", Hash, Locked);
        }

        public bool IsChanged()
        {
            return DefaultState != Locked;
        }
    }
}
