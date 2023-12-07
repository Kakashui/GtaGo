using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.Character;
using Whistler.Core.Models;
using Whistler.Entities;
using Whistler.GUI.Documents.Enums;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.VehicleSystem.Models
{
    public class ExtVehicle : Vehicle
    {
        public VSession Session { get; set; } = null;

        public ExtVehicle(NetHandle handle) : base(handle)
        {
            
        }

        public void Initialize(uint model)
        {
            Data = new TemporaryVehicle(model);
            Session = new VSession(model);

            Engine = false;
            Locked = false;
            IsLocked = false;
            TurnSignal = 0;
            IsFreezed = false;
            DoorState = 0;
            AllOccupants = new Dictionary<sbyte, ExtPlayer>();
        }

        public void Initialize(VehicleBase vehicleModel)
        {
            Data = vehicleModel;
            Session = new VSession(NAPI.Util.GetHashKey(vehicleModel.ModelName));

            Engine = false;
            Locked = false;
            IsLocked = false;
            TurnSignal = 0;
            IsFreezed = false;
            DoorState = 0;
            AllOccupants = new Dictionary<sbyte, ExtPlayer>();
        }

        public void InitializeLicense(int vehClass)
        {
            if (Model == 2452219115 || Model == 55628203 || Model == 3005788552 || vehClass == 13) return;

            RequiredLicense = vehClass switch
            {
                0 => LicenseName.Auto, // Compacts
                1 => LicenseName.Auto, // Sedans
                2 => LicenseName.Auto, // SUV
                3 => LicenseName.Auto, // Coupes
                4 => LicenseName.Auto, // Muscle
                5 => LicenseName.Auto, // Sports Classic
                6 => LicenseName.Auto, // Sports
                7 => LicenseName.Auto, // Super
                8 => LicenseName.Moto, // Motorcycles
                9 => LicenseName.Auto, // Off-road
                10 => LicenseName.Truck, // Industrial
                11 => LicenseName.Auto, // Utility & Trailers
                12 => LicenseName.Auto, // Vans
                14 => LicenseName.Both, // Boats
                15 => LicenseName.Helicopter, // Helicopters
                16 => LicenseName.Fly, // Planes
                17 => LicenseName.Auto, // Service
                18 => LicenseName.Auto, // Emergency
                19 => LicenseName.Auto, // Military
                20 => LicenseName.Truck, // Commercial
                21 => LicenseName.Truck, // Trains
                22 => LicenseName.Auto, // Open Wheels
                _ => null, // Не требует лицензии
            };
        }

        public LicenseName? RequiredLicense { get; private set; } = null;
        public bool Engine { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public int TurnSignal { get; set; } = 0;
        public bool IsFreezed { get; set; } = false;
        public int DoorState { get; set; } = 0;
        public Dictionary<sbyte, ExtPlayer> AllOccupants { get; set; } = new Dictionary<sbyte, ExtPlayer>();
        public VehicleBase Data { get; set; }
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
