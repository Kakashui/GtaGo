//#region Server

global.isCarThiefWorker = false;
global.isCarUnlocked = false;
global.isCarStarted = false;
global.agent = null;
global.mapRadius = null;

// user entered to colshape and pressed E
mp.events.add('WORK::CARTHIEF::DIAGNOSTIC::SERVER', () => {
    global.gui.openPage("WorkMiniGame");
})

mp.events.add('WORK::CARTHIEF::DIAGNOSTIC::RESULT::VUE', (isSuccess) => {
    mp.events.callRemote("WORK::CARTHIEF::GAME::RESULT::CLIENT", isSuccess);
    global.gui.close();
})

mp.events.add('WORK::CARTHIEF::PED::CRETATE', (hash, x, y, z, angle, dimension) => {
    agent = mp.peds.newValid(hash, new mp.Vector3(x, y, z), angle, dimension);
})

mp.events.add('WORK::CARTHIEF::PED::DELETE', () => {
    if (agent !== null) agent.destroy();
})

mp.events.add("WORK::CARTHIEF::CREATE::RADIUS", (position, radius, color) => {
    // let position = JSON.parse(pos)
    
    mapRadius = mp.game.ui.addBlipForRadius(position.x, position.y, position.z, radius);
	mp.game.invoke('0x45FF974EEE1C8734', mapRadius, 175);
	mp.game.invoke('0x03D7FB09E75D6B7E', mapRadius, color);
})

mp.events.add("WORK::CARTHIEF::DELETE::RADIUS", () => {
    if (mapRadius == null) return;
    mp.game.invoke('0x45FF974EEE1C8734', mapRadius, 0);
    mp.game.invoke('0xFF0B610F6BE0D7AF');
})

// open veh
mp.keys.bind(global.Keys.Key_O, false, function () {
    if (!isCarThiefWorker || isCarUnlocked) return;
    mp.events.callRemote("WORK::CARTHIEF::GAME::START::CLIENT", 'O');
})

// start veh
mp.keys.bind(global.Keys.Key_I, false, function () {
    // Police try to start car?
    if (!mp.players.local.isInAnyVehicle(false)) return;

    let fraction = mp.players.local.getVariable('fraction');
    if (fraction == 7) {
        mp.events.callRemote("WORK::CARTHIEF::POLICEMAN::START::ENGINE");
        // Could break a car!
        return;
    }

    if (!isCarThiefWorker || !isCarUnlocked) return;
    mp.events.callRemote("WORK::CARTHIEF::GAME::START::CLIENT", 'I');
})


mp.events.add('WORK::CARTHIEF::CHANGE::STATE::SERVER', (isPlaying) => {
    global.isCarThiefWorker = isPlaying;
})

mp.events.add('WORK::CARTHIEF::CHANGE::LOCK::SERVER', (isLocked) => {
    global.isCarUnlocked = isLocked;
})

mp.events.add('CARTHIEF::TIME::SET::SERVER', (timeLeftSeconds) => {
    global.gui.setData('hud/startWorkTimer', timeLeftSeconds);
    //global.gui.setData('hud/setIsKillStat', true); 
    //global.gui.setData('hud/startKillstatTimer', timeLeftSeconds);
})

//#endregion

mp.events.add('GOTOMARK::SEND', () => {
    try {
        let blipIterator = mp.game.invoke('0x186E5D252FA50E7D');
        let totalBlipsFound = mp.game.invoke('0x9A3FF3DE163034E8');
        let FirstInfoId = mp.game.invoke('0x1BEDE233E6CD2A1F', blipIterator);
        let NextInfoId = mp.game.invoke('0x14F96AA50D6FBEA7', blipIterator);
        let coord = {x:0,y:0,z:0};
        for (let i = FirstInfoId, blipCount = 0; blipCount != totalBlipsFound; blipCount++, i = NextInfoId) {
            if (mp.game.invoke('0x1FC877464A04FC4F', i) == 8) {
                coord = mp.game.ui.getBlipInfoIdCoord(i);
                break;
            }
        }

        let vehicle = mp.players.local.vehicle;

        const position = vehicle.position;
        position.x = coord.x;
        position.y = coord.y;
        position.z = 150;
        vehicle.setCoordsNoOffset(position.x, position.y, position.z, false, false, false);
        vehicle.freezePosition(true);

        setTimeout(() => {
            vehicle.freezePosition(false);
            let position2 = mp.players.local.position;
            position2.z = mp.game.gameplay.getGroundZFor3dCoord(position2.x, position2.y, position2.z, 0.0, false) + 0.5;
            vehicle.setCoordsNoOffset(position2.x, position2.y, position2.z, false, false, false);
            vehicle.setRotation(0, 0, vehicle.getRotation(2).z, 2, true);
            vehicle.setOnGroundProperly();
            //mp.events.callRemote('GOTOMARK::GET', mp.players.local.vehicle.position.x, mp.players.local.vehicle.position.y, mp.players.local.vehicle.position.z, false);
        }, 1000);

        //mp.events.callRemote('GOTOMARK::GET', mp.players.local.vehicle.position.x, mp.players.local.vehicle.position.y, mp.players.local.vehicle.position.z, true);
       
	}catch(e){
        if(global.sendException)mp.serverLog(`Error in carThiefWork.GOTOMARK::SEND: ${e.name }\n${e.message}\n${e.stack}`);
    }
});