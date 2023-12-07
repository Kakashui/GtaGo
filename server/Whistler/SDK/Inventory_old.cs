using GTANetworkAPI;
using Whistler.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static Whistler.Core.ArmedBody;

namespace Whistler.SDK
{
    public enum ItemType
    {
        Mask = -1,
        Gloves = -3,
        Leg = -4,
        Bag = -5,
        Feet = -6,
        Jewelry = -7,
        Undershit = -8,
        BodyArmor = -9,
        Unknown = -10,
        Top = -11,
        Hat = -12,
        Glasses = -13,
        Accessories = -14,

        Debug = 0,
        BagWithMoney = 12,
        Material = 13,    
        Drugs = 14,       
        BagWithDrill = 15,
        HealthKit = 1,    
        GasCan = 2,       
        Crisps = 3,       
        Beer = 4,         
        Pizza = 5,        
        Burger = 6,       
        HotDog = 7,       
        Sandwich = 8,     
        eCola = 9,        
        Sprunk = 10,      
        Lockpick = 11,    
        ArmyLockpick = 16,
        Pocket = 17,      
        Cuffs = 18,       
        CarKey = 19,      
        Cigarettes = 32,  
        Present = 40,     
        KeyRing = 41,     
        Radio = 42,       
        Pet = 43,         
        AnimalSkin = 44,  
        Steak = 45,       

        /* Drinks */
        RusDrink1 = 20,
        RusDrink2 = 21,
        RusDrink3 = 22,

        YakDrink1 = 23,
        YakDrink2 = 24,
        YakDrink3 = 25,

        LcnDrink1 = 26,
        LcnDrink2 = 27,
        LcnDrink3 = 28,

        ArmDrink1 = 29,
        ArmDrink2 = 30,
        ArmDrink3 = 31,

        /* Weapons */
        /* Pistols */
        Pistol = 100,
        CombatPistol = 101,
        Pistol50 = 102,
        SNSPistol = 103,
        HeavyPistol = 104,
        VintagePistol = 105,
        MarksmanPistol = 106,
        Revolver = 107,
        APPistol = 108,
        FlareGun = 110,
        DoubleAction = 111,
        PistolMk2 = 112,
        SNSPistolMk2 = 113,
        RevolverMk2 = 114,
        /* SMG */
        MicroSMG = 115,
        MachinePistol = 116,
        SMG = 117,
        AssaultSMG = 118,
        CombatPDW = 119,
        MG = 120,
        CombatMG = 121,
        Gusenberg = 122,
        MiniSMG = 123,
        SMGMk2 = 124,
        CombatMGMk2 = 125,
        /* Rifles */
        AssaultRifle = 126,
        CarbineRifle = 127,
        AdvancedRifle = 128,
        SpecialCarbine = 129,
        BullpupRifle = 130,
        CompactRifle = 131,
        AssaultRifleMk2 = 132,
        CarbineRifleMk2 = 133,
        SpecialCarbineMk2 = 134,
        BullpupRifleMk2 = 135,
        /* Sniper */
        SniperRifle = 136,
        HeavySniper = 137,
        MarksmanRifle = 138,
        HeavySniperMk2 = 139,
        MarksmanRifleMk2 = 140,
        /* Shotguns */
        PumpShotgun = 141,
        SawnOffShotgun = 142,
        BullpupShotgun = 143,
        AssaultShotgun = 144,
        Musket = 145,
        HeavyShotgun = 146,
        DoubleBarrelShotgun = 147,
        SweeperShotgun = 148,
        PumpShotgunMk2 = 149,
        /* MELEE WEAPONS */
        StunGun = 109,
        Knife = 180,
        Nightstick = 181,
        Hammer = 182,
        Bat = 183,
        Crowbar = 184,
        GolfClub = 185,
        Bottle = 186,
        Dagger = 187,
        Hatchet = 188,
        KnuckleDuster = 189,
        Machete = 190,
        Flashlight = 191,
        SwitchBlade = 192,
        PoolCue = 193,
        Wrench = 194,
        BattleAxe = 195,
        /* Ammo */
        PistolAmmo = 200,
        SMGAmmo = 201,
        RiflesAmmo = 202,
        SniperAmmo = 203,
        ShotgunsAmmo = 204,
        /* Fishing */
        //rods
        LowRod = 250,
        MiddleRod = 251,
        HightRod = 252,
        PerfectRod = 253,
        //cage
        LowFishingCage = 254,
        MiddleFishingCage = 255,
        HightFishingCage = 256,
        //bait
        FishingBait = 257,
        //serach spot
        FishingMap = 258,
        FishingLicense = 259
    }

}
