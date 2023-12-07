const waeaponConfigs = require('./weaponConfigs'); 
const attachInfos = require('./weaponSlotsConfig');



module.exports = class WeaponModel{
    constructor(player, slot, weaponData, position = undefined, heading = undefined){
        /*
            weaponData имеет 9 элементов
            0: id оружия
            1-7: инвексы установленых компонентов для каждого из слотов
            8: количество патронов 
        */
        this.id = weaponData[0];
        this.config = waeaponConfigs[this.id];
        if(!this.config) {
            mp.serverLog(`WeaponModel no config for weapon ${this.id}`);
            return;
        }   
        this.weaponHash = this.config.Hash;
        this.ammoType = this.config.AmmoType;
        this.lastReport = 0;
        this.player = player;   
        this.weaponData = weaponData;
        this.position = position;
        this.slot = slot;
        this.rotateX = 0;
        this.rotateY = 0;
        this.ammo = this.weaponData[7] || 0;
        this.rotateZ = heading;
        this.createModel();        
    }

    createModel(){
        const skin = this.weaponData[6] < 0 ? undefined : this.config.Components[6][this.weaponData[6]];            
        if(!mp.game.weapon.hasWeaponAssetLoaded(this.weaponHash)){
            mp.game.weapon.requestWeaponAsset(this.weaponHash, 31, 0);
            for (let index = 0; !mp.game.weapon.hasWeaponAssetLoaded(this.weaponHash) && index < 250 ; index++) {
                mp.game.wait(0);
            }                    
        }
    
        const pos = this.position === undefined ? new mp.Vector3(this.player.position.x, this.player.position.y, this.player.position.z): this.position;
        this.weapon = mp.game.weapon.createWeaponObject(this.weaponHash, 0 , pos.x, pos.y, pos.z, true, 0, skin == undefined ? 0 : this.getSkin(skin));
        if(this.rotateZ !== undefined) this.updateRotation();
        for (let index = 0; this.weapon === 0 && index < 250 ; index++) {
            mp.game.wait(0);
        }
        if(this.weapon == 0) return false;
        for (let index = 1; index < 6; index++) {
            this.setComponent(index, this.weaponData[index]);
        }
        return true;
    }

    attach(){
        const attachInfo = attachInfos[this.slot];
        global.customWeaponsModels.push(this.weapon);
        if(attachInfo == undefined) return;
        mp.game.invoke(global.NATIVES.ATTACH_ENTITY_TO_ENTITY, this.weapon, this.player.handle, +attachInfo.bone, 
            +attachInfo.offset.x, +attachInfo.offset.y, +attachInfo.offset.z, 
            +attachInfo.rotate.x,  +attachInfo.rotate.y,  +attachInfo.rotate.z,
             true, false, false, false, 0, true
        );
    }

    destroy(){       
        mp.game.object.deleteObject(this.weapon);
        const index = global.customWeaponsModels.findIndex(w=>w == this.weapon);
        if(index !== -1){
            global.customWeaponsModels.splice(index, 1);
        }
    }

    requestComponent(hash){
        const model = mp.game.weapon.getWeaponComponentTypeModel(+hash);
        if(model === 0 || !mp.game.streaming.isModelValid(model)) return;
        if(!mp.game.streaming.hasModelLoaded(model)){
            mp.game.streaming.requestModel2(model); 
            for (let index = 0; !mp.game.streaming.hasModelLoaded(model) && index < 250; index++) {
                mp.game.wait(0);
            }
        }
    }

    getSkin(skin){
        const model = mp.game.weapon.getWeaponComponentTypeModel(+skin);
        if(model === 0 || !mp.game.streaming.isModelValid(model)) return 0;
        if(!mp.game.streaming.hasModelLoaded(model)){
            mp.game.streaming.requestModel2(model); 
            for (let index = 0; !mp.game.streaming.hasModelLoaded(model) && index < 250; index++) {
                mp.game.wait(0);
            }
        }      
        return model;
    }
   
   
    updateRotation(){
        mp.game.invoke(global.NATIVES.SET_ENTITY_ROTATION, this.weapon, +this.rotateX + .01, +this.rotateY + .01, +this.rotateZ + .01, 1, true)
    }

    removeComponent(slot){
        if(
            slot < 1 || 
            this.weaponData[slot] == undefined ||
            this.config.Components[slot] == undefined ||
            this.config.Components[slot][this.weaponData[slot]] == undefined
        ) return;
        if(slot == 6){
            mp.game.object.deleteObject(this.weapon);
            this.weaponData[slot] = -1;
            this.createModel();
        }else{
            const element = this.config.Components[slot][this.weaponData[slot]];
            mp.game.weapon.removeWeaponComponentFromWeaponObject(this.weapon, +element);
            this.weaponData[slot] = -1;
        }
    }

    setComponent(type, index){
        if(
            type < 1 || 
            this.weaponData[type] == undefined ||
            this.config.Components[type] == undefined ||
            this.config.Components[type][index] == undefined
        ) return;
        this.weaponData[type] = index;
        if(type == 6){
            mp.game.object.deleteObject(this.weapon);
            this.createModel();
        }else{
            const element = this.config.Components[type][index];
            this.requestComponent(element)
            mp.game.weapon.giveWeaponComponentToWeaponObject(this.weapon, +element);
        }
    }
}
//3l6NZhpImg1jdk5h
//lglxUS7L1Rm741s1