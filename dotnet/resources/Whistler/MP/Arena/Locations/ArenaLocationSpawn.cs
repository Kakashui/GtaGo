using GTANetworkAPI;

namespace Whistler.MP.Arena.Locations
{
    public class ArenaLocationSpawn
    {
        public Vector3 Position { get; }
        
        public float Heading { get; }

        public bool IsFree { get; set; } = true;

        public ArenaLocationSpawn(Vector3 position, float heading)
        {
            Position = position;
            Heading = heading;
        }
        
        public ArenaLocationSpawn(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Heading = rotation.Z;
        }
    }
}