using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whistler.Jobs.CarThief
{
    class Agent
    {
        public Agent(Vector3 position, float rotation)
        {
            try
            {
                int index = WorkManager.rnd.Next(0, Work.AgentData.Count);
                this.Name = Work.AgentData.ElementAt(index).Key;
                this.Hash = Work.AgentData.ElementAt(index).Value;
                this.Position = position;
                this.Rotation = rotation;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput($"Agent.Constructor(): {ex.ToString()}");
            }
        }

        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public int Hash { get; set; }

    }
}
