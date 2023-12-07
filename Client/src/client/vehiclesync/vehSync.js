

let nextUpdateDoorBrokeTime = Date.now();
let _intervalUpdateDoorBroke = 500;

let nextUpdateDirtTime = Date.now();
let _intervalUpdateDirt = 20000;

let currDirtLvl = 0;
let coefDirtLvl = 0.3;

let _healthWhenEngineBreak = 50;
let oldEngineHealth = 1000;

let _distance = 0;
let _oldPosition = new mp.Vector3(0, 0, 0);

let driverSeat = 0;

function CheckExistVehicle(vehicle) {
    return (vehicle && vehicle.type === 'vehicle' && vehicle.handle !== 0);
}

mp.events.add("viewVariableData", (vehicle, variable) => {
    try {
        if (vehicle) {
            mp.gui.chat.push(`${JSON.stringify(global.getVariable(vehicle, variable, "None"))}`);
            mp.serverLog(JSON.stringify(global.getVariable(vehicle, variable, "None")));
        }

    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("veh:offRadio", () => {
    if (mp.players.local.vehicle) {
        mp.events.callRemote('VehStream_RadioChange', mp.players.local.vehicle, 255);
        mp.game.audio.setRadioToStationName("OFF");
    }
});

// F - seats in driver, G - seats in passanger
mp.events.add('render', () => {

    mp.game.controls.disableControlAction(0, 58, true);

    if (global.getVariable(mp.players.local, 'InDeath', false) || global.cuffed)
        mp.game.controls.disableControlAction(0, 23, true);
});

mp.keys.bind(global.Keys.Key_G, false, () => {
    try {
        if (global.checkIsAnyActivity()) return;
        if (global.getVariable(mp.players.local, 'InDeath', false)) return;
        if (!mp.players.local.vehicle) {
            let vehicle = getClosestVehicleInRange(5);
            global.seatVehicleOnClearPlace(vehicle, mp.players.local, 5000, 5)
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.seatVehicleOnClearPlace: ${e.name}\n${e.message}\n${e.stack}`);
        return false;
    }
});

global.seatVehicleOnClearPlace = (vehicle, ped, time, distance) => {
    try {
        if (vehicle === null)
            return false;
        if (!vehicle || !vehicle.isAnySeatEmpty() || vehicle.getSpeed() > 5)
            return false;
        if (calcDist(vehicle.position, ped.getCoords(true)) > distance)
            return false;
        let maxSeat = mp.game.vehicle.getVehicleModelMaxNumberOfPassengers(vehicle.model);
        for (let seatIndex = 0; seatIndex < maxSeat - 1; seatIndex++) {
            if (vehicle.isSeatFree(seatIndex) && vehicle.getDoorLockStatus() == 1) {
                if (seatIndex > 2) {
                    ped.taskOpenVehicleDoor(vehicle.handle, time, 1, 2);
                    setTimeout(() => {
                        ped.setIntoVehicle(vehicle.handle, seatIndex);
                    }, time);
                } else
                    ped.taskEnterVehicle(vehicle.handle, time, seatIndex, 2, 1, 0);
                return true;
            }
        }
        return false;

    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.seatVehicleOnClearPlace: ${e.name}\n${e.message}\n${e.stack}`);
        return false;
    }
}

function getClosestVehicleInRange(range) {
    const vehiclesInRange = [];
    mp.vehicles.forEachInStreamRange((vehicle) => {
        const distSquared = calcDist(vehicle.position, mp.players.local.position);
        if (distSquared <= range) {
            vehiclesInRange.push({ vehicle, distSquared });
        }
    });

    vehiclesInRange.sort((a, b) => a.distSquared - b.distSquared);

    return vehiclesInRange[0] ? vehiclesInRange[0].vehicle : null;
}

global.calcDist = function (v1, v2) {
    return mp.game.system.vdist(
        v1.x,
        v1.y,
        v1.z,
        v2.x,
        v2.y,
        v2.z
    );
}


mp.events.add('render', () => {
    if (!mp.players.local.isInAnyVehicle(true) || !mp.players.local.vehicle)
        return;
    let vehicle = mp.players.local.vehicle;
    if (vehicle.getPedInSeat(-1) !== mp.players.local.handle)
        return;

    let torque = global.getVariable(vehicle, 'ENGINETORQUE', 1);
    let transmissCoef = global.getVariable(vehicle, 'veh:coefTransm', 1);
    vehicle.setEngineTorqueMultiplier(torque * transmissCoef);

    //Костыль, чтоб делать проверти только у личных авто, -100 - лишние проверки не делаем
    let dontCheck = global.getVariable(vehicle, 'veh:doorBreak', -100);


    if (Date.now() > nextUpdateDirtTime) {
        nextUpdateDirtTime = Date.now() + _intervalUpdateDirt;

        //Обновление грязи
        let vehClear = global.getVariable(vehicle, 'veh:vehDirtClear', 0) * 1000;
        if (Date.now() > vehClear) {
            let dirt = vehicle.getDirtLevel();
            let newDirt = currDirtLvl + (dirt - currDirtLvl) * coefDirtLvl;
            currDirtLvl = newDirt > 15 ? 15 : newDirt;
        }
        mp.events.callRemote('veh:setDirtLevel', currDirtLvl);

        //отправляем пройденное расстояние
        if (_distance > 1) {
            if (dontCheck != -100)
                mp.events.callRemote('veh:addDistance', vehicle, _distance);
            mp.events.call('personalEvents:invokeEvents', "Driving", _distance);
            _distance = 0;
        }
    }

    //Ломаем тормоза
    if (dontCheck != -100 && global.getVariable(vehicle, 'veh:BrakesBroke', false)) {
        if (vehicle.getSpeed() > 2)
            if (vehicle.getSpeedVector(true).y > 0)
                mp.game.controls.disableControlAction(27, 72, true);
            else if (vehicle.getSpeedVector(true).y < 0)
                mp.game.controls.disableControlAction(27, 71, true);
        mp.game.controls.disableControlAction(27, 76, true);
    }

    //Проверки на повреждения
    if (Date.now() > nextUpdateDoorBrokeTime) {
        nextUpdateDoorBrokeTime = Date.now() + _intervalUpdateDoorBroke;

        if (dontCheck != -100) {
            //ломаем двери
            let door = global.getVariable(vehicle, 'veh:doorBreak', 0);
            for (let i = 0; i < 8; i++) {
                if ((door >> i) % 2 == 0 && vehicle.isDoorDamaged(i))
                    mp.events.callRemote('veh:doorBroken', i);
            }

            //Ломаем движок
            if (oldEngineHealth > _healthWhenEngineBreak && vehicle.getEngineHealth() <= _healthWhenEngineBreak)
                mp.events.callRemote('veh:engBroken');
            oldEngineHealth = vehicle.getEngineHealth();
        }

        //Пройденное расстояние
        let diffDist = mp.game.gameplay.getDistanceBetweenCoords(vehicle.position.x, vehicle.position.y, vehicle.position.z, _oldPosition.x, _oldPosition.y, _oldPosition.z, true);
        if (diffDist < 100)
            _distance += diffDist;
        _oldPosition = vehicle.position;
    }
});


mp.events.add("entityControllerChange", (vehicle, newController) => {
    if (newController !== mp.players.local || vehicle.type !== "vehicle") return;
    vehicle.freezePosition(true);
    setTimeout(() => {
        if (vehicle && vehicle.handle != 0)
            vehicle.freezePosition(false);
    }, 1000);
});

//Sync data on stream in
mp.events.add("entityStreamIn", (vehicle) => {
    try {
        if (vehicle.type !== "vehicle") return;
        if (!vehicle)
            return;
        vehicle.freezePosition(true);
        //Needed to stop vehicles from freaking out
        mp.game.streaming.requestCollisionAtCoord(vehicle.position.x, vehicle.position.y, vehicle.position.z);
        vehicle.setLoadCollisionFlag(true);

        //Set doors unbreakable for a moment
        let x = 0;
        for (x = 0; x < 8; x++) {
            vehicle.setDoorBreakable(x, false);
        }

        let enginePower = global.getVariable(vehicle, 'ENGINEPOWER', 0);
        let engineHealth = global.getVariable(vehicle, 'veh:engineHealth', 1000);
        SetVehicleEnginePower(vehicle, enginePower, engineHealth);

        let engState = global.getVariable(vehicle, 'veh:engineStatus', false);
        vehicle.setEngineOn(engState, engState, !engState);
        vehicle.setUndriveable(true);

        let doorStatus = global.getVariable(vehicle, 'veh:doorStatus', 0);
        for (let index = 0; index < 8; index++) {
            let st = (doorStatus >> index) % 2;
            if (st == 1)
                vehicle.setDoorOpen(index, false, false);
            else
                vehicle.setDoorShut(index, false);
        }

        let doorBreak = global.getVariable(vehicle, 'veh:doorBreak', 0);
        for (let index = 0; index < 8; index++) {
            let st = (doorBreak >> index) % 2;
            if (st == 1)
                vehicle.setDoorBroken(index, true);
        }

        let dirtLevel = global.getVariable(vehicle, 'veh:dirtLevel', 0);
        vehicle.setDirtLevel(dirtLevel);

        let turnSignal = global.getVariable(vehicle, 'veh:turnSignal', 0);
        if (turnSignal % 2 == 1)
            vehicle.setIndicatorLights(1, true);
        else
            vehicle.setIndicatorLights(1, false);
        if (turnSignal > 1)
            vehicle.setIndicatorLights(0, true);
        else
            vehicle.setIndicatorLights(0, false);

        let isFreeze = global.getVariable(vehicle, 'veh:isFreeze', false);
        vehicle.freezePosition(isFreeze);

        global.EntityStreamInTuning(vehicle);
        global.VehicleSetSharedDataHandlingMods(vehicle);
        //Make doors breakable again
        setTimeout(() => {
            for (x = 0; x < 8; x++) {
                if (vehicle && mp.vehicles.exists(vehicle))
                    vehicle.setDoorBreakable(x, true);
            }
            vehicle.freezePosition(false);
        }, 1500);
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("player:teleportInCar", (position, time) => {
    let vehicle = mp.players.local.vehicle;
    if (!vehicle) return;
    vehicle.setCoordsNoOffset(position.x, position.y, position.z, false, false, false);
    mp.events.call("FixTeleportInVehicle", time);
});

mp.events.add("FixTeleportInVehicle", (time) => {
    let vehicle = mp.players.local.vehicle;
    if (!vehicle) return;
    setTimeout(() => {
        vehicle.setOnGroundProperly();
        vehicle.freezePosition(true);
        setTimeout(() => {
            vehicle.freezePosition(false);
            vehicle.setOnGroundProperly();
        }, time);
        vehicle.setPaintFade(1);
    }, 0);
});

mp.events.add("playerEnterVehicle", (vehicle, seat) => {
    try {
        if (!CheckExistVehicle(vehicle))
            return;
        //mp.serverLog(`seat - ${seat}`);
        if (seat == -1) {
            let engState = global.getVariable(vehicle, 'veh:engineStatus', false);
            vehicle.setEngineOn(engState, engState, !engState);
            vehicle.setUndriveable(!engState);
            _oldPosition = vehicle.position;
            currDirtLvl = global.getVariable(vehicle, 'veh:dirtLevel', 5);
            _distance = 0;
            oldEngineHealth = vehicle.getEngineHealth();

            mp.players.local.setConfigFlag(32, true);
            updateBeltSound();
            godSpeedOn = false;
            global.reduce = false;
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.playerEnterVehicle: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("VehStream_SetSirenSound", (veh, status) => {
    try {
        if (!CheckExistVehicle(veh))
            return;
        if (veh.getClass() == 18)
            veh.setSirenSound(status);
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.VehStream_SetSirenSound: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

let soundIdBelt = 2;
let timerSoundBelt = null;

function updateBeltSound() {
    try {
        let vehicle = mp.players.local.vehicle;
        if (!CheckExistVehicle(vehicle))
            return;
        if (mp.players.local.vehicle.getPedInSeat(-1) != mp.players.local.handle)
            return;
        let belt = mp.players.local.getConfigFlag(32, true);
        let engine = vehicle.getIsEngineRunning();
        stopBeltTimeout();
        if (engine && belt) {
            mp.game.audio.playSoundFromEntity(soundIdBelt, "Crate_Beeps", vehicle.handle, "MP_CRATE_DROP_SOUNDS", true, 0);
            timerSoundBelt = setTimeout(() => {
                mp.game.audio.playSoundFromEntity(soundIdBelt, "DEVICE", vehicle.handle, "EPSILONISM_04_SOUNDSET", true, 0);
            }, 5000);
        }
        else
            mp.game.audio.playSoundFromEntity(soundIdBelt, "DEVICE", vehicle.handle, "EPSILONISM_04_SOUNDSET", true, 0);
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.updateBeltSound: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function stopBeltTimeout() {
    if (timerSoundBelt) {
        clearTimeout(timerSoundBelt);
        timerSoundBelt = null;
    }
}

mp.keys.bind(global.Keys.Key_1, false, () => {
    if (global.checkIsAnyActivity()) return;
    if (mp.keys.isUp(global.Keys.Key_RCONTROL))
        taskWarpIntoVehicleDriverSeat();
});
function taskWarpIntoVehicleDriverSeat() {
    try {
        let vehicle = mp.players.local.vehicle;
        if (!CheckExistVehicle(vehicle))
            return;
        if (mp.players.local.vehicle.getPedInSeat(-1) == mp.players.local.handle)
            return;
        mp.players.local.taskWarpIntoVehicle(vehicle.handle, -1);
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.taskWarpIntoVehicleDriverSeat: ${e.name}\n${e.message}\n${e.stack}`);
    }
};

mp.events.add("VehStream_SetLockStatus", (veh, status) => {
    try {
        if (!CheckExistVehicle(veh))
            return;
        if (status)
            mp.game.audio.playSoundFromEntity(1, "Remote_Control_Close", veh.handle, "PI_Menu_Sounds", true, 0);
        else
            mp.game.audio.playSoundFromEntity(1, "Remote_Control_Open", veh.handle, "PI_Menu_Sounds", true, 0);


        veh.setIndicatorLights(0, true);
        veh.setIndicatorLights(1, true);
        setTimeout(() => {
            veh.setIndicatorLights(0, false);
            veh.setIndicatorLights(1, false);
        }, 2000);

    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.VehStream_SetLockStatus: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("VehStream_PlayerExitVehicleAttempt", (vehicle, engState) => {
    if (!CheckExistVehicle(vehicle))
        return;
    vehicle.setEngineOn(engState, engState, !engState);
    vehicle.setUndriveable(engState);
    if (vehicle != null && _distance > 1) {
        mp.events.callRemote('veh:addDistance', vehicle, _distance);
        _distance = 0;
    }
});

mp.events.add("gui:ready", () => {
    global.keyActions["belt"].subscribe(beltPressKey, true);
})

function beltPressKey() {
    if (!global.localplayer.isInAnyVehicle(true)) return;
    let flag = mp.players.local.getConfigFlag(32, true);
    mp.players.local.setConfigFlag(32, !flag);
    global.gui.setData('speedometer/setBelt', flag);
    //mp.events.callRemote('seatbelt');    
    global.RefreshHints();
    updateBeltSound();
}


// mp.keys.bind(global.Keys.Key_J, true, () => {
//     if (!global.localplayer.isInAnyVehicle(true)) return;
//     let flag = mp.players.local.getConfigFlag(32, true);
//     mp.players.local.setConfigFlag(32, !flag);
//     global.gui.setData('speedometer/setBelt', flag);
//     //mp.events.callRemote('seatbelt');    
//     global.RefreshHints();
// });

function SetVehicleEnginePower(vehicle, power, engineHealth) {
    let pwr = (power + 100) * (engineHealth + 1000) / 2000 - 100;
    vehicle.setEnginePowerMultiplier(pwr);
}




mp.events.addDataHandler('ENGINEPOWER', (vehicle, enginePower) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof enginePower) !== 'undefined' && enginePower != null) {
        let engineHealth = global.getVariable(vehicle, 'veh:engineHealth', 1000);
        SetVehicleEnginePower(vehicle, enginePower, engineHealth);
    }
});

mp.events.addDataHandler('veh:engineHealth', (vehicle, engineHealth) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof engineHealth) !== 'undefined' && engineHealth != null) {
        let enginePower = global.getVariable(vehicle, 'ENGINEPOWER', 0);
        SetVehicleEnginePower(vehicle, enginePower, engineHealth);
    }
});

mp.events.addDataHandler('veh:engineStatus', (vehicle, status) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof status) !== 'undefined' && status != null) {
        vehicle.setEngineOn(status, status, !status);
        vehicle.setUndriveable(!status);
        if (mp.players.local.vehicle == vehicle) {
            updateBeltSound();
            global.RefreshHints();
        }
    }

});

mp.events.addDataHandler('veh:doorStatus', (vehicle, doorStatus) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof doorStatus) !== 'undefined' && doorStatus != null) {
        for (let index = 0; index < 8; index++) {
            let st = (doorStatus >> index) % 2;
            if (st == 1)
                vehicle.setDoorOpen(index, false, false);
            else
                vehicle.setDoorShut(index, false);
        }
    }
});

mp.events.addDataHandler('veh:doorBreak', (vehicle, doorBreak) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof doorBreak) !== 'undefined' && doorBreak != null) {
        for (let index = 0; index < 8; index++) {
            let st = (doorBreak >> index) % 2;
            if (st == 1)
                vehicle.setDoorBroken(index, true);
        }
    }
});

mp.events.addDataHandler('veh:dirtLevel', (vehicle, dirtLevel) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof dirtLevel) !== 'undefined' && dirtLevel != null) {
        vehicle.setDirtLevel(dirtLevel);
        if (vehicle.getPedInSeat(-1) == mp.players.local.handle) {
            currDirtLvl = dirtLevel;
        }

    }
});

mp.events.addDataHandler('veh:isFreeze', (vehicle, isFreezed) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof isFreezed) !== 'undefined' && isFreezed != null) {
        vehicle.freezePosition(isFreezed);
    }
});

mp.events.addDataHandler('veh:turnSignal', (vehicle, turnSignal) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof turnSignal) !== 'undefined' && turnSignal != null) {
        if (turnSignal % 2 == 1)
            vehicle.setIndicatorLights(1, true);
        else
            vehicle.setIndicatorLights(1, false);
        if (turnSignal > 1)
            vehicle.setIndicatorLights(0, true);
        else
            vehicle.setIndicatorLights(0, false);
    }
});



//car key

let carKeyOpen = false;

mp.events.add('vehicle::key::openKey', (vehicleId) => {
    mp.events.call('cef:inv:close');
    global.gui.setData('hud/openKey', JSON.stringify({ show: true, vehicleId: vehicleId }));
    carKeyOpen = true;
    global.showCursor(true);
});

mp.keys.bind(global.Keys.Key_DOWN, false, () => {
    if (carKeyOpen) {
        carKeyOpen = false;
        global.gui.setData('hud/openKey', JSON.stringify({ show: false, vehicleId: 0 }));
        global.showCursor(false);
    }
});
