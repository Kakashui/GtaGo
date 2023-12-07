module.exports = {
	ped: null,
	peds: [],
	pos: null,
	rot: null,
	posOff: null,
	rotOff: null,
	speed: .025,
	pedModel: 0x705E61F2,
	animations:{
		"0":["switch@michael@sitting", "idle", 1],
		"1":["amb@prop_human_seat_sunlounger@male@base","base",1],
		"2":["amb@lo_res_idles@","lying_face_up_lo_res_base",1],
		"3":["amb@prop_human_seat_bar@male@elbows_on_bar@base","base", 1],
		"4":["anim@amb@casino@peds@","amb_prop_human_seat_bar_male_elbows_on_bar_idle_c", 1],
		"5":["anim@amb@business@weed@weed_sorting_seated@", "sorter_left_sort_v1_sorter01", 1]
	},
	//amb@code_human_in_bus_passenger_idles@male@sit@idle_b idle_e
	//
	currentAnimation: 5,
	animationDict(index){
		if(index === undefined || this.animations[index] === undefined)
			return this.animations[this.currentAnimation][0]
		return this.animations[index][0]
	},
	animationName(index){
		if(index === undefined || this.animations[index] === undefined)
			return this.animations[this.currentAnimation][1]
		return this.animations[index][1]
	},
	animationFlag(index){
		if(index === undefined || this.animations[index] === undefined)
			return this.animations[this.currentAnimation][2]
		return this.animations[index][2]
	},
	previousAnim(){
		this.currentAnimation--;
		if(this.animations[this.currentAnimation] === undefined)this.currentAnimation = 0;
		this.update();
	},
	nextAnim(){
		this.currentAnimation++;
		if(this.animations[this.currentAnimation] === undefined)this.currentAnimation = Object.values(this.animations).length -1;
		this.update();
	},
	create(pos){
		try {
			if(this.ped !== null){
				this.cancel()
			}
			//this.config = config;
            global.showCursor(true);
			this.pos = pos;
			this.rot =  new mp.Vector3();
			this.load();
			this.ped = mp.peds.newValid(this.pedModel, this.pos, -40, mp.players.local.dimension);
			this.ped.freezePosition(true);
            this.ped.setCollision(false,true);
            //this.ped.attachTo(nahdle, 0, offs.x, offs.y, offs.z,0,0, place.rot.z, false, false, false, true,0,true);
            
			this.update();
		} catch (e) {
			if(global.sendException) mp.serverLog(`Error in testPed.handle: ${e.name}\n${e.message}\n${e.stack}`);
		}
	},
	load(){
		Object.values(this.animations).forEach(anim => {
			mp.game.streaming.requestAnimDict(anim[0]);
		});
		// this.config.places.forEach(el => {
		// 	const ped = mp.peds.newValid(this.pedModel, this.pos, this.rot, mp.players.local.dimension);
		// 	const offs = global.GetOffsetPosition(new mp.Vector3(), this.rot.z, el.pos);
		// 	// ped.taskPlayAnimAdvanced(
		// 	// 	"switch@michael@sitting", "idle", 
		// 	// 	this.pos.x + offs.x , this.pos.y + offs.y ,this.pos.z + offs.z, 
		// 	// 	this.rot.x + el.rot.x, this.rot.y + el.rot.y, this.rot.z + el.rot.z,
		// 	// 	8, 1, -1, 5642, 0, 2, 1
		// 	// );
		// 	ped.freezePosition(true);
		// 	ped.setCollision(false,true);
		// 	// ped.setHeading(this.rot.z + el.rot.z);
		// 	// ped.setCoordsNoOffset(this.pos.x + offs.x , this.pos.y + offs.y ,this.pos.z + offs.z,false, false, false);
		// 	ped.attachTo(this.handle, 0, offs.x, offs.y, offs.z,0,0, el.rot.z,false, false, false, true,0,true);
		// 	ped.taskPlayAnim(this.animationDict(ped.anim), this.animationName(ped.anim), 8.0, 0, -1, this.animationFlag(ped.anim), 1, true, true, true);

		// 	// ped.taskPlayAnim("switch@michael@sitting", "idle", 8.0, 0, -1, 1, 0, false, false, false);
        //     // ped.setRotation(this.rot.x + el.rot.x, this.rot.y + el.rot.y, this.rot.z + el.rot.z, 2, true)
        //     // ped.setCoords(this.pos.x + offs.x , this.pos.y + offs.y ,this.pos.z + offs.z, true,true,true, false);
		// 	this.peds.push(ped);
		// });
	},
    save(){
		const pos = global.GetOffsetPosition(new mp.Vector3(), this.rot.z * -1, this.posOff);
		//mp.events.callRemote("mlbr:sit:pos:save", this.config.model, JSON.stringify(pos), JSON.stringify(this.rotOff), this.currentAnimation);
		this.peds.forEach(ped => {
			ped.destroy();
		});
		this.ped.destroy();
        this.ped = null;
        global.showCursor(false);
		this.peds = [];
    },
	cancel(){
		if(this.ped !== null){
			this.ped.destroy();
            this.ped = null;
            global.showCursor(false);
        }
		// this.peds.forEach(ped => {
		// 	ped.destroy();
		// });
		// this.peds = [];
		//mp.events.callRemote("mlbr:sit:pos:cancel", this.config.model);
		mp.events.callRemote("mlbr:sit:pos:save:2", JSON.stringify(this.pos), JSON.stringify(this.rot));
	},
	delete(){
		if(this.ped !== null){
			this.ped.destroy();
			this.ped = null;
			global.showCursor(false);
		}
		this.peds.forEach(ped => {
			ped.destroy();
		});
		this.peds = [];
		//mp.events.callRemote("mlbr:sit:pos:delete", this.config.model);
	},
	move(mouse){		
		try{
			if (this.ped == null ) return;
			const camDir = global.gameplayCam.getDirection();
			const right = global.GetOffsetPosition(new mp.Vector3(), 90, camDir);
			const dirX = mouse.dirrectX;
			const dirY = mouse.dirrectY;
			this.pos.x += right.x * dirX * this.speed;
			this.pos.y += right.y * dirX * this.speed;
			this.pos.x += camDir.x * dirY * this.speed;
			this.pos.y += camDir.y * dirY * this.speed;
			this.update();
		} catch (e) {
			if(global.sendException) mp.serverLog(`Error in testPed.move: ${e.name}\n${e.message}\n${e.stack}`);
		}
	},	
	rotate(mouse){
		try{
			if (this.ped == null ) return;
			this.rot.z += mouse.dirrectX * 80 * this.speed;
			this.update();
		} catch (e) {
			if(global.sendException) mp.serverLog(`Error in testPed.move: ${e.name}\n${e.message}\n${e.stack}`);
		}
	},
	moveZ(mouse){
		try{
			if (this.ped == null ) return;
			this.pos.z += mouse.dirrectY * this.speed;
			this.update();
		} catch (e) {
			if(global.sendException) mp.serverLog(`Error in testPed.move: ${e.name}\n${e.message}\n${e.stack}`);
		}
	},
	update(){
		//this.ped.clearTasksImmediately();
		//this.ped.taskPlayAnimAdvanced(this.animationDict(),this.animationName(), this.pos.x, this.pos.y, this.pos.z, this.rot.x, this.rot.y, this.rot.z, 8, 1, -1, this.animationFlag(), 0, 2, 1);
		//this.ped.taskPlayAnimAdvanced(this.animationDict(), this.animationName(), this.pos.x, this.pos.y, this.pos.z, this.rot.x, this.rot.y, this.rot.z, 8.0, 0, -1, this.animationFlag(), 1, true, true, true);
		this.ped.setRotation(this.rot.x, this.rot.y, this.rot.z, 2, true);
		this.ped.setCoords(this.pos.x, this.pos.y, this.pos.z, true, true, true, true);
		this.ped.taskPlayAnim(this.animationDict(),this.animationName(), 8.0, 0, -1, this.animationFlag(), 1, true, true, true);
	}
}