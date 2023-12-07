
//god speed
let godSpeedModOn = false;
let godSpeedOn = false;
let step = 10;
let currSpeed = 200;

mp.events.add('godspeedon', (newspeed, newstep) => {
    if (global.getVariable(mp.players.local, 'ALVL', 0) < 1)
        return;
    godSpeedModOn = !godSpeedModOn;
    step = newstep;
    currSpeed = newspeed / 3.6;
});

mp.keys.bind(global.Keys.Key_NUMPAD8, false, function () {
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 || !godSpeedModOn || global.chatActive || mp.gui.cursor.visible || global.editing || global.gui.isOpened()) return;
    currSpeed += (step / 3.6);
    if (currSpeed > 1000)
        currSpeed = 1000;
});
mp.keys.bind(global.Keys.Key_NUMPAD2, false, function () {
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 || !godSpeedModOn || global.chatActive || mp.gui.cursor.visible || global.editing || global.gui.isOpened()) return;
    currSpeed -= (step / 3.6);
    if (currSpeed < -1000)
        currSpeed = -1000;
});
mp.keys.bind(global.Keys.Key_NUMPAD7, false, function () {
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 || !godSpeedModOn || global.chatActive || mp.gui.cursor.visible || global.editing || global.gui.isOpened()) return;
    step /= 10;
    if (step < 1)
        step = 1;
});
mp.keys.bind(global.Keys.Key_NUMPAD9, false, function () {
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 || !godSpeedModOn || global.chatActive || mp.gui.cursor.visible || global.editing || global.gui.isOpened()) return;
    step *= 10;
    if (step > 1000)
        step = 1000;
});
mp.keys.bind(global.Keys.Key_NUMPAD5, false, function () {
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 || !godSpeedModOn || global.chatActive || mp.gui.cursor.visible || global.editing || global.gui.isOpened()) return;
    godSpeedOn = !godSpeedOn;
});
mp.keys.bind(global.Keys.Key_NUMPAD0, false, function () {
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 || !godSpeedModOn || global.chatActive || mp.gui.cursor.visible || global.editing || global.gui.isOpened()) return;

    if (mp.players.local.isInAnyVehicle(true)) {
        if (mp.players.local.vehicle) {
            mp.players.local.vehicle.setForwardSpeed(currSpeed);
        }
    }
});
mp.keys.bind(global.Keys.Key_NUMPAD1, false, function () {
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 || !godSpeedModOn || global.chatActive || mp.gui.cursor.visible || global.editing || global.gui.isOpened()) return;

    if (mp.players.local.isInAnyVehicle(true)) {
        if (mp.players.local.vehicle) {
            mp.players.local.vehicle.setMaxSpeed(currSpeed);
        }
    }
});


mp.events.add('render', () => {
    if (!godSpeedModOn)
        return;
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 || !godSpeedModOn) return;

    mp.game.graphics.drawText('GodSpeed:' + godSpeedOn, [0.5, 0.005], {
        font: 7,
        color: [255, 100, 100, 185],
        scale: [0.4, 0.4],
        outline: true
    });
    mp.game.graphics.drawText('Speed:' + Math.round(currSpeed * 3.6), [0.5, 0.030], {
        font: 7,
        color: [255, 100, 100, 185],
        scale: [0.4, 0.4],
        outline: true
    });
    mp.game.graphics.drawText('Step:' + step, [0.5, 0.055], {
        font: 7,
        color: [255, 100, 100, 185],
        scale: [0.4, 0.4],
        outline: true
    });

    if (godSpeedOn && mp.players.local.isInAnyVehicle(true)) {
        if (mp.players.local.vehicle) {
            mp.players.local.vehicle.setForwardSpeed(currSpeed);
        }
    }
});