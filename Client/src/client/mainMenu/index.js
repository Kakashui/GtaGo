require('./fractionMenu.js')
require('./referals')
let mainMenuOpened = false;
mp.keys.bind(global.Keys.Key_M, false, ()=> {
    if(mainMenuOpened || global.chatActive || global.gui.isOpened()) return;
    mainMenuOpened = global.gui.openPage("OptionsMenu");
})

mp.keys.bind(global.Keys.Key_ESCAPE, false, ()=> {
    close();
})

mp.events.add("mmenu:stats:update", (data) =>{
    global.gui.setData('optionsMenu/setStats', data);
})

mp.events.add("mmenu:open:donate", () =>{
    close();
    mp.events.call("dshop:open");
})

mp.events.add("mmenu:props:update", (data) =>{
    global.gui.setData('optionsMenu/setProps', data);
})

mp.events.add("mmenu:products:update", (data) =>{
    global.gui.setData('optionsMenu/setProducts', data);
})

mp.events.add("mmenu:setting:set", (name, status) =>{   
    mp.storage.data.mainSettings[name] = status;
    mp.storage.flush();
})

mp.events.add("mmenu:bp:update", (bp) =>{   
    global.gui.setData("optionsMenu/updateBonusPoints", `${bp}`);
})

mp.events.add("cef:mmenu:close", () =>{
    close();
})

mp.events.add("cef:mmenu:capt:open", ()=>{
    close();
    global.gui.setData("optionsMenu/setAttack", 'false');
    mp.events.callRemote("mmenu:captteam")
})

function close(){    
    if(!mainMenuOpened) return;
    global.gui.close();
    mainMenuOpened = false
}