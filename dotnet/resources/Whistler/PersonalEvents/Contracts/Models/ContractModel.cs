using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families;
using Whistler.Families.FamilyMenu;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.PersonalEvents.Models.Rewards;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Contracts.Models
{
    class ContractModel
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int MaxLevel { get; set; }
        public List<RewardBase> Rewards { get; set; }
        public List<ContractCoords> Coords { get; set; }
        public int MinReputation { get; set; }
        public int MinMembers { get; set; }
        public int MaxLevelOnOnePlayer { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ContractTypes ContractType { get; set; }
        public ContractNames ContractName { get; set; }
        public string Image { get; set; }
        public int MinutesToComplete { get; set; }
        public int PriceContract { get; set; }
        public OrgActivityType FamilyType { get; set; }
        public ContractModel(bool isActive, string name, string desc, int maxLevel, List<RewardBase> rewards, List<ContractCoords> coords, int minReputation, int minMembers, ContractTypes contractType, ContractNames contractName, string image, int minutesToComplete, int priceContract, int maxLevelOnOnePlayer, OrgActivityType familyType = OrgActivityType.Invalid)
        {
            IsActive = isActive;
            Name = name;
            Desc = desc;
            MaxLevel = maxLevel;
            Rewards = rewards;
            Coords = coords;
            MinReputation = minReputation;
            MinMembers = minMembers;
            ContractType = contractType;
            ContractName = contractName;
            Image = image;
            CreateInteractPoints();
            MinutesToComplete = minutesToComplete;
            PriceContract = priceContract;
            MaxLevelOnOnePlayer = maxLevelOnOnePlayer;
            FamilyType = familyType;
        }

        private void CreateInteractPoints()
        {
            foreach (var coord in Coords)
            {
                coord.CreateInteractions(EnterPredicate, this);
            }
        }

        private bool EnterPredicate(ColShape shape, ExtPlayer player)
        {
            return GetContractProgress(player)?.InProgress ?? false;
        }

        public ContractProgress GetContractProgress(ExtPlayer player)
        {
            return GetContractInfo(player)?.ContractList.GetValueOrDefault(ContractName);
        }

        public ContractInfo GetContractInfo(ExtPlayer player)
        {
            switch (ContractType)
            {
                case ContractTypes.Single:
                    return player.Character.EventModel.Contracts;
                case ContractTypes.Family:
                    return player.GetFamily()?.FamilyContracts;
            }
            return null;
        }


        public void AcceptContract(ExtPlayer player)
        {
            ContractInfo contractInfo = null;
            switch (ContractType)
            {
                case ContractTypes.Single:
                    contractInfo = player.Character.EventModel.Contracts;
                    break;
                case ContractTypes.Family:
                    var family = player.GetFamily();
                    if (family == null)
                    {
                        Notify.SendError(player, "options_program_25");
                        return;
                    }
                    if (!family.CanAccessToBizWar(player))
                    {
                        Notify.SendError(player, "options_program_26");
                        return;
                    }
                    if (family.OrgActiveType != FamilyType && FamilyType != OrgActivityType.Invalid)
                    {
                        Notify.SendError(player, "options_program_34");
                        return;
                    }
                    contractInfo = family.FamilyContracts;
                    break;
            }
            if (ContractType == ContractTypes.Single)
            {
                if (!Wallet.TryChange(player.Character, -PriceContract))
                {
                    Notify.SendError(player, "options_program_33");
                    return;
                }
            }
            else
            {
                if (!Wallet.TryChange(player.GetFamily(), -PriceContract))
                {
                    Notify.SendError(player, "options_program_33");
                    return;
                }
            }
            if (!contractInfo?.AcceptContract(player, this) ?? false)
                Notify.SendError(player, "options_program_24");
            if (ContractType == ContractTypes.Single)
                Wallet.MoneySub(player.Character, PriceContract, "Money_StartContract");
            else
                Wallet.MoneySub(player.GetFamily(), PriceContract, "Money_StartContract");
        }

        public int ContractProgressed(ExtPlayer player, int count)
        {
            var contractInfo = GetContractInfo(player);
            int res = contractInfo.AddProgress(player, this, count);
            if (res > 0)
            {
                contractInfo.CompleteContract(player, this);
                return res;
            }
            return 0;
        }
    }
}
