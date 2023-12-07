using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MP.Arena.Locations;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.MP.Arena.Racing
{
    public class Racer
    {
        public ExtPlayer Player { get; }

        public RacerState CurrentState { get; set; }

        public uint VehicleHash { get; }
        
        public ExtVehicle Vehicle { get; set; }

        public ArenaLocationSpawn CurrentSpawn { get; }

        private Queue<Vector3> _сheckpoints = new Queue<Vector3>();

        public int TotalSeconds { get; set; }
        
        public Racer(ExtPlayer player, uint vehicleHash, ArenaLocationSpawn spawn, List<Vector3> checkpoints)
        {
            Player = player;
            CurrentSpawn = spawn;
            VehicleHash = vehicleHash;
            checkpoints.ForEach(c => _сheckpoints.Enqueue(c));
        }

        public void Delete()
        {
            if (CurrentState == RacerState.Registered)
            {
            }
            else
            {
                if (Player.HasData("dm:default:spawn"))
                {
                    Vector3 oldPosition = Player.GetData<Vector3>("dm:default:spawn");
                    Player.ChangePosition(oldPosition);
                }
                
                SafeTrigger.UpdateDimension(Player,  0);
                SafeTrigger.ClientEvent(Player, "race:clear");
                Vehicle.CustomDelete();
            }
        }

        public void Spawn(int vehicleColor, uint dimension)
        {
            SafeTrigger.SetData(Player, "dm:default:spawn", Player.Position);
            
            SafeTrigger.UpdateDimension(Player,  dimension);
            CurrentState = RacerState.Playing;
            Vehicle = VehicleManager.CreateTemporaryVehicle(VehicleHash, CurrentSpawn.Position, new Vector3(0, 0, CurrentSpawn.Heading), "RACER", VehicleAccess.Work, Player, dimension);

            VehicleCustomization.SetColor(Vehicle, new Color(vehicleColor), 1, true);
            VehicleStreaming.SetEngineState(Vehicle, true);

            
            Player.CustomSetIntoVehicle(Vehicle, VehicleConstants.DriverSeatClientSideBroken);
            SafeTrigger.ClientEvent(Player, "race:start", Vehicle.Value);
            SetNextCheckPoint();
        }

        public void OnRacerFinished(int timeInMs)
        {
            TotalSeconds = timeInMs;
            Delete();
        }
        
        public void SetNextCheckPoint()
        {
            var currentCheckPoint = _сheckpoints.Dequeue();

            if (_сheckpoints.Count == 0)
                SafeTrigger.ClientEvent(Player, "race:setFinish", JsonConvert.SerializeObject(currentCheckPoint));                
            else
                SafeTrigger.ClientEvent(Player, "race:setCP", JsonConvert.SerializeObject(currentCheckPoint),
                    JsonConvert.SerializeObject(_сheckpoints.Peek()));
        }
    }
}