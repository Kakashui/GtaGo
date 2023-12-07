

let colShapeSchool = null;


mp.events.add("playerExitColshape", playerExitColshapeHandler);
function playerExitColshapeHandler(shape) {
    if (shape == colShapeSchool) {
        mp.events.callRemote('startquest:enterSchoolBlip');
        colShapeSchool.destroy();
        colShapeSchool = null;
    }
}

mp.events.add("startquest:Stage7AutoSchool", (shapePos) => {
    try {
        colShapeSchool = mp.colshapes.newCircle(shapePos.x, shapePos.y, 1, mp.players.local.dimension)
    }
    catch (e) {
        mp.serverLog(`Error in startquest:Stage7AutoSchool: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("startquest:Stage7AutoSchool:stop", () => {
    try {
        if (colShapeSchool) {
            colShapeSchool.destroy();
            colShapeSchool = null;
        }
    }
    catch (e) {
        mp.serverLog(`Error in startquest:Stage7AutoSchool:stop: ${e.name}\n${e.message}\n${e.stack}`);
    }
});