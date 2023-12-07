let markers = [];
let blips = [];

mp.game.invoke('0xB98236CAAECEF897', true);


mp.events.add('createBlip', (id, sprite, name, position, scale, color, dimension) => {
    blips[id] = mp.blips.new(sprite, position,
        {
            name: name,
            scale: scale,
            color: color,
            alpha: 255,
            drawDistance: 100,
            shortRange: false,
            rotation: 0,
            dimension: dimension,
        }
    );
});
mp.events.add('deleteBlip', (uid) => {
    if (typeof blips[uid] != "undefined") {
        blips[uid].destroy();
        blips[uid] = undefined;
    }
});

mp.events.add('createCheckpoint', function (uid, type, position, scale, dimension, r, g, b, dir) {
    if (typeof markers[uid] != "undefined") {
        markers[uid].destroy();
        markers[uid] = undefined;
    }
    markers[uid] = mp.checkpoints.new(type, position, scale,
        {
            color: [r, g, b, 200],
            visible: true,
            dimension: dimension
        });
});
mp.events.add('createMarker', function (uid, type, position, scale, dimension, r, g, b, rot) {
    if (typeof markers[uid] != "undefined") {
        markers[uid].destroy();
        markers[uid] = undefined;
    }
    markers[uid] = mp.markers.new(type, position, scale,
        {
            rotation: rot,
            color: [r, g, b, 200],
            visible: true,
            dimension: dimension
        });
});

mp.events.add('deleteCheckpoint', function (uid) {
    if (typeof markers[uid] == "undefined") return;
    markers[uid].destroy();
    markers[uid] = undefined;
});

mp.events.add('createWaypoint', function (x, y) {
    mp.game.ui.setNewWaypoint(x, y);
});

let workBlip = null;
mp.events.add('createWorkBlip', function (position) {
    if (workBlip != null)
        workBlip.destroy();
    workBlip = mp.blips.new(0, position,
        {
            name: "checkpoint",
            scale: 1,
            color: 49,
            alpha: 255,
            drawDistance: 100,
            shortRange: false,
            rotation: 0,
            dimension: 0,
        });
});
mp.events.add('deleteWorkBlip', function () {
    if (workBlip != null)
        workBlip.destroy();
    workBlip = null;
});

let familyBlip = null;
mp.events.add('createFamilyBlip', function (position) {
    if (familyBlip != null) {
        familyBlip.destroy();
    }

    familyBlip = mp.blips.new(357, position, {
        name: "Family",
        scale: 1,
        color: 5,
        alpha: 255,
        drawDistance: 100,
        shortRange: true,
        rotation: 0,
        dimension: 0,
    });
});

mp.events.add('deleteFamilyBlip', function () {
    if (familyBlip != null) {
        familyBlip.destroy();
    }

    familyBlip = null;
});

let garageBlip = null;
mp.events.add('createGarageBlip', function (position) {
    if (garageBlip != null)
        garageBlip.destroy();
    garageBlip = mp.blips.new(524, position,
        {
            name: "Garage",
            scale: 1,
            color: 45,
            alpha: 255,
            drawDistance: 100,
            shortRange: true,
            rotation: 0,
            dimension: 0,
        });
});

let MisBlip = [null, null, null, null, null, null, null];
mp.events.add('createMissionGarageBlip', (position, val) => {
    try {
        if (MisBlip[val] != null)
            MisBlip[val].destroy();
        MisBlip[val] = mp.blips.new(473, position,
            {
                name: "Closed Garage",
                scale: 1,
                color: 45,
                alpha: 125,
                drawDistance: 100,
                shortRange: true,
                rotation: 0,
                dimension: 0,
            });
    } catch (e) { 
        if(global.sendException) mp.serverLog(`Error in checkpoints.createMissionGarageBlip: ${e.name}\n${e.message}\n${e.stack}`);
     }

});

mp.events.add('deleteMissionGarageBlip', () => {
    try {
        MisBlip.forEach(function (bliper) {
            if (bliper != null)
                bliper.destroy();
            bliper = null;
        });
    } catch (e) {  
        if(global.sendException) mp.serverLog(`Error in checkpoints.deleteMissionGarageBlip: ${e.name}\n${e.message}\n${e.stack}`);
     }
});

mp.events.add('deleteGarageBlip', function () {
    if (garageBlip != null)
        garageBlip.destroy();
    garageBlip = null;
});

mp.events.add('changeBlipColor', function (blip, color) {
    try {
        if (blip == null) return;
        blip.setColour(color);
    } catch (e) {
        if(global.sendException) mp.serverLog(`Error in checkpoints.changeBlipColor: ${e.name}\n${e.message}\n${e.stack}`);
       }
});

mp.events.add('changeBlipAlpha', function (blip, alpha) {
    try {
        if (blip == null) return;
        blip.setAlpha(alpha);
    } catch (e) {
        if(global.sendException) mp.serverLog(`Error in checkpoints.changeBlipAlpha: ${e.name}\n${e.message}\n${e.stack}`);
      }
});

let PrisMafBlip = null;
mp.events.add('createPrisMafBlip', (position) => {
    try {
        if (PrisMafBlip != null)
            PrisMafBlip.destroy();
        PrisMafBlip = mp.blips.new(310, position,
            {
                name: "Prison Mafia",
                scale: 1,
                color: 4,
                alpha: 255,
                drawDistance: 0,
                shortRange: true,
                rotation: 0,
                dimension: 0,
            }
        );
    }
    catch (e) {
        if(global.sendException) mp.serverLog(`Error in checkpoints.createPrisMafBlip: ${e.name}\n${e.message}\n${e.stack}`);
       }

});

mp.events.add('deletePrisMafBlip', () => {
    try {
        if (PrisMafBlip != null)
            PrisMafBlip.destroy();
        PrisMafBlip = null;
    }
    catch (e) { 
        if(global.sendException) mp.serverLog(`Error in checkpoints.deletePrisMafBlip: ${e.name}\n${e.message}\n${e.stack}`);
       }
});