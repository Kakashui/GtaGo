using GTANetworkAPI;

namespace Whistler.Jobs.TruckersJob
{
    public class TruckInfo
    {
        public uint TruckHash { get; }
        public bool TruckTrailer { get; }
        public Color Color { get; set; }
        
        public TruckInfo(uint truckHash, bool trailer, Color color = new Color())
        {
            TruckHash = truckHash;
            TruckTrailer = trailer;
            Color = color;
        }
    }
}