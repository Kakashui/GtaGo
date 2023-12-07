//dialogCallback

mp.events.add("dthscrtimer",(time)=>{
    global.gui.setData('deathScreen/updateTime', time);
})

mp.events.add("dthscr",(medics)=>{
    global.gui.setData('deathScreen/showBtns', medics);
    global.gui.setData('deathScreen/setTime', 180);
    global.gui.close();
    setTimeout(()=>{ 
        global.showHud(false);
        mp.gui.chat.show(true);
        //global.showCursor(true);
    }, 500)
})

mp.events.add("dthscrclose",()=>{
    global.gui.close();
    mp.game.cam.doScreenFadeOut(0);
	global.customCamera.switchOff(0);
	setTimeout(()=>{
		mp.game.cam.doScreenFadeIn(1000);
	}, 1000)
    global.gui.setData('deathScreen/close');
    global.showCursor(false);
})

let sf;
let dethOn = false;
let cameraPos = new mp.Vector3();
let controlBegine = Date.now();

const deathDict = "dead";
const deathAnims = ["dead_a","dead_b","dead_c","dead_d","dead_e","dead_f","dead_g","dead_h"];
const playersInDeath = [];

function deathHandler(inDeath){
	try {
        if(dethOn === inDeath) return;
		if(inDeath == true){
			//sf = mp.game.graphics.requestScaleformMovie("mp_big_message_freemode");
			// while(!mp.game.graphics.hasScaleformMovieLoaded(sf))
			// 	mp.game.wait(0);
			controlBegine = Date.now() + 3000;
			// mp.game.graphics.pushScaleformMovieFunction(sf, "SHOW_SHARD_WASTED_MP_MESSAGE");
			// mp.game.graphics.pushScaleformMovieFunctionParameterString("~r~WASTED");
			// mp.game.graphics.pushScaleformMovieFunctionParameterString("");
			// mp.game.graphics.pushScaleformMovieFunctionParameterInt(5);
			// mp.game.graphics.popScaleformMovieFunctionVoid();
			cameraPos = mp.players.local.position;
			cameraPos.z += 1.2;			
			global.customCamera.setPos(cameraPos);
			global.customCamera.setPoint(mp.players.local.position);
			global.customCamera.switchOn(2500);
			mp.game.audio.playSoundFrontend(-1, "Bed", "WastedSounds", true);
			mp.game.graphics.setTimecycleModifier("MP_death_grade");
		}else{
			mp.game.cam.doScreenFadeOut(0);
			global.customCamera.switchOff(0);
			mp.game.graphics.setTimecycleModifier("default");
			setTimeout(()=>{
				mp.game.cam.doScreenFadeIn(1000);
			}, 500)
		}
		dethOn = inDeath;
		
	} catch (e) {
		mp.serverLog(`scaleform: ${e.message}`);
	}
}


mp.events.addDataHandler("InDeath", (entity, isDeath) => {
	if(isDeath){
		entity.currentDeathAnim = deathAnims[Math.floor(Math.random()*deathAnims.length)];
		playersInDeath.push(entity);
	}else{
		const index = playersInDeath.findIndex(p=>p === entity);
		if(index !== -1){
			if(playersInDeath[index] && playersInDeath[index].doesExist())
				playersInDeath[index].clearTasksImmediately();

			playersInDeath.splice(index, 1);
		}
	}
    if (entity === mp.players.local)
        deathHandler(isDeath);
});

mp.events.add('render', () => {   
	try{	
		if(dethOn){
			//mp.game.graphics.drawScaleformMovieFullscreen(sf, 255, 255, 255, 255, false);
			if(Date.now() > controlBegine){
				const dir = global.gameplayCam.getDirection();
				const point = new mp.Vector3(cameraPos.x + dir.x, cameraPos.y + dir.y, cameraPos.z + dir.z);	
				global.customCamera.setPoint(point);
				global.customCamera.update();
			}
		}		
	} catch (e) {
		mp.serverLog(`deathscreen.render: ${e.message}`);
	}
})

setInterval(() => {
	playersInDeath.forEach(player => {
		if(player && player.doesExist() && player.currentDeathAnim){
			if(!player.isPlayingAnim(deathDict, player.currentDeathAnim, 3))
				player.taskPlayAnim(deathDict, player.currentDeathAnim, 8.0, 8.0, -1, 1, 0, false, false, false);
		}
	});
}, 500);
