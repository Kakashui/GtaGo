using GTANetworkAPI;

namespace Whistler.Core.OldPets
{
    public class PedDto
    {
        public int ID { get; set; }
        public int Model { get; set; }
        public Vector3 Position { get; set; }
        public int ControllerId { get; set; }
        public int State { get; set; }
    }
}
