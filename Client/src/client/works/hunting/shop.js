let isHuntingShopOpened = false;

mp.events.add({
    "huntingStore:open": (data) => {
        global.gui.setData('huntingShop/setData', data);
        isHuntingShopOpened = global.gui.openPage('HuntingShop');
    },

    "huntingStore::close": () => {
        global.gui.close();
        isHuntingShopOpened = false;
    },

    "huntingStore::select": (key) => {
        mp.events.callRemote('huntingStore:select', key);
    },
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function() {
    if (!isHuntingShopOpened) return;
    
    mp.events.call('huntingStore::close');
});