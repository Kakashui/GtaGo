using GTANetworkAPI;

namespace Whistler.Jobs.TruckersJob
{
    public class TruckInfo
    {
        public uint TruckHash { get; }
        public Color Color { get; set; }
        
        public TruckInfo(uint truckHash, Color color = new Color())
        {
            TruckHash = truckHash;
            Color = color;
        }
    }
}