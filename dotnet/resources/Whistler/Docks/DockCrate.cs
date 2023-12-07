using GTANetworkAPI;
using Whistler.Entities;

namespace Whistler.Docks
{
    internal class DockCrate
    {
        public Object Template { get; private set; }
        public ExtPlayer PlayerWorking { get; private set; }

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

        public void Claim(ExtPlayer player)
        {
            PlayerWorking = player;
        }
    }
}