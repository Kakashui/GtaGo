let onBizColshape = false;
let isInfoPanelOpen = false;

mp.events.add({
    // Server events //
    "businesses:openInfoPanel": (data) => {
        global.gui.setData("businessPurchase/setBusinessData", data);
        isInfoPanelOpen = global.gui.openPage("BusinessPurchase");
    },

    // CEF events //
    "businesses::infoPanel_closeClick": () => {
        global.gui.close();
        global.showCursor(false);

        isInfoPanelOpen = false;
    },

    "businesses::infoPanel_buyClick": () => {
        mp.events.callRemote('businesses::buyBusiness');
    }
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function() {
    if (!isInfoPanelOpen) return;
    
    mp.events.call('businesses::infoPanel_closeClick');
});