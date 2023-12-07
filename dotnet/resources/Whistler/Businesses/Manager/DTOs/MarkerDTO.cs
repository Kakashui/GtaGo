using GTANetworkAPI;

namespace Whistler.Businesses.Manager.DTOs
{
    public class MarkerDTO
    {
        public int BizId { get; set; }
        public Vector3 Position { get; set; }
        public int Range { get; set; }
    }
}