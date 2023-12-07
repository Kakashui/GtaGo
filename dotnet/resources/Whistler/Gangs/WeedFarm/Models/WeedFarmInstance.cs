using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Fractions.Models;
using Whistler.SDK;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.Inventory.Models;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Configs;
using Whistler.VehicleSystem.Models;
using Whistler.VehicleSystem;
using Whistler.Entities;

namespace Whistler.Gangs.WeedFarm.Models
{
    internal class WeedFarmInstance
    {
        public WeedFarmInstance(Vector3 enterPoint, Vector3 vehiclePosition, Vector3 vehicleRotation)
        {
            EnterPoint = enterPoint;
            VehiclePosition = vehiclePosition;
            VehicleRotation = vehicleRotation;
            Dimension = Dimensions.RequestPrivateDimension();
            _subscribers = new List<ExtPlayer>();
            Main.DatabaseSave += Save;
        }
        public int Id { get; set; }
        public Vector3 EnterPoint { get; set; }
        public Vector3 VehiclePosition { get; set; }
        public Vector3 VehicleRotation { get; set; }
        public uint Dimension { get; set; }
        public Fraction Owner { get; set; }
        public DateTime OccupationDate { get; set; }
        public List<int> Components { get; set; }
        public List<int> Places { get; set; }
        public string BattleTimerKey { get; set; }

        public int OnDrying { get; set; }
        public int OnPacking { get; set; }
        public Blip Blip { get; set; }
        public int OnDelivery
        {
            get
            {
                var item = Inventory.Items.FirstOrDefault(i => i.Name == ItemNames.Marijuana);
                return item == null ? 0 : item.Count;
            }
            set
            {
                if(Inventory == null)
                {
                    //var weight = WeedFarmService.PlacePoints.Count * WeedFarmService.PlanstOnSpot * NarcoticConfigs.Config[ItemNames.Marijuana].Weight;
                    Inventory = new InventoryModel(WeedFarmService.FarmStockWeight * 1000, 30, InventoryTypes.WeedStock, true);
                }

                if (value > 0)
                {
                    var item = Inventory.Items.FirstOrDefault(i => i.Name == ItemNames.Marijuana);
                    if (item == null)
                    {
                        Inventory.AddItem(new Narcotic(ItemNames.Marijuana, value, false, true), -1, LogAction.None);
                    }                        
                    else
                        item.Count = value;
                }
                else
                    Inventory.RemoveItems(item => item.Name == ItemNames.Marijuana, LogAction.None);
            }
        }
        public int IrrigationSystemHealth { get; set; }
        public int LightSystemHealth { get; set; }
        public int DryingSystemHealth { get; set; }
        public int VentilationSystemHealth { get; set; }
        public ExtVehicle FarmVehicle { get; set; }
        public InventoryModel Inventory { get; set; }
        private List<ExtPlayer> _subscribers { get; set; }
        public Dictionary<int, ExtPlayer> SeatPlaces = new Dictionary<int, ExtPlayer>
        {
            {0, null },
            {1, null }
        };

        private bool _componentsChanged = false;
        private bool _stadyChanged = false;
        private string _timer = "";

        public void Subscribe(ExtPlayer player)
        {
            _subscribers.Add(player);
            SafeTrigger.ClientEvent(player,"weedfarm:instance:update", Places,  Components, OnDrying, OnPacking, OnDelivery, IrrigationSystemHealth, LightSystemHealth, DryingSystemHealth, VentilationSystemHealth);
        }

        public void Unsubscribe(ExtPlayer player)
        {
            if (_subscribers.Contains(player))
               _subscribers.Remove(player);
            for (int i = 0; i < SeatPlaces.Count; i++)
            {
                var place = SeatPlaces[i];
                if (place == player)
                {
                    SeatPlaces[i] = null;
                }
            }           
        }

        public void Save()
        {         
            if (_componentsChanged)
            {
                MySQL.Query("UPDATE `weedfarm` SET `components`=@prop0 WHERE `id`=@prop1", JsonConvert.SerializeObject(Components), Id);
                _componentsChanged = false;
            }

            if (_stadyChanged)
            {
                MySQL.Query("UPDATE `weedfarm` SET `onPacking`=@prop0, `onDrying`=@prop1, `onDelivery`=@prop2 WHERE `id`=@prop3", OnPacking, OnDrying, OnDelivery, Id);
                _stadyChanged = false;
            }

        }

        internal void RespawnFarmCar()
        {
            ExtVehicle veh = FarmVehicle.Data.Vehicle;
            if (veh == null) return;

            veh.Position = VehiclePosition;
            veh.Rotation = VehicleRotation;
            foreach (var entity in veh.AllOccupants.Values)
            {
                var player = entity;
                if (player == null) continue;

                player.ChangePosition(null);
                SafeTrigger.ClientEvent(player, "FixTeleportInVehicle", 1000);
            }
        }

        public void UpdatePlace(int index, int stady)
        {
            Places[index] = stady;
            foreach (var player in _subscribers)
            {
                SafeTrigger.ClientEvent(player,"weedfarm:instance:place:update", index, stady);
            }
            _componentsChanged = true;
        }

