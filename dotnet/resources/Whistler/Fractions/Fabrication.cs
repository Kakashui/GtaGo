using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using System.Data;
using Whistler.VehicleSystem;

namespace Whistler.Fractions
{
    class AlcoFabrication //: Script
    {
        /*
            {10, "La Cosa Nostra" },
            {11, "Russian Mafia" },
            {12, "Yakuza" },
            {13, "Armenian Mafia" },
        */
        //private static WhistlerLogger _logger = new WhistlerLogger("AlcoFabrication");
        //private static Dictionary<int, Vector3> EnterAlcoShop = new Dictionary<int, Vector3>()
        //{
        //    //{ 10, new Vector3(-1388.761, -586.3921, 29.09945) },
        //    { 12, new Vector3(-564.5512, 275.6993, 82.05249) },
        //    //{ 13, new Vector3(-430.1028, 261.2774, 81.88689) },
        //};
        //private static Dictionary<int, Vector3> ExitAlcoShop = new Dictionary<int, Vector3>()
        //{
        //    //{ 10, new Vector3(-1387.458, -588.3003, 29.19951) },
        //    { 12, new Vector3(-564.487, 277.4747, 82.14633) },
        //    //{ 13, new Vector3(380.9767, -1001.358, -100.12004) },
        //};
        //private static Dictionary<int, string> ClubsNames = new Dictionary<int, string>()
        //{
        //    { 10, "Bahama Mamas West" },
        //    { 11, "Vanila Unicorn" },
        //    { 12, "Tequi-la-la" },
        //    { 13, "Split Sides West Comedy Club" },
        //};

        //public static Dictionary<int, Stock> ClubsStocks = new Dictionary<int, Stock>();
        //private static int MaxMats = 4000;
        //private static Dictionary<int, Vector3> UnloadPoints = new Dictionary<int, Vector3>()
        //{
        //    { 10, new Vector3(-1404.037, -633.443, 27.68337) },
        //    { 11, new Vector3(141.3792, -1281.576, 28.4372) },
        //    { 12, new Vector3(-564.1531, 302.2027, 82.258) },
        //    { 13, new Vector3(-452.4567, 290.8813, 82.233) },
        //};
        //private static Dictionary<int, Vector3> BuyPoints = new Dictionary<int, Vector3>()
        //{
        //    { 10, new Vector3(-1394.523, -602.7082, 29.31955) },
        //    { 11, new Vector3(126.4378, -1282.892, 28.27888) },
        //    { 12, new Vector3(-560.0757, 286.7839, 81.17632) },
        //    { 13, new Vector3(-435.964, 274.2971, 82.30211) },
        //};

        //private static List<int> DrinksPrices = new List<int>() { 75, 115, 150 };
        //private static List<int> DrinksMats = new List<int>() { 5, 7, 10 };
        //private static Dictionary<int, List<ItemType>> DrinksInClubs = new Dictionary<int, List<ItemType>>()
        //{
        //    { 10, new List<ItemType>(){ItemType.LcnDrink1, ItemType.LcnDrink2, ItemType.LcnDrink3} },
        //    { 11, new List<ItemType>(){ItemType.RusDrink1, ItemType.RusDrink2, ItemType.RusDrink3} },
        //    { 12, new List<ItemType>(){ItemType.YakDrink1, ItemType.YakDrink2, ItemType.YakDrink3} },
        //    { 13, new List<ItemType>(){ItemType.ArmDrink1, ItemType.ArmDrink2, ItemType.ArmDrink3} },
        //};

