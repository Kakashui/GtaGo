const waeaponConfigs = require('./weaponConfigs');
const WeaponData = require('./WeaponData');
let waitWeapon = Date.now();
let spamProtection = 0;
let spamProtectionTime = 1000;
let dmgCoefs = [1.3, .6, .5]; // 0 - head, 1 - arms/legs, 2-melee
let damageConfig = {};
function isSpam(){
    if(spamProtection > Date.now()) return true;
    spamProtection = Date.now() + spamProtectionTime;
}

function updateAmmoInHud(){
    global.gui.setData('hud/updateData', JSON.stringify({name: 'ammo', value: mp.players.local.currentWeaponData ? mp.players.local.currentWeaponData.ammo : 0 }));
}

async function CurrentWeaponDataHandler(player, newValue, oldValue){
    if (!player || player.type !== 'player') return;
    if(newValue){
        const data = new WeaponData(newValue);
        waitWeapon = Date.now() + 2000;
        while (player.weapon !== data.weaponHash &&  waitWeapon > Date.now()) {
            await mp.game.waitAsync(0);
        }
        updateWeaponComponents(player, data);
    }else{
        removeAllWeaponComponents(player);
    }

    if(player === mp.players.local){
        if(mp.players.local.currentWeaponData){
            while (mp.players.local.currentWeaponData && mp.players.local.getAmmoInClip(mp.players.local.currentWeaponData.weaponHash) !== mp.players.local.currentWeaponData.ammo &&  waitWeapon > Date.now()) {
                mp.players.local.setAmmoInClip(mp.players.local.currentWeaponData.weaponHash, mp.players.local.currentWeaponData.ammo);
                await mp.game.waitAsync(0);
            }
        }
        updateAmmoInHud();
    }
}

function updateWeaponComponents(player, newData) {
    if(!player.currentWeaponData){
        setWeaponComponents(player, newData);
    }else{
        if(newData.weaponHash !== player.currentWeaponData.weaponHash){ 
            removeAllWeaponComponents(player);
            setWeaponComponents(player, newData);
        }else{
            for (let index = 0; index < player.currentWeaponData.components.length; index++) {
                const newComponent = newData.components[index];
                const oldComponent = player.currentWeaponData.components[index];
                if(newComponent === oldComponent) continue;
                if(oldComponent !== 0)
                    mp.game.invoke(global.NATIVES.REMOVE_WEAPON_COMPONENT_FROM_PED, player.handle, player.currentWeaponData.weaponHash >> 0, oldComponent >> 0)
    
                if(newComponent !== 0)
                    mp.game.invoke(global.NATIVES.GIVE_WEAPON_COMPONENT_TO_PED, player.handle, newData.weaponHash >> 0, newComponent >> 0);
            }
            player.currentWeaponData = newData;            
        }        
    }
}

function setWeaponComponents(player, data) {
    if(data !== undefined){
        player.currentWeaponData = data;
    }
    if(player.currentWeaponData){
        player.currentWeaponData.components.forEach(component=>{
            if(component !== 0)
                mp.game.invoke(global.NATIVES.GIVE_WEAPON_COMPONENT_TO_PED, player.handle, player.currentWeaponData.weaponHash >> 0, component >> 0);
        });  
    }          
}

function removeAllWeaponComponents(player) {
    if(!player.currentWeaponData) return;
    player.currentWeaponData.components.forEach(component => {
        if(component !== 0)
            mp.game.invoke(global.NATIVES.REMOVE_WEAPON_COMPONENT_FROM_PED, player.handle, player.currentWeaponData.weaponHash >> 0, component >> 0);
    });
    if(player === mp.players.local)
        mp.players.local.setAmmoInClip(mp.players.local.currentWeaponData.weaponHash, 0);
    player.currentWeaponData = undefined;
}

