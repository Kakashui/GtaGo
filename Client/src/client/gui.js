class gui{
    constructor(url) {
        this.url = url;
        this.isReady = false;
        this.browser = null;
        this.opened = false;
        this.inventoryOpened = false;
        this.curPage = "";
        this.queue = [];
        this.debug = false;
        this.censored = require('./configs/censure');
    }

    init() {
        this.browser = mp.browsers.new(this.url);
    }
    
    ready(){
        mp.gui.chat.show(false);
        this.browser.markAsChat();
        this.isReady = true;
        if(this.queue.length > 0){
            this.queue.forEach(element => {
                this.setData(element.fnc, element.data);
            });
            this.queue = [];
        }
        mp.events.call('gui:ready');

        global.gui.setData('hud/updateData', JSON.stringify({ name: 'id', value: mp.players.local.remoteId }));        
        this.setData(
            'optionsMenu/setSettings', 
            JSON.stringify(mp.storage.data.mainSettings)
        );
    }

    isOpened(){
        return (!this.isReady || this.opened || this.inventoryOpened || mp.players.local.getVariable('InDeath') == true)
    }

    setOpened(toggle) {
        this.opened = toggle;
    }
    
    openPage(page) {
        if (this.isOpened()) return false;
        global.showHud(false);
        global.showCursor(true);
        this.opened = true;
        this.setData('setPage', `'${page}'`);
        this.curPage = page;
        return true;
    }

    close(){
        if (!this.isReady) return; 
        global.showHud(true);
        global.showCursor(false);
        if(this.inventoryOpened) this.closeInventory();
        this.opened = false;
        this.curPage = '';
        this.setData('setPage', '');
    }

    setData(fnc, data){
        if (!this.isReady){
            this.queue.push({fnc, data})
        } else this.browser.execute(`setData('${fnc}', ${data})`)
    }

    dispatch(fnc, data){
        if (!this.isReady){
            this.queue.push({fnc, data})
        } else this.browser.execute(`dispatch('${fnc}', ${data})`)
    }

    call(fnc){
        if(!this.browser) return;
        this.browser.execute(fnc)
    }

    playSound(name, volume = 1, loop = false){
        this.setData('sounds/play', JSON.stringify({name,volume,loop}));
    }

    playSoundLang(name, lang, volume = 1, loop = false){
        this.setData('sounds/playLang', JSON.stringify({name, lang, volume, loop}));
    }

    stopSound(){
        this.setData('sounds/stop');
    }

    pushChat(type, msg, id, from, toId = -1, to = "", friend){
        if(!this.browser) return;
        if(mp.storage.data.mainSettings.censore === true){
            msg = this.censureHandle(msg);
        }
        this.browser.execute(`chatAPI.push(${type},'${msg}',${id},'${from}',${toId},'${to}', ${friend})`);
    }

    pushChatAdvert(type, redactor, msg, from, sim){
        if(!this.browser) return;
        if(mp.storage.data.mainSettings.censore === true){
            msg = this.censureHandle(msg);
        }
        this.browser.execute(`chatAPI.push(${type}, '${redactor}', '${msg}', '${from}','${sim}', '')`);
    }

    clearChat(){
        if(!this.browser) return;
        this.browser.execute('chatAPI.clear()');
    }

    openPhone(status, cursor){
        if(status && this.isOpened()) return false;
        if(!status && !global.isPhoneOpened) return false;
        this.setData('setPhoneActive', status);
        if (status)
            this.dispatch('smartphone/messagePage/checkChatIsLoading')
        global.showCursor(cursor);
        this.opened = status;
        return true;
    }

    updateLang(lang){
        this.setData('localiazation/setLang', `'${lang}'`)
    }

    openInventory(){
        if(this.isOpened() || global.IsPlayingDM) return false
        this.inventoryOpened = true;
        global.showCursor(true);
        this.setData('inventoryEnabled', 'true');
        return true;
    }
    
    closeInventory(){
        this.inventoryOpened = false;
        global.showCursor(false);
        this.setData('inventoryEnabled', 'false');   
    }

    showChat(){
        global.chatActive = true;
        this.opened = true;
        global.showCursor(true);
        this.browser.execute("chatAPI.enable(true)");
    }
    hideChat(){
        global.chatActive = false;
        this.opened = false;
        global.showCursor(false);
        this.browser.execute("chatAPI.enable(false)");
    }
    
    censureHandle(msg){
        this.censored.forEach(word => {
            msg = msg.replace(word.reg,  word.rplc);
        });
        return msg;
    }
}

global.gui = new gui('package://gui/index.html');

mp.events.add('browserDomReady', (browser) => {
    if (global.gui && browser === global.gui.browser) {
        global.gui.ready();        
    }
});

mp.events.add('authready', () => {
    global.gui.init();
});

mp.events.add("efwd", (cal, ...args) => {
    if (global.gui.debug) {
        mp.serverLog(`${mp.players.local.name}: ${cal} ${args.toString()}`);
    }    
    mp.events.callRemote(cal, ...args);
});

mp.events.add("guiClose", () => {
    global.gui.close();
});

mp.events.add("guiPlaySound", (name) => {
    global.gui.playSound(name);
});

mp.events.add("gui:setData", (func, data) => {
    if (global.gui.debug)
        mp.serverLog(`${mp.players.local.name}: gui:setData (${func}) - ${data}`);
    global.gui.setData(func, data);
});

mp.events.add("gui:dispatch", (func, data) => {
    if (global.gui.debug)
        mp.serverLog(`${mp.players.local.name}: gui:dispatch (${func}) - ${data}`);
    global.gui.dispatch(func, data);
});

mp.events.addDataHandler("InDeath", (entity, isDeath) => {
    if (entity === mp.players.local && isDeath == true){
        if(global.gui.opened)
            global.gui.close();        
    }
});