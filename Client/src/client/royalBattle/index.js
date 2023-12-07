require('./timecycles');
require('./battleMenu');


// let range = 500;
let height = 250;


let timeoutZone = 20
let currentZone =
{
    Center: new mp.Vector3(4991, -4886, -100),
    Range: 0,
}
let nextZone =
{
    Center: new mp.Vector3(4991, -4886, -100),
    Range: 0,
}
let zoneStartTime = Date.now();
let zoneConstrictionTime = 5;

let zoneDamage = 7;
let zoneDamageInterval = 350;



let inBattle = false;
global.isFight = false;
let inZone = true;

let isDemorganFight = false;

let zoneBlip = null;
let damageInterval = null;

//sound
let soundTimeout = null;
let soundVolume = 0.5;

//hud
let phase = 0;
let myKills = 0;
let allPlayers = 0;

mp.events.add('render', () => {
    if (!inBattle && !global.isDemorgan)
        return;
    let color = getColor();
    let zone = getZone();
    mp.game.graphics.drawMarker(
        1,
        zone.Center.x, zone.Center.y, zone.Center.z,
        0, 0, 0,
        0, 0, 0,
        zone.Range * 1.981, zone.Range * 1.981, height,
        color, 255 - color, 50, 156,
        false, false, 2,
        false, null, null, false
    );

    if (!global.isFight && !isDemorganFight)
        return;
    let dist = mp.game.gameplay.getDistanceBetweenCoords(zone.Center.x, zone.Center.y, zone.Center.z, mp.players.local.position.x, mp.players.local.position.y, zone.Center.z, true)
    if (inZone != dist < zone.Range)
        OnPlayerChangeInCircle(dist < zone.Range);

    // mp.game.graphics.drawText(`dist - ${dist}, zone.Range - ${zone.Range}`, [0.5, 0.035], {
    //     font: 7,
    //     color: [255, 100, 100, 185],
    //     scale: [0.4, 0.4],
    //     outline: true
    // });
    // mp.game.graphics.drawText(`timecyclesIndex - ${timecyclesIndex}, currTimecycle - ${currTimecycle}`, [0.5, 0.005], {
    //     font: 7,
    //     color: [255, 100, 100, 185],
    //     scale: [0.4, 0.4],
    //     outline: true
    // });
})

function getColor() {
    let color = 255;
    if (zoneStartTime > Date.now())
        return color;
    if (timeoutZone > 0) {
        color = Math.round(getTimeCoeff(zoneStartTime, timeoutZone) * 255);
    }
    return color;
}

function getZone() {
    let length = getTimeCoeff(zoneStartTime + (timeoutZone - zoneConstrictionTime) * 1000, zoneConstrictionTime);
    let zone =
    {
        Center: new mp.Vector3(currentZone.Center.x + (nextZone.Center.x - currentZone.Center.x) * length, currentZone.Center.y + (nextZone.Center.y - currentZone.Center.y) * length, currentZone.Center.Z),
        Range: nextZone.Range + (currentZone.Range - nextZone.Range) * (1 - length)
    }
    return zone;
}

function getTimeCoeff(startTime, timeLength) {
    if (timeLength == 0)
        return 0;
    let value = (Date.now() - startTime) / (timeLength * 1000);
    if (value > 1)
        return 1;
    if (value < 0)
        return 0;
    return value;
}
mp.events.add('royalBattle:createZone', (currZoneJson, nextZoneJson, constrictionTime, zoneTimer, isDemorgan) => {
    currentZone = parseZone(currZoneJson);
    nextZone = parseZone(nextZoneJson);
    zoneConstrictionTime = constrictionTime
    zoneStartTime = Date.now();
    timeoutZone = zoneTimer;
    setTimeout(() => {
        updateBlip(nextZone);
    }, (timeoutZone - constrictionTime) * 500);

    setTimeout(() => {
        soundTimeout = PlaySound('constrictionZone');
    }, (timeoutZone - constrictionTime) * 1000);
    if (isDemorgan) {
        isDemorganFight = true;
        PlaySound('demorganStartBattle')
    } else {
        inBattle = true;
        global.isFight = true;
        phase = 1;
        global.gui.setData('hud/setPhaseTimer', JSON.stringify({ show: true, title: `Phase ${phase}`, time: timeoutZone }));
    }
})

mp.events.add('royalBattle:updateZone', (nextZoneJson, constrictionTime, zoneTimer, isDemorgan) => {
    currentZone = nextZone
    nextZone = parseZone(nextZoneJson);
    zoneConstrictionTime = constrictionTime
    zoneStartTime = Date.now();
    timeoutZone = zoneTimer;
    setTimeout(() => {
        updateBlip(nextZone);
    }, (timeoutZone - constrictionTime) * 500);
    setTimeout(() => {
        soundTimeout = PlaySound('constrictionZone');
    }, (timeoutZone - constrictionTime) * 1000);
    if (!isDemorgan) {
        global.gui.setData('hud/setPhaseTimer', JSON.stringify({ show: false, title: ``, time: 0 }));
        phase++;
        global.gui.setData('hud/setPhaseTimer', JSON.stringify({ show: true, title: `Phase ${phase}`, time: timeoutZone }));
    }
})

