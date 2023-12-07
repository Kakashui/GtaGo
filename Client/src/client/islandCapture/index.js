let zone = null;
let height = 250;
let damageHP = 10;
let damageTime = 350;

mp.events.add('islandCapt::loadZone', (startTime, totalTime, constrictionTime, startZoneCenter, startZoneRange, endZoneCenter, endZoneRange) => {
    if (zone != null) {
        global.deleteZone(zone);
        zone = null;
    }
    let start = Date.now() - (startTime * 1000);
    zone = global.newZone(height, startZoneCenter, startZoneRange, endZoneCenter, endZoneRange, start, totalTime, constrictionTime, damageHP, damageTime)
})
mp.events.add('islandCapt::unloadZone', () => {
    if (zone != null) {
        global.deleteZone(zone);
        zone = null;
    }
})