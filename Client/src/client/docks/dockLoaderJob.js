let unloadPoints;
let currentUnloadPoint;
let objToTrack;
let objToTrackStartZPosition;
let timer;
let arrowMarker;
let vehMarker;
let unloadMarker;
let workBlip = null;

function calculateDistance(v1, v2) {
    let dx = v1.x - v2.x;
    let dy = v1.y - v2.y;
    let dz = v1.z - v2.z;

    return Math.sqrt(dx * dx + dy * dy + dz * dz);
}

function randomIntFromInterval(min, max) { // min and max included 
    return Math.floor(Math.random() * (max - min + 1) + min);
}

function placeOnGround(obj){
    obj.setActivatePhysicsAsSoonAsItIsUnfrozen(true);
    obj.placeOnGroundProperly();
    obj.setDynamic(true);
    obj.freezePosition(false);
}

mp.events.add('dockLoader:init', (cratesRemoteIds, points) => {
    cratesRemoteIds = JSON.parse(cratesRemoteIds);
    unloadPoints = JSON.parse(points);
    cratesRemoteIds.forEach(e => {
        let obj = mp.objects.atRemoteId(e);
        if(!obj || obj.handle === 0) return;
        
        for (let index = 0; !obj.doesExist() && index < 250; index++) {
            mp.game.wait(0);
        }
        placeOnGround(obj);
    });
});


mp.events.add('dockLoader:playerUnloaded', () => {
    if (objToTrack == undefined || objToTrack == null || currentUnloadPoint == null) return;
    if (calculateDistance(objToTrack.getCoords(false), currentUnloadPoint) > 5) return;
    objToTrack = null;
    if (unloadMarker != null){
        unloadMarker.destroy();
        unloadMarker = null;
    } 
    currentUnloadPoint = null;
    if (timer != null) clearInterval(timer);
    mp.events.callRemote("playerUnloadedDockCrate")
});

mp.events.add("dockLoader:destroyMarker", () => {
    global.sendTip('tip_docker_up');
    if (vehMarker != null){
        vehMarker.destroy();
        vehMarker = null;
    }
});

mp.events.add('dockLoader:vehicleLoaded', (position) => {
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

mp.events.add('dockLoader:CrateObjectsRequested', (id) => {
    let obj = mp.objects.atRemoteId(id);
    
    objToTrack = obj;
    objToTrackStartZPosition = obj.position.z;

    obj.notifyStreaming = true;
    obj.setDynamic(true);
    obj.freezePosition(false);
    if (unloadMarker != null && unloadMarker != undefined) {
        unloadMarker.destroy();
        unloadMarker = null;
    }
    arrowMarker = mp.markers.new(20, new mp.Vector3(obj.position.x, obj.position.y, obj.position.z + 2.5), 2, {
        bobUpAndDown: true,
        rotation: new mp.Vector3(0, 180, 0),
        color: [182, 211, 0, 200]
    });
    if (timer != null) clearInterval(timer);
    timer = setInterval(checkIfCrateLifted, 500);
    createWorkBlip(obj.position);
});

mp.events.add('dockLoader:stopedWorking', () => {
    if (timer !== null) clearInterval(timer);
    currentUnloadPoint = null;
    objToTrack = null;
    if (unloadMarker !== null && unloadMarker != undefined) {
        unloadMarker.destroy();
        unloadMarker = null;
    }
    if (arrowMarker !== null && arrowMarker != undefined) {
        arrowMarker.destroy();
        arrowMarker = null;
    }
    if (vehMarker != null){   
        vehMarker.destroy();
        vehMarker = null;
    }
    if (workBlip != null && mp.blips.exists(workBlip)){
        workBlip.destroy();
    }
});

function checkIfCrateLifted(){
    if (!objToTrack || objToTrack.handle == 0) return;
    objToTrack.freezePosition(false);
    if (Math.abs(objToTrack.getCoords(false).z - objToTrackStartZPosition) > 0.2){
        var randomIndex = randomIntFromInterval(0, unloadPoints.length - 1);
        currentUnloadPoint = unloadPoints[randomIndex];
        if (timer != null) clearInterval(timer);
        createWorkBlip(currentUnloadPoint);
        mp.events.call('notify', 2, 9, "DockLoader_6", 3000);

        if (arrowMarker != null) {
            arrowMarker.destroy();
            arrowMarker = null;
        }

        unloadMarker = mp.markers.new(0, currentUnloadPoint, 1, {
            rotation: new mp.Vector3(0, 0, 0),
            color: [182, 211, 0, 200],
        });
    }
}

function createWorkBlip(position){
    if (workBlip != null && mp.blips.exists(workBlip)){
        workBlip.destroy();
    }
    workBlip = mp.blips.new(478, position,
        {
            name: "Dock Unload Point",
            scale: 1.3,
            color: 3,
            alpha: 255,
            shortRange: false,
            rotation: 0,
            dimension: 0,
        });
    mp.game.invoke('0x4F7D8A9BFB0B43E9', workBlip.handle, true);
}