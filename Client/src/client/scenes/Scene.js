const configs = require('../configs/scenes')

const states = {
    "enter": 0,
    "base": 1,
    "action": 2,
    "exit": 3,
    "sequence": 4,
    "completed": 5,
    "loop": 6
}

module.exports = class Scene{
    constructor(player, id) {
        try {
            this.id = id;
            const config = configs[id];
            this.player = player;
            
            if(this.player.remoteId === mp.players.local.remoteId)
                global.inAction = true;

            this.isLooped = config.isLooped;
            this.sequence = config.sequence;
            this.sequenceStage = 0;

            this.attachs = config.attachs;
            this.props = [];
            this.canCancel = config.canCancel;

            this.enter = config.enter;
            this.base = config.base;
            this.exit = config.exit;
            this.action = config.action;
            
            this.enterEffects = config.enterEffects;
            this.actionEffects = config.actionEffects;

            this.state = this.sequence === null ? states.enter : states.sequence;
            
            this.pos = this.player.position;
            this.rot = this.player.getRotation(2);
            this.initAttachs();
            this.initEffects();
            this.initAnims();
            if(this.state === states.sequence)
                this.initSequence();
            else
                this.initActionAnim();
        } catch (e) {
            if(global.sendException) mp.serverLog(`Scene.ctor: ${e.name}\n${e.message}\n${e.stack}`);
        }
    } 
    
    checkEffects(time){
        const effects = this.state === states.enter ? this.enterEffects : this.actionEffects;
        if(effects !== null){
            effects.forEach(effect => {
                if(!effect.used && effect.startAt < time){
                    this.callEffect(effect);
                }
            });
        }
    }

    callEffect(effect){
        effect.used = true;
        const boneIndex = mp.players.local.getBoneIndex(effect.boneId);
        mp.game.graphics.setPtfxAssetNextCall(effect.asset);   
        const fxHandle = mp.game.graphics.startParticleFxLoopedOnEntityBone(
            effect.name, this.player.handle, 
            effect.offset.x, effect.offset.y,  effect.offset.z, 
            effect.rotate.x, effect.rotate.y,  effect.rotate.z,
            boneIndex, effect.scale,
            false, false,false
        )
        setTimeout(()=>{
            mp.game.graphics.removeParticleFx(fxHandle, false);
        }, effect.duration)
    }

    resetEffects(){
        const effects = this.state === states.enter ? this.enterEffects : this.actionEffects;
        if(effects !== null){
            effects.forEach(effect => {
                effect.used = false;
            });
        }        
    }
    initAnims(){
        if(this.sequence !== null){
            this.sequence.forEach(anim => {
                this.loadAnimDict(anim.dictionary);
            });
        }

        if(this.enter !== null)
            this.loadAnimDict(this.enter.dictionary);

        if(this.base !== null)
            this.loadAnimDict(this.base.dictionary);

        if(this.action !== null)
            this.loadAnimDict(this.action.dictionary);
    
        if(this.exit !== null)
            this.loadAnimDict(this.exit.dictionary);    
    }
    
    initSequence(){       
        const anim = this.currentSequenceAnim();
        this.playAnim(anim);
    }

    initActionAnim(){
       
        if(this.enter === null) 
            this.state = states.base;

        const anim = this.state === states.base ? this.base : this.enter;
        this.playAnim(anim);       
    }

    initEffects(){
        try {
            if(this.actionEffects !== null){
                this.actionEffects.forEach(effect => {
                    if(!mp.game.streaming.hasNamedPtfxAssetLoaded(effect.asset))
                        mp.game.streaming.requestNamedPtfxAsset(effect.asset);         
                });
            }
            if(this.enterEffects !== null){
                this.enterEffects.forEach(effect => {
                    if(!mp.game.streaming.hasNamedPtfxAssetLoaded(effect.asset))
                        mp.game.streaming.requestNamedPtfxAsset(effect.asset);         
                });
            }            
        } catch (e) {
            if(global.sendException) mp.serverLog(`sceneUpdate: ${e.name}\n${e.message}\n${e.stack}`);
        }
    }

    initAttachs(){
        if(this.attachs !== null){
            this.attachs.forEach(prop => {
                this.doAttach(prop)
            });
        }
    }   

    async doAttach(prop){
        const hash = mp.game.joaat(prop.model);
        this.loadProp(hash);
        const obj = mp.objects.new(hash, this.pos, {dimension: this.player.dimension});
        //obj.isAttached()
        //let index = 0        
        //console.logInfo(`add scene ${obj.doesExist()}`);
        while (!obj.doesExist()) {
            //global.debugText = `check: ${index++}`;
            await mp.game.waitAsync(0);
        }
        obj.setCollision(false, false);
        obj.setCanBeDamaged(false);
        obj.attachTo(this.player.handle,
            this.player.getBoneIndex(prop.boneId),
            prop.offset.x, prop.offset.y, prop.offset.z, 
            prop.rotate.x, prop.rotate.y, prop.rotate.z, 
            false, false, false, false, 2, true);
        // obj.setCollision(false, false);
        this.props.push({obj, deleteBefore: prop.deleteBefore})
    }

    isCompleeted(){
        let time, anim;
        switch (this.state) {
            case states.loop:
                anim = this.currentSequenceAnim();
                if(!this.player.isPlayingAnim(anim.dictionary, anim.name, 3) || mp.players.local.vehicle)
                    if(this.player.remoteId === mp.players.local.remoteId)
                        mp.events.callRemote("scene:action:cancel");         
                return false;
            case states.sequence:               
                anim = this.currentSequenceAnim();
                time = this.player.getAnimCurrentTime(anim.dictionary, anim.name);
                if(!this.player.isPlayingAnim(anim.dictionary, anim.name, 3) || mp.players.local.vehicle){
                    if(this.player.remoteId === mp.players.local.remoteId)
                        mp.events.callRemote("scene:action:cancel");         
                    return false;
                }
                    
                if(time === 0 || time >= anim.stop){
                    if(!this.nextAimInSequence()){
                        if(this.isLooped){
                            this.sequenceStage = this.sequence.length - 1;
                            this.state = states.loop;
                        }else{
                            this.resetEffects();
                            if(mp.players.local.remoteId === this.player.remoteId)
                                mp.events.callRemote("scene:seqence:callback");
                            this.cancelHandle();
                        }
                    }
                    else 
                    {
                        const anim = this.currentSequenceAnim();                        
                        this.playAnim(anim);
                    }
                }
                else
                    this.checkEffects(time);
                return false;
            case states.enter:
                if(this.enter !== null){
                    time = this.player.getAnimCurrentTime(this.enter.dictionary, this.enter.name);
                    //global.debugText = `enter: ${time.toFixed(3)}`;
                    if(time === 0 || time >= this.enter.stop){
                        this.resetEffects();
                        this.state = states.base;
                        this.playAnim(this.base);
                    }else 
                        this.checkEffects(time);
                }else {
                    this.state = states.base; 
                    this.playAnim(this.base);
                }               
                return false;
            case states.base:
                if(!this.player.isPlayingAnim(this.base.dictionary, this.base.name, 3) || mp.players.local.vehicle){                    
                    if(this.player.remoteId === mp.players.local.remoteId)
                        mp.events.callRemote("scene:action:cancel");         
                }
                //lobal.debugText = `base: ${time.toFixed(3)}`;
                if(this.id === 13 && mp.players.local.remoteId === this.player.remoteId && !global.binoculars)
                    global.binoculars = true;

                return false;
            case states.action:
                time = this.player.getAnimCurrentTime(this.action.dictionary, this.action.name);
                //global.debugText = `action: ${time.toFixed(3)}`;
                if(time === 0 || time >= this.action.stop){
                    this.resetEffects();
                    this.state = states.base;
                    this.playAnim(this.base);
                }else
                    this.checkEffects(time);
                return false;
            case states.exit:
                time = this.player.getAnimCurrentTime(this.exit.dictionary, this.exit.name);
                //global.debugText = `exit: ${time.toFixed(3)}`;
                if(time === 0 || time >= this.exit.stop)
                {
                    this.state = states.completed;
                }
                return false;
        
            default:
                return true;
        }
    }

    playAnim(anim){
        //this.player.clearTasksImmediately();
        this.player.taskPlayAnim(anim.dictionary, anim.name, 8.0, 8.0, -1, anim.flag, anim.start, false, false, false);
    }

    currentSequenceAnim(){
        return this.sequence[this.sequenceStage]
    }

    nextAimInSequence(){
        this.sequenceStage += 1;
        return this.currentSequenceAnim() !== undefined;
    }

    rightClick(){
        if(this.isLooped || (!this.canCancel && this.state !== states.base)) return;
        mp.events.callRemote("scene:action:cancel");
    }

    cancelHandle(){
        this.props.forEach(prop=>{
            if(prop.deleteBefore){
                //if(prop.obj.doesExist()){
                    prop.obj.destroy();
                    prop.obj = null;
                //}
            }                    
        });
        if(this.exit !== null && this.player.handle !== 0){      
            this.state = states.exit;
            this.playAnim(this.exit);
        }            
        else 
            this.state = states.completed;
    }

    
    leftClick(){        
        if(this.action === null || this.state !== states.base) return;
        mp.events.callRemote("scene:action:request");
    }
    
    actionHandle(){
        this.state = states.action;
        this.playAnim(this.action);
    }

    loadProp(hash){
        if(!mp.game.streaming.isModelValid(hash)){
            mp.serverLog(`bad model ${hash}`);
            return;
        }
        if(!mp.game.streaming.hasModelLoaded(hash)){
            mp.game.streaming.requestModel(hash);
            for (let index = 0; index < 150 && !mp.game.streaming.hasModelLoaded(hash); index++) {
                mp.game.wait(0);
            }
        }
    }

    loadAnimDict(dict){
        if(!mp.game.streaming.doesAnimDictExist(dict)){
            mp.serverLog(`bad danim dictionary ${dict}`);
            return;
        }
        if(!mp.game.streaming.hasAnimDictLoaded(dict)){
            mp.game.streaming.requestAnimDict(dict)
            for (let index = 0; index < 150 && !mp.game.streaming.hasAnimDictLoaded(dict); index++) {
                mp.game.wait(0);
            }
        }
    }

    destroy(){
        if(this.player.remoteId === mp.players.local.remoteId){
            global.binoculars = false;
            global.inAction = false;
        }
        this.props.forEach(prop=>{
            if(prop.obj /*&& prop.obj.doesExist()*/)
                prop.obj.destroy();
        })
        this.props = [];
        if(this.player.handle !== 0)
            this.player.clearTasksImmediately();
    }
}