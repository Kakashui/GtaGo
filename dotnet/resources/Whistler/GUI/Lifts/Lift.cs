using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.GUI.Lifts
{
    internal class Lift
    {
        private static int _lastId = 0;
        private static Dictionary<int, Lift> _liftList = new Dictionary<int, Lift>();
        private static object _lockObject = new object();
        public static Lift Create(Func<ExtPlayer, bool> enterPredicate = null)
        {
            int id;
            lock (_lockObject)
            {
                 id = ++_lastId;
            }
            Lift lift = new Lift(id, enterPredicate);
            _liftList.Add(id, lift);
            return lift;
        }
        public static void LiftPressButton(ExtPlayer player, int liftId, int floor)
        {
            if (!_liftList.ContainsKey(liftId))
                return;
            _liftList[liftId].PressButton(player, floor);
        }

        private int Id;
        private Func<ExtPlayer, bool> _enterPredicate;
        private List<Floor> _floors = new List<Floor>();
        public Lift(int id, Func<ExtPlayer, bool> enterPredicate)
        {
            Id = id;
            _enterPredicate = enterPredicate;
        }

     
        public Lift AddFloor(string name, Vector3 position, Vector3 rotation = null, uint dimension = 0, bool marker = true, Func<ExtPlayer, bool> enterPredicate = null, Func<ExtPlayer, bool> exitPredicate = null, bool input = true, bool exit = true)
        {
            Floor floor = new Floor(name, position, rotation, dimension, exitPredicate, exit);
            if (input)
                floor.AddInteract(OpenLift, enterPredicate == null ? _enterPredicate : enterPredicate, marker);
            _floors.Add(floor);
            return this;
        }

        public static bool DeleteLift(Lift lift)
        {
            if (_liftList.ContainsKey(lift.Id))
            {
                lift.Destroy();
                _liftList.Remove(lift.Id);
                return true;
            }
            return false;
        }
        public bool DeleteLift()
        {
            if (_liftList.ContainsKey(Id))
            {
                Destroy();
                _liftList.Remove(Id);
                return true;
            }
            return false;
        }

        private void OpenLift(ExtPlayer player)
        {
            if (player.IsInVehicle) return;
            if (player.Character.Following != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_120", 3000);
                return;
            }
            SafeTrigger.ClientEvent(player, "openliftmenu", Id, JsonConvert.SerializeObject(_floors.Where(item => item.IsExit(player)).Select(item => item.Name)));
        }
        private void PressButton(ExtPlayer player, int floor)
        {
            var floors = _floors.Where(item => item.IsExit(player)).ToList();
            if (floors.Count < floor)
                return;
            floors[floor].GoToFloor(player);
        }

        private void Destroy()
        {
            _floors.ForEach(floor =>
            {
                floor.Destroy();
            });
        }

    }
}
