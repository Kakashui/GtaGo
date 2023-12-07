const moveSettings = {
    size: {
        x: 35,
        y: 35
    },
    showIcons:[true, true, false, true],
    values:[
        {//LEFT X
            value: -40,
            min: -110,
            max: 30,
            step: 5,
            invert: true,
            enabled: true,
            callback: "camMoveAngleX"
        },
        {//LEFT Y
            value: 0.1,
            min: -.5,
            max: .8,
            step: .05,
            invert: false,
            enabled: true,
            callback: "camMoveCamZ"
        },
        {//RIGHT X
            value: 2,
            min: 1,
            max: 3,
            step: .1,
            invert: false,
            enabled: false,
            callback: ""
        },
        {//RIGHT Y
            value: 0,
            min: -1,
            max: 1,
            step: .05,
            invert: true,
            enabled: false,
            callback: "camMovePointZ"
        },
        { //WHEELE
            value: 1,
            min: .6,
            max: 1.2,
            step: .2,
            invert: false,
            enabled: true,
            callback: "camSetDist"
        }
    ]
}

mp.events.add('openBarber', (price)=>{
    barberScene.blocked = false;
    global.showHud(false); 
    mp.game.cam.doScreenFadeOut(0);
    setTimeout(()=>{

        let gender = global.localplayer.getVariable("GENDER");
        global.gui.setData("barbershop/setData", JSON.stringify({gender, price}));
    
        global.customCamera.setPos(new mp.Vector3(barberScene.position.x, barberScene.position.y, barberScene.position.z + 1.6));
        global.customCamera.setPoint(new mp.Vector3(barberScene.position.x, barberScene.position.y, barberScene.position.z + 1.6));
        global.customCamera.moveCamZ(.1);
        global.customCamera.setDist(1);
        global.customCamera.moveAngleX(-40);
        global.customCamera.switchOn(0);
        global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
        global.gui.setData('mouseMove/setEnebled', true);
        
        playEnterAnim();
    },1000)
})

mp.events.add('closeBarber', ()=>{
    if(barberScene.blocked) return; 
    global.gui.setData('mouseMove/setEnebled', false);
    global.gui.close();
    global.showHud(false); 
    global.customCamera.switchOff(0);
    playExitAnim();
})


mp.events.add('buyBarber', (type, style, color)=>{
    let gender = global.localplayer.getVariable("GENDER");
    if((gender && +style == 23) || (!gender && style == 24)) return; 

    if(barberScene.blocked) return;
    barberScene.blocked = true;

    mp.events.callRemote('buyBarber', type, +style, +color);
})

mp.events.add('buyBarberCallback', (type, price)=>{
    switch (+type) {
        case 1:
            mp.events.call('notify', 1, 9, "client_161", 3000);
            barberScene.blocked = false;
            break;
        case 2:
            mp.events.call('notify', 1, 9, "client_162", 3000);   
            barberScene.blocked = false;         
            break;
        case 3:
            mp.events.call('notify', 1, 9, "client_163", 3000);   
            barberScene.blocked = false;
            break;
        case 4:            
            mp.events.call('notify', 2, 9, `${"client_164"} ${price}$`, 3000);               
            playIdleAnim();
            break;       
        default:
            barberScene.blocked = false;
            break;
    }
})

mp.events.add('changeBarber', (type, style, color)=>{
    switch (type) {
        case 'hairstyle':
            const gender = global.localplayer.getVariable("GENDER");
            if((gender && +style == 23) || (!gender && style == 24)) return; 
            global.setClothing(global.localplayer, 2, +style, 0, 0);
            global.localplayer.setHairColor(+color, 0);
            break;
        case 'eyebrows':
            global.localplayer.setHeadOverlay(2, +style, 100, +color, +color);
            break;
        case 'torso':
            global.localplayer.setHeadOverlay(10, +style, 100, +color, +color);            
            break;
        case 'lenses':
            global.localplayer.setEyeColor(+style);
            break;
        case 'pomade':
            global.localplayer.setHeadOverlay(8, +style, 100, +color, +color);  
            break;
        case 'blush':
            global.localplayer.setHeadOverlay(5, +style, 100, +color, +color);  
            break;
        case 'shadows':
            global.localplayer.setHeadOverlay(4, +style, 100, 0, 0); 
            global.localplayer.setHeadOverlayColor(4, 2, +color, +color); // makeup 
            break;
        case 'beard':
            global.localplayer.setHeadOverlay(1, +style, 100, +color, +color);
            break;
    
        default:
            break;
    }
})

mp.events.add("render", ()=>{
    if(barberScene.handler.begine && barberScene.handler.condition()){
        barberScene.handler.begine = false;
        barberScene.handler.callback();
    }
})

