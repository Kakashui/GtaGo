let oldDimension = 0;


let pedsaying = null;
let pedtext = "";
let pedtext2 = null;


let lastEntCheck = 0;
let checkInterval = 200;
let backlightColor = [196, 17, 21];
let blockcontrols = false;
let hasmoney = false;
let isInSafeZone = -1;
let safeZones = {[-1]: false};
let lastCuffUpdate = Date.now();

const animalModels = [
    1125994524,
    2910340283,
    1126154828,
    1832265812,
    1318032802,
    351016938,
]

const pokerTableHash = mp.game.joaat("poker_table_gta_go");

function getLookingAtEntity() {
    let startPosition = global.localplayer.getBoneCoords(12844, 0.5, 0, 0);
    let resolution = mp.game.graphics.getScreenActiveResolution(1, 1);
    let secondPoint = mp.game.graphics.screen2dToWorld3d([resolution.x / 2, resolution.y / 2, (2 | 4 | 8)]);
    if (secondPoint == undefined) return null;

    startPosition.z -= 0.3;
    const result = mp.raycasting.testPointToPoint(startPosition, secondPoint, global.localplayer, (2 | 4 | 8 | 16));

    if (typeof result !== 'undefined') {
        // для проверки на педы. Если наведен на педа, то testPointToPoint
        // возвращает raycast, где .entity - число
        // в остальных случаях .entity - это EntityMp
        if (typeof result.entity === 'number') {
            if (global.isHandlePet(result.entity)) {
                return mp.peds.atHandle(result.entity);
            }
        }
        else {
            if (typeof result.entity.type === 'undefined') return null;
        
            let entPos = result.entity.position;
            let lPos = global.localplayer.position;
            if (mp.game.gameplay.getDistanceBetweenCoords(entPos.x, entPos.y, entPos.z, lPos.x, lPos.y, lPos.z, true) > 8) return null;
            
            if(result.entity.model == pokerTableHash) {
                global.updatePokerLable(result.entity.handle);
                return null;
            }
            //if (result.entity.type == 'object' && result.entity.getVariable('data:object:id') == undefined) return null;

            return result.entity;
        }
    }
    return null;
}

function getNearestObjects() {
    try {
        let tempO = null;
        let tempODist = null;
        if  (global.localplayer.isInAnyVehicle(false)) {
            mp.players.forEach((player) => {
                if(!player) return;
                let posL = global.localplayer.position;
                let posO = player.position;
                let distance = mp.game.gameplay.getDistanceBetweenCoords(posL.x, posL.y, posL.z, posO.x, posO.y, posO.z, true);

                if  (global.localplayer != player && global.localplayer.dimension === player.dimension && distance < 2) {
                    if (tempO === null || distance < tempODist) {
                        tempO = player;
                        tempODist = distance;
                    }
                }
            });
        }
        else {
            mp.objects.forEach((object) => {
                if(!object) return;
                let posL = global.localplayer.position;
                let posO = object.position;

                if (object.getVariable('data:object:id') != undefined && global.localplayer.dimension === object.dimension ) {
                    let distance = mp.game.gameplay.getDistanceBetweenCoords(posL.x, posL.y, posL.z, posO.x, posO.y, posO.z, true);
                
                    if (distance < 3) {
                        if (tempO === null || distance < tempODist) {
                            tempO = object;
                            tempODist = distance;
                        }
                    }   
                }
            });
        }
        global.nearestObject = tempO;
    } catch (e) {
        if(global.sendException)mp.serverLog(`getNearestObjects: ${e.name }\n${e.message}\n${e.stack}`);
    }
}

let lastArmour = 0;
function checkArmorPlayer(){
    try {
        const armour = mp.players.local.getArmour();
        if(lastArmour !== armour){
            lastArmour = armour;
            mp.events.callRemote("equip:armor:check", armour)
        }
    } catch (e) {
        if(global.sendException)mp.serverLog(`checkArmorPlayer: ${e.name }\n${e.message}\n${e.stack}`);
	}    
}

setInterval(checkArmorPlayer, 2500)

mp.events.add('blockMove', function (argument) {
    blockcontrols = argument;
});

mp.events.add('CUFFED', function (argument) {
    global.cuffed = argument;
    
    if (argument) {
        mp.game.invoke(global.getNative("SET_FOLLOW_PED_CAM_VIEW_MODE"), 4);
    }
});

mp.events.add('hasMoney', function (argument) {
    hasmoney = argument;
    if (!argument) global.localplayer.setEnableHandcuffs(false);
});

