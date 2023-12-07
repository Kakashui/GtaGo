using GTANetworkAPI;
using System;
using System.Linq;
using System.Text;
using Whistler.Helpers;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.Jobs.CarThief
{
    class Car
    {
        public Car(Vector3 position, float rotation)
        {
            try
            {
                this.Position = position;
                this.Rotation = rotation;
                this.IsOccupied = false;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Car.public public Car(Vector3 position, float rotation): {ex.ToString()}");
            }
        }

        public Car(Thief thief, Car carLocation)
        {
            try
            {
                string randomCarModel = Work.CarModels.GetRandomElement();

                Color randomColor1 = Work.CarColors.GetRandomElement();
                string randomNumber = VehicleManager.GenerateNumber(false);

                this.Vehicle = VehicleManager.CreateTemporaryVehicle(randomCarModel, carLocation.Position, new Vector3(0, 0, carLocation.Rotation), randomNumber, VehicleAccess.WorkCarThief, thief.Player);
                VehicleStreaming.SetEngineState(Vehicle, false);
                VehicleStreaming.SetLockStatus(Vehicle, true);
                VehicleCustomization.SetColor(Vehicle, randomColor1, 1, true);
                VehicleCustomization.SetColor(Vehicle, randomColor1, 1, false);
                this.Model = randomCarModel;
                this.Number = this.Vehicle.NumberPlate;
                this.Position = this.Vehicle.Position;
                this.Thief = thief;
                this.Vehicle.NumberPlate = randomNumber;

                carLocation.IsOccupied = true;
                this.CarLocation = carLocation;

                VehicleCustomization.SetMod(this.Vehicle, ModTypes.WindowToning, 1);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Car.public Car(Thief thief): {ex.ToString()}");
            }
        }


        public ExtVehicle Vehicle { get; set; }
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public Thief Thief { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }
        public bool IsNumberChanged { get; set; } = false;
        public bool IsOccupied { get; set; } = false; 
        public Car CarLocation { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public void UpdateRandomNumber()
        {
            try
            {
                string newNumber = VehicleManager.GenerateNumber(false);
                this.Number = newNumber;
                this.Vehicle.NumberPlate = newNumber;
                this.IsNumberChanged = true;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Car.UpdateRundomNumber(): {ex.ToString()}");
            }
        }

        public int GetExistedMinutes()
        {
            return (int)Math.Round(DateTime.Now.Subtract(this.CreateTime).TotalMinutes);
        }

        public override string ToString()
        {
            return $"{Model} ({Number} (edited:{IsNumberChanged}) {GetExistedMinutes()} mins) ";
        }
    }
}
