const props = [
    'weed_standard_equip',
    'weed_upgrade_equip',
    'weed_low_security',
    'weed_security_upgrade',
    'weed_set_up',
    'weed_chairs',
    'weed_production',
    'weed_drying',
    'weed_hosea',
    'weed_hoseb',
    'weed_hosec',
    'weed_hosed',
    'weed_hosee',
    'weed_hosef',
    'weed_hoseg',
    'weed_hoseh',
    'weed_hosei',
    'weed_growtha_stage1',
    'weed_growtha_stage2',
    'weed_growtha_stage3',
    'weed_growthb_stage1',
    'weed_growthb_stage2',
    'weed_growthb_stage3',
    'weed_growthc_stage1',
    'weed_growthc_stage2',
    'weed_growthc_stage3',
    'weed_growthd_stage1',
    'weed_growthd_stage2',
    'weed_growthd_stage3',
    'weed_growthe_stage1',
    'weed_growthe_stage2',
    'weed_growthe_stage3',
    'weed_growthf_stage1',
    'weed_growthf_stage2',
    'weed_growthf_stage3',
    'weed_growthg_stage1',
    'weed_growthg_stage2',
    'weed_growthg_stage3',
    'weed_growthh_stage1',
    'weed_growthh_stage2',
    'weed_growthh_stage3',
    'weed_growthi_stage1',
    'weed_growthi_stage2',
    'weed_growthi_stage3',
    'light_growtha_stage23_standard',
    'light_growthb_stage23_standard',
    'light_growthc_stage23_standard',
    'light_growthd_stage23_standard',
    'light_growthe_stage23_standard',
    'light_growthf_stage23_standard',
    'light_growthg_stage23_standard',
    'light_growthh_stage23_standard',
    'light_growthi_stage23_standard',
    'light_growtha_stage23_upgrade',
    'light_growthb_stage23_upgrade',
    'light_growthc_stage23_upgrade',
    'light_growthd_stage23_upgrade',
    'light_growthe_stage23_upgrade',
    'light_growthf_stage23_upgrade',
    'light_growthg_stage23_upgrade',
    'light_growthh_stage23_upgrade',
    'light_growthi_stage23_upgrade'
];

const interiorID = mp.game.interior.getInteriorAtCoords(1051.491, -3196.536, -39.14842);

global.checkFarm = () => {
    if(mp.game.interior.getInteriorAtCoords(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z) === interiorID)
        mp.events.callRemote("weedfarm:instance:check");
}

const weedPalet = mp.game.joaat("prop_weed_pallet");
let palet = null;

props.forEach(prop => {
    mp.game.interior.disableInteriorProp(interiorID, prop);
});

mp.game.interior.refreshInterior(interiorID);
props.length = 0;
const letters = {
    "0": "a",
    "1": "b",
    "2": "c",
    "3": "d",
    "4": "e",
    "5": "f",
    "6": "g",
    "7": "h",
    "8": "i",
}
const componentList = { 
    "0": "weed_set_up",
    "1": "weed_security_upgrade",
    "2": "weed_upgrade_equip",
    "3": "weed_chairs"
}
function loadComponentsToVehicle(time) {    
    mp.players.local.freezePosition(true);
    global.controlsManager.disableAll();
    mp.events.call('notify', 3, 9, `weedfarm:veh:load@${time}`, 3000);
    setTimeout(()=>{
        mp.players.local.freezePosition(false);
        global.controlsManager.enableAll();
        mp.events.call('notify', 3, 9, `weedfarm:veh:load:ok`, 3000);
    },  time * 1000);
}
let places = [];