async function reloadWeapon(ammo) {
    if(!mp.players.local.currentWeaponData) return;
    mp.players.local.currentWeaponData.ammo = +ammo;
    updateAmmoInHud();
    mp.players.local.setAmmoInClip(mp.players.local.currentWeaponData.weaponHash, 0);
    mp.game.invoke(global.NATIVES.SET_CURRENT_PED_WEAPON, mp.players.local.handle, mp.game.joaat('weapon_unarmed') >> 0, true);
    await mp.game.waitAsync(0);
    mp.game.invoke(global.NATIVES.SET_PED_AMMO, mp.players.local.handle, mp.players.local.currentWeaponData.weaponHash >> 0, mp.players.local.currentWeaponData.ammo, false);
    await mp.game.waitAsync(0);
    mp.game.invoke(global.NATIVES.SET_CURRENT_PED_WEAPON, mp.players.local.handle, mp.players.local.currentWeaponData.weaponHash >> 0, true);
    //global.inAction = false;
}

async function cancelSwapWeapon(){
    if(mp.players.local.weapon !== mp.players.local.currentWeaponData.weaponHash)
        mp.players.local.giveWeapon(mp.players.local.currentWeaponData.weaponHash, 0, true);
    mp.players.local.taskSwapWeapon(false);
    setTimeout(requestReloadWeapon, 500);
}

async function onWeaponShot(targetPosition, targetEntity){
    if (global.IsPlayingDM) return;
    if (mp.players.local.currentWeaponData) {
        mp.players.local.currentWeaponData.ammo--;
        updateAmmoInHud();
        const ammo =  mp.game.invoke(global.NATIVES.GET_AMMO_IN_PED_WEAPON, mp.players.local.handle, mp.players.local.currentWeaponData.weaponHash >> 0);
        //.logInfo(`check ${mp.players.local.weapon}: ${ammo} / ${mp.players.local.currentWeaponData.ammo} `)
        if(ammo === 0){
            if(mp.players.local.currentWeaponData.ammo > 0)
                mp.players.local.currentWeaponData.ammo = 0;
            await cancelSwapWeapon();
        }else if(mp.players.local.currentWeaponData.ammo < 1){
            mp.players.local.setAmmoInClip(mp.players.local.currentWeaponData.weaponHash, 0);
            await cancelSwapWeapon();
        }else{
            if(ammo !== mp.players.local.currentWeaponData.ammo)               
                mp.players.local.setAmmoInClip(mp.players.local.currentWeaponData.weaponHash, mp.players.local.currentWeaponData.ammo);
        }         
    }
}

function requestReloadWeapon(){
    if(isSpam()) return;
    if(!global.loggedin || 
        mp.players.local.getVariable('InDeath') == true ||
        global.inAction ||
        global.isPhoneOpened ||
        global.chatActive || 
        global.gui.isOpened() || 
        mp.gui.cursor.visible || 
        global.IsPlayingDM == true ||
        !mp.players.local.currentWeaponData || 
        mp.players.local.currentWeaponData.weaponHash !== mp.players.local.weapon
    ) return;
    if(global.playerInventory.items.find(i=>i.id == mp.players.local.currentWeaponData.ammoType) === -1) return;
    //global.inAction = true;
    mp.events.callRemote("weapon:reload", mp.players.local.currentWeaponData.ammo || 0);
}

async function entityStreamIn(player){  
    if (!player || player.type !== 'player') return;
    if (player.currentWeaponData){
        waitWeapon = Date.now() + 2000;
        while (player.currentWeaponData && player.weapon !== player.currentWeaponData.weaponHash &&  waitWeapon > Date.now()) {
            await mp.game.waitAsync(10);
        }
        setWeaponComponents(player);
    }
}

