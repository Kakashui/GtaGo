const moveSettings = {
    size: {
        x: 45,
        y: 45
    },
    showIcons:[true, true, false, true],
    values:[
        {//LEFT X
            value: 90,
            min: -90,
            max: 270,
            step: 5,
            invert: true,
            enabled: true,
            callback: "camMoveAngleX"
        },
        {//LEFT Y
            value: .4,
            min: -.5,
            max: 1.5,
            step: .05,
            invert: false,
            enabled: true,
            callback: "camMoveCamZ"
        },
        {//RIGHT X
            value: 2,
            min: 1,
            max: 3,
            step: .1,
            invert: false,
            enabled: false,
            callback: ""
        },
        {//RIGHT Y
            value: .9,
            min: -1.1,
            max: .9,
            step: .05,
            invert: true,
            enabled: true,
            callback: "camMovePointZ"
        },
        { //WHEELE
            value: 2,
            min: .5,
            max: 2,
            step: .5,
            invert: false,
            enabled: true,
            callback: "camSetDist"
        }
    ]
}

let tattooList = [];

mp.events.add('tattoo:open', (price) => {
    const gender = mp.players.local.getVariable("GENDER") ? true : false;
    global.gui.setData("tattooShop/setData", JSON.stringify({price, gender}));
    if(!global.gui.openPage("TattooShop")) return;
    mp.game.cam.doScreenFadeOut(50);
    mp.players.local.freezePosition(true);
    
    const pos = mp.players.local.position;
    global.customCamera.setPos(new mp.Vector3(1864.089, 3747.348, 33) );
    global.customCamera.setPoint(new mp.Vector3(1864.089, 3747.348, 33));
    global.customCamera.moveCamZ(.9);
    global.customCamera.movePointZ(.9);
    global.customCamera.setDist(2);
    global.customCamera.moveAngleX(90);
    global.customCamera.switchOn(0);
    global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
    global.gui.setData('mouseMove/setEnebled', true)
  
    mp.game.cam.doScreenFadeIn(1000);      
})

mp.events.add('tattoo:list:update', list => {
    tattooList = list;
    global.gui.setData('tattooShop/updateTattooList', JSON.stringify(list));
})

const categories = {
    "torso": 0,
    "head": 1,
    "leftarm": 2,
    "rightarm": 3,
    "leftleg": 4,
    "rightleg": 5,
}

mp.events.add('tattoo:select', (dict, name, slots)=>{
    mp.players.local.clearDecorations();
    tattooList.forEach(tattoo => {
        mp.players.local.setDecoration(tattoo.Collection, tattoo.Overlay);
    });
    mp.players.local.setDecoration(dict, name); // applay tattoo
})


mp.events.add('tattoo:buy', (category, id) => {
    if ( global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;
    const catId = categories[category];
    mp.events.callRemote("tattoo:buy", catId, id);
});


mp.events.add('tattoo:close', () => {
    if (global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;
    mp.players.local.freezePosition(false);
    global.gui.setData('mouseMove/setEnebled', false);
    global.gui.close();

    mp.players.local.clearDecorations();
    global.customCamera.switchOff(0);
    mp.events.callRemote("tattoo:close");
})

mp.events.add('tattoo:remove:open', () => {
    const gender = mp.players.local.getVariable("GENDER") ? true : false;
    global.gui.setData("tattooShop/setGender", gender);
    if(!global.gui.openPage("RemovingTattoo")) return;
    mp.game.cam.doScreenFadeOut(50);
    mp.players.local.freezePosition(true);
    
    const pos = mp.players.local.position;
    global.customCamera.setPos(new mp.Vector3(323.8332, -582.7084, 43.12974) );
    global.customCamera.setPoint(new mp.Vector3(323.8332, -582.7084, 43.12974));
    global.customCamera.moveCamZ(.9);
    global.customCamera.movePointZ(.9);
    global.customCamera.setDist(2);
    global.customCamera.moveAngleX(90);
    global.customCamera.switchOn(0);
    global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
    global.gui.setData('mouseMove/setEnebled', true)
  
    mp.game.cam.doScreenFadeIn(1000);      
})

mp.events.add('tattoo:remove:close', () => {
    if (global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;
    mp.players.local.freezePosition(false);
    global.gui.setData('mouseMove/setEnebled', false);
    global.gui.close();

    mp.players.local.clearDecorations();
    global.customCamera.switchOff(0);
    mp.events.callRemote("tattoo:remove:close");
})
