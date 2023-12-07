const { getFullAddressByPosition } = require('../zonesNames');

mp.events.add("phone:gps:load", (gpsData) => {
    
    gpsData = JSON.parse(gpsData);

    for (gpsCat in gpsData) {
        gpsData[gpsCat].forEach(gpsItem => {
            const pos = gpsItem.Position;
            gpsItem.Address = getFullAddressByPosition(pos);
        });
    }

    mp.events.call('gui:setData', 'smartphone/gps_loadData', JSON.stringify(gpsData));
});


mp.events.add("phone::gps::setWaypoint", (x, y) => {
    mp.game.ui.setNewWaypoint(x, y);
});