function updateFarmInstance(pl, components, onDrying, onPacking, onDelivery, irrigation, light, drying, ventilation) {
    try {
        mp.game.cam.doScreenFadeOut(0);
        places = pl;
        global.gui.setData("setLoadScreen", true);
        // if(!mp.game.streaming.isIplActive("bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo")){
        //     mp.game.streaming.requestIpl("bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo");
        //     while (!mp.game.streaming.isIplActive("bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo")) {
        //         mp.game.wait(0);
        //     }
        // }
        props.forEach(prop=>
            mp.game.interior.disableInteriorProp(interiorID, prop)
        )
        props.length = 0;
        places.forEach((place, index) => {
            if(place > 0){
                props.push(`weed_growth${letters[index]}_stage${place}`);
                props.push(`weed_hose${letters[index]}`);
            }            
        });
        components.forEach(comp => {
            if(componentList[comp])
                props.push(componentList[comp]);
        });

        if(onDrying > 0){
            props.push("weed_drying");
        }

        if(onPacking > 0){
            props.push("weed_production");
        }
        props.forEach(prop => {
            mp.game.interior.enableInteriorProp(interiorID, prop);
        });
        mp.game.interior.refreshInterior(interiorID);
        const onGrowing = places.filter(p=>p > 0).length;
        global.gui.setData("weedFarm/updateFarmData", JSON.stringify({components, onGrowing, onDrying, onPacking, onDelivery, irrigation, light, drying, ventilation}));
        setTimeout(()=>{
            mp.game.cam.doScreenFadeIn(350);
            global.gui.setData("setLoadScreen", false);
            if(palet === null){
                if(!mp.game.streaming.hasModelLoaded(weedPalet)){
                    mp.game.streaming.requestModel2(weedPalet);
                    while (!mp.game.streaming.hasModelLoaded(weedPalet)) {
                        mp.game.wait(0);
                    }
                }
                palet = mp.objects.new(weedPalet, new mp.Vector3(1041.32, -3192.605, -39), {dimension: mp.players.local.dimension});
            }
        }, 1500);
    } catch (error) {
        mp.serverLog(error.message)
    }
}

function updateFarmInstanceComponents(component) {
    if(componentList[component]){
        props.push(componentList[component]);
        mp.game.interior.enableInteriorProp(interiorID, componentList[component]);
        mp.game.interior.refreshInterior(interiorID);
        global.gui.setData("weedFarm/updateComponents", component)
    }
}

let opened = false;

function openFarmMenu(OnDelivery) {
    updateFarmInstanceOnDelivery(OnDelivery);
    global.gui.openPage("WeedFarm");
    opened = true;
}

function close(){
    if(opened)
    {
        global.gui.close();
        opened = false;
    }
}

function updateFarmInstancePlace(index, stady) {
    const old = props.findIndex(p=>p.indexOf(`weed_growth${letters[index]}_stage`) !== -1);
    const hose = `weed_hose${letters[index]}`;
    if(stady === 0){
        if(old !== -1){
            mp.game.interior.disableInteriorProp(interiorID, props[old]);
            props.splice(old, 1);
            const oldHose = props.findIndex(p=>p === hose);
            if(oldHose !== -1){
                mp.game.interior.disableInteriorProp(interiorID, props[oldHose]);
                props.splice(oldHose, 1);
            }
        }
    }else{
        const newProp = `weed_growth${letters[index]}_stage${stady}`;
        if(old === -1)
            props.push(newProp);
        else{
            mp.game.interior.disableInteriorProp(interiorID, props[old])
            props.splice(old, 1, newProp);
        }
        mp.game.interior.enableInteriorProp(interiorID, newProp);
        if(props.findIndex(p=>p === hose) === -1){
            props.push(hose)
            mp.game.interior.enableInteriorProp(interiorID, hose);
        }
            
    }
    mp.game.interior.refreshInterior(interiorID);
    places[index] = stady;
    const onGrowing = places.filter(p=>p > 0).length;
    global.gui.setData("weedFarm/updateStady", JSON.stringify({key: "onGrowing", value: onGrowing}));
}

function setGpsPath(x, y){
    //mp.serverLog(`x: ${x} y: ${y}`);
    mp.game.ui.setNewWaypoint(1065.894, -3183.636);
    mp.events.call('notify', 2, 9, "weedfarm:comp:gps:ok", 3000);
    mp.events.callRemote("weedfarm:comp:path:set", x, y);
}

function updateFarmInstanceStadyState(prop, isDelete) {
    const exists = props.findIndex(p=>p === prop);
    if(isDelete){
        if(exists !== -1){
            mp.game.interior.disableInteriorProp(interiorID, prop);
            props.splice(exists, 1);
            mp.game.interior.refreshInterior(interiorID);
        };       
    }else{
        if(exists === -1){
            mp.game.interior.enableInteriorProp(interiorID, prop);
            props.push(prop);
            mp.game.interior.refreshInterior(interiorID);
        }
    }
}

function updateFarmInstanceOnDrying(count) {
    updateFarmInstanceStadyState('weed_drying', count < 1);    
    global.gui.setData("weedFarm/updateStady", JSON.stringify({key: "onDrying", value: count}));
}

