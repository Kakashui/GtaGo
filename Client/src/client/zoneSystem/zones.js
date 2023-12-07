const ZoneModel = require('./zoneModel');

let zones = [];
global.newZone = (height, startPosition, startRange, endPosition, endRange, timeoutZone, zoneConstrictionTime, damageHP, damageTime) => {
    let zone = new ZoneModel(height, startPosition, startRange, endPosition, endRange, timeoutZone, zoneConstrictionTime, damageHP, damageTime);
    zones.push(zone);
    return zone.id;
};


global.deleteZone = (id) => {
    let index = zones.findIndex(item => item.id == id);
    if (index >= 0)
    {
        zones.splice(index, 1)[0].destroy();
    }
};
global.newZoneParams = (id, endPosition, endRange, timeoutZone, zoneConstrictionTime) => {
    let index = zones.findIndex(item => item.id == id);
    if (index >= 0)
        zones[index].updateParams(endPosition, endRange, timeoutZone, zoneConstrictionTime)
};

mp.events.add('render', () => {
    zones.forEach(zone => {
        zone.drawZone();
    });
});

// mp.events.add('render', () => {
//     mp.game.graphics.drawMarker(
//         1,
//         0, 0, 73,
//         0, 0, 0,
//         0, 0, 0,
//         10, 10, 2000,
//         255, 255, 50, 156,
//         false, false, 2,
//         false, null, null, false
//     );
// });