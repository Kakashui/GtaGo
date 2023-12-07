let opened = false;

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});

mp.events.add('carwash::exit', () => {
    CloseMenu();
});


function CloseMenu() {
    global.gui.close();
    opened = false;
}

mp.events.add('carwash::openMenu', (id, wash, wax, ceramic) => {
    if (global.checkIsAnyActivity()) return;
    opened = global.gui.openPage('CarWash');
    global.gui.setData('carWash/setData', JSON.stringify({id, wash, wax, ceramic}))
});


mp.events.add('carwash::startEffect', (position) => {
    try {
        StartEffect(new mp.Vector3(position.x, position.y, position.z + 2.5), 8, 'scr_carwash', 'ent_amb_car_wash_jet', 0, 180, 0)
        StartEffect(position, 6, 'scr_carwash', 'ent_amb_car_wash', 0, 0, 0)
    }
    catch (e) {
        mp.serverLog(`Error in carWash.startEffect: ${e.name }\n${e.message}\n${e.stack}`);
    }
});


function StartEffect(position, scale, asset, name, rotX, rotY, rotZ) {
    if(!mp.game.streaming.hasNamedPtfxAssetLoaded(asset))
        mp.game.streaming.requestNamedPtfxAsset(asset);    
    mp.game.graphics.setPtfxAssetNextCall(asset);   
    const fxHandle = mp.game.graphics.startParticleFxLoopedAtCoord(
        name, 
        position.x, position.y, position.z, //position
        rotX, rotY, rotZ, //rotate
        scale,
        false, false, false, false
    )
    setTimeout(()=>{
        mp.game.graphics.removeParticleFx(fxHandle, false);
    }, 10000)
}