function updateFarmInstanceOnPacking(count) {
    updateFarmInstanceStadyState('weed_production', count < 1);    
    global.gui.setData("weedFarm/updateStady", JSON.stringify({key: "onPacking", value: count}));
}

function updateFarmInstanceOnDelivery(count) {    
    global.gui.setData("weedFarm/updateStady", JSON.stringify({key: "onDelivery", value: count}));
}

function resetInstance() {
    props.forEach(prop => {
        mp.game.interior.disableInteriorProp(interiorID, prop);
    });
    mp.game.interior.refreshInterior(interiorID);    
    props.length = 0;
    places.length = 0;
    if(palet !== null){
        palet.destroy();
        palet = null;
    }
}

const seatPositions = {
    "0": {"pos":{"x":1039.20642,"y":-3205.952,"z":-39.14023}, "rot":{"x":0.0,"y":0.0,"z":92.0}},
    "1": {"pos":{"x":1037.34717,"y":-3205.80322,"z":-39.1210938}, "rot":{"x":0.0,"y":0.0,"z":-72.0}}
}

const animDict = "anim@amb@business@weed@weed_sorting_seated@";
const baseName = "base_sorter_left_sorter01";
const actionName = "sorter_left_sort_v1_sorter01";

let begineAction = false;
let playerOnPlace = false;
let spamProtection = 0;

function actionHandler() {
    if(begineAction){
        if(mp.players.local.isPlayingAnim(animDict, actionName, 3)){
            //global.debugText = `${mp.players.local.getAnimCurrentTime(animDict, actionName)}`;
            if(mp.players.local.getAnimCurrentTime(animDict, actionName) > .9){      
                //mp.players.local.clearTasks();      
                mp.players.local.taskPlayAnim(animDict, baseName, 8.0, 8.0, -1, 2, 0, false, false, false);
                begineAction = false;
            }  
        }        
    }
}

function onClick(x, y, upOrDown, leftOrRight) {
    if(!playerOnPlace || begineAction || spamProtection > Date.now() || upOrDown === "up" || leftOrRight === "right") return;
    spamProtection = Date.now() + 500;
    mp.events.callRemote("weedfarm:sort:action:request");
}

function startSortJob(){
    global.inAction = true;
    playerOnPlace = true;
    begineAction = false;
    mp.events.add("render", actionHandler);
    mp.events.add('click', onClick);
    controlsManager.disableAll(1,2);
}

function stopSortJob(){    
    global.inAction = false;
    playerOnPlace = false;
    begineAction = false;
    mp.events.remove("render", actionHandler);
    mp.events.remove("click", onClick);
    controlsManager.enableAll();
}

function seatToSortPlace(player, place){
    player.freezePosition(true);
    player.setCollision(false,true);
    player.setRotation(place.rot.x, place.rot.y, place.rot.z, 2, true);
    player.setCoords(place.pos.x, place.pos.y, place.pos.z, true, true, true, true);
    const animName = player === mp.players.local ? baseName :  actionName;
    const flag = player === mp.players.local ? 2 : 1;
    checkAnimDict();
    player.taskPlayAnim(animDict, animName, 8.0, 8.0, -1, flag, 0, false, false, false);
}

function releaseSortPlace(player){
    player.setCollision(true, true);
    player.freezePosition(false);
    player.clearTasksImmediately();
}

function seatPlaceHandler(player, key){
    if(player.type !== 'player' || !player.handle) return;
    if(seatPositions[key] === undefined){
        releaseSortPlace(player);
        if(mp.players.local === player)
            stopSortJob();
    }  
    else{
        seatToSortPlace(player, seatPositions[key]);
        if(mp.players.local === player)
            startSortJob();
    }        
}

function sortActionResponce(){
    checkAnimDict();
    //mp.players.local.clearTasks();
    mp.players.local.taskPlayAnim(animDict, actionName, 8.0, 8.0, -1, 2, 0, false, false, false);
    begineAction = true;
}

function checkAnimDict() {
    if(mp.game.streaming.hasAnimDictLoaded(animDict)) return;
    mp.game.streaming.requestAnimDict(animDict);
    while (!mp.game.streaming.hasAnimDictLoaded(animDict))
        mp.game.wait(0);
}

