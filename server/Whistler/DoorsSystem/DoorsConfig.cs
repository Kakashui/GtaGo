using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Collections;

namespace Whistler.DoorsSystem
{
    public class DoorsConfig
    {
        private List<DoorComplect> _doors;
        private List<DoorComplect> _interractedDoors;
        public List<int> Changed
        {
            get
            {
                return _interractedDoors.Where(d => d.IsChanged()).Select(d => d.Hash).ToList();
            }
        }
        public DoorsConfig()
        {
            _doors = new List<DoorComplect>();

            _doors.Add(new DoorComplect(
                "geto_home1:entry",
                new Vector3(-13.64512, -1441.138, 31.04724),
                new List<Door>
                {
                new Door("v_ilev_fa_frontdoor", new Vector3(-14.86892, -1441.182, 31.19323))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "police:vinewood:stock",
                new Vector3(588.228, -8.793819, 82.97115),
                new List<Door>
                {
                    new Door("v_ilev_arm_secdoor", new Vector3(587.2617,-8.413556, 82.88877))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:entry",
                new Vector3(637.819, 2.09473, 82.8883),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3(637.267, 0.6447992, 81.77871)),
                    new Door("goverment_door_b_light", new Vector3(638.156, 3.084809, 81.77871))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:interrogation",
                new Vector3(618.242, -2.891464, 83.40837),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_2", new Vector3(618.6752, -1.723899, 82.84883))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:relax",
                new Vector3(603.7998, -3.546293, 82.92204),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_1", new Vector3(603.491, -4.643977, 82.84801))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:office",
                new Vector3(610.3824, -9.659199, 82.82001),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_1", new Vector3(611.5206, -10.08526, 82.8485)),
                    new Door("v_ilev_police_door_black_1", new Vector3(609.241, -9.234158, 82.84798))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:boss",
                new Vector3(561.373, -27.78788, 82.87099),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3(561.7715, -26.69364, 81.74203))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:entry_office_1",
                new Vector3(604.3372, -23.91649, 82.80021),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_1", new Vector3(603.196, -23.48956, 82.84798)),
                    new Door("v_ilev_police_door_black_1", new Vector3(605.4756, -24.34066, 82.8485))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:room_1",
                new Vector3(589.3937, -26.58258, 82.78424),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3(588.2429, -26.13994, 81.74219))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:room_2",
                new Vector3(579.8773, -23.01531, 82.77633),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3(578.7088, -22.5804, 81.74153))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:room_3",
                new Vector3(571.4293, -19.85105, 82.796),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3(570.264, -19.42767, 81.74071))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:entry_office_2",
                new Vector3(566.3691, -9.888311, 82.79298),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_1", new Vector3(565.2325, -9.463183, 82.8485)),
                    new Door("v_ilev_police_door_black_1", new Vector3(567.5111, -10.31388, 82.8485))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:conference_1_1",
                new Vector3(580.1688, -30.27964, 82.80132),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3(581.3239, -30.71605, 81.74406))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:conference_1_2",
                new Vector3(567.0071, -25.3836, 82.79093),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3(565.855, -24.95774, 81.74171))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:cloakroom",
                new Vector3(598.6998, -17.01911, 83.05782),
                new List<Door>
                {
                    new Door("v_ilev_arm_secdoor", new Vector3(599.0636, -16.04956, 82.88857))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:office_transition",
                new Vector3(591.3605, -5.722603, 82.77194),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_1", new Vector3(590.9342, -6.866064, 82.84798)),
                    new Door("v_ilev_police_door_black_1", new Vector3(591.7876, -4.580454, 82.8485))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:security",
                new Vector3(575.701, -4.08672, 83.11169),
                new List<Door>
                {
                    new Door("v_ilev_arm_secdoor", new Vector3(574.7305, -3.733751, 82.88879))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:class",
                new Vector3(587.6337, 4.064736, 83.01062),
                new List<Door>
                {
                    new Door("v_ilev_arm_secdoor", new Vector3( 588.5959, 3.66887, 82.88898))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:kpz_transition_1",
                new Vector3(569.1747, 7.466656, 82.79314),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_1", new Vector3(568.7545, 6.32374, 82.8485)),
                    new Door("v_ilev_police_door_black_1", new Vector3(569.605, 8.60177, 82.8485))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:kpz_transition_2",
                new Vector3(555.8804, 6.767722, 82.79609),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_1", new Vector3(555.4594, 5.62534, 82.8485)),
                    new Door("v_ilev_police_door_black_1", new Vector3(556.3099, 7.90337, 82.8485))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:conference_2",
                new Vector3(550.356, -7.734417, 82.76962),
                new List<Door>
                {
                    new Door("v_ilev_police_door_black_1", new Vector3(550.7803, -6.593941, 82.8485)),
                    new Door("v_ilev_police_door_black_1", new Vector3(549.9288, -8.874659, 82.8485))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:shooting_range",
                new Vector3(546.0627, -9.585775, 83.07955),
                new List<Door>
                {
                    new Door("v_ilev_arm_secdoor", new Vector3(545.1747, -9.058297, 82.89233))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:shower",
                new Vector3(542.633, 2.233013, 83.0816),
                new List<Door>
                {
                    new Door("v_ilev_police_door_wood_1", new Vector3(542.6883, 2.333819, 82.84774))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "police:vinewood:restroom",
                new Vector3(546.3307, 12.12834, 82.86761),
                new List<Door>
                {
                    new Door("v_ilev_police_door_wood_1", new Vector3(546.3758, 12.21277, 82.84836))
                },
                true,
                true
            ));
            //EMS
            _doors.Add(new DoorComplect(
                "hospital:vinewood:cloakroom",
                new Vector3(322.1155, -590.9471, 43.1331),
                new List<Door>
                {
                    new Door("v_ilev_hosp_door_white", new Vector3(323.1802, -591.3309, 43.24577))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:entry",
                new Vector3(299.5629, -584.7557, 43.45903),
                new List<Door>
                {
                new Door("hosp_enter_door_r", new Vector3(299.9657, -583.6624, 42.25973)),
                new Door("hosp_enter_door_l", new Vector3(299.1628, -585.8682, 42.25973))
                },
                false,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_left_1",
                new Vector3(311.3285, -568.483, 43.2071),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(312.3786, -568.8623, 43.24121))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_left_2",
                new Vector3(320.3877, -572.6398, 43.19053),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(319.3586, -572.2745, 43.25445))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_left_3",
                new Vector3(327.5016, -575.1868, 43.21661),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(326.3764, -574.7646, 43.24855)),
                new Door("v_ilev_hosp_door_white", new Vector3(328.6354, -575.5869, 43.24855))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_left_4",
                new Vector3(340.0043, -579.7473, 43.22858),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(338.8793, -579.3412, 43.24121)),
                new Door("v_ilev_hosp_door_white", new Vector3(341.1366, -580.1628, 43.24121))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_transition",
                new Vector3(346.205, -585.0967, 43.23907),
                new List<Door>
                {
                new Door("v_ilev_fib_door2", new Vector3(346.6172, -583.9647, 43.26465))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_left_5",
                new Vector3(351.0147, -583.7668, 43.14777),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(349.9863, -583.3708, 43.25128))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_left_6",
                new Vector3(355.6556, -585.4288, 43.1795),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(356.6943, -585.7977, 43.24121))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_right_1",
                new Vector3(325.439, -579.1672, 43.21636),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(324.3214, -578.731, 43.24121)),
                new Door("v_ilev_hosp_door_white", new Vector3(326.5772, -579.5521, 43.24121))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_right_2",
                new Vector3(342.6181, -587.55, 43.20559),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(343.6481, -587.8959, 43.24121))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_right_3",
                new Vector3(350.1634, -592.791, 43.22066),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(351.2892, -593.207, 43.24386)),
                new Door("v_ilev_hosp_door_white", new Vector3(349.0333, -592.3859, 43.24386))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:left_corridor_right_4",
                new Vector3(359.7911, -596.2996, 43.22646),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(358.6659, -595.8918, 43.24293)),
                new Door("v_ilev_hosp_door_white", new Vector3(360.9177, -596.7115, 43.24293))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:right_corridor_left_1",
                new Vector3(313.0865, -582.3988, 43.22848),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(311.9606, -581.9913, 43.24121)),
                new Door("v_ilev_hosp_door_white", new Vector3(314.2182, -582.813, 43.24121))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "hospital:vinewood:right_corridor_right_2",
                new Vector3(329.8716, -593.7874, 43.21136),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(330.9204, -594.1481, 43.24145))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:right_1",
                new Vector3(310.3459, -590.8871, 43.25626),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(309.9731, -591.9142, 43.25626))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:right_2",
                new Vector3(307.4112, -598.944, 43.16924),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(307.7924, -597.9055, 43.25523))
                },
                true,
                true
            ));
            //FIB
            _doors.Add(new DoorComplect(
                "fib:parking:door",
                new Vector3(183.7265, -724.3046, 34.29744),
                new List<Door>
                {
                    new Door("prop_pris_door_02", new Vector3(184.1353, -723.1947, 34.14559))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "fib:parking:gate",
                new Vector3(84.44587, -693.205, 31.98824),
                new List<Door>
                {
                    new Door("hei_prop_station_gate", new Vector3(57.02479, -690.6066, 30.66128)),
                    new Door("hei_prop_station_gate", new Vector3(86.3513, -688.002, 30.88934))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "fib:office:rightdoor",
                new Vector3(127.1263, -765.0555, 242.753),
                new List<Door>
                {
                    new Door("v_ilev_fib_door2", new Vector3(127.2092, -764.6935, 242.302))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "fib:office:leftdoor",
                new Vector3(139.0332, -767.2325, 242.7385),
                new List<Door>
                {
                    new Door("v_ilev_fib_door2", new Vector3(138.5112, -768.8054, 242.3022))
                },
                true,
                true
            ));
            //Army
            _doors.Add(new DoorComplect(
                "army:tower:entry",
                new Vector3(-2342.868, 3266.498, 32.83969),
                new List<Door>
                {
                    new Door("v_ilev_ct_doorr", new Vector3(-2343.531, 3265.371, 32.95998)),
                    new Door("v_ilev_ct_doorl", new Vector3(-2342.231, 3267.624, 32.95998))
                },
                true,
                true
            ));
            //Vineyard - (LACOSTA)
            _doors.Add(new DoorComplect(
                "vineyard:house:1",
                new Vector3(-1886.398, 2050.876, 141.2531),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1887.44, 2051.249, 141.2503)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1885.365, 2050.494, 141.2503))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:2",
                new Vector3(-1860.507, 2054.098, 141.282),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1861.611, 2054.107, 141.2811)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1859.402, 2054.107, 141.2811))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:3",
                new Vector3(-1857.114, 2058.848, 141.2857),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1857.124, 2057.742, 141.2811)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1857.124, 2059.95, 141.2811))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:4",
                new Vector3(-1874.462, 2069.604, 141.2611),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1873.424, 2069.229, 141.2522)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1875.5, 2069.984, 141.2522))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:5",
                new Vector3(-1886.131, 2073.847, 141.2618),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1885.093, 2073.476, 141.2522)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1887.168, 2074.231, 141.2522))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:6",
                new Vector3(-1893.827, 2075.151, 141.2565),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1892.981, 2074.439, 141.2522)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1894.673, 2075.859, 141.2522))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:7",
                new Vector3(-1908.743, 2082.245, 140.6796),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1908.036, 2083.09, 140.6749)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1909.455, 2081.399, 140.6749))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:8",
                new Vector3(-1911.286, 2074.833, 140.6834),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1912.13, 2075.536, 140.6751)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1910.439, 2074.117, 140.6751))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:9",
                new Vector3(-1908.717, 2072.667, 140.6817),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1909.557, 2073.377, 140.6751)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1907.865, 2071.958, 140.6751))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:house:10",
                new Vector3(-1889.044, 2051.835, 141.26),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1890.086, 2052.212, 141.2503)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-1888.011, 2051.457, 141.2503))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:stock",
                new Vector3(-1888.8, 2068.929, 133.854),
                new List<Door>
                {
                    new Door("v_ilev_phroofdoor", new Vector3(-1887.906, 2068.252, 133.8203))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "vineyard:garage",
                new Vector3(-1925.399, 2050.64, 141.6573),
                new List<Door>
                {
                    new Door("go_mafia_garage_door", new Vector3(-1927.484, 2042.257, 141.5762)),
                    new Door("go_mafia_garage_door", new Vector3(-1925.96, 2048.227, 141.5707)),
                    new Door("go_mafia_garage_door", new Vector3(-1925.96, 2048.227, 141.5707))
                },
                true,
                true
            ));
            //Ranch - ranch (Lahmadju)
            _doors.Add(new DoorComplect(
                "ranch:house:1",
                new Vector3(1395.931, 1141.803, 114.7408),
                new List<Door>
                {
                    new Door("v_ilev_ra_door4l", new Vector3(1395.92, 1142.904, 114.7902)),
                    new Door("v_ilev_ra_door4r", new Vector3(1395.92, 1140.705, 114.7902))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:2",
                new Vector3(1390.655, 1132.22, 114.3039),
                new List<Door>
                {
                    new Door("go_ranch_door_2", new Vector3(1390.666, 1133.317, 114.4808)),
                    new Door("go_ranch_door_1", new Vector3(1390.666, 1131.117, 114.4808))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:3",
                new Vector3(1399.386, 1128.305, 114.3328),
                new List<Door>
                {
                    new Door("go_ranch_door_2", new Vector3(1398.289, 1128.314, 114.4836)),
                    new Door("go_ranch_door_1", new Vector3(1400.489, 1128.314, 114.4836))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:4",
                new Vector3(1401.595, 1128.316, 114.3497),
                new List<Door>
                {
                    new Door("go_ranch_door_2", new Vector3(1400.488, 1128.314, 114.4836)),
                    new Door("go_ranch_door_1", new Vector3(1402.688, 1128.314, 114.4836))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:5",
                new Vector3(1406.507, 1128.44, 114.4715),
                new List<Door>
                {
                    new Door("v_ilev_ra_door4r", new Vector3(1407.548, 1128.425, 114.4901))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:6",
                new Vector3(1409.292, 1145.144, 114.3539),
                new List<Door>
                {
                    new Door("go_ranch_door_1", new Vector3(1409.292, 1144.054, 114.4869)),
                    new Door("go_ranch_door_2", new Vector3(1409.292, 1146.254, 114.4869))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:7",
                new Vector3(1409.271, 1147.353, 114.3508),
                new List<Door>
                {
                    new Door("go_ranch_door_1", new Vector3(1409.292, 1146.254, 114.4869)),
                    new Door("go_ranch_door_2", new Vector3(1409.292, 1148.454, 114.4869))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:8",
                new Vector3(1409.284, 1149.558, 114.3538),
                new List<Door>
                {
                    new Door("go_ranch_door_1", new Vector3(1409.292, 1148.454, 114.4869)),
                    new Door("go_ranch_door_2", new Vector3(1409.292, 1150.654, 114.4869))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:9",
                new Vector3(1408.142, 1160.057, 114.3102),
                new List<Door>
                {
                    new Door("go_ranch_door_1", new Vector3(1408.142, 1158.965, 114.4812)),
                    new Door("go_ranch_door_2", new Vector3(1408.144, 1161.162, 114.4811))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:10",
                new Vector3(1408.145, 1164.733, 114.3355),
                new List<Door>
                {
                    new Door("go_ranch_door_1", new Vector3(1408.142, 1163.636, 114.4812)),
                    new Door("go_ranch_door_2", new Vector3(1408.143, 1165.834, 114.4811))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:11",
                new Vector3(1390.375, 1162.343, 114.3223),
                new List<Door>
                {
                    new Door("go_ranch_door_2", new Vector3(1390.381, 1163.434, 114.4786)),
                    new Door("go_ranch_door_1", new Vector3(1390.38, 1161.236, 114.4787))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:house:12",
                new Vector3(1395.854, 1152.761, 114.3278),
                new List<Door>
                {
                    new Door("go_ranch_door_2", new Vector3(1395.85, 1153.858, 114.4847)),
                    new Door("go_ranch_door_1", new Vector3(1395.849, 1151.66, 114.4849))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:stock:entry",
                new Vector3(1444.444, 1132.807, 114.4298),
                new List<Door>
                {
                    new Door("v_ilev_247_offdorr", new Vector3(1395.931, 1141.803, 114.7408))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:stock:interroom",
                new Vector3(1448.827, 1139.533, 114.5346),
                new List<Door>
                {
                    new Door("v_ilev_247_offdorr", new Vector3(1447.646, 1139.536, 114.4293))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "ranch:stock:gate",
                new Vector3(1442.271, 1142.121, 114.2892),
                new List<Door>
                {
                    new Door("go_mafia4gate_a", new Vector3(1442.226, 1143.578, 113.3137)),
                    new Door("go_mafia4gate_b", new Vector3(1442.297, 1140.649, 113.3052))
                },
                true,
                true
            ));
            //Villa Court - vilage with cort (AUX)
            _doors.Add(new DoorComplect(
                "villa_court:gate",
                new Vector3(-1477.736, 884.9515, 183.5514),
                new List<Door>
                {
                new Door("prop_lrggate_01_l", new Vector3(-1478.257, 882.243, 183.0719)),
                new Door("prop_lrggate_01_r", new Vector3(-1477.229, 887.649, 183.0719))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_court:house:1",
                new Vector3(-1520.634, 848.3794, 181.8443),
                new List<Door>
                {
                    new Door("prop_ret_door_03", new Vector3(-1519.648, 848.842, 181.8971))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_court:house:2",
                new Vector3(-1491.006, 851.9854, 181.7296),
                new List<Door>
                {
                    new Door("go_prop_kt1_06_door_l", new Vector3(-1490.456, 850.8521, 181.8577)),
                    new Door("go_prop_kt1_06_door_r", new Vector3(-1491.515, 853.1234, 181.8577))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_court:house:3",
                new Vector3(-1501.648, 856.2175, 181.7635),
                new List<Door>
                {
                    new Door("prop_ret_door_03", new Vector3(-1500.657, 856.6772, 181.8662))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_court:house:stock",
                new Vector3(-1513.468, 848.0083, 181.6267),
                new List<Door>
                {
                    new Door("apa_p_mp_door_04", new Vector3(-1513.899, 848.9388, 181.7494))
                },
                true,
                true
            ));
            //Villa Pool - vilage  (Mumino)
            _doors.Add(new DoorComplect(
                "villa_pool:gate",
                new Vector3(-135.2851, 972.5954, 236.3271),
                new List<Door>
                {
                    new Door("prop_lrggate_01c_l", new Vector3(-137.7991, 973.7089, 236.1143)),
                    new Door("prop_lrggate_01c_r", new Vector3(-132.7885, 971.5002, 236.1143))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "villa_pool:house:1",
                new Vector3(-112.3875, 985.6082, 235.892),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-112.7745, 986.6306, 236.0094))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:2",
                new Vector3(-104.7373, 976.7414, 235.8494),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-105.7958, 976.3409, 236.0094)),
                    new Door("mafia_3_door_a", new Vector3(-103.6745, 977.1142, 236.0094))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:3",
                new Vector3(-97.77643, 989.2894, 235.87),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-98.83441, 988.9056, 236.0094)),
                    new Door("mafia_3_door_a", new Vector3(-96.71363, 989.6785, 236.0094))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:4",
                new Vector3(-67.4799, 987.7167, 234.4676),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-68.33714, 988.4249, 234.6541)),
                    new Door("mafia_3_door_a", new Vector3(-66.60736, 986.9746, 234.6541))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:5",
                new Vector3(-57.40046, 984.0363, 235.3025),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-57.0203, 982.9868, 235.4725)),
                    new Door("mafia_3_door_a", new Vector3(-57.79253, 985.1084, 235.4725))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:6",
                new Vector3(-59.41599, 989.5591, 235.3165),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-59.02417, 988.4923, 235.4725)),
                    new Door("mafia_3_door_a", new Vector3(-59.79632, 990.6138, 235.4725))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:7",
                new Vector3(-62.61241, 998.8463, 234.4742),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-61.79213, 998.1297, 234.6555))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:8",
                new Vector3(-70.83208, 1008.778, 234.502),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-69.99136, 1008.031, 234.6562)),
                    new Door("mafia_3_door_a", new Vector3(-71.67133, 1009.539, 234.6562))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:9",
                new Vector3(-102.6377, 1011.297, 235.8497),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-103.0305, 1012.349, 236.012)),
                    new Door("mafia_3_door_a", new Vector3(-102.2579, 1010.226, 236.012))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:house:10",
                new Vector3(-111.1465, 999.1646, 235.8746),
                new List<Door>
                {
                    new Door("mafia_3_door_a", new Vector3(-110.0823, 999.5333, 236.0094)),
                    new Door("mafia_3_door_a", new Vector3(-112.204, 998.76, 236.0094))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "villa_pool:garage",
                new Vector3(-125.1246, 1011.383, 236.2932),
                new List<Door>
                {
                    new Door("prop_ld_garaged_01", new Vector3(-131.8882, 1008.921, 236.2932)),
                    new Door("prop_ld_garaged_01", new Vector3(-125.1246, 1011.383, 236.2932))
                },
                true,
                true
            ));

            //MICHAEL HHOUSE
            _doors.Add(new DoorComplect(
               "michael:gate:car",
               new Vector3(-844.0151, 159.089, 67.2041),
               new List<Door>
               {
                    new Door("prop_lrggate_02_ld", new Vector3(-844.2006, 155.8123, 66.24381))
               },
               true,
               true
           ));
            _doors.Add(new DoorComplect(
               "michael:gate:people",
               new Vector3(-849.0275, 178.0843, 69.69681),
               new List<Door>
               {
                    new Door("prop_bh1_48_gate_1", new Vector3(-848.9343, 179.3079, 70.0247))
               },
               true,
               true
           ));

            _doors.Add(new DoorComplect(
               "michael:door:main",
               new Vector3(-816.4765, 178.2636, 72.34283),
               new List<Door>
               {
                    new Door("v_ilev_mm_doorm_r", new Vector3(-816.1068, 177.5109, 72.82738)),
                    new Door("v_ilev_mm_doorm_l", new Vector3(-816.716, 179.098, 72.82738))
               },
               true,
               true
           ));
            _doors.Add(new DoorComplect(
              "michael:door:back:1",
              new Vector3(-795.5394, 177.5771, 72.96831),
              new List<Door>
              {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-794.5051, 178.0124, 73.04045)),
                    new Door("prop_bh1_48_backdoor_l", new Vector3(-796.5657, 177.2214, 73.04045))
              },
              true,
              true
          ));
            _doors.Add(new DoorComplect(
               "michael:door:back:2",
               new Vector3(-793.7435, 181.5475, 73.02856),
               new List<Door>
               {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-794.1853, 182.568, 73.04045)),
                    new Door("prop_bh1_48_backdoor_l", new Vector3(-793.3943, 180.5057, 73.04045))
               },
               true,
               true
           ));
            _doors.Add(new DoorComplect(
               "michael:door:garage",
               new Vector3(-807.4322, 185.6509, 72.55756),
               new List<Door>
               {
                    new Door("v_ilev_mm_door", new Vector3(-806.2817, 186.0246, 72.62405)),
               },
               true,
               true
           ));
            // _doors.Add(new DoorComplect(
            //    "winevood:hose2:gate",
            //    new Vector3(-878.1193, 19.79912, 45.31781),
            //    new List<Door>
            //    {
            //         new Door("prop_lrggate_02", new Vector3(-875.4845, 18.12612, 44.4434)),
            //    },
            //    true,
            //    true
            //));

            //Citi_hall 
            //enterance center

            _doors.Add(new DoorComplect(
                "city_hall:c_door",
                new Vector3(-545.5228, -203.3607, 38.82366),
                new List<Door>
                {
                    new Door("go_cityhall_front_door", new Vector3 (-544.35, -202.6717, 38.86989)),
                    new Door("go_cityhall_front_door", new Vector3 (-546.7068, -204.0325, 38.86989))
                },
                false,
                true
            ));
            //enterance left

            _doors.Add(new DoorComplect(
                "city_hall:l_door",
                new Vector3(-556.6215, -228.3017, 38.62147),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-556.1461, -229.1515, 38.6424))
                },
                false,
                true
            ));
            //enterance right

            _doors.Add(new DoorComplect(
                "city_hall:r_door",
                new Vector3(-516.1498, -210.1173, 38.65063),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-515.6904, -210.9494, 38.63948))
                },
                false,
                true
            ));
            //back left

            _doors.Add(new DoorComplect(
                "city_hall:l_backdoor",
                new Vector3(-582.234, -195.1007, 38.64586),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-583.079, -195.5714, 38.66754))
                },
                false,
                true
            ));
            //back right

            _doors.Add(new DoorComplect(
                "city_hall:r_backdoor",
                new Vector3(-534.3331, -167.4282, 38.67284),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-533.4968, -166.9456, 38.67315))
                },
                false,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:l_hall_to_office",
                new Vector3(-570.8184, -216.2693, 38.45129),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-569.896, -215.7366, 38.49482)),
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-571.7305, -216.7957, 38.49482))
                },
                false,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:l_hall_to_c_1",
                new Vector3(-565.9094, -211.4695, 38.47787),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-565.3812, -212.3857, 38.49477)),
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-566.4403, -210.5512, 38.49477))
                },
                false,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:l_hall_to_c_2",
                new Vector3(-574.1949, -197.1255, 38.45304),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-573.666, -198.0387, 38.49554)),
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-574.7253, -196.2038, 38.49554))
                },
                false,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:r_hall_to_office",
                new Vector3(-521.6999, -187.9129, 38.4773),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-520.7775, -187.3761, 38.4945)),
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-522.612, -188.4353, 38.4945))
                },
                false,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:r_hall_to_c_1",
                new Vector3(-528.3085, -189.7687, 38.4646),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-527.7817, -190.6778, 38.49452)),
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-528.8408, -188.8433, 38.49452))
                },
                false,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:r_hall_to_c_2",
                new Vector3(-536.5993, -175.4142, 38.47031),
                new List<Door>
                {
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-536.0673, -176.3308, 38.49388)),
                    new Door("v_ilev_ch_glassdoor", new Vector3 (-537.1258, -174.4973, 38.49388))
                },
                false,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:3_floor_entry",
                new Vector3(-554.6953, -187.4776, 47.45279),
                new List<Door>
                {
                    new Door("go_cityhall_door", new Vector3 (-553.5665, -186.8318, 47.54007)),
                    new Door("go_cityhall_door", new Vector3 (-555.8183, -188.1319, 47.54007))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:3_floor_l_cabinet",
                new Vector3(-539.9923, -185.2584, 47.44903),
                new List<Door>
                {
                    new Door("go_cityhall_door", new Vector3 (-540.6429, -184.1251, 47.54192)),
                    new Door("go_cityhall_door", new Vector3 (-539.3419, -186.3786, 47.54192))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:3_floor_c_cabinet",
                new Vector3(-558.9829, -199.3213, 47.44596),
                new List<Door>
                {
                    new Door("go_cityhall_door", new Vector3 (-557.8593, -198.6742, 47.53978)),
                    new Door("go_cityhall_door", new Vector3 (-560.1111, -199.9744, 47.53978))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:3_floor_r_cabinet",
                new Vector3(-563.9799, -199.1042, 47.46255),
                new List<Door>
                {
                    new Door("go_cityhall_door", new Vector3 (-563.3284, -200.2264, 47.54161)),
                    new Door("go_cityhall_door", new Vector3 (-564.6292, -197.9735, 47.54161))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:3_floor_cloakroom",
                new Vector3(-564.6116, -196.3258, 47.45255),
                new List<Door>
                {
                    new Door("go_cityhall_door", new Vector3 (-563.6326, -195.7587, 47.53984))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "city_hall:3_floor_security",
                new Vector3(-542.0804, -183.326, 47.45291),
                new List<Door>
                {
                    new Door("go_cityhall_door", new Vector3 (-537.9496, -154.9054, 37.54913))
                },
                true,
                true
            ));
            //Parliament Central

            _doors.Add(new DoorComplect(
                "parliament:central:entry",
                new Vector3(2475.128, -384.1369, 94.41839),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3 (2475.127, -385.4296, 93.39353)),
                    new Door("goverment_door_b_light", new Vector3 (2475.127, -382.8382, 93.39359))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "parliament:central:backdoor",
                new Vector3(2460.566, -384.1299, 93.35416),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3 (2460.568, -382.837, 92.32228)),
                    new Door("goverment_door_b_light", new Vector3 (2460.568, -385.4268, 92.32183))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "parliament:central:2_floor_l",
                new Vector3(2463.041, -394.3868, 98.12984),
                new List<Door>
                {
                    new Door("goverment_door_a", new Vector3 (2464.344, -394.4405, 98.14114)),
                    new Door("goverment_door_a", new Vector3 (2461.744, -394.4405, 98.14114))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "parliament:central:2_floor_r",
                new Vector3(2463.046, -373.8229, 98.1114),
                new List<Door>
                {
                    new Door("goverment_door_a", new Vector3 (2461.744, -373.8236, 98.14026)),
                    new Door("goverment_door_a", new Vector3 (2464.344, -373.8236, 98.14026))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "parliament:central:3_floor_l",
                new Vector3(2463.046, -394.44, 103.3304),
                new List<Door>
                {
                    new Door("goverment_door_a", new Vector3 (2464.344, -394.4405, 103.3471)),
                    new Door("goverment_door_a", new Vector3 (2461.744, -394.4405, 103.3471))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "parliament:central:3_floor_r",
                new Vector3(2463.048, -373.8257, 103.317),
                new List<Door>
                {
                    new Door("goverment_door_a", new Vector3 (2461.744, -373.8236, 103.3471)),
                    new Door("goverment_door_a", new Vector3 (2464.344, -373.8236, 103.3471))
                },
                true,
                true
            ));
            //Parliament Left

            _doors.Add(new DoorComplect(
                "parliament:left:entry",
                new Vector3(2519.786, -415.2739, 94.15539),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3 (2520.703, -416.1875, 93.11843)),
                    new Door("goverment_door_b_light", new Vector3 (2518.872, -414.3567, 93.11871))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "parliament:left:security",
                new Vector3(2525.846, -422.2766, 94.23695),
                new List<Door>
                {
                    new Door("v_med_corlowfilecab", new Vector3 (2520.956, -424.406, 93.1166))
                },
                true,
                true
            ));
            //Parliament Left

            _doors.Add(new DoorComplect(
                "parliament:right:entry",
                new Vector3(2519.797, -352.9724, 94.16727),
                new List<Door>
                {
                    new Door("goverment_door_b_light", new Vector3 (2518.871, 353.8813, 93.12227)),
                    new Door("goverment_door_b_light", new Vector3 (2520.703, -352.049, 93.12145))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "parliament:right:security",
                new Vector3(2525.836, -345.9754, 94.24054),
                new List<Door>
                {
                    new Door("v_med_corlowfilecab", new Vector3 (2526.725, -345.0884, 94.26785))
                },
                true,
                true
            ));

            //Mafia 5
            _doors.Add(new DoorComplect(
                "mafia_5:gate",
                new Vector3(-2557.934, 1913.297, 169.1261),
                new List<Door>
                {
                    new Door("prop_lrggate_01c_l", new Vector3(-2559.193, 1910.86, 169.0709)),
                    new Door("prop_lrggate_01c_r", new Vector3(-2556.658, 1915.716, 169.0709))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:garage",
                new Vector3(-2597.355, 1926.585, 167.3985),
                new List<Door>
                {
                    new Door("mafia_5_garage_door", new Vector3(-2597.095, 1926.618, 167.6456))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:main_entry",
                new Vector3(-2588.216, 1910.298, 167.541),
                new List<Door>
                {
                    new Door("go_apa_p_mp_door_apartfrt_door", new Vector3(-2587.021, 1910.384, 167.65))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:back_entry",
                new Vector3(-2598.739, 1879.758, 167.3272),
                new List<Door>
                {
                    new Door("go_door_5_mafia", new Vector3(-2599.58, 1880.748, 167.4572)),
                    new Door("go_door_5_mafia", new Vector3(-2597.883, 1878.775, 167.4572))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:ground_floor_entry",
                new Vector3(-2602.333, 1894.379, 163.7202),
                new List<Door>
                {
                    new Door("go_door_5_mafia", new Vector3(-2601.041, 1894.477, 163.8858)),
                    new Door("go_door_5_mafia", new Vector3(-2603.628, 1894.251, 163.8858))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:stock:1",
                new Vector3(-2603.129, 1916.775, 163.5483),
                new List<Door>
                {
                    new Door("mafia_5_cagedoor", new Vector3(-2604.263, 1916.66, 163.6972))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:stock:2",
                new Vector3(-2593.101, 1919.916, 160.3624),
                new List<Door>
                {
                    new Door("ch_prop_ch_service_door_01a", new Vector3(-2592.993, 1918.703, 160.5166))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:hall",
                new Vector3(-2589.865, 1908.842, 163.8542),
                new List<Door>
                {
                    new Door("apa_heist_apart2_door", new Vector3(-2589.756, 1907.648, 163.8802))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:small_room",
                new Vector3(-2583.092, 1905.534, 163.8475),
                new List<Door>
                {
                    new Door("apa_heist_apart2_door", new Vector3(-2582.991, 1904.35, 163.8802))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:billiards:1",
                new Vector3(-2591.499, 1904.449, 163.8755),
                new List<Door>
                {
                    new Door("apa_heist_apart2_door", new Vector3(-2591.606, 1905.647, 163.8802))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:billiards:2",
                new Vector3(-2593.032, 1895.231, 163.8694),
                new List<Door>
                {
                    new Door("apa_heist_apart2_door", new Vector3(-2591.827, 1895.336, 163.8802))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_5:bedroom",
                new Vector3(-2596.686, 1892.687, 163.8355),
                new List<Door>
                {
                    new Door("apa_heist_apart2_door", new Vector3(-2596.791, 1893.877, 163.8802))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "home1:gate_1",
                new Vector3(-1453.795, -32.1254, 55.52244),
                new List<Door>
                {
                    new Door("prop_lrggate_01_l", new Vector3(-1452.903, -29.55032, 54.86719)),
                    new Door("prop_lrggate_01_r", new Vector3(-1454.664, -34.73074, 54.86434))
                },
                true,
                true
            )); ;
            _doors.Add(new DoorComplect(
                "home1:gate_2",
                new Vector3(-1472.688, -14.25324, 55.29909),
                new List<Door>
                {
                    new Door("prop_lrggate_01_l", new Vector3(-1475.353, -14.71867, 54.89672)),
                    new Door("prop_lrggate_01_r", new Vector3(-1469.96, -13.79652, 54.89387))
                },
                true,
                true
            ));
            //Mafia 2
            _doors.Add(new DoorComplect(
                "mafia_2:entry_1",
                new Vector3(-921.7756, 814.3923, 184.4897),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apartfrt_door_black", new Vector3(-920.5856, 814.4665, 184.5428))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:entry_2",
                new Vector3(-931.3074, 825.8658, 184.5068),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-930.2101, 825.948, 184.6073)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-932.4106, 825.7693, 184.6073))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:entry_3",
                new Vector3(-938.8608, 815.0391, 184.9263),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apartfrt_door_black", new Vector3(-938.7649, 813.8371, 184.9711))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:entry_4",
                new Vector3(-932.4454, 809.4331, 184.9297),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apartfrt_door_black", new Vector3(-931.2332, 809.5399, 184.9711))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:kitchen",
                new Vector3(-923.4094, 820.3359, 184.5083),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apart_door_black", new Vector3(-923.5196, 821.5334, 184.5414))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:laundry",
                new Vector3(-927.4034, 818.8119, 184.5592),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apart_door_black", new Vector3(-926.2137, 818.9183, 184.5414))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:restroom",
                new Vector3(-931.7354, 818.4386, 184.5275),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apart_door_black", new Vector3(-930.5425, 818.5429, 184.5468))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:gym",
                new Vector3(-933.699, 821.3365, 184.4584),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apart_door_black", new Vector3(-933.8157, 822.5195, 184.5492))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:stairs",
                new Vector3(-936.2279, 818.095, 184.9087),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apart_door_black", new Vector3(-937.4615, 817.9963, 184.9641))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:balcony_1_1",
                new Vector3(-925.2843, 814.0744, 187.9553),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-924.1852, 814.1672, 188.0746)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-926.3856, 813.986, 188.0756))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:balcony_1_2",
                new Vector3(-930.6882, 813.6469, 187.955),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-929.5876, 813.7196, 188.0746)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-931.79, 813.5666, 188.0756))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:entertainment",
                new Vector3(-923.0703, 817.7283, 188.0405),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apart_door_black", new Vector3(-922.9713, 816.53, 188.0207))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:hall",
                new Vector3(-929.2888, 819.898, 188.001),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apart_door_black", new Vector3(-928.0924, 820.0014, 188.0183)),
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:balcony_2",
                new Vector3(-931.3159, 825.924, 187.9559),
                new List<Door>
                {
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-932.415, 825.8253, 188.0756)),
                    new Door("prop_bh1_48_backdoor_r", new Vector3(-930.2152, 826.0037, 188.0746))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "mafia_2:bedroom",
                new Vector3(-933.7811, 823.5698, 188.0093),
                new List<Door>
                {
                    new Door("apa_p_mp_door_apart_door_black", new Vector3(-933.6738, 822.3608, 188.0199))

                },
                true,
                true
            ));
            //demorgan
            _doors.Add(new DoorComplect(
                "prison:main_entrance_1",
                new Vector3(1845.006, 2611.645, 45.86983),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1844.998, 2604.813, 44.63978))
                },
                false,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:main_entrance_2",
                new Vector3(1818.566, 2611.605, 45.88353),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1818.543, 2604.813, 44.611))
                },
                false,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:main_entrance_r",
                new Vector3(1792.784, 2616.968, 45.84258),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1799.608, 2616.975, 44.60325))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:main_entrance_l_1",
                new Vector3(1796.365, 2596.545, 46.27844),
                new List<Door>
                {
                    new Door("prop_fnclink_03gate5", new Vector3(1797.761, 2596.565, 46.38731))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:main_entrance_l_2",
                new Vector3(1796.695, 2591.715, 46.30163),
                new List<Door>
                {
                    new Door("prop_fnclink_03gate5", new Vector3(1798.09, 2591.687, 46.41784))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:main_entrance_small_1",
                new Vector3(1845.413, 2586.324, 45.93238),
                new List<Door>
                {
                    new Door("v_ilev_ph_door002", new Vector3(1845.402, 2585.239, 46.08133))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:main_entrance_small_2",
                new Vector3(1819.238, 2594.772, 46.06484),
                new List<Door>
                {
                    new Door("v_ilev_ph_door002", new Vector3(1819.224, 2593.681, 46.15534))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:main_entrance_small_3",
                new Vector3(1791.174, 2593.305, 46.0065),
                new List<Door>
                {
                    new Door("v_ilev_ph_door002", new Vector3(1791.161, 2594.368, 46.1037))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:main_entrance_small_4",
                new Vector3(1765.28, 2566.641, 45.65059),
                new List<Door>
                {
                    new Door("v_ilev_ph_door002", new Vector3(1766.314, 2566.634, 45.75524))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:jail:1",
                new Vector3(1690.18, 2590.686, 45.98945),
                new List<Door>
                {
                    new Door("v_ilev_ph_door002", new Vector3(1691.26, 2590.691, 46.08984))
                },
                true,
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:jail:2",
                new Vector3(1691.095, 2566.796, 45.63204),
                new List<Door>
                {
                    new Door("v_ilev_ph_door002", new Vector3(1692.177, 2566.795, 45.73507))
                },
                true,
                true
            ));

            _doors.Add(new DoorComplect(
                "prison:jail:3",
                new Vector3(1686.676, 2573.152, 50.7065),
                new List<Door>
                {
                    new Door("v_ilev_ph_door002", new Vector3(1687.682, 2573.136, 50.80465))
                },
                true,
                true
            ));

            //monzo
            _doors.Add(new DoorComplect(
              "monzo:1",
              new Vector3(-1129.15, 388.766, 70.76119),
              new List<Door>
              {
                    new Door("bh1_47_gate_2", new Vector3 (-1126.068, 388.979, 69.76716))
              },
              true,
              true
          ));
          _doors.Add(new DoorComplect(
               "monzo:2",
               new Vector3(-1091.787, 369.8571, 68.71388),
               new List<Door>
               {
                    new Door("bh1_47_gate_1", new Vector3 (-1091.374, 366.653, 67.70918))
               },
               true,
               true
           ));

            ///not interracted doors 
            _doors.Add(new DoorComplect(
               "police:vinewood:kpz",
               new Vector3(564.6141, 12.77213, 83.10648),
               new List<Door>
               {
                    new Door("police_grate", new Vector3 (565.6828,12.34088,83.06334))
               },
               true
           ));
            _doors.Add(new DoorComplect(
                "bank:pacific:safe",
                new Vector3(255.2283, 223.976, 102.3932),
                new List<Door>
                {
                    new Door(961976194, new Vector3(255.2283, 223.976, 102.3932))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "bank:pacific:door:main",
                new Vector3(232.6054, 214.1584, 106.4049),
                new List<Door>
                {
                    new Door("hei_prop_hei_bankdoor_new", new Vector3(232.6054, 214.1584, 106.4049)),
                    new Door("hei_prop_hei_bankdoor_new", new Vector3(231.5075, 216.5148, 106.4049))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "gunshop:morningwood:door",
                new Vector3(-1313.826, -389.1259, 36.84573),
                new List<Door>
                {
                    new Door("v_ilev_gc_door03", new Vector3(-1314.465, -391.6472, 36.84573)),
                    new Door("v_ilev_gc_door04", new Vector3(-1313.826, -389.1259, 36.84573))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "clothshop:takestylesity:door",
                new Vector3(418.5713, -806.3979, 29.64108),
                new List<Door>
                {
                    new Door("v_ilev_cs_door01_r", new Vector3(418.5713, -808.674, 29.64108)),
                    new Door("v_ilev_cs_door01", new Vector3(418.5713, -806.3979, 29.64108))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "army:dock:gate:1",
                new Vector3(-187.3406, -2515.309, 5.047173),
                new List<Door>
                {
                    new Door("prop_gate_docks_ld", new Vector3(-202.6151, -2515.309, 5.047173)),
                    new Door("prop_gate_docks_ld", new Vector3(-187.3406, -2515.309, 5.047173))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "lscustom:berton:door",
                new Vector3(-356.0905, -134.7714, 40.01295),
                new List<Door>
                {
                    new Door("prop_com_ls_door_01", new Vector3(-356.0905, -134.7714, 40.01295))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "lscustom:2:door",
                new Vector3(723.116, -1088.831, 23.83201),
                new List<Door>
                {
                    new Door("prop_id2_11_gdoor", new Vector3(723.116, -1088.831, 23.23201))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "lscustom:3:door",
                new Vector3(-205.6828, -1310.683, 30.8957),
                new List<Door>
                {
                    new Door("lr_prop_supermod_door_01", new Vector3(-205.6828, -1310.683, 30.29572))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "fib:door:faer:close",
                new Vector3(-205.6828, -1310.683, 30.8957),
                new List<Door>
                {
                    new Door("v_ilev_fib_door1", new Vector3(116.6982, -735.3205, 258.3023)),
                    new Door("v_ilev_fib_door1", new Vector3(119.1414, -736.2098, 258.3023))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/1",
                new Vector3(1835.285, 2689.104, 44.4467),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1835.285, 2689.104, 44.4467))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/1",
                new Vector3(1830.134, 2703.499, 44.4467),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1830.134, 2703.499, 44.4467))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/2",
                new Vector3(1776.701, 2747.148, 44.44669),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1776.701, 2747.148, 44.44669))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/2",
                new Vector3(1762.196, 2752.489, 44.44669),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1762.196, 2752.489, 44.44669))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/3",
                new Vector3(1662.011, 2748.703, 44.44669),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1662.011, 2748.703, 44.44669))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/3",
                new Vector3(1648.411, 2741.668, 44.44669),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1648.411, 2741.668, 44.44669))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/4",
                new Vector3(1584.653, 2679.75, 44.50947),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1584.653, 2679.75, 44.50947))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/4",
                new Vector3(1575.719, 2667.152, 44.50947),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1575.719, 2667.152, 44.50947))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/5",
                new Vector3(1547.706, 2591.282, 44.50947),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1547.706, 2591.282, 44.50947))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/5",
                new Vector3(1546.983, 2576.13, 44.39033),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1546.983, 2576.13, 44.39033))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/6",
                new Vector3(1550.93, 2482.743, 44.39529),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1550.93, 2482.743, 44.39529))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/6",
                new Vector3(1558.221, 2469.349, 44.39529),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1558.221, 2469.349, 44.39529))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/7",
                new Vector3(1652.984, 2409.571, 44.44308),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1652.984, 2409.571, 44.44308))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/7",
                new Vector3(1667.669, 2407.648, 44.42879),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1667.669, 2407.648, 44.42879))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/8",
                new Vector3(1749.142, 2419.812, 44.42517),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1749.142, 2419.812, 44.42517))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/8",
                new Vector3(1762.542, 2426.507, 44.43787),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1762.542, 2426.507, 44.43787))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_1/9",
                new Vector3(1808.992, 2474.545, 44.48077),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1808.992, 2474.545, 44.48077))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:big_dors_2/9",
                new Vector3(1813.749, 2488.907, 44.46368),
                new List<Door>
                {
                    new Door("prop_gate_prison_01", new Vector3(1813.749, 2488.907, 44.46368))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:1",
                new Vector3(1820.77, 2620.77, 45.95126),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1820.77, 2620.77, 45.95126))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:2",
                new Vector3(1845.79, 2698.621, 45.95531),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1845.79, 2698.621, 45.95531))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:3",
                new Vector3(1773.108, 2759.7, 45.88673),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1773.108, 2759.7, 45.88673))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:4",
                new Vector3(1651.161, 2755.436, 45.87868),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1651.161, 2755.436, 45.87868))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:5",
                new Vector3(1572.662, 2679.191, 45.72976),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1572.662, 2679.191, 45.72976))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:6",
                new Vector3(1537.811, 2585.995, 45.68915),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1537.811, 2585.995, 45.68915))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:7",
                new Vector3(1543.241, 2471.294, 45.71201),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1543.241, 2471.294, 45.71201))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:8",
                new Vector3(1658.584, 2397.722, 45.71526),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1658.584, 2397.722, 45.71526))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:9",
                new Vector3(1759.62, 2412.837, 45.71166),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1759.62, 2412.837, 45.71166))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "prison:tower:10",
                new Vector3(1821.17, 2476.265, 45.68915),
                new List<Door>
                {
                    new Door("v_ilev_gtdoor", new Vector3(1821.17, 2476.265, 45.68915))
                },
                true
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:2_right_1",
                new Vector3(311.119, -576.9609, 82.66291),
                new List<Door>
                {
                new Door("v_ilev_hosp_door_white", new Vector3(312.164, -577.3336, 82.73534))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:2_right_2",
                new Vector3(320.1232, -580.2488, 82.68005),
                new List<Door>
                {
                    new Door("v_ilev_hosp_door_white", new Vector3(319.0817, -579.8702, 82.73534))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:2_left_1",
                new Vector3(313.226, -571.1711, 82.64675),
                new List<Door>
                {
                    new Door("v_ilev_hosp_door_white", new Vector3(314.2674, -571.5482, 82.73534))
                },
                false
            ));
            _doors.Add(new DoorComplect(
                "hospital:vinewood:2_left_2",
                new Vector3(322.235, -574.4492, 82.66537),
                new List<Door>
                {
                    new Door("v_ilev_hosp_door_white", new Vector3(321.1916, -574.0684, 82.73534))
                },
                false
            ));

            InitInterractDoors();
        }

        private void InitInterractDoors()
        {
            _interractedDoors = _doors.Where(d => d.Interract).ToList();
        }

        public DoorComplect this[int hash]
        {
            get
            {
                return _interractedDoors.FirstOrDefault(d => d.Hash == hash);
            }
        }
        public string Serialize()
        {
            return JsonConvert.SerializeObject(_doors, Formatting.Indented);
        }
    }
}
