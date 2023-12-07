try {
    if(mp.tobj1 === undefined){
        const model = mp.game.joaat('prop_pencil_01');
        if(mp.game.streaming.isModelValid(model)){
            if(!mp.game.streaming.hasModelLoaded(model)){
                mp.game.streaming.requestModel2(model)
                while (mp.game.streaming.hasModelLoaded(model)) {
                    mp.game.wait(0);
                }
            }
            mp.tobj1 = mp.objects.new(model, mp.players.local.position);
            while (!mp.tobj1.doesExist()) {
                mp.game.wait(0)
            }
        }else{
            console.logInfo(`invalid model`)
        }
    }
    if(mp.tobj2 === undefined){
        const model = mp.game.joaat('prop_rum_bottle');
        if(mp.game.streaming.isModelValid(model)){
            if(!mp.game.streaming.hasModelLoaded(model)){
                mp.game.streaming.requestModel2(model)
                while (mp.game.streaming.hasModelLoaded(model)) {
                    mp.game.wait(0);
                }
            }
            mp.tobj2 = mp.objects.new(model, mp.players.local.position);
            while (!mp.tobj2.doesExist()) {
                mp.game.wait(0)
            }
        }else{
            console.logInfo(`invalid model`)
        }
    }
    mp.tobj1.attachTo(mp.players.local.handle, mp.players.local.getBoneIndex(28422),0,0,0,0,0,0,false, false, false, false,2,true);
    mp.tobj2.attachTo(mp.players.local.handle, mp.players.local.getBoneIndex(28422),0,0,0,0,0,0,false, false, false, false,2,true);
} catch (e) {
    console.logInfo(`${e.name} ${e.message} ${e.stack}`)
}


const model = mp.game.joaat("p_amb_clipboard_01");
if(mp.game.streaming.isModelValid(model)){
    if(!mp.game.streaming.hasModelLoaded(model)){
        mp.game.streaming.requestModel2(model)
        while (mp.game.streaming.hasModelLoaded(model)) {
            mp.game.wait(0);
        }
    }
    mp.tobj2.model = model;
}else console.logInfo(`invalid model`)


try {
    if(mp.tobj1 === undefined){
        const model = mp.game.joaat('prop_rum_bottle');
        if(mp.game.streaming.isModelValid(model)){
            if(!mp.game.streaming.hasModelLoaded(model)){
                mp.game.streaming.requestModel2(model)
                while (mp.game.streaming.hasModelLoaded(model)) {
                    mp.game.wait(0);
                }
            }
            mp.tobj1 = mp.objects.new(model, mp.players.local.position);
            while (!mp.tobj1.doesExist()) {
                mp.game.wait(0)
            }
        }else{
            console.logInfo(`invalid model`)
        }
    }
    
    mp.tobj1.attachTo(mp.players.local.handle, mp.players.local.getBoneIndex(60309),0,0,0,0,0,0,false, false, false, false,2,true);
} catch (e) {
    console.logInfo(`${e.name} ${e.message} ${e.stack}`)
}




/*roulette chances
/chanceroulette 0 5000 3500 500 840 50 5
/chanceroulette 1 4000 4000 1000 800 200 10
/chanceroulette 2 3000 4000 1500 1000 250 50
/chanceroulette 3 2000 4000 2000 1500 400 100
*/
const pos = mp.players.local.position;
const interior = mp.game.interior.getInteriorAtCoords(pos.x, pos.y, pos.z);
mp.game.interior.enableInteriorProp(interior, "Card_Suit_Diamonds");
mp.game.interior.refreshInterior(interior);



mp.game.streaming.requestIpl("vw_casino_main");
mp.game.streaming.requestIpl("hei_dlc_windows_casino");
mp.game.streaming.requestIpl("hei_dlc_casino_aircon");
mp.game.streaming.requestIpl("hei_dlc_casino_door");
mp.game.streaming.requestIpl("vw_dlc_casino_door");


mp.game.streaming.removeIpl("vw_casino_main");
mp.game.streaming.removeIpl("hei_dlc_windows_casino");
mp.game.streaming.removeIpl("hei_dlc_casino_aircon");
mp.game.streaming.removeIpl("hei_dlc_casino_door");
mp.game.streaming.removeIpl("vw_dlc_casino_door");

const luckyWheelHash = mp.game.joaat("vw_prop_vw_luckywheel_02a")
mp.game.streaming.requestModel(luckyWheelHash);

while(!mp.game.streaming.hasModelLoaded(luckyWheelHash)){
    mp.game.wait(100);
}

luckyWheelObject = mp.objects.new(luckyWheelHash, new mp.Vector3(1111.052, 229.8579, -49.133));
mp.game.streaming.setModelAsNoLongerNeeded(luckyWheelHash);


const SCREEN_DIAMONDS = "CASINO_DIA_PL";
const SCREEN_SKULLS = "CASINO_HLW_PL";
const SCREEN_SNOW = "CASINO_SNWFLK_PL";
const SCREEN_WIN = "CASINO_WIN_PL";

const targetName = "casinoscreen_01";
const targetModel = mp.game.joaat('vw_vwint01_video_overlay');
const textureDict = "Prop_Screen_Vinewood";
const textureName = "BG_Wall_Colour_4x4";

const player = mp.players.atRemoteId(1);
player.taskPlayAnim("amb@code_human_wander_drinking@beer@male@base", "static", 8.0, 0, -1, 49, 0, false, false, false);

mp.game.cam.shakeGameplayCam("DRUNK_SHAKE", 1.0);
mp.game.cam.stopGameplayCamShaking(true);


mp.game.graphics.stopScreenEffect("SwitchHUDIn");
mp.game.graphics.startScreenEffect("SwitchHUDIn", 1000, true);

