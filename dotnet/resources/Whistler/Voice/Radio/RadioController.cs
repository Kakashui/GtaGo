using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Voice.Radio
{
    static class RadioController
    {
        private static Dictionary<string, RadioRoom> _radioRoomsByID = new Dictionary<string, RadioRoom>();
        
        public static bool CreateRoom(string ID)
        {
            if (_radioRoomsByID.ContainsKey(ID)) return false;

            _radioRoomsByID.Add(ID, new RadioRoom(ID));
            return true;
        }

        public static RadioRoom GetRoom(string ID)
        {
            if (ID != null && _radioRoomsByID.ContainsKey(ID))
                return _radioRoomsByID[ID];
            else
                return null;
        }

        public static bool RemoveRoom(string ID)
        {
            if (_radioRoomsByID.ContainsKey(ID)) return false;

            _radioRoomsByID[ID].OnRoomRemove();
            return true;
        }
    }
}
