let peds = [];

mp.events.add({
    "businesses:setPeds": (data) => {
        if(peds.length > 0) return;
            const lPeds = JSON.parse(data);

            lPeds.forEach(ped => {
                setPed(ped);
            })
            global.bizPedLoaded = true;
    },

    "businesses:setPed": (data) => {
        const ped = JSON.parse(data);
        setPed(ped);
    },

    "businesses:clearPeds": () => {
        peds.forEach(ped => {
            ped.ped.destroy();
            ped.label.destroy();
        });

        peds = [];
    },
})

function setPed(pedInfo) {
    const ped = mp.peds.newValid(pedInfo.Model, pedInfo.Position, pedInfo.Rotation.z, pedInfo.Dimension);
    if(ped == null) return;

    const labelPositon = new mp.Vector3(pedInfo.Position.x, pedInfo.Position.y, pedInfo.Position.z + 1);
    const label = mp.labels.new(pedInfo.Name, labelPositon, {
        los: true,
        drawDistance: 5,
        font: 4,
        color: [255, 255, 255, 235],
        scale: [0.5, 0.5],
        dimension: pedInfo.Dimension
    });
    peds.push({ ped: ped, label: label });
};