let currentColshape
let currentMarker
let vehMarker
let opened = false

mp.events.add('truckers:setCheckPointRoutePath', (position) => {
    position = JSON.parse(position);
    resetCurrentRoutePath();

    currentColshape = mp.colshapes.newCircle(position.x, position.y, 2);
    currentColshape.isTrucker = true;

    currentMarker = mp.markers.new(20, new mp.Vector3(position.x, position.y, position.z + 0.5), 2, {
        rotation: new mp.Vector3(0, 180, 0),
        color: [182, 211, 0, 200],
    });
    createWorkBlip(position);
});

mp.events.add('truckers:resetCurrentRoutePath', () => {
    resetCurrentRoutePath();
    if (vehMarker != null){            
        vehMarker.destroy();
        vehMarker = null;
    }
});

function resetCurrentRoutePath(){
    if (currentColshape != null){
        currentColshape.destroy();
        currentColshape = null;
    }
    if (currentMarker != null){
        currentMarker.destroy();
        currentMarker = null;
    }
    if (workBlip != null){
        workBlip.destroy();
        workBlip = null;
    }
}

mp.events.add("playerEnterColshape", (shape) => {
    if (shape.isTrucker) {
        mp.events.callRemote("truckers:playerEnteredCheckpoint");
    }
});

mp.events.add('truckers:vehicleLoaded', (position) => {
    position = JSON.parse(position);
    if (vehMarker != null){
        vehMarker.destroy();
        vehMarker = null;
    }
    vehMarker = mp.markers.new(20, new mp.Vector3(position.x, position.y, position.z + 4), 2, {
        rotation: new mp.Vector3(0, 180, 0),
        color: [182, 211, 0, 200]
    });
});

mp.events.add("truckers:destroyMarker", () => {
    if (vehMarker != null) {
        vehMarker.destroy();
        vehMarker = null;
    }
});

mp.events.add('truckers:openRentPage', (truckersData) => {
    global.gui.setData('truckersMenu/setData', truckersData);
    opened = global.gui.openPage("TruckersMenu");
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened) {
        global.gui.close();
        opened = false;
    }
});

mp.events.add('truckers:startWorkTimer', (timeLeftSeconds) => {
    global.gui.setData('hud/startWorkTimer', timeLeftSeconds);
});

mp.events.add('truckers:stopWorkTimer', () => {
    global.gui.setData('hud/stopWorkTimer');
});

mp.events.add('truckersMenu:setTruck', (id) => {
    global.gui.close();
    opened = false;
    mp.events.callRemote("truckers:setTruck", id);
});

let workBlip = null;
function createWorkBlip(position){
    if (workBlip != null)
        workBlip.destroy();
    workBlip = mp.blips.new(615, position,
        {
            name: "Truck waypoint",
            scale: 1.3,
            color: 69,
            alpha: 255,
            drawDistance: 100,
            shortRange: false,
            rotation: 0,
            dimension: 0,
        });
    mp.game.invoke('0x4F7D8A9BFB0B43E9', workBlip.handle, true);
}