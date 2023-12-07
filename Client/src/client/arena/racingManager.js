let currentCheckPoint;
let currentBlip;
let currentRaceStartTime;
let isRacingNow

mp.events.add({
    'race:start': (vehicleRemoteId) => {
        currentRaceStartTime = Date.now();
        //if (!mp.players.local.isInAnyVehicle(false)) return;
        
        for (let i = 0; i <= 4000; i++){
            if (mp.vehicles.atRemoteId(vehicleRemoteId).doesExist()) break;
        }
        let veh = mp.vehicles.atRemoteId(vehicleRemoteId);
        mp.players.local.setIntoVehicle(veh.handle, -1)
        
        mp.game.audio.playSoundFrontend(-1, "5s_To_Event_Start_Countdown", "UNIQ_FM_Events_Soundset", false);
        for (let i = 5; i >= 1; i--){
            mp.gui.notify(mp.gui.notifyType.INFO, `${i}`, 2000);
        }
        mp.gui.notify(mp.gui.notifyType.INFO, 'GO!', 2000);

        if (mp.players.local.vehicle)
            mp.players.local.vehicle.freezePosition(true);
        
        isRacingNow = true;
        
        setTimeout(() => {
            if (!mp.players.local.vehicle) return;
            mp.players.local.vehicle.freezePosition(false);
        }, 5000)
    },
    'render': () => {
        if (!isRacingNow) return;
        mp.game.controls.disableControlAction(2, 75, true);
    },
    'race:setCP': (point, nextPoint) => {
        const p = JSON.parse(point);
        createCheckPoint(6, new mp.Vector3(p.x, p.y, p.z + 1), JSON.parse(nextPoint), false)
        createBlip(127, JSON.parse(point), 1);
    },
    'race:setFinish': (point) => {
        createCheckPoint(4, JSON.parse(point), new mp.Vector3(0, 0, 0), true);
        createBlip(309, JSON.parse(point), 2);
    },
    'race:clear': clear,
    'race:openMenu': openMenu,
    'playerEnterCheckpoint': (checkpoint) => {
        //if (player !== mp.players.local) return;
        if (!checkpoint.hasOwnProperty('isRacing')) return;
        if (!mp.players.local.isInAnyVehicle(false)) return;
        
        if (checkpoint.isRacingFinish === true) {
            mp.events.callRemote('race:enteredFinish', ((Date.now() - currentRaceStartTime) / 1000))
            mp.game.audio.playSoundFrontend(-1, "Enter_Area", "DLC_Lowrider_Relay_Race_Sounds", false)
        }
        else {
            mp.events.callRemote('race:enteredCP')
            mp.game.audio.playSoundFrontend(-1, "Enter_Area", "DLC_Lowrider_Relay_Race_Sounds", false)
        }
    }
});

mp.keys.bind(global.Keys.Key_F, false, function () {
    if (!isRacingNow || !mp.players.local.isInAnyVehicle(false)) return;
    mp.events.callRemote('race:openleaveDialog')
});

function createCheckPoint(type, position, nextPos, isFinish) {
    if (currentCheckPoint) currentCheckPoint.destroy();
    currentCheckPoint = mp.checkpoints.new(type, position, 8, {
        direction: nextPos,
        dimension: mp.players.local.dimension
    });
    currentCheckPoint.isRacingFinish = isFinish;
    currentCheckPoint.isRacing = true;
}

function createBlip(type, position, color) {
    if (currentBlip) currentBlip.destroy();
    currentBlip = mp.blips.new(type, position, {
        color: color,
        shortRange: false,
        dimension: mp.players.local.dimension
    });
    // setBlipRoute
    mp.game.invoke('0x4F7D8A9BFB0B43E9', currentBlip.handle, true);
}

function clear() {
    if (currentBlip) currentBlip.destroy();
    if (currentCheckPoint) currentCheckPoint.destroy();
    currentBlip = null;
    currentCheckPoint = null;
    isRacingNow = false;
    mp.events.call('arena:tracking:stop')
}

let menuOpened = false;
function openMenu() {
    menuOpened = true;
    global.gui.openPage('Events')
}
mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (menuOpened) {
        global.gui.close();
        menuOpened = false;
    }
});