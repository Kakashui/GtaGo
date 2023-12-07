const Scene = require('./Scene')
const scenes = {};
let lastCheck = 0;

function isSpam(){
    if(lastCheck > Date.now()) return true;
    lastCheck = Date.now() + 500;
    return false;
}

function localScene() {
    return scenes[mp.players.local.remoteId];
}

function tick(){
    const forDelete = [];
    for (const key in scenes) {
        if(scenes[key].isCompleeted()){
            scenes[key].destroy();
            forDelete.push(key)
        }
    }
    
    if(forDelete.length > 0){
        forDelete.forEach(key => {
            delete scenes[key];
        });
    }

    if(localScene()){
        mp.game.controls.disableControlAction(0, 45, true);
        mp.game.controls.disableControlAction(0, 69, true);
        mp.game.controls.disableControlAction(0, 140, true);
        mp.game.controls.disableControlAction(0, 141, true);
        mp.game.controls.disableControlAction(0, 142, true);
    }
        
}

function addScene(player, id){
    //console.logInfo(`add scene ${player.handle}`);
    if(scenes[player.remoteId]){
        scenes[player.remoteId].destroy()
    }
    if(player.handle !== 0)
        scenes[player.remoteId] = new Scene(player, id);
}

function removeScene(player){
    if(scenes[player.remoteId]){
        scenes[player.remoteId].cancelHandle();
    }
}

function sceneUpdate(player, newSceneId){
    try {
        if(player.type !== "player") return;
        if(newSceneId)
            addScene(player, newSceneId);
        else 
            removeScene(player);
    } catch (e) {
        if(global.sendException) mp.serverLog(`sceneUpdate: ${e.name}\n${e.message}\n${e.stack}`);
    }    
}

function resetSceneLocal(){
    removeScene(mp.players.local);
}

function actionHandle(id){
    if(scenes[id])
        scenes[id].actionHandle();
}

function actionDelay(time){
    global.inAction = true;
    setTimeout(()=>{
        global.inAction = false;
    }, time * 1000)
}

function streamIn(player){
    if(player.type !== "player") return;
    const sceneId = player.getVariable("scene:current");
    if(sceneId)
        addScene(player, sceneId);
}

function streamOut(player){
    if(player.type !== "player") return;
    if(scenes[player.remoteId]){
        scenes[player.remoteId].destroy();
        delete scenes[player.remoteId];
    }
}

function playerQuit(player){
    if(player.type !== "player") return;
    if(scenes[player.remoteId]){
        scenes[player.remoteId].destroy();
        delete scenes[player.remoteId];
    }
}

mp.events.add("scene:reset:local", resetSceneLocal);
mp.events.addDataHandler("scene:current", sceneUpdate);
mp.events.add("scene:doaction", actionHandle);
mp.events.add('click', (x, y, upOrDown, leftOrRight) => {
    if(!localScene() || upOrDown === "up" || isSpam()) return;
    if(leftOrRight === "right")
        localScene().rightClick();
    else
        localScene().leftClick();
});
mp.events.add("render", tick);
mp.events.add("scene:action:delay", actionDelay)


mp.events.add('entityStreamIn', streamIn);
mp.events.add('entityStreamOut', streamOut);
mp.events.add("playerQuit", playerQuit);

let obj = null;
const pos = new mp.Vector3(-1891.9628, -3122.3472, 13.944367);
const hash = mp.game.joaat("prop_tequila_bottle");
function doesExistCheck() {
    if(obj === null){
        obj = mp.objects.new(hash, pos, {dimension: mp.players.local.dimension});
    }else{
        console.logInfo(`dows exists: ${obj.doesExist()}`);
        obj.destroy();
        obj = null;
    }
}

mp.events.add("does:exists", doesExistCheck);
