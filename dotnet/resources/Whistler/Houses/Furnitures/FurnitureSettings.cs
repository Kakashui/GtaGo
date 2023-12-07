using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Whistler.Businesses;

namespace Whistler.Houses.Furnitures
{
    internal static class FurnitureSettings
    {
        private static Dictionary<string, FurnitureSetting> _allAvailableFurnitures;
        public static IReadOnlyDictionary<string, FurnitureSetting> AllAvailableFurnitures => _allAvailableFurnitures;
        public static FurnitureStoreDTO StoreDTO;

        public const int BizTypeId = 27;
        public const int HouseSafeWeight = 100000;
        public const int HouseSafeSize = 60;
        public const int HouseWardrobeWeight = 60000;
        public const int HouseWardrobeSize = 50;
        public static string[] SafeModelHashes = { "p_v_43_safe_s", "prop_ld_int_safe_01" };
        public static string[] WardrobeModelHashes = { "prop_cabinet_01", "prop_cabinet_01b" };

        public static void InitFurnitureSettings()
        {
            _allAvailableFurnitures = new Dictionary<string, FurnitureSetting>
            {
                ["apa_mp_h_stn_sofa2seat_02"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Rotterdam"),
                ["apa_mp_h_stn_sofacorn_01"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Dins"),
                ["apa_mp_h_stn_sofacorn_05"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Leni"),
                ["apa_mp_h_stn_sofacorn_06"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Portu"),
                ["apa_mp_h_stn_sofacorn_07"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Menli"),
                ["apa_mp_h_stn_sofacorn_08"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Griton"),
                ["apa_mp_h_stn_sofacorn_09"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Griton"),
                ["apa_mp_h_stn_sofacorn_10"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Norfolk"),
                ["apa_mp_h_stn_sofa_daybed_01"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Kano"),
                ["apa_mp_h_stn_sofa_daybed_02"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Vitio"),
                ["apa_mp_h_yacht_sofa_01"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Shiteno"),
                ["apa_mp_h_yacht_sofa_02"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Moss"),
                ["bkr_prop_clubhouse_sofa_01a"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Kusken"),
                ["ex_mp_h_off_sofa_003"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Disent"),
                ["ex_mp_h_off_sofa_01"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Misl"),
                ["ex_mp_h_off_sofa_02"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Misl 2"),
                ["hei_heist_stn_sofa2seat_03"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Berg"),
                ["hei_heist_stn_sofa2seat_06"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Orson"),
                ["hei_heist_stn_sofa3seat_01"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Kotne"),
                ["hei_heist_stn_sofa3seat_02"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Chiron"),
                ["hei_heist_stn_sofa3seat_06"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Inole"),
                ["hei_heist_stn_sofacorn_05"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Leni"),
                ["prop_t_sofa_02"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Loft"),
                ["prop_yaught_sofa_01"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Hewit"),
                ["p_lev_sofa_s"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Fabius"),
                ["p_res_sofa_l_s"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Verona"),
                ["p_sofa_s"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Murano"),
                ["p_v_med_p_sofa_s"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Lightsey"),
                ["p_yacht_sofa_01_s"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Hewit"),
                ["v_res_tre_sofa_s"] = new FurnitureSetting(FurnitureShopType.Sofas, 200, "Sharpe"),
                
                ["apa_mp_h_str_avunitl_04"] = new FurnitureSetting(FurnitureShopType.TvTables, 200, "Cologne"),
                ["apa_mp_h_str_avunitm_01"] = new FurnitureSetting(FurnitureShopType.TvTables, 200, "Albury"),
                ["apa_mp_h_str_avunitm_03"] = new FurnitureSetting(FurnitureShopType.TvTables, 200, "Albury"),
                ["apa_mp_h_str_avunits_01"] = new FurnitureSetting(FurnitureShopType.TvTables, 200, "Albury"),
                ["apa_mp_h_str_avunits_04"] = new FurnitureSetting(FurnitureShopType.TvTables, 200, "Albury"),
                ["hei_heist_str_avunitl_03"] = new FurnitureSetting(FurnitureShopType.TvTables, 200, "Linz"),
                ["hei_heist_str_avunits_01"] = new FurnitureSetting(FurnitureShopType.TvTables, 200, "Avila"),
                
                ["apa_mp_h_str_sideboardl_06"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Nordic"),
                ["apa_mp_h_str_sideboardl_11"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Concept"),
                ["apa_mp_h_str_sideboardl_09"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Avila"),
                ["apa_mp_h_str_sideboardl_13"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Oscar"),
                ["apa_mp_h_str_sideboardl_14"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Livorno"),
                ["apa_mp_h_str_sideboardm_02"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Fiji"),
                ["apa_mp_h_str_sideboardm_03"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Fiji 2"),
                ["apa_mp_h_str_sideboards_01"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Malom small"),
                ["apa_mp_h_str_sideboards_02"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Linz Big"),
                ["hei_heist_str_sideboardl_02"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Dave"),
                ["hei_heist_str_sideboardl_03"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Deans"),
                ["hei_heist_str_sideboardl_04"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Sharpe"),
                ["hei_heist_str_sideboardl_05"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Malom big"),
                ["hei_heist_str_sideboards_02"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Linz small"),
                ["apa_mp_h_bed_chestdrawer_02"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Virginia"),
                ["hei_heist_bed_chestdrawer_04"] = new FurnitureSetting(FurnitureShopType.Pedestals, 200, "Ravenna"),
                
                ["apa_mp_h_din_chair_04"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Ames"),
                ["apa_mp_h_din_chair_08"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Valencia"),
                ["apa_mp_h_din_chair_12"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Darcy"),
                ["apa_mp_h_din_chair_09"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Venus"),
                ["hei_heist_din_chair_02"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Geneva"),
                ["hei_heist_din_chair_05"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Gross"),
                ["hei_heist_din_chair_06"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Ames"),
                ["prop_table_04_chr"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Camel"),
                ["prop_table_06_chr"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Diva"),
                
                ["apa_mp_h_stn_chairarm_01"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto"),
                ["apa_mp_h_stn_chairarm_02"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Shell"),
                ["apa_mp_h_stn_chairarm_03"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Ergo"),
                ["apa_mp_h_stn_chairarm_09"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Liberty"),
                ["apa_mp_h_stn_chairarm_11"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Rustic"),
                ["apa_mp_h_stn_chairarm_12"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Rotterdam"),
                ["apa_mp_h_stn_chairarm_13"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Ottawa"),
                ["apa_mp_h_stn_chairarm_23"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Blues"),
                ["apa_mp_h_stn_chairarm_24"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Geneva"),
                ["apa_mp_h_stn_chairarm_25"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Bonnie"),
                ["apa_mp_h_stn_chairarm_26"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Teos"),
                ["apa_mp_h_stn_chairstool_12"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Brick"),
                ["apa_mp_h_stn_chairstrip_01"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Magni"),
                ["apa_mp_h_stn_chairstrip_02"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto"),
                ["apa_mp_h_stn_chairstrip_03"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Blues"),
                ["apa_mp_h_stn_chairstrip_04"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Milan"),
                ["apa_mp_h_stn_chairstrip_05"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Milan"),
                ["apa_mp_h_stn_chairstrip_06"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto"),
                ["apa_mp_h_stn_chairstrip_07"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto"),
                ["apa_mp_h_stn_chairstrip_08"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Milan"),
                ["apa_mp_h_yacht_armchair_01"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Shiteno"),
                ["apa_mp_h_yacht_armchair_03"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Fabius"),
                ["apa_mp_h_yacht_armchair_04"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Moss"),
                ["apa_mp_h_yacht_strip_chair_01"] =new FurnitureSetting(FurnitureShopType.ArmChairs, 200, "Fabius"),
                ["ba_prop_battle_club_chair_01"] = new FurnitureSetting(FurnitureShopType.ArmChairs, 200, "Burgos"),
                ["ba_prop_battle_club_chair_02"] = new FurnitureSetting(FurnitureShopType.ArmChairs, 200, "Evora"),
                ["ba_prop_battle_club_chair_03"] = new FurnitureSetting(FurnitureShopType.ArmChairs, 200, "Corsica"),
                ["bkr_prop_biker_boardchair01"] = new FurnitureSetting(FurnitureShopType.ArmChairs, 200, "Burgos"),
                ["bkr_prop_biker_chairstrip_01"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Magni"),
                ["bkr_prop_clubhouse_armchair_01a"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Madrid"),
                ["bkr_prop_weed_chair_01a"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Charlie"),
                ["ex_mp_h_off_easychair_01"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Ottawa"),
                ["ex_mp_h_off_chairstrip_01"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto 1"),
                ["ex_mp_h_stn_chairstrip_010"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto 2"),
                ["ex_mp_h_stn_chairstrip_011"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto 3"),
                ["ex_mp_h_stn_chairstrip_07"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto 4"),
                ["ex_prop_offchair_exec_01"] = new FurnitureSetting(FurnitureShopType.ArmChairs, 200, "Chester"),
                ["ex_prop_offchair_exec_03"] = new FurnitureSetting(FurnitureShopType.ArmChairs, 200, "Bons"),
                ["hei_heist_stn_chairarm_04"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Prague"),
                ["hei_heist_stn_chairarm_06"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Dublin"),
                ["hei_heist_stn_chairstrip_01"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Veneto"),
                ["imp_prop_impexp_offchair_01a"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Chester"),
                ["prop_chair_02"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Medius"),
                ["prop_chair_03"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Borneo"),
                ["prop_chair_04a"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Kotne"),
                ["prop_chair_04b"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Kotne"),
                ["prop_chair_05"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Odens"),
                ["prop_clown_chair"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Oslo"),
                ["prop_cs_office_chair"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Monty"),
                ["prop_off_chair_05"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Ivers"),
                ["prop_yaught_chair_01"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Hewit"),
                ["p_yacht_chair_01_s"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Hewit"),
                ["p_armchair_01_s"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Lazio"),
                ["p_dinechair_01_s"] = new FurnitureSetting(FurnitureShopType.Chairs, 200, "Oxford"),
                ["xm_mp_h_stn_chairarm_13"] = new FurnitureSetting(FurnitureShopType.Stools, 200, "Borneo"),
                
                ["apa_mp_h_bed_double_08"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Sharon"),
                ["apa_mp_h_bed_double_09"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Derryl"),
                ["apa_mp_h_bed_wide_05"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Willow"),
                ["apa_mp_h_yacht_bed_01"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Cleveland"),
                ["apa_mp_h_bed_with_table_02"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Plidy"),
                ["apa_mp_h_yacht_bed_02"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Auckland"),
                ["gr_prop_bunker_bed_01"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Esmeralda"),
                ["ex_prop_exec_bed_01"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Camcot"),
                ["p_lestersbed_s"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Oscar"),
                ["p_mbbed_s"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Graph"),
                ["v_res_msonbed_s"] = new FurnitureSetting(FurnitureShopType.Beds, 200, "Willow"),
                
                ["hei_heist_bed_table_dble_04"] = new FurnitureSetting(FurnitureShopType.BedSideTables, 200, "Griton"),
                ["apa_mp_h_bed_table_wide_12"] = new FurnitureSetting(FurnitureShopType.BedSideTables, 200, "York"),
                
                ["apa_mp_h_din_table_01"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Brick"),
                ["apa_mp_h_din_table_04"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Diko"),
                ["apa_mp_h_din_table_05"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Loft"),
                ["apa_mp_h_din_table_06"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Ames"),
                ["apa_mp_h_din_table_11"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Denmark"),
                ["ba_prop_int_edgy_table_01"] =new FurnitureSetting(FurnitureShopType.Desks, 200, "Haller"),
                ["ba_prop_int_edgy_table_02"] =new FurnitureSetting(FurnitureShopType.Desks, 200, "Haller"),
                ["ba_prop_int_glam_table"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Aktsent"),
                ["gr_dlc_gr_yacht_props_table_03"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Hewit"),
                ["hei_heist_din_table_06"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Chicago"),
                ["hei_heist_din_table_07"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Kira"),
                ["prop_table_04"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Berger"),
                ["prop_table_06"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Marius"),
                ["v_ilev_liconftable_sml"] = new FurnitureSetting(FurnitureShopType.Desks, 200, "Clyde"),
                
                ["apa_mp_h_yacht_coffee_table_01"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Tiffany"),
                ["apa_mp_h_yacht_coffee_table_02"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Sheffield"),
                ["apa_mp_h_yacht_side_table_01"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Derry"),
                ["apa_mp_h_yacht_side_table_02"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Medley"),
                ["ch_prop_ch_coffe_table_02"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Lacoste"),
                ["ch_prop_table_casino_short_02a"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Benfleet"),
                ["hei_prop_yah_table_01"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Hewit Small"),
                ["hei_prop_yah_table_02"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Hewit Big"),
                ["prop_fbi3_coffee_table"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Oscar"),
                ["prop_patio_lounger1_table"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Givara"),
                ["prop_tablesmall_01"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Burleigh"),
                ["prop_t_coffe_table"] = new FurnitureSetting(FurnitureShopType.CoffeeTables, 200, "Tiffany Lite"),           
                
                ["apa_mp_h_floorlamp_a"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Mona"),
                ["apa_mp_h_floorlamp_b"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Medius"),
                ["apa_mp_h_floorlamp_c"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Aster"),
                ["apa_mp_h_floor_lamp_int_08"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Carlos"),
                ["apa_mp_h_lit_floorlampnight_05"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Kano"),
                ["apa_mp_h_lit_floorlampnight_07"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Sheri"),
                ["apa_mp_h_lit_floorlampnight_14"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Aster"),
                ["apa_mp_h_lit_floorlamp_01"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Erwin"),
                ["apa_mp_h_lit_floorlamp_02"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Medius"),
                ["apa_mp_h_lit_floorlamp_03"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Leeds"),
                ["apa_mp_h_lit_floorlamp_06"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Deans"),
                ["apa_mp_h_lit_floorlamp_10"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Cambridge"),
                ["apa_mp_h_lit_floorlamp_13"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Sheri"),
                ["apa_mp_h_lit_floorlamp_17"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Newburry"),
                ["hei_heist_lit_floorlamp_04"] = new FurnitureSetting(FurnitureShopType.FloorLamps, 200, "Nosta"),
                
                ["apa_mp_h_acc_plant_palm_01"] = new FurnitureSetting(FurnitureShopType.Plants, 200, "Branchea"),
                ["apa_mp_h_acc_plant_tall_01"] = new FurnitureSetting(FurnitureShopType.Plants, 200, "Calamus"),
                ["ch_prop_ch_planter_01"] = new FurnitureSetting(FurnitureShopType.Plants, 200, "Calamus"),
                ["prop_plant_int_01a"] = new FurnitureSetting(FurnitureShopType.Plants, 200, "Cariota"),
                ["prop_plant_int_03a"] = new FurnitureSetting(FurnitureShopType.Plants, 200, "Gioforba"),
                ["prop_plant_int_03b"] = new FurnitureSetting(FurnitureShopType.Plants, 200, "Gioforba"),
                ["prop_plant_int_03c"] = new FurnitureSetting(FurnitureShopType.Plants, 200, "Gioforba"),
                ["p_int_jewel_plant_02"] = new FurnitureSetting(FurnitureShopType.Plants, 200, "Fern"),
                
                ["prop_ld_int_safe_01"] = new FurnitureSetting(FurnitureShopType.Storage, 300, "Black Safe"),
                ["p_v_43_safe_s"] = new FurnitureSetting(FurnitureShopType.Storage, 150, "Safe S"),
                ["prop_cabinet_01"] = new FurnitureSetting(FurnitureShopType.Storage, 200, "Wardrobe"),
                ["prop_cabinet_01b"] = new FurnitureSetting(FurnitureShopType.Storage, 300, "Wardrobe"),
            };
            StoreDTO = new FurnitureStoreDTO();
            StoreDTO.Categories = new List<FurnitureCategoryDTO>();
            foreach (var name in Enum.GetNames(typeof(FurnitureShopType)))
            {
                StoreDTO.Categories.Add(new FurnitureCategoryDTO
                {
                    Key = name.ToLower(),
                    Name = "furnitureShop_" + name.ToLower()
                });               
            }
            StoreDTO.Products = new List<FurnitureItemDTO>();
            foreach (var (model, furniture) in _allAvailableFurnitures)
            {
                StoreDTO.Products.Add(new FurnitureItemDTO
                {
                    Key = model,
                    Category = furniture.FurnitureType.ToString().ToLower(),
                    Cost = furniture.Cost,
                    Description = "",
                    Name = furniture.Name
                });
            }

            HandleBusinessSettings();
            Parse();
        }

        private static void HandleBusinessSettings()
        {
            foreach (var item in _allAvailableFurnitures.Select(item => item.Value.Name).ToList())
            {
                BusinessesSettings.DeleteProduct(BizTypeId, item);
            }
            var setts = BusinessesSettings.GetBusinessSettings(BizTypeId).Products.FirstOrDefault(item => item.Name == "Parts");
            if (setts == null || setts.MaxPrice > 150)
            {
                BusinessesSettings.DeleteProduct(BizTypeId, "Parts");
                BusinessesSettings.AddNewProduct(BizTypeId, "Parts", "$", 100, 150, 70, 1000000);
            }
        }

        public static void Parse()
        {
            if (Directory.Exists("interfaces/gui/src/configs/furniture"))
            { 
                using var w = new StreamWriter("interfaces/gui/src/configs/furniture/data.js");
                w.Write($"export default {JsonConvert.SerializeObject(StoreDTO)}");
            }
        }
    }

    internal class FurnitureStoreDTO
    {
        [JsonProperty("categories")]
        public List<FurnitureCategoryDTO> Categories { get; set; }

        [JsonProperty("products")]
        public List<FurnitureItemDTO> Products { get; set; }
    }

    internal class FurnitureItemDTO
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }

    internal class FurnitureCategoryDTO
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}