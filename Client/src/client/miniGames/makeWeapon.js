let opened = false;
let details = 0;
function nextGameStage(exp, money){
    details--;
    global.gui.setData("gameMakeWeapon/userData/updateUserExperience", exp);
    global.gui.setData("gameMakeWeapon/userData/addUserGain", money);
    global.gui.setData("gameMakeWeapon/userData/setDetails", details);
    setTimeout(
    ()=>{
        global.gui.setData("gameMakeWeapon/gameState/setWaitNewGame", true);
    }, 2000);    
}

function getDetails(){
    if(global.inAction || details > 4) return;
    global.inAction = true;
    global.controlsManager.disableAll();
    mp.players.local.taskPlayAnim("anim@scripted@freemode@postertag@collect_can@heeled@", "poster_tag_collect_can_var02_female", 8.0, -1, -1, 50, -1, true, true, true);
    setTimeout(()=>{
        global.inAction = false;
        details = 5;
        global.controlsManager.enableAll();
        mp.events.call('notify', 2, 9, "mw:game:detils:get", 3000);
        mp.players.local.clearTasksImmediately();
    }, 3000)
}

function quitGame() {    
    if(!opened) return;
    global.gui.close();
    opened = false;
}

function openGame(exp){
    if (opened || gui.isOpened()) return;
    if(details < 1){
        mp.events.call('notify', 1, 9, "mw:game:nodetails", 3000);
        return;
    }
    global.gui.setData("gameMakeWeapon/userData/setDetails", details);
    global.gui.setData("gameMakeWeapon/userData/setName", mp.players.local.name.replace('_', ' '));
    global.gui.setData("gameMakeWeapon/userData/updateUserExperience", exp);
    global.gui.setData("gameMakeWeapon/userData/updateUserGain", 0);
    global.gui.openPage("GameMakeWeapon");
    opened = true;
}

mp.events.add("mg:makeweapon:stage:next", nextGameStage);
mp.events.add("mg:makeweapon:game:open", openGame);
mp.events.add("mg:makeweapon:quit", quitGame);
mp.events.add("mw:game:detail:get", getDetails);