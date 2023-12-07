
class EffectManager{
    constructor() {
        this.screenEffefcts = [];
        this.shotEffect = null;
        this.cameraShake = 0;
        this.nextShakeUpdate = Date.now();
        this.config = require('./configs/effects');
        mp.events.add("effect:add", this.addEffect.bind(this));
        mp.events.add("effect:remove", this.removeEffect.bind(this));
        mp.events.add('playerWeaponShot', this.onShot.bind(this));
        setInterval(this.check.bind(this), 1000);
    }

    check(){
        this.checkScreenEffect();
        //reportMenu:hotkeysthis.checkShotEffect();
        this.checkCameraEffect();
    }

    checkScreenEffect(){
        const forDelete = [];
        this.screenEffefcts.forEach(e=>{
            e.time--;
            if(e.time < 1)
                forDelete.push(e.name)
        })
        forDelete.forEach(name=>{                
            mp.game.graphics.stopScreenEffect(name);
            const index = this.screenEffefcts.findIndex(e=>e.name === name);
            if(index !== -1)
                this.screenEffefcts.splice(index, 1);
        })
    }
    checkShotEffect(){
        if(this.shotEffect === null || this.shotEffect.time < 0) return;
        this.shotEffect.time--;
        if(this.shotEffect.time < 1) 
            this.shotEffect = null;
    }
    checkCameraEffect(){
        if(this.cameraShake < 0) return;
        if(this.cameraShake > 0){
            const time = Math.max(.25, Math.min(5.0, this.cameraShake * .01));
            mp.game.cam.setGameplayCamShakeAmplitude(time)
            this.cameraShake--;       
        }else{
            mp.game.cam.stopGameplayCamShaking(true);
            this.cameraShake = -1;
        }            
    }
    addCameraEffect(time){
        this.cameraShake = time;
        mp.game.cam.shakeGameplayCam("DRUNK_SHAKE", this.cameraShake * .01);      
    }
    addShotEffect(effect, time){
        this.shotEffect = {name:effect.name, asset: effect.asset, scale: effect.scale, time};
    }
    addScreenEffect(effect, time){
        const exists = this.screenEffefcts.find(e=>e.name === effect.name);
        if(time < 0) time = 60*60*24;
        if(exists)
            exists.time = time;
        else{
            this.screenEffefcts.push({name:effect.name, time});
            mp.game.graphics.startScreenEffect(effect.name, 1500, true);
        }
            
    }
    addEffect(id, time){
        const effect = this.config[id];
        if(!effect) return;

        switch (effect.type) {
            case 0:
                this.addCameraEffect(time)
                break;
            case 1:
                this.addScreenEffect(effect, time)
                break;            
            case 2:
                this.addShotEffect(effect, time)            
                break;        
            default:
                break;
        }
    }
    removeEffect(id){
        const effect = this.config[id];
        if(!effect) return;
        switch (effect.type) {
            case 0:
                this.cameraShake = 0;
                break;
            case 1:          
            case 2:
                const exists = this.screenEffefcts.find(e=>e.name === effect.name);
                if(exists)
                    exists.time = 0        
                break;        
            default:
                break;
        }
    }

    onShot(pos, entity){
        if(!entity) return;
        if(global.IsPlayingDM){
            mp.events.call('particles:playAtPosition', pos, "core", "ent_sht_telegraph_pole", 0.6, 1000);
        }else{
            if (this.shotEffect === null) return;            
            mp.events.call('particles:playAtPosition', pos, this.shotEffect.asset, this.shotEffect.name, this.shotEffect.scale, 1000);
        }
    }
}

module.exports = new EffectManager();