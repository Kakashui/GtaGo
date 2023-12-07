global.isTransporteurWorker = false;
global.isFlyToCar = false;
global.createdVehicle = null;

mp.events.add('WORK::TRANSPORTEUR::CHANGE::STATE::SERVER', (stateName, boolValue) => {
    if (stateName == "isTransporteurWorker") {
        isTransporteurWorker = boolValue;
    }
    else if (stateName == "isFlyToCar") {
        isFlyToCar = boolValue;
    }
})

mp.events.add('WORK::TRANSPORTEUR::MAGNET::VEHICLE::SERVER', () => {
    setTimeout(() => {
        if(mp.players.local.vehicle)
            mp.players.local.vehicle.enableCargobobHook(1);
    }, 5000);
})

mp.events.add('WORK::TRANSPORTEUR::CREATE::VEHICLE::SERVER', (x, y, z, modelName, vehNumber) => {
    createdVehicle = mp.vehicles.new(mp.game.joaat(modelName), new mp.Vector3(x, y, z),
    {
        numberPlate: vehNumber,
        color: [[255, 0, 0],[255,0,0]]
    });
})

mp.events.add('WORK::TRANSPORTEUR::DELIVER::VEHICLE::SERVER', () => {
    if (!mp.players.local.vehicle || createdVehicle == null) return;
    if (mp.players.local.vehicle.isAttachedToCargobob(createdVehicle.handle)) {
        mp.events.callRemote("WORK::TRANSPORTEUR::DELIVER::END::CLIENT");
    }
})

mp.events.add('WORK::TRANSPORTEUR::DELETE::VEHICLE::SERVER', () => {
    if (createdVehicle == null) return;
    createdVehicle.destroy();
    createdVehicle = null;
})

mp.events.add('render', () => {
    if (!mp.players.local.vehicle || !global.isTransporteurWorker || !global.isFlyToCar || global.createdVehicle == null) return;
    
    try {
        if (mp.players.local.vehicle.isAttachedToCargobob(createdVehicle.handle)) {
            isFlyToCar = false;
            mp.events.callRemote("WORK::TRANSPORTEUR::DELIVER::START::CLIENT");
        }
    } catch (e) {
        //mp.gui.chat.push("CATCH WORKED");
        //mp.gui.chat.push(e.message);
    }
    
})

mp.events.add('WORK::TRANSPORTEUR::TIMER::SET::SERVER', (timeLeftSeconds) => {
    global.gui.setData('hud/startWorkTimer', timeLeftSeconds);
})

mp.events.add('WORK::TRANSPORTEUR::TIMER::STOP::SERVER', () => {
    global.gui.setData('hud/stopWorkTimer');
})