        //public static Dictionary<ItemType, Vector3> AlcoPosOffset = new Dictionary<ItemType, Vector3>()
        //{
        //    { ItemType.LcnDrink1, new Vector3(0.15, -0.25, -0.1) },
        //    { ItemType.LcnDrink2, new Vector3(0.15, -0.25, -0.1) },
        //    { ItemType.LcnDrink3, new Vector3(0.15, -0.23, -0.1) },
        //    { ItemType.RusDrink1, new Vector3(0.15, -0.23, -0.1) },
        //    { ItemType.RusDrink2, new Vector3(0.15, -0.23, -0.1) },
        //    { ItemType.RusDrink3, new Vector3(0.15, -0.23, -0.1) },
        //    { ItemType.YakDrink1, new Vector3(0.12, -0.02, -0.03) },
        //    { ItemType.YakDrink2, new Vector3(0.15, -0.23, -0.10) },
        //    { ItemType.YakDrink3, new Vector3(0.15, 0.03, -0.06) },
        //    { ItemType.ArmDrink1, new Vector3(0.15, -0.18, -0.10) },
        //    { ItemType.ArmDrink2, new Vector3(0.15, -0.18, -0.10) },
        //    { ItemType.ArmDrink3, new Vector3(0.15, -0.18, -0.10) },
        //};
        //public static Dictionary<ItemType, Vector3> AlcoRotOffset = new Dictionary<ItemType, Vector3>()
        //{
        //    { ItemType.LcnDrink1, new Vector3(-80, 0, 0) },
        //    { ItemType.LcnDrink2, new Vector3(-80, 0, 0) },
        //    { ItemType.LcnDrink3, new Vector3(-80, 0, 0) },
        //    { ItemType.RusDrink1, new Vector3(-80, 0, 0) },
        //    { ItemType.RusDrink2, new Vector3(-80, 0, 0) },
        //    { ItemType.RusDrink3, new Vector3(-80, 0, 0) },
        //    { ItemType.YakDrink1, new Vector3(-80, 0, 0) },
        //    { ItemType.YakDrink2, new Vector3(-80, 0, 0) },
        //    { ItemType.YakDrink3, new Vector3(-80, 0, 0) },
        //    { ItemType.ArmDrink1, new Vector3(-80, 0, 0) },
        //    { ItemType.ArmDrink2, new Vector3(-80, 0, 0) },
        //    { ItemType.ArmDrink3, new Vector3(-80, 0, 0) },
        //};

        //[ServerEvent(Event.ResourceStart)]
        //public void Event_ResourceStart()
        //{
        //    try
        //    {
        //        //NAPI.World.DeleteWorldProp(NAPI.Util.GetHashKey("prop_strip_door_01"), new Vector3(127.9552, -1298.503, 29.41962), 30f); //X:127,9552 Y:-1298,503 Z:29,41962

        //        ////NAPI.Object.CreateObject(NAPI.Util.GetHashKey("v_ilev_ph_gendoor006"), new Vector3(-1386.99683, -586.663208, 30.4694996), new Vector3(0, 0, 33.9277153), 255, NAPI.GlobalDimension);
        //        ////NAPI.Object.CreateObject(NAPI.Util.GetHashKey("v_ilev_ph_gendoor006"), new Vector3(-1389.17236, -588.086914, 30.4694996), new Vector3(0, -0, -147.719879), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1397.15088, -598.213379, 29.3224068), new Vector3(0, 0, -18.1152821), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1397.08069, -600.813477, 29.3224068), new Vector3(0, -0, -138.115219), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1399.99353, -600.623291, 29.3224068), new Vector3(0, -0, 119.884583), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1401.09326, -601.223145, 29.3224068), new Vector3(0, 0, -24.1148987), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1399.75366, -602.2229, 29.3224068), new Vector3(0, 0, -41.9143143), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1399.51343, -604.222656, 29.3224068), new Vector3(0, -0, -94.9140701), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1401.00488, -606.364746, 29.3224068), new Vector3(0, -0, -161.913498), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1403.46729, -604.663086, 29.3224068), new Vector3(0, -0, 129.486343), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1404.37708, -603.463379, 29.3224068), new Vector3(0, -0, 124.486282), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1400.23792, -610.231201, 29.3224068), new Vector3(0, 0, 55.4857788), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1397.53857, -613.230469, 29.3224068), new Vector3(0, -0, -174.514252), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1395.72827, -611.990723, 29.3224068), new Vector3(0, -0, -104.513588), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1396.07861, -609.95874, 29.3224068), new Vector3(0, 0, -55.5132561), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1397.49976, -608.927734, 29.3224068), new Vector3(0, 0, -20.513237), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("apa_mp_h_stn_chairarm_03"), new Vector3(-1396.10718, -615.662598, 29.3224068), new Vector3(0, -0, -127.513763), 255, NAPI.GlobalDimension);

        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(371.9039, -990.349854, -98.0589447), new Vector3(0, 0, -89.7690125), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(376.57428, -990.049927, -96.4589691), new Vector3(0, -0, -179.769073), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(372.103912, -1004.65002, -98.0589447), new Vector3(0, 0, -89.7690048), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(377.604248, -1004.15015, -98.0589447), new Vector3(0, 0, 0.23099421), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(383.774689, -1004.15015, -98.0589447), new Vector3(0, -0, 90.2309723), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(381.704498, -992.852905, -98.0589447), new Vector3(0, -0, 90.2309647), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(388.004883, -998.751465, -98.0589447), new Vector3(0, -0, -179.769104), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(370.503998, -998.349854, -98.0589447), new Vector3(0, 0, -89.7690048), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(365.674225, -996.549805, -98.4589691), new Vector3(0, -0, -179.769073), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("prop_huge_display_01"), new Vector3(365.594147, -998.883057, -98.4589691), new Vector3(0, 0, 0.231002808), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("lr_prop_clubstool_01"), new Vector3(376.016602, -999.739929, -100.028435), new Vector3(0, 0, 89.774147), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("lr_prop_clubstool_01"), new Vector3(375.938019, -1000.87671, -100.028435), new Vector3(0, 0, 81.4817657), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("lr_prop_clubstool_01"), new Vector3(375.919434, -1001.87073, -100.028435), new Vector3(0, -0, 109.426132), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("lr_prop_clubstool_01"), new Vector3(376.111938, -1003.08606, -100.028435), new Vector3(0, -0, 136.909775), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("lr_prop_clubstool_01"), new Vector3(375.815247, -990.539917, -99.7284622), new Vector3(0, 0, -4.8109498), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("p_yoga_mat_03_s"), new Vector3(373.065887, -999.187195, -98.9689713), new Vector3(8.40430744e-07, 89.9999466, 179.998123), 255, NAPI.GlobalDimension);
        //        //NAPI.Object.CreateObject(NAPI.Util.GetHashKey("hei_heist_kit_bin_01"), new Vector3(373.325378, -999.457397, -99.9999771), new Vector3(0, 0, 47.6214714), 255, NAPI.GlobalDimension);

