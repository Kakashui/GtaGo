mp.keys.bind(global.Keys.Key_E, false, actionButton); //e

//data handler
mp.events.addDataHandler("fshActive", setFishingActive);

//remote events
mp.events.add("fshShow", showFisingAction);
mp.events.add("fshFShop", openFishTraderPage);
mp.events.add("fshUpCage", updateCage);
mp.events.add("fshMGStart", startMiniGame);
mp.events.add("fshDSpots", deleteSeaSpots);
mp.events.add("fshUSpots", updateSeaSpots);

//cef events
mp.events.add("cef_buyGear", buyGear);
mp.events.add("cef_dropFish", dropFish);
mp.events.add("cef_endMiniGame", endMG);



mp.events.add("render",()=>{
    if(mp.players.local.rod !== undefined) {
        mp.game.controls.disableControlAction(0, 30, true);
        mp.game.controls.disableControlAction(0, 31, true);
        mp.game.controls.disableControlAction(0, 32, true);
        mp.game.controls.disableControlAction(0, 33, true);
        mp.game.controls.disableControlAction(0, 34, true);
        mp.game.controls.disableControlAction(0, 35, true);
        mp.game.controls.disableControlAction(0, 45, true);
        mp.game.controls.disableControlAction(0, 69, true);
        mp.game.controls.disableControlAction(0, 140, true);
        mp.game.controls.disableControlAction(0, 141, true);
        mp.game.controls.disableControlAction(0, 142, true);
    }
})

function checkTick(){
    if(fishers.length > 0){
        for (let index = 0; index < fishers.length; index++) {
            const fisher = fishers[index];
            if(!fisher.rod) fishers.splice(index, 1);
        }
    }
}

const fishers = [];
let checkTimer = Date.now();
let checkPeriod = 1;

global.fishingMiniGame = false;
let lastPress = Date.now();
const FISHING_ROD_MODEL = "prop_fishing_rod_01";
const BONE_LEFTHAND_ID = 60309;
mp.players.local.rod = undefined;
let seaBlips = [];

const SPOT_RADIUS_IN_SEA = 20;
const BLIP_SPOT_SPRITE = 317;
const BLIP_SPOT_COLOR = 29;
const BLIP_SPOT_SPRITE_CIRCLE = 1;

const ANIM_DICT = "amb@world_human_stand_fishing@idle_a";
const ANIM_NAME = "idle_c";

mp.game.streaming.requestAnimDict(ANIM_DICT);
/**
 * comon functions
 */
function actionButton(){
    if(!global.loggedin || global.isPhoneOpened || global.inAction) return;
    if (gui.isOpened()) return;
    if(mp.players.local.isSwimming()) return;
    if(mp.players.local.vehicle) return;
    if(lastPress > Date.now()) {
        return ;
    }
    lastPress = Date.now() + 1000;
    mp.events.callRemote("fshAction");
}

async function setFishingActive(entity, value){
    if(entity.type !== 'player' || !mp.players.exists(entity)) return;
    switch (value) {
        case 0:
            if(entity.rod) {
                entity.rod.destroy();
                entity.rod = undefined;
            }
            break;
        case 1:
            if(entity.rod) entity.rod.destroy();
            entity.rod = mp.objects.new(mp.game.joaat(FISHING_ROD_MODEL), entity.position ,{alpha: 255, dimension: entity.dimension});
            while (entity.rod && !entity.rod.doesExist()) {
                await mp.game.waitAsync(0);
            }
            if(!entity.rod) return;
            entity.rod.attachTo(entity.handle, entity.getBoneIndex(BONE_LEFTHAND_ID), 0, -0.02, 0, 0, -10, 20, true, true, false, false, 0, true);
            entity.taskPlayAnim(ANIM_DICT, ANIM_NAME, 8.0, 1.0, -1, 2, 1.0, false, false, false);
            break;
        case 2:
            entity.taskPlayAnim(ANIM_DICT, ANIM_NAME, 8.0, 1.0, -1, 3, 1.0, false, false, false);
            break;
        default:
            break;
    }
}
let opened = false;

mp.keys.bind(global.Keys.Key_ESCAPE, false, function() {
    if (global.gui.curPage == "FishingStore") {
        global.gui.close();
    }
});

function setData(name, data){
    global.gui.setData('fishing/setData', {name, data});
}
/**
 * end comon functions
 */


/**
 * remote events
*/
function showFisingAction(show){
    if(!show && mp.players.local.rod) {
        mp.players.local.rod.destroy();
        mp.players.local.rod = undefined;
    }
    global.fishingMiniGame = show;
    global.gui.setData('fishing/setShowAction', `${global.fishingMiniGame}`);
}

function openFishTraderPage(cageData){
    global.gui.setData("fishingStore/setCageItems", JSON.stringify(cageData));
    global.gui.openPage("FishingStore");
}

function updateCage(data){   
    global.gui.setData('fishingStore/setCageItems', JSON.stringify(data));
}

function startMiniGame(difficult){
    global.fishingMiniGame = true;
    global.gui.opened = true;
    global.gui.setData('fishing/startMinigame', `'${difficult}'`)
}

function deleteSeaSpots(){
    seaBlips.forEach(blip => {
        blip.destroy();
    });
    seaBlips = [];
}

function updateSeaSpots(positions){
    if(seaBlips.length > 0) deleteSeaSpots();
    const seaSpots = JSON.parse(positions);
    seaSpots.forEach(pos => {
        seaBlips.push(mp.blips.new(BLIP_SPOT_SPRITE, new mp.Vector3(pos.x, pos.y, pos.z), {color: BLIP_SPOT_COLOR, name: "client_12", scale: 2, shortRange: true}));
    });    
    mp.events.call('notify', 4, 9, "client_13", 3000);
}
/**
 * end remote events
*/


/**
 * cef events
*/

function buyGear(id){
    if(lastPress > Date.now()) {
        return ;
    }
    lastPress = Date.now() + 1000;
    mp.events.callRemote("fshBuyGear", id);
}

function dropFish(id, count){
    mp.events.callRemote("fshDropFish", id, count);
}

function cellFish(id, count){
    mp.events.callRemote("fshCellFish", id, count);
}

function endMG(status){
    global.fishingMiniGame = false;
    global.gui.opened = false;
    mp.events.callRemote("fshMGEnd", status);
}