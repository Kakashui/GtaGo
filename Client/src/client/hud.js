let cruiseSpeed = -1;
let cruiseLastPressed = 0;
let showHint = true;
mp.game.ui.setHudColour(18, 182, 211, 0, 0);// green color

let hudstatus =
{
    safezone: null, // Last safezone size
    online: 0, // Last online int
    id: 0, // Player id// uuid
    lastCheck: 0,
    updateInterval: 250,
    street: null,
    area: null,

    invehicle: false,
    updatespeedTimeout: 0, // Timeout for optimization speedometer
    engine: false,
    doors: true,
    fuel: 0,
    health: 0
}

function getMinimapAnchor() {
    let sfX = 1.0 / 20.0;
    let sfY = 1.0 / 20.0;
    let safeZone = mp.game.graphics.getSafeZoneSize();
    let aspectRatio = mp.game.graphics.getScreenAspectRatio(false);
    let resolution = mp.game.graphics.getScreenActiveResolution(0, 0);
    let scaleX = 1.0 / resolution.x;
    let scaleY = 1.0 / resolution.y;

    let minimap = {
        width: scaleX * (resolution.x / (4 * aspectRatio)),
        height: scaleY * (resolution.y / 5.674),
        scaleX: scaleX,
        scaleY: scaleY,
        leftX: scaleX * (resolution.x * (sfX * (Math.abs(safeZone - 1.0) * 10))),
        bottomY: 1.0 - scaleY * (resolution.y * (sfY * (Math.abs(safeZone - 1.0) * 10))),
    };

    minimap.rightX = minimap.leftX + minimap.width;
    minimap.topY = minimap.bottomY - minimap.height;
    return minimap;
}

