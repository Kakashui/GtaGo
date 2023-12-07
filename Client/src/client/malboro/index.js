//require('./interiors');

mp.game.streaming.requestAnimDict("mini@prostitutes@sexlow_veh");
mp.game.streaming.requestAnimDict("mini@prostitutes@sexnorm_veh");
mp.game.streaming.requestAnimDict("misscarsteal2pimpsex");
mp.game.streaming.requestAnimDict("rcmpaparazzo_2");
mp.game.streaming.requestAnimDict("switch@michael@sitting");

//const Console = require('./Console');
const devAttach = require('./devAttach');
const devPed = require('./devPed');
const devPed2 = require('./devPed_v2');
const devObject = require('./devObject');
const customCamera = require('./CustomCamera');
global.debugText = "";

mp.events.add("mal:timecycle", (cycle)=>{
	mp.gui.chat.push(cycle);
	mp.game.graphics.setTimecycleModifier(cycle);
});

let draw = 0;
mp.keys.bind(global.Keys.Key_Z, false, ()=>{
	global.setClothing(mp.players.local, 11, draw, 0, 0);
	mp.gui.chat.push(`${draw}`);
	draw -= 1;
})

mp.keys.bind(global.Keys.Key_X, false, ()=>{ 
	global.setClothing(mp.players.local, 11, draw, 0, 0);
	mp.gui.chat.push(`${draw}`);
	draw += 1;
})
mp.keys.bind(global.Keys.Key_F2, false, ()=>{	
	if(devPed.ped !== null)devPed.delete();
	//if(devPed2.ped !== null)devPed2.delete();
})
mp.keys.bind(global.Keys.Key_F3, false, ()=>{
	//devPed2.cancel();
	devObject.deleteObject();
	cmeraSwitch(false);
	devAttach.destroy();
	devPed.cancel();
});

mp.keys.bind(global.Keys.Key_F5, false, testMethod);

mp.game.streaming.requestAnimDict("amb@code_human_in_bus_passenger_idles@female@sit@base");
mp.game.streaming.requestAnimDict("amb@code_human_in_bus_passenger_idles@male@sit@base");

const mouseSates = {
	"release": 0,
	"left": 1,
	"right": 2
}

const mouse = {
	state: mouseSates.release,
	sx: 0,
	sy: 0,
	x: 0,
	y: 0,
	dirrectX: 0,
	dirrectY: 0
}

function cmeraSwitch(toggle) {
	if(toggle){
		customCamera.setPos(new mp.Vector3(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z));
		customCamera.setPoint(new mp.Vector3(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z));
		customCamera.moveCamZ(.1);
		customCamera.setDist(2);
		customCamera.moveAngleX(90);
		customCamera.switchOn(200);
	}else{
		customCamera.switchOff(200);
	}
	global.showCursor(toggle);
}

function onPedMove() {
	if(mp.keys.isDown(global.Keys.Key_SHIFT)) return;
	if(devPed.ped === null) return;

	if(mp.keys.isDown(global.Keys.Key_ALT)){
		devPed.speed = .1;
	} else {
		devPed.speed = .025;
	}		

	if(mouse.state === mouseSates.right){
		devPed.rotate(mouse);
	}else if(mouse.state === mouseSates.left){	
		if(mp.keys.isDown(global.Keys.Key_CONTROL))
			devPed.moveZ(mouse);			
		else 
			devPed.move(mouse);
	}
}

function onPedMove2() {
	if(mp.keys.isDown(global.Keys.Key_SHIFT)) return;
	if(devPed2.ped === null) return;

	if(mp.keys.isDown(global.Keys.Key_ALT)){
		devPed2.speed = .1;
	} else {
		devPed2.speed = .025;
	}		

	if(mouse.state === mouseSates.right){
		devPed2.rotate(mouse);
	}else if(mouse.state === mouseSates.left){	
		if(mp.keys.isDown(global.Keys.Key_CONTROL))
			devPed2.moveZ(mouse);			
		else 
			devPed2.move(mouse);
	}
}

function onCameraMove() {
	if(!customCamera.isActive) return;
	if(mp.keys.isDown(global.Keys.Key_ALT))
		customCamera.speed = .1;		
	else 
		customCamera.speed = .025;

	if(mouse.state === mouseSates.left){
		if(mp.keys.isDown(global.Keys.Key_SHIFT)){
			customCamera.mouseMove(mouse.sx - mouse.x, mouse.sy - mouse.y);
		}
	}
}