function parseZone(zoneJson) {
    let zone = JSON.parse(zoneJson);
    zone.Center = new mp.Vector3(zone.Center.x, zone.Center.y, zone.Center.z);
    return zone;
}

function updateBlip(zone) {
    if (zoneBlip) {
        mp.game.ui.removeBlip(zoneBlip);
    }
    if (!inBattle && !global.isDemorgan)
        return;
    if (zone != null) {
        zoneBlip = mp.game.ui.addBlipForRadius(zone.Center.x, zone.Center.y, zone.Center.z, zone.Range);
        mp.game.invoke(global.getNative("SET_BLIP_SPRITE"), zoneBlip, 9);
        mp.game.invoke(global.getNative("SET_BLIP_ALPHA"), zoneBlip, 120);
        mp.game.invoke(global.getNative("SET_BLIP_COLOUR"), zoneBlip, 2);
    }
}

mp.events.add('royalBattle:startBattle', (countPlayers) => {
    allPlayers = countPlayers;
    myKills = 0;
    global.gui.setData('hud/setRoyalBattleStats', JSON.stringify({ alive: allPlayers, kills: myKills }));

    mp.players.local.freezePosition(true);
    global.gui.setData("setLoadScreen", 'true');
    setTimeout(() => {
        mp.players.local.freezePosition(false);
        global.gui.setData("setLoadScreen", 'false');
        mp.players.local.taskParachute(true);
        //PlaySound('startBattle');
    }, 2000);
});

mp.events.add('royalBattle:updateKills', (kills) => {
    myKills = kills;
    global.gui.setData('hud/setRoyalBattleStats', JSON.stringify({ alive: allPlayers, kills: myKills }));
});

mp.events.add('royalBattle:updateCountPlayers', (countPlayers) => {
    allPlayers = countPlayers;
    global.gui.setData('hud/setRoyalBattleStats', JSON.stringify({ alive: allPlayers, kills: myKills }));
});

mp.events.add('royalBattle:endBattle', (timeToSendToStartPosition) => {
    global.isFight = false;
    if (damageInterval != null) {
        clearInterval(damageInterval);
        damageInterval = null;
    }
    if (soundTimeout != null) {
        clearTimeout(soundTimeout);
        soundTimeout = null;
    }
    setTimeout(() => {
        inBattle = false;
        global.gui.setData("setLoadScreen", 'true');
        setTimeout(() => {
            global.gui.setData("setLoadScreen", 'false');
        }, 2000);
        updateBlip(null);
        global.gui.setData('hud/setPhaseTimer', JSON.stringify({ show: false, title: '', time: 0 }));
    }, timeToSendToStartPosition - 500);


    currentZone =
    {
        Center: new mp.Vector3(4991, -4886, -100),
        Range: 0,
    }
    nextZone =
    {
        Center: new mp.Vector3(4991, -4886, -100),
        Range: 0,
    }
});
mp.events.add('royalBattle:endDemorganBattle', () => {
    isDemorganFight = false;
});

mp.events.add("onConnectionLost", () => {
    try {
        updateBlip(null);
    } catch (e) {
        mp.game.graphics.notify(` ${e.name}\n${e.message}\n${e.stack}`);
    }
});

function OnPlayerChangeInCircle(toggle) {
    inZone = toggle;
    if (damageInterval != null) {
        clearInterval(damageInterval);
        damageInterval = null;
    }
    if (inZone) {
    }
    else {
        damageInterval = setInterval(() => {
            DamagePlayer()
        }, 1500);

    }
}

function DamagePlayer() {
    mp.game.graphics.setTimecycleModifier('damage');
    mp.players.local.applyDamageTo(zoneDamage, true);
    setTimeout(() => {
        mp.game.graphics.setTimecycleModifier('default');
    }, zoneDamageInterval);
}


function PlaySound(soundName) {
    if (!inBattle && !global.isDemorgan)
        return;
    global.gui.playSound(soundName, soundVolume, false);
}

// mp.keys.bind(global.Keys.Key_NUMPAD8, false, function () {
//     timecyclesIndex++;
//     currTimecycle = global.timecycles[timecyclesIndex];
//     mp.game.graphics.setTimecycleModifier(currTimecycle);
// });

// mp.keys.bind(global.Keys.Key_NUMPAD2, false, function () {
//     timecyclesIndex--;
//     if (timecyclesIndex < 0)
//         timecyclesIndex = 0;
//     currTimecycle = global.timecycles[timecyclesIndex];
//     mp.game.graphics.setTimecycleModifier(currTimecycle);
// });

// mp.keys.bind(global.Keys.Key_NUMPAD1, false, function () {
// });