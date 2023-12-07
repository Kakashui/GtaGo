using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.NewDonateShop;
using Whistler.NewDonateShop.Configs;
using Whistler.PersonalEvents.Achievements.Models;
using Whistler.PersonalEvents.Models.Rewards;

namespace Whistler.PersonalEvents.Achievements
{
    class AchieveSettings : Script
    {
        public AchieveSettings()
        {

            Dictionary<int, AchieveParams> achieves = new Dictionary<int, AchieveParams>();
            
            foreach (var item in AchievesSettings)
            {
                achieves.Add((int)item.Key, item.Value);
            }

            
            if (Directory.Exists("client/personalEvents/configs"))
            {
                using (var w = new StreamWriter("client/personalEvents/configs/achievesConfig.js"))
                {
                    w.Write($"module.exports = {JsonConvert.SerializeObject(AchievesSettings.Values)}");
                }
                using (var w = new StreamWriter("client/personalEvents/configs/playerActionSyncParams.js"))
                {
                    w.Write($"module.exports = {JsonConvert.SerializeObject(ClientPlayerActionSyncIntervals)}");
                }
            }
            if (Directory.Exists("interfaces/gui/src/configs/personalEvents"))
            {
                using (var w = new StreamWriter("interfaces/gui/src/configs/personalEvents/achievesConfig.js"))
                {
                    w.Write($"export default {JsonConvert.SerializeObject(achieves)}");
                }
            }
        }
        public static readonly Dictionary<PlayerActions, ClientActionSyncParams> ClientPlayerActionSyncIntervals = new Dictionary<PlayerActions, ClientActionSyncParams>
        {
            [PlayerActions.Driving] = new ClientActionSyncParams(0, 5000),
        };
        public static readonly Dictionary<AchieveNames, AchieveParams> AchievesSettings = new Dictionary<AchieveNames, AchieveParams>
        {
            // [AchieveNames.Shopaholic] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Шопоголик",
            //     shortDesc: "Потратить $9.000.000 на одежду", 
            //     desc: "Потратить $9.000.000 на одежду",
            //     maxLevel: 9000000,
            //     achieveName: AchieveNames.Shopaholic,
            //     playerActions: PlayerActions.BuyClothes,
            //     rewards: new List<RewardBase> { new BonusPointReward(20), new GoCoinsReward(200), new RespectReward(10) },
            //     isClient: false),

            // [AchieveNames.ProfessionalKiller] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Профессиональный убийца",
            //     shortDesc: "Совершить 500 убийств на каптах и/или бизварах", 
            //     desc: "Совершить 500 убийств на каптах и/или бизварах",
            //     maxLevel: 500,
            //     achieveName: AchieveNames.ProfessionalKiller,
            //     playerActions: PlayerActions.KillOnCaptAndBizwar,
            //     rewards: new List<RewardBase> { new BonusPointReward(30), new GoCoinsReward(200), new RespectReward(40) },
            //     isClient: false),

            // [AchieveNames.GameManiak] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Игровой маньяк",
            //     shortDesc: "Отыграть 1500 часов", 
            //     desc: "Отыграть 1500 часов",
            //     maxLevel: 1500*60,
            //     achieveName: AchieveNames.GameManiak,
            //     playerActions: PlayerActions.PlayingOnServer,
            //     rewards: new List<RewardBase> { new BonusPointReward(100), new GoCoinsReward(1000), new DonateInventoryReward(DonateService.Items[24000]) },
            //     isClient: false),

            // [AchieveNames.KingOfTruckers] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Король дальнобойщиков",
            //     shortDesc: "Совершить 1000 перевозок", 
            //     desc: "Совершить 1000 перевозок",
            //     maxLevel: 1000,
            //     achieveName: AchieveNames.KingOfTruckers,
            //     playerActions: PlayerActions.CompleteTruckCarry,
            //     rewards: new List<RewardBase> { new BonusPointReward(100), new GoCoinsReward(200), new MoneyReward(77805) },
            //     isClient: false),

            // [AchieveNames.KindDoctor] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Добрый врач",
            //     shortDesc: "Вылечить 500 людей", 
            //     desc: "Вылечить 500 людей",
            //     maxLevel: 500,
            //     achieveName: AchieveNames.KindDoctor,
            //     playerActions: PlayerActions.HealPlayer,
            //     rewards: new List<RewardBase> { new BonusPointReward(100), new GoCoinsReward(300), new RespectReward(80) },
            //     isClient: false),

            // [AchieveNames.SuperTaxi] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Супер таксист",
            //     shortDesc: "Совершить 1500 перевозок на такси", 
            //     desc: "Совершить 1500 перевозок на такси",
            //     maxLevel: 1500,
            //     achieveName: AchieveNames.SuperTaxi,
            //     playerActions: PlayerActions.CompleteTaxiCarry,
            //     rewards: new List<RewardBase> { new BonusPointReward(20), new GoCoinsReward(300), new VehicleReward(DonateService.Items[12503]) },
            //     isClient: false),

            // [AchieveNames.MSofFishing] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Мастер спорта по рыбной ловле",
            //     shortDesc: "Поймать 2500 рыб", 
            //     desc: "Поймать 2500 рыб",
            //     maxLevel: 2500,
            //     achieveName: AchieveNames.MSofFishing,
            //     playerActions: PlayerActions.CathAFish,
            //     rewards: new List<RewardBase> { new BonusPointReward(120), new GoCoinsReward(100) },
            //     isClient: false),

            // //[AchieveNames.KingOfMP] = new AchieveParams(
            // //    isActive: true,
            // //    isHidden: false,
            // //    name: "Король мероприятий",
            // //    shortDesc: "Выиграть 50 мероприятий", 
            // //    desc: "Выиграть 50 мероприятий",
            // //    maxLevel: 2500,
            // //    achieveName: AchieveNames.KingOfMP,
            // //    playerActions: PlayerActions.WinMP,
            // //    rewards: new List<RewardBase> { new MoneyReward(10000) },
            // //    isClient: false),

            // [AchieveNames.HapplyFamilyMan] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Счастливый семьянин",
            //     shortDesc: "Соединить брачные узы воедино", 
            //     desc: "Соединить брачные узы воедино",
            //     maxLevel: 1,
            //     achieveName: AchieveNames.HapplyFamilyMan,
            //     playerActions: PlayerActions.Mariage,
            //     rewards: new List<RewardBase> { new RespectReward(60) },
            //     isClient: false),

            // //[AchieveNames.DesperateMove] = new AchieveParams(
            // //    isActive: true,
            // //    isHidden: false,
            // //    name: "Отчаянный ход",
            // //    shortDesc: "Выиграть в казино, поставив на 0", 
            // //    desc: "Выиграть в казино, поставив на 0",
            // //    maxLevel: 1,
            // //    achieveName: AchieveNames.DesperateMove,
            // //    playerActions: PlayerActions.WinInCasinoOnZero,
            // //    rewards: new List<RewardBase> { new MoneyReward(10000) },
            // //    isClient: false),

            // [AchieveNames.IronLiver] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Железная печень",
            //     shortDesc: "Выпить 300 бутылок алкоголя", 
            //     desc: "Выпить 300 бутылок алкоголя",
            //     maxLevel: 300,
            //     achieveName: AchieveNames.IronLiver,
            //     playerActions: PlayerActions.DringAlco,
            //     rewards: new List<RewardBase> { new BonusPointReward(100), new GoCoinsReward(100), new RespectReward(10) },
            //     isClient: false),

            // //[AchieveNames.RegularCustomer] = new AchieveParams(
            // //    isActive: true,
            // //    isHidden: false,
            // //    name: "Постоянный клиент",
            // //    shortDesc: "Выпивать 10 бокалов алкоголя 10 дней подряд", 
            // //    desc: "Выпивать 10 бокалов алкоголя 10 дней подряд",
            // //    maxLevel: 100,
            // //    achieveName: AchieveNames.RegularCustomer,
            // //    playerActions: PlayerActions.DringAlco,
            // //    rewards: new List<RewardBase> { new MoneyReward(10000) },
            // //    isClient: false),

            // [AchieveNames.FromTheHeart] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "От чистого сердца",
            //     shortDesc: "Пожертвовать 1кк в мерии", 
            //     desc: "Пожертвовать 1кк в мерии",
            //     maxLevel: 1000000,
            //     achieveName: AchieveNames.FromTheHeart,
            //     playerActions: PlayerActions.DonateInGOV,
            //     rewards: new List<RewardBase> { new BonusPointReward(30), new RespectReward(60) },
            //     isClient: false),

            // [AchieveNames.LoveOurCountry] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Люблю нашу страну",
            //     shortDesc: "Пожертвовать в банке 1кк", 
            //     desc: "Пожертвовать в банке 1кк",
            //     maxLevel: 1000000,
            //     achieveName: AchieveNames.LoveOurCountry,
            //     playerActions: PlayerActions.DonateInBank,
            //     rewards: new List<RewardBase> { new BonusPointReward(50), new GoCoinsReward(50), new RespectReward(30) },
            //     isClient: false),

            // [AchieveNames.WeAreForTheState] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "А мы вообще за государство",
            //     shortDesc: "Помоги государству сохранить ресурсы", 
            //     desc: "Помоги государству сохранить ресурсы",
            //     maxLevel: 1,
            //     achieveName: AchieveNames.WeAreForTheState,
            //     playerActions: PlayerActions.RepairNGStation,
            //     rewards: new List<RewardBase> { new BonusPointReward(50), new RespectReward(100) },
            //     isClient: false),

            // //[AchieveNames.TheBestHunter] = new AchieveParams(
            // //    isActive: true,
            // //    isHidden: false,
            // //    name: "Лучший охотник",
            // //    shortDesc: "Убить 500 животных на охоте", 
            // //    desc: "Убить 500 животных на охоте",
            // //    maxLevel: 500,
            // //    achieveName: AchieveNames.TheBestHunter,
            // //    playerActions: PlayerActions.KillAnimal,
            // //    rewards: new List<RewardBase> { new MoneyReward(10000) },
            // //    isClient: false),

            // [AchieveNames.HeadOfFarmers] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Начальник фермеров",
            //     shortDesc: "Выполнить 15000 действий на ферме", 
            //     desc: "Выполнить 15000 действий на ферме",
            //     maxLevel: 15000,
            //     achieveName: AchieveNames.HeadOfFarmers,
            //     playerActions: PlayerActions.MoveOnFarm,
            //     rewards: new List<RewardBase> { new BonusPointReward(200), new GoCoinsReward(500), new DonateInventoryReward(DonateService.Items[24000]) },
            //     isClient: false),

            // [AchieveNames.KingOfRacing] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Король гонок",
            //     shortDesc: "Победить 100 раз в гонках на арене",
            //     desc: "Победить 100 раз в гонках на арене",
            //     maxLevel: 100,
            //     achieveName: AchieveNames.KingOfRacing,
            //     playerActions: PlayerActions.WinRace,
            //     rewards: new List<RewardBase> { new GoCoinsReward(1000), new RespectReward(100) },
            //     isClient: false),

            // //[AchieveNames.KingOfArena] = new AchieveParams(
            // //    isActive: true,
            // //    isHidden: false,
            // //    name: "Король арены",
            // //    shortDesc: "Завершить 100 матчей на Арене",
            // //    desc: "Завершить 100 матчей на Арене",
            // //    maxLevel: 100,
            // //    achieveName: AchieveNames.KingOfArena,
            // //    playerActions: PlayerActions.WinArena,
            // //    rewards: new List<RewardBase> { new MoneyReward(10000) },
            // //    isClient: false),

            // [AchieveNames.TheBestShooter] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Лучший стрелок",
            //     shortDesc: "победить 5 раз в Охоте за головами",
            //     desc: "победить 5 раз в Охоте за головами",
            //     maxLevel: 5,
            //     achieveName: AchieveNames.TheBestShooter,
            //     playerActions: PlayerActions.WinRoyalBattle,
            //     rewards: new List<RewardBase> { new GoCoinsReward(1500) },
            //     isClient: false),

            // [AchieveNames.TheBestMiner] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Лучший рудокоп",
            //     shortDesc: "Подорвать 1000 зарядов динамита",
            //     desc: "Подорвать 1000 зарядов динамита",
            //     maxLevel: 1000,
            //     achieveName: AchieveNames.TheBestMiner,
            //     playerActions: PlayerActions.ExplodeDynamite,
            //     rewards: new List<RewardBase> { new GoCoinsReward(1500) },
            //     isClient: false),

            // [AchieveNames.Lucky] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Везунчик",
            //     shortDesc: "Получить алмаз при переплавке металла",
            //     desc: "Получить алмаз при переплавке металла",
            //     maxLevel: 1,
            //     achieveName: AchieveNames.Lucky,
            //     playerActions: PlayerActions.MakeDiamond,
            //     rewards: new List<RewardBase> { new GoCoinsReward(1500) },
            //     isClient: false),

            // [AchieveNames.Lucky] = new AchieveParams(
            //     isActive: true,
            //     isHidden: false,
            //     name: "Взломщик",
            //     shortDesc: "Взломать 1000 домов",
            //     desc: "Взломать 1000 домов",
            //     maxLevel: 1000,
            //     achieveName: AchieveNames.HouseCracker,
            //     playerActions: PlayerActions.BreakHouseLock,
            //     rewards: new List<RewardBase> { new GoCoinsReward(1500), new RespectReward(100), new BonusPointReward(200) },
            //     isClient: false),





            // //[AchieveNames.HideAchieve] = new AchieveParams(
            // //    isActive: true,
            // //    isHidden: true,
            // //    name: "Новогодний Дэд Моррроз",
            // //    shortDesc: "Подарить 50 подарков",
            // //    desc: "Подарить 50 подарков во время события 2022 НГ",
            // //    maxLevel: 50,
            // //    achieveName: AchieveNames.HideAchieve,
            // //    playerActions: PlayerActions.GivePresent,
            // //    rewards: new List<RewardBase> { new MoneyReward(10000) },
            // //    isClient: false),

            // //[AchieveNames.Driving5000KM] = new AchieveParams(
            // //    isActive: true,
            // //    isHidden: false,
            // //    name: "Даша путешественница",
            // //    shortDesc: "Проехать 5000 км", 
            // //    desc: "Проехать 5000 км на любом автотранспорте за рулем",
            // //    maxLevel: 5000000,
            // //    achieveName: AchieveNames.Driving5000KM,
            // //    playerActions: PlayerActions.Driving,
            // //    rewards: new List<RewardBase> { new MoneyReward(100000) },
            // //    isClient: true),
        };
    }
}
