using GTANetworkAPI;
using Whistler.Core;

namespace Whistler.Houses
{
    public class HouseType
    {
        public string Name { get; }
        public Vector3 Position { get; }
        public string IPL { get; set; }
        public float PetRotation { get; }

        public HouseType(string name, Vector3 position, float rotation, string ipl = "")
        {
            Name = name;
            Position = position;
            IPL = ipl;
            PetRotation = rotation;
        }

        public void Create()
        {
            if (IPL != "") NAPI.World.RequestIpl(IPL);

            InteractShape.Create(Position, 2, 4, NAPI.GlobalDimension)
                .AddInteraction(HouseManager.ExitHouse, "interact_20");

        }
    }
}