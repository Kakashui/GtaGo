using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Inventory.Enums
{
    public enum ItemNames
    {
       
        Invalid,

        //clothes
        /*
         *  1 - Маска
            2 - Перчатки
            3 - Серьги
            4 - Штаны
            5 - Маленький рюкзак
            6 - Средний рюкзак
            7 - Большой рюкзак
            8 - Обувь
            9 - Аксессуары
            10 - Ювелирка
            11 - Футболки
            12 - Броники
            13 - Верхняя одежда
            14 - Головные уборы
            15 - Очки
            16 - Часы
            17 - Браслеты
         */
        Mask,
        Gloves,
        Ear,
        Leg,
        BackpackLight,
        BackpackMedium,
        BackpackLarge,
        Feet,
        Accessories,
        Jewelry,
        Shirt,
        BodyArmor,
        Top,
        Hat,
        Glasses,
        Watches,
        Bracelets,

        //

        Material,
        HealthKit,

        GasCan,
        Crisps,
        Beer,
        Pizza,
        Burger,
        HotDog,
        Sandwich,
        eCola,
        Sprunk,
        Lockpick,
        ArmyLockpick,
        Pocket,
        Cuffs,
        CarKey,
        Cigarettes,
        Present,
        KeyRing,
        Radio,
        Pet,
        AnimalSkin,

        /* Drinks */
        RusDrink1,
        RusDrink2,
        RusDrink3,
        YakDrink1,
        YakDrink2,
        YakDrink3,
        LcnDrink1,
        LcnDrink2,
        LcnDrink3,
        ArmDrink1,
        ArmDrink2,
        ArmDrink3,

        /* Weapons */
        /* Pistols */
        Pistol,
        CombatPistol,
        Pistol50,
        SNSPistol,
        HeavyPistol,
        VintagePistol,
        MarksmanPistol,
        Revolver,
        APPistol,
        FlareGun,
        DoubleAction,
        PistolMk2,
        SNSPistolMk2,
        RevolverMk2,
        /* SMG */
        MicroSMG,
        MachinePistol,
        SMG,
        AssaultSMG,
        CombatPDW,
        MG,
        CombatMG,
        Gusenberg,
        MiniSMG,
        SMGMk2,
        CombatMGMk2,
        /* Rifles */
        AssaultRifle,
        CarbineRifle,
        AdvancedRifle,
        SpecialCarbine,
        BullpupRifle,
        CompactRifle,
        AssaultRifleMk2,
        CarbineRifleMk2,
        SpecialCarbineMk2,
        BullpupRifleMk2,
        /* Sniper */
        SniperRifle,
        HeavySniper,
        MarksmanRifle,
        HeavySniperMk2,
        MarksmanRifleMk2,
        /* Shotguns */
        PumpShotgun,
        SawnOffShotgun,
        BullpupShotgun,
        AssaultShotgun,
        Musket,
        HeavyShotgun,
        DoubleBarrelShotgun,
        SweeperShotgun,
        PumpShotgunMk2,
        /* MELEE WEAPONS */
        StunGun,
        Knife,
        Nightstick,
        Hammer,
        Bat,
        Crowbar,
        GolfClub,
        Bottle,
        Dagger,
        Hatchet,
        KnuckleDuster,
        Machete,
        Flashlight,
        SwitchBlade,
        PoolCue,
        Wrench,
        BattleAxe,
        /* Ammo */
        PistolAmmo,
        SMGAmmo,
        RiflesAmmo,
        SniperAmmo,
        ShotgunsAmmo,
        /* Fishing */
        //rods
        LowRod,
        MiddleRod,
        HightRod,
        PerfectRod,
        //cage
        LowFishingCage,
        MiddleFishingCage,
        HightFishingCage,
        //bait
        FishingBait,
        //serach spot
        FishingMap,
        FishingLicense,

        /*Heavy Weapons(New Weapon)*/
        GrenadeLauncher,
        RPG,
        Minigun,
        Firework,
        Railgun,
        HomingLauncher,
        GrenadeLauncherSmoke,
        CompactGrenadeLauncher,

        /*Food*/
        Snickers,
        KitKat,
        Gum,
        ChickenFele,
        BeefSteak,
        Kebab,
        AlpenGold,
        Banana,
        Pumpkin,
        Orange,
        Apple,
        Peach,
        Melon,
        Lemon,

        /*Drink*/
        AppleJuice,
        CarrotJuice,
        AlpelsinJuice,
        Borjomi,
        MineralWater,
        Kvass,
        Milk,
        RedBull,
        Coffee,
        Tea,

        /*Med*/
        Adrenalin, 

        /*Alcohol*/
        WhiteWine,
        RedWine,
        Negroni,
        Pinacolada,
        Mojito,
        Daiquiri,
        TequilaSunrise,
        Margarita,
        Cristal,
        Lambrusco,
        Alexandra,
        LaurentPerrier,
        Whiskey,
        Cognac,
        Vodka,
        Chacha,
        Moonshine,
        Tequila,
        Rom,

        /*Narco*/
        Cocaine,
        Amphetamine,
        Heroin,
        Marijuana,
        LSD,
        Ecstasy,

        /*Other*/
        TobaccoForKolyannaya,
        CoalsForKolyann,
        Screwdriver,
        Milt,
        WeaponBox,
        AmmoBox,
        MedkitBox,

        Snowballs,
        ArmorBox,
        Tablet,
        StandartCostume,
        Guitar,
        Camera,
        Microphone,
        MusketAmmo,
        Binoculars,
        Lighter,
        Bong,
        Clipboard,
        Bandage,

        /*Seeds*/
        CabbageSeed,
        PumpkinSeed,
        ZucchiniSeed,
        WatermelonSeed,
        TomatoSeed,
        StrawberrySeed,
        RaspberriesSeed,
        RadishSeed,
        PotatoesSeed,
        OrangeSeed,
        CucumberSeed,
        CarrotSeed,
        BananaSeed,
        AppleSeed,

        /*Fertilizer*/
        FertilizerStandVegetable,
        FertilizerStandBerry,
        FertilizerStandFruit,
        FertilizerBigVegetable,
        FertilizerBigBerry,
        FertilizerBigFruit,

        /*Food*/
        Cabbage,
        Zucchini,
        Watermelon,
        Tomato,
        Strawberry,
        Raspberries,
        Radish,
        Potatoes,
        Cucumber,
        Carrot,

        /*Watering*/
        WateringBig,
        WateringMedium,
        WateringLow,

        FoodBox,

        /*coals*/
        CommonCoal,
        BrownCoalm,
        Anthracite,

        /*resources*/
        CommonAluminum,
        PerfectAluminum,
        CommonCopper,
        PerfectCopper,
        CommonIron,
        PerfectIron,
        Diamond,

        /*Ore*/
        AluniniumOre,
        CopperOre,
        IronOre,

        /*Ore Mining gears*/
        Dynamite,
        Detector,
        Stetoskop,

        LowHealthKit,

        LowRepairKit,

        Ball,
        ThrowablesAmmo
    }
}
