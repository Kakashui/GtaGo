const { object } = require('./malboro/devObject');
let sendingExcept = false;
class EnviromentActions{
    constructor() {
        this.config = require('./configs/seats');
        this.models = [];
        for (const key in this.config) {
            this.models.push(parseInt(key));
        }
        this.model = 0;
        this.handle = 0;
        this.spamProtection = 0;
        this.distance = 1.5;
        this.isSit = false;
        this.actionVisible = false;
        this.animations = {
            "0":["switch@michael@sitting", "idle", 1],
            "1":["amb@prop_human_seat_sunlounger@male@base","base",1],
            "2":["amb@lo_res_idles@","lying_face_up_lo_res_base",1],
            "3":["amb@prop_human_seat_bar@male@elbows_on_bar@base","base", 1],
            "4":["anim@amb@casino@peds@","amb_prop_human_seat_bar_male_elbows_on_bar_idle_c", 1]
        }
        this.init();
    }
    animationDict(index){
		if(index === undefined || this.animations[index] === undefined)
			return this.animations[0][0];
		return this.animations[index][0]
	}
	animationName(index){
		if(index === undefined || this.animations[index] === undefined)
			return this.animations[0][1];
		return this.animations[index][1];
	}
	animationFlag(index){
		if(index === undefined || this.animations[index] === undefined)
			return this.animations[0][2];
		return this.animations[index][2];
	}
    init(){
        mp.keys.bind(Keys.Key_C, false, this.keyC.bind(this));
        mp.keys.bind(Keys.Key_BACK, false, this.keyBack.bind(this));
        setInterval(this.onEachTick.bind(this), 1000)
    }
    async loadAnimDict(dict){
        if(!mp.game.streaming.doesAnimDictExist(dict)){
            mp.serverLog(`bad danim dictionary ${dict}`);
            return;
        }
        if(!mp.game.streaming.hasAnimDictLoaded(dict)){
            mp.game.streaming.requestAnimDict(dict);
            while (!mp.game.streaming.hasAnimDictLoaded(dict)) {
                await mp.game.waitAsync(0)
            }
        }
    }  
    async sit(){
        if(global.inAction) return;
        const config = this.config[this.model];
        const position = mp.game.invokeVector3(global.NATIVES.GET_ENTITY_COORDS, this.handle, true);
        const rotation = mp.game.invokeVector3(global.NATIVES.GET_ENTITY_ROTATION, this.handle, 2);
        const freePlaces = [];
        config.places.forEach(p => {  
            const offs = global.GetOffsetPosition(new mp.Vector3(), rotation.z, p.pos); 
            const sitPos = new mp.Vector3(position.x + offs.x, position.y + offs.y, position.z + offs.z);    
            const sitZ = rotation.z + p.rot.z;
            let isFree = true;
            mp.players.streamed.every(pl=>{
                if(pl.remoteId !== mp.players.local.remoteId){
                    const dist = mp.game.system.vdist(pl.position.x, pl.position.y, pl.position.z, sitPos.x, sitPos.y, sitPos.z);
                    if(dist < .4){
                        isFree = false;
                        return false;
                    }
                } 
                return true; 
            })
            const dist = mp.game.system.vdist(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z, sitPos.x, sitPos.y, sitPos.z);
            if(isFree)
                freePlaces.push({sitPos, sitZ, dist});
        });
        if(freePlaces.length === 0){
            mp.events.call('notify', 1, 9, "env:sit:occupied", 3000);
            return;
        }
        await this.loadAnimDict(this.animationDict(config.anim));
        //mp.players.local.freezePosition(true);
        //mp.players.local.setCollision(false,true);
        const place = this.getClosestPlace(freePlaces);
        this.hideAction();
        this.isSit = true;
        global.inAction = true;
        global.controlsManager.disableAll(1,2);
        mp.players.local.taskPlayAnimAdvanced(
            this.animationDict(config.anim), 
            this.animationName(config.anim), 
            place.sitPos.x, place.sitPos.y, place.sitPos.z, 0, 0, place.sitZ, 8.0, 0, -1, this.animationFlag(config.anim), 0, 2, 1)
        this.showBackAction();
    }

    getClosestPlace(places){
        if(places.length === 1) return places[0];
        let closest = places[0];
        for (let index = 1; index < places.length; index++) {
            const place = places[index];
            if(place.dist < closest.dist)
                closest = place;
        }
        return closest;        
    }

    hideAction(){
        if(!this.actionVisible) return;
        this.actionVisible = false;
        global.gui.setData('hud/setPromptData', JSON.stringify({ show: false, items: [] }));
    }
    showSitAction(){
        if(!this.isSit && !this.actionVisible){
            this.actionVisible = true;
            global.gui.setData('hud/setPromptData', JSON.stringify({ show: true, items: [{ key: 'C', text: "env:action:sit"}] }));
        }
    }    
    showBackAction(){
        if(this.isSit){
            this.actionVisible = true;
            global.gui.setData('hud/setPromptData', JSON.stringify({ show: true, items: [{ key: '🠐', text: "env:action:sit:free"}] }));  
        }
    }
    keyC(){
        if(mp.players.local.vehicle) return;
        if(!this.isSit && this.model !== 0){
            if(this.spamProtection > Date.now()) return;
            this.spamProtection = Date.now() + 500;
            if(this.config[this.model] !== undefined && this.config[this.model].type === "sit")
                this.sit();
        }
    }
    keyBack(){
        if(this.isSit){
            if(this.spamProtection > Date.now()) return;   
            mp.players.local.freezePosition(false);
            this.spamProtection = Date.now() + 500;
            this.isSit = false;
            global.controlsManager.enableAll();
            global.inAction = false;
            mp.players.local.clearTasksImmediately();
            this.hideAction();
        }
    }
    onEachTick(){
        try {
            if(mp.players.local.vehicle){
                if(this.actionVisible === true)
                    this.hideAction();
                if(this.model !== 0)
                    this.model = 0;
                return;
            }
            this.handle = 0;           
            const pos = mp.players.local.position;
            for (let index = 0; (this.handle === 0 && index < this.models.length); index++) {
                this.model = this.models[index];
                this.handle = mp.game.object.getClosestObjectOfType(pos.x, pos.y, pos.z, this.distance, this.model, false, true, true);
            }
            if(this.handle === 0){
                this.hideAction();
                this.model = 0;
            }
            else
                this.showSitAction();
        } catch (e) {
            if(global.sendException && !sendingExcept) {
                sendingExcept = true;
                mp.serverLog(`onEachTick ${e.name}\n${e.message}\n${e.stack}`);
            }
        }
    }
}

module.exports = new EnviromentActions();