using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;

namespace Whistler.Inventory.Configs
{
    class SkinCostumeConfigs
    {
        private static Dictionary<CostumeNames, CostumeModel> SkinList = new Dictionary<CostumeNames, CostumeModel>();

        public static CostumeModel GetConfig(CostumeNames name)
        {
            return SkinList.GetValueOrDefault(name);
        }

        public static Dictionary<CostumeNames, CostumeModel> GetSkins()
        {
            return SkinList;
        }

        static SkinCostumeConfigs()
        {
            SkinList = new Dictionary<CostumeNames, CostumeModel>();

            SkinList.Add(CostumeNames.MBase, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>(), new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FBase, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>(), new Dictionary<CostumePropsSlots, CostumeElement>()));

            #region MALE
            SkinList.Add(CostumeNames.MFR1Gang, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(375, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(102, 14) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(77, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR1Getto, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(523, 2) },
                    { CostumeClothesSlots.Top, new CostumeElement(128, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(511, 2) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(8, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR2Gang, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(306, 10) },
                    { CostumeClothesSlots.Legs, new CostumeElement(117, 9) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(93, 8) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(13, 3) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR2Getto, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(523, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(334, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 8) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 2) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR3Gang, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(14, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(1, 9) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(7, 7) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR3Getto, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(524, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(14, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(512, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR4Getto, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(525, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(515, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 8) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR5Gang, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(79, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(117, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(77, 7) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR5Getto, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(523, 1) },
                    { CostumeClothesSlots.Top, new CostumeElement(281, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR6Office1, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(11, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(35, 0) },
                    { CostumeClothesSlots.Accessories , new CostumeElement(22, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Undershirts , new CostumeElement(158, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                }));
            //SkinList.Add(CostumeNames.MFR6Office2, new CostumeModel(true,
            //    new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(119, 11) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(25, 2) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR6Office3, new CostumeModel(true,
            //    new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Undershirts, new CostumeElement(0, 71) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(25, 2) },
            //        { CostumeClothesSlots.Top, new CostumeElement(28, 2) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR6Office4, new CostumeModel(true,
            //    new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(13, 0) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(54, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(10, 2) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>
            //    {
            //        { CostumePropsSlots.Glasses, new CostumeElement(1, 1) },
            //    }));
            //SkinList.Add(CostumeNames.MFR6Office5, new CostumeModel(true,
            //    new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(4, 0) },
            //        { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(10, 2) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>
            //    {
            //        { CostumePropsSlots.Glasses, new CostumeElement(1, 1) },
            //    }));
            //SkinList.Add(CostumeNames.MFR6Office6, new CostumeModel(true,
            //    new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(142, 0) },
            //        { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(10, 2) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>
            //    {
            //        { CostumePropsSlots.Glasses, new CostumeElement(1, 1) },
            //    }));
            //SkinList.Add(CostumeNames.MFR6Office7, new CostumeModel(true,
            //    new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Undershirts, new CostumeElement(4, 12) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(28, 4) },
            //        { CostumeClothesSlots.Top, new CostumeElement(32, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR6Office8, new CostumeModel(true,
            //    new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(28, 12) },
            //        { CostumeClothesSlots.Top, new CostumeElement(32, 1) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR6Office9, new CostumeModel(true,
            //    new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(28, 15) },
            //        { CostumeClothesSlots.Top, new CostumeElement(32, 2) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(25, 2) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR6Sport, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(533, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(508, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 18) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(18, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 6) }
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR6Security, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(349, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(35, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(17, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(153, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(22, 3) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Glasses, new CostumeElement(2, 0) },
                    { CostumePropsSlots.Hats, new CostumeElement(9, 0) },
                }));
            SkinList.Add(CostumeNames.MFR7Police1, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(504, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(153, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Police2, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(504, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(153, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Police3, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(504, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(153, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Police4, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(504, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(153, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Police5, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(504, 4) },
                    { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(129, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Police6, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(504, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(129, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Police7, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(504, 6) },
                    { CostumeClothesSlots.Legs, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(129, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Police8, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(505, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(130, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(74, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                }));
            SkinList.Add(CostumeNames.MFR7Police9, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(505, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(130, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(74, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                }));
            SkinList.Add(CostumeNames.MFR7Police10, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(505, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(130, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(74, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                }));
            SkinList.Add(CostumeNames.MFR7Police11, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(505, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(130, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(74, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                }));
            SkinList.Add(CostumeNames.MFR7Police12, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(505, 4) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(130, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(74, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                }));
            SkinList.Add(CostumeNames.MFR7Police13, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(505, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(60, 3) },
                    { CostumeClothesSlots.Bags, new CostumeElement(505, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(74, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Swat, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(111, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(130, 1) },
                    { CostumeClothesSlots.Bags, new CostumeElement(514, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(31, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(153, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(39, 0) },
                }));
            //SkinList.Add(CostumeNames.MFR7Sport, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //    { CostumeClothesSlots.Top, new CostumeElement(533, 4) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(508, 4) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(501, 13) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(22, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7Operative, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(29, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(35, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(31, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(75, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR7AF, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(220, 20) },
                    { CostumeClothesSlots.Legs, new CostumeElement(121, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(58, 6) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(172, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(19, 0) },
                    { CostumePropsSlots.Glasses, new CostumeElement(8, 0) },
                }));
            SkinList.Add(CostumeNames.MFR7SwatDaily, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(61, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(130, 3) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(16, 2) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(179, 0) },
                    //{ CostumeClothesSlots.Bags, new CostumeElement(514, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(150, 0) },
                }));
            SkinList.Add(CostumeNames.MFR8Ems1, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(542, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 1) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(126, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(86, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Decals, new CostumeElement(57, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(16, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFR8Ems2, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(542, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(126, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(86, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Decals, new CostumeElement(57, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(16, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFR8Paramedic1, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(249, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(86, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Decals, new CostumeElement(57, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(122, 0) },
                }, 12));
            SkinList.Add(CostumeNames.MFR8Paramedic2, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(249, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(86, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Decals, new CostumeElement(57, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(122, 1) },
                }, 12));
            //SkinList.Add(CostumeNames.MFR8Ems3, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(250, 0) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Ems4, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(249, 0) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(86, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Ems5, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(249, 0) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(86, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Ems6, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(250, 1) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(96, 1) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(126, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Ems7, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(234, 4) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(96, 1) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(126, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Ems8, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(89, 2) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(96, 1) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(126, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(82, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Ems9, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(349, 5) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(20, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(127, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(82, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Ems10, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(349, 5) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(20, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(127, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(82, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Ems11, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //    {
            //        { CostumeClothesSlots.Top, new CostumeElement(23, 3) },
            //        { CostumeClothesSlots.Legs, new CostumeElement(20, 0) },
            //        { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
            //        { CostumeClothesSlots.Accessories, new CostumeElement(127, 0) },
            //        { CostumeClothesSlots.Undershirts, new CostumeElement(13, 0) },
            //        { CostumeClothesSlots.Torsos, new CostumeElement(10, 0) },
            //    }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.MFR8Sport, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(533, 1) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(508, 1) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(501, 4) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(107, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR8Work, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                    { CostumeClothesSlots.Top, new CostumeElement(348, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(77, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(127, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR9Swat, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(319, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(33, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(26, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(147, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 2) },
                    { CostumeClothesSlots.Bags, new CostumeElement(514, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(129, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(117, 0) },
                }));
            //SkinList.Add(CostumeNames.MFR9Sport, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(533, 5) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(508, 5) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(501, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(55, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR9HRT, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(53, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(33, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(172, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(514, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(16, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(141, 0) },
                }));
            SkinList.Add(CostumeNames.MFR9Casual, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(53, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(33, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(172, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(514, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(16, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(143, 0) },
                    { CostumePropsSlots.Glasses, new CostumeElement(5, 0) },
                }));
            SkinList.Add(CostumeNames.MFR9AirForce, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(244, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(31, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(172, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(514, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(16, 2) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(0, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(115, 0) },
                }));
            SkinList.Add(CostumeNames.MFR10Gang, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(321, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(116, 8) },
                { CostumeClothesSlots.Shoes, new CostumeElement(1, 2) },
                { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR11Gang, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(192, 10) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(11, 120) },
                { CostumeClothesSlots.Shoes, new CostumeElement(45, 2) },
                { CostumeClothesSlots.Legs, new CostumeElement(22, 1) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR12Gang, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(10, 1) },
                { CostumeClothesSlots.Legs, new CostumeElement(24, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(77, 16) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR12GangLeader, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(311, 6) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
                { CostumeClothesSlots.Shoes, new CostumeElement(77, 8) },
                { CostumeClothesSlots.Legs, new CostumeElement(24, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(5, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>
            {
                { CostumePropsSlots.Glasses, new CostumeElement(5, 5) },
            }));
            SkinList.Add(CostumeNames.MFR13Gang, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(275, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(107, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(84, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(7, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR13GangLeader, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(275, 1) },
                { CostumeClothesSlots.Legs, new CostumeElement(107, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(84, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(7, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army1, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(208, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(88, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army2, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(220, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(86, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(63, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army3, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(220, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army4, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(222, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(86, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(63, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army5, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(222, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army6, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(221, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(86, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(63, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army7, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(221, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army8, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(219, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army9, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(222, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(86, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(63, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army10, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(232, 7) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(14, 121) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 14) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army11, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(228, 15) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 9) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 6) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army12, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(222, 14) },
                { CostumeClothesSlots.Legs, new CostumeElement(86, 14) },
                { CostumeClothesSlots.Shoes, new CostumeElement(63, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army13, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(221, 5) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 5) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army14, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(220, 4) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 4) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Army15, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(222, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(11, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR14Camo1, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(221, 5) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 5) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(141, 5) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFR14Camo2, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(222, 10) },
                { CostumeClothesSlots.Legs, new CostumeElement(87, 10) },
                { CostumeClothesSlots.Shoes, new CostumeElement(62, 2) },
                { CostumeClothesSlots.Torsos, new CostumeElement(158, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFR14Camo3, new CostumeModel(true, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(222, 15) },
                    { CostumeClothesSlots.Legs, new CostumeElement(87, 15) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(24, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(158, 15) },
                    { CostumeClothesSlots.Bags, new CostumeElement(103, 2) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(10, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFR14Camo4, new CostumeModel(true, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(87, 12) },
                    { CostumeClothesSlots.Top, new CostumeElement(222, 12) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(24, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(158, 12) },
                    { CostumeClothesSlots.Bags, new CostumeElement(103, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFR14Sport, new CostumeModel(true, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(533, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(508, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 14) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(53, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR15NewsLeader, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(20, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(24, 5) },
                { CostumeClothesSlots.Shoes, new CostumeElement(23, 14) },
                { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR15Sport, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(533, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(508, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(501, 17) },
                { CostumeClothesSlots.Torsos, new CostumeElement(53, 1) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR16Gang, new CostumeModel(true, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(518, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(74, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(24, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(29, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(129, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(3, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR17Mery, new CostumeModel(true, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(2, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(28, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR17GovCostume1, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(349, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(24, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(23, 9) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(154, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR17GovCostume2, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(349, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(35, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(23, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR17GovCostume3, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(349, 6) },
                    { CostumeClothesSlots.Legs, new CostumeElement(24, 3) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(23, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR17GovCostume4, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(31, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(24, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(33, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref1, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(26, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(35, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                { CostumeClothesSlots.Accessories, new CostumeElement(0, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref2, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(250, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(35, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                { CostumeClothesSlots.Accessories, new CostumeElement(0, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref3, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(250, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(35, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(10, 0) },
                { CostumeClothesSlots.Accessories, new CostumeElement(128, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref4, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(13, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Accessories, new CostumeElement(10, 2) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref5, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(20, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(20, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 9) },
                { CostumeClothesSlots.Accessories, new CostumeElement(26, 1) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(2, 12) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref6, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(13, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 2) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Accessories, new CostumeElement(37, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref7, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(28, 2) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 2) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Accessories, new CostumeElement(26, 1) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref8, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(102, 1) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 2) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Accessories, new CostumeElement(26, 3) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref9, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(4, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
                { CostumeClothesSlots.Accessories, new CostumeElement(27, 2) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref10, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(35, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Accessories, new CostumeElement(27, 4) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref11, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(27, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(5, 12) },
                { CostumeClothesSlots.Accessories, new CostumeElement(27, 1) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref12, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(28, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
                { CostumeClothesSlots.Accessories, new CostumeElement(26, 9) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Ref13, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(112, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(25, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 12) },
                { CostumeClothesSlots.Accessories, new CostumeElement(26, 5) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFR18Sport, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(533, 2) },
                { CostumeClothesSlots.Legs, new CostumeElement(508, 2) },
                { CostumeClothesSlots.Shoes, new CostumeElement(501, 18) },
                { CostumeClothesSlots.Torsos, new CostumeElement(53, 1) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFRCost1, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                    { CostumeClothesSlots.Top, new CostumeElement(120, 11) },
                    { CostumeClothesSlots.Legs, new CostumeElement(60, 11) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(22, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(158, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            SkinList.Add(CostumeNames.MFRCost2, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                    { CostumeClothesSlots.Top, new CostumeElement(120, 10) },
                    { CostumeClothesSlots.Legs, new CostumeElement(60, 10) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(22, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(158, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            SkinList.Add(CostumeNames.MFRCost3, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                    { CostumeClothesSlots.Top, new CostumeElement(120, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(60, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(22, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(158, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            SkinList.Add(CostumeNames.MFRCost4, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                    { CostumeClothesSlots.Top, new CostumeElement(120, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(60, 3) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(26, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(21, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(144, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(20, 3) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>
            {
                    { CostumePropsSlots.Ears, new CostumeElement(2, 0) },
                    { CostumePropsSlots.Wathes, new CostumeElement(22, 0) },
            }, 10));
            //SkinList.Add(CostumeNames.MFRCost5, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 4) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 4) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 5) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost6, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 5) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 5) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 4) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost7, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 6) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 6) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 2) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost8, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 7) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 7) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 14) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost9, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 8) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 8) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 13) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost10, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 9) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 9) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 11) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost11, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 10) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 10) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 10) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost12, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 11) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 11) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 5) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost13, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 12) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 12) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));
            //SkinList.Add(CostumeNames.MFRCost14, new CostumeModel(true, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(534, 13) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(34, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(509, 13) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(3, 10) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>(), 10));

            #endregion

            #region FEMALE
            SkinList.Add(CostumeNames.FFR1Gang, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(35, 6) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(30, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(61, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 6) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(5, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR1Getto, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(526, 1) },
                    { CostumeClothesSlots.Top, new CostumeElement(559, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 4) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR2Gang, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(317, 10) },
                    { CostumeClothesSlots.Legs, new CostumeElement(102, 20) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(79, 10) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR2Getto, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(526, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(559, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 8) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 2) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR3Gang, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(75, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(102, 5) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR3Getto, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(526, 3) },
                    { CostumeClothesSlots.Top, new CostumeElement(560, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(504, 2) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR4Getto, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(526, 4) },
                    { CostumeClothesSlots.Top, new CostumeElement(561, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(519, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 8) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(15, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR5Gang, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(149, 10) },
                    { CostumeClothesSlots.Legs, new CostumeElement(87, 9) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR5Getto, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(526, 2) },
                    { CostumeClothesSlots.Top, new CostumeElement(560, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(506, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(15, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR6Office1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(333, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Accessories , new CostumeElement(23, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                    { CostumeClothesSlots.Undershirts , new CostumeElement(189, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR6Office2, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(250, 2) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(36, 2) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR6Office3, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(250, 2) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(47, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR6Office4, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(250, 2) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(64, 1) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(29, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>
            //{
            //    { CostumePropsSlots.Glasses, new CostumeElement(0, 1) },
            //}));
            //SkinList.Add(CostumeNames.FFR6Office5, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(7, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(21, 2) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(6, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(29, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>
            //{
            //    { CostumePropsSlots.Glasses, new CostumeElement(0, 1) },
            //}));
            //SkinList.Add(CostumeNames.FFR6Office6, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(139, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(21, 2) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(6, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(29, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>
            //{
            //    { CostumePropsSlots.Glasses, new CostumeElement(0, 1) },
            //}));
            //SkinList.Add(CostumeNames.FFR6Office7, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(6, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(6, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(42, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(1, 20) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(12, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR6Office8, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(2, 26) },
            //    { CostumeClothesSlots.Top, new CostumeElement(6, 2) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(6, 2) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(42, 2) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR6Office9, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Legs, new CostumeElement(50, 0) },
            //    { CostumeClothesSlots.Top, new CostumeElement(7, 1) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(0, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(22, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR6Sport, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(510, 2) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(504, 2) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(503, 18) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(62, 1) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR6Security, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(367, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(37, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(18, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(189, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(23, 3) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Glasses, new CostumeElement(25, 0) },
                }));
            SkinList.Add(CostumeNames.FFR7Police1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(500, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(37, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(88, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(189, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police2, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(37, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(189, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(88, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police3, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(500, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(37, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(189, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(88, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police4, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(500, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(37, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(189, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(88, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police5, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(500, 4) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(159, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(88, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police6, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(500, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(159, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(88, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police7, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(501, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(505, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police8, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(501, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(505, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police9, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(501, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(505, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police10, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(501, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(505, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police11, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(501, 4) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(505, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police12, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(501, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(505, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Police13, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(501, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(505, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(85, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Swat, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(103, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(32, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(509, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(36, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(189, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(38, 0) },
                }));
            SkinList.Add(CostumeNames.FFR7Sport, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                { CostumeClothesSlots.Top, new CostumeElement(510, 4) },
                { CostumeClothesSlots.Legs, new CostumeElement(504, 4) },
                { CostumeClothesSlots.Shoes, new CostumeElement(503, 13) },
                { CostumeClothesSlots.Torsos, new CostumeElement(117, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Operative, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(11, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(36, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(67, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7AF, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(239, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(127, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(58, 6) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(36, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(19, 0) },
                    { CostumePropsSlots.Glasses, new CostumeElement(11, 0) },
                }));
            SkinList.Add(CostumeNames.FFR7SwatDaily, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(54, 1) },
                { CostumeClothesSlots.Legs, new CostumeElement(136, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(49, 0) },
                { CostumeClothesSlots.Bags, new CostumeElement(509, 0) },
                { CostumeClothesSlots.BodyArmor, new CostumeElement(18, 2) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>
            {
                { CostumePropsSlots.Hats, new CostumeElement(149, 0) },
            }));
            SkinList.Add(CostumeNames.FFR7Costume1, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(57, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(8, 1) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(57, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Costume2, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(305, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(27, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(57, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Costume3, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(7, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(8, 1) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(57, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Costume4, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(25, 7) },
                    { CostumeClothesSlots.Legs, new CostumeElement(27, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(57, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(27, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR7Costume5, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(25, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(27, 1) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(57, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 2) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR8Ems1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(549, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(99, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(101, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Decals, new CostumeElement(66, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(69, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR8Ems2, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(549, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(99, 1) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(96, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(101, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Decals, new CostumeElement(66, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(69, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR8Paramedic1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(257, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(99, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(101, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Decals, new CostumeElement(65, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR8Paramedic2, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(257, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(99, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(101, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Decals, new CostumeElement(65, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems3, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(258, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(99, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(96, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(109, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems4, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(257, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(99, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(96, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(101, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems5, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(257, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(99, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(96, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(101, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems6, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(258, 1) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(99, 1) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(96, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(106, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems7, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(9, 1) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(99, 1) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(96, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(9, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems8, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(364, 4) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(71, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(99, 1) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(96, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(9, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems9, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(332, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(97, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(9, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems10, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(332, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(97, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(9, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Ems11, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(305, 4) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(8, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(0, 71) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(1, 3) },
            //    { CostumeClothesSlots.Accessories, new CostumeElement(97, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFR8Sport, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(510, 1) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(504, 1) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(501, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(117, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR8Work, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                    { CostumeClothesSlots.Top, new CostumeElement(367, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(99, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(88, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(10, 1) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(97, 0) },
                { CostumeClothesSlots.BodyArmor, new CostumeElement(18, 2) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR9Swat, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(48, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(32, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(31, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(116, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(509, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(149, 0) },
                }));
            //SkinList.Add(CostumeNames.FFR9Sport, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(510, 5) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(504, 5) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(503, 0) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(62, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR9AirForce, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(252, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(30, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(218, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(509, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(18, 2) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(0, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(114, 0) },
                }));
            SkinList.Add(CostumeNames.FFR10Gang, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(8, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
                { CostumeClothesSlots.Legs, new CostumeElement(11, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(33, 2) },
                { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR11Gang, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(194, 6) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(11, 141) },
                { CostumeClothesSlots.Legs, new CostumeElement(45, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(46, 2) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR12Gang, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(315, 5) },
                { CostumeClothesSlots.Legs, new CostumeElement(37, 1) },
                { CostumeClothesSlots.Shoes, new CostumeElement(18, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR12GangLeader, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(315, 5) },
                { CostumeClothesSlots.Legs, new CostumeElement(11, 1) },
                { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR13Gang, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(288, 1) },
                { CostumeClothesSlots.Legs, new CostumeElement(114, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(88, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(6, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR13GangLeader, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(288, 1) },
                { CostumeClothesSlots.Legs, new CostumeElement(114, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(88, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(6, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army1, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(212, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(91, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army2, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(230, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(89, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(66, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army3, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(230, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army4, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(232, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(89, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(66, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army5, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(232, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army6, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(231, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(89, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(66, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army7, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(231, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army8, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(226, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(11, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army9, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(232, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(89, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(66, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army10, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(243, 7) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(141, 14) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 14) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army11, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(238, 15) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 9) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army12, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(232, 14) },
                { CostumeClothesSlots.Legs, new CostumeElement(89, 14) },
                { CostumeClothesSlots.Shoes, new CostumeElement(66, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army13, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(231, 5) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 5) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army14, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(230, 4) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 4) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Army15, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(232, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(90, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 6) },
                { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR14Camo1, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Legs, new CostumeElement(90, 5) },
                { CostumeClothesSlots.Top, new CostumeElement(224, 5) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>(), 13));
            SkinList.Add(CostumeNames.FFR14Camo2, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Legs, new CostumeElement(90, 10) },
                { CostumeClothesSlots.Top, new CostumeElement(224, 10) },
                { CostumeClothesSlots.Shoes, new CostumeElement(65, 2) },
                { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>(), 13));
            SkinList.Add(CostumeNames.FFR14Camo3, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(224, 15) },
                    { CostumeClothesSlots.Legs, new CostumeElement(90, 15) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(24, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(171, 15) },
                    { CostumeClothesSlots.Bags, new CostumeElement(98, 2) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(7, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 13));
            SkinList.Add(CostumeNames.FFR14Camo4, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(90, 12) },
                    { CostumeClothesSlots.Top, new CostumeElement(224, 12) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(24, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(7, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(171, 12) },
                    { CostumeClothesSlots.Bags, new CostumeElement(98, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 13));
            SkinList.Add(CostumeNames.FFR14Sport, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(510, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(504, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 18) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(62, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR15NewsLeader, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(24, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 20) },
                { CostumeClothesSlots.Legs, new CostumeElement(36, 2) },
                { CostumeClothesSlots.Shoes, new CostumeElement(14, 0) },
                { CostumeClothesSlots.Torsos, new CostumeElement(5, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR15Sport, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(510, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(504, 3) },
                { CostumeClothesSlots.Shoes, new CostumeElement(503, 17) },
                { CostumeClothesSlots.Torsos, new CostumeElement(12, 8) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR16Gang, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(262, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(102, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(9, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR17Mery, new CostumeModel(false, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(1, 51) },
                    { CostumeClothesSlots.Legs, new CostumeElement(8, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(13, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR17GovCostume1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(367, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(6, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(29, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(24, 9) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(190, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));

            SkinList.Add(CostumeNames.FFR17GovCostume2, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(367, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(6, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(24, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR17GovCostume3, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(367, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(6, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(24, 9) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(23, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR17GovCostume4, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(367, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(6, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(24, 9) },
                    { CostumeClothesSlots.Accessories, new CostumeElement(23, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref1, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(86, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(37, 1) },
                { CostumeClothesSlots.Shoes, new CostumeElement(64, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref2, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(86, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(37, 1) },
                { CostumeClothesSlots.Shoes, new CostumeElement(64, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref3, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(86, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(37, 1) },
                { CostumeClothesSlots.Shoes, new CostumeElement(64, 0) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref4, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(91, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(23, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(42, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref5, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(90, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(23, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(42, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref6, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(91, 1) },
                { CostumeClothesSlots.Legs, new CostumeElement(52, 1) },
                { CostumeClothesSlots.Shoes, new CostumeElement(42, 2) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref7, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(92, 1) },
                { CostumeClothesSlots.Legs, new CostumeElement(52, 1) },
                { CostumeClothesSlots.Shoes, new CostumeElement(42, 2) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref8, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(24, 7) },
                { CostumeClothesSlots.Legs, new CostumeElement(52, 1) },
                { CostumeClothesSlots.Shoes, new CostumeElement(42, 2) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref9, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(7, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(34, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref10, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(24, 3) },
                { CostumeClothesSlots.Legs, new CostumeElement(34, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 26) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref11, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(58, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(34, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 28) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref12, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(57, 0) },
                { CostumeClothesSlots.Legs, new CostumeElement(34, 0) },
                { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 13) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Ref13, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(25, 2) }, //топ
                { CostumeClothesSlots.Legs, new CostumeElement(34, 0) }, //штаны
                { CostumeClothesSlots.Shoes, new CostumeElement(6, 0) },
                { CostumeClothesSlots.Undershirts, new CostumeElement(0, 13) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFR18Sport, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            {
                { CostumeClothesSlots.Top, new CostumeElement(510, 2) },
                { CostumeClothesSlots.Legs, new CostumeElement(504, 2) },
                { CostumeClothesSlots.Shoes, new CostumeElement(503, 18) },
            }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFRCost1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(28, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(500, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 1) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(24, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFRCost2, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(28, 14) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 2) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 3) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(24, 3) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFRCost3, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(28, 7) },
                    { CostumeClothesSlots.Legs, new CostumeElement(36, 1) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 2) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(24, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFRCost4, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(8, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(7, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(25, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Undershirts, new CostumeElement(71, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Ears, new CostumeElement(2, 0) },
                }));
            //SkinList.Add(CostumeNames.FFRCost5, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 4) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 4) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 4) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost6, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 5) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 5) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 5) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost7, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 6) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 6) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 1) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost8, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 7) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 7) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 1) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost9, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 8) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 8) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 4) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost10, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 9) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 9) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 4) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost11, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 10) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 10) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 1) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost12, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 11) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 11) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 4) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost13, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 12) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 12) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 1) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));
            //SkinList.Add(CostumeNames.FFRCost14, new CostumeModel(false, new Dictionary<CostumeClothesSlots, CostumeElement>
            //{
            //    { CostumeClothesSlots.Top, new CostumeElement(512, 13) },
            //    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
            //    { CostumeClothesSlots.Undershirts, new CostumeElement(41, 0) },
            //    { CostumeClothesSlots.Legs, new CostumeElement(505, 13) },
            //    { CostumeClothesSlots.Shoes, new CostumeElement(22, 11) },
            //    { CostumeClothesSlots.BodyArmor, new CostumeElement(4, 0) },
            //}, new Dictionary<CostumePropsSlots, CostumeElement>()));


            #endregion

            #region Family MALE
            SkinList.Add(CostumeNames.MFAMSladeBMW, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(527, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(509, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(19, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(507, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMSladePunisher, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(49, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(34, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(24, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(507, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMTutashxia, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 11) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 7) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 17) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMModest, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 10) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 8) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(511, 5) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 8) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMGrubie, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 7) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 14) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 8) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMTareta, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 8) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 12) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMWensetti, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 6) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 13) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 18) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMAux, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(512, 7) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 15) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(506, 6) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 5) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMOyama, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(553, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(512, 17) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(75, 5) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFAMBosko, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(554, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(512, 18) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(75, 6) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFAMLacosta, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(512, 12) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(15, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(75, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMBillionaire, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(511, 11) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 6) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMLJT, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 12) },
                    { CostumeClothesSlots.Legs, new CostumeElement(511, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(8, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMCostello, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(544, 9) },
                    { CostumeClothesSlots.Legs, new CostumeElement(512, 16) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 18) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MFAMCrowd, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(553, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(512, 11) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 18) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMGroznensky, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(511, 13) },
                    { CostumeClothesSlots.Legs, new CostumeElement(516, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 2) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMEscobarov, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(511, 12) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 18) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMAzerMaf, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 13) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 7) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 17) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));

            SkinList.Add(CostumeNames.MFAMLega, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(543, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(518, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 2) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(6, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMGrozny, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(543, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(518, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(6, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMSniper, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(543, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(518, 3) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 3) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMXan, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(543, 4) },
                    { CostumeClothesSlots.Legs, new CostumeElement(518, 4) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMKhalid, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(543, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(518, 5) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(512, 5) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMArmenMafia, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(543, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(518, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(511, 7) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 3) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMDark, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(543, 6) },
                    { CostumeClothesSlots.Legs, new CostumeElement(518, 6) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMSoprano, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(543, 7) },
                    { CostumeClothesSlots.Legs, new CostumeElement(518, 7) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMMazyProject, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(528, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(552, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 2) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 8) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(4, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMMazeCostume1, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(533, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(561, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMMazeCostume2, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(533, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(563, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMMazeCostume3, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(533, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(562, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMMazeCostume4, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(532, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(562, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMMazeCostume5, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(533, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(563, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.MFAMMazeCostume6, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(532, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(561, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(28, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));

            #endregion

            #region Family FEMALE
            SkinList.Add(CostumeNames.FFAMTutashxia, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 8) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 13) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMModest, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 7) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 12) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 8) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMGrubie, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 9) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMTareta, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 4) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 11) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 6) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMWensetti, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 10) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 6) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(26, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMAux, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(528, 15) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 8) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 5) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(157, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 5) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMOyama, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(535, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(504, 16) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(18, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(79, 6) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMBosko, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(536, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(504, 15) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(18, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(79, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMLacosta, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(542, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(504, 21) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(18, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(79, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMBillionaire, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(510, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 14) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMLJT, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(514, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 9) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMCostello, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(527, 16) },
                    { CostumeClothesSlots.Legs, new CostumeElement(506, 5) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(153, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(2, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.FFAMCrowd, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(535, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(504, 8) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 18) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(18, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMGroznensky, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(510, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(506, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(24, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.FFAMEscobarov, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(510, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 15) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMAzerMaf, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(509, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(507, 13) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(13, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMLega, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(550, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(523, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(506, 5) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(50, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMGrozny, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(550, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(523, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(506, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(50, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 2) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMSniper, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(553, 1) },
                    { CostumeClothesSlots.Legs, new CostumeElement(524, 1) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(506, 2) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 3) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMXan, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(553, 2) },
                    { CostumeClothesSlots.Legs, new CostumeElement(524, 2) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(506, 5) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMKhalid, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(553, 3) },
                    { CostumeClothesSlots.Legs, new CostumeElement(524, 3) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(506, 2) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(47, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 4) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMArmenMafia, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(553, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(524, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(503, 10) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 3) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMDark, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(553, 4) },
                    { CostumeClothesSlots.Legs, new CostumeElement(524, 4) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(506, 5) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(6, 1) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMSoprano, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(553, 5) },
                    { CostumeClothesSlots.Legs, new CostumeElement(524, 5) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(501, 1) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(12, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMMazeCostume1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(535, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(572, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMMazeCostume2, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(534, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(563, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMMazeCostume3, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(535, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(563, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMMazeCostume4, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(535, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(573, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMMazeCostume5, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(534, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(573, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(14, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            SkinList.Add(CostumeNames.FFAMMazeCostume6, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Legs, new CostumeElement(534, 0) },
                    { CostumeClothesSlots.Top, new CostumeElement(572, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(502, 3) },
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(30, 4) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>()));
            #endregion

            #region Other
            SkinList.Add(CostumeNames.MRBForm1, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(57, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(48, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MRBForm2, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(154, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(47, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(66, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MRBForm3, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(168, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(4, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(1, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MRBForm4, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(193, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(19, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(7, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.MRBForm5, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(251, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(98, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(71, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));


            SkinList.Add(CostumeNames.FRBForm1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(49, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(0, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(2, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.FRBForm2, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(192, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(32, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            SkinList.Add(CostumeNames.FRBForm3, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(329, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(61, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(33, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>(), 12));
            #endregion

            #region Work Costume
            //Male
            SkinList.Add(CostumeNames.MWorkMail1, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(8, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(17, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(8, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Bags, new CostumeElement(44, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(76, 10) },
                }, 12));
            SkinList.Add(CostumeNames.MWorkFireFighter1, new CostumeModel(true,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(314, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(120, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(138, 0) },
                }, 12));

            //Family
            SkinList.Add(CostumeNames.FWorkMail1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(75, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(1, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(1, 1) },
                    { CostumeClothesSlots.Bags, new CostumeElement(44, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(75, 10) },
                }, 12));
            SkinList.Add(CostumeNames.FWorkFireFighter1, new CostumeModel(false,
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {
                    { CostumeClothesSlots.Top, new CostumeElement(325, 0) },
                    { CostumeClothesSlots.Legs, new CostumeElement(126, 0) },
                    { CostumeClothesSlots.Torsos, new CostumeElement(3, 0) },
                    { CostumeClothesSlots.Shoes, new CostumeElement(25, 0) },
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {
                    { CostumePropsSlots.Hats, new CostumeElement(137, 0) },
                }, 12));
            #endregion
        }
    }
}
