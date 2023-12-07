global.defaultCLothes =
{
    [true]:
    {
        [3]: 15,
        [4]: 61,
        [5]: 0,
        [6]: 34,
        [7]: 0,
        [8]: 15,
        [9]: 0,
        [10]: 0,
        [11]: 15,
    },
    [false]:
    {
        [3]: 15,
        [4]: 56,
        [5]: 0,
        [6]: 35,
        [7]: 0,
        [8]: 6,
        [9]: 0,
        [10]: 0,
        [11]: 101,
    },
}


const components = {
    "0": {
        "0": 1,
        "1": 196,
        "2": 80,
        "3": 241,
        "4": 145,
        "5": 100,
        "6": 106,
        "7": 121,
        "8": 233,
        "9": 56,
        "10": 128,
        "11": 400
    },
    "1": {
        "0": 1,
        "1": 195,
        "2": 76,
        "3": 196,
        "4": 138,
        "5": 100,
        "6": 102,
        "7": 152,
        "8": 187,
        "9": 56,
        "10": 120,
        "11": 382
    }
}

global.setClothing = (entity, id, drawable, color1, color2) => {
    const component = components[(entity.getVariable("GENDER")) ? "1" : "0"][id];
    //mp.serverLog(`setClothing.check: ${entity.name} ${component.start} ${component.end}`);
    let drawable_fixed = drawable > 499 ? drawable - 500 + component : drawable;
    entity.setComponentVariation(id, drawable_fixed, color1, color2);
}

mp.events.addDataHandler("GENDER", (entity, gender) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        global.updateClientClothes(entity);
    }
});

mp.events.add('entityStreamIn', (entity) => {
    global.updateClientClothes(entity);
});

mp.events.add('playerSpawn', (entity) => {
    global.updateClientClothes(entity);
});

