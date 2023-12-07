mp.events.addPhone({
    // CEF events
    "phone::appStore::installApp": (appId) => {
        mp.events.callRemote('phone:appStore:install', appId);
    },

    "phone::appStore::removeApp": (appId) => {
        mp.events.callRemote('phone:appStore:remove', appId);
    }
});