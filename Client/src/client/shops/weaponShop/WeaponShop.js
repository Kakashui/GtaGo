const WeaponModel = require('../../WeaponSystem/WeaponModel');
const ItemModel = require('./ItemModel');
const moveSettings = require('./moveSettings');

module.exports = class WeaponShop{
    constructor() {
        this.model = null;
        this.slotName = "weapon:shop";
        this.trargetPosition = new mp.Vector3(251.994, -52.306, 71.151);
        this.cameraPosition = new mp.Vector3(252.787, -50.306, 71.151);
        this.heading = 160;
        this.baseData = [-1,-1,-1,-1,-1,-1, 0];
        this.camera = mp.cameras.new("default", this.cameraPosition, new mp.Vector3(), 40);
        this.camera.pointAtCoord(this.trargetPosition.x, this.trargetPosition.y, this.trargetPosition.z);
        this.shopOpened = false;
    }

    open(prices){
        if (this.shopOpened || gui.isOpened()) return;
        mp.game.cam.doScreenFadeOut(0);
        this.update(-1,-1,-1);
        global.gui.setData('weaponShop/setWeapons', JSON.stringify(prices));  
        setTimeout(()=>{
            this.shopOpened = true;
            this.camera.setActive(true);
            mp.game.cam.renderScriptCams(true, false, 0, true, false);
            mp.game.cam.renderScriptCams(true, false, 0, true, false); 
            mp.game.streaming.setFocusArea(this.trargetPosition.x, this.trargetPosition.y, this.trargetPosition.z, 0, 0, 0);
            mp.game.cam.doScreenFadeIn(300);  
            global.gui.openPage('WeaponShop');
            global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
            global.gui.setData('mouseMove/setEnebled', true);
        }, 500)
    }

    close(){
        if(!this.shopOpened) return;
        global.gui.setData('weaponShop/close');
        mp.game.cam.doScreenFadeOut(0);
        global.gui.close();
        mp.events.callRemote("wshop:close"); //<<<<<<<<<<<<<<<<<
        setTimeout(()=>{
            this.shopOpened = false;
            this.camera.setActive(false);
            mp.game.cam.renderScriptCams(false, false, 0, true, false);
            this.update(-1, -1, -1);
            mp.game.invoke (global.NATIVES.RESET_FOCUS_AREA); //<<<<<<<<<<<<<<<<<
            mp.game.cam.doScreenFadeIn(300);
            global.gui.setData('mouseMove/setEnebled', false);
        }, 500)
    }

    buy(count, payCard){
        if(this.model == null) return;
        var data = this.model.weaponData === undefined ? [] : this.model.weaponData.slice(1,7);
        mp.events.callRemote("wshop:buy:weapon", this.model.id, JSON.stringify(data), count, payCard);
    }

    moveX(val){
        if(this.model == null) return;
        this.model.rotateX = val;
        this.model.updateRotation();
    }
    moveY(val){
        if(this.model == null) return;
        this.model.rotateY = val;
        this.model.updateRotation();
    }

    moveZ(val){
        if(this.model == null) return;
        this.model.rotateZ = val;
        this.model.updateRotation();
    }

    update(id, slot, compIndex, model){
        if(id == -1){
            if(this.model !== null) this.model.destroy(false);            
            this.model = null;
        }else{
            if(this.model === null) {
                this.model = model == -1 ?
                    new WeaponModel(mp.players.local, this.slotName, [id, ...this.baseData], this.trargetPosition, this.heading) :
                    new ItemModel(mp.players.local, id, model, this.trargetPosition, this.heading)
            }else{
                if(this.model.id != id || slot == -1){
                    this.model.destroy(false);
                    this.model = model == -1 ? new WeaponModel(mp.players.local, this.slotName, [id, ...this.baseData],this.trargetPosition, this.heading) :
                    new ItemModel(mp.players.local, id, model, this.trargetPosition, this.heading);
                }
            }
    
            if(compIndex == -1)
                this.model.removeComponent(slot, compIndex);
            else
                this.model.setComponent(slot, compIndex);
        }        
        global.gui.setData('weaponShop/weaponLoaded'); 
    }
}