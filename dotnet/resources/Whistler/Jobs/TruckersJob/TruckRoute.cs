using GTANetworkAPI;
using Whistler.Core;
using Whistler.VehicleSystem.Models;

namespace Whistler.Jobs.TruckersJob
{
    /// <summary>
    /// Маршрут движения грузовика
    /// </summary>
    internal struct TruckRoute
    {
        public Vector3 LoadPoint { get; }

        public Vector3 UnloadPoint { get; }
        public Order OrderFromDock { get; }

        public Truck.CurrentRouteType CurrentRouteType { get; set; }

        public ExtVehicle Trailer { get; set; }

        public TruckRoute(Vector3 loadPoint, Vector3 unloadPoint, ExtVehicle trailer, Order orderFromDock = null)
        {
            LoadPoint = loadPoint;
            UnloadPoint = unloadPoint;
            CurrentRouteType = Truck.CurrentRouteType.None;
            Trailer = trailer;
            OrderFromDock = orderFromDock;
        }
    }
}