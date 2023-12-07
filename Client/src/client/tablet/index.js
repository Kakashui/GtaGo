let tabletOn = false;

function open(){
    global.gui.close();
    global.gui.openPage("GameTablet");
    tabletOn = true;
}

function close(){ 
    global.gui.close();
    mp.events.callRemote("scene:action:cancel");
    tabletOn = false;
}

mp.events.add("tablet:open", open);

mp.keys.bind(global.Keys.Key_ESCAPE, false, ()=>{
    if(tabletOn) {
        mp.game.ui.setPauseMenuActive(false);
        close();
    }
})