mp.events.add('click', (x, y, upOrDown, leftOrRight, relativeX, relativeY, worldPosition, hitEntity) => {
    if (upOrDown === "up"){
		mouse.state = mouseSates.release;
		mouse.x = 0;
		mouse.y = 0;
		mouse.sx = 0;
		mouse.sy = 0;
		mouse.dirrectX = 0;
		mouse.dirrectY = 0;
		devAttach.onMouseUp();
		if(mp.keys.isDown(global.Keys.Key_SHIFT))
			customCamera.mouseUp(leftOrRight === "left");
	}else {
		mouse.state = leftOrRight === "left" ? mouseSates.left : mouseSates.right;		
		mouse.x = x;
		mouse.y = y;
		mouse.sx = x;
		mouse.sy = y;
	}
});

let isSit = false;

function testMethod(){	
	try {		
		if(devPed.ped === null){
			if(targetEntity.valid)
				mp.events.callRemote("devped", targetEntity.model)
		}else 
			devPed.save();
		
	} catch (e) {
		console.logInfo(`testMethod ${e.name}\n${e.message}\n${e.stack}`);
	}
}

let targetEntity = {
	handle: 0,
	model: 0,
	position: null,
	rotation: null,
	valid: false,
	update(raycast){
		this.handle = raycast.entity;
		this.model = mp.game.invoke(global.NATIVES.GET_ENTITY_MODEL, this.handle);
		this.valid = true;
		this.position = mp.game.invokeVector3(global.NATIVES.GET_ENTITY_COORDS, this.handle, true);
        this.rotation = mp.game.invokeVector3(global.NATIVES.GET_ENTITY_ROTATION, this.handle, 2);
		global.debugText = `${this.model} / ${JSON.stringify(this.position)}`;
	},
	noObject(){
		if(!this.valid) return;
		this.valid = false;
	},
};

const distance = 10;
function raycast() {
	try {		
		let position = global.gameplayCam.getCoord();
		let direction = global.gameplayCam.getDirection();
		let farAway = new mp.Vector3((direction.x * distance) + (position.x), (direction.y * distance) + (position.y), (direction.z * distance) + (position.z)); 
		mp.game.graphics.drawLine(new mp.Vector3(position.x, position.y, position.z), new mp.Vector3(farAway.x, farAway.y, farAway.z), 255, 255, 255, 255);
		
		let result = mp.raycasting.testPointToPoint(position, farAway, mp.players.local.handle, 1, 16);
		if(result && result.entity && typeof result.entity === 'number'){
			if(targetEntity.handle !== result.entity)
				targetEntity.update(result)
		}
		else 
			targetEntity.noObject()
	} catch (e) {
		mp.serverLog(`raycast ${e.name}\n${e.message}\n${e.stack}`);
	}
}

function nearObject() {
	const pos = mp.players.local.position;
	const object = mp.game.object.getClosestObjectOfType(pos.x, pos.y, pos.z, 2, 2142033519, false, false, false);
	if(switchValue && object !== 0){
		mp.players.local.attachTo(object, 0,0,0,0,0,0,0,false, false, false, true,0,true);
	}else mp.players.local.detach(false, true);
};

let switchValue = false;
let invId = 0;