        //        //NAPI.Blip.CreateBlip(93, new Vector3(-1388.761, -586.3921, 29.09945), 1, 0, "Bahama Mamas West", 255, 0, true);
        //        //NAPI.Blip.CreateBlip(121, new Vector3(141.3792, -1281.576, 28.2172), 1, 0, "Vanila Unicorn", 255, 0, true);
        //        //NAPI.Blip.CreateBlip(136, new Vector3(-564.5512, 275.6993, 81.98249), 1, 0, "Tequi-la-la", 255, 0, true);
        //        //NAPI.Blip.CreateBlip(205, new Vector3(-430.1028, 261.2774, 81.88689), 1, 0, "Split Sides West Comedy Club", 255, 0, true);

        //        //var result = MySQL.QueryRead("SELECT * FROM alcoclubs");
        //        //if (result == null || result.Rows.Count == 0)
        //        //{
        //        //    _logger.WriteError("DB alcoclubs return null result.", nLog.Type.Warn);
        //        //    return;
        //        //}
        //        //foreach (DataRow Row in result.Rows)
        //        //{
        //        //    var id = Convert.ToInt32(Row["id"]);
        //        //    var stock = new Stock(Convert.ToInt32(Row["mats"]), Convert.ToInt32(Row["alco1"]), Convert.ToInt32(Row["alco2"]), Convert.ToInt32(Row["alco3"]),
        //        //        Convert.ToSingle(Convert.ToInt32(Row["pricemod"]) / 100), UnloadPoints[id] + new Vector3(0, 0, 0.8));
        //        //    ClubsStocks.Add(id, stock);
        //        //}
        //        //_logger.WriteError("AlcoClubs are loaded", nLog.Type.Success);

