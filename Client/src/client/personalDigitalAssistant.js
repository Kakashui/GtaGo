let opened = false;
let lastcheck = Date.now();

mp.keys.bind(global.Keys.Key_OEM_7, false, function () {
    if (!opened)
        OpenMenu()
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});


mp.events.add('pda:exit', () => {
    CloseMenu();
});

function OpenMenu() {
    if (global.checkIsAnyActivity()) return;
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    mp.events.callRemote('pda:pressOpenMenu');
}

function CloseMenu() {
    global.gui.close();
    opened = false;
    mp.events.callRemote('pda:closeMenu');
}

mp.events.add('pda:openAndLoad', (frac, wantedPlayer, wantedVehicles, helpList) => {
    if (global.chatActive || global.gui.isOpened()) return;
    let type = frac == 'POLICE' ? 0 : frac == 'FIB' ? 1 : 2;
    global.gui.setData("personalDigitalAssistant/setType", JSON.stringify(type));
    global.gui.setData("personalDigitalAssistant/setHumanListWanted", wantedPlayer);
    global.gui.setData("personalDigitalAssistant/setCarListWanted", wantedVehicles);
    let helps = JSON.parse(helpList);
    let pos = mp.players.local.position;
    helps.forEach(item => {
        item.distance = Math.round(mp.game.gameplay.getDistanceBetweenCoords(item.position.x, item.position.y, item.position.z, pos.x, pos.y, pos.z, true));
    });
    global.gui.setData("personalDigitalAssistant/setHelpList", JSON.stringify(helps));
    //global.gui.setData("personalDigitalAssistant/dataBase/setPrisoners", JSON.stringify([]));
    global.gui.setData("personalDigitalAssistant/setSearchHuman", "{}");
    global.gui.setData("personalDigitalAssistant/setSearchCar", "{}");
    opened = global.gui.openPage('PersonalDigitalAssistant');
});


mp.events.add('pda:loadArrests', (jsonPrisoners) => {
    var prisoners = JSON.parse(jsonPrisoners);
    prisoners.forEach(prison => {
        global.gui.setData("personalDigitalAssistant/dataBase/updatePrisoners", JSON.stringify(prison));
    });
});


mp.events.add('pda:open', () => {
    if (global.chatActive || global.gui.isOpened()) return;
    global.gui.setData("personalDigitalAssistant/setSearchHuman", "{}");
    global.gui.setData("personalDigitalAssistant/setSearchCar", "{}");
    opened = global.gui.openPage('PersonalDigitalAssistant');
});

mp.events.add('pda:setWanted', (type, id, wantedlvl, reason) => {
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    if (type == 'car')
        mp.events.callRemote('pda:setVehicleWantedLvl', id, wantedlvl, reason);
    else if (type == 'player')
        mp.events.callRemote('pda:setPlayerWantedLvl', id, wantedlvl, reason);
});

mp.events.add('pda:setSearchHuman', (text, type) => {
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    mp.events.callRemote('pda:searchPlayer', text, type);
});

mp.events.add('pda:findCar', (text) => {
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    mp.events.callRemote('pda:searchVehicle', text);
});

mp.events.add('pda:toHelp', (idHelp) => {
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    mp.events.callRemote('pda:acceptCall', idHelp);
});

mp.events.add('pda:pushCode', (code) => {
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    mp.events.callRemote('pda:callNeedHelp', code);
});


mp.events.add('pda:releaseOnBail', (id, amount) => {
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    mp.events.callRemote('pda:releaseFromKPZ', id, amount)
});

mp.events.add('pda:overrideBail', (id) => {
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    mp.events.callRemote('pda:blockCanBeIssue', id)
});


mp.events.add('pda:updwantedpl', (wantedPlayer) => {
    global.gui.setData("personalDigitalAssistant/updatePlayerWantedLVL", wantedPlayer);
});

mp.events.add('pda:updwantedveh', (wantedVehicle) => {
    global.gui.setData("personalDigitalAssistant/updateVehicleWantedLVL", wantedVehicle);
});

mp.events.add('pda:updhelpers', (id, helpers) => {
    global.gui.setData("personalDigitalAssistant/updateHelpers", JSON.stringify({id: id, helpers: JSON.parse(helpers)}));    
});

mp.events.add('pda:returnFindPlayer', (findPlayer) => {
    global.gui.setData("personalDigitalAssistant/setSearchHuman", findPlayer);
});

mp.events.add('pda:returnFindVehicle', (findVehicle) => {
    global.gui.setData("personalDigitalAssistant/setSearchCar", findVehicle);
});

mp.events.add('pda:callPolice', (callJson) => {
    let call = JSON.parse(callJson);
    let pos = mp.players.local.position;
    call.distance = Math.round(mp.game.gameplay.getDistanceBetweenCoords(call.position.x, call.position.y, call.position.z, pos.x, pos.y, pos.z, true));
    global.gui.setData("personalDigitalAssistant/addIntoHelpList", JSON.stringify(call));
});

mp.events.add('pda:delcallPolice', (callId) => {
    global.gui.setData("personalDigitalAssistant/removeFromHelpList", JSON.stringify(callId));
});

mp.events.add('pda:updatePrisoner', (prison) => {
    global.gui.setData("personalDigitalAssistant/dataBase/updatePrisoners", prison);
});


