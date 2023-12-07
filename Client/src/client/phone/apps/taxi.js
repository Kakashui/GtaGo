const { getFullAddressByPosition } = require('../zonesNames');

let isWaitingForWaypoint = false;
let waypointPosition = null;

mp.events.add({
    // CEF events
    "phone::taxi::sendToMap": () => {
        isWaitingForWaypoint = true;
        global.closeGoPhone();
        mp.game.ui.setFrontendActive(true);
    },

    "phone::taxi::requestTaxi": (paymentByCard) => {
      mp.events.callRemote('phone:taxi:requestTaxi', paymentByCard, waypointPosition.x, waypointPosition.y)
    }
    
    // Server events
});

// Rage events
mp.events.add("playerCreateWaypoint", (position, toggle) => {
    if (isWaitingForWaypoint) {
      mp.game.ui.setFrontendActive(false);
      isWaitingForWaypoint = false;
      waypointPosition = position;
      
      mp.events.call('gui:setData', 'smartphone/taxiPage/taxi_setSpecifyLocation', JSON.stringify({
          name: getFullAddressByPosition(position),
          position: position,
          type: 'known'
      }));

      setTimeout(() => {
        global.openGoPhone();
      }, 100);
    }
});

//#region playerCreateWaypoint event fix
// Stuff for fixing playerCreateWaypoint event on 0.3.7, can be deleted on 1.1 i think.
// https://rage.mp/forums/topic/3875-solution-for-waypoint/

let waypoint;

mp.events.add('render', () => {
  // Waypoint
  if (waypoint !== mp.game.invoke('0x1DD1F58F493F1DA5')) {
    waypoint = mp.game.invoke('0x1DD1F58F493F1DA5');
    let blipIterator = mp.game.invoke('0x186E5D252FA50E7D');
    let FirstInfoId = mp.game.invoke('0x1BEDE233E6CD2A1F', blipIterator);
    let NextInfoId = mp.game.invoke('0x14F96AA50D6FBEA7', blipIterator);
    for (let i = FirstInfoId; mp.game.invoke('0xA6DB27D19ECBB7DA', i) != 0; i = NextInfoId) {
      if (mp.game.invoke('0xBE9B0959FFD0779B', i) == 4 ) {
        var coord = mp.game.ui.getBlipInfoIdCoord(i);
        mp.events.call("playerCreateWaypoint", coord, Boolean(waypoint));
      };
    };
  };
});
//#endregion playerCreateWaypoint event fix