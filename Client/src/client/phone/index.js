// Common phone storage init
if (!mp.storage.data.phone) {
    mp.storage.data.phone = { };
    // mp.storage.flush();
}

mp.phone = {
    onStartCallback: [],
    
    onStart: (callback) => {
        mp.phone.onStartCallback.push(callback);
    },

    invokeStart: () => {
        mp.phone.onStartCallback.forEach(callback => {
            callback();
        });
    }
}

require('./eventsProxy');
require('./main');
require('./states');

require('./apps/settings');
require('./apps/appStore');
require('./apps/contacts');
require('./apps/calls');
require('./apps/messenger');
require('./apps/gps');
require('./apps/taxi');
require('./apps/taxiJob');
require('./apps/camera');

mp.events.add('gui:ready', () => {
    mp.phone.invokeStart();
});

mp.events.add('cefLog', (txt) => {
    if (global.gui.debug)
        mp.serverLog(txt);
});