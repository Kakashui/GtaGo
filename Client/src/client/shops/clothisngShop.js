const moveSettings = {
    size: {
        x: 35,
        y: 35
    },
    showIcons:[true, true, false, true],
    values:[
        {//LEFT X
            value: 1,
            min: -60,
            max: 30,
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
            value: 0,
            min: -1,
            max: 1,
            step: .05,
            invert: true,
            enabled: true,
            callback: "camMovePointZ"
        },
        { //WHEELE
            value: 2.5,
            min: .5,
            max: 3,
            step: .5,
            invert: false,
            enabled: true,
            callback: "camSetDist"
        }
    ]
}
    
function clearClothes() {
    const gender =  global.localplayer.getVariable("GENDER") ? 1 : 0;

    global.localplayer.clearProp(0);
    global.localplayer.clearProp(1);
    global.localplayer.clearProp(2);
    global.localplayer.clearProp(6);
    global.localplayer.clearProp(7);

    global.setClothing(global.localplayer, 1, global.clothesEmpty[gender][1], 0, 0);
    global.setClothing(global.localplayer, 3, global.clothesEmpty[gender][3], 0, 0);
    global.setClothing(global.localplayer, 4, global.clothesEmpty[gender][4], 0, 0);
    global.setClothing(global.localplayer, 5, global.clothesEmpty[gender][5], 0, 0);
    global.setClothing(global.localplayer, 6, global.clothesEmpty[gender][6], 0, 0);
    global.setClothing(global.localplayer, 7, global.clothesEmpty[gender][7], 0, 0);
    global.setClothing(global.localplayer, 8, global.clothesEmpty[gender][8], 0, 0);
    global.setClothing(global.localplayer, 9, global.clothesEmpty[gender][9], 0, 0);
    global.setClothing(global.localplayer, 10, global.clothesEmpty[gender][10], 0, 0);
    global.setClothing(global.localplayer, 11, global.clothesEmpty[gender][11], 0, 0);
}    

mp.events.add('openClothes', (price, pos) => {
    const gender = global.localplayer.getVariable("GENDER") ? 1 : 0;
    global.gui.setData('clothingStore/setData', JSON.stringify({gender, price}));
    if(!global.gui.openPage('ClothingStore')) return;    
    mp.game.cam.doScreenFadeOut(50);
    global.localplayer.freezePosition(true);
    clearClothes();
    global.customCamera.setPos(new mp.Vector3(pos.x, pos.y, pos.z) );
    global.customCamera.setPoint(new mp.Vector3(pos.x, pos.y, pos.z + .2));
    global.customCamera.moveCamZ(.4);
    global.customCamera.setDist(2.5);
    global.customCamera.moveAngleX(-1);
    global.customCamera.switchOn(0);
    global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
    global.gui.setData('mouseMove/setEnebled', true);
    mp.game.cam.doScreenFadeIn(1000);      
    //global.gui.playSound("editor_bg", .04, true);
})


function closeShop(){
    global.localplayer.freezePosition(false);
    global.gui.setData('mouseMove/setEnebled', false);
    global.gui.close();    
    global.customCamera.switchOff(0);
    mp.events.callRemote('cancelClothes');    
}

mp.events.add('componentVariation', (clotheId, variation, color) => {
    //mp.serverLog(`componentVariation: ${clotheId} - ${variation} - ${color}`);
    global.setClothing(global.localplayer, +clotheId, +variation, +color, 0);
});
mp.events.add('propVariation', (cloheId, vaiation, color) => {
    //mp.serverLog(`propVariation: ${clotheId} - ${variation} - ${color}`);
    global.setProp(global.localplayer, +cloheId, +vaiation, +color, true);
});
mp.events.add('buyClothes', (type, style, color) => {
    mp.events.callRemote('buyClothes', type, style, color);
})
mp.events.add('closeClothes', () => {
    if ( global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;
    closeShop();
})