let clone = -1;
let preview = -1;
function switchVal() {
	switchValue = !switchValue;
	// if(invId !== 0){
	// 	mp.game.interior.disableInterior(invId, switchValue);
	// 	//mp.game.interior.refreshInterior(invId);
	// }
	if(switchValue){
		pedFunk();		
	}else{
		// if(clone !== -1){
		// 	mp.game.invoke(global.NATIVES.DELETE_ENTITY, clone);
		// 	clone = -1;
		// }			
    	mp.game.invoke(global.NATIVES.RESET_SCRIPT_GFX_ALIGN);
		mp.game.ui.setFrontendActive(false);		
		mp.game.invoke(global.NATIVES.SetMouseCursorVisibleInMenus, 1);
		mp.game.invoke(global.NATIVES.SetPauseMenuPedLighting, 0);//
		mp.game.invoke(global.NATIVES.SetPauseMenuPedSleepState, 0);//
	}
}
const hashes = [
	mp.game.joaat("FE_MENU_VERSION_SP_PAUSE"),
	mp.game.joaat("FE_MENU_VERSION_MP_PAUSE"),
	mp.game.joaat("FE_MENU_VERSION_CREATOR_PAUSE"),
	mp.game.joaat("FE_MENU_VERSION_CUTSCENE_PAUSE"),
	mp.game.joaat("FE_MENU_VERSION_SAVEGAME"),
	mp.game.joaat("FE_MENU_VERSION_PRE_LOBBY"),
	mp.game.joaat("FE_MENU_VERSION_LOBBY"),
	mp.game.joaat("FE_MENU_VERSION_MP_CHARACTER_SELECT"),
	mp.game.joaat("FE_MENU_VERSION_MP_CHARACTER_CREATION"),
	mp.game.joaat("FE_MENU_VERSION_EMPTY"),
	mp.game.joaat("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"),
	mp.game.joaat("FE_MENU_VERSION_TEXT_SELECTION"),
	mp.game.joaat("FE_MENU_VERSION_CORONA"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_LOBBY"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_JOINED_PLAYERS"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_INVITE_PLAYERS"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_INVITE_FRIENDS"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_INVITE_CREWS"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_INVITE_MATCHED_PLAYERS"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_INVITE_LAST_JOB_PLAYERS"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_RACE"),
	mp.game.joaat("FE_MENU_VERSION_CORONA_BETTING"),
	mp.game.joaat("FE_MENU_VERSION_JOINING_SCREEN"),
	mp.game.joaat("FE_MENU_VERSION_LANDING_MENU"),
	mp.game.joaat("FE_MENU_VERSION_LANDING_KEYMAPPING_MENU")
]
let index = 0;
let flag = -1;
//let GetHash = mp.game.joaat("FE_MENU_VERSION_EMPTY");
//let GetHash = mp.game.joaat("FE_MENU_VERSION_CORONA");;
//let GetHash = mp.game.joaat("FE_MENU_VERSION_EMPTY_NO_BACKGROUND");
async function pedFunk()
{
    let heading = mp.players.local.getHeading();
	let pos = mp.players.local.position;
    clone =  mp.players.local.clone(heading, false, false);
	mp.game.invoke(global.NATIVES.SET_ENTITY_COORDS, clone, pos.x, pos.y, pos.z - 10, false, false, false, true);	
	mp.game.invoke(global.NATIVES.FREEZE_ENTITY_POSITION, clone, true);
	mp.game.invoke(global.NATIVES.SET_ENTITY_VISIBLE, clone, false, false);
	//mp.game.invoke(global.NATIVES.GivePedToPauseMenu, clone, 2);
    //let clonePos = mp.game.invokeVector3(global.NATIVES.GET_ENTITY_COORDS, clone, false);
    //mp.game.invoke(global.NATIVES.SetScriptGfxAlign, 67, 67);	
    //mp.game.invoke(global.NATIVES.SetScriptGfxAlignParams, 100, 100, 500, 500);
	mp.game.ui.activateFrontendMenu(hashes[index++], true, -1);
	//mp.game.ui.setFrontendActive(false);
    //mp.game.invoke(RAGE.Game.Natives.ActivateFrontendMenu, GetHash, 0, -1);
    mp.game.invoke(global.NATIVES.SetMouseCursorVisibleInMenus, 0);
	//await mp.game.waitAsync(200);
    //RAGE.Game.Network.NetworkSetEntityVisibleToNetwork(clone, false);
    await mp.game.waitAsync(200);
	mp.game.entity.setObjectAsNoLongerNeeded(clone);
	preview = mp.game.invoke(global.NATIVES.GivePedToPauseMenu, clone, 2);
    mp.game.invoke(global.NATIVES.SetMouseCursorVisibleInMenus, 0);
    mp.game.invoke(global.NATIVES.SetPauseMenuPedLighting, 1);//
    mp.game.invoke(global.NATIVES.SetPauseMenuPedSleepState, 1);//
	mp.console.logInfo(`${flag}`)
	//mp.game.ui.setFrontendActive(false);
    //mp.console.logInfo(`show`);
}

mp.keys.bind(Keys.Key_E, false, switchVal)
mp.keys.bind(Keys.Key_RIGHT, false, devPed.nextAnim.bind(devPed))
mp.keys.bind(Keys.Key_LEFT, false, devPed.previousAnim.bind(devPed))

mp.events.add("render",()=>{
	//raycast();
	//nearObject();
	// const ppos = mp.players.local.position;
	// const interiorID = mp.game.interior.getInteriorAtCoords(ppos.x, ppos.y, ppos.z);
	// invId = interiorID;
	// global.debugText = `id: ${invId}/${mp.game.interior.isInteriorDisabled(invId)}`;
	mp.game.graphics.drawText(global.debugText, [.5, .8], {
		font: 7,
		color: [255, 255, 255, 185],
		scale: [0.4, 0.4],
		outline: true
	});
	if(mouse.state === mouseSates.release) return;
	const pos = mp.gui.cursor.position;
	mouse.dirrectX = 0;
	mouse.dirrectY = 0;
	if(pos[0] !== mouse.x){
		mouse.dirrectX = mouse.x - pos[0] > 0 ? 1 : -1;
		mouse.x  = pos[0];
	}
	if(pos[1] !== mouse.y){
		mouse.dirrectY = mouse.y - pos[1] > 0 ? 1 : -1;
		mouse.y  = pos[1];
	}
	//onPedMove();
	//onPedMove2();
	//devAttach.onMouseMove(mouse);
	//onCameraMove();
	//devObject.onMouseMove(mouse);
});


