let peds = [
    // { key: 1, Pos: new mp.Vector3(-805.0755, -222.98671, 37.252777), Hash: -1932625649, Angle: 168.28566, Name: "Brandon" },
    { key: 1, Pos: new mp.Vector3(-790.4354, -208.20871, 37.252777), Hash: -1932625649, Angle: 128.28566, Name: "Brandon" },
    // { key: 2, Pos: new mp.Vector3(-117.20474, -608.2347, 37.75329), Hash: 365775923, Angle: -86.20102, Name: "Leo" },
    { key: 2, Pos: new mp.Vector3(-122.82194, -586.0485, 37.75329), Hash: 365775923, Angle: -157.90102, Name: "Leo" },
    { key: 4, Pos: new mp.Vector3(273.38412, -1162.9672, 29.272997), Hash: -96953009, Angle: 6.32013, Name: "Mary" },
    { key: 8, Pos: new mp.Vector3(75.75917, -1747.0688, 29.629784), Hash: -815646164, Angle: 39.4371, Name: "Don" },
];
let createdPeds = null;
let labels = null;
let mailPos = new mp.Vector3(-535.79486, -170.78406, 38.219646);
let createdBlips = {};
let createdMarkers = {};

let ped1Colplete = false;
let ped2Colplete = false;
let ped3Colplete = false;

mp.events.add("startquest:Stage3DeliveryOfMail:delBlip", (id) => {
    if (createdBlips[id]) {
        createdBlips[id].destroy();
        createdBlips[id] = undefined;
    }
    if (createdMarkers[id]) {
        createdMarkers[id].destroy();
        createdMarkers[id] = undefined;
    }
});

mp.events.add("startquest:Stage3DeliveryOfMail:stop", () => {
    peds.forEach(ped => {
        if (createdBlips[ped.key]) {
            createdBlips[ped.key].destroy();
            createdBlips[ped.key] = undefined;
        }
        if (createdMarkers[ped.key]) {
            createdMarkers[ped.key].destroy();
            createdMarkers[ped.key] = undefined;
        }
    });
    if (createdPeds != null)
        destroyPeds();
});

mp.events.add("startquest:Stage3DeliveryOfMail", () => {
    if (createdPeds != null)
        destroyPeds();
    createdPeds = [];
    labels = [];
    let pedModel;
    let blip;
    let marker;
    let label;
    peds.forEach(ped => {
        pedModel = mp.peds.newValid(ped.Hash, ped.Pos, ped.Angle, 0);
        createdPeds.push(pedModel);

        label = mp.labels.new(ped.Name, new mp.Vector3(ped.Pos.x, ped.Pos.y, ped.Pos.z + 1),
            {
                los: false,
                font: 4,
                drawDistance: 10,
                color: [255, 255, 255, 200],
                dimension: 0
            });
        labels.push(label);

        blip = mp.blips.new(1, ped.Pos,
            {
                name: 'Target',
                scale: 1,
                color: 46,
                alpha: 255,
                drawDistance: 100,
                shortRange: false,
                rotation: 0,
                dimension: 0,
            });
        if (createdBlips[ped.key]) {
            createdBlips[ped.key].destroy();
            createdBlips[ped.key] = undefined;
        }
        createdBlips[ped.key] = blip


        marker = mp.markers.new(0, new mp.Vector3(ped.Pos.x, ped.Pos.y, ped.Pos.z + 2), 1,
            {
                rotation: new mp.Vector3(),
                color: [50, 200, 100, 200],
                visible: true,
                dimension: 0
            });
        if (createdMarkers[ped.key]) {
            createdMarkers[ped.key].destroy();
            createdMarkers[ped.key] = undefined;
        }
        createdMarkers[ped.key] = marker
    });
});

mp.events.add("startquest:Stage3DeliveryOfMail:close", () => {
    if (createdPeds != null)
        destroyPeds();
});

function destroyPeds() {
    createdPeds.forEach(createdPed => {
        createdPed.destroy();
    });
    createdPeds = null;
    if (labels != null) {
        labels.forEach(label => {
            label.destroy();
        });
        labels = null;
    }
}