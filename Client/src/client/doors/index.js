const doorConfigs = require('./configs')

const doors = [];
const actionDistance = 1.3;
const drawDistance = 2;
const size = .08;

let door = null;
let lastClick = Date.now();
const spamProtectionTime = 750;

let sendingExcept = false;

function updateDoorState(doors, state){
    doors.forEach(d => {
        mp.game.object.doorControl(d.Hash, d.Position.x, d.Position.y, d.Position.z, state, 0.0, 0.0, 0.0);
    });
}

function getClothestDoor(){
    return doors.find(d=> mp.game.gameplay.getDistanceBetweenCoords(d.point.x, d.point.y, d.point.z, mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z, true) < drawDistance);
}

function getDistance(){
    if(!door) return -1;
    return mp.game.gameplay.getDistanceBetweenCoords(door.point.x, door.point.y, door.point.z, mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z, false)
}

// doorConfigs.forEach((door) =>
// {
//     updateDoorState(door.Doors, door.Locked);
//     if(door.Interract){
//         try {
//             doors.push({
//                 hash: door.Hash,
//                 name: door.Name,
//                 point: new mp.Vector3(door.ActionPoint.x, door.ActionPoint.y, door.ActionPoint.z),
//                 locked: door.Locked,               
//                 doors: door.Doors,
//                 defaultState: door.DefaultState
//             })
//         } catch (e) {
//             if(global.sendException) mp.serverLog(`doorConfigs.foreach: ${e.name}\n${e.message}\n${e.stack}`);
//         }
//     } 
// });

mp.keys.bind(global.Keys.Key_E, false, ()=>{
    if(lastClick > Date.now()) return;
    lastClick = Date.now() + spamProtectionTime;
    const distance = getDistance();
    if(distance > 0 && distance < actionDistance){
        mp.events.callRemote("doors:action:state", door.hash, !door.locked)
    }
});

mp.events.add("doors:state:set", (hash, state)=>{
    try {
        const targetDoor = doors.find(d=>d.hash === hash);
        if(targetDoor) {
            updateDoorState(targetDoor.doors, state == true);
            targetDoor.locked = (state == true);
        }
    } catch (e) {
        if(global.sendException) mp.serverLog(`doors:state:set: ${e.name}\n${e.message}\n${e.stack}`);
    }
})

mp.events.add("doors:state:sync", (states)=>{
    doorConfigs.forEach((door) =>
    {
        updateDoorState(door.Doors, door.Locked);
        if(door.Interract){
            try {
                doors.push({
                    hash: door.Hash,
                    name: door.Name,
                    point: new mp.Vector3(door.ActionPoint.x, door.ActionPoint.y, door.ActionPoint.z),
                    locked: door.Locked,               
                    doors: door.Doors,
                    defaultState: door.DefaultState
                })
            } catch (e) {
                if(global.sendException) mp.serverLog(`doorConfigs.foreach: ${e.name}\n${e.message}\n${e.stack}`);
            }
        } 
        mp.game.wait(0);
    });
    try {        
        if(!states) return;
        if(states.length > 0){
            states.forEach(hash => {        
                const targetDoor = doors.find(d=>d.hash == hash);
                if(targetDoor) {
                    targetDoor.locked = !targetDoor.defaultState;
                    updateDoorState(targetDoor.doors, targetDoor.locked);
                }
            });
        }
    } catch (e) {
        if(global.sendException) mp.serverLog(`doors:state:sync: ${e.name}\n${e.message}\n${e.stack}`);
    }
})

mp.events.add("playerCommand", (command) => {
    try {
        
	const args = command.split(/[ ]+/);
	const commandName = args[0].toLocaleLowerCase();
    const accessName = args[1];
    if(commandName === "doorcmd"){
        mp.gui.chat.push('dooracc, doorcheck, doorname');
        return
    }        
    if(!commandName || !door) return;		
	if (commandName === "dooracc"){
        if(!accessName) return;
		mp.events.callRemote("doors:access:add", door.hash, accessName);
    }
	else if(commandName === "doorcheck")
        mp.events.callRemote("doors:access:check", door.hash);
    else if(commandName === "doorname")
        mp.gui.chat.push(door.name);
    } catch (e) {
        if(global.sendException) mp.serverLog(`doors.playerCommand: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

setInterval(() => {    
    door = getClothestDoor();
}, 250);
    
if (!mp.game.graphics.hasStreamedTextureDictLoaded("mpsafecracking")) {
    mp.game.graphics.requestStreamedTextureDict("mpsafecracking", true);
}


mp.events.add("render", ()=>{
    try {
        if(door){
            const screen = mp.game.graphics.world3dToScreen2d(door.point.x, door.point.y, door.point.z);
            if(!screen) return;
            //const distace = ((drawDistance - getDistance()) * .025);
            const distace = Math.min(size / getDistance(), size);
            mp.game.graphics.drawSprite("mpsafecracking", door.locked ? "lock_closed" : "lock_open", screen.x, screen.y, distace * .6, distace, 0, 255, 255, 255, 100);
        }
    } catch (e) {
        if (global.sendException && !sendingExcept) {
            sendingExcept = true;
            mp.serverLog(`doorConfigs.foreach: ${e.name}\n${e.message}\n${e.stack}`);
        } 
    }
});