using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using GTANetworkAPI;
using Whistler.Core;
using System.Linq;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Businesses
{
    
   
    class TuningParser: Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(TuningParser));

        public static Dictionary<int, ColorCust> StandartColors = new Dictionary<int, ColorCust>()
                {
                    {0,  new ColorCust(0, 13, 17, 22)},
                    {1,  new ColorCust(1, 28, 29, 33)},
                    {2,  new ColorCust(2, 50, 56, 61)},
                    {3,  new ColorCust(3, 69, 75, 79)},
                    {4,  new ColorCust(4, 153, 157, 160)},
                    {5,  new ColorCust(5, 194, 196, 198)},
                    {6,  new ColorCust(6, 151, 154, 151)},
                    {7,  new ColorCust(7, 99, 115, 128)},
                    {8,  new ColorCust(8, 99, 98, 92)},
                    {9,  new ColorCust(9, 60, 63, 71)},
                    {10, new ColorCust(10, 68, 78, 84)},
                    {11, new ColorCust(11, 29, 33, 41)},
                    {12, new ColorCust(12, 19, 24, 31)},
                    {13, new ColorCust(13, 38, 40, 42)},
                    {14, new ColorCust(14, 81, 85, 84)},
                    {15, new ColorCust(15, 21, 25, 33)},
                    {16, new ColorCust(16, 30, 36, 41)},
                    {17, new ColorCust(17, 51, 58, 60)},
                    {18, new ColorCust(18, 140, 144, 149)},
                    {19, new ColorCust(19, 57, 67, 77)},
                    {20, new ColorCust(20, 80, 98, 114)},
                    {21, new ColorCust(21, 30, 35, 47)},
                    {22, new ColorCust(22, 54, 58, 63)},
                    {23, new ColorCust(23, 160, 161, 153)},
                    {24, new ColorCust(24, 211, 211, 211)},
                    {25, new ColorCust(25, 183, 191, 202)},
                    {26, new ColorCust(26, 119, 135, 148)},
                    {27, new ColorCust(27, 192, 14, 26)},
                    {28, new ColorCust(28, 218, 25, 24)},
                    {29, new ColorCust(29, 182, 17, 27)},
                    {30, new ColorCust(30, 165, 30, 35)},
                    {31, new ColorCust(31, 123, 26, 34)},
                    {32, new ColorCust(32, 142, 27, 31)},
                    {33, new ColorCust(33, 111, 24, 24)},
                    {34, new ColorCust(34, 73, 17, 29)},
                    {35, new ColorCust(35, 182, 15, 37)},
                    {36, new ColorCust(36, 212, 74, 23)},
                    {37, new ColorCust(37, 194, 148, 79)},
                    {38, new ColorCust(38, 247, 134, 22)},
                    {39, new ColorCust(39, 207, 31, 33)},
                    {40, new ColorCust(40, 115, 32, 33)},
                    {41, new ColorCust(41, 242, 125, 32)},
                    {42, new ColorCust(42, 255, 201, 31)},
                    {43, new ColorCust(43, 156, 16, 22)},
                    {44, new ColorCust(44, 222, 15, 24)},
                    {45, new ColorCust(45, 143, 30, 23)},
                    {46, new ColorCust(46, 169, 71, 68)},
                    {47, new ColorCust(47, 177, 108, 81)},
                    {48, new ColorCust(48, 55, 28, 37)},
                    {49, new ColorCust(49, 19, 36, 40)},
                    {50, new ColorCust(50, 18, 46, 43)},
                    {51, new ColorCust(51, 18, 56, 60)},
                    {52, new ColorCust(52, 49, 66, 63)},
                    {53, new ColorCust(53, 21, 92, 45)},
                    {54, new ColorCust(54, 27, 103, 112)},
                    {55, new ColorCust(55, 102, 184, 31)},
                    {56, new ColorCust(56, 34, 56, 62)},
                    {57, new ColorCust(57, 29, 90, 63)},
                    {58, new ColorCust(58, 45, 66, 63)},
                    {59, new ColorCust(59, 69, 89, 75)},
                    {60, new ColorCust(60, 101, 134, 127)},
                    {61, new ColorCust(61, 34, 46, 70)},
                    {62, new ColorCust(62, 35, 49, 85)},
                    {63, new ColorCust(63, 48, 76, 126)},
                    {64, new ColorCust(64, 71, 87, 143)},
                    {65, new ColorCust(65, 99, 123, 167)},
                    {66, new ColorCust(66, 57, 71, 98)},
                    {67, new ColorCust(67, 214, 231, 241)},
                    {68, new ColorCust(68, 118, 175, 190)},
                    {69, new ColorCust(69, 52, 94, 114)},
                    {70, new ColorCust(70, 11, 156, 241)},
                    {71, new ColorCust(71, 47, 45, 82)},
                    {72, new ColorCust(72, 40, 44, 77)},
                    {73, new ColorCust(73, 35, 84, 161)},
                    {74, new ColorCust(74, 110, 163, 198)},
                    {75, new ColorCust(75, 17, 37, 82)},
                    {76, new ColorCust(76, 27, 32, 62)},
                    {77, new ColorCust(77, 39, 81, 144)},
                    {78, new ColorCust(78, 96, 133, 146)},
                    {79, new ColorCust(79, 36, 70, 168)},
                    {80, new ColorCust(80, 66, 113, 225)},
                    {81, new ColorCust(81, 59, 57, 224)},
                    {82, new ColorCust(82, 31, 40, 82)},
                    {83, new ColorCust(83, 37, 58, 167)},
                    {84, new ColorCust(84, 28, 53, 81)},
                    {85, new ColorCust(85, 76, 95, 129)},
                    {86, new ColorCust(86, 88, 104, 142)},
                    {87, new ColorCust(87, 116, 181, 216)},
                    {88, new ColorCust(88, 255, 207, 32)},
                    {89, new ColorCust(89, 251, 226, 18)},
                    {90, new ColorCust(90, 145, 101, 50)},
                    {91, new ColorCust(91, 224, 225, 61)},
                    {92, new ColorCust(92, 152, 210, 35)},
                    {93, new ColorCust(93, 155, 140, 120)},
                    {94, new ColorCust(94, 80, 50, 24)},
                    {95, new ColorCust(95, 71, 63, 43)},
                    {96, new ColorCust(96, 34, 27, 25)},
                    {97, new ColorCust(97, 101, 63, 35)},
                    {98, new ColorCust(98, 119, 92, 62)},
                    {99, new ColorCust(99, 172, 153, 117)},
                    {100, new ColorCust(100, 108, 107, 75)},
                    {101, new ColorCust(101, 64, 46, 43)},
                    {102, new ColorCust(102, 164, 150, 95)},
                    {103, new ColorCust(103, 70, 35, 26)},
                    {104, new ColorCust(104, 117, 43, 25)},
                    {105, new ColorCust(105, 191, 174, 123)},
                    {106, new ColorCust(106, 223, 213, 178)},
                    {107, new ColorCust(107, 247, 237, 213)},
                    {108, new ColorCust(108, 58, 42, 27)},
                    {109, new ColorCust(109, 120, 95, 51)},
                    {110, new ColorCust(110, 181, 160, 121)},
                    {111, new ColorCust(111, 255, 255, 246)},
                    {112, new ColorCust(112, 234, 234, 234)},
                    {113, new ColorCust(113, 176, 171, 148)},
                    {114, new ColorCust(114, 69, 56, 49)},
                    {115, new ColorCust(115, 42, 40, 43)},
                    {116, new ColorCust(116, 114, 108, 87)},
                    {117, new ColorCust(117, 106, 116, 124)},
                    {118, new ColorCust(118, 53, 65, 88)},
                    {119, new ColorCust(119, 155, 160, 168)},
                    {120, new ColorCust(120, 88, 112, 161)},
                    {121, new ColorCust(121, 234, 230, 222)},
                    {122, new ColorCust(122, 223, 221, 208)},
                    {123, new ColorCust(123, 242, 173, 46)},
                    {124, new ColorCust(124, 249, 164, 88)},
                    {125, new ColorCust(125, 131, 197, 102)},
                    {126, new ColorCust(126, 241, 204, 64)},
                    {127, new ColorCust(127, 76, 195, 218)},
                    {128, new ColorCust(128, 78, 100, 67)},
                    {129, new ColorCust(129, 188, 172, 143)},
                    {130, new ColorCust(130, 248, 182, 88)},
                    {131, new ColorCust(131, 252, 249, 241)},
                    {132, new ColorCust(132, 255, 255, 251)},
                    {133, new ColorCust(133, 129, 132, 76)},
                    {134, new ColorCust(134, 255, 255, 255)},
                    {135, new ColorCust(135, 242, 31, 153)},
                    {136, new ColorCust(136, 253, 214, 205)},
                    {137, new ColorCust(137, 223, 88, 145)},
                    {138, new ColorCust(138, 246, 174, 32)},
                    {139, new ColorCust(139, 176, 238, 110)},
                    {140, new ColorCust(140, 8, 233, 250)},
                    {141, new ColorCust(141, 10, 12, 23)},
                    {142, new ColorCust(142, 12, 13, 24)},
                    {143, new ColorCust(143, 14, 13, 20)},
                    {144, new ColorCust(144, 159, 158, 138)},
                    {145, new ColorCust(145, 98, 18, 118)},
                    {146, new ColorCust(146, 11, 20, 33)},
                    {147, new ColorCust(147, 17, 20, 26)},
                    {148, new ColorCust(148, 107, 31, 123)},
                    {149, new ColorCust(149, 30, 29, 34)},
                    {150, new ColorCust(150, 188, 25, 23)},
                    {151, new ColorCust(151, 45, 54, 42)},
                    {152, new ColorCust(152, 105, 103, 72)},
                    {153, new ColorCust(153, 122, 108, 85)},
                    {154, new ColorCust(154, 195, 180, 146)},
                    {155, new ColorCust(155, 90, 99, 82)},
                    {156, new ColorCust(156, 129, 130, 127)},
                    {157, new ColorCust(157, 175, 214, 228)},
                    {158, new ColorCust(158, 122, 100, 64)},
                    {159, new ColorCust(159, 127, 106, 72 )},
                    };
        public TuningParser()
        {
            try
            {
                if (Directory.Exists("client/configs"))
                {
                    using (var w = new StreamWriter("client/configs/tuning.js"))
                    {
                        w.WriteLine($"global.tuningPartsPrice = JSON.parse(`{JsonConvert.SerializeObject(BusinessManager.TuningParts)}`);");
                        w.WriteLine($"global.tuningWheels = JSON.parse(`{JsonConvert.SerializeObject(BusinessManager.TuningWheels)}`);");
                        w.WriteLine($"global.tuningColorPrice = JSON.parse(`{JsonConvert.SerializeObject(BusinessManager.PriceTypeColor)}`);");
                    }
                }    
            }
            catch (Exception e) { _logger.WriteError("lsCustomBuyTuning: " + e.ToString()); }
        }
        [Command("parsecolor")]
        public static void RemoteEvent_parsecolor(ExtPlayer player)
        {
            try
            {
                if (!Group.CanUseAdminCommand(player, "vehc")) return;
                List<ColorCust> list = new List<ColorCust>()
                {
                    { new ColorCust(0, 13, 17, 22)},
                    { new ColorCust(1, 28, 29, 33)},
                    { new ColorCust(2, 50, 56, 61)},
                    { new ColorCust(3, 69, 75, 79)},
                    { new ColorCust(4, 153, 157, 160)},
                    { new ColorCust(5, 194, 196, 198)},
                    { new ColorCust(6, 151, 154, 151)},
                    { new ColorCust(7, 99, 115, 128)},
                    { new ColorCust(8, 99, 98, 92)},
                    { new ColorCust(9, 60, 63, 71)},
                    { new ColorCust(10, 68, 78, 84)},
                    { new ColorCust(11, 29, 33, 41)},
                    { new ColorCust(12, 19, 24, 31)},
                    { new ColorCust(13, 38, 40, 42)},
                    { new ColorCust(14, 81, 85, 84)},
                    { new ColorCust(15, 21, 25, 33)},
                    { new ColorCust(16, 30, 36, 41)},
                    { new ColorCust(17, 51, 58, 60)},
                    { new ColorCust(18, 140, 144, 149)},
                    { new ColorCust(19, 57, 67, 77)},
                    { new ColorCust(20, 80, 98, 114)},
                    { new ColorCust(21, 30, 35, 47)},
                    { new ColorCust(22, 54, 58, 63)},
                    { new ColorCust(23, 160, 161, 153)},
                    { new ColorCust(24, 211, 211, 211)},
                    { new ColorCust(25, 183, 191, 202)},
                    { new ColorCust(26, 119, 135, 148)},
                    { new ColorCust(27, 192, 14, 26)},
                    { new ColorCust(28, 218, 25, 24)},
                    { new ColorCust(29, 182, 17, 27)},
                    { new ColorCust(30, 165, 30, 35)},
                    { new ColorCust(31, 123, 26, 34)},
                    { new ColorCust(32, 142, 27, 31)},
                    { new ColorCust(33, 111, 24, 24)},
                    { new ColorCust(34, 73, 17, 29)},
                    { new ColorCust(35, 182, 15, 37)},
                    { new ColorCust(36, 212, 74, 23)},
                    { new ColorCust(37, 194, 148, 79)},
                    { new ColorCust(38, 247, 134, 22)},
                    { new ColorCust(39, 207, 31, 33)},
                    { new ColorCust(40, 115, 32, 33)},
                    { new ColorCust(41, 242, 125, 32)},
                    { new ColorCust(42, 255, 201, 31)},
                    { new ColorCust(43, 156, 16, 22)},
                    { new ColorCust(44, 222, 15, 24)},
                    { new ColorCust(45, 143, 30, 23)},
                    { new ColorCust(46, 169, 71, 68)},
                    { new ColorCust(47, 177, 108, 81)},
                    { new ColorCust(48, 55, 28, 37)},
                    { new ColorCust(49, 19, 36, 40)},
                    { new ColorCust(50, 18, 46, 43)},
                    { new ColorCust(51, 18, 56, 60)},
                    { new ColorCust(52, 49, 66, 63)},
                    { new ColorCust(53, 21, 92, 45)},
                    { new ColorCust(54, 27, 103, 112)},
                    { new ColorCust(55, 102, 184, 31)},
                    { new ColorCust(56, 34, 56, 62)},
                    { new ColorCust(57, 29, 90, 63)},
                    { new ColorCust(58, 45, 66, 63)},
                    { new ColorCust(59, 69, 89, 75)},
                    { new ColorCust(60, 101, 134, 127)},
                    { new ColorCust(61, 34, 46, 70)},
                    { new ColorCust(62, 35, 49, 85)},
                    { new ColorCust(63, 48, 76, 126)},
                    { new ColorCust(64, 71, 87, 143)},
                    { new ColorCust(65, 99, 123, 167)},
                    { new ColorCust(66, 57, 71, 98)},
                    { new ColorCust(67, 214, 231, 241)},
                    { new ColorCust(68, 118, 175, 190)},
                    { new ColorCust(69, 52, 94, 114)},
                    { new ColorCust(70, 11, 156, 241)},
                    { new ColorCust(71, 47, 45, 82)},
                    { new ColorCust(72, 40, 44, 77)},
                    { new ColorCust(73, 35, 84, 161)},
                    { new ColorCust(74, 110, 163, 198)},
                    { new ColorCust(75, 17, 37, 82)},
                    { new ColorCust(76, 27, 32, 62)},
                    { new ColorCust(77, 39, 81, 144)},
                    { new ColorCust(78, 96, 133, 146)},
                    { new ColorCust(79, 36, 70, 168)},
                    { new ColorCust(80, 66, 113, 225)},
                    { new ColorCust(81, 59, 57, 224)},
                    { new ColorCust(82, 31, 40, 82)},
                    { new ColorCust(83, 37, 58, 167)},
                    { new ColorCust(84, 28, 53, 81)},
                    { new ColorCust(85, 76, 95, 129)},
                    { new ColorCust(86, 88, 104, 142)},
                    { new ColorCust(87, 116, 181, 216)},
                    { new ColorCust(88, 255, 207, 32)},
                    { new ColorCust(89, 251, 226, 18)},
                    { new ColorCust(90, 145, 101, 50)},
                    { new ColorCust(91, 224, 225, 61)},
                    { new ColorCust(92, 152, 210, 35)},
                    { new ColorCust(93, 155, 140, 120)},
                    { new ColorCust(94, 80, 50, 24)},
                    { new ColorCust(95, 71, 63, 43)},
                    { new ColorCust(96, 34, 27, 25)},
                    { new ColorCust(97, 101, 63, 35)},
                    { new ColorCust(98, 119, 92, 62)},
                    { new ColorCust(99, 172, 153, 117)},
                    { new ColorCust(100, 108, 107, 75)},
                    { new ColorCust(101, 64, 46, 43)},
                    { new ColorCust(102, 164, 150, 95)},
                    { new ColorCust(103, 70, 35, 26)},
                    { new ColorCust(104, 117, 43, 25)},
                    { new ColorCust(105, 191, 174, 123)},
                    { new ColorCust(106, 223, 213, 178)},
                    { new ColorCust(107, 247, 237, 213)},
                    { new ColorCust(108, 58, 42, 27)},
                    { new ColorCust(109, 120, 95, 51)},
                    { new ColorCust(110, 181, 160, 121)},
                    { new ColorCust(111, 255, 255, 246)},
                    { new ColorCust(112, 234, 234, 234)},
                    { new ColorCust(113, 176, 171, 148)},
                    { new ColorCust(114, 69, 56, 49)},
                    { new ColorCust(115, 42, 40, 43)},
                    { new ColorCust(116, 114, 108, 87)},
                    { new ColorCust(117, 106, 116, 124)},
                    { new ColorCust(118, 53, 65, 88)},
                    { new ColorCust(119, 155, 160, 168)},
                    { new ColorCust(120, 88, 112, 161)},
                    { new ColorCust(121, 234, 230, 222)},
                    { new ColorCust(122, 223, 221, 208)},
                    { new ColorCust(123, 242, 173, 46)},
                    { new ColorCust(124, 249, 164, 88)},
                    { new ColorCust(125, 131, 197, 102)},
                    { new ColorCust(126, 241, 204, 64)},
                    { new ColorCust(127, 76, 195, 218)},
                    { new ColorCust(128, 78, 100, 67)},
                    { new ColorCust(129, 188, 172, 143)},
                    { new ColorCust(130, 248, 182, 88)},
                    { new ColorCust(131, 252, 249, 241)},
                    { new ColorCust(132, 255, 255, 251)},
                    { new ColorCust(133, 129, 132, 76)},
                    { new ColorCust(134, 255, 255, 255)},
                    { new ColorCust(135, 242, 31, 153)},
                    { new ColorCust(136, 253, 214, 205)},
                    { new ColorCust(137, 223, 88, 145)},
                    { new ColorCust(138, 246, 174, 32)},
                    { new ColorCust(139, 176, 238, 110)},
                    { new ColorCust(140, 8, 233, 250)},
                    { new ColorCust(141, 10, 12, 23)},
                    { new ColorCust(142, 12, 13, 24)},
                    { new ColorCust(143, 14, 13, 20)},
                    { new ColorCust(144, 159, 158, 138)},
                    { new ColorCust(145, 98, 18, 118)},
                    { new ColorCust(146, 11, 20, 33)},
                    { new ColorCust(147, 17, 20, 26)},
                    { new ColorCust(148, 107, 31, 123)},
                    { new ColorCust(149, 30, 29, 34)},
                    { new ColorCust(150, 188, 25, 23)},
                    { new ColorCust(151, 45, 54, 42)},
                    { new ColorCust(152, 105, 103, 72)},
                    { new ColorCust(153, 122, 108, 85)},
                    { new ColorCust(154, 195, 180, 146)},
                    { new ColorCust(155, 90, 99, 82)},
                    { new ColorCust(156, 129, 130, 127)},
                    { new ColorCust(157, 175, 214, 228)},
                    { new ColorCust(158, 122, 100, 64)},
                    { new ColorCust(159, 127, 106, 72 )},
                    };
                for (int i = 0; i <= 159; i++)
                    for (int j = 0; j < 159; j++)
                    {
                        if (CompareColor(list[j], list[j + 1]) > 0)
                        {
                            ColorCust col = list[j];
                            list[j] = list[j + 1];
                            list[j + 1] = col;
                        }
                    }
                using (var w = new StreamWriter("Configs/colors.js"))
                {
                    w.Write($"global.RageColorsList = JSON.parse(`{JsonConvert.SerializeObject(list)}`);");
                }

            }
            catch
            {
            }
        }

        private static double CompareColor(ColorCust c1, ColorCust c2)
        {
            if (c1.Red == c2.Red && c1.Green == c2.Green && c1.Blue == c2.Blue)
                return 0;
            return Math.Sqrt(0.241 * c1.Red + 0.691 * c1.Green + 0.068 * c1.Blue) - Math.Sqrt(0.241 * c2.Red + 0.691 * c2.Green + 0.068 * c2.Blue);
        }
    }
    class ColorCust
    {
        public int Number { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public ColorCust(int n, int r, int g, int b)
        {
            Number = n;
            Red = r;
            Green = g;
            Blue = b;
        }
    }
}
