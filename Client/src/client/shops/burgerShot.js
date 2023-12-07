let isBurgerShotOpened = false;

mp.events.add({
    // Server events
    "burgerShot:open": (dto) => {
        global.gui.setData('burgerShop/setData', dto);
        isBurgerShotOpened = global.gui.openPage('BurgerShop');
    },

    // CEF events
    "burgerShot::close": () => {
        isBurgerShotOpened = false;
        global.gui.close();
    },

    "burgerShot::buy": (data) => {
        mp.events.callRemote("burgerShot:buyItems", data);
    }
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function() {
    if (!isBurgerShotOpened) return;
    
    mp.events.call('burgerShot::close');
});