mp.events.add('safeZone', function (id, active) {
    if (safeZones[id] === undefined)
        safeZones[id] = true;
    if (active){
        isInSafeZone = id;
    }
    else if (isInSafeZone == id){//чтоб не деактивировать другую зону, если они рядом находятся
        isInSafeZone = -1;
    }
    updateGreenZoneHud();
});

mp.events.add('safeZones:setInActiveZones', function (offZones) {
    offZones.forEach(id => {
        safeZones[id] = false;
    });
    updateGreenZoneHud();
});

mp.events.add('safeZones:setActiveZone', function (id, active) {
    safeZones[id] = active;
    updateGreenZoneHud();
});

function updateGreenZoneHud() {
    let inZZ = (isInSafeZone > 0 && safeZones[isInSafeZone] === true) == true;
    global.gui.setData('hud/toggleGreenZone', inZZ);
    global.inGreenZone = inZZ;
}

mp.keys.bind(global.Keys.Key_E, false, function () { // Interaction key
    if ((gui.isOpened() || global.localplayer.getVariable('InDeath') == true && !global.localplayer.isInAnyVehicle(false)) || global.IsPlayingDM == true ||  global.cuffed) return;
    if (global.circleOpen) {
        global.CloseCircle();
        return;
    }
    if (!global.loggedin || global.chatActive || Date.now() < global.lastCheck) return;
    if (global.cursorTarget)
    {
        if (global.inAction && global.cursorTarget.type !== "vehicle") return;
        switch (global.cursorTarget.type) {
            case "object":
                if (global.cursorTarget && mp.objects.exists(global.cursorTarget)) {
                    mp.events.callRemote('objectInteracted', global.cursorTarget);
                }
                global.cursorTarget = null;
                break;
            case "player":
                global.showCursor(true)
                mp.events.call('playerInteractedLocal', global.cursorTarget, 0);
                break;
            case "vehicle":
                global.showCursor(true)
                mp.events.call('playerInteractedLocal', global.cursorTarget, 1);
                //global.OpenCircle('Машина', 0);
                break;
            case "ped":
                if (!animalModels.includes(global.cursorTarget.model))
                    break;
                mp.events.call('playerInteractedLocal', global.cursorTarget, 2);
                //global.OpenCircle('Питомец', 0);
                break;
        }
    }
    
    else if (global.nearestObject != null && mp.objects.exists(global.nearestObject) && global.nearestObject.type == "object") 
        mp.events.callRemote('objectInteracted', global.nearestObject);
    global.lastCheck = Date.now() + 500;
});

let truckorderveh = null;

mp.events.add('SetOrderTruck', (vehicle) => {
    try {
        if(truckorderveh == null) truckorderveh = vehicle;
		else truckorderveh = null;
    } catch (e) {
        if(global.sendException)mp.serverLog(`Error in render.SetOrderTruck: ${e.name }\n${e.message}\n${e.stack}`);
	}
});

let lastCheckNearestObject = 0;
const checkNearestObjectInterval = 250;

let lastTimeLogSend = 0;
const logSendInterval = 10000;