        //        //#region Enter AlcoShops
        //        //foreach (var pair in EnterAlcoShop)
        //        //{
        //        //    var club = pair.Key;
        //        //    InteractShape.Create(pair.Value, 1, 2)
        //        //        .AddDefaultMarker()
        //        //        .AddInteraction((player) =>
        //        //        {
        //        //            EnterClub(player, club);
        //        //        }, "interact_27");
        //        //    NAPI.TextLabel.CreateTextLabel($"~g~Club\n\"{ClubsNames[pair.Key]}\"", pair.Value + new Vector3(0, 0, 0.5), 5f, 0.3f, 0, new Color(255, 255, 255), true, NAPI.GlobalDimension);
        //        //}
        //        //#endregion
        //        //#region Exit AlcoShops
        //        //foreach (var pair in ExitAlcoShop)
        //        //{
        //        //    var club = pair.Key;
        //        //    InteractShape.Create(pair.Value, 1, 2)
        //        //        .AddDefaultMarker()
        //        //        .AddInteraction((player) =>
        //        //        {
        //        //            ExitClub(player, club);
        //        //        }, "interact_28");
        //        //}
        //        //#endregion
        //        //#region Unloadpoints
        //        //foreach (var pair in UnloadPoints)
        //        //{
        //        //    var club = pair.Key;
        //        //    InteractShape.Create(pair.Value, 5, 5)
        //        //        .AddMarker(27, pair.Value, 5, InteractShape.DefaultMarkerColor)
        //        //        .AddInteraction((player) =>
        //        //        {
        //        //            SafeTrigger.SetData(player, "CLUB", club);
        //        //            InteractStock(player);
        //        //        });
        //        //}
        //        //#endregion
        //        //#region BuyPoints
        //        //foreach (var pair in BuyPoints)
        //        //{
        //        //    var club = pair.Key;
        //        //    InteractShape.Create(pair.Value, 1.5f, 2)
        //        //        .AddDefaultMarker()
        //        //        .AddInteraction((player) =>
        //        //        {
        //        //            SafeTrigger.SetData(player, "CLUB", club);
        //        //            InteractBuy(player);
        //        //        }, "interact_29");
        //        //}
        //        //#endregion
        //    }
        //    catch (Exception e) { _logger.WriteError("ServerStart: " + e.ToString()); }
        //}

        //private static void EnterClub(ExtPlayer player, int club)
        //{
        //    player.ChangePosition(ExitAlcoShop[club] + new Vector3(0, 0, 1.2));
        //}

        //private static void ExitClub(ExtPlayer player, int club)
        //{
        //    player.ChangePosition(EnterAlcoShop[club] + new Vector3(0, 0, 1.2));
        //}

        //private static void InteractStock(ExtPlayer player)
        //{
        //    if (!player.IsLogged()) return;

        //    if (player.Character.FractionID != player.GetData<string>("CLUB"))
        //    {
        //        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_143".Translate( (string)Fractions.Manager.getName(player.GetData<string>("CLUB"))), 3000);
        //        return;
        //    }

        //    if (!player.IsInVehicle)
        //    {
        //        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_144", 3000);
        //        return;
        //    }

        //    int club = player.GetData<string>("CLUB");

        //    var matCount = VehicleInventory.GetCountOfType(player.Vehicle, ItemType.Material);
        //    if (matCount == 0)
        //    {
        //        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_145", 3000);
        //        return;
        //    }

        //    if (ClubsStocks[club].Materials >= MaxMats)
        //    {
        //        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_146", 3000);
        //        return;
        //    }

        //    VehicleInventory.Remove(player.Vehicle, ItemType.Material, matCount);
        //    ClubsStocks[club].Materials += matCount;
        //    ClubsStocks[club].UpdateLabel();
        //    Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_147", 3000);
        //}

        //private static void InteractBuy(ExtPlayer player)
        //{
        //    if (!player.IsLogged()) return;
        //    //OpenBuyAlcoholMenu(player);
        //}

        //public static void ResistTimer(ExtPlayer player)
        //{
        //    if (!player.IsLogged()) return;

        //    if (player.GetData<string>("RESIST_TIME") == 0)
        //    {
        //        SafeTrigger.ClientEvent(player, "stopScreenEffect", "PPFilter");
        //        SafeTrigger.ClientEvent(player, "setResistStage", 0);

        //        player.ResetData("RESIST_BAN");
        //        //Main.StopT(player.GetData<string>("RESIST_TIMER"), "timer_27");
        //        Timers.Stop(player.GetData<string>("RESIST_TIMER"));
        //    }
        //    else
        //        SafeTrigger.SetData(player, "RESIST_TIME", player.GetData<string>("RESIST_TIME") - 1);
        //}