global.updateClientClothes = (player) => {
    try {
        if (player.type === 'player') {
            let clothes = player.getVariable('clothes::compon1');
            if (clothes != undefined) {
                global.setClothing(player, 1, clothes[0], clothes[1], 0);
            }
            clothes = player.getVariable('clothes::compon2');
            if (clothes != undefined) {
                global.setClothing(player, 2, clothes[0], clothes[1], 0);
            }
            clothes = player.getVariable('clothes::compon9');
            if (clothes != undefined) {
                global.setClothing(player, 9, clothes[0], clothes[1], 0);
            }
            let color = player.getVariable("makeup");
            if (color !== undefined) {
                player.setHeadOverlayColor(4, 2, +color, +color);
            }

            let costume = global.getVariable(player, "clothes::costume", -1);
            if (costume < 3) {
                setVariableClothes(player);
                global.updateClientProps(player);
            }
            else {
                setCostumes(player, costume);
            }
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`updateClientClothes: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

mp.events.addDataHandler("clothes::costume", (entity, costume) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (!costume || costume < 3) {
            setVariableClothes(entity);
            global.updateClientProps(entity);
        }
        else {
            setCostumes(entity, costume);
        }
    }
});

function setCostumes(player, variant) {
    try {
        if (player.type !== 'player') return;
        const config = global.costumeConfigs[variant];
        if (!config) {
            setVariableClothes(player);
            global.updateClientProps(player);
        }
        else {
            for (let i = 3; i <= 11; i++) {
                if (i == 9) continue;
                if (config.ClothesDto[i] === undefined) {
                    global.setClothing(player, i, global.defaultCLothes[config.Gender][i], 0, 0);
                }
                else {
                    global.setClothing(player, i, config.ClothesDto[i].Drawable, config.ClothesDto[i].Texture, 0);
                }
            }
            for (let i = 0; i <= 12; i++) {
                if (config.PropsDto[i] === undefined)
                    global.setProp(player, i, -1, -1);
                else
                    global.setProp(player, i, config.PropsDto[i].Drawable, config.PropsDto[i].Texture);
            }
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`setCostumes: ${e.name}\n${e.message}\n${e.stack}`);
    }

}
function setVariableClothes(player) {
    try {
        if (player.type !== 'player') return;

        clothes = player.getVariable('clothes::compon3');
        if (clothes != undefined) {
            global.setClothing(player, 3, clothes[0], clothes[1], 0);
        }
        clothes = player.getVariable('clothes::compon4');
        if (clothes != undefined) {
            global.setClothing(player, 4, clothes[0], clothes[1], 0);
        }
        clothes = player.getVariable('clothes::compon5');
        if (clothes != undefined) {
            global.setClothing(player, 5, clothes[0], clothes[1], 0);
        }
        clothes = player.getVariable('clothes::compon6');
        if (clothes != undefined) {
            global.setClothing(player, 6, clothes[0], clothes[1], 0);
        }
        clothes = player.getVariable('clothes::compon7');
        if (clothes != undefined) {
            global.setClothing(player, 7, clothes[0], clothes[1], 0);
        }
        clothes = player.getVariable('clothes::compon8');
        if (clothes != undefined) {
            global.setClothing(player, 8, clothes[0], clothes[1], 0);
        }
        clothes = player.getVariable('clothes::compon9');
        if (clothes != undefined) {
            global.setClothing(player, 9, clothes[0], clothes[1], 0);
        }
        clothes = player.getVariable('clothes::compon10');
        if (clothes != undefined) {
            global.setClothing(player, 10, clothes[0], clothes[1], 0);
        }
        clothes = player.getVariable('clothes::compon11');
        if (clothes != undefined) {
            global.setClothing(player, 11, clothes[0], clothes[1], 0);
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`setVariableClothes: ${e.name}\n${e.message}\n${e.stack}`);
    }

}



mp.events.addDataHandler("clothes::compon1", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        global.setClothing(entity, 1, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon2", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        global.setClothing(entity, 2, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon3", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
            global.setClothing(entity, 3, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon4", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
            global.setClothing(entity, 4, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon5", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
            global.setClothing(entity, 5, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon6", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
            global.setClothing(entity, 6, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon7", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
            global.setClothing(entity, 7, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon8", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
            global.setClothing(entity, 8, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon9", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        global.setClothing(entity, 9, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon10", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
            global.setClothing(entity, 10, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("clothes::compon11", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
            global.setClothing(entity, 11, clothes[0], clothes[1], 0);
    }
});

mp.events.addDataHandler("makeup", (entity, color) => {
    if (entity.type === 'player' && mp.players.exists(entity))
        entity.setHeadOverlayColor(4, 2, +color, +color);
});

/*
let currClothes = 1;
let drawable = 0;
let texture = 0;
let step = 1;
let settable = true;

mp.events.add('render', () => {
    mp.game.graphics.drawText('currClothes:' + currClothes, [0.5, 0.005], {
        font: 7,
        color: [255, 255, 255, 185],
        scale: [0.4, 0.4],
        outline: true
    });
    mp.game.graphics.drawText('drawable:' + drawable, [0.5, 0.030], {
        font: 7,
        color: [255, 255, 255, 185],
        scale: [0.4, 0.4],
        outline: true
    });
    mp.game.graphics.drawText('texture:' + texture, [0.5, 0.055], {
        font: 7,
        color: [255, 255, 255, 185],
        scale: [0.4, 0.4],
        outline: true
    });
    mp.game.graphics.drawText('step:' + step, [0.5, 0.080], {
        font: 7,
        color: [255, 255, 255, 185],
        scale: [0.4, 0.4],
        outline: true
    });
    mp.game.graphics.drawText('settable:' + settable, [0.5, 0.105], {
        font: 7,
        color: [255, 255, 255, 185],
        scale: [0.4, 0.4],
        outline: true
    });
});


mp.keys.bind(global.Keys.Key_NUMPAD8, false, function () {
    if (texture >= 26)
        texture = 0;
    else
        texture++;
    if (settable)
        global.setClothing(global.localplayer, currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD2, false, function () {
    if (texture <= 0)
        texture = 26;
    else
        texture--;
    if (settable)
        global.setClothing(global.localplayer, currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD4, false, function () {
    drawable -= step;
    if (drawable < 0)
        drawable = 600;
    if (settable)
        global.setClothing(global.localplayer, currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD6, false, function () {
    drawable += step;
    if (drawable > 600)
        drawable = 0;
    if (settable)
        global.setClothing(global.localplayer, currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD9, false, function () {
    if (currClothes >= 11)
        currClothes = 0;
    else
        currClothes++;
    if (settable)
        global.setClothing(global.localplayer, currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD7, false, function () {
    if (currClothes <= 0)
        currClothes = 11;
    else
        currClothes--;
    if (settable)
        global.setClothing(global.localplayer, currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD0, false, function () {
    drawable = 0;
    texture = 0;
    if (settable)
        global.setClothing(global.localplayer, currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD3, false, function () {
    step *= 10;
    if (step >= 1000)
        step = 100;
});

mp.keys.bind(global.Keys.Key_NUMPAD1, false, function () {
    step /= 10;
    if (step < 1)
        step = 1;
});

mp.keys.bind(global.Keys.Key_NUMPAD5, false, function () {
    settable = !settable;
});
//*/