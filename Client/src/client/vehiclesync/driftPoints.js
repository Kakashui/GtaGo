
//Drift points
let points = 0;
let basepoint = 0;
let prevCheck = Date.now();
let prevDriftOn = Date.now();
let startDriftTime = Date.now();
let pointInSecond = 50;
let dropPointsTime = 2000;

let minSpeedX = 6;

let minSpeed = 10;

let oldHealth = 1000;

let factor = 1;

mp.events.add('render', () => {
    if (mp.storage.data.mainSettings.showDrift && mp.players.local.isInAnyVehicle(true) && mp.players.local.vehicle) {
        let vehicle = mp.players.local.vehicle;

        let rotate = vehicle.getSpeedVector(true).x;

        if (points > 0 && Date.now() - prevDriftOn > dropPointsTime) {
            ClearScore();
        }
        if (vehicle.getSpeed() > minSpeed && Math.abs(rotate) > minSpeedX && vehicle.getSpeedVector(true).y > 0) {
            prevDriftOn = Date.now();
            if (points == 0)
                startDriftTime = Date.now();
            let amount = ((Date.now() - prevCheck) / 1000 * pointInSecond) * Math.sqrt(Math.abs(rotate)) * Math.sqrt(vehicle.getSpeed());
            basepoint += amount;
            factor = Math.round(basepoint / 2000) <= 0 ? 1 : Math.round(basepoint / 2000);
            points += amount * factor;
        }
        prevCheck = Date.now();

        if (vehicle.getHealth() < oldHealth)
            ClearScore();
        oldHealth = vehicle.getHealth();

        if (points > 0) {
            let rotateDeg = Math.sqrt(Math.abs(rotate)) * (rotate / Math.abs(rotate));
            global.gui.setData('hud/setDriftScore', JSON.stringify({ show: true, value: Math.round(points), factor: factor, rotateDeg: Math.round(rotateDeg * 3) }));
        }
    }
});

function ClearScore() {
    points = 0;
    basepoint = 0;
    global.gui.setData('hud/setDriftScore', JSON.stringify({ show: false, value: 0, factor: 0, rotateDeg: 0 }));
}