using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.PersonalEvents.Contracts.ContractActions;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.PersonalEvents.Models.Rewards;

namespace Whistler.PersonalEvents.Contracts
{
    class ContractSettings : Script
    {
        public ContractSettings()
        {

            Dictionary<int, ContractModel> contracts = new Dictionary<int, ContractModel>();
            foreach (var item in ContractsSettings)
            {
                contracts.Add((int)item.Key, item.Value);
            }

            if (Directory.Exists("interfaces/gui/src/configs/personalEvents"))
            {
                using (var w = new StreamWriter("interfaces/gui/src/configs/personalEvents/contractConfig.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(contracts)}");
                }
            }

            
            if (Directory.Exists("client/personalEvents/configs"))
            {
                using (var w = new StreamWriter("client/personalEvents/configs/contractsConfig.js"))
                {
                    w.Write($"module.exports = {JsonConvert.SerializeObject(ContractsSettings.Values)}");
                }
            }
        }
        private static readonly Dictionary<AbstractItemNames, AbstractItemConfig> ItemConfigs = new Dictionary<AbstractItemNames, AbstractItemConfig>
        {
            [AbstractItemNames.Cement] = new AbstractItemConfig(8000),
            [AbstractItemNames.Meat] = new AbstractItemConfig(10000),
            [AbstractItemNames.Rubbish] = new AbstractItemConfig(5000),
            [AbstractItemNames.WeaponBox] = new AbstractItemConfig(100000),
        };
        public static AbstractItemConfig GetAbstractItemConfig(AbstractItemNames abstractItemName)
        {
            if (ItemConfigs.ContainsKey(abstractItemName))
                return ItemConfigs[abstractItemName];
            return new AbstractItemConfig();
        }
        public static readonly Dictionary<ContractNames, ContractModel> ContractsSettings = new Dictionary<ContractNames, ContractModel>
        {
            [ContractNames.TransferCement] = new ContractModel(
                isActive: true,
                name: "Цемент",
                desc: "На цементном заводе работает некий Билл, проявите дружелюбие и помогите перевезти цемент на стройку",
                maxLevel: 625,
                rewards: new List<RewardBase> { new MoneyReward(1250000) },
                coords: new List<ContractCoords>()
                {
                    new ContractCoords("Cement", new Vector3(973.4, -1937.1, 31.32), new GetItemInHand(AbstractItemNames.Cement)),
                    new ContractCoords("CementStock", new Vector3(-99.99, -1049.9, 26.45), new StockOnVehicleItem(AbstractItemNames.Cement)),
                },
                minReputation: 10,
                minMembers: 5,
                contractType: ContractTypes.Family,
                contractName: ContractNames.TransferCement,
                image: "contract-1.png",
                minutesToComplete: 1440,
                priceContract: 250000,
                maxLevelOnOnePlayer: 125),

            [ContractNames.TransferMeat] = new ContractModel(
                isActive: true,
                name: "Мясо",
                desc: "Помоги сотрудникам мясокомбината в Палетто Бэй перевезти продукцию на склад, нужна машина с большим багажником",
                maxLevel: 100,
                rewards: new List<RewardBase> { new MoneyReward(100_000) },
                coords: new List<ContractCoords>()
                {
                    new ContractCoords("Meat", new Vector3(-107.6714, 6201.532, 30.125759), new GetItemInHand(AbstractItemNames.Meat)),
                    new ContractCoords("MeatStock", new Vector3(1243.2894, -3149.1973, 4.628235), new StockOnVehicleItem(AbstractItemNames.Meat)),
                },
                minReputation: 1,
                minMembers: 1,
                contractType: ContractTypes.Single,
                contractName: ContractNames.TransferMeat,
                image: "contract-2.png",
                minutesToComplete: 240,
                priceContract: 10000,
                maxLevelOnOnePlayer: 100,
                familyType: Common.OrgActivityType.Invalid),

            [ContractNames.DeliverWeapons] = new ContractModel(
                isActive: true,
                name: "А кто оружие возить будет",
                desc: "Развезите ящики с оружием по магазинам, не забудьте про охрану",
                maxLevel: 10,
                rewards: new List<RewardBase> { new MoneyReward(1250000) },
                coords: new List<ContractCoords>()
                {
                    new ContractCoords("Weapons", new Vector3(-285.3027, -2679.0317, 5.549989), new GetItemInHand(AbstractItemNames.WeaponBox)),
                    new ContractCoords(
                        "Gun Shops", 
                        new List<Vector3> 
                        { 
                            new Vector3(-1144.26721, 2681.32739, 17.1934849),
                            new Vector3(-1322.40625, -393.3032, 35.5468575),
                            new Vector3(-3157.39526, 1128.05591, 19.9437576),
                            new Vector3(-320.936981, 6096.156, 30.5619617),
                            new Vector3(-665.8908, -951.2752, 20.5285564),
                            new Vector3(1705.17224, 3761.89331, 33.36404),
                            new Vector3(234.880936, -34.6257629, 68.81105),
                            new Vector3(2582.83228, 294.0868, 107.5573),
                            new Vector3(28.9141788, -1109.4165, 28.4019276),
                            new Vector3(822.096436, -2143.62378, 27.8903347),
                        }, 
                        new StockOnVehicleItem(AbstractItemNames.WeaponBox)),
                },
                minReputation: 10,
                minMembers: 10,
                contractType: ContractTypes.Family,
                contractName: ContractNames.DeliverWeapons,
                image: "contract-1.png",
                minutesToComplete: 1440,
                priceContract: 250000,
                maxLevelOnOnePlayer: 1,
                familyType: Common.OrgActivityType.Invalid),

            //[ContractNames.ClearCity] = new ContractModel(
            //    isActive: true,
            //    name: "Чистка мусора",
            //    desc: "Мы за чистый город, помогите нашему городу получить 1 место в мире по чистоте, помогите отряду скаутов собрать нужное количество человек и вывезти мусор, раскиданный по всему штату",
            //    maxLevel: 100,
            //    rewards: new List<RewardBase> { new MoneyReward(80000) },
            //    coords: new List<ContractCoords>()
            //    {
            //        new ContractCoords("GarbageDump ", new Vector3(2416.77, 3096.18, 47.25), new StockOnVehicleItem(AbstractItemNames.Rubbish)),
            //    },
            //    minReputation: 1,
            //    minMembers: 1,
            //    contractType: ContractTypes.Single,
            //    contractName: ContractNames.ClearCity,
            //    image: "contract-2.png",
            //    minutesToComplete: 240,
            //    priceContract: 8000,
            //    maxLevelOnOnePlayer: 100),




            //[ContractNames.TransferMicroChips] = new ContractModel(
            //    isActive: true, 
            //    name: "Контракт на микросхемы", 
            //    desc: "Доставить микросхемы на склад", 
            //    maxLevel: 5, 
            //    rewards: new List<RewardBase> { new MoneyReward(10000) }, 
            //    coords: new List<ContractCoords>() 
            //    { 
            //        new ContractCoords("MicroChips", new Vector3(0, 0, 71.13), new GetItemInHand(AbstractItemNames.MicroChip)),
            //        new ContractCoords("MicroChipsStock", new Vector3(5, 0, 71.13), new StockOnVehicleItem(AbstractItemNames.MicroChip)),
            //    }, 
            //    minReputation: 10, 
            //    minMembers: 10, 
            //    contractType: ContractTypes.Family, 
            //    contractName: ContractNames.TransferMicroChips,
            //    image: "contract-1.png",
            //    minutesToComplete: 240,
            //    priceContract: 5000,
            //    maxLevelOnOnePlayer: 5),
        };
    }
}
