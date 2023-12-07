using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Whistler.GUI;
using Whistler.MoneySystem;
using Whistler.SDK;
using Whistler.Businesses;
using Whistler.Houses;
using Whistler.Helpers;
using Whistler.Customization.Models;
using Whistler.Customization.Enums;
using Whistler.Customization;
using Whistler.Core.QuestPeds;
using Whistler.Businesses.Models;
using Whistler.Entities;

namespace Whistler.Core
{
    internal class BusinessTattoo
    {
        public List<TattooZones> Slots { get; set; }
        public string Name { get; set; }
        public uint Dictionary { get; set; }
        public uint MaleHash { get; set; }
        public uint FemaleHash { get; set; }
        public int Price { get; set; }

        public BusinessTattoo(List<TattooZones> slots, string name, string dictionary, string malehash, string femalehash, int price)
        {
            Slots = slots;
            Name = name;
            Dictionary = NAPI.Util.GetHashKey(dictionary);
            MaleHash = NAPI.Util.GetHashKey(malehash);
            FemaleHash = NAPI.Util.GetHashKey(femalehash);
            Price = price;
        }
    }

    partial class BusinessManager : Script
    {
        private static List<BusinessTattoo> _allTattoosConfig { get; set; }
        public static List<List<BusinessTattoo>> BusinessTattoos = new List<List<BusinessTattoo>>()
        {
            // Torso
            new List<BusinessTattoo>()
            {
	            // Левый сосок  -   0
                // Правый сосок -   1
                // Живот        -   2
                // Левый низ спины    -   3
	            // Правый низ спины    -   4
                // Левый верх спины   -   5
                // Правый верх спины   -   6
                // Левый бок    -   7
                // Правый бок   -   8
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Refined Hustler", "mpbusiness_overlays", "MP_Buis_M_Stomach_000", String.Empty, 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Rich", "mpbusiness_overlays", "MP_Buis_M_Chest_000", String.Empty, 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "$$$", "mpbusiness_overlays", "MP_Buis_M_Chest_001", String.Empty, 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Makin' Paper", "mpbusiness_overlays", "MP_Buis_M_Back_000", String.Empty, 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "High Roller", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Chest_000", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Makin' Money", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Chest_001", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Love Money", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Chest_002", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Diamond Back", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Stom_000", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightEdge }, "Santo Capra Logo", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Stom_001", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightEdge }, "Money Bag", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Stom_002", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Respect", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Back_000", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Gold Digger", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Back_001", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Carp Outline", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_005", "MP_Xmas2_F_Tat_005", 40000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Carp Shaded", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_006", "MP_Xmas2_F_Tat_006", 40000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Time To Die", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_009", "MP_Xmas2_F_Tat_009", 9000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Roaring Tiger", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_011", "MP_Xmas2_F_Tat_011", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftEdge }, "Lizard", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_013", "MP_Xmas2_F_Tat_013", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Japanese Warrior", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_015", "MP_Xmas2_F_Tat_015", 18000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Loose Lips Outline", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_016", "MP_Xmas2_F_Tat_016", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Loose Lips Color", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_017", "MP_Xmas2_F_Tat_017", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest}, "Royal Dagger Outline", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_018", "MP_Xmas2_F_Tat_018", 18000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest}, "Royal Dagger Color", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_019", "MP_Xmas2_F_Tat_019", 18000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy, TattooZones.RightEdge }, "Executioner", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_028", "MP_Xmas2_F_Tat_028", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Bullet Proof", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_000_M", "MP_Gunrunning_Tattoo_000_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Crossed Weapons", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_001_M", "MP_Gunrunning_Tattoo_001_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Butterfly Knife", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_009_M", "MP_Gunrunning_Tattoo_009_F", 16000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Cash Money", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_010_M", "MP_Gunrunning_Tattoo_010_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Dollar Daggers", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_012_M", "MP_Gunrunning_Tattoo_012_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Wolf Insignia", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_013_M", "MP_Gunrunning_Tattoo_013_F", 16000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Backstabber", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_014_M", "MP_Gunrunning_Tattoo_014_F", 17000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Dog Tags", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_017_M", "MP_Gunrunning_Tattoo_017_F", 17000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Dual Wield Skull", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_018_M", "MP_Gunrunning_Tattoo_018_F", 17000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Pistol Wings", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_019_M", "MP_Gunrunning_Tattoo_019_F", 17000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Crowned Weapons", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_020_M", "MP_Gunrunning_Tattoo_020_F", 17000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack }, "Explosive Heart", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_022_M", "MP_Gunrunning_Tattoo_022_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Micro SMG Chain", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_028_M", "MP_Gunrunning_Tattoo_028_F", 17000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Win Some Lose Some", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_029_M", "MP_Gunrunning_Tattoo_029_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Crossed Arrows", "mphipster_overlays", "FM_Hip_M_Tat_000", "FM_Hip_F_Tat_000", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Chemistry", "mphipster_overlays", "FM_Hip_M_Tat_002", "FM_Hip_F_Tat_002", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftEdge }, "Feather Birds", "mphipster_overlays", "FM_Hip_M_Tat_006", "FM_Hip_F_Tat_006", 1500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Infinity", "mphipster_overlays", "FM_Hip_M_Tat_011", "FM_Hip_F_Tat_011", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Antlers", "mphipster_overlays", "FM_Hip_M_Tat_012", "FM_Hip_F_Tat_012", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Boombox", "mphipster_overlays", "FM_Hip_M_Tat_013", "FM_Hip_F_Tat_013", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightTopBack }, "Pyramid", "mphipster_overlays", "FM_Hip_M_Tat_024", "FM_Hip_F_Tat_024", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack }, "Watch Your Step", "mphipster_overlays", "FM_Hip_M_Tat_025", "FM_Hip_F_Tat_025", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy, TattooZones.RightEdge }, "Sad", "mphipster_overlays", "FM_Hip_M_Tat_029", "FM_Hip_F_Tat_029", 25500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Shark Fin", "mphipster_overlays", "FM_Hip_M_Tat_030", "FM_Hip_F_Tat_030", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Skateboard", "mphipster_overlays", "FM_Hip_M_Tat_031", "FM_Hip_F_Tat_031", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightTopBack }, "Paper Plane", "mphipster_overlays", "FM_Hip_M_Tat_032", "FM_Hip_F_Tat_032", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Stag", "mphipster_overlays", "FM_Hip_M_Tat_033", "FM_Hip_F_Tat_033", 16500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy, TattooZones.RightEdge }, "Sewn Heart", "mphipster_overlays", "FM_Hip_M_Tat_035", "FM_Hip_F_Tat_035", 25750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack }, "Tooth", "mphipster_overlays", "FM_Hip_M_Tat_041", "FM_Hip_F_Tat_041", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Triangles", "mphipster_overlays", "FM_Hip_M_Tat_046", "FM_Hip_F_Tat_046", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Cassette", "mphipster_overlays", "FM_Hip_M_Tat_047", "FM_Hip_F_Tat_047", 12750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Block Back", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_000_M", "MP_MP_ImportExport_Tat_000_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Power Plant", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_001_M", "MP_MP_ImportExport_Tat_001_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Tuned to Death", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_002_M", "MP_MP_ImportExport_Tat_002_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Serpents of Destruction", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_009_M", "MP_MP_ImportExport_Tat_009_F", 5000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Take the Wheel", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_010_M", "MP_MP_ImportExport_Tat_010_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Talk Shit Get Hit", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_011_M", "MP_MP_ImportExport_Tat_011_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "King Fight", "mplowrider_overlays", "MP_LR_Tat_001_M", "MP_LR_Tat_001_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Holy Mary", "mplowrider_overlays", "MP_LR_Tat_002_M", "MP_LR_Tat_002_F", 17500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftEdge }, "Gun Mic", "mplowrider_overlays", "MP_LR_Tat_004_M", "MP_LR_Tat_004_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightTopBack }, "Amazon", "mplowrider_overlays", "MP_LR_Tat_009_M", "MP_LR_Tat_009_F", 4750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Bad Angel", "mplowrider_overlays", "MP_LR_Tat_010_M", "MP_LR_Tat_010_F", 9000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Love Gamble", "mplowrider_overlays", "MP_LR_Tat_013_M", "MP_LR_Tat_013_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Love is Blind", "mplowrider_overlays", "MP_LR_Tat_014_M", "MP_LR_Tat_014_F", 9250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Sad Angel", "mplowrider_overlays", "MP_LR_Tat_021_M", "MP_LR_Tat_021_F", 7500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Royal Takeover", "mplowrider_overlays", "MP_LR_Tat_026_M", "MP_LR_Tat_026_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Turbulence", "mpairraces_overlays", "MP_Airraces_Tattoo_000_M", "MP_Airraces_Tattoo_000_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Pilot Skull", "mpairraces_overlays", "MP_Airraces_Tattoo_001_M", "MP_Airraces_Tattoo_001_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Winged Bombshell", "mpairraces_overlays", "MP_Airraces_Tattoo_002_M", "MP_Airraces_Tattoo_002_F", 15250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack,  TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Balloon Pioneer", "mpairraces_overlays", "MP_Airraces_Tattoo_004_M", "MP_Airraces_Tattoo_004_F", 7000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Parachute Belle", "mpairraces_overlays", "MP_Airraces_Tattoo_005_M", "MP_Airraces_Tattoo_005_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Bombs Away", "mpairraces_overlays", "MP_Airraces_Tattoo_006_M", "MP_Airraces_Tattoo_006_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Eagle Eyes", "mpairraces_overlays", "MP_Airraces_Tattoo_007_M", "MP_Airraces_Tattoo_007_F", 15250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Demon Rider", "mpbiker_overlays", "MP_MP_Biker_Tat_000_M", "MP_MP_Biker_Tat_000_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Both Barrels", "mpbiker_overlays", "MP_MP_Biker_Tat_001_M", "MP_MP_Biker_Tat_001_F", 17500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Web Rider", "mpbiker_overlays", "MP_MP_Biker_Tat_003_M", "MP_MP_Biker_Tat_003_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Made In America", "mpbiker_overlays", "MP_MP_Biker_Tat_005_M", "MP_MP_Biker_Tat_005_F", 18500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Chopper Freedom", "mpbiker_overlays", "MP_MP_Biker_Tat_006_M", "MP_MP_Biker_Tat_006_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Freedom Wheels", "mpbiker_overlays", "MP_MP_Biker_Tat_008_M", "MP_MP_Biker_Tat_008_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Skull Of Taurus", "mpbiker_overlays", "MP_MP_Biker_Tat_010_M", "MP_MP_Biker_Tat_010_F", 4250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "R.I.P. My Brothers", "mpbiker_overlays", "MP_MP_Biker_Tat_011_M", "MP_MP_Biker_Tat_011_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Demon Crossbones", "mpbiker_overlays", "MP_MP_Biker_Tat_013_M", "MP_MP_Biker_Tat_013_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Clawed Beast", "mpbiker_overlays", "MP_MP_Biker_Tat_017_M", "MP_MP_Biker_Tat_017_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Skeletal Chopper", "mpbiker_overlays", "MP_MP_Biker_Tat_018_M", "MP_MP_Biker_Tat_018_F", 14000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Gruesome Talons", "mpbiker_overlays", "MP_MP_Biker_Tat_019_M", "MP_MP_Biker_Tat_019_F", 19750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Flaming Reaper", "mpbiker_overlays", "MP_MP_Biker_Tat_021_M", "MP_MP_Biker_Tat_021_F", 16250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Western MC", "mpbiker_overlays", "MP_MP_Biker_Tat_023_M", "MP_MP_Biker_Tat_023_F", 19750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "American Dream", "mpbiker_overlays", "MP_MP_Biker_Tat_026_M", "MP_MP_Biker_Tat_026_F", 19250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Bone Wrench", "mpbiker_overlays", "MP_MP_Biker_Tat_029_M", "MP_MP_Biker_Tat_029_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Brothers For Life", "mpbiker_overlays", "MP_MP_Biker_Tat_030_M", "MP_MP_Biker_Tat_030_F", 16500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Gear Head", "mpbiker_overlays", "MP_MP_Biker_Tat_031_M", "MP_MP_Biker_Tat_031_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Western Eagle", "mpbiker_overlays", "MP_MP_Biker_Tat_032_M", "MP_MP_Biker_Tat_032_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Brotherhood of Bikes", "mpbiker_overlays", "MP_MP_Biker_Tat_034_M", "MP_MP_Biker_Tat_034_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Gas Guzzler", "mpbiker_overlays", "MP_MP_Biker_Tat_039_M", "MP_MP_Biker_Tat_039_F", 20250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "No Regrets", "mpbiker_overlays", "MP_MP_Biker_Tat_041_M", "MP_MP_Biker_Tat_041_F", 16500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Ride Forever", "mpbiker_overlays", "MP_MP_Biker_Tat_043_M", "MP_MP_Biker_Tat_043_F", 15500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Unforgiven", "mpbiker_overlays", "MP_MP_Biker_Tat_050_M", "MP_MP_Biker_Tat_050_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Biker Mount", "mpbiker_overlays", "MP_MP_Biker_Tat_052_M", "MP_MP_Biker_Tat_052_F", 18500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Reaper Vulture", "mpbiker_overlays", "MP_MP_Biker_Tat_058_M", "MP_MP_Biker_Tat_058_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Faggio", "mpbiker_overlays", "MP_MP_Biker_Tat_059_M", "MP_MP_Biker_Tat_059_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "We Are The Mods!", "mpbiker_overlays", "MP_MP_Biker_Tat_060_M", "MP_MP_Biker_Tat_060_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack}, "SA Assault", "mplowrider2_overlays", "MP_LR_Tat_000_M", "MP_LR_Tat_000_F", 15500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack}, "Love the Game", "mplowrider2_overlays", "MP_LR_Tat_008_M", "MP_LR_Tat_008_F", 15250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftEdge }, "Lady Liberty", "mplowrider2_overlays", "MP_LR_Tat_011_M", "MP_LR_Tat_011_F", 15500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Royal Kiss", "mplowrider2_overlays", "MP_LR_Tat_012_M", "MP_LR_Tat_012_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Two Face", "mplowrider2_overlays", "MP_LR_Tat_016_M", "MP_LR_Tat_016_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Death Behind", "mplowrider2_overlays", "MP_LR_Tat_019_M", "MP_LR_Tat_019_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Dead Pretty", "mplowrider2_overlays", "MP_LR_Tat_031_M", "MP_LR_Tat_031_F", 15250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Reign Over", "mplowrider2_overlays", "MP_LR_Tat_032_M", "MP_LR_Tat_032_F", 15600),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Abstract Skull", "mpluxe_overlays", "MP_LUXE_TAT_003_M", "MP_LUXE_TAT_003_F", 20750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Eye of the Griffin", "mpluxe_overlays", "MP_LUXE_TAT_007_M", "MP_LUXE_TAT_007_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Flying Eye", "mpluxe_overlays", "MP_LUXE_TAT_008_M", "MP_LUXE_TAT_008_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Ancient Queen", "mpluxe_overlays", "MP_LUXE_TAT_014_M", "MP_LUXE_TAT_014_F", 3600),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Smoking Sisters", "mpluxe_overlays", "MP_LUXE_TAT_015_M", "MP_LUXE_TAT_015_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack}, "Feather Mural", "mpluxe_overlays", "MP_LUXE_TAT_024_M", "MP_LUXE_TAT_024_F", 15250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "The Howler", "mpluxe2_overlays", "MP_LUXE_TAT_002_M", "MP_LUXE_TAT_002_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest, TattooZones.RightChest, TattooZones.Tummy}, "Geometric Galaxy", "mpluxe2_overlays", "MP_LUXE_TAT_012_M", "MP_LUXE_TAT_012_F", 10000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Cloaked Angel", "mpluxe2_overlays", "MP_LUXE_TAT_022_M", "MP_LUXE_TAT_022_F", 9000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Reaper Sway", "mpluxe2_overlays", "MP_LUXE_TAT_025_M", "MP_LUXE_TAT_025_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Cobra Dawn", "mpluxe2_overlays", "MP_LUXE_TAT_027_M", "MP_LUXE_TAT_027_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Geometric Design T", "mpluxe2_overlays", "MP_LUXE_TAT_029_M", "MP_LUXE_TAT_029_F", 7500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Bless The Dead", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_000_M", "MP_Smuggler_Tattoo_000_F", 1500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Dead Lies", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_002_M", "MP_Smuggler_Tattoo_002_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Give Nothing Back", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_003_M", "MP_Smuggler_Tattoo_003_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Never Surrender", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_006_M", "MP_Smuggler_Tattoo_006_F", 15500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "No Honor", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_007_M", "MP_Smuggler_Tattoo_007_F", 17500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack}, "Tall Ship Conflict", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_009_M", "MP_Smuggler_Tattoo_009_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "See You In Hell", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_010_M", "MP_Smuggler_Tattoo_010_F", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Torn Wings", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_013_M", "MP_Smuggler_Tattoo_013_F", 15500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Jolly Roger", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_015_M", "MP_Smuggler_Tattoo_015_F", 17000),
                new BusinessTattoo(new List<TattooZones>(){TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Skull Compass", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_016_M", "MP_Smuggler_Tattoo_016_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Framed Tall Ship", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_017_M", "MP_Smuggler_Tattoo_017_F", 7500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Finders Keepers", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_018_M", "MP_Smuggler_Tattoo_018_F", 9000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Lost At Sea", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_019_M", "MP_Smuggler_Tattoo_019_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest, TattooZones.RightChest }, "Dead Tales", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_021_M", "MP_Smuggler_Tattoo_021_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack }, "X Marks The Spot", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_022_M", "MP_Smuggler_Tattoo_022_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Pirate Captain", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_024_M", "MP_Smuggler_Tattoo_024_F", 7500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Claimed By The Beast", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_025_M", "MP_Smuggler_Tattoo_025_F", 7500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest, TattooZones.RightChest }, "Wheels of Death", "mpstunt_overlays", "MP_MP_Stunt_Tat_011_M", "MP_MP_Stunt_Tat_011_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftEdge }, "Punk Biker", "mpstunt_overlays", "MP_MP_Stunt_Tat_012_M", "MP_MP_Stunt_Tat_012_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Bat Cat of Spades", "mpstunt_overlays", "MP_MP_Stunt_Tat_014_M", "MP_MP_Stunt_Tat_014_F", 4100),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest}, "Vintage Bully", "mpstunt_overlays", "MP_MP_Stunt_Tat_018_M", "MP_MP_Stunt_Tat_018_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Engine Heart", "mpstunt_overlays", "MP_MP_Stunt_Tat_019_M", "MP_MP_Stunt_Tat_019_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Road Kill", "mpstunt_overlays", "MP_MP_Stunt_Tat_024_M", "MP_MP_Stunt_Tat_024_F", 7000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Winged Wheel", "mpstunt_overlays", "MP_MP_Stunt_Tat_026_M", "MP_MP_Stunt_Tat_026_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Punk Road Hog", "mpstunt_overlays", "MP_MP_Stunt_Tat_027_M", "MP_MP_Stunt_Tat_027_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Majestic Finish", "mpstunt_overlays", "MP_MP_Stunt_Tat_029_M", "MP_MP_Stunt_Tat_029_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightTopBack }, "Man's Ruin", "mpstunt_overlays", "MP_MP_Stunt_Tat_030_M", "MP_MP_Stunt_Tat_030_F", 15500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Sugar Skull Trucker", "mpstunt_overlays", "MP_MP_Stunt_Tat_033_M", "MP_MP_Stunt_Tat_033_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack}, "Feather Road Kill", "mpstunt_overlays", "MP_MP_Stunt_Tat_034_M", "MP_MP_Stunt_Tat_034_F", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack }, "Big Grills", "mpstunt_overlays", "MP_MP_Stunt_Tat_037_M", "MP_MP_Stunt_Tat_037_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Monkey Chopper", "mpstunt_overlays", "MP_MP_Stunt_Tat_040_M", "MP_MP_Stunt_Tat_040_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Brapp", "mpstunt_overlays", "MP_MP_Stunt_Tat_041_M", "MP_MP_Stunt_Tat_041_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Ram Skull", "mpstunt_overlays", "MP_MP_Stunt_Tat_044_M", "MP_MP_Stunt_Tat_044_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Full Throttle", "mpstunt_overlays", "MP_MP_Stunt_Tat_046_M", "MP_MP_Stunt_Tat_046_F", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack}, "Racing Doll", "mpstunt_overlays", "MP_MP_Stunt_Tat_048_M", "MP_MP_Stunt_Tat_048_F", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "Blackjack", "multiplayer_overlays", "FM_Tat_Award_M_003", "FM_Tat_Award_F_003", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Hustler", "multiplayer_overlays", "FM_Tat_Award_M_004", "FM_Tat_Award_F_004", 4250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Angel", "multiplayer_overlays", "FM_Tat_Award_M_005", "FM_Tat_Award_F_005", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Los Santos Customs", "multiplayer_overlays", "FM_Tat_Award_M_008", "FM_Tat_Award_F_008", 12400),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Blank Scroll", "multiplayer_overlays", "FM_Tat_Award_M_011", "FM_Tat_Award_F_011", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Embellished Scroll", "multiplayer_overlays", "FM_Tat_Award_M_012", "FM_Tat_Award_F_012", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Seven Deadly Sins", "multiplayer_overlays", "FM_Tat_Award_M_013", "FM_Tat_Award_F_013", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack }, "Trust No One", "multiplayer_overlays", "FM_Tat_Award_M_014", "FM_Tat_Award_F_014", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Clown", "multiplayer_overlays", "FM_Tat_Award_M_016", "FM_Tat_Award_F_016", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Clown and Gun", "multiplayer_overlays", "FM_Tat_Award_M_017", "FM_Tat_Award_F_017", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Clown Dual Wield", "multiplayer_overlays", "FM_Tat_Award_M_018", "FM_Tat_Award_F_018", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Clown Dual Wield Dollars", "multiplayer_overlays", "FM_Tat_Award_M_019", "FM_Tat_Award_F_019", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Faith T", "multiplayer_overlays", "FM_Tat_M_004", "FM_Tat_F_004", 4100),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Skull on the Cross", "multiplayer_overlays", "FM_Tat_M_009", "FM_Tat_F_009", 9000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "LS Flames", "multiplayer_overlays", "FM_Tat_M_010", "FM_Tat_F_010", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack}, "LS Script", "multiplayer_overlays", "FM_Tat_M_011", "FM_Tat_F_011", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Los Santos Bills", "multiplayer_overlays", "FM_Tat_M_012", "FM_Tat_F_012", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightTopBack }, "Eagle and Serpent", "multiplayer_overlays", "FM_Tat_M_013", "FM_Tat_F_013", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Evil Clown", "multiplayer_overlays", "FM_Tat_M_016", "FM_Tat_F_016", 7750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "The Wages of Sin", "multiplayer_overlays", "FM_Tat_M_019", "FM_Tat_F_019", 7500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Dragon T", "multiplayer_overlays", "FM_Tat_M_020", "FM_Tat_F_020", 7000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest, TattooZones.Tummy, TattooZones.RightEdge }, "Flaming Cross", "multiplayer_overlays", "FM_Tat_M_024", "FM_Tat_F_024", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftChest }, "LS Bold", "multiplayer_overlays", "FM_Tat_M_025", "FM_Tat_F_025", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy, TattooZones.RightEdge }, "Trinity Knot", "multiplayer_overlays", "FM_Tat_M_029", "FM_Tat_F_029", 6100),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Lucky Celtic Dogs", "multiplayer_overlays", "FM_Tat_M_030", "FM_Tat_F_030", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest }, "Flaming Shamrock", "multiplayer_overlays", "FM_Tat_M_034", "FM_Tat_F_034", 1700),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.Tummy }, "Way of the Gun", "multiplayer_overlays", "FM_Tat_M_036", "FM_Tat_F_036", 20000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightChest, TattooZones.LeftChest }, "Stone Cross", "multiplayer_overlays", "FM_Tat_M_044", "FM_Tat_F_044", 19500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftBottomBack, TattooZones.RightBottomBack, TattooZones.LeftTopBack, TattooZones.RightTopBack }, "Skulls and Rose", "multiplayer_overlays", "FM_Tat_M_045", "FM_Tat_F_045", 7500),
            },

            // Head
            new List<BusinessTattoo>(){
	            // Передняя шея -   0
                // Левая шея    -   1
                // Правая шея   -   2
                // Задняя шея   -   3
	            // Левая щека - 4
                // Правая щека - 5

                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckFront }, "Cash is King", "mpbusiness_overlays", "MP_Buis_M_Neck_000", String.Empty, 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckLeft }, "Bold Dollar Sign", "mpbusiness_overlays", "MP_Buis_M_Neck_001", String.Empty, 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Script Dollar Sign", "mpbusiness_overlays", "MP_Buis_M_Neck_002", String.Empty, 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckBack }, "$100", "mpbusiness_overlays", "MP_Buis_M_Neck_003", String.Empty, 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckLeft }, "Val-de-Grace Logo", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Neck_000", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Money Rose", "mpbusiness_overlays", String.Empty, "MP_Buis_F_Neck_001", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Los Muertos", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_007", "MP_Xmas2_F_Tat_007", 62000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckLeft }, "Snake Head Color", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_025", "MP_Xmas2_F_Tat_025", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Beautiful Death", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_029", "MP_Xmas2_F_Tat_029", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckLeft }, "Lock & Load", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_003_M", "MP_Gunrunning_Tattoo_003_F", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Beautiful Eye", "mphipster_overlays", "FM_Hip_M_Tat_005", "FM_Hip_F_Tat_005", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckLeft }, "Geo Fox", "mphipster_overlays", "FM_Hip_M_Tat_021", "FM_Hip_F_Tat_021", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.CheekRight }, "Morbid Arachnid", "mpbiker_overlays", "MP_MP_Biker_Tat_009_M", "MP_MP_Biker_Tat_009_F", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "FTW", "mpbiker_overlays", "MP_MP_Biker_Tat_038_M", "MP_MP_Biker_Tat_038_F", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckLeft }, "Western Stylized", "mpbiker_overlays", "MP_MP_Biker_Tat_051_M", "MP_MP_Biker_Tat_051_F", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckLeft }, "Sinner", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_011_M", "MP_Smuggler_Tattoo_011_F", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Thief", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_012_M", "MP_Smuggler_Tattoo_012_F", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckLeft }, "Stunt Skull", "mpstunt_overlays", "MP_MP_Stunt_Tat_000_M", "MP_MP_Stunt_Tat_000_F", 6000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.CheekRight }, "Scorpion", "mpstunt_overlays", "MP_MP_Stunt_Tat_004_M", "MP_MP_Stunt_Tat_004_F", 1000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Toxic Spider", "mpstunt_overlays", "MP_MP_Stunt_Tat_006_M", "MP_MP_Stunt_Tat_006_F", 1000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Bat Wheel", "mpstunt_overlays", "MP_MP_Stunt_Tat_017_M", "MP_MP_Stunt_Tat_017_F", 1000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.NeckRight }, "Flaming Quad", "mpstunt_overlays", "MP_MP_Stunt_Tat_042_M", "MP_MP_Stunt_Tat_042_F", 6000),
            },

            // Left Arm
            new List<BusinessTattoo>()
            {
                // Кисть        -   0
                // До локтя     -   1
                // Выше локтя   -   2

                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "$100 Bill", "mpbusiness_overlays", "MP_Buis_M_LeftArm_000", String.Empty, 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "All-Seeing Eye", "mpbusiness_overlays", "MP_Buis_M_LeftArm_001", String.Empty, 4900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Greed is Good", "mpbusiness_overlays", String.Empty, "MP_Buis_F_LArm_000", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Skull Rider", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_000", "MP_Xmas2_F_Tat_000", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Electric Snake", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_010", "MP_Xmas2_F_Tat_010", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "8 Ball Skull", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_012", "MP_Xmas2_F_Tat_012", 13500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftWrist }, "Time's Up Outline", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_020", "MP_Xmas2_F_Tat_020", 8500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftWrist }, "Time's Up Color", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_021", "MP_Xmas2_F_Tat_021", 8500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftWrist }, "Sidearm", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_004_M", "MP_Gunrunning_Tattoo_004_F", 1550),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Bandolier", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_008_M", "MP_Gunrunning_Tattoo_008_F", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Spiked Skull", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_015_M", "MP_Gunrunning_Tattoo_015_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Blood Money", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_016_M", "MP_Gunrunning_Tattoo_016_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Praying Skull", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_025_M", "MP_Gunrunning_Tattoo_025_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Serpent Revolver", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_027_M", "MP_Gunrunning_Tattoo_027_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Diamond Sparkle", "mphipster_overlays", "FM_Hip_M_Tat_003", "FM_Hip_F_Tat_003", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftWrist }, "Bricks", "mphipster_overlays", "FM_Hip_M_Tat_007", "FM_Hip_F_Tat_007", 14500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Mustache", "mphipster_overlays", "FM_Hip_M_Tat_015", "FM_Hip_F_Tat_015", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Lightning Bolt", "mphipster_overlays", "FM_Hip_M_Tat_016", "FM_Hip_F_Tat_016", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Pizza", "mphipster_overlays", "FM_Hip_M_Tat_026", "FM_Hip_F_Tat_026", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Padlock", "mphipster_overlays", "FM_Hip_M_Tat_027", "FM_Hip_F_Tat_027", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Thorny Rose", "mphipster_overlays", "FM_Hip_M_Tat_028", "FM_Hip_F_Tat_028", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftWrist }, "Stop", "mphipster_overlays", "FM_Hip_M_Tat_034", "FM_Hip_F_Tat_034", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Sunrise", "mphipster_overlays", "FM_Hip_M_Tat_037", "FM_Hip_F_Tat_037", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Sleeve", "mphipster_overlays", "FM_Hip_M_Tat_039", "FM_Hip_F_Tat_039", 5500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Triangle White", "mphipster_overlays", "FM_Hip_M_Tat_043", "FM_Hip_F_Tat_043", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftWrist }, "Peace", "mphipster_overlays", "FM_Hip_M_Tat_048", "FM_Hip_F_Tat_048", 8500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Piston Sleeve", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_004_M", "MP_MP_ImportExport_Tat_004_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Scarlett", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_008_M", "MP_MP_ImportExport_Tat_008_F", 25750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "No Evil", "mplowrider_overlays", "MP_LR_Tat_005_M", "MP_LR_Tat_005_F", 13900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Los Santos Life", "mplowrider_overlays", "MP_LR_Tat_027_M", "MP_LR_Tat_027_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "City Sorrow", "mplowrider_overlays", "MP_LR_Tat_033_M", "MP_LR_Tat_033_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Toxic Trails", "mpairraces_overlays", "MP_Airraces_Tattoo_003_M", "MP_Airraces_Tattoo_003_F", 22700),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Urban Stunter", "mpbiker_overlays", "MP_MP_Biker_Tat_012_M", "MP_MP_Biker_Tat_012_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Macabre Tree", "mpbiker_overlays", "MP_MP_Biker_Tat_016_M", "MP_MP_Biker_Tat_016_F", 15000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Cranial Rose", "mpbiker_overlays", "MP_MP_Biker_Tat_020_M", "MP_MP_Biker_Tat_020_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Live to Ride", "mpbiker_overlays", "MP_MP_Biker_Tat_024_M", "MP_MP_Biker_Tat_024_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Good Luck", "mpbiker_overlays", "MP_MP_Biker_Tat_025_M", "MP_MP_Biker_Tat_025_F", 7500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Chain Fist", "mpbiker_overlays", "MP_MP_Biker_Tat_035_M", "MP_MP_Biker_Tat_035_F", 1500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Ride Hard Die Fast", "mpbiker_overlays", "MP_MP_Biker_Tat_045_M", "MP_MP_Biker_Tat_045_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Muffler Helmet", "mpbiker_overlays", "MP_MP_Biker_Tat_053_M", "MP_MP_Biker_Tat_053_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Poison Scorpion", "mpbiker_overlays", "MP_MP_Biker_Tat_055_M", "MP_MP_Biker_Tat_055_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Love Hustle", "mplowrider2_overlays", "MP_LR_Tat_006_M", "MP_LR_Tat_006_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Skeleton Party", "mplowrider2_overlays", "MP_LR_Tat_018_M", "MP_LR_Tat_018_F", 13700),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "My Crazy Life", "mplowrider2_overlays", "MP_LR_Tat_022_M", "MP_LR_Tat_022_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Archangel & Mary", "mpluxe_overlays", "MP_LUXE_TAT_020_M", "MP_LUXE_TAT_020_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){TattooZones.LeftForearm }, "Gabriel", "mpluxe_overlays", "MP_LUXE_TAT_021_M", "MP_LUXE_TAT_021_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){TattooZones.LeftForearm }, "Fatal Dagger", "mpluxe2_overlays", "MP_LUXE_TAT_005_M", "MP_LUXE_TAT_005_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){TattooZones.LeftForearm }, "Egyptian Mural", "mpluxe2_overlays", "MP_LUXE_TAT_016_M", "MP_LUXE_TAT_016_F", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Divine Goddess", "mpluxe2_overlays", "MP_LUXE_TAT_018_M", "MP_LUXE_TAT_018_F", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Python Skull", "mpluxe2_overlays", "MP_LUXE_TAT_028_M", "MP_LUXE_TAT_028_F", 4550),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Geometric Design LA", "mpluxe2_overlays", "MP_LUXE_TAT_031_M", "MP_LUXE_TAT_031_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Honor", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_004_M", "MP_Smuggler_Tattoo_004_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Horrors Of The Deep", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_008_M", "MP_Smuggler_Tattoo_008_F", 12250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Mermaid's Curse", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_014_M", "MP_Smuggler_Tattoo_014_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "8 Eyed Skull", "mpstunt_overlays", "MP_MP_Stunt_Tat_001_M", "MP_MP_Stunt_Tat_001_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftWrist }, "Big Cat", "mpstunt_overlays", "MP_MP_Stunt_Tat_002_M", "MP_MP_Stunt_Tat_002_F", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Moonlight Ride", "mpstunt_overlays", "MP_MP_Stunt_Tat_008_M", "MP_MP_Stunt_Tat_008_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Piston Head", "mpstunt_overlays", "MP_MP_Stunt_Tat_022_M", "MP_MP_Stunt_Tat_022_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Tanked", "mpstunt_overlays", "MP_MP_Stunt_Tat_023_M", "MP_MP_Stunt_Tat_023_F", 25750),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Stuntman's End", "mpstunt_overlays", "MP_MP_Stunt_Tat_035_M", "MP_MP_Stunt_Tat_035_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Kaboom", "mpstunt_overlays", "MP_MP_Stunt_Tat_039_M", "MP_MP_Stunt_Tat_039_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Engine Arm", "mpstunt_overlays", "MP_MP_Stunt_Tat_043_M", "MP_MP_Stunt_Tat_043_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm }, "Burning Heart", "multiplayer_overlays", "FM_Tat_Award_M_001", "FM_Tat_Award_F_001", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Racing Blonde", "multiplayer_overlays", "FM_Tat_Award_M_007", "FM_Tat_Award_F_007", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Racing Brunette", "multiplayer_overlays", "FM_Tat_Award_M_015", "FM_Tat_Award_F_015", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Serpents", "multiplayer_overlays", "FM_Tat_M_005", "FM_Tat_F_005", 12900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftForearm, TattooZones.LeftShoulder }, "Oriental Mural", "multiplayer_overlays", "FM_Tat_M_006", "FM_Tat_F_006", 9800),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Zodiac Skull", "multiplayer_overlays", "FM_Tat_M_015", "FM_Tat_F_015", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Lady M", "multiplayer_overlays", "FM_Tat_M_031", "FM_Tat_F_031", 12250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftShoulder }, "Dope Skull", "multiplayer_overlays", "FM_Tat_M_041", "FM_Tat_F_041", 13000),
            },
            
            // RightArm
            new List<BusinessTattoo>()
            {
                // Кисть        -   0
                // До локтя     -   1
                // Выше локтя   -   2

                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Dollar Skull", "mpbusiness_overlays", "MP_Buis_M_RightArm_000", String.Empty, 13900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Green", "mpbusiness_overlays", "MP_Buis_M_RightArm_001", String.Empty, 13900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Dollar Sign", "mpbusiness_overlays", String.Empty, "MP_Buis_F_RArm_000", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Snake Outline", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_003", "MP_Xmas2_F_Tat_003", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Snake Shaded", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_004", "MP_Xmas2_F_Tat_004", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Death Before Dishonor", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_008", "MP_Xmas2_F_Tat_008", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "You're Next Outline", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_022", "MP_Xmas2_F_Tat_022", 1500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "You're Next Color", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_023", "MP_Xmas2_F_Tat_023", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightWrist }, "Fuck Luck Outline", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_026", "MP_Xmas2_F_Tat_026", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightWrist }, "Fuck Luck Color", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_027", "MP_Xmas2_F_Tat_027", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightWrist }, "Grenade", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_002_M", "MP_Gunrunning_Tattoo_002_F", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Have a Nice Day", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_021_M", "MP_Gunrunning_Tattoo_021_F", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Combat Reaper", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_024_M", "MP_Gunrunning_Tattoo_024_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Single Arrow", "mphipster_overlays", "FM_Hip_M_Tat_001", "FM_Hip_F_Tat_001", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Bone", "mphipster_overlays", "FM_Hip_M_Tat_004", "FM_Hip_F_Tat_004", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Cube", "mphipster_overlays", "FM_Hip_M_Tat_008", "FM_Hip_F_Tat_008", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightWrist }, "Horseshoe", "mphipster_overlays", "FM_Hip_M_Tat_010", "FM_Hip_F_Tat_010", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Spray Can", "mphipster_overlays", "FM_Hip_M_Tat_014", "FM_Hip_F_Tat_014", 8000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Eye Triangle", "mphipster_overlays", "FM_Hip_M_Tat_017", "FM_Hip_F_Tat_017", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Origami", "mphipster_overlays", "FM_Hip_M_Tat_018", "FM_Hip_F_Tat_018", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Geo Pattern", "mphipster_overlays", "FM_Hip_M_Tat_020", "FM_Hip_F_Tat_020", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Pencil", "mphipster_overlays", "FM_Hip_M_Tat_022", "FM_Hip_F_Tat_022", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightWrist }, "Smiley", "mphipster_overlays", "FM_Hip_M_Tat_023", "FM_Hip_F_Tat_023", 8500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Shapes", "mphipster_overlays", "FM_Hip_M_Tat_036", "FM_Hip_F_Tat_036",13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Triangle Black", "mphipster_overlays", "FM_Hip_M_Tat_044", "FM_Hip_F_Tat_044",13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Mesh Band", "mphipster_overlays", "FM_Hip_M_Tat_045", "FM_Hip_F_Tat_045", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Mechanical Sleeve", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_003_M", "MP_MP_ImportExport_Tat_003_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Dialed In", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_005_M", "MP_MP_ImportExport_Tat_005_F", 4550),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Engulfed Block", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_006_M", "MP_MP_ImportExport_Tat_006_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Drive Forever", "mpimportexport_overlays", "MP_MP_ImportExport_Tat_007_M", "MP_MP_ImportExport_Tat_007_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Seductress", "mplowrider_overlays", "MP_LR_Tat_015_M", "MP_LR_Tat_015_F", 1580),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Swooping Eagle", "mpbiker_overlays", "MP_MP_Biker_Tat_007_M", "MP_MP_Biker_Tat_007_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Lady Mortality", "mpbiker_overlays", "MP_MP_Biker_Tat_014_M", "MP_MP_Biker_Tat_014_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Eagle Emblem", "mpbiker_overlays", "MP_MP_Biker_Tat_033_M", "MP_MP_Biker_Tat_033_F", 1580),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Grim Rider", "mpbiker_overlays", "MP_MP_Biker_Tat_042_M", "MP_MP_Biker_Tat_042_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Skull Chain", "mpbiker_overlays", "MP_MP_Biker_Tat_046_M", "MP_MP_Biker_Tat_046_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Snake Bike", "mpbiker_overlays", "MP_MP_Biker_Tat_047_M", "MP_MP_Biker_Tat_047_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "These Colors Don't Run", "mpbiker_overlays", "MP_MP_Biker_Tat_049_M", "MP_MP_Biker_Tat_049_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Mum", "mpbiker_overlays", "MP_MP_Biker_Tat_054_M", "MP_MP_Biker_Tat_054_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Lady Vamp", "mplowrider2_overlays", "MP_LR_Tat_003_M", "MP_LR_Tat_003_F", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Loving Los Muertos", "mplowrider2_overlays", "MP_LR_Tat_028_M", "MP_LR_Tat_028_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Black Tears", "mplowrider2_overlays", "MP_LR_Tat_035_M", "MP_LR_Tat_035_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Floral Raven", "mpluxe_overlays", "MP_LUXE_TAT_004_M", "MP_LUXE_TAT_004_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Mermaid Harpist", "mpluxe_overlays", "MP_LUXE_TAT_013_M", "MP_LUXE_TAT_013_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Geisha Bloom", "mpluxe_overlays", "MP_LUXE_TAT_019_M", "MP_LUXE_TAT_019_F", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Intrometric", "mpluxe2_overlays", "MP_LUXE_TAT_010_M", "MP_LUXE_TAT_010_F", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Heavenly Deity", "mpluxe2_overlays", "MP_LUXE_TAT_017_M", "MP_LUXE_TAT_017_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Floral Print", "mpluxe2_overlays", "MP_LUXE_TAT_026_M", "MP_LUXE_TAT_026_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Geometric Design RA", "mpluxe2_overlays", "MP_LUXE_TAT_030_M", "MP_LUXE_TAT_030_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Crackshot", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_001_M", "MP_Smuggler_Tattoo_001_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Mutiny", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_005_M", "MP_Smuggler_Tattoo_005_F", 1580),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Stylized Kraken", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_023_M", "MP_Smuggler_Tattoo_023_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Poison Wrench", "mpstunt_overlays", "MP_MP_Stunt_Tat_003_M", "MP_MP_Stunt_Tat_003_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Arachnid of Death", "mpstunt_overlays", "MP_MP_Stunt_Tat_009_M", "MP_MP_Stunt_Tat_009_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Grave Vulture", "mpstunt_overlays", "MP_MP_Stunt_Tat_010_M", "MP_MP_Stunt_Tat_010_F", 11900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Coffin Racer", "mpstunt_overlays", "MP_MP_Stunt_Tat_016_M", "MP_MP_Stunt_Tat_016_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightWrist }, "Biker Stallion", "mpstunt_overlays", "MP_MP_Stunt_Tat_036_M", "MP_MP_Stunt_Tat_036_F", 8250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "One Down Five Up", "mpstunt_overlays", "MP_MP_Stunt_Tat_038_M", "MP_MP_Stunt_Tat_038_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Seductive Mechanic", "mpstunt_overlays", "MP_MP_Stunt_Tat_049_M", "MP_MP_Stunt_Tat_049_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Grim Reaper Smoking Gun", "multiplayer_overlays", "FM_Tat_Award_M_002", "FM_Tat_Award_F_002", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Ride or Die RA", "multiplayer_overlays", "FM_Tat_Award_M_010", "FM_Tat_Award_F_010", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Brotherhood", "multiplayer_overlays", "FM_Tat_M_000", "FM_Tat_F_000", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Dragons", "multiplayer_overlays", "FM_Tat_M_001", "FM_Tat_F_001", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Dragons and Skull", "multiplayer_overlays", "FM_Tat_M_003", "FM_Tat_F_003", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder }, "Flower Mural", "multiplayer_overlays", "FM_Tat_M_014", "FM_Tat_F_014", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm, TattooZones.RightShoulder, TattooZones.RightWrist }, "Serpent Skull RA", "multiplayer_overlays", "FM_Tat_M_018", "FM_Tat_F_018", 5500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Virgin Mary", "multiplayer_overlays", "FM_Tat_M_027", "FM_Tat_F_027", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Mermaid", "multiplayer_overlays", "FM_Tat_M_028", "FM_Tat_F_028", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightForearm }, "Dagger", "multiplayer_overlays", "FM_Tat_M_038", "FM_Tat_F_038", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightShoulder }, "Lion", "multiplayer_overlays", "FM_Tat_M_047", "FM_Tat_F_047", 13000),
            },

            // LeftLeg
            new List<BusinessTattoo>()
            {
	            // До колена    -   0
                // Выше колена  -   1

                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Single", "mpbusiness_overlays", String.Empty, "MP_Buis_F_LLeg_000", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Spider Outline", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_001", "MP_Xmas2_F_Tat_001", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Spider Color", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_002", "MP_Xmas2_F_Tat_002", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Patriot Skull", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_005_M", "MP_Gunrunning_Tattoo_005_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftHip }, "Stylized Tiger", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_007_M", "MP_Gunrunning_Tattoo_007_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg, TattooZones.LeftHip }, "Death Skull", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_011_M", "MP_Gunrunning_Tattoo_011_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftHip }, "Rose Revolver", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_023_M", "MP_Gunrunning_Tattoo_023_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Squares", "mphipster_overlays", "FM_Hip_M_Tat_009", "FM_Hip_F_Tat_009", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Charm", "mphipster_overlays", "FM_Hip_M_Tat_019", "FM_Hip_F_Tat_019", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Black Anchor", "mphipster_overlays", "FM_Hip_M_Tat_040", "FM_Hip_F_Tat_040", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "LS Serpent", "mplowrider_overlays", "MP_LR_Tat_007_M", "MP_LR_Tat_007_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Presidents", "mplowrider_overlays", "MP_LR_Tat_020_M", "MP_LR_Tat_020_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Rose Tribute", "mpbiker_overlays", "MP_MP_Biker_Tat_002_M", "MP_MP_Biker_Tat_002_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Ride or Die LL", "mpbiker_overlays", "MP_MP_Biker_Tat_015_M", "MP_MP_Biker_Tat_015_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Bad Luck", "mpbiker_overlays", "MP_MP_Biker_Tat_027_M", "MP_MP_Biker_Tat_027_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Engulfed Skull", "mpbiker_overlays", "MP_MP_Biker_Tat_036_M", "MP_MP_Biker_Tat_036_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftHip }, "Scorched Soul", "mpbiker_overlays", "MP_MP_Biker_Tat_037_M", "MP_MP_Biker_Tat_037_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftHip }, "Ride Free", "mpbiker_overlays", "MP_MP_Biker_Tat_044_M", "MP_MP_Biker_Tat_044_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftHip }, "Bone Cruiser", "mpbiker_overlays", "MP_MP_Biker_Tat_056_M", "MP_MP_Biker_Tat_056_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg, TattooZones.LeftHip }, "Laughing Skull", "mpbiker_overlays", "MP_MP_Biker_Tat_057_M", "MP_MP_Biker_Tat_057_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Death Us Do Part", "mplowrider2_overlays", "MP_LR_Tat_029_M", "MP_LR_Tat_029_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Serpent of Death", "mpluxe_overlays", "MP_LUXE_TAT_000_M", "MP_LUXE_TAT_000_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Cross of Roses", "mpluxe2_overlays", "MP_LUXE_TAT_011_M", "MP_LUXE_TAT_011_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Dagger Devil", "mpstunt_overlays", "MP_MP_Stunt_Tat_007_M", "MP_MP_Stunt_Tat_007_F", 13900),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftHip }, "Dirt Track Hero", "mpstunt_overlays", "MP_MP_Stunt_Tat_013_M", "MP_MP_Stunt_Tat_013_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg, TattooZones.LeftHip }, "Golden Cobra", "mpstunt_overlays", "MP_MP_Stunt_Tat_021_M", "MP_MP_Stunt_Tat_021_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Quad Goblin", "mpstunt_overlays", "MP_MP_Stunt_Tat_028_M", "MP_MP_Stunt_Tat_028_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Stunt Jesus", "mpstunt_overlays", "MP_MP_Stunt_Tat_031_M", "MP_MP_Stunt_Tat_031_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Dragon and Dagger", "multiplayer_overlays", "FM_Tat_Award_M_009", "FM_Tat_Award_F_009", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Melting Skull", "multiplayer_overlays", "FM_Tat_M_002", "FM_Tat_F_002", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Dragon Mural", "multiplayer_overlays", "FM_Tat_M_008", "FM_Tat_F_008", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Serpent Skull LL", "multiplayer_overlays", "FM_Tat_M_021", "FM_Tat_F_021", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Hottie", "multiplayer_overlays", "FM_Tat_M_023", "FM_Tat_F_023", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Smoking Dagger", "multiplayer_overlays", "FM_Tat_M_026", "FM_Tat_F_026", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Faith LL", "multiplayer_overlays", "FM_Tat_M_032", "FM_Tat_F_032", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg, TattooZones.LeftHip }, "Chinese Dragon", "multiplayer_overlays", "FM_Tat_M_033", "FM_Tat_F_033", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Dragon LL", "multiplayer_overlays", "FM_Tat_M_035", "FM_Tat_F_035", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.LeftLeg }, "Grim Reaper", "multiplayer_overlays", "FM_Tat_M_037", "FM_Tat_F_037", 13250),
            },
            
            // RightLeg
            new List<BusinessTattoo>()
            {
	            // До колена    -   0
                // Выше колена  -   1

                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Diamond Crown", "mpbusiness_overlays", String.Empty, "MP_Buis_F_RLeg_000", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Floral Dagger", "mpchristmas2_overlays", "MP_Xmas2_M_Tat_014", "MP_Xmas2_F_Tat_014", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Combat Skull", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_006_M", "MP_Gunrunning_Tattoo_006_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Restless Skull", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_026_M", "MP_Gunrunning_Tattoo_026_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightHip }, "Pistol Ace", "mpgunrunning_overlays", "MP_Gunrunning_Tattoo_030_M", "MP_Gunrunning_Tattoo_030_F", 24850),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Grub", "mphipster_overlays", "FM_Hip_M_Tat_038", "FM_Hip_F_Tat_038", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Sparkplug", "mphipster_overlays", "FM_Hip_M_Tat_042", "FM_Hip_F_Tat_042", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Ink Me", "mplowrider_overlays", "MP_LR_Tat_017_M", "MP_LR_Tat_017_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Dance of Hearts", "mplowrider_overlays", "MP_LR_Tat_023_M", "MP_LR_Tat_023_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg, TattooZones.RightHip }, "Dragon's Fury", "mpbiker_overlays", "MP_MP_Biker_Tat_004_M", "MP_MP_Biker_Tat_004_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Western Insignia", "mpbiker_overlays", "MP_MP_Biker_Tat_022_M", "MP_MP_Biker_Tat_022_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightHip }, "Dusk Rider", "mpbiker_overlays", "MP_MP_Biker_Tat_028_M", "MP_MP_Biker_Tat_028_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightHip }, "American Made", "mpbiker_overlays", "MP_MP_Biker_Tat_040_M", "MP_MP_Biker_Tat_040_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "STFU", "mpbiker_overlays", "MP_MP_Biker_Tat_048_M", "MP_MP_Biker_Tat_048_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "San Andreas Prayer", "mplowrider2_overlays", "MP_LR_Tat_030_M", "MP_LR_Tat_030_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Elaborate Los Muertos", "mpluxe_overlays", "MP_LUXE_TAT_001_M", "MP_LUXE_TAT_001_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Starmetric", "mpluxe2_overlays", "MP_LUXE_TAT_023_M", "MP_LUXE_TAT_023_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg, TattooZones.RightHip }, "Homeward Bound", "mpsmuggler_overlays", "MP_Smuggler_Tattoo_020_M", "MP_Smuggler_Tattoo_020_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Demon Spark Plug", "mpstunt_overlays", "MP_MP_Stunt_Tat_005_M", "MP_MP_Stunt_Tat_005_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightHip }, "Praying Gloves", "mpstunt_overlays", "MP_MP_Stunt_Tat_015_M", "MP_MP_Stunt_Tat_015_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Piston Angel", "mpstunt_overlays", "MP_MP_Stunt_Tat_020_M", "MP_MP_Stunt_Tat_020_F", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightHip }, "Speed Freak", "mpstunt_overlays", "MP_MP_Stunt_Tat_025_M", "MP_MP_Stunt_Tat_025_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Wheelie Mouse", "mpstunt_overlays", "MP_MP_Stunt_Tat_032_M", "MP_MP_Stunt_Tat_032_F", 12000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg, TattooZones.RightHip }, "Severed Hand", "mpstunt_overlays", "MP_MP_Stunt_Tat_045_M", "MP_MP_Stunt_Tat_045_F", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Brake Knife", "mpstunt_overlays", "MP_MP_Stunt_Tat_047_M", "MP_MP_Stunt_Tat_047_F", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Skull and Sword", "multiplayer_overlays", "FM_Tat_Award_M_006", "FM_Tat_Award_F_006", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "The Warrior", "multiplayer_overlays", "FM_Tat_M_007", "FM_Tat_F_007", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Tribal", "multiplayer_overlays", "FM_Tat_M_017", "FM_Tat_F_017", 13000),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Fiery Dragon", "multiplayer_overlays", "FM_Tat_M_022", "FM_Tat_F_022", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Broken Skull", "multiplayer_overlays", "FM_Tat_M_039", "FM_Tat_F_039", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg, TattooZones.RightHip }, "Flaming Skull", "multiplayer_overlays", "FM_Tat_M_040", "FM_Tat_F_040", 4500),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Flaming Scorpion", "multiplayer_overlays", "FM_Tat_M_042", "FM_Tat_F_042", 13250),
                new BusinessTattoo(new List<TattooZones>(){ TattooZones.RightLeg }, "Indian Ram", "multiplayer_overlays", "FM_Tat_M_043", "FM_Tat_F_043", 13250)
            }

        };

        [ServerEvent(Event.ResourceStart)]
        public void InitTattooSalon()
        {
            _allTattoosConfig = new List<BusinessTattoo>();
            foreach (var range in BusinessTattoos)
            {
                _allTattoosConfig.AddRange(range);
            }

            CustomizationService.InitDoctor();           
        }

        [RemoteEvent("tattoo:close")]
        public static void RemoteEvent_cancelTattoo(ExtPlayer player)
        {
            try
            {
                Business biz = BizList[player.GetData<int>("BODY_SHOP")];
                SafeTrigger.UpdateDimension(player,  0);
                player.ChangePosition(biz.EnterPoint + new Vector3(0, 0, 1.12));
                player.Character.ExteriorPos = null;
                player.Character.Customization.Apply(player);
            }
            catch (Exception e) { _logger.WriteError("CancelBody: " + e.ToString()); }
        }

        [RemoteEvent("tattoo:remove:close")]
        public static void RemoteEvent_cancelTattooRemove(ExtPlayer player)
        {
            try
            {
                var character = player.Character;
                player.ChangePosition(character.ExteriorPos);
                character.ExteriorPos = null;
                SafeTrigger.UpdateDimension(player,  0);
                player.Character.Customization.Apply(player);
            }
            catch (Exception e) { _logger.WriteError("CancelBody: " + e.ToString()); }
        }

        [RemoteEvent("tattoo:remove:buy")]
        public static void RemoteEvent_buyTattoo(ExtPlayer player, int index)
        {
            try
            {
                var customization = player.Character.Customization;
                if(index < 0 || customization.Tattoos.Count > index)
                {
                    var tattoo = customization.Tattoos[index];
                    var config = GetTattooConfig(tattoo);
                    if(config == null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Не удалось удалить татуировку", 3000);
                        return;
                    }

                    if (player.Character.Money < config.Price)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Недостаточно денежных средств", 3000);
                        return;
                    }
                    Wallet.MoneySub(player.Character, config.Price, "Удаление тату");
                    customization.Tattoos.RemoveAt(index);
                    customization.Apply(player);
                    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Вы успешно удалили татуировку", 3000);
                    CustomizationService.UpdateTattoos(customization);
                }
                else Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Не удалось удалить татуировку", 3000);
                
            }
            catch (Exception e) { _logger.WriteError("BuyTattoo: " + e.ToString()); }
        }

        private static BusinessTattoo GetTattooConfig(Decoration tattoo)
        {
            return _allTattoosConfig.FirstOrDefault(t => t.Dictionary == tattoo.Collection && (t.FemaleHash == tattoo.Overlay || t.MaleHash == tattoo.Overlay));
        }

        [RemoteEvent("tattoo:buy")]
        public static void RemoteEvent_buyTattoo(ExtPlayer player, int zone, int tattooID)
        {
            try
            {
                var tattoo = BusinessTattoos[zone][tattooID];
                Business biz = BizList[player.GetData<int>("BODY_SHOP")];
                var priceModel = biz.GetProductPriceByProductId(0, tattoo.Price);

                TakeProd(player, biz, player.Character, new BuyModel("Tattoos", priceModel.MaterialsAmount, true, 
                    (cnt) =>
                    {
                        var tattooHash = player.GetGender() ? tattoo.MaleHash : tattoo.FemaleHash;

                        var tattoos = player.Character.Customization.Tattoos;
                        tattoos.Add(new Decoration { Collection = tattoo.Dictionary, Overlay = tattooHash });

                        var customization = player.Character.Customization;
                        customization.Apply(player);
                        Notify.Send(player, NotifyType.Info, NotifyPosition.BottomCenter, $"Вам набили татуировку {tattoo.Name} за {priceModel.Price}$", 3000);
                        CustomizationService.UpdateTattoos(customization);
                        return cnt;
                    }), "Оплата тату", null);

            }
            catch (Exception e) { _logger.WriteError("BuyTattoo: " + e.ToString()); }
        }
    }
}
