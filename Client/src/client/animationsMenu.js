let isAnimationsMenuOpened = false;
let isAnimPlayed = false;
mp.events.add('gui:ready', loadAnimationsSettings);

let lastMessage = 0;
mp.events.add({
    "animations:setPlay": (isPlayed) => {
        if (isPlayed) {
            global.gui.setData('hud/setPromptData', JSON.stringify({ show: true, items: [{ key: '🠐', text: 'Animations_hint' }] }));
        }
        else {
            global.gui.setData('hud/setPromptData', JSON.stringify({ show: false, items: [] }));
        }

        isAnimPlayed = isPlayed;
    },
    
    // CEF events
    "animations::close": () => {
        isAnimationsMenuOpened = false;
        global.gui.close();
    },

    "animations::setQuickAccess": (quickKey, animKey) => {
        mp.storage.data.animations.quickAccess[quickKey] = animKey;
        //mp.storage.flush();
    },

    "animations::setFavourite": (animKey, isFav) => {
        if (isFav)
            mp.storage.data.animations.favorites[animKey] = true;
        else
            delete mp.storage.data.animations.favorites[animKey];

        // mp.storage.flush();
    },

    "animations::play": (animationKey) => {
        if(global.inAction ||  global.isPhoneOpened) return;
        mp.events.callRemote('animations:play', animationKey);
    }
});

mp.keys.bind(global.Keys.Key_BACK, false, function() {
    if (!global.loggedin || global.chatActive || global.editing || global.gui.isOpened()) return;

    if (isAnimPlayed) {
       mp.events.callRemote('animations:stop');
    } 
});

mp.keys.bind(global.Keys.Key_U, false, function() {
    if (isAnimationsMenuOpened) return;

    if (global.checkIsAnyActivity()) return;

    if (global.localplayer.isInAnyVehicle(true)) return;

    isAnimationsMenuOpened = global.gui.openPage('AnimationsMenu');
    
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function() {
    if (!isAnimationsMenuOpened) return;
    
    mp.events.call('animations::close');
});

function loadAnimationsSettings() {
    //#region Storage init
    if (!mp.storage.data.animations) {
        mp.storage.data.animations = {
            quickAccess: {},
            favorites: {}
        }

        //mp.storage.flush();
    }
    //#endregion Storage init
    
    global.gui.setData('animationsMenu/init',
        JSON.stringify({
            quickAccess: mp.storage.data.animations.quickAccess,
            favorites: mp.storage.data.animations.favorites
        }));
}

mp.keys.bind(global.Keys.Key_Q, false, () => playAnimFromQuickAccess('Q'));
mp.keys.bind(global.Keys.Key_W, false, () => playAnimFromQuickAccess('W'));
mp.keys.bind(global.Keys.Key_E, false, () => playAnimFromQuickAccess('E'));
mp.keys.bind(global.Keys.Key_R, false, () => playAnimFromQuickAccess('R'));
mp.keys.bind(global.Keys.Key_T, false, () => playAnimFromQuickAccess('T'));
mp.keys.bind(global.Keys.Key_Y, false, () => playAnimFromQuickAccess('Y'));
mp.keys.bind(global.Keys.Key_U, false, () => playAnimFromQuickAccess('U'));
mp.keys.bind(global.Keys.Key_I, false, () => playAnimFromQuickAccess('I'));

function playAnimFromQuickAccess(button) {
    if (global.inAction || !isAnimationsMenuOpened) return;
    
    const animationKey = mp.storage.data.animations.quickAccess[button];
    if (animationKey) {
        mp.events.call('animations::play', animationKey);
    }
}