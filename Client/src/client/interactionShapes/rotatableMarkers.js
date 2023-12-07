mp.events.add({
    'interact:loadMarkers': (markers) => {
        markers = JSON.parse(markers);
        
        markers.forEach((markerInfo) => {    
            //const marker = createMarkerFromInfo(markerInfo, new mp.Vector3(0, 0, 0));
            
            _rotatableMarkers.push({
                // object: marker,
                // currentRotation: new mp.Vector3(0, 0, 0),
                info: markerInfo
            });
        });
    },
    
    'interact:destroyMarker': (markerId) => {
        const markerIdx = _rotatableMarkers.findIndex((m) => m.info.ID === markerId);
        if (markerIdx !== -1 /*&& _rotatableMarkers[markerIdx].object*/) {
            // _rotatableMarkers[markerIdx].object.destroy();
            _rotatableMarkers.splice(markerIdx, 1);
        }
    }
});

const _rotatableMarkers = [];
const GLOBAL_DIMENSION = 4294967295;

function createMarkerFromInfo(info, rotation) {
    const marker = mp.markers.new(info.Type, info.Position, info.Scale, {
        color: [info.Color.Red, info.Color.Green, info.Color.Blue, info.Color.Alpha],
        dimension: info.Dimension,
        rotation: rotation
    });

    return marker;
}

function getRotatedVector3ByZ(rotation) {
    const z = (rotation.z <= -360 || rotation.z >= 360) ? 0 : rotation.z - 2;
    const newRotation = new mp.Vector3(0, 0, z);

    return newRotation;
}

// setInterval(() => {
//     if (!global.loggedin) return;
    
//     try {
//         _rotatableMarkers.forEach((marker) => {
//             if (marker && marker.object &&
//                 (marker.info.Dimension === mp.players.local.dimension || marker.info.Dimension == GLOBAL_DIMENSION)) {
//                 const pos1 = mp.players.local.position;
//                 const pos2 = marker.info.Position;
//                 const distance = mp.game.system.vdist(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);
    
//                 if (distance < 10) {
//                     const newRotation = getRotatedVector3ByZ(marker.currentRotation);
                    
//                     marker.object.destroy();
//                     marker.object = createMarkerFromInfo(marker.info, newRotation);
//                     marker.currentRotation = newRotation;
//                 }
//             }
//         });
//     }
//     catch (e){ 
//         if(global.sendException) mp.serverLog(`rotatableMarker.setInterval: ${e.name}\n${e.message}\n${e.stack}`);
//     }
// }, 50);

let currRotate = 0;
mp.events.add('render', () => {
    currRotate = currRotate > 360 ? 0 : currRotate + 0.5;
    _rotatableMarkers.forEach((marker) => {
        if (marker.info.Dimension === mp.players.local.dimension || marker.info.Dimension == GLOBAL_DIMENSION) {
            const pos1 = mp.players.local.position;
            const pos2 = marker.info.Position;
            const distance = mp.game.system.vdist(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);

            if (distance < 50) {
                global.drawRotateMarkerInRender(marker.info.Type, marker.info.Position, marker.info.Scale, marker.info.Color)
            }
        }
    });
})

global.drawRotateMarkerInRender = (type, pos, scale, color) => {
    mp.game.graphics.drawMarker(
        type,
        pos.x, pos.y, pos.z,
        0, 0, 0,
        0, 0, currRotate,
        scale, scale, scale,
        color.Red, color.Green, color.Blue, color.Alpha,
        false, false, 2,
        false, null, null, false
    );
}