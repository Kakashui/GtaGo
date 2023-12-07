using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Whistler.Core.CustomSync;
using Whistler.Gangs.WeedFarm;
using Whistler.Inventory;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Enums;
using Whistler.Scenes.Actions;
using Whistler.SDK;

namespace Whistler.Scenes.Configs
{
    class ScenesConfig
    {
        private Dictionary<int, SceneModel> _scenes;
        public ScenesConfig()
        {
            _scenes = new Dictionary<int, SceneModel>(); 
            _scenes.Add(
                (int)SceneNames.DrinkSprunk,
                new SceneModel()
                    .AddEnterAnim("mini@sprunk", "plyr_buy_drink_pt1", .5f, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddBaseAnim("mini@sprunk", "plyr_buy_drink_pt1", .9f, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame, AnimFlag.UpperBody)
                    .AddAction("mini@sprunk", "plyr_buy_drink_pt2", 0, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddExitAnim("mini@sprunk", "plyr_buy_drink_pt3", .3f, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("prop_ecola_can", 28422, new Vector3(), new Vector3(), true)
                    .AddActionCallback(ItemActions.Drink)
            );

            _scenes.Add(
                (int)SceneNames.Bong,
                new SceneModel()
                    .AddSequence("anim@safehouse@bong", "bong_stage1", .15f, .7f, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddActionEffect(new ActionEffectModel("scr_safehouse", "scr_sh_bong_smoke", .5f, 3000, 2, 12844, new Vector3(0, .1515, 0), new Vector3()))
                    .AddActionEffect(new ActionEffectModel("scr_safehouse", "scr_sh_bong_smoke", .6f, 3000, 2, 12844, new Vector3(0, .1515, 0), new Vector3()))
                    .AddActionEffect(new ActionEffectModel("scr_safehouse", "scr_sh_lighter_flame", .3f, 2300, 1, 6286, new Vector3(.07, .0025, .095), new Vector3()))
                    .AddSequenceCallback(ItemActions.Narcotics)
                    .AddAttach("p_cs_lighter_01", 28422, new Vector3(.0025, .005, -.0575), new Vector3())
                    .AddAttach("prop_bong_01", 60309, new Vector3(), new Vector3())
            );

            _scenes.Add(
                (int)SceneNames.Cigarette,
                new SceneModel()
                    .AddEnterAnim("AMB@WORLD_HUMAN_SMOKING@MALE@MALE_A@ENTER", "ENTER", 0, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddEnterEffect(new ActionEffectModel("scr_safehouse", "scr_sh_bong_smoke", .5f, 3000, 2, 12844, new Vector3(0, .1515, 0), new Vector3()))
                    .AddEnterEffect(new ActionEffectModel("scr_safehouse", "scr_sh_bong_smoke", .85f, 3000, 2, 12844, new Vector3(0, .1515, 0), new Vector3()))
                    .AddBaseAnim("AMB@WORLD_HUMAN_SMOKING@MALE@MALE_A@IDLE_A", "IDLE_A", .89f, 1, AnimFlag.StopLastFrame, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("prop_cs_ciggy_01", 28422, new Vector3(), new Vector3())
                    //.AddAttach("p_cs_lighter_01", 28422, new Vector3(0.115, .018, -.0095), new Vector3(-165, -94, 0))
                    .AddAction("AMB@WORLD_HUMAN_SMOKING@MALE@MALE_A@IDLE_A", "IDLE_B", 0, 1, AnimFlag.UpperBody, AnimFlag.CanMove)
                    .AddActionCallback(ItemActions.SmokingCigarette)
                    .AddActionEffect(new ActionEffectModel("scr_safehouse", "scr_sh_bong_smoke", .47f, 3000, 2, 12844, new Vector3(0, .1515, 0), new Vector3()))
                    .AddActionEffect(new ActionEffectModel("scr_safehouse", "scr_sh_bong_smoke", .65f, 5000, 2.5f, 12844, new Vector3(), new Vector3()))
                    .AddExitAnim("AMB@WORLD_HUMAN_SMOKING@MALE@MALE_A@EXIT", "EXIT", .35f, 1, AnimFlag.UpperBody, AnimFlag.CanMove)
            );

            _scenes.Add(
                (int)SceneNames.Blunt,
                new SceneModel()
                    .AddBaseAnim("safe@franklin@ig_13", "blunt_idle_a", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("p_cs_joint_01", 6286, new Vector3(), new Vector3())
                    .AddAction("safe@franklin@ig_13", "blunt_idle_a", 0, .55f, AnimFlag.UpperBody, AnimFlag.CanMove)
                    .AddActionCallback(player => true)
            );
            
            _scenes.Add(
                (int)SceneNames.Guitar,
                new SceneModel()
                    .AddBaseAnim("amb@world_human_musician@guitar@male@base", "base", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("prop_acc_guitar_01", 60309, new Vector3(), new Vector3())
                    .AddAction("amb@world_human_musician@guitar@male@idle_a", "idle_c", 0, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddActionCallback(player => true)
                    .MarkAsCancelled()
            );
            
            _scenes.Add(
                 (int)SceneNames.Microphone,
                 new SceneModel()
                     .AddBaseAnim("missmic4premiere", "interview_short_lazlow", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAttach("p_ing_microphonel_01", 6286, new Vector3(.05,.06,0), new Vector3(-72, 14, 0))
                     .AddActionCallback(player => true)
                    .MarkAsCancelled()
             );

            _scenes.Add(
                 (int)SceneNames.Camera,
                 new SceneModel()
                     .AddBaseAnim("missmic4premiere", "interview_short_camman", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAttach("prop_v_cam_01", 28422, new Vector3(), new Vector3())
                     .AddActionCallback(player => true)
                    .MarkAsCancelled()
             );

            _scenes.Add(
                 (int)SceneNames.Burger,
                 new SceneModel()
                     .AddBaseAnim("mp_player_inteat@burger", "mp_player_int_eat_burger_enter", .8f, 1, AnimFlag.StopLastFrame, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAction("mp_player_inteat@burger", "mp_player_int_eat_burger", 0, 1,  AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAttach("prop_cs_burger_01", 60309, new Vector3(), new Vector3())
                     .AddActionCallback(ItemActions.Eat)
             );

            _scenes.Add(
                 (int)SceneNames.Sandwich,
                 new SceneModel()
                     .AddBaseAnim("amb@world_human_seat_wall_eating@male@both_hands@idle_a", "idle_a", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAction("amb@world_human_seat_wall_eating@male@both_hands@idle_a", "idle_c", 0, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAttach("prop_sandwich_01", 28422, new Vector3(), new Vector3())
                     .AddActionCallback(ItemActions.Eat)
             );

            _scenes.Add(
                 (int)SceneNames.HotDog,
                 new SceneModel()
                     .AddBaseAnim("mp_player_inteat@burger", "mp_player_int_eat_burger_enter", .8f, 1, AnimFlag.StopLastFrame, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAction("mp_player_inteat@burger", "mp_player_int_eat_burger", 0, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAttach("prop_cs_hotdog_01", 60309, new Vector3(.05, .01, -.01), new Vector3(0,0,75))
                     .AddActionCallback(ItemActions.Eat)
             );

            _scenes.Add(
                 (int)SceneNames.Clipboard,
                 new SceneModel()
                     .AddBaseAnim("anim@amb@board_room@supervising@", "dissaproval_01_lo_amy_skater_01", .8f, 1, AnimFlag.StopLastFrame, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAction("anim@amb@board_room@supervising@", "dissaproval_01_lo_amy_skater_01", 0, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAttach("p_amb_clipboard_01", 60309, new Vector3(-.005, .035, .0175), new Vector3(-84,4,-12))
                     .AddAttach("prop_pencil_01", 28422, new Vector3(), new Vector3())
                     .AddActionCallback(player => true)
                     .MarkAsCancelled()
             );

            _scenes.Add(
                 (int)SceneNames.Beer,
                 new SceneModel()
                     .AddBaseAnim("amb@code_human_wander_drinking@beer@male@base", "static", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAction("amb@code_human_wander_drinking@beer@male@idle_a", "idle_c", 0, 1, AnimFlag.CanMove, AnimFlag.UpperBody)
                     .AddAttach("prop_beer_logopen", 28422, new Vector3(0,0,-.15), new Vector3())
                     .AddActionCallback(ItemActions.Aclohol)
             );

            _scenes.Add(
                 (int)SceneNames.Binoculars,
                 new SceneModel()
                     .AddBaseAnim("amb@lo_res_idles@", "world_human_binoculars_lo_res_base", 0, 1, AnimFlag.Looped, AnimFlag.CanMove)
                     .AddAttach("prop_binoc_01", 28422, new Vector3(), new Vector3())
                     .AddActionCallback(player => true)
                    //.AddActionCallback(ItemActions.Aclohol)
                     .MarkAsCancelled()
             );

            _scenes.Add(
                 (int)SceneNames.Tablet,
                 new SceneModel()
                     .AddSequence("anim@arena@amb@seat_drone_tablet@male@var_a@", "tablet_enter", .2f, 1, AnimFlag.UpperBody, AnimFlag.CanMove)
                     .AddSequence("anim@arena@amb@seat_drone_tablet@male@var_a@", "tablet_idle_l", 0, 1, AnimFlag.Looped, AnimFlag.UpperBody, AnimFlag.CanMove)
                     .AddExitAnim("anim@arena@amb@seat_drone_tablet@male@var_a@", "tablet_exit", 0, .6f, AnimFlag.UpperBody, AnimFlag.CanMove)
                     .AddAttach("prop_cs_tablet", 28422, new Vector3(), new Vector3())
                     .AddActionOnStart(player=> SafeTrigger.ClientEvent(player,"tablet:open"))
                     .MarkAsLooped()
             );

            _scenes.Add(
                 (int)SceneNames.Smartphone,
                 new SceneModel()
                     .AddSequence("anim@cellphone@in_car@ds", "cellphone_call_listen_base", 0, 1, AnimFlag.UpperBody, AnimFlag.CanMove)
                     .AddAttach("p_amb_phone_01", 28422, new Vector3(), new Vector3())
                     .AddActionOnStart(player => SafeTrigger.ClientEvent(player,"tablet:open"))
                     .MarkAsLooped()
             );
            _scenes.Add(
                 (int)SceneNames.MedKit,
                 new SceneModel()
                     .AddSequence("amb@code_human_wander_texting_fat@male@enter", "enter", .3f, 1, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                     .AddSequence("amb@code_human_wander_texting_fat@male@idle_a", "idle_b", 0, 1, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                     .AddSequence("amb@code_human_wander_texting_fat@male@exit", "exit", 0, .5f, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                     .AddAttach("prop_ld_health_pack", 28422, new Vector3(0, .0925, -.005), new Vector3(180, 84, -82))
                     .AddSequenceCallback(ItemActions.Medicaments)
             );
            _scenes.Add(
                 (int)SceneNames.Bandage,
                 new SceneModel()
                     .AddSequence("oddjobs@bailbond_hobotwitchy", "base", 0, 1, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                     .AddSequence("oddjobs@bailbond_hobotwitchy", "base", 0, 1, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                     .AddSequence("oddjobs@bailbond_hobotwitchy", "base", 0, 1, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                     .AddAttach("prop_gaffer_arm_bind", 60309, new Vector3(.09, .005, .0075), new Vector3(-67, -2, 114))
                     .AddSequenceCallback(ItemActions.Medicaments)
             );
            _scenes.Add(
                 (int)SceneNames.Whiskey,
                 new SceneModel()
                     .AddBaseAnim("amb@world_human_drinking@coffee@male@base", "base", 0, 1, AnimFlag.UpperBody, AnimFlag.Looped, AnimFlag.CanMove)
                     .AddAction("amb@world_human_drinking@coffee@male@idle_a", "idle_b", 0, 1, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                     .AddAttach("prop_drink_whisky", 28422, new Vector3(0,0,-.05), new Vector3(0,0,0))
                     .AddActionCallback(ItemActions.Aclohol)
             );
            _scenes.Add(
                 (int)SceneNames.Champange,
                 new SceneModel()
                    .AddEnterAnim("anim@amb@nightclub@mini@drinking@champagne_drinking@base@", "intro", .7f, 1, AnimFlag.UpperBody, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddBaseAnim("anim@amb@nightclub@mini@drinking@champagne_drinking@base@", "bottle_hold_idle", 0, 1, AnimFlag.UpperBody, AnimFlag.Looped, AnimFlag.CanMove)
                    .AddAction("anim@amb@nightclub@mini@drinking@champagne_drinking@base@", "outro", 0, .5f, AnimFlag.UpperBody, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddAttach("prop_champ_01b", 28422, new Vector3(0, 0, -.175), new Vector3(0, 0, 0))
                    .AddActionCallback(ItemActions.Aclohol)
            );
            _scenes.Add(
                 (int)SceneNames.RedWine,
                 new SceneModel()
                    .AddBaseAnim("anim@heists@heist_safehouse_intro@wine@window", "wine_window_idle", 0, 1, AnimFlag.UpperBody, AnimFlag.Looped, AnimFlag.CanMove)
                    .AddAction("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one", 0, .9f, AnimFlag.UpperBody, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddAttach("prop_drink_redwine", 28422, new Vector3(0.035, 0.0325, 0), new Vector3(-5, 1, 0))
                    .AddActionCallback(ItemActions.Aclohol)
            );
            _scenes.Add(
                 (int)SceneNames.Sunrise,
                 new SceneModel()
                     .AddBaseAnim("amb@world_human_drinking@coffee@male@base", "base", 0, 1, AnimFlag.UpperBody, AnimFlag.Looped, AnimFlag.CanMove)
                     .AddAction("amb@world_human_drinking@coffee@male@idle_a", "idle_b", 0, 1, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                    .AddAttach("prop_tequsunrise", 28422, new Vector3(0, 0, -.1275), new Vector3(0,0,0))
                    .AddActionCallback(ItemActions.Aclohol)
            );
            _scenes.Add(
                 (int)SceneNames.Tequila,
                 new SceneModel()
                     .AddBaseAnim("anim@heists@heist_safehouse_intro@wine@window", "wine_window_idle", 0, 1, AnimFlag.UpperBody, AnimFlag.Looped, AnimFlag.CanMove)
                    .AddAction("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one", 0, .9f, AnimFlag.UpperBody, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddAttach("prop_tequila", 28422, new Vector3(.005, .01, -.0225), new Vector3(-14, -7, -88))
                    .AddActionCallback(ItemActions.Aclohol)
            );
            _scenes.Add(
                 (int)SceneNames.WhiteWine,
                 new SceneModel()
                     .AddBaseAnim("anim@heists@heist_safehouse_intro@wine@window", "wine_window_idle", 0, 1, AnimFlag.UpperBody, AnimFlag.Looped, AnimFlag.CanMove)
                    .AddAction("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one", 0, .9f, AnimFlag.UpperBody, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddAttach("prop_drink_whtwine", 28422, new Vector3(.025, .015, 0), new Vector3(-9, 6, 0))
                    .AddActionCallback(ItemActions.Aclohol)
            );
            _scenes.Add(
                 (int)SceneNames.Daiquiri,
                 new SceneModel()
                     .AddBaseAnim("anim@heists@heist_safehouse_intro@wine@window", "wine_window_idle", 0, 1, AnimFlag.UpperBody, AnimFlag.Looped, AnimFlag.CanMove)
                    .AddAction("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one", 0, .9f, AnimFlag.UpperBody, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddAttach("prop_daiquiri", 28422, new Vector3(.005, .01, -.0225), new Vector3(-14, -7, -88))
                    .AddActionCallback(ItemActions.Aclohol)
            );
            _scenes.Add(
                 (int)SceneNames.Mojito,
                 new SceneModel()
                     .AddBaseAnim("amb@world_human_drinking@coffee@male@base", "base", 0, 1, AnimFlag.UpperBody, AnimFlag.Looped, AnimFlag.CanMove)
                     .AddAction("amb@world_human_drinking@coffee@male@idle_a", "idle_b", 0, 1, AnimFlag.UpperBody, AnimFlag.StopLastFrame, AnimFlag.CanMove)
                    .AddAttach("prop_mojito", 28422, new Vector3(0, 0, -.14), new Vector3(0, 0, 0))
                    .AddActionCallback(ItemActions.Aclohol)
            );

            _scenes.Add(
                (int)SceneNames.Vodka,
                new SceneModel()
                    .AddBaseAnim("anim@amb@nightclub@mini@drinking@drinking_shots@ped_c@normal", "glass_hold", 0, 1, AnimFlag.CanMove, AnimFlag.Looped, AnimFlag.UpperBody)
                    .AddAction("anim@amb@nightclub@mini@drinking@drinking_shots@ped_c@normal", "drink", 0, .4f, AnimFlag.CanMove, AnimFlag.StopLastFrame, AnimFlag.UpperBody)
                    .AddAttach("prop_shot_glass", 6286, new Vector3(.0675, -.0025, -.0525), new Vector3(-67, 1, 0))
                    .AddActionCallback(ItemActions.Aclohol)
            );

            _scenes.Add(
                (int)SceneNames.Rom,
                new SceneModel()
                    .AddBaseAnim("anim@amb@casino@hangout@ped_female@stand_withdrink@01a@base", "base", 0, 1, AnimFlag.CanMove, AnimFlag.Looped, AnimFlag.UpperBody)
                    .AddAction("anim@amb@casino@hangout@ped_female@stand_withdrink@01a@idles", "idle_a", 0, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame, AnimFlag.UpperBody)
                    .AddAttach("prop_rum_bottle", 60309, new Vector3(-.03,-.03,-.1875), new Vector3(-5, 16, -1))
                    .AddActionCallback(ItemActions.Aclohol)
            );

            _scenes.Add(
                (int)SceneNames.Cognak,
                new SceneModel()
                    .AddBaseAnim("anim@amb@casino@hangout@ped_female@stand_withdrink@01a@base", "base", 0, 1, AnimFlag.CanMove, AnimFlag.Looped, AnimFlag.UpperBody)
                    .AddAction("anim@amb@casino@hangout@ped_female@stand_withdrink@01a@idles", "idle_a", 0, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame, AnimFlag.UpperBody)
                    .AddAttach("prop_rum_bottle", 60309, new Vector3(.0025, -.005, -.225), new Vector3(2, 7, 1))
                    .AddActionCallback(ItemActions.Aclohol)
            );

            _scenes.Add(
                (int)SceneNames.Chacha,
                new SceneModel()
                    .AddBaseAnim("anim@amb@casino@hangout@ped_female@stand_withdrink@01a@base", "base", 0, 1, AnimFlag.CanMove, AnimFlag.Looped, AnimFlag.UpperBody)
                    .AddAction("anim@amb@casino@hangout@ped_female@stand_withdrink@01a@idles", "idle_a", 0, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame, AnimFlag.UpperBody)
                    .AddAttach("prop_tequila_bottle", 60309, new Vector3(-.0275, -.0075, -.28), new Vector3(-1, 10, -0))
                    .AddActionCallback(ItemActions.Aclohol)
            );

            _scenes.Add(
                (int)SceneNames.SeatSeed,
                new SceneModel()
                    .AddBaseAnim("special_ped@griff@trevor_1@trevor_1a", "convo_trevor_whatareyoudoing_0", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("p_cs_papers_01", 6286, new Vector3(0.0375, 0.005, -0.06), new Vector3(-150, -20, 0))
                    .AddAction("amb@world_human_gardener_plant@female@idle_a", "idle_a_female", 0, 0.5F, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddActionCallback(ItemActions.SeatSeed)
            );

            _scenes.Add(
                (int)SceneNames.WateringSeed,
                new SceneModel()
                    .AddBaseAnim("switch@trevor@digging", "001433_01_trvs_26_digging_idle", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("prop_wateringcan", 6286, new Vector3(0.2225, -0.19, -0.145), new Vector3(-178, -60, -66))
                    .AddAction("weapon@w_sp_jerrycan", "fire", 0, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame, AnimFlag.UpperBody)
                    .AddActionCallback(ItemActions.WateringSeed)
            );
            //mp.events.call("devattach", "special_ped@griff@trevor_1@trevor_1a", "convo_trevor_whatareyoudoing_0", 49, 6286, '["prop_cs_sack_01"]')
            //mp.events.call("devattach", "gestures@m@car@truck@casual@ds", "gesture_shrug_soft", 49, 6286, '["ng_proc_crate_03a"]')

            _scenes.Add(
                (int)SceneNames.Fertilizing,
                new SceneModel()
                    .AddBaseAnim("special_ped@griff@trevor_1@trevor_1a", "convo_trevor_whatareyoudoing_0", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("prop_cs_sack_01", 6286, new Vector3(0.02, -0.065, -0.1225), new Vector3(166, -276, 41))
                    .AddAction("amb@world_human_gardener_plant@female@idle_a", "idle_a_female", 0, 0.1F, AnimFlag.CanMove, AnimFlag.StopLastFrame, AnimFlag.UpperBody)
                    .AddActionCallback(ItemActions.FertilizeringSeed)
            );

            _scenes.Add(
                (int)SceneNames.Harvesting,
                new SceneModel()
                    .AddBaseAnim("gestures@m@car@truck@casual@ds", "gesture_shrug_soft", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("ng_proc_crate_03a", 6286, new Vector3(0.1125, -0.365, -0.215), new Vector3(-65, -28, -31))
                    .AddAction("anim@heists@money_grab@briefcase", "put_down_case", 0, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame, AnimFlag.UpperBody)
                    .AddActionCallback(ItemActions.Harvesting)
            );

            _scenes.Add(
                (int)SceneNames.WeedSeed,
                new SceneModel()
                .AddSequence("amb@world_human_gardener_plant@female@idle_a", "idle_a_female", 0, 0.6F, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                .AddSequenceCallback(WeedFarmService.PlaceSeedAction)
            ); 
            
             _scenes.Add(
                (int)SceneNames.WeedGrow,
                new SceneModel()
                .AddSequence("anim@amb@business@weed@weed_inspecting_lo_med_hi@", "weed_crouch_checkingleaves_idle_01_inspector", 0, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                .AddSequenceCallback(WeedFarmService.PlaceGrowAction)
            );

            _scenes.Add(
                (int)SceneNames.WeedDelivery,
                new SceneModel()
                .AddSequence("anim@narcotics@trash", "drop_front", 0, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                .AddSequenceCallback(WeedFarmService.DeliveryJobAction)
            );

            _scenes.Add(
                (int)SceneNames.DynamitePlant,
                new SceneModel()
                    .AddBaseAnim("gestures@m@car@truck@casual@ds", "gesture_shrug_soft", 0, 1, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("stt_prop_c4_stack", 64097, new Vector3(0.1375, -0.075, -0.035), new Vector3(-27.02, -8.02, -56))
                    //.AddAttach("stt_prop_c4_stack", 6286, new Vector3(0.0375, 0.005, -0.06), new Vector3(-150, -20, 0))
                    .AddAction("weapons@first_person@aim_rng@generic@projectile@thermal_charge@", "plant_floor", 0, 1, AnimFlag.CanMove, AnimFlag.StopLastFrame)
                    .AddActionCallback(ItemActions.DynamitePlant)
            );

            _scenes.Add(
                (int)SceneNames.ScannerUse,
                new SceneModel()
                    .AddBaseAnim("move_characters@sandy@texting", "sandy_text_loop_base", 0, 0, AnimFlag.Looped, AnimFlag.CanMove, AnimFlag.UpperBody)
                    .AddAttach("w_am_digiscanner", 6286, new Vector3(0.115, 0.0625, -0.0075), new Vector3(-74.02, -54.02, -120))
                    .AddActionOnStart(Jobs.SteelMaking.OreMining.OnUseDetector)
                    .AddActionOnFinish(Jobs.SteelMaking.OreMining.OnStopUseDetector)
            );
        }   

        public SceneModel this[int name]
        {
            get
            {
                return _scenes[name];
            }
        }
        public SceneModel this[SceneNames name]
        {
            get
            {
                return _scenes[(int)name];
            }
        }

        public void Parse()
        {
            if (Directory.Exists("client/configs"))
            {
                using var r1 = new StreamWriter("client/configs/scenes.js");
                r1.Write($"module.exports = {JsonConvert.SerializeObject(_scenes, Formatting.Indented)}");
            }
        }
    }
}
