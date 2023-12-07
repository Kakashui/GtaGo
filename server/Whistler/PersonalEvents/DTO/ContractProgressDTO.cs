using System;
using System.Collections.Generic;
using System.Text;
using Whistler.PersonalEvents.Contracts;
using Whistler.PersonalEvents.Contracts.Models;

namespace Whistler.PersonalEvents.DTO
{
    class ContractProgressDTO
    {
        public int CountCompleted { get; set; }
        public int CurrentLevel { get; set; }
        public bool InProgress { get; set; }
        public int SecondToEndContract { get; set; }
        public int ContractName { get; set; }
        public List<GTANetworkAPI.Vector3> PointsVisited { get; set; }
        public ContractProgressDTO(ContractProgress contract, ContractNames contractName)
        {
            InProgress = contract.InProgress;
            CurrentLevel = contract.CurrentLevel;
            CountCompleted = contract.CountCompleted;
            SecondToEndContract = (int)(contract.ExpirationDate - DateTime.Now).TotalSeconds;
            ContractName = (int)contractName;
            PointsVisited = contract.PointsVisited;
        }
    }
}