const barberScene = {
    id: -1,
    blocked: false,
	position: new mp.Vector3(138.3647, -1709.252, 28.3182),
    dict: "misshair_shop@barbers",
    handler:{
        begine: false,
        condition(){},
        callback(){}
    },
	barber:{
		entity: null,
		model: -2109222095,
        enter: "keeper_enterchair",
        exit: "keeper_exitchair",
        base: "keeper_base",
        idle: "keeper_idle_b"
    },
    player:{
		entity: null,
		model: 0,
		enter: "player_enterchair",
        exit: "player_exitchair",
        idle: "player_idle_b"
	},
	scissors:{
		entity: null,
		model: 3070787497,
		enter: "scissors_enterchair",
        exit: "scissors_exitchair",
        idle: "scissors_idle_b"
    },
    createSceneObjects(){
        mp.game.streaming.requestModel(this.barber.model);
        mp.game.streaming.requestModel(this.scissors.model);	
        for (let index = 0;(!mp.game.streaming.hasModelLoaded(this.barber.model) || !mp.game.streaming.hasModelLoaded(this.scissors.model)) && index < 250; index++) {
            mp.game.wait(0);
        };
        
        if(this.barber.entity == null) this.barber.entity = mp.peds.newValid(this.barber.model, this.position, -40, mp.players.local.dimension)
        if(this.scissors.entity == null) this.scissors.entity = mp.objects.new(this.scissors.model, this.position, {dimension: mp.players.local.dimension});
    },
    deleteSceneObjects(){
        if(this.barber.entity != null) this.barber.entity.destroy();
        if(this.scissors.entity != null) this.scissors.entity.destroy();
        this.barber.entity = null;
        this.scissors.entity = null;
    },
    loadDict(){
        if(!mp.game.streaming.hasAnimDictLoaded(this.dict)){            
            mp.game.streaming.requestAnimDict(this.dict);
            for (let index = 0;!mp.game.streaming.hasAnimDictLoaded(this.dict) && index < 250; index++) {
                mp.game.wait(0);
            };
        }
    }
}

function playEnterAnim(){
    try {
        barberScene.loadDict();
        barberScene.createSceneObjects();

        barberScene.handler.callback = ()=>{
            global.gui.openPage("Barbershop");
        }

        barberScene.handler.condition = ()=>{
            return mp.players.local.getAnimCurrentTime(barberScene.dict, barberScene.player.enter) > .9;
        }

        barberScene.id = mp.game.ped.createSynchronizedScene(barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 2);	
        mp.game.ped.setSynchronizedSceneLooped(barberScene.id, false);
        mp.game.ped.setSynchronizedScenePhase(barberScene.id, 0);

        mp.players.local.taskPlayAnimAdvanced(barberScene.dict, barberScene.player.enter, barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 8, 1, -1, 5642, 0, 2, 1);
        barberScene.scissors.entity.playSynchronizedAnim(barberScene.id, barberScene.scissors.enter, barberScene.dict, 1000, 8, 0, 1000);	
        barberScene.barber.entity.taskPlayAnimAdvanced(barberScene.dict, barberScene.barber.enter, barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 8, 1, -1, 5642, 0, 2, 1);

        mp.game.cam.doScreenFadeIn(100);
        barberScene.handler.begine = true;
    } catch (e) {
        if(global.sendException) mp.serverLog(`Error in basesync.detachObject: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function playIdleAnim(){
    try {
        barberScene.createSceneObjects();

        barberScene.handler.callback = ()=>{
            barberScene.blocked = false;
        }

        barberScene.handler.condition = ()=>{
            return mp.players.local.getAnimCurrentTime(barberScene.dict, barberScene.player.idle) > .9;
        }

        barberScene.id = mp.game.ped.createSynchronizedScene(barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 2);	
        mp.game.ped.setSynchronizedSceneLooped(barberScene.id, false);
        mp.game.ped.setSynchronizedScenePhase(barberScene.id, 0);

        mp.players.local.taskPlayAnimAdvanced(barberScene.dict, barberScene.player.idle, barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 8, 1, -1, 5642, 0, 2, 1);
        barberScene.scissors.entity.playSynchronizedAnim(barberScene.id, barberScene.scissors.idle, barberScene.dict, 1000, 8, 0, 1000);	
        barberScene.barber.entity.taskPlayAnimAdvanced(barberScene.dict, barberScene.barber.idle, barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 8, 1, -1, 5642, 0, 2, 1);

        barberScene.handler.begine = true;
    } catch (error) {
        if(global.sendException) mp.serverLog(`Error in basesync.detachObject: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function playExitAnim(){
    try {
        barberScene.createSceneObjects();

        barberScene.handler.callback = ()=>{
            mp.events.callRemote('closeBarber');
            barberScene.deleteSceneObjects();
            mp.players.local.clearTasksImmediately();
            mp.players.local.setCollision(true, true);
        }

        barberScene.handler.condition = ()=>{
            return mp.players.local.getAnimCurrentTime(barberScene.dict, barberScene.player.exit) > .9;
        }

        barberScene.id = mp.game.ped.createSynchronizedScene(barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 2);	
        mp.game.ped.setSynchronizedSceneLooped(barberScene.id, false);
        mp.game.ped.setSynchronizedScenePhase(barberScene.id, 0);

        mp.players.local.taskPlayAnimAdvanced(barberScene.dict, barberScene.player.exit, barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 8, 1, -1, 5642, 0, 2, 1);
        barberScene.scissors.entity.playSynchronizedAnim(barberScene.id, barberScene.scissors.exit, barberScene.dict, 1000, 8, 0, 1000);	
        barberScene.barber.entity.taskPlayAnimAdvanced(barberScene.dict, barberScene.barber.exit, barberScene.position.x, barberScene.position.y, barberScene.position.z, 0, 0, -40, 8, 1, -1, 5642, 0, 2, 1);        
        global.showHud(true);
        barberScene.handler.begine = true;
    } catch (error) {
        if(global.sendException) mp.serverLog(`Error in basesync.detachObject: ${e.name}\n${e.message}\n${e.stack}`);
    }
}
