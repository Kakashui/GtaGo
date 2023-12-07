let timer
const MAX_PLAYER_SPEED = 7.2
const MAX_DISTANCE_PER_SECOND = 170;

global.activateAntiCheat = function () {
    mp.players.local.setMaxSpeed(MAX_PLAYER_SPEED);
}

global.disableAntiCheat = function () {
    if (timer) clearInterval(timer)
}

const forbiddenWeapon = [
    1119849093,
    2982836145,
    1752584910,
    1834241177,
    3056410471,
    125959754,
    1672152130,
    1305664598,
    2726580491,
    2138347493,
    3125143736,
    2481070269,
    741814745,
    615608432,
    2874559379
]

global.chw = () => {
    if (global.getVariable(mp.players.local, 'ALVL', 0) < 1) {
        let interval = setInterval(() => {
            if (global.getVariable(mp.players.local, 'ALVL', 0) > 0) {
                clearInterval(interval);
                return;
            }
            const weapon = global.getCurrentPlayerWeapon();
            if (forbiddenWeapon.find(w => w === weapon))
                mp.events.callRemote("ac:w:fb", weapon);
        }, 1000)
    }
}

setInterval(() => {
    checkTeleport()
}, 200)

let prevPosition = mp.players.local.position;
let prevDimension = mp.players.local.dimension;
let prevCheck = Date.now();
let blockReport = Date.now();
let currSpeed = 0;
let newPositionTeleport = null;

mp.events.add('teleport:newPos', (pos) => {    
    mp.players.local.position = pos;
    prevPosition = pos;
    blockReport = Date.now() + 1000;
});

mp.events.add('teleport:toVehicle', (vehicle, pos, seat) => {    
    mp.players.local.position = pos;
    prevPosition = pos;
    blockReport = Date.now() + 3000;
    seatInToVegicle(vehicle, seat);
});

async function seatInToVegicle(vehicle, seat) {
    while (!(vehicle && vehicle.type === 'vehicle' && vehicle.handle !== 0)) {
        await mp.game.waitAsync(0);
        blockReport = Date.now() + 1000;
    }
    mp.players.local.setIntoVehicle(vehicle.handle, seat);
}

global.teleport_newPos = (pos) => {    
    mp.players.local.position = pos;
    prevPosition = pos;
    blockReport = Date.now() + 1000;
};

function checkTeleport() {
    if (global.getVariable(mp.players.local, 'ALVL', 0) > 0)
        return;

    if (global.fly.flying) {
        blockReport = Date.now() + 2000;
        return;
    }
    let time = (Date.now() - prevCheck) / 1000;
    prevCheck = Date.now();
    let currPos = mp.players.local.position;
    let distance = mp.game.system.vdist(currPos.x, currPos.y, currPos.z, prevPosition.x, prevPosition.y, prevPosition.z);
    currSpeed = Math.round(distance * 3.6 / time);
    if (checkPlayerTeleport(currSpeed, currPos)) {
        blockReport = Date.now() + 1000;
        mp.events.callRemote("ac:teleport", `${Math.round(prevPosition.x)}, ${Math.round(prevPosition.y)}, ${Math.round(prevPosition.z)}`, `${Math.round(currPos.x)}, ${Math.round(currPos.y)}, ${Math.round(currPos.z)}`, Math.round(currSpeed), Math.round(distance));
    }
    prevPosition = currPos;
    prevDimension = mp.players.local.dimension;
}

function checkPlayerTeleport(currSpeed, currPos) {
    if (global.fly.flying)
        return false;
    if (blockReport >= Date.now())
        return false;
    if (prevPosition.z < -130 && currPos.z > -70)
        return false;
    if (prevDimension != mp.players.local.dimension)
        return false;
    if (mp.players.local.vehicle)
        return currSpeed > 600 &&
            ((mp.players.local.vehicle.getSpeed() * 3.6).toFixed() > 580 || (mp.players.local.vehicle.getSpeed() * 3.6).toFixed() < 300) &&
            mp.players.local.vehicle.getPedInSeat(-1) === mp.players.local.handle
    else
        return currSpeed > 250;
}

mp.events.add('render', () => {
    mp.game.graphics.drawText(currSpeed, [0.5, 0.005], {
        font: 7,
        color: [255, 100, 150, 185],
        scale: [0.4, 0.4],
        outline: true
    });
});