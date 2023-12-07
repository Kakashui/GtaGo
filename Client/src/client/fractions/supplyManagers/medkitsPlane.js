const config = {
    
    landing: {
        createPoint: new mp.Vector3(-3233.014, 3565.014, 200),

        startPoint: new mp.Vector3(-2557.25, 3175.917, 33),
        endPoint: new mp.Vector3(-2356.761, 3059.933, 33)
    },

    takeoff: {
        endPoint: new mp.Vector3(-1105.839, 2342.379, 270)
    }
};

const entities = {
    plane: null,
    pilot: null
};

mp.events.add({
    "medkitsPlane:enterPlaneZone": () => {

    },

    "medkitsPlane:exitPlaneZone": () => {
        
    },

    "medkitsPlane:startLanding": () => {
        const { plane, pilot } = createPlaneEntities(config.landing.createPoint);

        pilot.taskPlaneLand(plane.handle, config.landing.startPoint.x, config.landing.startPoint.y, config.landing.startPoint.z,
            config.landing.endPoint.x, config.landing.endPoint.y, config.landing.endPoint.z);

        entities.plane = plane;
        entities.pilot = pilot;
    },

    "medkitsPlane:startTakeoff": () => {
        const { plane, pilot } = entities;
        
        pilot.taskPlaneMission(plane.handle, 0, 0, config.takeoff.endPoint.x, config.takeoff.endPoint.y, config.takeoff.endPoint.z, 4, 100, 0, 90, 0, 0);
    }
});

async function createPlaneEntities(planePosition) {
    const playerPos = mp.players.local.position;
    
    const plane = mp.vehicles.new(mp.game.joaat("titan"), planePosition,
    {
        heading: 0,
        numberPlate: "M3DK17",
        engine: true,
        locked: true,
        dimension: 0
    });
    for (let index = 0; plane.handle !== 0 && !plane.doesExist() && index < 250; index++) {
        mp.game.wait(0);
    };
    plane.setRotation(0, 0, 240, 2, true);
    plane.freezePosition(true);
        
    const pilotPed = mp.peds.newValid(1349953339, new mp.Vector3(playerPos.x, playerPos.y, playerPos.z - 20), 0, 0);
    if(pilotPed == null) return;
   
    mp.gui.chat.push(`plane and pilot ped succesfully created`);
    plane.setEngineOn(true, true, true);

    
    if(pilotPed !== null)pilotPed.freezePosition(false);
    mp.game.wait(0);
    if(pilotPed !== null)pilotPed.setIntoVehicle(plane.handle, -1);

    plane.freezePosition(false);
    plane.setEngineOn(true, true, true);
    
    return { plane: plane, pilot: pilotPed };
}