        public void MoveToDrying()
        {
            OnDrying += WeedFarmService.PlanstOnSpot;
            _stadyChanged = true;
            foreach (var player in _subscribers)
            {
                SafeTrigger.ClientEvent(player,"weedfarm:instance:drying:update", OnDrying);
            }
            return;
        }

        public void MoveToPacking()
        {
            var count = Math.Min(OnDrying, WeedFarmService.CountPlantsOnDryStep);
            if (OnPacking + count > WeedFarmService.PlacePoints.Count * WeedFarmService.PlanstOnSpot)
                return;
            OnDrying -= count;
            OnPacking += count;
            _stadyChanged = true;
            foreach (var player in _subscribers)
            {
                SafeTrigger.ClientEvent(player,"weedfarm:instance:packing:update", OnPacking);
                SafeTrigger.ClientEvent(player,"weedfarm:instance:drying:update", OnDrying);
            }
        }
        internal void RequestPackAction(ExtPlayer player)
        {
            if (OnPacking > 0)
            {
                var item = new Narcotic(ItemNames.Marijuana, 1, false, true);
                if (!Inventory.CanAddItem(item))
                {
                    player.SendError("Недостаточно места на складе.");
                    return;
                }
                OnPacking--;
                Inventory.AddItem(item, -1, LogAction.Create);
                foreach (var p in _subscribers)
                {
                    SafeTrigger.ClientEvent(p, "weedfarm:instance:packing:update", OnPacking);
                    SafeTrigger.ClientEvent(p, "weedfarm:instance:delivery:update", OnDelivery);
                }
                _stadyChanged = true;
                SafeTrigger.ClientEvent(player,"weedfarm:sort:action:responce");
            }
            else
            {
                player.SendInfo("На складе нету больше товара готового к сортировке, стоит немного подождать.");
                ReleasePackPoint(player);
            }
        }

        public void AddComponent(int component)
        {
            if(!Components.Contains(component)){
                Components.Add(component);
                _subscribers.ForEach(p => SafeTrigger.ClientEvent(p, "weedfarm:instance:comp:update", component));
                _componentsChanged = true;
            }
        }

        public void SetOwner(Fraction fraction)
        {
            if (Owner == fraction) return;
            Reset();
            Owner = fraction;
            OccupationDate = DateTime.Now;
            foreach (var member in fraction.OnlineMembers.Values)
            {
                member.SendSuccess($"Поздравляем! Ваша группировка получила контроль над фермой №{Id}.");
            }

            MySQL.Query("UPDATE `weedfarm` SET `ownerId`=@prop0, `occupationDate`=@prop1 WHERE `id`=@prop2", JsonConvert.SerializeObject(Owner.Id), MySQL.ConvertTime(OccupationDate), Id);
        }

        public void UpdateOccupationDate(DateTime date)
        {
            OccupationDate = date;
            MySQL.Query("UPDATE `weedfarm` SET `occupationDate`=@prop0 WHERE `id`=@prop1", MySQL.ConvertTime(OccupationDate), Id);
        }

        private void Reset()
        {
            Timers.Stop(_timer);
            for (int i = 0; i < Places.Count; i++)
                Places[i] = 0;

            Inventory.RemoveItems((player) => true, LogAction.None);
            OnDrying = 0;
            OnPacking = 0;

            Components.Clear();

            _componentsChanged = true;
            _stadyChanged = true;

            for (int i = 0; i < SeatPlaces.Count; i++)
            {
                var place = SeatPlaces[i];
                if (place.IsLogged())
                    place.SetSharedData("weedfarm:seat", -1);
                SeatPlaces[i] = null;
            }          

            KickOldOwnerMembers();
        }

        private void KickOldOwnerMembers()
        {
            if(_subscribers.Count > 0)
            {
                foreach (var player in _subscribers)
                {
                    player.ChangePosition(EnterPoint);
                    SafeTrigger.UpdateDimension(player,  0);
                    SafeTrigger.ClientEvent(player,"weedfarm:instance:reset");
                }
                _subscribers.Clear();
            }
            
        }

        private void WaitGrow(int index)
        {
            Timers.StartOnce(1000 * 60 * WeedFarmService.GrowTime, () => {
                UpdatePlace(index, Places[index] + 1);
                if (Places[index] < 3)
                    WaitGrow(index);
            });
        }

        internal void PlantSeed(int palceIndex)
        {
            UpdatePlace(palceIndex, 1);
            WaitGrow(palceIndex);           
        }

        internal void PackPointAction(ExtPlayer player, int key)
        {
            if (SeatPlaces[key] == player) return;
            if (SeatPlaces[key].IsLogged())
                player.SendError("Это место занято. Тут уже кто-то работает.");
            else
            {
                SafeTrigger.SetSharedData(player, "weedfarm:seat", key);
                SeatPlaces[key] = player;
            }
        }
       
        internal void ReleasePackPoint(ExtPlayer player)
        {
            if (SeatPlaces.Any(s => s.Value == player))
            {
                var key = SeatPlaces.First(s => s.Value == player).Key;
                SeatPlaces[key] = null;
            }
            SafeTrigger.SetSharedData(player, "weedfarm:seat", -1);
        }

        internal void DryingLoop()
        {
            if (OnDrying < 1) return;
            MoveToPacking();
        }
    }
}
