
const flyCamera = mp.cameras.new('default',new mp.Vector3(0, 10, 90) , new mp.Vector3(-95, 19, 0), 50);
const controls = mp.game.controls;
const gameplayCam = mp.cameras.new('gameplay');

global.fly = {
    flying: false, 
    f: 2.0, 
    w: 2.0, 
    h: 2.0, 
    point_distance: 1000, 
    pos: new mp.Vector3(0,0,0)
};

mp.events.add("AGM", (toggle) => {
	global.pidrgm = toggle == true;
	mp.players.local.setInvincible(toggle = true);
	// mp.game.graphics.notify(toggle ? 'GM: ~g~Enabled' : 'GM: ~r~Disabled');
});

const controlsIds = {
    W: 32,
    S: 33,
    A: 34,
    D: 35, 
    Space: 321,
    LCtrl: 326,
    LMB: 24,
	RMB: 25
};

function flyOn(toggle)
{
    try {
        flyCamera.setActive(true);
        var pos = new mp.Vector3(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z + .8);
        global.fly.pos = pos;
        flyCamera.setCoord(fly.pos.x, fly.pos.y, fly.pos.z);
        mp.game.cam.renderScriptCams(true, false, 0, true, false);    
        mp.events.callRemote('FlyToggle', true, global.fly.pos.z);
        global.fly.flying = toggle;
        setTimeout(()=>{
            mp.players.local.setInvincible(true);
            mp.players.local.freezePosition(true);
            mp.players.local.setCollision(false, false);
        }, 100);
    } catch (e) {
        mp.serverLog(`flyOn ${e.message}`);
    }
}

function flyOff(){
    try {
        flyCamera.setActive(false);
        if(!global.pidrgm) mp.players.local.setInvincible(false);
        mp.players.local.freezePosition(false);
        mp.players.local.setCollision(true, true);

        if (!controls.isControlPressed(0, controlsIds.Space)) {
            global.fly.pos.z = mp.game.gameplay.getGroundZFor3dCoord(global.fly.pos.x, global.fly.pos.y, global.fly.pos.z, 0.0, false);
            mp.players.local.setCoordsNoOffset(global.fly.pos.x, global.fly.pos.y, global.fly.pos.z, false, false, false);
        }else{
            mp.players.local.setCoordsNoOffset(global.fly.pos.x, global.fly.pos.y, global.fly.pos.z, false, false, false);
        } 
        mp.game.cam.renderScriptCams(false, false, 0, true, false);
        mp.events.callRemote('FlyToggle', false, global.fly.pos.z);
        global.fly.flying = false;
        mp.game.invoke (global.NATIVES.RESET_FOCUS_AREA);
    } catch (e) {
        mp.serverLog(`flyOff ${e.message}`);
    }    
}

let oldCamPos = new mp.Vector3();

