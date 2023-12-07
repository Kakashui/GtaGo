//global.debug = true;

global.sendException = false;

global.chatActive = false;
global.loggedin = false;
global.localplayer = mp.players.local;
global.cursorTarget = null;
global.nearestObject = null;
global.friends = {};
global.Keys = {};
global.pidrgm = false;
global.inGreenZone = false;
global.cuffed = false;
global.spectating = false;
global.acheat = {};
global.bodyCam = null;
global.bodyCamStart = new mp.Vector3(0, 0, 0);
global.lastCheck = 0;
global.chatLastCheck = 0;
global.pocketEnabled = false;
global.openOutType = -1;
global.inventoryOpened = false;
global.editing = false;
global.LOCAL_ADMIN_LVL = 0;
global.circleEntity = null;
global.circleOpen = false;
global.boardObjects = [];
global.passports = {};
global.customCamera = null;
global.esptoggle = 0;
global.fishingMiniGame = false;
global.clothesEmpty = JSON.parse(`{"1":{"1":0,"3":15,"4":21,"5":0,"6":34,"7":0,"8":15,"9":0,"10":0,"11":15},"0":{"1":0,"3":15,"4":15,"5":0,"6":35,"7":0,"8":6,"9":0,"10":0,"11":15}}`);
global.characterEditor = false;
global.bizPedLoaded = false;
global.clientPedLoaded = false;
global.frontendSoundsEnabled = true;
global.customWeaponsModels = [];
global.editorPosition = new mp.Vector3(-811.697, 175.1083, 76.74536);
global.editorRotation = new mp.Vector3(-0.01, 0.01, 294.01);
global.inAction = false;
global.cursorShow = false;
global.UUID = 0;
global.console = mp.console;


mp.events.add('gui:ready', () => {
    global.passports = mp.storage.data.passports || {};
});

mp.events.add('setUUID', (uuid) => {
    global.UUID = uuid;
});

mp.events.add('SendClientExceptions', (value) => {
    global.sendException = value;
});


mp.serverLog = (log)=>{
    mp.events.callRemote('srv_consoleLog', log);
}

mp.gui.notify = (type, text, ms = 3000) => {
    mp.events.call('notify', type, 9, text, ms);
}

mp.gui.notifyType = {
    ALERT: 0,
    ERROR: 1,
    SUCCESS: 2,
    INFO: 3,
    WARNING: 4
}

global.checkIsAnyActivity = () => {
    return !global.loggedin || global.chatActive || global.editing || global.gui.isOpened() || global.IsPlayingDM || global.cuffed || global.inAction;
};


global.getVariable = (entity, key, defaultValue) => {
    if (entity) {
        let value = entity.getVariable(key);
        if (value || value === 0)
            return value;
    }
    return defaultValue;
}

let tempHide = false;
setInterval(()=>{
    if(tempHide && !mp.game.ui.isPauseMenuActive()){
        tempHide = false;
        mp.gui.cursor.visible = true;//global.gui.curPage 
    }
}, 100)


global.showCursor = (state) => {
    if (!global.loggedin)
        mp.gui.cursor.visible = true;
    else{
        mp.gui.cursor.visible = state;
    }
}

global.GetOffsetPosition = (pos, rotZ, offset) =>
{
    let newPos = new mp.Vector3(pos.x, pos.y, pos.z + offset.z);
    let dist = Math.sqrt(offset.x * offset.x + offset.y * offset.y)
    let offsetUnit = new mp.Vector3(offset.x / dist, offset.y / dist, 0);
    let acosA = Math.acos(offsetUnit.x);
    let asinA = Math.asin(offsetUnit.y);
    let A = (asinA >= 0) ? acosA : -acosA;
    A += (rotZ * Math.PI / 180);
    newPos.x += Math.cos(A) * dist;
    newPos.y += Math.sin(A) * dist;

    return newPos;
}