using GTANetworkAPI;

namespace Whistler.Docks
{
    public class DockRentPoint
    {
        public Vector3 Position { get; set; }

        public bool Occupied { get; set; }

        public float Heading { get; set; }

        public DockRentPoint(Vector3 position, float heading)
        {
            Position = position;
            Heading = heading;
        }
    }
}