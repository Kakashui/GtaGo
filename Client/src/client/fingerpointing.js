//Fingerpointing
let checkTime = 0; 
let active = false;
const gameplayCam = mp.cameras.new("gameplay");

function getRelativePitch() {
    let camRot = gameplayCam.getRot(2);
    return camRot.x - mp.players.local.getPitch();
}

function stopFinger() {
    if (active) {
        active = false;
        mp.game.invoke(global.getNative("REQUEST_TASK_MOVE_NETWORK_STATE_TRANSITION"), mp.players.local.handle, "Stop");
        mp.game.streaming.removeAnimDict("anim@mp_point");
        
        if (!mp.players.local.isInjured()) {
            mp.players.local.clearSecondaryTask();
        }
        if (!mp.players.local.isInAnyVehicle(true)) {
            mp.game.invoke(global.getNative("SET_PED_CURRENT_WEAPON_VISIBLE"), mp.players.local.handle, 1, 1, 1, 1);
        }
        mp.players.local.setConfigFlag(36, false);
        mp.players.local.clearSecondaryTask();
        mp.players.local.clearTasksImmediately();
    }
}

async function startFinger() {
    if (!active) {
        active = true;
        if(!mp.game.streaming.hasAnimDictLoaded("anim@mp_point")){                    
            mp.game.streaming.requestAnimDict("anim@mp_point");
            while (!mp.game.streaming.hasAnimDictLoaded("anim@mp_point")) {
                await mp.game.waitAsync(0);
            }
        }
        mp.game.invoke(global.getNative("SET_PED_CURRENT_WEAPON_VISIBLE"), mp.players.local.handle, 0, 1, 1, 1);
        mp.players.local.setConfigFlag(36, true)
        mp.players.local.taskMoveNetwork("task_mp_pointing", 0.5, false, "anim@mp_point", 24);
    }
}

function getPlayerByRemoteId(remoteId) {
    let pla = mp.players.atRemoteId(remoteId);
    if (pla == undefined || pla == null) {
        return null;
    }
    return pla;
}

mp.events.add("render", ()=>{
    if (active){
        let camPitch = getRelativePitch();
        if (camPitch < -70.0) {
            camPitch = -70.0;
        }
        else if (camPitch > 42.0) {
            camPitch = 42.0;
        }                
        camPitch = (camPitch + 70.0) / 112.0;
        let camHeading = mp.game.cam.getGameplayCamRelativeHeading();
        let cosCamHeading = mp.game.system.cos(camHeading);
        let sinCamHeading = mp.game.system.sin(camHeading);

        if (camHeading < -180.0)
            camHeading = -180.0;
        else if (camHeading > 180.0)
            camHeading = 180.0;

        camHeading = (camHeading + 180.0) / 360.0;
        let coords = mp.players.local.getOffsetFromGivenWorldCoords((cosCamHeading * -0.2) - (sinCamHeading * (0.4 * camHeading + 0.3)), (sinCamHeading * -0.2) + (cosCamHeading * (0.4 * camHeading + 0.3)), 0.6);
        let blocked = (typeof mp.raycasting.testPointToPoint([coords.x, coords.y, coords.z - 0.2], [coords.x, coords.y, coords.z + 0.2], mp.players.local.handle, 7) !== 'undefined');
        const viewMode = mp.game.invoke(global.getNative("GET_FOLLOW_PED_CAM_VIEW_MODE"));
        mp.game.invokeFloat(global.getNative("SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT"), mp.players.local.handle, "Pitch", camPitch)
        mp.game.invokeFloat(global.getNative("SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT"), mp.players.local.handle, "Heading", camHeading * -1.0 + 1.0)
        mp.game.invoke(global.getNative("SET_TASK_MOVE_NETWORK_SIGNAL_BOOL"), mp.players.local.handle, "isBlocked", blocked)
        mp.game.invoke(global.getNative("SET_TASK_MOVE_NETWORK_SIGNAL_BOOL"), mp.players.local.handle, "isFirstPerson", viewMode == 4)

        if (Date.now() >  checkTime) {
            checkTime = Date.now() + 200;
            mp.events.callRemote("fpsync.update", camPitch, camHeading);
        }
    }
})

mp.events.add("fpsync.update", (id, camPitch, camHeading) => {
    let netPlayer = getPlayerByRemoteId(parseInt(id));
    if (netPlayer != null) {
        if (netPlayer !== mp.players.local) {
            netPlayer.lastReceivedPointing = Date.now() + 1000;

            if (!netPlayer.pointingInterval) {
                netPlayer.pointingInterval = setInterval(() => {
                    if (Date.now() > netPlayer.lastReceivedPointing) {
                        clearInterval(netPlayer.pointingInterval);

                        delete netPlayer.lastReceivedPointing;
                        delete netPlayer.pointingInterval;
                        mp.game.invoke(global.getNative("REQUEST_TASK_MOVE_NETWORK_STATE_TRANSITION"), netPlayer.handle, "Stop");

                        if (!netPlayer.isInAnyVehicle(true)) {
                            mp.game.invoke(global.getNative("SET_PED_CURRENT_WEAPON_VISIBLE"), netPlayer.handle, 1, 1, 1, 1);
                        }
                        netPlayer.setConfigFlag(36, false);
                        mp.game.streaming.removeAnimDict("anim@mp_point");
                    }
                }, 500);
                if(!mp.game.streaming.hasAnimDictLoaded("anim@mp_point")){
                    mp.game.streaming.requestAnimDict("anim@mp_point");
                    while (!mp.game.streaming.hasAnimDictLoaded("anim@mp_point")) {
                        mp.game.wait(0);
                    }
                }
                mp.game.invoke(global.getNative("SET_PED_CURRENT_WEAPON_VISIBLE"), netPlayer.handle, 0, 1, 1, 1);
                netPlayer.setConfigFlag(36, true)
                netPlayer.taskMoveNetwork("task_mp_pointing", 0.5, false, "anim@mp_point", 24);                
            }

            mp.game.invoke(global.getNative("SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT"), netPlayer.handle, "Pitch", camPitch)
            mp.game.invoke(global.getNative("SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT"), netPlayer.handle, "Heading", camHeading * -1.0 + 1.0)
            mp.game.invoke(global.getNative("SET_TASK_MOVE_NETWORK_SIGNAL_BOOL"), netPlayer.handle, "isBlocked", 0);
            mp.game.invoke(global.getNative("SET_TASK_MOVE_NETWORK_SIGNAL_BOOL"), netPlayer.handle, "isFirstPerson", 0);
        }
    }
});

mp.keys.bind(global.Keys.Key_N, true, () => {
    if ( !mp.gui.cursor.visible) {
        startFinger();
    }
});

mp.keys.bind(global.Keys.Key_N, false, () => {
    stopFinger();
});