mp.events.add('gui:ready', () => {
    global.gui.setData('hud/setMinimapSize', JSON.stringify(getMinimapAnchor()));
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, () => {
    if (mp.game.ui.isPauseMenuActive()) {
        global.gui.setData('hud/setMinimapSize', JSON.stringify(getMinimapAnchor()));
        global.showHud(false)
        global.showHud(true)
    }
})

// HUD events
mp.events.add('notify', (type, layout, msg, time) => {
    let data = {
        type: type,
        position: layout,
        message: msg,
        time: time
    }
    global.gui.setData('notifyList/notify', JSON.stringify(data));
});

let currentLvl;
let state = '';
let expAfterFill = 0;
mp.events.add('exp:upd', (exp, lvl) => {
    if (global.frontendSoundsEnabled)
        mp.game.audio.playSoundFrontend(-1 , 'BASE_JUMP_PASSED', 'HUD_AWARDS', false)
    if (currentLvl === lvl) {
        state = 'charge';
        global.gui.setData('hud/setNextExp', exp);
        global.gui.setData('hud/toggleLevelUpShow', true);
    }
    else {
        state = 'fill';
        global.gui.setData('hud/setNextExp', 3 + currentLvl * 3);
        global.gui.setData('hud/toggleLevelUpShow', true);
        expAfterFill = exp;
        currentLvl = lvl;
    }
});
mp.events.add('exp:timerStoped', () => {
    switch (state) {
        case 'charge':
            setTimeout(() => {
                global.gui.setData('hud/toggleLevelUpShow', false);
            }, 5000);
            state = '';
            return;
        case 'fill':
            if (expAfterFill !== 0){
                global.gui.setData('hud/toggleLevelUpShow', false);
                state = 'charge';
                global.gui.setData('hud/setLevel', currentLvl);
                global.gui.setData('hud/setNextExp', expAfterFill);
                
                global.gui.setData('hud/toggleLevelUpShow', true);
                expAfterFill = 0;
            }
            setTimeout(() => {
                global.gui.setData('hud/toggleLevelUpShow', false);
                global.gui.setData('hud/setLevel', currentLvl);
            }, 5000);
            return;
        case '':
            setTimeout(() => {
                global.gui.setData('hud/toggleLevelUpShow', false);
            }, 5000);
    }
});
mp.events.add('exp:init', (exp, lvl) => {
    currentLvl = lvl;
    global.gui.setData('hud/setLevelUp', JSON.stringify({ lvl, exp }));
});


mp.events.add('alert', (message) => {
    global.gui.setData('hud/setNotificationShow', JSON.stringify(message));
});

mp.events.add('ready', function () {
    //hudstatus.id = global.localplayer.getVariable('REMOTE_ID');
    hudstatus.id = global.UUID;//пробуй так будет работать
    global.gui.setData('hud/updateData', JSON.stringify({name: 'id', value: hudstatus.id}));
});

let introMoneyGiven = false;
mp.events.add('UpdateMoney', function (value) {
    global.gui.setData('hud/updateData', JSON.stringify({name: 'money', value}));
    if (global.frontendSoundsEnabled && introMoneyGiven)
        mp.game.audio.playSoundFrontend(-1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS", false);  
    if (!introMoneyGiven) introMoneyGiven = true;
});
mp.events.add('UpdateBank', function (value) {
    global.gui.setData('hud/updateData', JSON.stringify({name: 'bank', value}));
});
mp.events.add('setWanted', function (lvl) {
    global.gui.setData("hud/setWantedLevels", lvl)
});
mp.events.add("hud:arrest:timer:update", (time, reason)=>{
    global.gui.setData("hud/setArrestTimer", JSON.stringify({time, reason}))
});
mp.events.add("hud:arrest:timer:reset", ()=>{
    global.gui.setData("hud/resetArrestTimer")
});
function UpdateOnlineHud() {
    hudstatus.online = mp.players.length + A_ONL;
    global.gui.setData('hud/updateData', JSON.stringify({name: 'online', value: hudstatus.online}));
}
 mp.keys.bind(global.Keys.Key_F5, true, function () { // F5 key
     global.gui.setData('hud/showHelp', 'true');
 });


mp.events.add('setCruiseSpeed', function (speed) {
    speed = parseInt(speed);
    if (speed === NaN || speed < 1) return;
    if (!mp.players.local.isInAnyVehicle(true) || !mp.players.local.vehicle || mp.players.local.vehicle.getPedInSeat(-1) != mp.players.local.handle) return;
	let vclass = mp.players.local.vehicle.getClass();
	if(vclass == 14 || vclass == 15 || vclass == 16) return;
	if (mp.players.local.vehicle.isOnAllWheels() == false) return;
	let veh = mp.players.local.vehicle;
	let curSpeed = veh.getSpeed();
	if(speed < curSpeed) {
		mp.events.call('notify', 4, 9, "client_20", 6000);
		return;
	}
    speed = speed / 3.6; // convert from kph to mps
    let maxSpeed = mp.game.vehicle.getVehicleModelMaxSpeed(veh.model);
    if (speed > maxSpeed) speed = maxSpeed;
    veh.setMaxSpeed(speed);
    //mp.gui.execute(`HUD.cruiseColor='#eebe00'`);
    cruiseSpeed = speed;
});

mp.events.add('newPassport', function (player, pass) {
    if (player && mp.players.exists(player))
        global.passports[player.name] = pass;
        mp.storage.data.passports = global.passports;
        //mp.storage.flush()
});

let showAltTabHint = false;
mp.events.add('showAltTabHint', function () {
    showAltTabHint = true;
    setTimeout(function () { showAltTabHint = false; }, 10000);
});

let A_ONL = 0;
mp.events.add('A_ONL', newCheatedOnline => {
    A_ONL = newCheatedOnline;
    UpdateOnlineHud();
});


mp.events.add('render', () => {
    // double-cursor fix
    if (!global.loggedin) return;

    // Disable HUD components.    
    mp.game.ui.hideHudComponentThisFrame(2); // HUD_WEAPON_ICON
    mp.game.ui.hideHudComponentThisFrame(3); // HUD_CASH
    mp.game.ui.hideHudComponentThisFrame(6); // HUD_VEHICLE_NAME
    mp.game.ui.hideHudComponentThisFrame(7); // HUD_AREA_NAME
    mp.game.ui.hideHudComponentThisFrame(8); // HUD_VEHICLE_CLASS
    mp.game.ui.hideHudComponentThisFrame(9); // HUD_STREET_NAME
    mp.game.ui.hideHudComponentThisFrame(19); // HUD_WEAPON_WHEEL
    mp.game.ui.hideHudComponentThisFrame(20); // HUD_WEAPON_WHEEL_STATS
    mp.game.ui.hideHudComponentThisFrame(22); // MAX_HUD_WEAPONS
    
    if (hudstatus.online != mp.players.length) {
        UpdateOnlineHud();
    }

    if (mp.players.local.isInAnyVehicle(false) && mp.players.local.vehicle != null) {
        if (cruiseSpeed != -1) // kostyl'
                veh.setMaxSpeed(cruiseSpeed);
                
		if (mp.players.local.vehicle.getPedInSeat(-1) == mp.players.local.handle && hudstatus.lastCheck < Date.now()) {
            hudstatus.lastCheck = Date.now() + hudstatus.updateInterval;
            if (!hudstatus.invehicle) 
                global.gui.setData('speedometer/setInVeh', true);
			hudstatus.invehicle = true;

			let veh = mp.players.local.vehicle;
            

			if (veh.getVariable('PETROL') !== undefined && veh.getVariable('MAXPETROL') !== undefined) {
				let petrol = veh.getVariable('PETROL');
				let maxpetrol = veh.getVariable('MAXPETROL');
                if (petrol > maxpetrol)
                    petrol = maxpetrol;
				if (hudstatus.fuel != petrol && petrol >= 0) {
					global.gui.setData('speedometer/setMaxFuel', maxpetrol);
					global.gui.setData('speedometer/setCurFuel', petrol);
					hudstatus.fuel = petrol;
				}
			}

			let engine = veh.getIsEngineRunning();
			if (engine != null && engine !== hudstatus.engine) {
				global.gui.setData('speedometer/setEngine', engine == true);
				hudstatus.engine = engine;
			}

            let locked = veh.getDoorLockStatus() == 1 ? false : true;

            if (hudstatus.doors !== locked) {
                global.gui.setData('speedometer/setDoors', locked == true);
                hudstatus.doors = locked;
            }
            
            let speed = (veh.getSpeed() * 3.6).toFixed();
            global.gui.setData('speedometer/setCurSpeed', speed);
			hudstatus.updatespeedTimeout = Date.now();
		}
    } 
    else 
    {
        if (hudstatus.invehicle) {
            hudstatus.invehicle = false;
            global.gui.setData('speedometer/setInVeh', false);
            mp.players.local.setConfigFlag(32, false);    
            global.gui.setData('speedometer/setBelt', false);
        }
    }
});