const { getZoneNameByPosition, getStreetNameByPosition } = require('./zonesNames');
const DESTROY_MOBILE_PHONE = '0x3BC861DF703E5097';

let sendingExcept = false;

global.isPhoneOpened = false;

function openPhone(withCursor = true) {
    if (!mp.phone.fuckingPedCanOpenPhone(true))
        return;
    mp.phone.setTexting();
    global.isPhoneOpened = global.gui.openPhone(true, withCursor);
}

function closePhone() {
    if(mp.players.local.isInAir()) return;
    if(global.malboro || !global.isPhoneOpened) return;
    
    mp.phone.takeDown();
    global.gui.openPhone(false, false);
    global.isPhoneOpened = false;
}

global.openGoPhone = openPhone;
global.closeGoPhone = closePhone;

mp.keys.bind(global.Keys.Key_UP, false, () => {
    if (mp.players.local.getVariable('InDeath') == true) {
        return;
    }
    
    if (!global.isPhoneOpened && !gui.isOpened()) {
        openPhone();
    }
});

mp.keys.bind(global.Keys.Key_DOWN, false, () => {
    closePhone();
});

mp.events.add('phone::hide', () => {
    closePhone();
});

mp.events.addDataHandler("InDeath", (entity, isDeath) => {
    if (entity === mp.players.local && isDeath == true)
        closePhone();
});

let geopositionUpdateTimer = null;
mp.phone.onStart(() => {
    geopositionUpdateTimer = setInterval(() => {
        try {
            if (!global.loggedin)
                return;
            
            const pos = mp.players.local.isInAnyVehicle(false) ? mp.players.local.vehicle.position : mp.players.local.position;

            const geolocation = {
                currentPosition: {
                    X: pos.x,
                    Y: pos.y,
                    Z: pos.z
                },

                zoneName: getZoneNameByPosition(pos),
                streetName: getStreetNameByPosition(pos)
            }
                
            global.gui.setData('smartphone/setCurrentPosition', JSON.stringify(geolocation))
        }
        catch (e){ 
            if(global.sendException && !sendingExcept) {
                sendingExcept = true;
                mp.serverLog(`mp.phone.onStart geopositionUpdateTimer: ${e.name}\n${e.message}\n${e.stack}`);
            }
        }
    }, 5000);

});

mp.phone.open = openPhone;
mp.phone.close = closePhone;


mp.phone.fuckingPedCanOpenPhone = (notify) => {
    if(
        mp.game.ui.isPauseMenuActive() ||
        mp.players.local.isInAir() ||
        global.malboro || 
        global.isPhoneOpened || 
        global.inAction || 
        global.fishingMiniGame ||
        global.cuffed
    ) return false;
    if(global.getCurrentPlayerWeapon() !== 2725352035)
    {
        if (notify)
            mp.events.call('notify', 4, 9, 'act:canc:w', 3000);
        return false;
    }
    return true;
}