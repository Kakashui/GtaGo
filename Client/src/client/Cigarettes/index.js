const Cigarette = require('./Cigarette');

const dropAnimDict = "amb@code_human_in_car_mp_actions@first_person@smoke@std@ds@base";
const dropAnimName = "exit";

let sendingExcept = false;

function cigeretteExists(player){
    if(!player) return false;
    return player.hasOwnProperty("attachedCigarette");
}

function destroyIfExists(player){
    if(cigeretteExists(player)){
        player.attachedCigarette.destroy();
        delete player.attachedCigarette;
    }
}

function createCigarette(player){
    player.attachedCigarette = new Cigarette(player);
}


mp.events.addDataHandler("attach:cigarette", (entity, newData, oldData) => {
    try {        
        if(entity.type !== 'player'|| entity.handle == 0) return;
        if(entity.handle == mp.players.local.handle && newData == false){
            mp.players.local.clearTasksImmediately();
            mp.players.local.taskPlayAnim(dropAnimDict, dropAnimName, 8, 1, -1, 52, .35,false, false, false );
            mp.game.wait(0)
        }    
        destroyIfExists(entity);
        
        if(newData) 
            createCigarette(entity);

        if(!mp.game.streaming.hasAnimDictLoaded(dropAnimDict)) 
        mp.game.streaming.requestAnimDict(dropAnimDict);
    } catch (e) {        
        if (global.sendException) mp.serverLog(`siga.addDataHandler: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("entityStreamIn", (entity)=>{
    try {
        if (entity && entity.type === 'player') {
            let cigarette = entity.getVariable('attach:cigarette');
            if(cigarette){
                destroyIfExists(entity);            
                createCigarette(entity);
            }
        }        
    } catch (e) {
        if (global.sendException) mp.serverLog(`siga.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add("entityStreamOut", (entity)=>{
    if (entity && entity.type === 'player') {
        destroyIfExists(entity);
    }
});

let rightPressed = false;
let leftPressed = false;
mp.events.add("render", ()=>{
    try {        
        if(mp.game.controls.isControlPressed(0, 25)){
            if(!rightPressed) {
                rightPressed = true;
                if(cigeretteExists(mp.players.local)){
                    mp.events.callRemote('scene:cigarette:cancel');
                };
            };
        }else{
            if(rightPressed)
                rightPressed = false;
        }

        if(mp.game.controls.isControlPressed(0, 24)){
            if(!leftPressed){
                leftPressed = true;
                if(cigeretteExists(mp.players.local)){
                    mp.events.callRemote('scene:cigarette:smoking');
                }
            }
        }else{
            if(leftPressed)
                leftPressed = false;
        }
    } catch (e) {
        if (global.sendException && !sendingExcept) {
            sendingExcept = true;
            mp.serverLog(`siga.render: ${e.name}\n${e.message}\n${e.stack}`);
        } 
    }
})
