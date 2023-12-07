using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.MP.Arena.Enums;

namespace Whistler.MP.Arena.Locations
{
    internal static class ArenaLocationFactory
    {
        public static ArenaLocation CreateLocation(LocationName name)
        {
            return name switch
            {
                LocationName.Camp => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(-1148.713, 4949.869, 226.2277), 146),
                        new ArenaLocationSpawn(new Vector3(-1170.64, 4918.087, 222.3038), 270),
                        new ArenaLocationSpawn(new Vector3(-1143.703, 4895.347, 218.4503), 291),
                        new ArenaLocationSpawn(new Vector3(-1123.336, 4893.318, 218.4725), 27),
                        new ArenaLocationSpawn(new Vector3(-1111.093, 4904.887, 218.5953), 317),
                        new ArenaLocationSpawn(new Vector3(-1094.505, 4885.03, 215.7033), 302),
                        new ArenaLocationSpawn(new Vector3(-1069.048, 4894.081, 214.2714), 323),
                        new ArenaLocationSpawn(new Vector3(-1109.877, 4901.802, 218.5953), 296),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(-1054.898, 4917.905, 211.8189), 134.1327f),
                        new ArenaLocationSpawn(new Vector3(-1066.491, 4969.041, 209.6155), 159.3521f),
                        new ArenaLocationSpawn(new Vector3(-1078.74, 4936.112, 229.2217), 171.4388f),
                        new ArenaLocationSpawn(new Vector3(-1102.346, 4931.562, 218.3542), 204.5629f),
                        new ArenaLocationSpawn(new Vector3(-1102.244, 4958.443, 218.4277), 90.71169f),
                        new ArenaLocationSpawn(new Vector3(-1131.378, 4952.716, 222.2686), 150.0183f),
                        new ArenaLocationSpawn(new Vector3(-1142.742, 4950.855, 229.9885), 213.1697f),
                    }),
                LocationName.Recycling => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(2405.739, 3139.364, 47.07384), 68.8214f),
                        new ArenaLocationSpawn(new Vector3(2405.133, 3158.011, 47.10466), 189.4845f),
                        new ArenaLocationSpawn(new Vector3(2420.454, 3157.416, 48.21704), 177.2508f),
                        new ArenaLocationSpawn(new Vector3(2427.502, 3154.979, 47.06195), 68.6447f),
                        new ArenaLocationSpawn(new Vector3(2432.598, 3159.487, 47.07949), 168.5167f),
                        new ArenaLocationSpawn(new Vector3(2418.538, 3139.702, 47.04672), 135.6249f),
                        new ArenaLocationSpawn(new Vector3(2389.998, 3116.706, 47.05425), 129.5065f),
                        new ArenaLocationSpawn(new Vector3(2385.568, 3105.928, 47.03941), 13.55968f),
                        new ArenaLocationSpawn(new Vector3(2399.615, 3108.323, 47.05254), 39.76145f),
                        new ArenaLocationSpawn(new Vector3(2393.93, 3136.717, 50.91688), 150.0079f),
                        new ArenaLocationSpawn(new Vector3(2402.908, 3130.774, 50.92047), 150.7706f),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(2429.105, 3115.554, 47.0868), 105.0147f),
                        new ArenaLocationSpawn(new Vector3(2421.47, 3102.594, 47.03301), 104.6471f),
                        new ArenaLocationSpawn(new Vector3(2411.665, 3094.72, 47.03291), 74.98725f),
                        new ArenaLocationSpawn(new Vector3(2410.486, 3081.219, 47.0327), 26.92913f),
                        new ArenaLocationSpawn(new Vector3(2412.452, 3065.329, 47.0325), 37.71567f),
                        new ArenaLocationSpawn(new Vector3(2412.221, 3052.431, 47.0563), 37.25094f),
                        new ArenaLocationSpawn(new Vector3(2406.994, 3039.167, 47.0326), 100.4237f),
                        new ArenaLocationSpawn(new Vector3(2400.97, 3028.732, 47.03262), 0.8076814f),
                        new ArenaLocationSpawn(new Vector3(2429.579, 3123.112, 47.1150), 36.26935f),
                        new ArenaLocationSpawn(new Vector3(2425.996, 3134.576, 47.0250), 100.2117f),
                        new ArenaLocationSpawn(new Vector3(2415.776, 3130.786, 47.0382), 86.97387f),
                    }),
                LocationName.Sawmill => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(-467.0632, 5346.202, 80.70316), 90.90413f),
                        new ArenaLocationSpawn(new Vector3(-502.1601, 5374.341, 75.69307), 155.9113f),
                        new ArenaLocationSpawn(new Vector3(-545.7741, 5387.44, 70.223843), 152.1743f),
                        new ArenaLocationSpawn(new Vector3(-524.0469, 5357.327, 76.1412), 52.1739f),
                        new ArenaLocationSpawn(new Vector3(-516.2209, 5332.999, 80.26272), 252.4812f),
                        new ArenaLocationSpawn(new Vector3(-547.1492, 5311.057, 81.68192), 245.2054f),
                        new ArenaLocationSpawn(new Vector3(-567.7767, 5291.606, 75.40733), 131.629f),
                        new ArenaLocationSpawn(new Vector3(-521.1616, 5307.827, 80.23955), 208.822f),
                        new ArenaLocationSpawn(new Vector3(-539.2434, 5336.775, 88.95999), 224.2484f),
                        new ArenaLocationSpawn(new Vector3(-522.2786, 5308.303, 74.18338), 221.668f),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(-593.3793, 5362.666, 70.35631), 211.3326f),
                        new ArenaLocationSpawn(new Vector3(-571.7941, 5245.092, 70.48751), 24.57754f),
                        new ArenaLocationSpawn(new Vector3(-588.5139, 5347.036, 70.2876), 217.1863f),
                        new ArenaLocationSpawn(new Vector3(-548.3045, 5276.634, 74.06861), 347.5703f),
                        new ArenaLocationSpawn(new Vector3(-532.2936, 5267.404, 74.19836), 32.01464f),
                        new ArenaLocationSpawn(new Vector3(-500.89, 5233.085, 83.4381), 31.38487f),
                        new ArenaLocationSpawn(new Vector3(-503.6205, 5259.079, 80.61019), 342.5041f),
                        new ArenaLocationSpawn(new Vector3(-507.412, 5284.356, 80.51682), 68.90935f),
                        new ArenaLocationSpawn(new Vector3(-476.9981, 5300.124, 86.69825), 99.83284f),
                        new ArenaLocationSpawn(new Vector3(-494.4136, 5325.422, 80.56849), 99.83221f),
                    }),
                LocationName.Island => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(3708.681, 4914.662, 17.45335), 304.882f),
                        new ArenaLocationSpawn(new Vector3(3706.964, 4920.972, 17.03082), 15.3816f),
                        new ArenaLocationSpawn(new Vector3(3699.413, 4920.562, 16.65511), 41.0864f),
                        new ArenaLocationSpawn(new Vector3(3699.294, 4912.136, 17.48314), 359.116f),
                        new ArenaLocationSpawn(new Vector3(3688.846, 4918.37, 14.87225), 31.19217f),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(3623.611, 5037.826, 10.01625), 219.442f),
                        new ArenaLocationSpawn(new Vector3(3614.971, 5034.246, 9.870264), 290.2921f),
                        new ArenaLocationSpawn(new Vector3(3617.465, 5042.604, 9.132493), 235.9478f),
                        new ArenaLocationSpawn(new Vector3(3613.158, 5039.989, 8.891453), 215.7152f),
                        new ArenaLocationSpawn(new Vector3(3626.205, 5036.433, 10.38683), 209.4373f),
                    }),
                LocationName.Yacht => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(-2030.833, -1039.536, 4.762953), 67.6687f),
                        new ArenaLocationSpawn(new Vector3(-2033.393, -1035.134, 4.762358), 67.5391f),
                        new ArenaLocationSpawn(new Vector3(-2028.943, -1032.205, 4.762748), 66.9873f),
                        new ArenaLocationSpawn(new Vector3(-2021.864, -1031.923, 4.762022), 66.4045f),
                        new ArenaLocationSpawn(new Vector3(-2025.861, -1044.017, 4.762022), 68.9233f),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(-2110.971, -1009.814, 7.847294), 247.6564f),
                        new ArenaLocationSpawn(new Vector3(-2109.045, -1004.789, 7.851554), 249.169f),
                        new ArenaLocationSpawn(new Vector3(-2113.662, -1013.904, 7.851608), 245.3689f),
                        new ArenaLocationSpawn(new Vector3(-2101.829, -1012.043, 7.849493), 239.2683f),
                        new ArenaLocationSpawn(new Vector3(-2105.657, -1011.239, 8.882086), 245.8256f),
                    }),
                LocationName.Port => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(233.746, -3297.94, 39.41832), 353.9833f),
                        new ArenaLocationSpawn(new Vector3(227.1772, -3296.582, 39.41886), 354.8466f),
                        new ArenaLocationSpawn(new Vector3(220.015, -3296.105, 39.41946), 357.7816f),
                        new ArenaLocationSpawn(new Vector3(221.0542, -3306.368, 39.41452), 352.8464f),
                        new ArenaLocationSpawn(new Vector3(232.4302, -3306.795, 39.41505), 352.4738f),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(219.4642, -3149.084, 39.40844), 173.0123f),
                        new ArenaLocationSpawn(new Vector3(227.9456, -3150.886, 39.40844), 176.7353f),
                        new ArenaLocationSpawn(new Vector3(234.7403, -3151.505, 39.41422), 182.3722f),
                        new ArenaLocationSpawn(new Vector3(228.317, -3142.054, 39.41556), 177.5888f),
                        new ArenaLocationSpawn(new Vector3(222.4531, -3142.145, 39.41477), 169.6051f)
                    }),
                LocationName.Warship => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(3033.871, -4675.235, 4.957288), 192.9419f),
                        new ArenaLocationSpawn(new Vector3(3036.497, -4674.588, 4.957288), 192.5211f),
                        new ArenaLocationSpawn(new Vector3(3036.822, -4677.945, 4.957288), 180.1605f),
                        new ArenaLocationSpawn(new Vector3(3033.43, -4679.121, 4.957289), 183.6463f),
                        new ArenaLocationSpawn(new Vector3(3035.641, -4680.403, 4.957289), 198.2968f),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(3097.782, -4800.278, 5.957409), 120.0919f),
                        new ArenaLocationSpawn(new Vector3(3096.113, -4801.733, 5.957761), 103.8383f),
                        new ArenaLocationSpawn(new Vector3(3093.35, -4801.666, 5.958769), 03.7008f),
                        new ArenaLocationSpawn(new Vector3(3091.551, -4803.386, 5.959264), 105.082f),
                        new ArenaLocationSpawn(new Vector3(3089.697, -4803.335, 5.959953), 98.91254f),
                    }),
                LocationName.Fib => new ArenaLocation(
                    
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(149.9776, -740.2229, 253.0322), 171.3861f),
                        new ArenaLocationSpawn(new Vector3(149.1872, -744.5004, 253.0321), 196.603f),
                        new ArenaLocationSpawn(new Vector3(13.2444, -742.5928, 253.0321), 141.7257f),
                        new ArenaLocationSpawn(new Vector3(152.4497, -745.1243, 253.0321), 141.9489f),
                        new ArenaLocationSpawn(new Vector3(151.2541, -742.7468, 253.0321), 173.9234f)
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(120.4721, -724.9496, 253.0321), 196f),
                        new ArenaLocationSpawn(new Vector3(123.192, -726.2859, 253.0321), 154f),
                        new ArenaLocationSpawn(new Vector3(120.7209, -727.7587, 253.0321), 165),
                        new ArenaLocationSpawn(new Vector3(121.0861, -731.0858, 253.0321), 170f),
                        new ArenaLocationSpawn(new Vector3(118.3784, -730.0007, 253.0321), 164),
                    }),
                LocationName.Helipad => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(-76.90739, -812.1672, 326.175), 180.8043f),
                        new ArenaLocationSpawn(new Vector3(-70.23528, -814.071, 326.1751), 146.4767f),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(-73.32412, -825.5472, 326.1755), 13.3649f),
                        new ArenaLocationSpawn(new Vector3(-80.17666, -823.9624, 326.1754), 324.340f),
                    }),
                LocationName.Bridge => new ArenaLocation(
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(180.4472, -2466.257, 19.74201), 284.044f),
                        new ArenaLocationSpawn(new Vector3(182.7372, -2473.244, 19.74201), 278.367f),
                    },
                    new List<ArenaLocationSpawn>
                    {
                        new ArenaLocationSpawn(new Vector3(191.5889, -2463.341, 19.742), 91.60532f),
                        new ArenaLocationSpawn(new Vector3(193.8777, -2470.512, 19.742), 101.7675f),
                    }),
                _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
            };
        }
    }
}