using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.MP.Arena.Enums;
using Whistler.MP.Arena.Helpers;
using Whistler.MP.Arena.Interfaces;

namespace Whistler.MP.Arena.Locations
{
    internal class ArenaLocation
    {
        public List<ArenaLocationSpawn> AllSpawnPoints => RedTeamSpawnPoints.Union(GreenTeamSpawnPoints).ToList();
        
        public List<ArenaLocationSpawn> RedTeamSpawnPoints { get; }
        
        public List<ArenaLocationSpawn> GreenTeamSpawnPoints { get; }

        public ArenaLocationSpawn DefaultSpawnPoint => AllSpawnPoints.FirstOrDefault();
        
        public List<ArenaContentObject> ContentObjects { get; } = new List<ArenaContentObject>();

        public uint Dimension = ArenaDimensionHelper.GetUniqueDimension();
        
        public ArenaLocation(List<ArenaLocationSpawn> redTeamSpawnPoints, List<ArenaLocationSpawn> greenTeamSpawnPoints)
        {
            RedTeamSpawnPoints = redTeamSpawnPoints;
            GreenTeamSpawnPoints = greenTeamSpawnPoints;
        }

        public void LoadForLobby(IArenaLobby lobby)
        {
            
            foreach (var member in lobby.Members) LoadContentForClient(member.Player);
        }

        public ArenaLocationSpawn RequestSpawnPoint(TeamName team = TeamName.Unknown)
        {
            var freeSpawnPoint = team switch
            {
                TeamName.Unknown => AllSpawnPoints.FirstOrDefault(s => s.IsFree),
                TeamName.Green => GreenTeamSpawnPoints.FirstOrDefault(s => s.IsFree),
                _ => RedTeamSpawnPoints.FirstOrDefault(s => s.IsFree)
            };

            freeSpawnPoint ??= AllSpawnPoints.FirstOrDefault();
            
            freeSpawnPoint.IsFree = false;
            
            return freeSpawnPoint;
        }
        
        public ArenaLocationSpawn RequestRandomFreeSpawnPoint(TeamName team = TeamName.Unknown)
        {
            ArenaLocationSpawn freeSpawnPoint;
            switch (team)
            {
                case TeamName.Unknown:
                    freeSpawnPoint = AllSpawnPoints[new Random().Next(0, AllSpawnPoints
                        .Count(s => s.IsFree))];
                    break;
                case TeamName.Green:
                    freeSpawnPoint = GreenTeamSpawnPoints[new Random().Next(0, GreenTeamSpawnPoints
                        .Count(s => s.IsFree))];
                    break;
                default:
                    freeSpawnPoint = RedTeamSpawnPoints[new Random().Next(0, RedTeamSpawnPoints
                        .Count(s => s.IsFree))];
                    break;
            }

            freeSpawnPoint ??= AllSpawnPoints[new Random().Next(0, AllSpawnPoints.Count)];
            
            freeSpawnPoint.IsFree = false;
            
            return freeSpawnPoint;
        }

        public void DismissSpawnPoint(ArenaLocationSpawn point)
        {
            if (point != null) point.IsFree = true;
        }
        public void Unload(IBattleMember member)
        {
            UnloadContentForClient(member.Player);
        }
        
        private void LoadContentForClient(ExtPlayer player) =>
            ContentObjects.ForEach(o => o.LoadForPlayer(player));
        
        private void UnloadContentForClient(ExtPlayer player) =>
            ContentObjects.ForEach(o => o.UnloadForPlayer(player));
    }
}