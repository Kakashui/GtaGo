using GTANetworkAPI;
using Whistler.Jobs.AbstractEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Helpers;

namespace Whistler.Jobs.Transporteur
{
    class DeliverCar : AbstractVehicle
    {
        public DeliverCar(Pilot pilot, SpawnPoint vehLocation)
        {
            try
            {
                string randomCarModel = Work.CarModels.GetRandomElement();
                string randomNumber = GenerateRandomCarNumer(Work.CarNumberLength, Work.CarNumberDictionary);
                this.Model = randomCarModel;
                this.Number = randomNumber;
                this.Owner = pilot;

                vehLocation.IsOccupied = true;
                this.CarLocation = vehLocation; 

                Work.ClientCars.Add(this);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Pilot.Cargobob(Pilot pilot, SpawnPoint vehLocation): {ex.ToString()}");
            }
        }
    }
}
