using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fractions.PDA.Models
{
    class PoliceCallDTO
    {
        public int id { get; }
        public double time { get; }
        public int code { get; }
        public string name { get; }
        public Vector3 position { get; }
        public List<HelperCall> helpers { get; set; }
        public PoliceCallDTO(PoliceCall call)
        {
            id = call.Id;
            time = (call.Time - new DateTime(1, 1, 1, 0, 0, 0)).TotalSeconds;
            code = call.Code;
            name = call.Caller;
            position = call.Position;
            helpers = call.Helpers;
        }
    }
}
