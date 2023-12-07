using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.VehicleSystem.Models
{
    public class VehicleGo
    {
        public VehicleGo(uint model)
        {
            Data = new TemporaryVehicle(model);
            Model = model;

            Engine = false;
            Locked = true;
            TurnSignal = 0;
            IsFreezed = false;
            DoorState = 0;
            Occupants = new List<Player>();
        }
        public VehicleGo(VehicleBase vehicleModel)
        {
            Data = vehicleModel;
            Model = NAPI.Util.GetHashKey(vehicleModel.ModelName);

            Engine = false;
            Locked = true;
            TurnSignal = 0;
            IsFreezed = false;
            DoorState = 0;
            Occupants = new List<Player>();
        }
        public bool Engine { get; set; }
        public bool Locked { get; set; }
        public int TurnSignal { get; set; }
        public bool IsFreezed { get; set; }
        public int DoorState { get; set; }
        public List<Player> Occupants { get; set; }
        public VehicleBase Data { get; set; }
        public uint Model { get; set; }
        public VehConfig Config
        {
            get
            {
                return VehicleConfiguration.GetConfig(Model);
            }
        }

        /// <summary>
        /// Изнашиваемые авто
        /// </summary>
        /// <returns></returns>
        public bool IsWearable()
        {
            return Data is PersonalBaseVehicle;
        }
    }
}
