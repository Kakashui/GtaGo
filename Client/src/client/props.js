const components = {
    "0": 156,
    "1": 34,
    "2": 43,
    "3": 0,
    "4": 0,
    "5": 0,
    "6": 42,
    "7": 9,
    "8": 0,
    "9": 0,
    "10": 0,
    "11": 0,
    "12": 0
}

global.setProp = (entity, id, drawable, color1) => {
    const component = components[id];
    let drawable_fixed = drawable > 499 ? drawable - 500 + component : drawable;
    if (drawable_fixed > -1)
        entity.setPropIndex(id, drawable_fixed, color1, true);
    else
        entity.clearProp(id);
}

// mp.events.add('entityStreamIn', (entity) => {
//     global.updateClientProps(entity);
// });

// mp.events.add('playerSpawn', (entity) => {
//     global.updateClientProps(entity);
// });

global.updateClientProps = (player) => {
    try {
        if (player.type === 'player') {
            let clothes = player.getVariable('clothes::prop0');
            if (clothes != undefined) {
                    global.setProp(player, 0, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop1');
            if (clothes != undefined) {
                    global.setProp(player, 1, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop2');
            if (clothes != undefined) {
                    global.setProp(player, 2, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop3');
            if (clothes != undefined) {
                    global.setProp(player, 3, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop4');
            if (clothes != undefined) {
                    global.setProp(player, 4, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop5');
            if (clothes != undefined) {
                    global.setProp(player, 5, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop6');
            if (clothes != undefined) {
                    global.setProp(player, 6, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop7');
            if (clothes != undefined) {
                    global.setProp(player, 7, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop8');
            if (clothes != undefined) {
                    global.setProp(player, 8, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop9');
            if (clothes != undefined) {
                    global.setProp(player, 9, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop10');
            if (clothes != undefined) {
                    global.setProp(player, 10, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop11');
            if (clothes != undefined) {
                    global.setProp(player, 11, clothes[0], clothes[1]);
            }
            clothes = player.getVariable('clothes::prop12');
            if (clothes != undefined) {
                    global.setProp(player, 12, clothes[0], clothes[1]);
            }
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`updateClientProps: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

mp.events.addDataHandler("clothes::prop0", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 0, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop1", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 1, clothes[0], clothes[1]);
    }
});
mp.events.addDataHandler("clothes::prop2", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 2, clothes[0], clothes[1]);
    }
});
mp.events.addDataHandler("clothes::prop3", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 3, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop4", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 4, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop5", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 5, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop6", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 6, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop7", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 7, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop8", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 8, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop9", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 9, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop10", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 10, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop11", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 11, clothes[0], clothes[1]);
    }
});

mp.events.addDataHandler("clothes::prop12", (entity, clothes) => {
    if (entity.type === 'player' && mp.players.exists(entity)) {
        if (global.getVariable(entity, "clothes::costume", -1) < 3)
        global.setProp(entity, 12, clothes[0], clothes[1]);
    }
});

/*
let currClothes = 1;
let drawable = 0;
let texture = 0;

mp.events.add('render', () => {
    mp.game.graphics.drawText('currClothes:' + currClothes, [0.5, 0.005], {
      font: 7,
      color: [255, 255, 255, 185],
      scale: [0.5, 0.5],
      outline: true
    });
    mp.game.graphics.drawText('drawable:' + drawable, [0.5, 0.030], {
      font: 7,
      color: [255, 255, 255, 185],
      scale: [0.5, 0.5],
      outline: true
    });
    mp.game.graphics.drawText('texture:' + texture, [0.5, 0.045], {
      font: 7,
      color: [255, 255, 255, 185],
      scale: [0.5, 0.5],
      outline: true
    });
});


mp.keys.bind(global.Keys.Key_NUMPAD8, false, function () {
    if (texture > 26)
        texture = 0;
    else
        texture++;
    global.localplayer.setProp(currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD2, false, function () {
    if (texture <= 0)
        texture = 26;
    else
        texture--;
    global.localplayer.setProp(currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD4, false, function () {
    if (drawable <= 0)
        drawable = 360;
    else
        drawable--;
    global.localplayer.setProp(currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD6, false, function () {
    if (drawable >= 360)
        drawable = 0;
    else
        drawable++;
    global.localplayer.setProp(currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD9, false, function () {
    if (currClothes >= 11)
        currClothes = 0;
    else
        currClothes++;
    global.localplayer.setProp(currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD7, false, function () {
    if (currClothes <= 0)
        currClothes = 11;
    else
        currClothes--;
    global.localplayer.setProp(currClothes, drawable, texture, 0);
});

mp.keys.bind(global.Keys.Key_NUMPAD0, false, function () {
    drawable = 0;
    texture = 0;
    global.localplayer.setProp(currClothes, drawable, texture, 0);
});*/