mp.events.call('particles:playAtPosition', mp.players.local.position, 'core', 'ent_dst_elec_fire', .7, 1000);

mp.game.cam.shakeGameplayCam("FAMILY5_DRUG_TRIP_SHAKE", 2.0);
mp.game.cam.stopGameplayCamShaking(true);

mp.players.local.setMoveRateOverride(0.5);

mp.canSprint = true;
mp.events.add("render", ()=>{
    if(!mp.canSprint){
        mp.game.controls.disableControlAction(0, 21, true);
    }
})

mp.peds.new().taskPlayAnimAdvanced()
mp.players.local.taskPlayAnimAdvanced
mp.players.local.taskPlayAnim("amb@code_human_wander_drinking@beer@male@base", "static", 8.0, 0, -1, 50, 0, false, false, false);

const syncPropModel = mp.game.joaat("prop_cs_ciggy_01");
if(!mp.game.streaming.hasModelLoaded(syncPropModel)){
    mp.game.streaming.requestModel2(syncPropModel)
    while (mp.game.streaming.hasModelLoaded(syncPropModel)) {
        mp.game.wait(0);
    }
}
mp.syncProp = mp.objects.new(syncPropModel, mp.players.local.position, {alpha: 255, dimension: Number.MAX_SAFE_INTEGER})
mp.syncProp.dimension = Number.MAX_SAFE_INTEGER


mp.game.streaming.requestAnimDict("switch@michael@sitting");

const player = mp.players.atRemoteId(1);
player.clearTasksImmediately();

const player = mp.players.atRemoteId(1);
player.taskPlayAnim("amb@code_human_wander_drinking@beer@male@base", "static", 8.0, 0, -1, 49, 0, false, false, false);

try {

    mp.game.streaming.requestAnimDict("switch@michael@sitting");
    const player = mp.players.atRemoteId(1);   
    player.taskPlayAnim("switch@michael@sitting", "idle", 8.0, 0, -1, 1, 0, false, false, false);
    
    // player.taskPlayAnimAdvanced("switch@michael@sitting", "idle", 
    //     player.position.x, player.position.y, player.position.z,
    //     0,0,0,
    //     8, 1, -1, 4097, -1, 0, 0
    // );

} catch (e) {
    console.logInfo(`${e.name} ${e.message} ${e.stack}`)
}

const pos = new mp.Vector3(-1874.3005, -1209.0021, 12.5397);
const rot = new mp.Vector3(0,0,56);
mp.players.local.setCollision(false, false);
mp.players.local.taskPlayAnim("switch@michael@sitting", "idle", 8.0, 0, -1, 1, 0, false, false, false);
mp.players.local.setRotation(rot.x, rot.y, rot.z, 2, true)
mp.players.local.setCoords(pos.x, pos.y, pos.z, true,true,true, false);

const pos = mp.players.local.position;
enviromentManager.syncProp.setCoords(pos.x, pos.y, pos.z, true, true, true, true);
console.logInfo(enviromentManager.syncProp.doesExist())

enviromentManager.syncProp.destroy();
mp.players.local.setNoCollision(mp.players.local.handle, false);

const pos = mp.players.local.position;
mp.players.local.taskStartScenarioAtPosition("PROP_HUMAN_SEAT_BENCH",pos.x,pos,pos.z,90,-1,!0,!1)

const pos = mp.players.local.position;
mp.objects.new(-1091386327, pos);

mp.events.call("devattach", "anim@amb@business@weed@weed_sorting_seated@", "sorter_left_sort_v1_sorter01", 49, 28422, '["bkr_prop_weed_bud_02c"]')  // 36029 60309    6286 28422
mp.events.call("customCameraOff")

setClothing(mp.players.local, 2, 2, 36, 36);
mp.players.local.setHairColor(36, 0);

mp.events.call("customization:update", "beard", 2)
mp.events.call("customization:update", "beardColor1", 32)
mp.events.call("customization:update", "beardColor2", 32)

mp.players.local.setHeadOverlayColor(4, 2, 0, 2);
mp.players.local.setHeadOverlay(9, 1, 255,  0, 0);

mp.game.streaming.requestIpl("tr_tuner_meetup");
mp.game.streaming.requestIpl("TunerGarage");
mp.game.streaming.requestIpl("tr_tuner_race_line");
mp.game.streaming.requestIpl("tr_tuner_shop_burton");
mp.game.streaming.requestIpl("tr_tuner_shop_mesa");
mp.game.streaming.removeIpl("tr_tuner_shop_mission");
mp.game.streaming.removeIpl("tr_tuner_shop_rancho");


mp.game.streaming.removeIpl("bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo");


let interiorID = mp.game.interior.getInteriorAtCoords(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z);
console.logInfo(interiorID)
mp.game.interior.disableInteriorProp(interiorID, "weed_growthb_stage3");
mp.game.interior.enableInteriorProp(interiorID, "entity_set_style_2");
mp.game.interior.refreshInterior(interiorID);

let interiorID = mp.game.interior.getInteriorAtCoords(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z);
mp.game.interior.enableInteriorProp(interiorID, "weed_standard_equip");
mp.game.interior.refreshInterior(interiorID);


let interiorID = mp.game.interior.getInteriorAtCoords(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z);
mp.game.interior.disableInteriorProp(interiorID, "weed_upgrade_equip");
mp.game.interior.disableInteriorProp(interiorID, "weed_upgrade_equip");
mp.game.interior.refreshInterior(interiorID);

global.debugText = `${mp.game.object.getClosestObjectOfType(pos.x, pos.y, pos.z, 4, -1969563019, false, true, true)}`
//"v_corp_facebeanbagd"
//"bkr_prop_clubhouse_sofa_01a"

//"prop_bench_01a"