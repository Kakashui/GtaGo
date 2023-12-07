
let lastCheck = Date.now();
const floodTime = 1000;

mp.events.add("cef:access:requestData", () => {
    if(mp.storage.data.fastAccessButtons == undefined){
        mp.storage.data.fastAccessButtons = {"1": {type: "eq", id: 1}, "2": {type: "eq", id: 2}, "3": {type: "eq", id: 3}, "4": {type: "eq", id: 4}, "5": null, "6": null, "7": null, "8": null, "9": null, "10": null};
    }
    global.gui.setData('inventory/setFastAccessData', JSON.stringify(mp.storage.data.fastAccessButtons));
});

mp.events.add("cef:access:setButton", (key, button) => {
    mp.storage.data.fastAccessButtons[key] = JSON.parse(button);
    mp.storage.flush();
});

const keys = [global.Keys.Key_1, global.Keys.Key_2, global.Keys.Key_3, global.Keys.Key_4, global.Keys.Key_5, global.Keys.Key_6, global.Keys.Key_7, global.Keys.Key_8, global.Keys.Key_9, global.Keys.Key_0];

function canUseFastKey(){
    if (
        !global.loggedin || 
        mp.players.local.getVariable('InDeath') == true ||
        global.fishingMiniGame ||
        global.isPhoneOpened ||
        global.cuffed ||
        //global.cursorShow ||
        global.chatActive || 
        mp.players.local.isInAnyVehicle(true) ||
        lastCheck > Date.now() || 
        global.gui.isOpened() || 
        mp.gui.cursor.visible || 
        global.IsPlayingDM == true 
    ) return false;
    return true
}

function useFastKey(key){
    const button = mp.storage.data.fastAccessButtons[key];
    if(button == null) return;
    switch (button.type) {
        case "eq":
            if(global.playerEquip.weapons[button.id]){
                //global.setActiveWeapon(button.id || 0);
                let ammo = 0
                if(mp.players.local.currentWeaponData){
                    ammo = mp.players.local.currentWeaponData.ammo || 0;
                }
                lastCheck = Date.now() + floodTime;
                mp.events.callRemote("weapon:activate", button.id || 0, ammo)
            }
            break;
        case "inv":
            const index = global.playerInventory.items.findIndex(i=>i.id == button.id);
            if(index === -1) 
                global.gui.setData('inventory/resetFastAccessButton', `'${key}'`);
            else {                
                lastCheck = Date.now() + floodTime;
                mp.events.callRemote("inv:use:fast", button.id)
            }
            break;
            default:
            break;
    }
}

let lastMessage = 0;

keys.forEach((key, index) => {
    mp.keys.bind(key, false, function() {
        if(global.inAction || global.isPhoneOpened) return;
        if(canUseFastKey()){
            useFastKey(index + 1);
        }
    });
});
