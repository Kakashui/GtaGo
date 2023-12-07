using GTANetworkAPI;

namespace Whistler.Businesses.Manager.DTOs
{
    public class PedDTO
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public int Model { get; set; }
        public string Name { get; set; }
        public uint Dimension { get; set; } = 0;
    }
}
