/*
    Animation: {
        Name: string,
        Dictionary: string,
        Flag: int
    }
*/

const preloadAnimDictionary = (dictionary) => {
    if (!mp.game.streaming.doesAnimDictExist(dictionary)) return;
    
    mp.game.streaming.requestAnimDict(dictionary);
    for (let index = 0;(!mp.game.streaming.hasAnimDictLoaded(dictionary) && index < 250); index++) {
        mp.game.wait(0);
    }

    return;
}

const onAnimationDataChange = (entity, currentAnimation) => {
    try{
        if (entity.handle === 0 || (entity.type !== 'player' && entity.type !== 'ped')) return;
        if (!currentAnimation || currentAnimation === null) {
            if (entity._oldAnimation && entity._oldAnimation !== null) {
                const { Name, Dictionary, Flag } = entity._oldAnimation;
                if (Flag !== 39)
                    entity.stopAnimTask(`${Dictionary}`, `${Name}`, Flag);
            }

            if (!entity.isInAnyVehicle(true)) 
                entity.clearTasksImmediately();
        }else{
            currentAnimation = JSON.parse(currentAnimation);
            const { Name, Dictionary, Flag } = currentAnimation;

            preloadAnimDictionary(`${Dictionary}`);
            entity.taskPlayAnim(`${Dictionary}`, `${Name}`, 8.0, 0, -1, Flag, 0, false, false, false);

            entity._oldAnimation = currentAnimation;
        }
        if(mp.players.local === entity){
            global.inAction = (currentAnimation !== null);
        }
    }
    catch (e) {
        if(global.sendException) mp.serverLog(`animSync.onAnimationDataChange: ${e.name}\n${e.message}\n${e.stack}`);
    }
};

mp.events.addDataHandler('animSync:animation', onAnimationDataChange);

//mood
const moods = [null, "mood_aiming_1", "mood_angry_1", "mood_drunk_1", "mood_happy_1", "mood_injured_1", "mood_stressed_1"];
function SetMood(entity, mood) {
    try {
        if (mood == null) entity.clearFacialIdleAnimOverride();
        else mp.game.invoke('0xFFC24B988B938B38', entity.handle, mood, 0);
    } catch (e) {
        if(global.sendException) mp.serverLog(`animSync.SetMood: ${e.name}\n${e.message}\n${e.stack}`);
    }
}
mp.events.addDataHandler('playermood', (player, mood) => {
    try {
        if (!player || !mp.players.exists(player)) return;
        
        SetMood(player, moods[mood]);
    } catch (e) {
        if(global.sendException) mp.serverLog(`animSync.addDataHandler: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

//walkstyles
mp.game.streaming.requestClipSet("move_m@brave");
mp.game.streaming.requestClipSet("move_m@confident");
mp.game.streaming.requestClipSet("move_m@drunk@verydrunk");
mp.game.streaming.requestClipSet("move_m@shadyped@a");
mp.game.streaming.requestClipSet("move_m@sad@a");
mp.game.streaming.requestClipSet("move_f@sexy@a");
mp.game.streaming.requestClipSet("move_ped_crouched");
mp.game.streaming.requestClipSet("move_ped_crouched_strafing");
const walkstyles = [null, "move_m@brave", "move_m@confident", "move_m@drunk@verydrunk", "move_m@shadyped@a", "move_m@sad@a", "move_f@sexy@a", "move_ped_crouched"];
function SetWalkStyle(entity, walkstyle) {
    try {
        if (walkstyle == null || walkstyle == 0) 
        {
            entity.resetMovementClipset(0.25);            
            entity.resetStrafeClipset();
        }
        else 
        {
            entity.setMovementClipset(walkstyle, 0.25);
            
            if (walkstyle === "move_ped_crouched")
                entity.setStrafeClipset("move_ped_crouched_strafing");
            else       
                entity.resetStrafeClipset();
        }
    } catch (e) {
        if(global.sendException) mp.serverLog(`animSync.SetWalkStyle: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

mp.events.addDataHandler('playerws', (player, walkstyle) => {
    try {
        if (!player || !mp.players.exists(player)) return;

        SetWalkStyle(player, walkstyles[walkstyle]);
    } catch (e) {
        if(global.sendException) mp.serverLog(`animSync.Player_SetWalkStyle: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add('entityStreamIn', (entity) => {
    try {
        if (!entity || entity.type !== 'player' ) return;
        onAnimationDataChange(entity, entity.getVariable('animSync:animation'));
		SetWalkStyle(entity, walkstyles[global.getVariable(entity, 'playerws', 0)]);
		SetMood(entity, moods[global.getVariable(entity, 'playermood', 0)]);		
		if (global.getVariable(entity, 'INVISIBLE', false) == true) entity.setAlpha(0);
		else entity.setAlpha(255);
    } catch (e) { 
        if(global.sendException) mp.serverLog(`animSync.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);
    }
});
