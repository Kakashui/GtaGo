
global.VehicleNeons = new Map();

let nextUpdateNeonTime = Date.now();
let _intervalUpdateNeon = 500;


function CheckExistVehicle(vehicle) {
    return (vehicle && vehicle.type === 'vehicle' && vehicle.handle !== 0);
}

function GetNormalize(value, defaultValue) {
    if ((typeof value) === 'undefined' || value == undefined || value == null)
        return defaultValue;
    else
        return value;
}

function SetHLColor(vehicle, color) {
    try {
        if (vehicle && mp.vehicles.exists(vehicle))
            mp.game.invoke(global.getNative("_SET_VEHICLE_HEADLIGHTS_COLOUR"), vehicle.handle, color);
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in vehiclesync.SetHLColor: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

global.EntityStreamInTuning = (vehicle) => {

    let hlcolor = global.getVariable(vehicle, 'hlcolor', 0);
    SetHLColor(vehicle, hlcolor);

    let tyreColor = global.getVariable(vehicle, 'tyrecolor', [240, 240, 240]);
    vehicle.toggleMod(20, true);
    vehicle.setTyreSmokeColor(tyreColor[0], tyreColor[1], tyreColor[2]);

    let paintTypeP = global.getVariable(vehicle, 'paintTypeCarPrim', 1);
    vehicle.setModColor1(paintTypeP, 0, 0);


    let paintTypeS = global.getVariable(vehicle, 'paintTypeCarSec', 1);
    vehicle.setModColor2(paintTypeS, 0);

    let pearlColor = global.getVariable(vehicle, 'pearlColorCar', 0);
    let wheelcolor = global.getVariable(vehicle, 'wheelcolor', 0);

    vehicle.setExtraColours(pearlColor, wheelcolor);

    let flashingneon = global.getVariable(vehicle, 'veh:flashingneon', null);
    if (flashingneon != null) {
        vehicle.setNeonLightEnabled(0, true);
        vehicle.setNeonLightEnabled(1, true);
        vehicle.setNeonLightEnabled(2, true);
        vehicle.setNeonLightEnabled(3, true);
        global.VehicleNeons.delete(vehicle);
        if (flashingneon.length > 0)
            vehicle.setNeonLightsColour(flashingneon[0][0], flashingneon[0][1], flashingneon[0][2]);
        if (flashingneon.length > 1)
            global.VehicleNeons.set(vehicle, { colors: flashingneon, currColor: 0 });
    }
}

mp.events.addDataHandler('veh:flashingneon', (vehicle, flashingneon) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof flashingneon) !== 'undefined' && flashingneon != null) {
        vehicle.setNeonLightEnabled(0, true);
        vehicle.setNeonLightEnabled(1, true);
        vehicle.setNeonLightEnabled(2, true);
        vehicle.setNeonLightEnabled(3, true);
        global.VehicleNeons.delete(vehicle);
        if (flashingneon.length > 0)
            vehicle.setNeonLightsColour(flashingneon[0][0], flashingneon[0][1], flashingneon[0][2]);
        if (flashingneon.length > 1)
            global.VehicleNeons.set(vehicle, { colors: flashingneon, currColor: 0 });
    }
    else {
        vehicle.setNeonLightEnabled(0, false);
        vehicle.setNeonLightEnabled(1, false);
        vehicle.setNeonLightEnabled(2, false);
        vehicle.setNeonLightEnabled(3, false);
        global.VehicleNeons.delete(vehicle);
    }
});

mp.events.add('veh:setTyreBurst', () => {
    let vehicle = mp.players.local.vehicle;
    if (!CheckExistVehicle(vehicle))
        return;
    vehicle.setTyreBurst(0, false, 1000);
    vehicle.setTyreBurst(1, false, 1000);
    vehicle.setTyreBurst(4, false, 1000);
    vehicle.setTyreBurst(5, false, 1000);
});
mp.events.addDataHandler("hlcolor", (vehicle, color) => {
    if (!CheckExistVehicle(vehicle))
        return;
    SetHLColor(vehicle, GetNormalize(color, 0));
});

mp.events.addDataHandler("tyrecolor", (vehicle, color) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if ((typeof color) !== 'undefined' && color != null) {
        vehicle.toggleMod(20, true);
        vehicle.setTyreSmokeColor(color[0], color[1], color[2]);
    }
    else {
        vehicle.toggleMod(20, true);
        vehicle.setTyreSmokeColor(240, 240, 240);
    }
});

mp.events.addDataHandler("paintTypeCarPrim", (vehicle, paintType) => {
    if (!CheckExistVehicle(vehicle))
        return;
    vehicle.setModColor1(GetNormalize(paintType, 1), 0, 0);
    let pearlColor = global.getVariable(vehicle, 'pearlColorCar', 0);
    let wheelcolor = global.getVariable(vehicle, 'wheelcolor', 0);
    vehicle.setExtraColours(pearlColor, wheelcolor);
});

mp.events.addDataHandler("paintTypeCarSec", (vehicle, paintType) => {
    if (!CheckExistVehicle(vehicle))
        return;
    if (typeof (paintType) !== 'undefined' && paintType != null)
        vehicle.setModColor2(GetNormalize(paintType, 1), 0);
});

mp.events.addDataHandler("pearlColorCar", (vehicle, color) => {
    if (!CheckExistVehicle(vehicle))
        return;
    let wheelcolor = global.getVariable(vehicle, 'wheelcolor', 0);
    vehicle.setExtraColours(GetNormalize(color, 0), wheelcolor);
});

mp.events.addDataHandler("wheelcolor", (vehicle, color) => {
    if (!CheckExistVehicle(vehicle))
        return;
    let pearlColor = global.getVariable(vehicle, 'pearlColorCar', 0);
    vehicle.setExtraColours(pearlColor, GetNormalize(color, 0));
});


mp.events.add('render', () => {
    //Обновление неона
    if (Date.now() > nextUpdateNeonTime) {
        nextUpdateNeonTime = Date.now() + _intervalUpdateNeon;
        for (let key of global.VehicleNeons.keys()) {
            if (key != null && key != undefined && mp.vehicles.exists(key)) {
                global.VehicleNeons.get(key).currColor = ++global.VehicleNeons.get(key).currColor % global.VehicleNeons.get(key).colors.length;
                var color = global.VehicleNeons.get(key).colors[global.VehicleNeons.get(key).currColor];
                key.setNeonLightsColour(color[0], color[1], color[2]);
            }
        }
    }
});

mp.events.add('entityStreamOut', (vehicle) => {
    if (!vehicle || vehicle.type !== "vehicle") return;
    if (global.VehicleNeons.has(vehicle))
        global.VehicleNeons.delete(vehicle);
});