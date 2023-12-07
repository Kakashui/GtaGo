require('./adminesp');
require('./vehicleConfiguration');
require('./spectate');
require('./fly');
require('./fun');

mp.events.add("admin:checkinventory:responce", (equip, items)=>{
    global.gui.setData("inventory/updateCheckInventory", JSON.stringify({equip, items}))
    global.showCursor(true);
})

global.isDemorgan = false;

mp.events.add("admin:toDemorgan", (freeze)=>{ 
    global.isDemorgan = true;
    if (freeze)  
    {
        global.gui.setData("setLoadScreen", 'true');
        mp.players.local.freezePosition(true);
        setTimeout(() => {
            mp.players.local.freezePosition(false);
            global.gui.setData("setLoadScreen", 'false');
        }, 3000);
    } 
})

mp.events.add("admin:releaseDemorgan", ()=>{ 
    global.isDemorgan = false;
})

let spamProtection = 0;

mp.keys.bind(global.Keys.Key_F10, false, ()=>{
    if(spamProtection > Date.now()) return;
    spamProtection = Date.now() + 1000;
    if(global.getVariable(mp.players.local, 'IS_MEDIA', false) || global.getVariable(mp.players.local, 'ALVL', 0) > 7)
        mp.events.callRemote("media:mute:press")
});