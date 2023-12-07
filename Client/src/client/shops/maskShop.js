const moveSettings = {
    size: {
        x: 40,
        y: 40
    },
    showIcons:[true, false, false, false],
    values:[
        {//LEFT X
            value: 160,
            min: 110,
            max: 250,
            step: 5,
            invert: true,
            enabled: true,
            callback: "camMoveAngleX"
        },
        {//LEFT Y
            value: 0,
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
            value: 0,
            min: -1,
            max: 1,
            step: .05,
            invert: true,
            enabled: false,
            callback: "camMovePointZ"
        },
        { //WHEELE
            value: 1.5,
            min: .5,
            max: 3,
            step: .5,
            invert: false,
            enabled: true,
            callback: "camSetDist"
        }
    ]
}

let opened = false;

mp.events.add('openMasks', (price) => {
    global.gui.setData("maskShop/setPrice", price)
    if(!global.gui.openPage("MaskShop")) return;
    opened = true;
    global.localplayer.setRotation(0,0,70,2,true);
    global.localplayer.freezePosition(true);
    global.setClothing(global.localplayer, 1, 1, 0, 0);

    global.localplayer.clearProp(0);
    global.localplayer.clearProp(1);

    const pos = global.localplayer.position;
    global.customCamera.setPos(new mp.Vector3(pos.x, pos.y, pos.z + .6) );
    global.customCamera.setPoint(new mp.Vector3(pos.x, pos.y, pos.z + .6));
    global.customCamera.moveCamZ(0);
    global.customCamera.setDist(1.5);
    global.customCamera.moveAngleX(160);
    global.customCamera.switchOn(0);
    global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
    global.gui.setData('mouseMove/setEnebled', true);
})

mp.events.add('buyMasks', (variation, color) => {
    if ( global.lastCheck > Date.now()) return;
    global.lastCheck = new Date() + 500;
    //closeMaskShop()
    mp.events.callRemote('buyMasks', variation, color);
})

mp.events.add('closeMasks', () => {
    if ( global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;
    closeMaskShop()
})

mp.events.add('setMask', (variation, color) => {
    global.setClothing(global.localplayer, 1, variation, color, 0);
})

mp.keys.bind(global.Keys.Key_ESCAPE, false, ()=> {
    if(opened)
        closeMaskShop();
})

function closeMaskShop(){     
    global.localplayer.freezePosition(false);
    global.gui.setData('mouseMove/setEnebled', false);
    global.gui.close();   
    opened = false; 
    global.customCamera.switchOff(0);
    mp.events.callRemote('cancelMasks');  
}