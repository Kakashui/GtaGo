// Settings App init
if (!mp.storage.data.phone.settings) {
    mp.storage.data.phone.settings = {
        Wallpaper: 0,
        NotificationSound: 0,
        Ringtone: 0,
        IsAirplaneMode: false
    };

    // mp.storage.flush();
}

function loadConfigs() {    
    global.gui.setData('smartphone/setConfigWallpaper', mp.storage.data.phone.settings.Wallpaper);
    global.gui.setData('smartphone/setConfigNotificationSound', mp.storage.data.phone.settings.NotificationSound);
    global.gui.setData('smartphone/setConfigRingtoneSound', mp.storage.data.phone.settings.Ringtone);
    global.gui.setData('smartphone/setConfigIsAirplaneMode', mp.storage.data.phone.settings.IsAirplaneMode);
}

mp.phone.onStart(loadConfigs);

mp.events.addPhone({
    "phone::settings::setConfigWallpaper": (wallpaper) => {
        mp.storage.data.phone.settings.Wallpaper = wallpaper;
        // mp.storage.flush();
    },

    "phone::settings::setConfigNotificationSound": (notificationSound) => {
        mp.storage.data.phone.settings.NotificationSound = notificationSound;
        // mp.storage.flush();
    },

    "phone::settings::setConfigRingtone": (ringtone) => {
        mp.storage.data.phone.settings.Ringtone = ringtone;
        // mp.storage.flush();
    },

    "phone::settings::setConfigAirplaneMode": (isAirplaneMode) => {
        mp.storage.data.phone.settings.IsAirplaneMode = isAirplaneMode;
        // mp.storage.flush();
    }
});