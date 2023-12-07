using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Inventory.Models
{
    public class WorldObject
    {
        private static int _deleteMinutes = 10;
        private static int _counter = 0;
        public int Id { get; }
        public WorldObject(BaseItem item, Vector3 position, Vector3 rotation, uint dimension)
        {
            Id = _counter++;
            item.Index = Id;
            Item = item;
            DeleteDate = DateTime.Now.AddMinutes(_deleteMinutes);
            uint modelHash = item.GetModelHash();
            if (modelHash == 0) return;

            _object = NAPI.Object.CreateObject(modelHash, position, rotation, 255, dimension);
            _object.SetSharedData("data:object:id", Id);
        }
        public BaseItem Item { get; private set; }
        public DateTime DeleteDate { get; }
        public Vector3 Position { 
            get
            {
                return _object.Position;
            }
        }
        public uint Dimension
        {
            get
            {
                return _object.Dimension;
            }
        }
        private GTANetworkAPI.Object _object;

        public void Destroy()
        {
            _object?.Delete();
            _object = null;
            Item = null;
        }
    }
}
