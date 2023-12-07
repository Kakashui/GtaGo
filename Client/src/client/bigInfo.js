function showBigInfo(text) {
    global.gui.setData("hud/showBigInfo", `'${text}'`);
    if(global.gui.curPage == "" && !mp.gui.cursor.visible)
        global.showCursor(true);
}

function hideBigInfo() {
    global.gui.setData("hud/hideBigInfo");
    if(global.gui.curPage == "")
        global.showCursor(false);
}

mp.events.add("biginfo:show", showBigInfo)
mp.events.add("biginfo:hide", hideBigInfo)