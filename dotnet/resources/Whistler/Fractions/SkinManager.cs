using System.Collections.Generic;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.SDK;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using System.Linq;
using Whistler.Helpers;
using System;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Models;
using Whistler.GUI;
using Whistler.Core;
using System.IO;
using System.Text;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class SkinManager : Script
    {
        /// <summary>
        /// Доступные комплекты одежды для каждого ранга каждой фракции
        /// </summary>
        private static readonly Dictionary<int, FractionSkins> _listFractionSkins = new Dictionary<int, FractionSkins>()
        {
            {
                1,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(
                        new List<CostumeNames>(){ CostumeNames.MFR1Gang, CostumeNames.MFR1Getto }, 
                        new List<CostumeNames>(){ CostumeNames.FFR1Gang, CostumeNames.FFR1Getto }))
            },
            {
                2,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(
                        new List<CostumeNames>(){ CostumeNames.MFR2Gang, CostumeNames.MFR2Getto }, 
                        new List<CostumeNames>(){ CostumeNames.FFR2Gang, CostumeNames.FFR2Getto }))
            },
            {
                3,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(
                        new List<CostumeNames>(){ CostumeNames.MFR3Gang, CostumeNames.MFR3Getto }, 
                        new List<CostumeNames>(){ CostumeNames.FFR3Gang, CostumeNames.FFR3Getto }))
            },
            {
                4,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(
                        new List<CostumeNames>(){ CostumeNames.MFR4Getto }, 
                        new List<CostumeNames>(){ CostumeNames.FFR4Getto }))
            },
            {
                5,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(
                        new List<CostumeNames>(){ CostumeNames.MFR5Gang, CostumeNames.MFR5Getto }, 
                        new List<CostumeNames>(){ CostumeNames.FFR5Gang, CostumeNames.FFR5Getto }))
            },
            {
                6,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>()
                    {
                    },
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR6Office1, CostumeNames.MFR6Security, CostumeNames.MFRCost1, CostumeNames.MFRCost2, CostumeNames.MFRCost3,CostumeNames.MFRCost4 },
                                   new List<CostumeNames>(){ CostumeNames.FFR6Office1, CostumeNames.FFR6Security, CostumeNames.FFRCost1, CostumeNames.FFRCost2, CostumeNames.FFRCost3,CostumeNames.FFRCost4 }))
            },
            {
                7,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>()
                    {
                        { 1,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police1 }, new List<CostumeNames>() { CostumeNames.FFR7Police1}) },
                        { 2,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police2 }, new List<CostumeNames>() { CostumeNames.FFR7Police2}) },
                        { 3,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police3 }, new List<CostumeNames>() { CostumeNames.FFR7Police3}) },
                        { 4,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police4 }, new List<CostumeNames>() { CostumeNames.FFR7Police4}) },
                        { 5,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police5 }, new List<CostumeNames>() { CostumeNames.FFR7Police5}) },
                        { 6,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police6 }, new List<CostumeNames>() { CostumeNames.FFR7Police6}) },
                        { 7,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police7 }, new List<CostumeNames>() { CostumeNames.FFR7Police7}) },
                        { 8,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police8 }, new List<CostumeNames>() { CostumeNames.FFR7Police8}) },
                        { 9,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police9 }, new List<CostumeNames>() { CostumeNames.FFR7Police9}) },
                        { 10, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police10 }, new List<CostumeNames>() { CostumeNames.FFR7Police10 }) },
                        { 11, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police11 }, new List<CostumeNames>() { CostumeNames.FFR7Police11 }) },
                        { 12, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police12 }, new List<CostumeNames>() { CostumeNames.FFR7Police12 }) },
                        { 13, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR7Police13 }, new List<CostumeNames>() { CostumeNames.FFR7Police13 }) },
                    },
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR7Swat, CostumeNames.MFR7Operative, CostumeNames.MFR7AF, CostumeNames.MFR7SwatDaily }, 
                                   new List<CostumeNames>(){ CostumeNames.FFR7Swat, CostumeNames.FFR7Operative, CostumeNames.FFR7AF, CostumeNames.FFR7SwatDaily, CostumeNames.FFR7Costume1, CostumeNames.FFR7Costume2, CostumeNames.FFR7Costume3, CostumeNames.FFR7Costume4, CostumeNames.FFR7Costume5 }))
            },
            {
                8,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR8Work },
                                   new List<CostumeNames>(){ CostumeNames.FFR8Work }))
            },
            {
                9,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>()
                    {
                    },
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR9Swat, CostumeNames.MFR9HRT, CostumeNames.MFR9Casual, CostumeNames.MFR9AirForce },
                                   new List<CostumeNames>(){ CostumeNames.FFR9Swat, CostumeNames.FFR9AirForce }))
            },
            {
                10,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR10Gang }, new List<CostumeNames>(){ CostumeNames.FFR10Gang }))
            },
            {
                11,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR11Gang }, new List<CostumeNames>(){ CostumeNames.FFR11Gang }))
            },
            {
                12,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>()
                    {
                        { 9,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR12GangLeader}, new List<CostumeNames>() { CostumeNames.FFR12GangLeader }) },
                        { 10, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR12GangLeader}, new List<CostumeNames>() { CostumeNames.FFR12GangLeader }) },
                    },
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR12Gang }, new List<CostumeNames>(){ CostumeNames.FFR12Gang }))
            },
            {
                13,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>()
                    {
                        { 10, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR13GangLeader}, new List<CostumeNames>() { CostumeNames.FFR13GangLeader }) },
                    },
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR13Gang }, new List<CostumeNames>(){ CostumeNames.FFR13Gang }))
            },
            {
                14,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>()
                    {
                        { 1,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army1 }, new List<CostumeNames>() { CostumeNames.FFR14Army1  }) },
                        { 2,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army2 }, new List<CostumeNames>() { CostumeNames.FFR14Army2  }) },
                        { 3,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army3 }, new List<CostumeNames>() { CostumeNames.FFR14Army3  }) },
                        { 4,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army4 }, new List<CostumeNames>() { CostumeNames.FFR14Army4  }) },
                        { 5,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army5 }, new List<CostumeNames>() { CostumeNames.FFR14Army5  }) },
                        { 6,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army6 }, new List<CostumeNames>() { CostumeNames.FFR14Army6  }) },
                        { 7,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army7 }, new List<CostumeNames>() { CostumeNames.FFR14Army7  }) },
                        { 8,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army8 }, new List<CostumeNames>() { CostumeNames.FFR14Army8  }) },
                        { 9,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army9 }, new List<CostumeNames>() { CostumeNames.FFR14Army9  }) },
                        { 10, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army10}, new List<CostumeNames>() { CostumeNames.FFR14Army10 }) },
                        { 11, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army11}, new List<CostumeNames>() { CostumeNames.FFR14Army11 }) },
                        { 12, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army12 }, new List<CostumeNames>() { CostumeNames.FFR14Army12 }) },
                        { 13, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army13 }, new List<CostumeNames>() { CostumeNames.FFR14Army13 }) },
                        { 14, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army14 }, new List<CostumeNames>() { CostumeNames.FFR14Army14 }) },
                        { 15, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army15 }, new List<CostumeNames>() { CostumeNames.FFR14Army15 }) },
                        { 16, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army15 }, new List<CostumeNames>() { CostumeNames.FFR14Army15 }) },
                        { 17, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army15 }, new List<CostumeNames>() { CostumeNames.FFR14Army15 }) },
                        { 18, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army15 }, new List<CostumeNames>() { CostumeNames.FFR14Army15 }) },
                        { 19, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army15 }, new List<CostumeNames>() { CostumeNames.FFR14Army15 }) },
                        { 20, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR14Army15 }, new List<CostumeNames>() { CostumeNames.FFR14Army15 }) },
                    },
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR14Camo1, CostumeNames.MFR14Camo2, CostumeNames.MFR14Camo3, CostumeNames.MFR14Camo4 },
                                   new List<CostumeNames>(){ CostumeNames.FFR14Camo1, CostumeNames.FFR14Camo2, CostumeNames.FFR14Camo3, CostumeNames.FFR14Camo4 }))
            },
            {
                15,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>()
                    {
                        { 17, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR15NewsLeader}, new List<CostumeNames>() { CostumeNames.FFR15NewsLeader }) },
                    },
                    new SkinConfig(new List<CostumeNames>(){}, new List<CostumeNames>(){}))
            },
            {
                16,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>(),
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFR16Gang }, new List<CostumeNames>(){ CostumeNames.FFR16Gang }))
            },
            {
                17,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>
                    {

                        { 1,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume1,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume1, }) },
                        { 2,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume1,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume1, }) },
                        { 3,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume1,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume1, }) },
                        { 4,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume2,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume2, }) },
                        { 5,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume2,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume2, }) },
                        { 6,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume2,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume2, }) },
                        { 7,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume3,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume3, }) },
                        { 8,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume3,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume3, }) },
                        { 9,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume3,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume3, }) },
                        { 10, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR17GovCostume4,  }, new List<CostumeNames>() { CostumeNames.FFR17GovCostume4, }) },
                    },
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFRCost1 }, new List<CostumeNames>(){ CostumeNames.FFRCost1 }))
            },
            {
                18,
                new FractionSkins(
                    new Dictionary<int, SkinConfig>()
                    {
                        { 1,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref1,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref1, }) },
                        { 2,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref2,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref2, }) },
                        { 3,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref3,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref3, }) },
                        { 4,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref4,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref4, }) },
                        { 5,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref5,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref5, }) },
                        { 6,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref6,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref6, }) },
                        { 7,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref7,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref7, }) },
                        { 8,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref8,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref8, }) },
                        { 9,  new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref9,  }, new List<CostumeNames>() { CostumeNames.FFR18Ref9, }) },
                        { 10, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref10, }, new List<CostumeNames>() { CostumeNames.FFR18Ref10, }) },
                        { 11, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref11, }, new List<CostumeNames>() { CostumeNames.FFR18Ref11, }) },
                        { 12, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref12, }, new List<CostumeNames>() { CostumeNames.FFR18Ref12, }) },
                        { 13, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFR18Ref13, }, new List<CostumeNames>() { CostumeNames.FFR18Ref13, }) },
                    },
                    new SkinConfig(new List<CostumeNames>(){ CostumeNames.MFRCost1, CostumeNames.MFRCost2, CostumeNames.MFRCost3,CostumeNames.MFRCost4 },
                                   new List<CostumeNames>(){ CostumeNames.FFRCost1, CostumeNames.FFRCost2, CostumeNames.FFRCost3,CostumeNames.FFRCost4 }))
            },
        };

        /// <summary>
        /// Одежда для семей
        /// </summary>
        private static Dictionary<int, SkinConfig> _familySkins = new Dictionary<int, SkinConfig>
        {
            { 1, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMSladeBMW, CostumeNames.MFAMSladePunisher, CostumeNames.MFRCost1, CostumeNames.MFRCost3, CostumeNames.MFAMTutashxia, CostumeNames.MFAMModest, CostumeNames.MFAMGrubie, CostumeNames.MFAMTareta, CostumeNames.MFAMWensetti, CostumeNames.MFAMAux, CostumeNames.MFAMOyama, CostumeNames.MFAMBosko, CostumeNames.MFAMLacosta, CostumeNames.MFAMBillionaire, CostumeNames.MFAMLJT, CostumeNames.MFAMCostello, CostumeNames.MFAMCrowd, CostumeNames.MFAMEscobarov, CostumeNames.MFAMAzerMaf, CostumeNames.MFAMGrozny, CostumeNames.MFAMGroznensky, CostumeNames.MFAMLega, CostumeNames.MFAMSniper, CostumeNames.MFAMXan, CostumeNames.MFAMKhalid, CostumeNames.MFAMArmenMafia, CostumeNames.MFAMDark, CostumeNames.MFAMSoprano, }, 
                                new List<CostumeNames>() {                                                            CostumeNames.FFRCost1, CostumeNames.FFRCost3, CostumeNames.FFAMTutashxia, CostumeNames.FFAMModest, CostumeNames.FFAMGrubie, CostumeNames.FFAMTareta, CostumeNames.FFAMWensetti, CostumeNames.FFAMAux, CostumeNames.FFAMOyama, CostumeNames.FFAMBosko, CostumeNames.FFAMLacosta, CostumeNames.FFAMBillionaire, CostumeNames.FFAMLJT, CostumeNames.FFAMCostello, CostumeNames.FFAMCrowd, CostumeNames.FFAMEscobarov, CostumeNames.FFAMAzerMaf, CostumeNames.FFAMGrozny, CostumeNames.FFAMGroznensky, CostumeNames.FFAMLega, CostumeNames.FFAMSniper, CostumeNames.FFAMXan, CostumeNames.FFAMKhalid, CostumeNames.FFAMArmenMafia, CostumeNames.FFAMDark, CostumeNames.FFAMSoprano, }) },
            { 2, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMTutashxia, }, new List<CostumeNames>() { CostumeNames.FFAMTutashxia, }) },
            { 8, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMDark, }, new List<CostumeNames>() { CostumeNames.FFAMDark, }) },
            { 9, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMGrubie, }, new List<CostumeNames>() { CostumeNames.FFAMGrubie, }) },
            { 10, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMAux, }, new List<CostumeNames>() { CostumeNames.FFAMAux, }) },
            { 15, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMWensetti, }, new List<CostumeNames>() { CostumeNames.FFAMWensetti, }) },
            { 20, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMTareta, }, new List<CostumeNames>() { CostumeNames.FFAMTareta, }) },
            { 21, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMBosko, }, new List<CostumeNames>() { CostumeNames.MFAMBosko, }) },
            { 22, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMOyama, }, new List<CostumeNames>() { CostumeNames.FFAMOyama, }) },
            { 27, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMLJT, }, new List<CostumeNames>() { CostumeNames.MFAMLJT, }) },
            { 29, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMBillionaire, }, new List<CostumeNames>() { CostumeNames.MFAMBillionaire, }) },
            { 32, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMLacosta, }, new List<CostumeNames>() { CostumeNames.FFAMLacosta, }) },
            { 34, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMModest, }, new List<CostumeNames>() { CostumeNames.FFAMModest, }) },
            { 39, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMMazeCostume1, CostumeNames.MFAMMazeCostume2, CostumeNames.MFAMMazeCostume3, CostumeNames.MFAMMazeCostume4, CostumeNames.MFAMMazeCostume5, CostumeNames.MFAMMazeCostume6 }, 
                                 new List<CostumeNames>() { CostumeNames.FFAMMazeCostume1, CostumeNames.FFAMMazeCostume2, CostumeNames.FFAMMazeCostume3, CostumeNames.FFAMMazeCostume4, CostumeNames.FFAMMazeCostume5, CostumeNames.FFAMMazeCostume6 }) },
            { 40, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMCostello, }, new List<CostumeNames>() { CostumeNames.FFAMCostello, }) },
            { 47, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMCrowd, }, new List<CostumeNames>() { CostumeNames.FFAMCrowd, }) },
            { 50, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMEscobarov, }, new List<CostumeNames>() { CostumeNames.FFAMEscobarov, }) },
            { 54, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMAzerMaf, }, new List<CostumeNames>() { CostumeNames.FFAMAzerMaf, }) },
            { 57, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMGrozny, }, new List<CostumeNames>() { CostumeNames.FFAMGrozny, }) },
            { 58, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMGroznensky, CostumeNames.MFAMXan, }, new List<CostumeNames>() { CostumeNames.FFAMGroznensky, CostumeNames.FFAMXan, }) },
            { 62, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMArmenMafia, }, new List<CostumeNames>() { CostumeNames.FFAMArmenMafia, }) },
            { 69, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMKhalid, }, new List<CostumeNames>() { CostumeNames.FFAMKhalid, }) },
            { 70, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMLega, }, new List<CostumeNames>() { CostumeNames.FFAMLega, }) },
            { 73, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMSniper, }, new List<CostumeNames>() { CostumeNames.FFAMSniper, }) },
            { 85, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMSoprano, }, new List<CostumeNames>() { CostumeNames.FFAMSoprano, }) },
            { 132, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMMazyProject }, new List<CostumeNames>() {  }) },
            { 133, new SkinConfig(new List<CostumeNames>() { CostumeNames.MFAMSladePunisher,  }, new List<CostumeNames>() {  }) },
        };

        private static int _familyClothesPrice = 10000;
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(SkinManager));

        public static void OpenSkinMenu(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                var fraction = Manager.GetFraction(player);
                int fracLvl = player.Character.FractionLVL;
                if (fraction == null || !_listFractionSkins.ContainsKey(fraction.Id))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_141", 3000);
                    return;
                }
                SafeTrigger.ClientEvent(player, "frac:changeclothesmenu", JsonConvert.SerializeObject(_listFractionSkins[fraction.Id].GetSkins(fracLvl, player.GetGender()).Select(item => item.ToString())), (int)fraction.OrgActiveType);
            }
            catch (Exception e)
            {
                _logger.WriteError($"OpenSkinMenu:{e}");
            }
        }

        public static void OpenFamilySkinMenu(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                int family = player.Character.FamilyID;
                if (!_familySkins.ContainsKey(family))
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "local_141", 3000);
                    return;
                }
                SafeTrigger.ClientEvent(player, "family:changeclothesmenu", JsonConvert.SerializeObject(_familySkins[family].GetSkins(player.GetGender()).Select(item => item.ToString())));
            }
            catch (Exception e)
            {
                _logger.WriteError($"OpenSkinMenu:{e}");
            }
        }

        [RemoteEvent("frac:applyskin")]
        public static void ApplySkin(ExtPlayer player, string skinName, int type)
        {

            if (!player.IsLogged())
                return;
            if (skinName != "cancel")
            {
                switch (type)
                {
                    case 0:
                        ApplyFractionSkin(player, skinName);
                        break;
                    case 1:
                        ApplyFamilySkinSkin(player, skinName);
                        break;
                }
            }
        }

        private static void ApplyFractionSkin(ExtPlayer player, string skinName)
        {
            //Проверяем фракционную принадлежность 
            int fracID = player.Character.FractionID;
            int fracLvl = player.Character.FractionLVL;
            if (Enum.TryParse(skinName, out CostumeNames skin) && _listFractionSkins.ContainsKey(fracID) && _listFractionSkins[fracID].GetSkins(fracLvl, player.GetGender()).Contains(skin))
            {
                player.Character.OnDuty = true;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_520", 3000);
                GiveClothes(player, skin, ClothesOwn.Fraction);
            }
            if (skinName == "WorkInMyDress")
            {
                player.Character.OnDuty = true;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_520", 3000);
            }
            else if (skinName == "DressDefault")
            {
                player.Character.OnDuty = false;
                Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_521", 3000);
            }
        }
        private static void ApplyFamilySkinSkin(ExtPlayer player, string skinName)
        {
            DialogUI.Open(player, "familyMenu_109".Translate(_familyClothesPrice), new List<DialogUI.ButtonSetting>
            {
                new DialogUI.ButtonSetting
                {
                    Name = "dialog_0",
                    Icon = null,
                    Action = (p) =>
                    {
                        int family = p.Character.FamilyID;
                        if (Enum.TryParse(skinName, out CostumeNames skin) && _familySkins.ContainsKey(family) && _familySkins[family].GetSkins(player.GetGender()).Contains(skin))
                        {
                            GiveClothes(p, skin, ClothesOwn.Family, _familyClothesPrice);
                        }
                    }
                },
                new DialogUI.ButtonSetting
                {
                    Name = "dialog_1",
                    Icon = null,
                    Action = { }
                }
            });           
        }

        [RemoteEvent("frac:setskin")]
        public static void TryOnClothes(ExtPlayer player, string skinName)
        {
            try
            {
                if (!player.IsLogged())
                    return;

                if (Enum.TryParse(skinName, out CostumeNames skin) && SkinCostumeConfigs.GetConfig(skin) != null)
                {
                    CostumeModel skinConfig = SkinCostumeConfigs.GetConfig(skin);
                    SafeTrigger.ClientEvent(player,"frac:tryonclothes",
                        JsonConvert.SerializeObject(skinConfig.Clothes.ToDictionary(item => (int)item.Key, item => item.Value)),
                        JsonConvert.SerializeObject(skinConfig.Props.ToDictionary(item => (int)item.Key, item => item.Value)),
                        skinConfig.Gender);
                }
                else
                    player.GetEquip().Update(false);
            }
            catch (Exception e)
            {
                _logger.WriteError($"TryOnClothes:{e}");
            }
        }
        public static void GiveClothes(ExtPlayer player, CostumeNames skin, ClothesOwn ownerCloth, int money = 0)
        {
            try
            {
                if (!player.IsLogged())
                    return;
                bool gender = player.GetGender();
                var inventory = player.GetInventory();
                CostumeModel skinModel = SkinCostumeConfigs.GetConfig(skin);
                if (skinModel != null)
                {
                    var item = ItemsFabric.CreateCostume(ItemNames.StandartCostume, skin, ownerCloth, skinModel.Gender, true);
                    if (item == null)
                        return;
                    if (money > 0 && !Whistler.MoneySystem.Wallet.MoneySub(player.Character, money, "Money_BuyFamCloth"))
                    {
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_141", 3000);
                        return;
                    }    
                    if (inventory.AddItem(item))
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_536", 3000);
                    else
                        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_537", 3000);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"GiveClothes:{e}");
            }
        }
        public static void TakePlayerCostumes(int playerUUID, ClothesOwn ownerCloth)
        {
            try
            {
                InventoryModel inventory = null;
                Equip equip = null;
                var player = Trigger.GetPlayerByUuid(playerUUID);
                if (player != null)
                {
                    inventory = player.GetInventory();
                    equip = player.GetEquip();
                }
                else
                {
                    var result = MySQL.QueryRead("SELECT `equipId`, `inventoryId` FROM `characters` WHERE `uuid` = @prop0", playerUUID);
                    if (result != null && result.Rows.Count > 0)
                    {
                        var row = result.Rows[0];
                        var inventoryId = Convert.ToInt32(row["inventoryId"]);
                        var equipId = Convert.ToInt32(row["equipId"]);
                        inventory = InventoryService.GetById(inventoryId);
                        equip = EquipService.GetById(equipId);
                    }
                }
                if (inventory != null)
                {
                    inventory.RemoveItems(item => item.Type == Inventory.Enums.ItemTypes.Costume && (item as Costume).CostumeOwner == ownerCloth);
                }
                if (equip != null)
                {
                    equip.RemoveClothes(item => item.Key == ClothesSlots.Costume && (item.Value as Costume).CostumeOwner == ownerCloth);
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"TakePlayerCostumes:{e}");
            }
        }

        [Command("costumemenu")]
        public static void OpenCostumeMenu(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "costumemenu")) return;
            SafeTrigger.ClientEvent(player,"costumeMenu:open");
        }

        [RemoteEvent("costumeMenu:saveServer")]
        public static void OpenCostumeMenu(ExtPlayer player, string result)
        {
            if (!Group.CanUseAdminCommand(player, "costumemenu")) return;
            StreamWriter saveCoords = new StreamWriter("costumes.txt", true, Encoding.UTF8);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            saveCoords.Write($"{result}      \r\n");
            saveCoords.Close();
        }

    }


    class SkinConfig
    {
        public List<CostumeNames> MaleSkins { get; }
        public List<CostumeNames> FemaleSkins { get; }
        public SkinConfig(List<CostumeNames> maleSkins, List<CostumeNames> femaleSkins)
        {
            MaleSkins = maleSkins;
            FemaleSkins = femaleSkins;
        }

        public List<CostumeNames> GetSkins(bool gender)
        {
            if (gender)
                return MaleSkins;
            else
                return FemaleSkins;
        }
    }

    class FractionSkins
    {
        public Dictionary<int, SkinConfig> LevelSkins;
        public SkinConfig CommonSkins;
        public FractionSkins(Dictionary<int, SkinConfig> levelSkins, SkinConfig commonSkins)
        {
            LevelSkins = levelSkins;
            CommonSkins = commonSkins;
        }
        public List<CostumeNames> GetSkins(int rank, bool gender)
        {
            List<CostumeNames> skins = CommonSkins.GetSkins(gender).ToList();
            if (LevelSkins.ContainsKey(rank))
            {
                skins.AddRange(LevelSkins[rank].GetSkins(gender).ToList());
            }
            return skins;
        }
    }
}