        //public static void SaveAlco()
        //{
        //    try
        //    {
        //        foreach (var club in ClubsStocks)
        //        {
        //            MySQL.Query("UPDATE alcoclubs SET alco1={club.Value.Alco1},alco2={club.Value.Alco2},alco3={club.Value.Alco3}," +
        //                $"pricemod={Convert.ToInt32(club.Value.PriceModifier * 100)},mats={club.Value.Materials} WHERE id={club.Key}");
        //        }
        //    }
        //    catch (Exception e) { _logger.WriteError("SaveAlco: " + e.ToString()); }
        //}

        #region Buy Menu
        //public static void OpenBuyAlcoholMenu(ExtPlayer player)
        //{
        //    int club = player.GetData<string>("CLUB");
        //    var isOwner = player.Character.FractionID == club;
        //    var stock = new List<int>()
        //    {
        //        ClubsStocks[club].Materials,
        //        ClubsStocks[club].Alco1,
        //        ClubsStocks[club].Alco2,
        //        ClubsStocks[club].Alco3,
        //    };
        //    SafeTrigger.ClientEvent(player, "openAlco", club, ClubsStocks[club].PriceModifier, isOwner, stock);
        //}

        //[RemoteEvent("clubmenu::buy")]
        //public static void RemoteEvent_BuyDrink(ExtPlayer player, int drinkIndex)
        //{
        //    try
        //    {
        //        if (player.GetData<string>("CLUB") == -1) return;

        //        int club = player.GetData<string>("CLUB");
        //        var alcoCounts = new List<int>() { ClubsStocks[club].Alco1, ClubsStocks[club].Alco2, ClubsStocks[club].Alco3 };
                
        //        if (alcoCounts[drinkIndex] <= 0)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_148", 3000);
        //            return;
        //        }

        //        ItemType drinkItemType = DrinksInClubs[club][drinkIndex];

        //        var tryAdd = nInventory.TryAdd(player, new nItem(drinkItemType));

        //        if (tryAdd == -1 || tryAdd > 0)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_149", 3000);
        //            return;
        //        }

        //        if (!MoneySystem.Wallet.Change(player, -Convert.ToInt32(DrinksPrices[drinkIndex] * ClubsStocks[club].PriceModifier)))
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_150", 3000);
        //            return;
        //        }

        //        Stocks.fracStocks[club].Money += Convert.ToInt32(DrinksPrices[drinkIndex] * ClubsStocks[club].PriceModifier);
        //        GameLog.Money($"player({player.Character.UUID})", $"frac({club})", Convert.ToInt32(DrinksPrices[drinkIndex] * ClubsStocks[club].PriceModifier), $"buyAlco");
        //        nInventory.Add(player, new nItem(drinkItemType));

        //        switch (drinkIndex)
        //        {
        //            case 0:
        //                ClubsStocks[club].Alco1--;
        //                break;
        //            case 1:
        //                ClubsStocks[club].Alco2--;
        //                break;
        //            case 2:
        //                ClubsStocks[club].Alco3--;
        //                break;
        //        }
        //        ClubsStocks[club].UpdateLabel();
        //        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_151".Translate( nInventory.ItemsNames[(int)drinkItemType]), 3000);
        //    }
        //    catch (Exception e) { _logger.WriteError("clubmenu::buy: " + e.ToString()); }
        //}

        //[RemoteEvent("clubmenu::take")]
        //public static void RemoteEvent_TakeDrink(ExtPlayer player, int drinkIndex)
        //{
        //    try
        //    {
        //        if (player.GetData<string>("CLUB") == -1) return;
        //        if (!player.IsLogged()) return;

        //        int club = player.GetData<string>("CLUB");

        //        if (player.Character.FractionID != club || !Manager.canUseCommand(player, "takeclubalco", false))
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_152", 3000);
        //            return;
        //        }

        //        var alcoCounts = new List<int>() { ClubsStocks[club].Alco1, ClubsStocks[club].Alco2, ClubsStocks[club].Alco3 };
        //        if (alcoCounts[drinkIndex] <= 0)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_153", 3000);
        //            return;
        //        }

        //        ItemType drinkItemType = DrinksInClubs[club][drinkIndex];

        //        var tryAdd = nInventory.TryAdd(player, new nItem(drinkItemType));
        //        if (tryAdd == -1 || tryAdd > 0)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_154", 3000);
        //            return;
        //        }
        //        nInventory.Add(player, new nItem(drinkItemType));

