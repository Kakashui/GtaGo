using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.Enviroment.Models
{
    class SitPlace
    {
        [JsonProperty("pos")]
        public Vector3 PositionOffset { get; set; }
        [JsonProperty("rot")]
        public Vector3 RotationOffset { get; set; }
        [JsonIgnore]
        private Dictionary<uint, bool> _ocupied = new Dictionary<uint, bool>();       
        public bool TakePlace(uint hash)
        {
            if (!_ocupied.ContainsKey(hash))
            {
                _ocupied.Add(hash, true);
                return true;
            }else
            {
                if (_ocupied[hash])
                    return false;
                _ocupied[hash] = true;
                return true;
            }
        }
        public void FrePlace(uint hash)
        {
            if (_ocupied.ContainsKey(hash)) 
                _ocupied[hash] = false;
        }
    }
}
