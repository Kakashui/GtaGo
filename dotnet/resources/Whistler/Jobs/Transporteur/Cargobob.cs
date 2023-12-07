using GTANetworkAPI;
using Whistler.Jobs.AbstractEntity;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.VehicleSystem.Models;

namespace Whistler.Jobs.Transporteur
{
    class Cargobob : AbstractVehicle
    {
        //public DateTime CreateTime { get; set; } = DateTime.Now;

        public Cargobob(Pilot pilot, SpawnPoint vehLocation)
        {
            try
            {
                string randomCarModel = Work.HelicopterName;
                string randomNumber = GenerateRandomCarNumer(Work.CarNumberLength, Work.CarNumberDictionary);
                bool isLocked = false;
                bool isEngineStarted = false;
                VehicleHash vehicleHash = (VehicleHash)NAPI.Util.GetHashKey(randomCarModel);

                this.Vehicle = NAPI.Vehicle.CreateVehicle(vehicleHash, vehLocation.Position, vehLocation.Rotation, 1, 1, randomNumber, 255, isLocked, isEngineStarted) as ExtVehicle;
                this.Model = randomCarModel;
                this.Number = this.Vehicle.NumberPlate;
                this.Position = this.Vehicle.Position;
                this.Owner = pilot;
                this.Vehicle.NumberPlate = randomNumber;

                vehLocation.IsOccupied = true;
                this.CarLocation = vehLocation;

                Work.Cargobobs.Add(this); 
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.Cargobob(Pilot pilot, SpawnPoint vehLocation): {ex.ToString()}");
            }
        }

        public int GetExistedMinutes()
        {
            return (int)Math.Round(DateTime.Now.Subtract(this.CreateTime).TotalMinutes);
        }

        public override string ToString()
        {
            return $"{Model}, {Number}, {GetExistedMinutes()} mins";
        }
    }
}
