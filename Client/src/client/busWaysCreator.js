const buswayMarkers = [];

mp.events.add({
    "buswayscreator:pushMarker": pushMarker,
    "buswayscreator:popMarker": popMarker,
    "buswayscreator:clearMarkers": clearMarkers
});

function clearMarkers() {
    buswayMarkers.forEach(marker => {
        if (marker) {
            marker.destroy();
        }
    })
};

function pushMarker(position, isStop) {
    const markerColor = isStop ?  [0, 255, 0, 180] : [255, 255, 255, 180];  
    const marker = mp.markers.new(1, position, 4, {
        color: markerColor
    })
    
    buswayMarkers.push(marker);
};

function popMarker() {
    const marker = buswayMarkers.pop();

    if (marker) {
        marker.destroy();
    }
};