let lastTimeLogSend = 0;
const logSendInterval = 1000;
mp.events.add('render', () => {
    try {
        if (global.fly.flying || global.spectating) {        
            const fly = global.fly;
            const camDir = gameplayCam.getDirection();       
            let updated = false;
            let speed;
            if(!global.spectating){
                if(controls.isControlPressed(0, controlsIds.LMB)) speed = .7
                else if(controls.isControlPressed(0, controlsIds.RMB)) speed = 0.03
                else speed = 0.1
                if (controls.isControlPressed(0, controlsIds.W)) {
                    if (fly.f < 8.0) fly.f *= 1.025;
                    fly.pos.x += camDir.x * fly.f * speed;
                    fly.pos.y += camDir.y * fly.f * speed;
                    fly.pos.z += camDir.z * fly.f * speed;
                    updated = true;
                } else if (controls.isControlPressed(0, controlsIds.S)) {
                    if (fly.f < 8.0) fly.f *= 1.025;
                    fly.pos.x -= camDir.x * fly.f * speed;
                    fly.pos.y -= camDir.y * fly.f * speed;
                    fly.pos.z -= camDir.z * fly.f * speed;
                    updated = true;
                } else fly.f = 2.0;
                if (controls.isControlPressed(0, controlsIds.A)) {
                    if (fly.l < 8.0) fly.l *= 1.025;
                    fly.pos.x += (-camDir.y) * fly.l * speed;
                    fly.pos.y += camDir.x * fly.l * speed;
                    updated = true;
                } else if (controls.isControlPressed(0, controlsIds.D)) {
                    if (fly.l < 8.0) fly.l *= 1.05;
                    fly.pos.x -= (-camDir.y) * fly.l * speed;
                    fly.pos.y -= camDir.x * fly.l * speed;
                    updated = true;
                } else fly.l = 2.0;
                if (controls.isControlPressed(0, controlsIds.Space)) {
                    if (fly.h < 8.0) fly.h *= 1.025;
                    fly.pos.z += fly.h * speed;
                    updated = true;
                } else if (controls.isControlPressed(0, controlsIds.LCtrl)) {
                    if (fly.h < 8.0) fly.h *= 1.05;
                    fly.pos.z -= fly.h * speed;
                    updated = true;
                } else fly.h = 2.0;
                if (updated) {            
                    flyCamera.setCoord(fly.pos.x, fly.pos.y, fly.pos.z);
                }
            }
            if(global.spectating){
                if(global.sptarget && mp.players.exists(global.sptarget)){
                    const pos = new mp.Vector3(global.sptarget.position.x + 7, global.sptarget.position.y, global.sptarget.position.z + 2.3);
                    
                    if(fly.pos !== pos){
                        fly.pos = pos;
                        flyCamera.setCoord(fly.pos.x, fly.pos.y, fly.pos.z);
                    } 
                    flyCamera.pointAtCoord( global.sptarget.position.x,  global.sptarget.position.y,  global.sptarget.position.z);
                }else mp.events.callRemote("UnSpectate");                
            }else 
                flyCamera.pointAtCoord(fly.pos.x + camDir.x, fly.pos.y + camDir.y, fly.pos.z + camDir.z);

            mp.players.local.setCoordsNoOffset(fly.pos.x, fly.pos.y, -150.1, true, true, true);
            mp.game.streaming.setFocusArea(fly.pos.x, fly.pos.y, fly.pos.z, 0, 0, 0);
        }
    } catch (e) {
        if(global.sendException && Date.now() > lastTimeLogSend){
            lastTimeLogSend = Date.now() + logSendInterval;
            mp.serverLog(`newFly.render: ${e.name }\n${e.message}\n${e.stack}`);
        } 
    }
    
});

mp.keys.bind(global.Keys.Key_F11, false, function () {
    if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1) return;
    if(global.spectating){
        global.fly.flying = true;
        mp.events.callRemote("UnSpectate");
    }else{
        if(global.fly.flying) 
            flyOff()
        else 
            flyOn(true);
    }    
});

mp.events.add("admin:fly:pos", (x, y, z)=>{
    fly.pos = new mp.Vector3(x, y, z + .8);
    flyCamera.setCoord(fly.pos.x, fly.pos.y, fly.pos.z);
});

mp.events.add("spmode", (target, toggle) => {
    try {
        mp.players.local.freezePosition(toggle);
        if (toggle) {
            if (target && mp.players.exists(target)) {
                global.sptarget = target;
                global.spectating = true;
                if(!global.fly.flying){
                    flyOn(false);
                }else flyCamera.setCoord(fly.pos.x, fly.pos.y, fly.pos.z);
            } else mp.events.callRemote("UnSpectate");
        } else {
            global.sptarget = null;
            global.spectating = false;
            if(!global.fly.flying){
                flyOff();
            }
        }
    } catch (e) {
        if(global.sendException)
            mp.serverLog(`newFly.spmode: ${e.name }\n${e.message}\n${e.stack}`);
    }
	
});