function onOutgoingDamage(sourceEntity, targetEntity, sourcePlayer, weapon, boneIndex, damage){
    //console.logInfo(`outDamage ${weapon}:${damage}(${sourceEntity.type}/${targetEntity.type}/${sourcePlayer.type}/${boneIndex})`);
    //if(targetEntity.type === "vehicle") return false;
    if(global.IsPlayingDM || mp.players.local.weapon === 2725352035) return false;
    if(!mp.players.local.currentWeaponData || mp.players.local.currentWeaponData.weaponHash !== mp.players.local.weapon){
        if(global.getVariable(mp.players.local, 'ALVL', 0) > 0) return;
        mp.events.callRemote("weapon:cheat", `${mp.players.local.currentWeaponData ? mp.players.local.currentWeaponData.weaponHash : 0}`, `${mp.players.local.weapon}`);
        return true;
    }else return false;
    
}

function onIncomingDamage(sourceEntity, targetEntity, sourcePlayer, weapon, boneIndex, damage){
    //console.logInfo(`inDamage ${weapon}:${damage}(${sourceEntity.type}/${targetEntity.type}/${sourcePlayer.type}/${boneIndex})`);
    if(global.pidrgm === true) return true;
    if(sourcePlayer.type === "vehicle") return false;
    if(sourcePlayer.type === "player"){
        if (global.inGreenZone)
        {
            if (sourceEntity.weapon === 911657153)
                return false;
            return true;
        }
        let coef = 1;
        if(sourceEntity.currentWeaponData || global.IsPlayingDM){
            switch (boneIndex) {
                case 20:
                    coef = dmgCoefs[1];
                    break;
                case 2:
                case 6:
                case 12:
                case 14:
                case 16:
                case 18:
                    coef = dmgCoefs[2];
                    break;
                default:
                    break;
            }
            damage = parseInt((damageConfig[weapon] || 10) * coef);
            if(mp.players.local.getArmour() + mp.players.local.getHealth() <= damage) 
                return false;
            else
                mp.players.local.applyDamageTo(damage, true);
        } else if(sourceEntity.weapon === 2725352035){
            coef = dmgCoefs[3];
            damage = parseInt((damage || 10) * coef);
            if(mp.players.local.getArmour() + mp.players.local.getHealth() <= damage) 
                return false;
            else
                mp.players.local.applyDamageTo(damage, true);
        };
    }
    if(global.sendException)
        console.logInfo(`damage: ${damage}`);
    return true;
}


function updateDamageConfig(config) {
    damageConfig = config;   
    global.gui.setData("weaponsSetting/updateWeaponConfig", JSON.stringify(damageConfig))
    //console.logInfo(`${JSON.stringify(damageConfig)}`);
}

function updateDamageCoef(coef) {
    dmgCoefs = coef;
    global.gui.setData("weaponsSetting/updateWeaponCoefs", JSON.stringify(dmgCoefs))
    //console.logInfo(`${JSON.stringify(dmgCoefs)}`);
}

let menuOpened = false;

function openDamageSettingsMenu() {    
    if(global.gui.isOpened()) return;
    menuOpened = global.gui.openPage("WeaponsSetting");
}

function closeMenu() {
    if (menuOpened) {
        mp.game.ui.setPauseMenuActive(false);
        global.gui.close();
        menuOpened = false;
    }
}


mp.events.addDataHandler("weapon:current", CurrentWeaponDataHandler);

mp.events.add("weapon:reload", reloadWeapon);
mp.events.add("playerWeaponShot", onWeaponShot);
mp.events.add("entityStreamIn", entityStreamIn);
mp.events.add('outgoingDamage', onOutgoingDamage);
mp.events.add('incomingDamage', onIncomingDamage);
mp.events.add('weapon:damage:config:update', updateDamageConfig);
mp.events.add('weapon:damage:coef:update', updateDamageCoef);
mp.events.add('weapon:settings:open', openDamageSettingsMenu);

mp.keys.bind(global.Keys.Key_R, false, requestReloadWeapon);
mp.keys.bind(global.Keys.Key_ESCAPE, false, closeMenu);
