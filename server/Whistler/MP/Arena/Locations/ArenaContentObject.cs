using GTANetworkAPI;

namespace Whistler.MP.Arena.Locations
{
    internal class ArenaContentObject
    {
        public Vector3 Position { get; set; }
        
        public Vector3 Rotation { get; set; }

        public uint Dimension { get; set; }
        
        public void LoadForPlayer(Player player)
        {
            //TODO: TRIGGER like mebel
        }

        public void UnloadForPlayer(Player player)
        {
            //TODO: TRIGGER like mebel
        }
    }
}