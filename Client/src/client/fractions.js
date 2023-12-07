let skinsList = [];
let lastSkin = "DressDefault"
let openedTypeMenu = 0;

let baseClothes =
{
    [true]: {
        [3]:15,
        [4]:61,
        [5]:0,
        [6]:34,
        [7]:0,
        [8]:15,
        [9]:0,
        [10]:0,
        [11]:15,
    },
    [false]: {
        [3]:15,
        [4]:56,
        [5]:0,
        [6]:35,
        [7]:0,
        [8]:6,
        [9]:0,
        [10]:0,
        [11]:101,
    },
};

mp.events.add("frac:changeclothesmenu", (skins, fracType) => {
    let listSkin = JSON.parse(skins);
    skinsList = [{ id: 0, label: 'DressDefault' }];
    let id = 1;
    if (fracType == 3)
        skinsList.push({ id: id++, label: "WorkInMyDress" })

    listSkin.forEach(skin => {
        skinsList.push({ id: id++, label: skin })
    });
    if(global.gui.isOpened()) global.gui.close();
    global.gui.openPage('DressingUpMenu');
    global.gui.setData('dressingUpMenu/setDresses', JSON.stringify(skinsList));
    openedTypeMenu = 0;
});

mp.events.add("family:changeclothesmenu", (skins) => {
    let listSkin = JSON.parse(skins);
    skinsList = [];
    let id = 0;
    listSkin.forEach(skin => {
        skinsList.push({ id: id++, label: skin })
    });
    global.gui.openPage('DressingUpMenu');
    global.gui.setData('dressingUpMenu/setDresses', JSON.stringify(skinsList));
    openedTypeMenu = 1;
});

mp.events.add("frac:tryonclothes", (clothesJson, propsJson, gender) => {
    if (!global.loggedin)
        return;
    let clothes = JSON.parse(clothesJson);
    let props = JSON.parse(propsJson);

    for(let i = 3; i <= 11; i++)
    {
        if (clothes[i])
            global.setClothing(mp.players.local, i, clothes[i].Drawable, clothes[i].Texture, 0);
        else
            global.setClothing(mp.players.local, i, baseClothes[gender][i], 0, 0);
    }
    for(let i = 0; i <= 2; i++)
    {
        if (props[i])
            global.setProp(mp.players.local, i, props[i].Drawable, props[i].Texture)
        else
            global.setProp(mp.players.local, i, -1, 0)
    }
    for(let i = 6; i <= 7; i++)
    {
        if (props[i])
            global.setProp(mp.players.local, i, props[i].Drawable, props[i].Texture)
        else
            global.setProp(mp.players.local, i, -1, 0)
    }
        
});

mp.events.add("frac:setCurrDress", (skinId) => {
    if (!global.loggedin)
        return;
    let skin = skinsList.find(item => item.id == skinId);
    mp.events.callRemote('frac:setskin', skin.label);
});

mp.events.add("frac:applyDress", (skinId) => {
    if (!global.loggedin)
        return;
    let skin = skinsList.find(item => item.id == skinId);
    lastSkin = skin.label;
    global.gui.close();
    global.showCursor(false);
    global.updateClientClothes(mp.players.local);
    mp.events.callRemote('frac:applyskin', skin.label, openedTypeMenu);
});

mp.events.add("frac:cancelDress", () => {
    if (!global.loggedin)
        return;
    global.gui.close();
    global.showCursor(false)
    global.updateClientClothes(mp.players.local);
    mp.events.callRemote('frac:applyskin', 'cancel', openedTypeMenu);
});

let lastcheck = Date.now();

mp.keys.bind(global.Keys.Key_OEM_1, false, function () {
    //if (global.checkIsAnyActivity()) return;
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    if (!mp.players.local.vehicle)
        return;
    mp.events.callRemote('police:spawnSpike');
});

mp.events.addDataHandler('placeOnGround', DataHandlerPlaceOnGround);

async function DataHandlerPlaceOnGround(object, toggle)
{
    try {
        if(object.type !== 'object') return;
        let index = 0;
        while ((!object.doesExist() || object.handle === 0) && index++ < 1000) {
            await mp.game.waitAsync(0);
        }
        if(object.handle === 0) return;
        if (toggle == true)
        {
            object.placeOnGroundProperly();
            object.notifyStreaming = true;
        }     
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in fractions.placeOnGround: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

//Sync data on stream in
mp.events.add("entityStreamIn", (object) => {
    try {
        if (!object || object.type !== "object") return;
        if (global.getVariable(object, 'placeOnGround', false))
            object.placeOnGroundProperly();
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in fractions.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("friskInterface:openmenu", (items, target, targetId) => {
    try {
        global.gui.setData('friskInterface/setData', JSON.stringify({items: items, name: target, id: targetId}));
        global.gui.openPage('FriskInterface');
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in fractions.openfriskmenu: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("friskInterface:close", () => {
    CloseFriskMenu();
});

mp.events.add("friskInterface:takeAll", () => {
    mp.events.callRemote('friskInterface:takeAllIllegal');
    CloseFriskMenu();
});

function CloseFriskMenu() {
    global.gui.close();
}
