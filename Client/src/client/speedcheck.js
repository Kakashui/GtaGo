//let colChapeCamera = mp.colshapes.newSphere(1.0,1.0,75.0,10.0);
let Cameras = [
    {shape: mp.colshapes.newTube(294.0111, -499.0111, 43, 18, 5, {dimension: 0}), speedlimit: 80},
    {shape: mp.colshapes.newTube(219.5493, -688.0111, 37, 21, 5, {dimension: 0}), speedlimit: 80},
    {shape: mp.colshapes.newTube(-428.5026, -235.0853, 36, 18, 5, {dimension: 0}), speedlimit: 80},
    {shape: mp.colshapes.newTube(-544.0111, -290.0111, 35, 15, 5, {dimension: 0}), speedlimit: 80},
];
let reason = 'cl:speed:check';
Cameras.forEach(Camera => {   
    Camera.shape.dimension = 0;
});

function playerEnterColshapeHandler(shape) {
    Cameras.forEach(Camera => {        
        if(shape == Camera.shape) 
        {
            if  (mp.players.local.isInAnyVehicle(false) && mp.players.local.vehicle)
            {
                var speed = Math.floor (mp.players.local.vehicle.getSpeed() * 3.6);
                if (speed>Camera.speedlimit)
                {                   
                    let sum = Math.ceil((speed-Camera.speedlimit)/10.0)*50;
                    mp.events.callRemote('speeding_mulct', speed, Camera.speedlimit, sum, reason);
                }
            }
        }
    });
    
}

mp.events.add("playerEnterColshape", playerEnterColshapeHandler);
