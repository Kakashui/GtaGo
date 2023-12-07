let businessesMarkers = [];

mp.events.add({
    "businesses:setMarker": setMarker,
    "businesses:setMarkers": (data) => {
        const markersInfo = JSON.parse(data);

        markersInfo.forEach(markerInfo => {
            setMarker(markerInfo.BizId, markerInfo.Position, markerInfo.Range);
        });
    },
    "businesses:clearMarkers": clearMarkers
});

function clearMarkers() {
    businessesMarkers.forEach(marker => {
        marker.destroy();
    })

    businessesMarkers = [];
}

function setMarker(bizId, position, range) {
    if (businessesMarkers[bizId]) {
        businessesMarkers[bizId].destroy();
    }

    const marker = mp.markers.new(1, position, range, {
        color: [255, 255, 255, 200],
        dimension: 0
    });

    businessesMarkers[bizId] = marker;
};