        //        switch (drinkIndex)
        //        {
        //            case 0:
        //                ClubsStocks[club].Alco1--;
        //                break;
        //            case 1:
        //                ClubsStocks[club].Alco2--;
        //                break;
        //            case 2:
        //                ClubsStocks[club].Alco3--;
        //                break;
        //        }

        //        ClubsStocks[club].UpdateLabel();
        //        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_155".Translate( nInventory.ItemsNames[(int)drinkItemType],alcoCounts[drinkIndex] - 1), 3000);
        //    }
        //    catch (Exception e) { _logger.WriteError("clubmenu::take: " + e.ToString()); }
        //}

        //[RemoteEvent("clubmenu::setprice")]
        //public static void RemoteEvent_SetDrinkPrice(ExtPlayer player, int drinkIndex, int newPrice)
        //{
        //    try
        //    {
        //        if (player.GetData<string>("CLUB") == -1) return;
        //        if (!player.IsLogged()) return;

        //        int club = player.GetData<string>("CLUB");

        //        if (player.Character.FractionID != club || !Manager.canUseCommand(player, "setalcoprice", false))
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_156", 3000);
        //            return;
        //        }

        //        if (newPrice < 50 || newPrice > 150)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_157", 3000);
        //            return;
        //        }

        //        ClubsStocks[player.Character.FractionID].PriceModifier = newPrice / 100.0f;
        //        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_158".Translate( newPrice), 3000);
        //    }
        //    catch (Exception e) { _logger.WriteError("clubmenu::setprice: " + e.ToString()); }
        //}

        //[RemoteEvent("clubmenu::craft")]
        //public static void RemoteEvent_CraftDrink(ExtPlayer player, int drinkIndex)
        //{
        //    try
        //    {
        //        if (player.GetData<string>("CLUB") == -1) return;
        //        if (!player.IsLogged()) return;

        //        int club = player.GetData<string>("CLUB");

        //        if (player.Character.FractionID != club || !Manager.canUseCommand(player, "craftalco", false))
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_159", 3000);
        //            return;
        //        }

        //        var alcoCounts = new List<int>() { ClubsStocks[club].Alco1, ClubsStocks[club].Alco2, ClubsStocks[club].Alco3 };
        //        ItemType drinkItemType = DrinksInClubs[club][drinkIndex];

        //        if (alcoCounts[drinkIndex] >= 80)
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_160".Translate( drinkItemType.ToString()), 3000);
        //            return;
        //        }
        //        if (ClubsStocks[club].Materials < DrinksMats[drinkIndex])
        //        {
        //            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_161", 3000);
        //            return;
        //        }

        //        ClubsStocks[club].Materials -= DrinksMats[drinkIndex];
        //        switch (drinkIndex)
        //        {
        //            case 0:
        //                ClubsStocks[club].Alco1++;
        //                break;
        //            case 1:
        //                ClubsStocks[club].Alco2++;
        //                break;
        //            case 2:
        //                ClubsStocks[club].Alco3++;
        //                break;
        //        }

        //        ClubsStocks[club].UpdateLabel();
        //        Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Frac_162".Translate(nInventory.ItemsNames[(int)drinkItemType],alcoCounts[drinkIndex] + 1), 3000);
        //    }
        //    catch (Exception e) { _logger.WriteError("clubmenu::craft: " + e.ToString()); }
        //}
        #endregion

        internal class Stock
        {
            //public int Materials { get; set; }
            //public int Alco1 { get; set; }
            //public int Alco2 { get; set; }
            //public int Alco3 { get; set; }
            //public float PriceModifier { get; set; }

            //public TextLabel Label { get; set; }

            //public Stock(int mats, int a1, int a2, int a3, float price, Vector3 pos)
            //{
            //    Label = NAPI.TextLabel.CreateTextLabel($"~w~Materials: {mats}\n~w~Alcohol: {a1 + a2 + a3}", pos, 30f, 0.3f, 0, new Color());

            //    Materials = mats;
            //    Alco1 = a1;
            //    Alco2 = a2;
            //    Alco3 = a3;
            //    PriceModifier = price;
            //}

            //public void UpdateLabel()
            //{
            //    Label.Text = $"~w~Materials: {Materials}\n~w~Alcohol: {Alco1 + Alco2 + Alco3}";
            //}
        }
    }
}
