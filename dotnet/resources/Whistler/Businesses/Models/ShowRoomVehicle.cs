using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.Businesses.Models
{
    internal class ShowRoomVehicle
    {
        public readonly int Id = 0;
        public ExtVehicle Vehicle { get; private set; }
        public readonly Vector3 Position;
        public readonly float Rotation;
        public readonly string Model;

        public ShowRoomVehicle(Vector3 position, float rotation, string model, uint dimension = 0, int primaryColor = 111, int secondaryColor = 111)
        {
            Position = position;
            Model = model;
            Vehicle = VehicleManager.CreateTemporaryVehicle(model, position, rotation, "MIKZZZ", VehicleAccess.ShowRoom, dimension: dimension);
            if (Vehicle == null) return;

            Id = Vehicle.Data.ID;
            if (primaryColor != 0) Vehicle.PrimaryColor = primaryColor;
            if (secondaryColor != 0) Vehicle.SecondaryColor = secondaryColor;
            //VehicleStreaming.SetFreezePosition(Vehicle, true);
            VehicleStreaming.SetLockStatus(Vehicle, true);
            SafeTrigger.SetSharedData(Vehicle, "veh:showRoom", true);
        }

    }
}
