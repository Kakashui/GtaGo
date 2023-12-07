const keys = {
    [0x45]: "E",
    [0x49]: "I"
}
let currentHelp = [];
global.isInInteractShape = false;

Object.keys(keys).forEach((key) => {
    const k = parseInt(key);
    mp.keys.bind(k, false, () => handleKeyPressed(k));
});

mp.events.add({
    'interact:enterInteractShape': (helpKeys) => {
        global.isInInteractShape = true;        
        const dto = { show: true, items: [] };
        currentHelp = JSON.parse(helpKeys);
        currentHelp.forEach((info) => {
            dto.items.push({ key: keys[info.Key], text: info.Text });
        });
        if (global.frontendSoundsEnabled)
            mp.game.audio.playSoundFrontend(-1 , 'MP_RANK_UP', 'HUD_FRONTEND_DEFAULT_SOUNDSET', true);
        global.gui.setData('hud/setPromptData', JSON.stringify(dto));
    },

    'interact:exitInteractShape': () => {
        global.isInInteractShape = false;
        const dto = { show: false, items: [] };
        global.gui.setData('hud/setPromptData', JSON.stringify(dto));
    },
});

let _antiFloodCheck = 0;
function handleKeyPressed(key) {
    
    if (!global.loggedin || global.chatActive || global.editing || global.gui.isOpened() || global.IsPlayingDM || global.cuffed) return;
    if (_antiFloodCheck > Date.now()) return;
    _antiFloodCheck = Date.now() + 1000;

    if (!isInInteractShape || !currentHelp.find((info) => info.Key === key)) return;
    mp.events.callRemote('interact:interactKeyPressed', key);
}