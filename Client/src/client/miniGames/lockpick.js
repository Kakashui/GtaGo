let opened = false;

function quitGame() {    
    if(!opened) return;
    global.gui.close();
    opened = false;
}

function openGame(callback){
    if (opened || gui.isOpened()) return;
    global.gui.setData("gameLockpick/setCallback", JSON.stringify({callback}));
    global.gui.openPage("GameLockpick");
    opened = true;
}

mp.events.add("mg:lockpick:open", openGame);
mp.events.add("mg:lockpick:quit", quitGame);