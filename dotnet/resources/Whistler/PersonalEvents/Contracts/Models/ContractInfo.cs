using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Families.FamilyMenu;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Contracts.Models
{
    class ContractInfo
    {
        public Dictionary<ContractNames, ContractProgress> ContractList;
        private bool _isChanged = false;
        public ContractInfo()
        {
            ContractList = new Dictionary<ContractNames, ContractProgress>();
        }
        public ContractInfo(DataRowCollection rows)
        {
            ContractList = new Dictionary<ContractNames, ContractProgress>();
            foreach (DataRow row in rows)
            {
                ContractList.Add((ContractNames)Convert.ToInt32(row["contractName"]), new ContractProgress(row));
            }
        }

        public bool AcceptContract(ExtPlayer player, ContractModel contractModel)
        {
            if (ContractList.ContainsKey(contractModel.ContractName))
            {
                if (ContractList[contractModel.ContractName].StartContract(DateTime.Now.AddMinutes(contractModel.MinutesToComplete)))
                {
                    _isChanged = true;
                    UpdateContract(player, contractModel.ContractName, contractModel.ContractType);
                    return true;
                }
                else
                    return false;
            }
            else
            {
                var myContract = new ContractProgress(DateTime.Now.AddMinutes(contractModel.MinutesToComplete));
                ContractList.Add(contractModel.ContractName, myContract);
                MySQL.Query("INSERT INTO `contracts` (countCompleted, currentLevel, inProgress, expirationDate, countPlayersProgress, ownerid, ownerType, contractName) " +
                    "VALUES (@prop0, @prop1, @prop2, @prop3, @prop4, @prop5, @prop6, @prop7)",
                    myContract.CountCompleted,
                    myContract.CurrentLevel,
                    myContract.InProgress,
                    MySQL.ConvertTime(myContract.ExpirationDate),
                    JsonConvert.SerializeObject(myContract.CountPlayersProgress),
                    contractModel.ContractType == ContractTypes.Single ? player.Character.UUID : player.Character.FamilyID, 
                    (int)contractModel.ContractType, 
                    (int)contractModel.ContractName);
                UpdateContract(player, contractModel.ContractName, contractModel.ContractType);
                return true;
            }
        }


        public void UpdateContract(ExtPlayer player, ContractNames contractName, ContractTypes contractType)
        {
            if (!ContractList.ContainsKey(contractName))
                return;
            switch (contractType)
            {
                case ContractTypes.Single:
                    SafeTrigger.ClientEvent(player,"personalEvents:updateMyContracts", JsonConvert.SerializeObject(ContractList[contractName].GetDTO(contractName)));
                    break;
                case ContractTypes.Family:
                    var family = player.GetFamily();
                    if (family != null)
                        FamilyMenuManager.UpdateContract(family, JsonConvert.SerializeObject(ContractList[contractName].GetDTO(contractName)));
                    break;
            }
        }


        public void Save(ContractTypes contractTypes, int ownerID)
        {
            if (_isChanged)
            {
                _isChanged = false;
                foreach (var contract in ContractList)
                {
                    if (contract.Value.IsChanged)
                    {
                        contract.Value.IsChanged = false;
                        MySQL.Query("UPDATE `contracts` SET " +
                            "countCompleted = @prop0, " +
                            "currentLevel = @prop1, " +
                            "inProgress = @prop2, " +
                            "expirationDate = @prop3, " +
                            "countPlayersProgress = @prop4 " +
                            "WHERE ownerid = @prop5 AND " +
                            "ownerType = @prop6 AND " +
                            "contractName = @prop7", 
                            contract.Value.CountCompleted, 
                            contract.Value.CurrentLevel, 
                            contract.Value.InProgress, 
                            MySQL.ConvertTime(contract.Value.ExpirationDate), 
                            JsonConvert.SerializeObject(contract.Value.CountPlayersProgress),
                            ownerID, (int)contractTypes, (int)contract.Key);
                    }
                }
            }
        }

        private void Save(ContractTypes contractTypes, int ownerID, ContractNames contractName)
        {
            var contractProgress = ContractList[contractName];
            contractProgress.IsChanged = false;
            MySQL.Query("UPDATE `contracts` SET " +
                "countCompleted = @prop0, " +
                "currentLevel = @prop1, " +
                "inProgress = @prop2, " +
                "expirationDate = @prop3, " +
                "countPlayersProgress = @prop4 " +
                "WHERE ownerid = @prop5 AND " +
                "ownerType = @prop6 AND " +
                "contractName = @prop7",
                contractProgress.CountCompleted,
                contractProgress.CurrentLevel,
                contractProgress.InProgress,
                MySQL.ConvertTime(contractProgress.ExpirationDate),
                JsonConvert.SerializeObject(contractProgress.CountPlayersProgress),
                ownerID, (int)contractTypes, (int)contractName);
        }

        public string GetSerializeDTOString()
        {
            return JsonConvert.SerializeObject(ContractList.Select(item => item.Value.GetDTO(item.Key)));
        }
        public int AddProgress(ExtPlayer player, ContractModel contractModel, int count)
        {
            if (!ContractList.ContainsKey(contractModel.ContractName))
                return 0;
            int cnt = ContractList[contractModel.ContractName].AddProgress(player, contractModel.ContractType, count, contractModel.MaxLevel, contractModel.ContractType == ContractTypes.Family ? contractModel.MaxLevelOnOnePlayer : contractModel.MaxLevel);
            if (cnt > 0)
                _isChanged = true;
            return cnt;
        }
        public void CompleteContract(ExtPlayer player, ContractModel contractModel)
        {
            if (!ContractList.ContainsKey(contractModel.ContractName))
                return;
            if (ContractList[contractModel.ContractName].CurrentLevel >= contractModel.MaxLevel)
            {
                ContractList[contractModel.ContractName].InProgress = false;
                switch (contractModel.ContractType)
                {
                    case ContractTypes.Single:
                        foreach (var reward in contractModel.Rewards)
                        {
                            reward.GiveReward(player, contractModel.ContractName.ToString());
                        }
                        Save(contractModel.ContractType, player.Character.UUID, contractModel.ContractName);
                        break;
                    case ContractTypes.Family:
                        var family = player.GetFamily();
                        foreach (var reward in contractModel.Rewards)
                        {
                            reward.GiveReward(family, contractModel.ContractName.ToString());
                        }
                        Save(contractModel.ContractType, player.Character.FamilyID, contractModel.ContractName);
                        break;
                }
                ContractList[contractModel.ContractName].IsChanged = false;
            }
            UpdateContract(player, contractModel.ContractName, contractModel.ContractType);
        }
    }
}
