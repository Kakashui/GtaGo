using GTANetworkAPI;

namespace Whistler.Docks
{
    internal class DockCrate
    {
        public Object Template { get; private set; }
        public Player PlayerWorking { get; private set; }

        public bool IsFree => PlayerWorking == null;

        public int Id => Template.Value;

        private Vector3 _startPosition;

        public DockCrate(Object template)
        {
            Template = template;
            _startPosition = template.Position;
        }

        public void Reset()
        {
            Template.Delete();
            Template = NAPI.Object.CreateObject(519908417, _startPosition, new Vector3());
            PlayerWorking = null;
        }

        public void Claim(Player player)
        {
            PlayerWorking = player;
        }
    }
}