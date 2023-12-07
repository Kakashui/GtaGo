using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.PersonalEvents.DTO;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Contracts.Models
{
    class ContractProgress
    {
        public int CountCompleted;
        public int CurrentLevel;
        public bool InProgress;
        public DateTime ExpirationDate;
        public Dictionary<string, int> CountPlayersProgress;
        public bool IsChanged = false;

        public List<GTANetworkAPI.Vector3> PointsVisited;
        public ContractProgress(DateTime expirationDate)
        {
            InProgress = true;
            CurrentLevel = 0;
            ExpirationDate = expirationDate;
            CountPlayersProgress = new Dictionary<string, int>();
            IsChanged = false;
            PointsVisited = new List<GTANetworkAPI.Vector3>();
        }
        public ContractProgress(DataRow row)
        {
            InProgress = Convert.ToBoolean(row["inProgress"]);
            CurrentLevel = Convert.ToInt32(row["currentLevel"]);
            ExpirationDate = Convert.ToDateTime(row["expirationDate"]);
            CountCompleted = Convert.ToInt32(row["countCompleted"]);
            CountPlayersProgress = JsonConvert.DeserializeObject<Dictionary<string, int>>(row["countPlayersProgress"].ToString());
            IsChanged = false;
            PointsVisited = new List<GTANetworkAPI.Vector3>();
        }
        public ContractProgressDTO GetDTO(ContractNames contractName)
        {
            return new ContractProgressDTO(this, contractName);
        }
        public bool CheckProgress() => InProgress && DateTime.Now < ExpirationDate;
        public bool StartContract(DateTime expirationDate)
        {
            if (InProgress && DateTime.Now < ExpirationDate || ExpirationDate.AddDays(1) > expirationDate)
                return false;
            CountPlayersProgress = new Dictionary<string, int>();
            PointsVisited = new List<GTANetworkAPI.Vector3>();
            InProgress = true;
            CurrentLevel = 0;
            ExpirationDate = expirationDate;
            IsChanged = true;
            return true;
        }

        public int AddProgress(ExtPlayer player, ContractTypes contractTypes, int count, int max, int maxOnePlayer)
        {
            int cnt = Math.Min(Math.Min(count, maxOnePlayer - CountPlayersProgress.GetValueOrDefault(player.Account.Login, 0)), max - CurrentLevel);
            if (cnt > 0)
            {
                if (contractTypes == ContractTypes.Family)
                {
                    if (CountPlayersProgress.ContainsKey(player.Account.Login))
                        CountPlayersProgress[player.Account.Login] += cnt;
                    else
                        CountPlayersProgress.Add(player.Account.Login, cnt);
                }
                CurrentLevel += cnt;
                IsChanged = true;
                return cnt;
            }
            return 0;
        }
    }
}