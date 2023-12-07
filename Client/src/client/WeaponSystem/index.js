require('./currentWeaponHandler')
const WeaponModel = require('./WeaponModel');
const WeaponData = require('./WeaponData');
const weaponSlots = ["attach:weapon:1", "attach:weapon:2", "attach:weapon:3", "attach:weapon:4"/*, "weapon:current"*/];


function attachTo(player, slot, weapon){
    if(player.attachedWeapons.hasOwnProperty(slot)){
        player.attachedWeapons[slot].destroy();
        delete player.attachedWeapons[slot];
    }
    player.attachedWeapons[slot] = new WeaponModel(player, slot, weapon);
    mp.game.wait(0);
    player.attachedWeapons[slot].attach();
}

function detachSlot(player, slot){
    if(!player.attachedWeapons.hasOwnProperty(slot)) return;
    player.attachedWeapons[slot].destroy();
    delete player.attachedWeapons[slot];
}

function checkSlotExists(player){
    if(!player.hasOwnProperty("attachedWeapons")){
        player.attachedWeapons = {};
    } 
    if(!player.hasOwnProperty("this.player.weaponComponents")){
        player.weaponComponents = {};
    }
}

function getCurrentWeaponModel(){
    return mp.players.local.attachedWeapons["weapon:current"];    
}

mp.events.add('weapon:hideCurrentWeapon', hideCurrentWeapon);
mp.events.add('weapon:showCurrentWeapon', showCurrentWeapon);
mp.events.add('weapon:detachAll', () => detachAll(global.localplayer));
mp.events.add('weapon:updateAttach', () => updateAllAttach(global.localplayer));

function hideCurrentWeapon(player){
    if (player.type !== 'player' || !player.handle) return;
    detachSlot(player, "weapon:current")
}

function showCurrentWeapon(player){
    if (player.type !== 'player' || !player.handle) return;
    const data = player.getVariable("weapon:current");
    if(data)
        attachTo(player, "weapon:current", data);
}

function detachAll(player){
    weaponSlots.forEach(slot => {
        detachSlot(player, slot);
    });
}

function updateAllAttach(player){
    detachAll(player);
    weaponSlots.forEach(slot => {
        let data = player.getVariable(slot);
        if(data && data.length > 0){
            attachTo(player, slot, data);
        }
    });
}


function SetDamageModifier(){
    mp.game.player.setWeaponDefenseModifier(1.01);
    mp.game.player.setVehicleDefenseModifier(0.01);
}

weaponSlots.forEach(slot => {
    mp.events.addDataHandler(slot, (player, value)=>{
        try {
            if (player.type !== 'player' || !player.handle) return;
            checkSlotExists(player);
            if(value)
                attachTo(player, slot, value);
            else
                detachSlot(player, slot);
        } catch (e) {
            if(global.sendException) mp.serverLog(`weapon.addDataHandler.${slot}: ${e.name}\n${e.message}\n${e.stack}`);
        }
    });
});

mp.events.add("entityStreamIn", (player)=>{
    try {        
        if (!player || player.type !== 'player') return;
        checkSlotExists(player);
        updateAllAttach(player);
    } catch (e) {
        if(global.sendException) mp.serverLog(`weapon.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);        
    }
})

mp.events.add("entityStreamOut", (player)=>{
    try {       
        if (!player || player.type !== 'player') return;
        checkSlotExists(player);
        detachAll(player);
    } catch (e) {
        if(global.sendException) mp.serverLog(`weapon.entityStreamOut: ${e.name}\n${e.message}\n${e.stack}`);        
    }
})

mp.events.add("playerDeath", function(player, reason, killer) {
    try {        
        if (player.type !== 'player' || player.handle == 0) return;
        checkSlotExists(player);
        detachAll(player);
    } catch (e) {
        if(global.sendException) mp.serverLog(`weapon.playerDeath: ${e.name}\n${e.message}\n${e.stack}`);           
    }
});

mp.events.add("playerSpawn", (player)=>{
    try {        
        if (player.type !== 'player' || player.handle == 0) return;
        checkSlotExists(player);
        updateAllAttach(player);
        if(player === mp.players.local) {
            mp.game.wait(0);
            SetDamageModifier();
        }
    } catch (e) {
        if(global.sendException) mp.serverLog(`weapon.playerSpawn: ${e.name}\n${e.message}\n${e.stack}`);          
    }
});

mp.events.add("playerQuit", (player, exitType, reason) => {
    try {
        if (player.type !== 'player') return;
        checkSlotExists(player);
        detachAll(player);
    } catch (e) {
        if(global.sendException) mp.serverLog(`weapon.playerQuit: ${e.name}\n${e.message}\n${e.stack}`);          
    }
});

mp.events.add('common:setWeaponsDefenseModifier', () => {
    SetDamageModifier();
});



mp.events.add('render', () => {
    mp.game.controls.disableControlAction(2, 45, true); // reload control
    mp.game.controls.disableControlAction(1, 243, true); // CCPanelDisable

    mp.game.controls.disableControlAction(2, 12, true);
    mp.game.controls.disableControlAction(2, 13, true);
    mp.game.controls.disableControlAction(2, 14, true);
    mp.game.controls.disableControlAction(2, 15, true);
    mp.game.controls.disableControlAction(2, 16, true);
    mp.game.controls.disableControlAction(2, 17, true);

    mp.game.controls.disableControlAction(2, 36, true);
    mp.game.controls.disableControlAction(2, 37, true);
    mp.game.controls.disableControlAction(2, 99, true);
    mp.game.controls.disableControlAction(2, 100, true);

    mp.game.controls.disableControlAction(2, 157, true);
    mp.game.controls.disableControlAction(2, 158, true);
    mp.game.controls.disableControlAction(2, 159, true);
    mp.game.controls.disableControlAction(2, 160, true);
    mp.game.controls.disableControlAction(2, 161, true);
    mp.game.controls.disableControlAction(2, 162, true);
    mp.game.controls.disableControlAction(2, 163, true);
    mp.game.controls.disableControlAction(2, 164, true);
    mp.game.controls.disableControlAction(2, 165, true);

    mp.game.controls.disableControlAction(2, 261, true);
    mp.game.controls.disableControlAction(2, 262, true);

    //global.debugText = `1: ${mp.players.local.isDucking()}`;

    if (mp.players.local.currentWeaponData) { // heavy attack controls
        mp.game.controls.disableControlAction(2, 140, true);
        mp.game.controls.disableControlAction(2, 141, true);
        mp.game.controls.disableControlAction(2, 142, true);
        mp.game.controls.disableControlAction(2, 143, true);
        mp.game.controls.disableControlAction(2, 263, true);
        mp.game.controls.disableControlAction(2, 264, true);
    }
    if(global.inAction || global.isPhoneOpened || mp.keys.isDown(global.Keys.Key_R)){
        mp.game.controls.disableControlAction(0, 24, true);
        mp.game.controls.disableControlAction(0, 69, true);
        mp.game.controls.disableControlAction(0, 257, true);
    }
});



global.getCurrentPlayerWeapon = () => {
    return mp.game.invoke(global.NATIVES.GET_SELECTED_PED_WEAPON, mp.players.local.handle) >>> 0;
};

checkSlotExists(mp.players.local);
SetDamageModifier();