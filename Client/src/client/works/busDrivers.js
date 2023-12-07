let vehMarker
let currentCheckpoint;
let currentBlip;

mp.events.add({
    'bus:vehicleLoaded': (position) => {
        position = JSON.parse(position);
        if (vehMarker != null){
            vehMarker.destroy();
            vehMarker = null;
        }
        vehMarker = mp.markers.new(20, new mp.Vector3(position.x, position.y, position.z + 4), 2, {
            rotation: new mp.Vector3(0, 180, 0),
            color: [182, 211, 0, 200]
        });
    },
    'bus:destroyMarker':() => {
        if (vehMarker != null) {
            vehMarker.destroy();
            vehMarker = null;
        }
    },
    'bus:createCheckpoint': (position, r, g, b) => {
        if (currentBlip != null && mp.blips.exists(currentBlip)){
            currentBlip.destroy();
        }
        if (currentCheckpoint != null && mp.checkpoints.exists(currentCheckpoint)){
            currentCheckpoint.destroy();
        }
        position = JSON.parse(position);
        currentBlip = mp.blips.new(1, position,
            {
                name: "Bus Checkpoint",
                scale: 1.3,
                color: 24,
                alpha: 255,
                shortRange: false,
                rotation: 0,
                dimension: 0,
            });
        currentCheckpoint = mp.checkpoints.new(4, position, 4, 
            {
                color: [r, g, b, 200],
                visible: true,
            });
        mp.game.invoke('0x4F7D8A9BFB0B43E9', currentBlip.handle, true);
    },
    'bus:clear': () => {
        if (currentBlip != null && mp.blips.exists(currentBlip)){
            currentBlip.destroy();
        }
        if (currentCheckpoint != null && mp.checkpoints.exists(currentCheckpoint)){
            currentCheckpoint.destroy();
        }
    },
});