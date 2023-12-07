const mouseSates = {
	"release": 0,
	"left": 1,
	"right": 2
}
module.exports = {
	obj: null,
	effect: null,
	speed: .01,
	temp:{
		offsetX: .19,
		offsetY: .01,
		offsetZ: -.1,
		rotateX: -75.01,
		rotateY: -10.01,
		rotateZ: 0
	},
	saved:{
		offsetX: .19,
		offsetY: .01,
		offsetZ: -.1,
		rotateX: -75.01,
		rotateY: -10.01,
		rotateZ: 0
	},
	effectName: "scr_fbi_falling_debris",//"exp_grd_grenade_smoke", //"ent_anim_cig_smoke",

    onMouseMove(mouse) {
        if(mp.keys.isDown(global.Keys.Key_SHIFT)) return;
        if(this.obj === null) return;
        if(mp.keys.isDown(global.Keys.Key_ALT))
            this.speed = .01;		
        else 
            this.speed = .0025;
    
        if(mouse.state === mouseSates.right){	
             
            if(mp.keys.isDown(global.Keys.Key_CONTROL)){
                this.rotateZZ(mouse);
            }else{
                this.rotateXY(mouse);
            }
        }else if(mouse.state === mouseSates.left){
            if(mp.keys.isDown(global.Keys.Key_CONTROL))
            this.moveZ(mouse);			
            else 
            this.moveXY(mouse);
        }
    }, 
	onMouseUp(){
        this.saved.offsetX += this.temp.offsetX;
        this.saved.offsetY += this.temp.offsetY;
        this.saved.offsetZ += this.temp.offsetZ;
        this.saved.rotateX += this.temp.rotateX
        this.saved.rotateY += this.temp.rotateY
        this.saved.rotateZ += this.temp.rotateZ
		this.temp.offsetX = 0;
		this.temp.offsetY = 0;
		this.temp.offsetZ = 0;
		this.temp.rotateX = 0;
		this.temp.rotateY = 0;
		this.temp.rotateZ = 0;
	},
    
	moveXY(mouse){		
		try{
			if (this.obj === null ) return;
		
			this.temp.offsetX = parseFloat(((mouse.sx - mouse.x)* this.speed).toFixed(4));
			this.temp.offsetY = parseFloat(((mouse.sy - mouse.y)* this.speed).toFixed(4))
			this.attach();
		} catch (e) {
			if(global.sendException) mp.serverLog(`Error in testPed.move: ${e.name}\n${e.message}\n${e.stack}`);
		}
	},
	moveZ(mouse){
		try{		
			this.temp.offsetZ = parseFloat(((mouse.sy - mouse.y)* this.speed).toFixed(4));
			this.attach();
		} catch (e) {
			if(global.sendException) mp.serverLog(`Error in testPed.move: ${e.name}\n${e.message}\n${e.stack}`);
		}
	},
	rotateXY(mouse){
		try{
			if (this.obj === null ) return;
			this.temp.rotateX = Math.floor((mouse.sx - mouse.x)  * this.speed * 100);
			this.temp.rotateY = Math.floor((mouse.sy - mouse.y)  * this.speed * 100);
			this.attach();
		} catch (e) {
			if(global.sendException) mp.serverLog(`Error in testPed.move: ${e.name}\n${e.message}\n${e.stack}`);
		}
	},
	rotateZZ(mouse){
		try{
			if (this.obj === null ) return;
			this.temp.rotateZ = Math.floor((mouse.sy - mouse.y)  * this.speed * 50);
			this.attach();
		} catch (e) {
			if(global.sendException) mp.serverLog(`Error in testPed.move: ${e.name}\n${e.message}\n${e.stack}`);
		}
	},
	async create(dictionary, name, flag, boneId, jattachs){
        try {
            const attachs = JSON.parse(jattachs);
            if(attachs === null) return;
            global.malboro = true;	
            this.loadAnimDict(dictionary);
            mp.players.local.clearTasksImmediately();
            mp.players.local.taskPlayAnim(dictionary, name, 8.0, 1.0, -1, flag, 0, false, false, false);
            this.boneId = boneId; //R hand
            this.boneIndex = mp.players.local.getBoneIndex(this.boneId);
            if(this.obj !== null){
                this.destroy();
            }
            //const model = mp.game.joaat('ng_proc_cigarette01a');
            const model = mp.game.joaat(attachs[0]);//prop_acc_guitar_01 p_ing_microphonel_01 prop_acc_guitar_01 prop_v_cam_01
            if(!mp.game.streaming.hasModelLoaded(model)){
                mp.game.streaming.requestModel2(model);
                await mp.game.waitAsync(100);
            }
            this.obj = mp.objects.new(model, mp.players.local.position);
			while (!this.obj.doesExist()) {
				await mp.game.waitAsync(0);
			}
            
            this.attach();
        } catch (e) {
            if(global.sendException) mp.serverLog(`devAttach.create: ${e.name}\n${e.message}\n${e.stack}`);
        }
	},
	loadAnimDict(dict){
        if(!mp.game.streaming.doesAnimDictExist(dict)){
            mp.serverLog(`bad danim dictionary ${dict}`);
            return;
        }
        if(!mp.game.streaming.hasAnimDictLoaded(dict)){
            mp.game.streaming.requestAnimDict(dict)
            while (!mp.game.streaming.hasAnimDictLoaded(dict)) {
                mp.game.wait(0);
            }
        }
    },
	attach(){
		if(this.obj == null) return;
		global.debugText = `attach: ${parseFloat((this.saved.offsetX + this.temp.offsetX).toFixed(4))}, ${parseFloat((this.saved.offsetY + this.temp.offsetY).toFixed(4))}, ${parseFloat((this.saved.offsetZ + this.temp.offsetZ).toFixed(4))} / ${parseFloat((this.saved.rotateX + this.temp.rotateX).toFixed(4))}, ${parseFloat((this.saved.rotateY + this.temp.rotateY).toFixed(4))}, ${parseFloat((this.saved.rotateZ + this.temp.rotateZ).toFixed(4))}`;
		this.obj.detach(false, false);
		this.obj.attachTo(mp.players.local.handle, this.boneIndex, 
			this.saved.offsetX + this.temp.offsetX,
			this.saved.offsetY + this.temp.offsetY,
			this.saved.offsetZ + this.temp.offsetZ, 
			this.saved.rotateX + this.temp.rotateX,
			this.saved.rotateY + this.temp.rotateY,
			this.saved.rotateZ + this.temp.rotateZ,  
			false, false, false, false, 2, true);
	},

	startEffect(palyer){
		if(!mp.game.streaming.hasNamedPtfxAssetLoaded(this.effectName)){
			mp.game.streaming.requestNamedPtfxAsset(this.effectName);
			mp.game.wait(20);
			mp.gui.chat.push(`check: ${mp.game.streaming.hasNamedPtfxAssetLoaded(this.effectName)}`)
		}
		this.effect = mp.game.graphics.startParticleFxLoopedOnEntity2(this.effectName, palyer.position.x, palyer.position.y, palyer.position.z,0,0,0,1,false, false, false, 0) ;
	},
    destroy(){
        if(this.obj !== null){
            this.obj.destroy();
            this.obj = null
        }
        mp.players.local.clearTasksImmediately();
        this.saved.offsetX = 0;
		this.saved.offsetY = 0;
		this.saved.offsetZ = 0;
		this.saved.rotateX = 0;
		this.saved.rotateY = 0;
		this.saved.rotateZ = 0;
        this.temp.offsetX = 0;
		this.temp.offsetY = 0;
		this.temp.offsetZ = 0;
		this.temp.rotateX = 0;
		this.temp.rotateY = 0;
		this.temp.rotateZ = 0;
		global.malboro = false;	
    }
}