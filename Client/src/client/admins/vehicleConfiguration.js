mp.events.add({
    // SERVER EVENTS
    "vehicleConfiguration:openEditMenu": (data) => {
        global.gui.setData("devTools/setVehicleEdit", data);
        global.gui.setData("devTools/selectMenu", 1);
        global.gui.openPage("DevTools");
    },

    // GUI EVENTS
    "vehicleConfiguration::save": (configuration) => {
        mp.events.callRemote('vehicleConfiguration:saveConfig', configuration);
        global.gui.close();
    }
});