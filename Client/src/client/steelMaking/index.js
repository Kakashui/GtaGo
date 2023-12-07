let stoneModel = 'iron';//'prop_boxing_glove_01';//'prop_rock_5_d';//
let c4Model = 'stt_prop_c4_stack';
let flagInZone = -1;
let zonePosition = null;
let nextUpdateDate = Date.now();
let useDetector = false;
let objectC4 = {};
let objectsDrop = {};

let animTime = 2;



mp.events.add('OreVein:UseDetector', (value) => {
    useDetector = value;
})

mp.events.add('OreVein:EnterPoint', (point, position) => {
    flagInZone = point;
    zonePosition = position;
})

mp.events.add('OreVein:ExitPoint', (point) => {
    if (flagInZone == point)
        flagInZone = -1;
})

mp.events.add('OreVein:explosionDynamitTimer', (pos, time, point) => {
    try {
        global.gui.setData("timerTemo/setTimer", time);
        setTimeout(() => {
            objectC4[point] = mp.objects.new(mp.game.joaat(c4Model), new mp.Vector3(pos.x, pos.y, pos.z - 1), {
                alpha: 255,
                dimension: 0,
                rotation: new mp.Vector3(0, 0, 0)
            });
            while (!mp.objects.exists(objectC4[point])) {
                mp.game.wait(0);
            }
            objectC4[point].freezePosition(false);
            objectC4[point].setDynamic(true);
            objectC4[point].setActivatePhysicsAsSoonAsItIsUnfrozen(true);
            setTimeout(() => {
                if (objectC4[point]) {
                    objectC4[point].destroy();
                    objectC4[point] = null;
                }
            }, (time - animTime) * 1000);
        }, animTime * 1000);
    } catch (e) {
        if (global.sendException)
            mp.serverLog(`Error in steelMaking.OreVein:explosionDynamitTimer: ${e.name}\n${e.message}\n${e.stack}`);
    }
})


mp.events.add('OreVein:GetDropPositions', (pos, point, count) => {
    try {
        if (objectsDrop[point] !== null && objectsDrop[point] !== undefined) {
            SendPoint(point);
        }
        else {
            CreateOres(point, count, pos);
            setTimeout(() => {
                mp.game.fire.addExplosion(pos.x, pos.y, pos.z, 10, 1, false, true, 1);
                mp.game.fire.addExplosion(pos.x, pos.y + 0.1, pos.z, 9, 1, false, true, 1);
                mp.game.fire.addExplosion(pos.x + 0.1, pos.y + 0.1, pos.z, 8, 1, false, true, 1);
            }, 500);
            setTimeout(() => {
                SendPoint(point);
            }, 7000);
        }
    } catch (e) {
        if (global.sendException)
            mp.serverLog(`Error in steelMaking.OreVein:GetDropPositions: ${e.name}\n${e.message}\n${e.stack}`);
    }
})

function SendPoint(point) {
    try {
        let positions = [];
        objectsDrop[point].forEach(obj => {
            positions.push(obj.getCoords(false));
        });
        DestroyOres(point);

        mp.events.callRemote('OreVeid:CreateDrops', point, JSON.stringify(positions));
    } catch (e) {
        if (global.sendException)
            mp.serverLog(`Error in steelMaking.SendPoint: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function CreateOres(point, count, pos) {
    try {
        objectsDrop[point] = [];
        for (let i = 0; i < count; i++) {
            let obj = mp.objects.new(mp.game.joaat(stoneModel), new mp.Vector3(pos.x + (i % 3) * 0.3 - 0.3, pos.y + (i % 5) * 0.3 - 0.6, pos.z + 0.2 + 0.05 * i), {
                alpha: 255,
                dimension: 0,
                rotation: new mp.Vector3(0, 0, 0)
            });
            if (obj) {
                objectsDrop[point].push(obj);
                SetDinamic(obj);
            }
        }
    } catch (e) {
        if (global.sendException)
            mp.serverLog(`Error in steelMaking.CreateOres: ${e.name}\n${e.message}\n${e.stack}`);
    }
}
async function SetDinamic(obj) {
    while (obj && !obj.doesExist()) {
        await mp.game.waitAsync(0);
    }
    if (obj) {
        obj.freezePosition(false);
        obj.setDynamic(true);
        obj.setActivatePhysicsAsSoonAsItIsUnfrozen(true);
    }
}
function DestroyOres(point) {
    try {
        if (objectsDrop[point] != null && objectsDrop[point] != undefined) {
            objectsDrop[point].forEach(obj => {
                if (obj != null)
                    obj.destroy();
            });
            objectsDrop[point] = null;
        }
    } catch (e) {
        if (global.sendException)
            mp.serverLog(`Error in steelMaking.CreateOres: ${e.name}\n${e.message}\n${e.stack}`);
    }
}


mp.events.add('OreVein:explodeDynamit', (pos, point, count) => {
    try {
        if (objectC4[point]) {
            objectC4[point].destroy();
            objectC4[point] = null;
        }

        CreateOres(point, count, pos);
        setTimeout(() => {
            mp.game.fire.addExplosion(pos.x, pos.y, pos.z, 10, 1, true, false, 1);
            mp.game.fire.addExplosion(pos.x, pos.y + 0.1, pos.z, 9, 1, true, false, 1);
            mp.game.fire.addExplosion(pos.x + 0.1, pos.y + 0.1, pos.z, 8, 1, true, false, 1);
        }, 500);
        setTimeout(() => {
            DestroyOres(point);
        }, 10000);
    }
    catch (e) {
        mp.events.callRemote('srv_consoleLog', `Error in steelMaking.OreVein:explodeDynamit: ${e.name}\n${e.message}\n${e.stack}`);
    }
})


// mp.keys.bind(global.Keys.Key_NUMPAD8, false, function () {
//     mp.events.callRemote("OreVeid:savePoint");
// });

mp.events.add('render', () => {
    if (flagInZone >= 0 && useDetector) {
        if (nextUpdateDate < Date.now()) {
            let pos = mp.players.local.position;
            let dist = mp.game.gameplay.getDistanceBetweenCoords(zonePosition.x, zonePosition.y, zonePosition.z, pos.x, pos.y, pos.z, false);
            nextUpdateDate = Date.now() + (dist < 1 ? 1 : dist) * 100;
            mp.game.graphics.drawText(`.`, [0.01, 0.01], {
                scale: [2, 2],
                outline: true,
                color: [255, 0, 0, 255],
                font: 4
            });
            mp.game.audio.playSoundFrontend(-1, 'Beep_Red', 'DLC_HEIST_HACKING_SNAKE_SOUNDS', true);

        }
    }
});