mp.events.add('render', () => {
	try {
        if (global.localplayer.dimension != oldDimension) {
            mp.events.call("onChangeDimension", oldDimension, global.localplayer.dimension);
            oldDimension = global.localplayer.dimension;
        }
        if(mp.game.ui.isPauseMenuActive())
            mp.gui.cursor.visible = false; 
        if (global.phoneCameraIsOpened)
            mp.game.controls.disableControlAction(0, 200, true);

        // else 
        // if(mp.gui.cursor.visible !== global.cursorShow)
        //     mp.gui.cursor.visible = global.cursorShow;
            
        if(characterEditor) {
            mp.players.local.setRotation(global.editorRotation.x, global.editorRotation.y, global.editorRotation.z, 2, true);
            mp.players.local.setCoordsNoOffset(global.editorPosition.x, global.editorPosition.y, global.editorPosition.z, false, false, false);
        }
		if(pedsaying != null) {
			let pos = pedsaying.getBoneCoords(12844, 0.5, 0, 0);
			mp.game.graphics.drawText(pedtext, [pos.x, pos.y, pos.z+0.1], {
				font: 0,
				color: [255, 255, 255, 185],
				scale: [0.35, 0.35],
				outline: true
			});
			if(pedtext2 != null) {
				let pos = pedsaying.getBoneCoords(12844, 0.5, 0, 0);
				mp.game.graphics.drawText(pedtext2, [pos.x, pos.y, pos.z+0.017], {
					font: 0,
					color: [255, 255, 255, 185],
					scale: [0.35, 0.35],
					outline: true
				});
			}
		}
		if (!global.pidrgm) global.localplayer.setInvincible(false);
        mp.game.player.setLockonRangeOverride(1.5);
        mp.game.controls.disableControlAction(1, 7, true);
		// thanks to kemperrr
		if (mp.game.invoke(global.getNative('IS_CUTSCENE_ACTIVE'))) {
	        mp.game.invoke(global.getNative('STOP_CUTSCENE_IMMEDIATELY'))
		}

	    if (mp.game.invoke(global.getNative('GET_RANDOM_EVENT_FLAG'))) {
	        mp.game.invoke(global.getNative('SET_RANDOM_EVENT_FLAG'), false);
		}

		if (mp.game.invoke(global.getNative('GET_MISSION_FLAG'))) {
			mp.game.invoke(global.getNative('SET_MISSION_FLAG'), false);
		}


		if (global.pocketEnabled || global.cuffed) {
	        mp.game.controls.disableControlAction(2, 0, true);
	    }

	    if (blockcontrols) {
		    mp.game.controls.disableAllControlActions(2);
			mp.game.controls.enableControlAction(2, 30, true);
	        mp.game.controls.enableControlAction(2, 31, true);
		    mp.game.controls.enableControlAction(2, 32, true);
			mp.game.controls.enableControlAction(2, 1, true);
	        mp.game.controls.enableControlAction(2, 2, true);
		}
		if (hasmoney) {
	        global.localplayer.setEnableHandcuffs(true);
        }
        if (global.inGreenZone && global.getCurrentPlayerWeapon() != 911657153 && global.LOCAL_ADMIN_LVL == 0) {
            mp.game.controls.disableControlAction(2, 24, true);
            mp.game.controls.disableControlAction(2, 69, true);
            mp.game.controls.disableControlAction(2, 70, true);
            mp.game.controls.disableControlAction(2, 92, true);
            mp.game.controls.disableControlAction(2, 114, true);
            mp.game.controls.disableControlAction(2, 121, true);
            mp.game.controls.disableControlAction(2, 140, true);
            mp.game.controls.disableControlAction(2, 141, true);
            mp.game.controls.disableControlAction(2, 142, true);
            mp.game.controls.disableControlAction(2, 257, true);
            mp.game.controls.disableControlAction(2, 263, true);
            mp.game.controls.disableControlAction(2, 264, true);
            mp.game.controls.disableControlAction(2, 331, true);
        }
        
		if (mp.keys.isDown(32) && global.cuffed && lastCuffUpdate > Date.now()) {
			mp.events.callRemote("cuffUpdate");
	        lastCuffUpdate = Date.now() + 3000;
		}
        
		if (!global.localplayer.isInAnyVehicle(false) && !global.localplayer.isDead()) {
	        if (!global.circleOpen)
            global.cursorTarget = getLookingAtEntity();   
            if (global.cursorTarget && global.cursorTarget.getVariable && global.cursorTarget.getVariable('INVISIBLE') == true) global.cursorTarget = null;
		}
        else {
            if (global.cursorTarget !== global.nearestObject) global.cursorTarget = null;
        }
        
        if (Date.now() > lastCheckNearestObject) {
            getNearestObjects();
            lastCheckNearestObject = Date.now() + checkNearestObjectInterval;
        }

	    if (global.nearestObject != null && (global.cursorTarget == null || global.cursorTarget.type != "object")) {
            if (!mp.objects.exists(global.nearestObject)) return;
		    mp.game.graphics.drawText("•", [global.nearestObject.position.x, global.nearestObject.position.y, global.nearestObject.position.z], {
			    font: 0,
	            color: [182, 211, 0, 200],
		        scale: [0.5, 0.5],
			    outline: true
			});
		}
        else if (global.cursorTarget && !mp.players.local.isInAnyVehicle(false)) {
			if(global.cursorTarget != truckorderveh) {
                let pos = null;
                switch (global.cursorTarget.type) {
                    case "ped":
                        pos = global.cursorTarget.getCoords(true);
                        break;
                    default:
                        if(!global.cursorTarget) return;
                        pos = global.cursorTarget.position;
                        pos = new mp.Vector3(pos.x, pos.y, pos.z + 0.5);
                        break;
                }

				mp.game.graphics.drawText("•", [pos.x, pos.y, pos.z], {
					font: 0,
					color: [182, 211, 0, 200],
					scale: [0.5, 0.5],
					outline: false
				});
			}
		}
	} catch (e) {        
        if(global.sendException && Date.now() > lastTimeLogSend){
            lastTimeLogSend = Date.now() + logSendInterval;
            mp.serverLog(`Error in render.render: ${e.name }\n${e.message}\n${e.stack}`);
        } 
    }
});