mp.events.add("entityStreamIn", (player)=>{
    try {        
        if (!player || player.type !== 'player') return;
        var key = global.getVariable(player,"weedfarm:seat", -1);
        if(seatPositions[key] !== undefined)
            seatToSortPlace(player, seatPositions[key]);
    } catch (e) {
        if(global.sendException) mp.serverLog(`weapon.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);        
    }
})

let csh = null;
let mrkr = null;
let workBlip = null;
let cshPos = new mp.Vector3();
let playerInPosition = false;
let playerOnDeliveryJob = false;

function getNextDeliveryPoint(x, y, z) {
    cshPos = new mp.Vector3(x,y,z);
    cancelDeliveryPoint();
    csh = mp.colshapes.newCircle(x, y, 1.5);
    mrkr = mp.markers.new(0, cshPos, 1);
    createWorkBlip(cshPos);
    playerOnDeliveryJob = true;
    global.gui.setData("weedFarm/updateOnDeliveryJob", playerOnDeliveryJob);
    //mp.events.call("phone::gps::setWaypoint", x, y);
}

function createWorkBlip(position){
    destroyWorkBlip();
    workBlip = mp.blips.new(496, position,
        {
            name: "Weed delivery",
            scale: 1.0,
            color: 52,
            alpha: 255,
            drawDistance: 100,
            shortRange: false,
            rotation: 0,
            dimension: 0
        });
    mp.game.invoke("0x4F7D8A9BFB0B43E9", workBlip.handle, true);    
    mp.events.call("notify", 3, 9, "weedfarm:delivery:next", 3000);
}

function destroyWorkBlip(){
    if (workBlip !== null){
        workBlip.destroy();
        workBlip = null;
    }
}

function cancelDeliveryPoint() {
    if(csh !== null) {
        csh.destroy();
        csh = null;
    }
    if(mrkr !== null) {
        mrkr.destroy();
        mrkr = null;
    }
    destroyWorkBlip();
    playerInPosition = false;
    const dto = { show: false, items: [] };
    global.gui.setData('hud/setPromptData', JSON.stringify(dto));
    playerOnDeliveryJob = false;
    global.gui.setData("weedFarm/updateOnDeliveryJob", playerOnDeliveryJob);
}

function playerEnterColshapeHandler(player, shape) {
    if(csh !== null && mp.game.system.vdist(cshPos.x, cshPos.y, cshPos.z, mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z) < 3){
        playerInPosition = true;
        const dto = { show: true, items: [{"key": "E", "text": "Сделать закладку"}] };        
        global.gui.setData('hud/setPromptData', JSON.stringify(dto));
    }
}

function playerExitColshapeHandler(player, shape) {
    if(playerInPosition){
        playerInPosition = false;
        const dto = { show: false, items: [] };
        global.gui.setData('hud/setPromptData', JSON.stringify(dto));
    }
}

mp.events.add("weedfarm:veh:load", loadComponentsToVehicle);
mp.events.add("weedfarm:menu:open", openFarmMenu);
mp.events.add("weedfarm:instance:update", updateFarmInstance);
mp.events.add("weedfarm:instance:comp:update", updateFarmInstanceComponents);
mp.events.add("weedfarm:instance:place:update", updateFarmInstancePlace);
mp.events.add("weedfarm:instance:drying:update", updateFarmInstanceOnDrying);
mp.events.add("weedfarm:instance:packing:update", updateFarmInstanceOnPacking);
mp.events.add("weedfarm:instance:delivery:update", updateFarmInstanceOnDelivery);
mp.events.add("weedfarm:delivery:next", getNextDeliveryPoint);
mp.events.add("weedfarm:delivery:cancel", cancelDeliveryPoint);
mp.events.add("weed:path:set", setGpsPath);
mp.events.add("weedfarm:instance:reset", resetInstance);
mp.events.add("weedfarm:sort:action:responce", sortActionResponce);
mp.events.add("playerEnterColshape", playerEnterColshapeHandler);
mp.events.add("playerExitColshape", playerExitColshapeHandler);
mp.events.addDataHandler("weedfarm:seat", seatPlaceHandler)

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        close();
    if(playerOnPlace && !begineAction)
        mp.events.callRemote("weedfarm:sort:cancel")
});
mp.keys.bind(global.Keys.Key_E, false, function () {
    if (!playerInPosition || global.inAction) return;
    mp.events.callRemote("weedfarm:delivery:action")
});