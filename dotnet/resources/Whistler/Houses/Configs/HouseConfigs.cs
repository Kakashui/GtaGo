using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Houses.Models;

namespace Whistler.Houses.Configs
{
    class HouseConfigs
    {
        public static Dictionary<int, GarageType> GarageTypes = new Dictionary<int, GarageType>()
        {
            { 0, new GarageType(GarageCoordType.TwoPlace, GarageIPLType.NoIPL, 1, 0, "house:garage:type:one".Translate(1))},
            { 1, new GarageType(GarageCoordType.TwoPlace, GarageIPLType.NoIPL, 2, 200000, "house:garage:type:two".Translate(2))},
            { 2, new GarageType(GarageCoordType.FivePlace, GarageIPLType.NoIPL, 3, 300000, "house:garage:type:two".Translate(3))},
            { 3, new GarageType(GarageCoordType.FivePlace, GarageIPLType.NoIPL, 4, 400000, "house:garage:type:two".Translate(4))},
            { 4, new GarageType(GarageCoordType.FivePlace, GarageIPLType.NoIPL, 5, 500000, "house:garage:type".Translate(5))},
            { 6, new GarageType(GarageCoordType.TenPlace, GarageIPLType.NoIPL, 10, 1000000, "house:garage:type".Translate(10))},
            { 7, new GarageType(GarageCoordType.G30place, GarageIPLType.NoIPL, 15, 4500000, "house:garage:type1".Translate(15))},
            { 8, new GarageType(GarageCoordType.G30place, GarageIPLType.NoIPL, 20, 6000000, "house:garage:type1".Translate(20))},
            { 9, new GarageType(GarageCoordType.G30place, GarageIPLType.NoIPL, 25, 7500000, "house:garage:type1".Translate(25))},
            { 10, new GarageType(GarageCoordType.G30place, GarageIPLType.NoIPL, 30, 9000000, "house:garage:type1".Translate(30))},
            { 11, new GarageType(GarageCoordType.G50Place, GarageIPLType.NoIPL, 35, 10500000, "house:garage:type1".Translate(35))},
            { 12, new GarageType(GarageCoordType.G50Place, GarageIPLType.NoIPL, 40, 12000000, "house:garage:type1".Translate(40))},
            { 13, new GarageType(GarageCoordType.G50Place, GarageIPLType.NoIPL, 45, 13500000, "house:garage:type1".Translate(45))},
            { 14, new GarageType(GarageCoordType.G50Place, GarageIPLType.NoIPL, 50, 15000000, "house:garage:type1".Translate(50))},
            { 16, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle1, 12, 5000000, "Tuner гараж на 12 мест")},
            { 17, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle2, 12, 5000000, "Tuner гараж на 12 мест")},
            { 18, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle3, 12, 5000000, "Tuner гараж на 12 мест")},
            { 19, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle4, 12, 5000000, "Tuner гараж на 12 мест")},
            { 20, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle5, 12, 5000000, "Tuner гараж на 12 мест")},
            { 21, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle6, 12, 5000000, "Tuner гараж на 12 мест")},
            { 22, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle7, 12, 5000000, "Tuner гараж на 12 мест")},
            { 23, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle8, 12, 5000000, "Tuner гараж на 12 мест")},
            { 24, new GarageType(GarageCoordType.TunerGarage, GarageIPLType.TunerGarageStyle9, 12, 5000000, "Tuner гараж на 12 мест")},
            { 25, new GarageType(GarageCoordType.G52Place, GarageIPLType.NoIPL, 52, 15000000, "house:garage:type:elite".Translate(52))},
        };
        public static Dictionary<GarageCoordType, GarageCoordsModel> GarageCoordModels = new Dictionary<GarageCoordType, GarageCoordsModel>()
        {
            { GarageCoordType.TwoPlace, new GarageCoordsModel(
                new Vector3(178.9356, -1000.594, -100),
                new Vector3(172.66, -1007.306, -99.9199),
                new Vector3(169.86597, -1000.5858, -98.99998),
                new List<Vector3>(){
                    new Vector3(170.6935, -1004.269, -99.41191),
                    new Vector3(174.3777, -1003.795, -99.41129),
                },
                new List<Vector3>(){
                    new Vector3(-0.1147747, 0.02747092, 183.3471),
                    new Vector3(-0.1562817, 0.01328733, 175.7529),
                })},
            { GarageCoordType.FivePlace, new GarageCoordsModel(
                new Vector3(206.9094, -999.0917, -100),
                new Vector3(195.8676, -1006.121, -99.92),
                new Vector3(205.72539, -995.2872, -98.999916),
                new List<Vector3>(){
                    new Vector3(200.7814, -997.5886, -99.41073),
                    new Vector3(197.3544, -997.4301, -99.41062),
                    new Vector3(193.8947, -997.2777, -99.41056),
                    new Vector3(203.6087, -1005.661, -98.85284),
                    new Vector3(203.4404, -1002.429, -98.85185),
                },
                new List<Vector3>(){
                    new Vector3(-0.1146501, -0.03047129, 165.095),
                    new Vector3(-0.1124166, -0.03466159, 163.7391),
                    new Vector3(-0.1131818, -0.03073582, 163.4609),
                    new Vector3(0.02571397, -0.3010964, 92.8107),
                    new Vector3(0.03179136, -0.2467785, 91.52057),
                })},
            { GarageCoordType.TenPlace, new GarageCoordsModel(
                new Vector3(240.411, -1004.753, -100),
                new Vector3(224.8484, -1005.053, -99.9199),
                new Vector3(234.00348, -976.54724, -98.99996),
                new List<Vector3>(){
                    new Vector3(233.1973, -981.9477, -99.41358),
                    new Vector3(233.0927, -986.0645, -99.41795),
                    new Vector3(233.0163, -989.8713, -99.41821),
                    new Vector3(232.9416, -993.6203, -99.41826),
                    new Vector3(232.8721, -997.5245, -99.41066),
                    new Vector3(223.6207, -978.5778, -99.41045),
                    new Vector3(223.4642, -983.1303, -99.41094),
                    new Vector3(223.5002, -987.4559, -99.4104),
                    new Vector3(223.494, -991.6437, -99.41273),
                    new Vector3(223.4669, -995.4849, -99.41091),
                },
                new List<Vector3>(){
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 227),
                    new Vector3(0, 0, 227),
                    new Vector3(0, 0, 227),
                    new Vector3(0, 0, 227),
                    new Vector3(0, 0, 227),
                })},
            { GarageCoordType.G30place, new GarageCoordsModel(
                new Vector3(4616.65777, -0.14928502, 200.1200002),
                new Vector3(4621.93507, -0.1716583, 200.1200002),
                new Vector3(4613.1567, 5.542334, 200.99997),
                new List<Vector3>(){
                    new Vector3(4595.87585, -11.17133, 200.1200002),//1
                    new Vector3(4584.42291, -11.17133, 200.1200002),
                    new Vector3(4579.45054, -11.17133, 200.1200002),
                    new Vector3(4569.33218, -11.17133, 200.1200002),
                    new Vector3(4598.01467, 4.0878441, 200.1200002),//5
                    new Vector3(4593.64362, 4.0878441, 200.1200002),
                    new Vector3(4585.86563, 4.0878441, 200.1200002),
                    new Vector3(4581.84975, 4.0878441, 200.1200002),
                    new Vector3(4577.68468, 4.0878441, 200.1200002),
                    new Vector3(4569.65805, 4.0878441, 200.1200002),
                    new Vector3(4565.38393, 4.0878441, 200.1200002),
                    new Vector3(4560.58043, 4.0878441, 200.1200002),
                    new Vector3(4554.28994, 0.8775998, 200.1200002),//13
                    new Vector3(4554.28994, -4.466756, 200.1200002),
                    new Vector3(4615.27796, -32.72151, 200.1200002),//15
                    new Vector3(4606.80334, -32.72151, 200.1200002),
                    new Vector3(4598.01467, -32.72151, 200.1200002),
                    new Vector3(4593.64362, -32.72151, 200.1200002),
                    new Vector3(4585.86563, -32.72151, 200.1200002),
                    new Vector3(4581.84975, -32.72151, 200.1200002),
                    new Vector3(4577.68468, -32.72151, 200.1200002),
                    new Vector3(4569.65805, -32.72151, 200.1200002),
                    new Vector3(4565.38393, -32.72151, 200.1200002),
                    new Vector3(4560.58043, -32.72151, 200.1200002),
                    new Vector3(4554.28994, -29.30894, 200.1200002),//25
                    new Vector3(4554.28994, -24.73788, 200.1200002),
                    new Vector3(4595.87585, -18.20844, 200.1200002),//27
                    new Vector3(4584.42291, -18.20844, 200.1200002),
                    new Vector3(4579.45054, -18.20844, 200.1200002),
                    new Vector3(4569.33218, -18.20844, 200.1200002),
                },
                new List<Vector3>(){
                    new Vector3(0, 0, 0),//1
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 180),//5
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 270),//13
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 0),//15
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 270),//25
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 180),//27
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                })},
            { GarageCoordType.G50Place, new GarageCoordsModel(
                new Vector3(3782.15758, 469.5164, 202.644107),
                new Vector3(3782.54144, 476.12762, 202.644107),
                new Vector3(3744.0474, 462.68124, 203.56421),
                new List<Vector3>(){
                    new Vector3(3749.9274, 472.11994, 202.644107),//1
                    new Vector3(3755.3423, 472.11994, 202.644107),
                    new Vector3(3761.3461, 472.11994, 202.644107),
                    new Vector3(3765.2382, 472.11994, 202.644107),
                    new Vector3(3769.4537, 472.11994, 202.644107),
                    new Vector3(3794.8086, 473.11994, 202.644107),
                    new Vector3(3799.0637, 473.11994, 202.644107),
                    new Vector3(3803.2479, 473.11994, 202.644107),
                    new Vector3(3809.4546, 473.11994, 202.644107),
                    new Vector3(3813.9925, 473.11994, 202.644107),
                    new Vector3(3808.8091, 457.07304, 202.644107),//11
                    new Vector3(3804.7656, 457.07304, 202.644107),
                    new Vector3(3798.5374, 457.07304, 202.644107),
                    new Vector3(3794.3092, 457.07304, 202.644107),
                    new Vector3(3789.8653, 457.07304, 202.644107),
                    new Vector3(3773.9220, 457.07304, 202.644107),
                    new Vector3(3769.6321, 457.07304, 202.644107),
                    new Vector3(3765.3638, 457.07304, 202.644107),
                    new Vector3(3759.2633, 457.07304, 202.644107),
                    new Vector3(3755.0379, 457.07304, 202.644107),
                    new Vector3(3820.8209, 467.01385, 202.644107),//21
                    new Vector3(3820.8209, 462.25143, 202.644107),
                    new Vector3(3820.8209, 457.64421, 202.644107),
                    new Vector3(3820.8209, 451.28818, 202.644107),
                    new Vector3(3820.8209, 444.95802, 202.644107),
                    new Vector3(3820.8209, 440.33104, 202.644107),
                    new Vector3(3820.8209, 435.93761, 202.644107),
                    new Vector3(3808.2842, 433.40052, 202.644107),//28
                    new Vector3(3804.3811, 433.40052, 202.644107),
                    new Vector3(3796.6981, 433.40052, 202.644107),
                    new Vector3(3788.9179, 433.40052, 202.644107),
                    new Vector3(3784.4559, 433.40052, 202.644107),
                    new Vector3(3780.2084, 433.40052, 202.644107),
                    new Vector3(3775.9414, 433.40052, 202.644107),
                    new Vector3(3768.4916, 433.40052, 202.644107),
                    new Vector3(3760.6178, 433.40052, 202.644107),
                    new Vector3(3756.5287, 433.40052, 202.644107),
                    new Vector3(3743.4977, 432.40683, 202.644107),//38
                    new Vector3(3743.4977, 440.07399, 202.644107),
                    new Vector3(3743.4977, 447.68913, 202.644107),
                    new Vector3(3755.0379, 448.31988, 202.644107),//41
                    new Vector3(3759.2633, 448.31988, 202.644107),
                    new Vector3(3765.3638, 448.31988, 202.644107),
                    new Vector3(3769.6321, 448.31988, 202.644107),
                    new Vector3(3773.9220, 448.31988, 202.644107),
                    new Vector3(3789.8653, 448.31988, 202.644107),
                    new Vector3(3794.3092, 448.31988, 202.644107),
                    new Vector3(3798.5374, 448.31988, 202.644107),
                    new Vector3(3804.7656, 448.31988, 202.644107),
                    new Vector3(3808.8091, 448.31988, 202.644107),
                },
                new List<Vector3>(){
                    new Vector3(0, 0, 180),//1
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 0),//11
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 90),//21
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 0),//28
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 270),//38
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 180),//41
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                    new Vector3(0, 0, 180),
                })},
            //-3400.00000000" y="-600.00000000" z="450.00000000 old
            //1660.00000000" y="-490.00000000" z="105.00000000 new
            { GarageCoordType.G52Place, new GarageCoordsModel(
                new Vector3(-3409.506, -599.9435, 450.05) + new Vector3(5060, 110, -345),
                new Vector3(-3400, -600, 450) + new Vector3(5060, 110, -345),
                new Vector3(1680.2682, -463.8589, 106.00335),
                new List<Vector3>(){
                    new Vector3(-3376, -590.0, 450) + new Vector3(5060, 110, -345),//1
                    new Vector3(-3376, -586.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3376, -582.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3436, -590.0, 450) + new Vector3(5060, 110, -345),//4
                    new Vector3(-3436, -586.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3436, -582.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3391, -575.5, 450) + new Vector3(5060, 110, -345),//7
                    new Vector3(-3391, -570.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3391, -566.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3391, -562.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3391, -556.5, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3403, -577.0, 450) + new Vector3(5060, 110, -345),//12
                    new Vector3(-3403, -570.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3403, -566.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3403, -562.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3403, -556.5, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3409, -577.0, 450) + new Vector3(5060, 110, -345),//17
                    new Vector3(-3409, -570.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3409, -566.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3409, -562.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3409, -556.5, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3421, -575.5, 450) + new Vector3(5060, 110, -345),//22
                    new Vector3(-3421, -570.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3421, -566.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3421, -562.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3421, -556.5, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3376, -610.0, 450) + new Vector3(5060, 110, -345),//27
                    new Vector3(-3376, -614.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3376, -618.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3436, -610.0, 450) + new Vector3(5060, 110, -345),//30
                    new Vector3(-3436, -614.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3436, -618.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3391, -624.5, 450) + new Vector3(5060, 110, -345),//33
                    new Vector3(-3391, -630.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3391, -634.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3391, -638.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3391, -643.5, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3403, -623.0, 450) + new Vector3(5060, 110, -345),//38
                    new Vector3(-3403, -630.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3403, -634.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3403, -638.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3403, -643.5, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3409, -623.0, 450) + new Vector3(5060, 110, -345),//43
                    new Vector3(-3409, -630.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3409, -634.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3409, -638.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3409, -643.5, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3421, -624.5, 450) + new Vector3(5060, 110, -345),//48
                    new Vector3(-3421, -630.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3421, -634.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3421, -638.0, 450) + new Vector3(5060, 110, -345),
                    new Vector3(-3421, -643.5, 450) + new Vector3(5060, 110, -345),
                },
                new List<Vector3>(){
                    new Vector3(0, 0, 90),//1
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 270),//4
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 120),//7
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 240),//12
                    new Vector3(0, 0, 240),
                    new Vector3(0, 0, 240),
                    new Vector3(0, 0, 240),
                    new Vector3(0, 0, 240),
                    new Vector3(0, 0, 120),//17
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 120),
                    new Vector3(0, 0, 240),//22
                    new Vector3(0, 0, 240),
                    new Vector3(0, 0, 240),
                    new Vector3(0, 0, 240),
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 90),//27
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 270),//30
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 60),//33
                    new Vector3(0, 0, 60),
                    new Vector3(0, 0, 60),
                    new Vector3(0, 0, 60),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 300),//38
                    new Vector3(0, 0, 300),
                    new Vector3(0, 0, 300),
                    new Vector3(0, 0, 300),
                    new Vector3(0, 0, 300),
                    new Vector3(0, 0, 60),//43
                    new Vector3(0, 0, 60),
                    new Vector3(0, 0, 60),
                    new Vector3(0, 0, 60),
                    new Vector3(0, 0, 60),
                    new Vector3(0, 0, 300),//48
                    new Vector3(0, 0, 300),
                    new Vector3(0, 0, 300),
                    new Vector3(0, 0, 300),
                    new Vector3(0, 0, 270),
                })},
            { GarageCoordType.TunerGarage, new GarageCoordsModel(
                new Vector3(-1357.715, 152.3564, -100.19434),
                new Vector3(-1357.886, 166.5899, -99.95017),
                new Vector3(-1347.6741, 161.18549, -99.19425),
                new List<Vector3>(){
                    new Vector3(-1329.554, 156, -99.05546),
                    new Vector3(-1333.545, 156, -99.05555),
                    new Vector3(-1337.241, 156, -99.05589),
                    new Vector3(-1340.914, 156, -99.05517),
                    new Vector3(-1325.562, 148.5, -99.05551),
                    new Vector3(-1325.498, 145, -99.05609),
                    new Vector3(-1340.637, 141, -99.05552),
                    new Vector3(-1328.407, 141, -99.05547),
                    new Vector3(-1332.222, 141, -99.05516),
                    new Vector3(-1336.321, 141, -99.05526),
                    new Vector3(-1363.838, 161, -98.85555),
                    new Vector3(-1353.003, 165, -99.05524),
                },
                new List<Vector3>(){
                    new Vector3(0, 0, 162),
                    new Vector3(0, 0, 162),
                    new Vector3(0, 0, 162),
                    new Vector3(0, 0, 162),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 90),
                    new Vector3(0, 0, 30),
                    new Vector3(0, 0, 30),
                    new Vector3(0, 0, 30),
                    new Vector3(0, 0, 30),
                    new Vector3(0, 0, 270),
                    new Vector3(0, 0, 180),
                })},
        };

    }
}