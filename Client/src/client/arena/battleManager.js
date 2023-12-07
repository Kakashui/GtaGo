const PlayersBlipTracker = require('./PlayersBlipTracker')
let tracker

mp.events.add({
    'arena:battle:start': (members, currentMember, startTimer) => {
        global.IsPlayingDM = true;
        
        mp.events.call('weapon:detachAll');
        
        global.gui.setData('hud/setKillstatCurrentUser', currentMember);
        global.gui.setData('hud/setKillstatType', JSON.stringify({value: ''}));
        global.gui.setData('hud/setKillstatItems', members);
        
        if (startTimer === true) global.gui.setData('hud/startKillstatTimer', 5 * 60);
        global.gui.setData('hud/setIsKillStat', true);
        global.gui.close();
        
        mp.events.add('playerDeath', deathHandler);
        
        global.gui.playSound('play', 0.2, false);
        mp.keys.bind(global.Keys.Key_I, false, leaveArena)
        
        const dto = { show: true, items: [{ key: 'I', text: 'cl:btlmngr:leave' }] }
        global.gui.setData('hud/setPromptData', JSON.stringify(dto));
    },
    
    'arena:battle:stop': () => {
        global.IsPlayingDM = false;
        
        mp.events.call('weapon:updateAttach');

        global.gui.setData('hud/setIsKillStat', false);
        global.gui.setData('hud/setIsFullKillStat', false);
        
        mp.events.call('notify', 2, 9, "arena_dm_28", 4000);
        
        mp.events.remove('playerDeath', deathHandler);
        global.gui.setData('hud/stopKillstatTimer');
        
        openResultWindow();

        const dto = { show: false, items: [] }
        global.gui.setData('hud/setPromptData', JSON.stringify(dto));
        mp.keys.unbind(global.Keys.Key_I, false, leaveArena)
        
        if (tracker) tracker.stop();
    },
    
    'arena:tracking:start': (teammateRemoteIds) => {
        tracker = new PlayersBlipTracker(JSON.parse(teammateRemoteIds));
        tracker.startTracking();
    },

    'arena:tracking:stop': () => {
        if (tracker) tracker.stop();
    },
    
    'arena:sr': (data) => {
        if (global.IsPlayingDM){
            mp.events.call('notify', 1, 9, "arena_dm_32", 4000);
            return;
        }
        resultWindowOpened = true;
        global.showCursor(true);
        global.gui.setData('hud/setKillstatItems', data);
        global.gui.setData('hud/setKillstatType', JSON.stringify({value: 'global_rating'}));
        global.gui.setData('hud/setIsFullKillStat', true);
    },
    
    'arena:endRound': (winnerTeam) => {
        if (winnerTeam === 0) global.gui.playSound('twin2', 0.2, false);
        else global.gui.playSound('ctwin1', 0.2, false);
    }
});

function leaveArena() {
    if (!global.chatActive)
        mp.events.callRemote('ab:leave')
}

function deathHandler(player, reason, killer) {
    if (!global.IsPlayingDM) return;
    if (!player || !killer) return;

    if (killer.name !== mp.players.local.name) return;

    //mp.game.graphics.startScreenEffect('SuccessNeutral', 300, false);
    
    global.gui.playSound('kill', 0.2, false);
}

let resultWindowOpened = false;
function closeResultWindow() {
    if (!resultWindowOpened) return;
    resultWindowOpened = false;
    global.showCursor(false);
    global.gui.setData('hud/setIsFullKillStat', false);
}
function openResultWindow() {
    resultWindowOpened = true;
    global.gui.setData('hud/setKillstatType', JSON.stringify({value: 'match_result'}));
    global.showCursor(true);
    global.gui.setData('hud/setIsFullKillStat', true);
}
mp.keys.bind(global.Keys.Key_ESCAPE, false, closeResultWindow);