mp.events.add("cSetProp", (data) =>
{
    let objs = JSON.parse(data)
    let pos = objs.position
	let prop = objs.prop
	let mode = objs.mode
	let interiorID = mp.game.interior.getInteriorAtCoords(pos[0], pos[1], pos[2])
	if(interiorID > 0){
		if(mode > 0){
			mp.game.interior.enableInteriorProp(interiorID, prop)
		}else{
			mp.game.interior.disableInteriorProp(interiorID, prop)
		}
		mp.game.interior.refreshInterior(interiorID)
	}else{
		mp.events.callRemote("sLogIt","Prop: error ID")
	}
})

const lathe = {
	dict: "anim@amb@machinery@lathe@",
	anims:[
		"adjust_handle_amy_skater_01",
		"adjust_handle_gr_prop_gr_lathe_01c",
		"base_amy_skater_01",
		"base_gr_prop_gr_2stackcrate_01a",
		"base_gr_prop_gr_lathe_01c",
		"base_gr_prop_gr_part_lathe_01a",
		"clean_surface_01_amy_skater_01",
		"clean_surface_01_gr_prop_gr_lathe_01c",
		"clean_surface_02_amy_skater_01",
		"clean_surface_02_gr_prop_gr_lathe_01c",
		"clean_surface_03_amy_skater_01",
		"clean_surface_03_gr_prop_gr_2stackcrate_01a",
		"clean_surface_03_gr_prop_gr_lathe_01c",
		"load_01_amy_skater_01",
		"load_01_gr_prop_gr_2stackcrate_01a"
	],
	model: "gr_prop_gr_lathe_01c",
	position: null,
	object: null,
	current: 0,
	check(){
		if(this.object == null){
			this.position = mp.players.local.getOffsetFromInWorldCoords(0, 2 , 1.2);
			this.object = mp.objects.new(mp.game.joaat("gr_prop_gr_lathe_01c"), this.position, {dimension: mp.players.local.dimension});
		} 
		this.playAnim();
	},
	playAnim(){
		this.loadDict();

		//const obj = mp.objects.new(mp.game.joaat(gr_prop_gr_lathe_01c), this.position, {dimension: mp.players.local.dimension});
		//this.object.playAnim(this.anims[0], this.dict, 1000, true, true, false, 0, 5642);

		mp.players.local.taskPlayAnimAdvanced(this.dict, this.anims[this.current], this.position.x, this.position.y, this.position.z, 0, 0, -40, 8, 1, -1, 5642, 0, 2, 1);
		this.current += 1; 
		if(this.current == this.anims.length) this.current = 0;
	},
	loadDict(){
		if(!mp.game.streaming.hasAnimDictLoaded(this.dict)){            
			mp.game.streaming.requestAnimDict(this.dict);
			for (let index = 0;!mp.game.streaming.hasAnimDictLoaded(this.dict) && index < 100; index++) {
				mp.game.wait(10);
			};
        }
	}
}

mp.events.add("devattach", (dictionary, name, flag, boneId, attachs)=>{
	devAttach.create(dictionary, name, flag, boneId, attachs);
		cmeraSwitch(true);
})

mp.events.add("customCameraOff", ()=>{
		cmeraSwitch(false);
})

mp.events.add("dev:ped:create", (configString)=>{	
	const config = JSON.parse(configString);
	if(config.model !== targetEntity.model) return;
	devPed.create(targetEntity, config);
})

mp.events.add("dev:ped:create:2", ()=>{	
	devPed2.create(mp.players.local.getOffsetFromInWorldCoords(0, 1, 0));
})

mp.events.add("dev:object:create", (model)=>{
	devObject.createObject(model);
})

const commands = {
	eval(code){
		eval(code);
	}
}

mp.events.add("consoleCommand", (command) => {
    const split = command.split(' ');
	const fnc= commands[split[0]];
	if(fnc !== undefined){
		split.splice(0,1);
		fnc(split.join(' '));
	}else
		console.logInfo(`Command ${split[0]} not found`);
});

global.setAmmoLocal = (count)=>{
	console.logInfo(`weapon: ${mp.players.local.weapon}`);
	mp.players.local.setAmmoInClip(mp.players.local.weapon, count);
}

let obj = null;
mp.events.add("ironcheck", async (model) => {
	const pos = mp.players.local.position;
	if(obj !== null){
		obj.destroy();
		obj = null;
		return;
	}
    obj = mp.objects.new(mp.game.joaat(model), new mp.Vector3(pos.x + Math.random() - 0.5, pos.y + Math.random() - 0.5, pos.z + 0.1), {
		alpha: 255,
		dimension: 0,
		rotation: new mp.Vector3(0, 0, 0)
	});
	while (!obj.doesExist()) {
		await mp.game.waitAsync(0);
	}

	obj.setDynamic(true);
	//obj.setActivatePhysicsAsSoonAsItIsUnfrozen(true);
	obj.setCollision(true, true);
	//obj.setHasGravity(true);
	obj.freezePosition(false);
});