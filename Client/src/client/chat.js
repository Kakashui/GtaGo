
mp.events.add('chat:api:action',(type, msg, id, idTo)=>{
    try{      
        if(global.mediaMute && (type === 0 || type === 1 ||type === 2 ||type === 10)) return;
        const player = mp.players.atRemoteId(id);
        const playerTo = mp.players.atRemoteId(idTo);

        if(mp.players.exists(player) && mp.storage.data.mainSettings.muteLowLevel){
            const lvl = global.getVariable(player, 'lvl', 0);
            if(lvl < mp.storage.data.mainSettings.muteLowLevelValue) return;
        }
        
        const fromText = player ? player.name.replace('_', ' ') : '';
        const toText = playerTo ? playerTo.name.replace('_', ' ') : '';
        const isFriend = global.iKnowThisPlayer(player);
        global.gui.pushChat(type, msg, id, fromText, idTo, toText, isFriend);
    } catch (e) {
        if(global.sendException) mp.serverLog(`chat:api:action: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add('chat:api:advert',(type, redactorId, msg, from, sim)=>{
    try{ 
        if(global.mediaMute) return;
        let redactor = mp.players.atRemoteId(redactorId);
        global.gui.pushChatAdvert(type, redactor ? redactor.name.replace('_', ' ') : 'Unknown', msg, from, sim);
    } catch (e) {
        if(global.sendException) mp.serverLog(`chat:api:advert: ${e.name}\n${e.message}\n${e.stack}`);
    }
});
global.mediaMute = false;
mp.events.add("media:mute:state", (state)=>{
    global.mediaMute = state;
    if(global.mediaMute){
        global.gui.clearChat();
        mp.events.call('notify', 4, 9, "media:mute:on:self", 3000);
    }else
        mp.events.call('notify', 4, 9, "media:mute:off:self", 3000);
})

mp.events.add('chat:api:clear',()=>{
    global.gui.clearChat();
});

mp.keys.bind(global.Keys.Key_T, false, ()=>{
    if(global.chatActive || global.gui.opened) return;
    global.gui.showChat();
});

mp.events.add("cahat:api:disable",()=>{
    global.chatActive = false;
    global.gui.close();
});