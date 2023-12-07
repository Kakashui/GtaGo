

const moveSettings = {
    size: {
        x: 70,
        y: 35
    },
    position: {
        left: '90%',
        top: '90%',
    },
    showIcons: [false, false, false, false],
    values: [
        {//LEFT X
            value: 160,
            min: -1800,
            max: 1800,
            step: 5,
            invert: true,
            enabled: true,
            callback: "camMoveAngleX"
        },
        {//LEFT Y
            value: 1,
            min: 0,
            max: 1.6,
            step: .2,
            invert: false,
            enabled: true,
            callback: "camMoveCamZ"
        },
        {//RIGHT X
            value: 2,
            min: 1,
            max: 3,
            step: .1,
            invert: false,
            enabled: false,
            callback: ""
        },
        {//RIGHT Y
            value: 0,
            min: -1,
            max: 1,
            step: .1,
            invert: true,
            enabled: false,
            callback: "camMovePointZ"
        },
        { //WHEELE
            value: 4,
            min: 2,
            max: 6,
            step: .5,
            invert: false,
            enabled: true,
            callback: "camSetDist"
        }
    ]
}

let typeStartPage = {
    [2]: 'MiddlePage',
    [3]: 'MiddlePage',
    [4]: 'MiddlePage',
    [5]: 'MotorPage',
    [15]: 'MiddlePage',
    [20]: 'MiddlePage',
    [21]: 'MiddlePage',
    [22]: 'MiddlePage',
    [23]: 'MiddlePage',
    [24]: 'MiddlePage',
    [25]: 'LamboPage',
    [26]: 'BugattiPage',
    [29]: 'MercedesPage',
    [30]: 'BmwPage',
    [31]: 'RollsPage',
    [32]: 'BentleyPage',
    [33]: 'FerrariPage',
    [34]: 'DiamondPage',
    [35]: 'LuxePage',
    [36]: 'JeepPage',
    [37]: 'CasualPage',
    [38]: 'GaragePage',
}

let opened = false;
let spanwPosition = new mp.Vector3(1660, -457.8873, 105.8244);  //+ new Vector3(5060, 110, -345)
//let spanwPosition = new mp.Vector3(-1508.435, -2992.874, -83.2728);  //
let autoModels = [];
let lockSelector = false;

let currIndexVeh = -1;
let countVeh = 0;
let currentAuto = {
    color: { r: 0, g: 0, b: 0 },
    entity: null,
    rotate: 90,
    startTestDrivePos: null,
    canTestDrive: false
}
let specificationMaxValues = {
    speed: 400,
    braking: 300,
    acceleration: 400,
    traction: 400
}
let maxValueSpecifications = 6;

mp.events.add('carshop:open', (vehicles, bizType) => {
    if (gui.isOpened()) {
        ExitMenu()
        return;
    }
    autoModels = JSON.parse(vehicles);
    countVeh = autoModels.length;
    currIndexVeh = 0;
    if (typeStartPage[bizType]) {
        global.gui.setData('carDealership/setCurrentVehicle', JSON.stringify(typeStartPage[bizType]));
    }
    else {
        global.gui.setData('carDealership/setCurrentVehicle', JSON.stringify('MiddlePage'));
    }
    global.gui.setData('carDealership/setCurrentPage', JSON.stringify('StartPage'));

    LoadShop();
});

function SetVehicle(index) {
    if (!autoModels[index])
        return;
    if (lockSelector) return;
    lockSelector = true;
    if (!currentAuto.entity) {
        currentAuto.entity = mp.vehicles.new(mp.game.joaat(autoModels[index].model), spanwPosition, {
            heading: 0,
            numberPlate: 'ELITE',
            alpha: 255,
            color: [
                [0, 0, 0],
                [0, 0, 0]
            ],
            locked: false,
            engine: false,
            dimension: global.localplayer.dimension
        });
        //if(!currentAuto.entity) return;
        for (let index = 0; ((currentAuto.entity.handle !== 0 || !currentAuto.entity.doesExist()) && index < 500); index++) {
            mp.game.wait(0);
        }
    }
    else
    {
        let newModel = mp.game.joaat(autoModels[index].model);
        currentAuto.entity.model = newModel;
        for (let index = 0; (currentAuto.entity.model != newModel && index < 2500); index++) {
            mp.game.wait(0);
        }
    }

    currentAuto.entity.setCustomPrimaryColour(currentAuto.color.r, currentAuto.color.g, currentAuto.color.b);
    currentAuto.entity.setCustomSecondaryColour(currentAuto.color.r, currentAuto.color.g, currentAuto.color.b);
    currentAuto.entity.setDirtLevel(0);
    //currentAuto.entity.setRotation(0, 0, currentAuto.rotate, 2, true);

    global.gui.setData('carDealership/setData', JSON.stringify({ data: autoModels[index], specifications: GetSpecData(currentAuto.entity) }));
    lockSelector = false;
}

function GetSpecData(vehicle) {
    let data = null;
    if (vehicle.type !== 'vehicle' || vehicle === undefined || vehicle.handle == 0) {
        data = {
            speed: 0,
            braking: 0,
            acceleration: 0,
            traction: 0,
        };
    }
    else {
        data = {
            speed: Math.round(((mp.game.vehicle.getVehicleModelMaxSpeed(vehicle.model) / 1.2) / specificationMaxValues.speed) * maxValueSpecifications),
            braking: Math.round(((vehicle.getMaxBraking() * 100) / specificationMaxValues.braking) * maxValueSpecifications),
            acceleration: Math.round(((vehicle.getAcceleration() * 100) / specificationMaxValues.acceleration) * maxValueSpecifications),
            traction: Math.round(((vehicle.getMaxTraction() * 10) / specificationMaxValues.traction) * maxValueSpecifications),
        }
        if (data.speed == 0)
            data.speed = 1;
        if (data.braking == 0)
            data.braking = 1;
        if (data.acceleration == 0)
            data.acceleration = 1;
        if (data.traction == 0)
            data.traction = 1;
    }
    return data;
}
let objects = [];
function LoadAutoroomProps() {
    // let intID = mp.game.interior.getInteriorAtCoords(-1505.783, -3012.587, -80.000);
    // let propList = [
    //     "Int02_ba_truckmod",
    // ];
    // for (const propName of propList) {
    //     if (!mp.game.interior.isInteriorPropEnabled(intID, propName)) {
    //         mp.game.interior.enableInteriorProp(intID, propName);
    //     }
    // }
    // mp.game.interior.refreshInterior(intID);
    //CreateModel('vw_prop_vw_casino_podium_01a', sum(spanwPosition, new mp.Vector3(0, 0, 0.1)), new mp.Vector3(0, 0, 0));

};


function sum(v1, v2) {
    return new mp.Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
}

function CreateModel(model, pos, rot) {
    mp.objects.new(mp.game.joaat(model), pos, {
        rotation: rot,
        alpha: 255,
        dimension: 4294967295
    });
}

LoadAutoroomProps();

mp.events.add('carDealership:setColor', (r, g, b) => {

    if (currentAuto.entity == null) return;
    currentAuto.color = { r: r, g: g, b: b };
    currentAuto.entity.setCustomPrimaryColour(r, g, b);
    currentAuto.entity.setCustomSecondaryColour(r, g, b)
});

mp.events.add('carDealership:prevVehicle', () => {
    if (currentAuto.entity == null) return;
    if (lockSelector) return;
    if (global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;

    currIndexVeh--;
    if (currIndexVeh < 0)
        currIndexVeh = countVeh - 1;
    SetVehicle(currIndexVeh);
});

mp.events.add('carDealership:nextVehicle', () => {
    if (currentAuto.entity == null) return;
    if (lockSelector) return;
    if (global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;

    currIndexVeh++;
    if (currIndexVeh >= countVeh)
        currIndexVeh = 0;
    SetVehicle(currIndexVeh);
});

mp.events.add('carDealership:buyVehicleCash', (forFamily) => {
    if (currentAuto.entity == null) return;
    if (global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;

    UnloadShop();
    BuyVehicle(true, forFamily)
});

mp.events.add('carDealership:buyVehicleCard', (forFamily) => {
    if (currentAuto.entity == null) return;
    if (global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;

    UnloadShop();
    BuyVehicle(false, forFamily)
});

function BuyVehicle(typeMoney, forFamily) {
    mp.events.callRemote('carshop:buyvehicle', autoModels[currIndexVeh].model, currentAuto.color.r, currentAuto.color.g, currentAuto.color.b, typeMoney, forFamily);
}


const testdrive = {
    isGoing: false,
    timeLeft: 60,
    timer: null
};

function testDriveTimer() {
    testdrive.timeLeft--;

    if (testdrive.timeLeft < 0) {
        mp.events.callRemote('endTestDriveAuto');

        clearInterval(testdrive.timer);
        testdrive.timer = null;
    }
}
mp.events.add('carDealership:testDrive', () => {

    if (currentAuto.entity == null) return;

    testdrive.isGoing = true;
    testdrive.timeLeft = 300;
    testdrive.timer = setInterval(testDriveTimer, 1000);

    UnloadShop();
    mp.events.callRemote('testDriveAuto', autoModels[currIndexVeh].model, currentAuto.color.r, currentAuto.color.g, currentAuto.color.b);
});

mp.events.add('endTestDrive', (dead) => {
    if (currentAuto.entity != null) return;

    testdrive.isGoing = false;

    if (testdrive.timer != null) {
        clearInterval(testdrive.timer);
        testdrive.timer = null;
    }

    if (!dead)
        LoadShop();
});

mp.events.add('render', () => {

    if (!testdrive.isGoing) return;
    mp.game.controls.disableControlAction(2, 75, true);

    mp.game.graphics.drawText(`End in ${testdrive.timeLeft}`, [0.5, 0.9], {
        font: 0,
        color: [255, 255, 255, 255],
        scale: [0.5, 0.5],
        outline: false
    });
});




mp.events.add('carDealership:closeInterface', () => {
    if (opened)
        ExitMenu();
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        ExitMenu();
});

function ExitMenu() {
    if (UnloadShop())
        mp.events.callRemote('carshop:exitMenu');
}

function LoadShop() {

    global.goodScreenFadeOut(200, 2000, 500);
    setTimeout(() => {
        //camera settings
        const pos = spanwPosition;
        global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
        global.gui.setData('mouseMove/setEnebled', true);
        opened = global.gui.openPage('CarDealership')
        if (!opened) return;
        global.customCamera.setPos(new mp.Vector3(pos.x, pos.y, pos.z + .6));
        global.customCamera.setPoint(new mp.Vector3(pos.x, pos.y, pos.z + .6));
        global.customCamera.moveCamZ(0);
        global.customCamera.setDist(4);
        global.customCamera.moveAngleX(160);
        global.customCamera.switchOn(0);
        SetVehicle(currIndexVeh);
    }, 500);
}

function UnloadShop() {
    if (!opened)
        return false;
    global.goodScreenFadeOut(200, 2000, 500);
    opened = false;
    setTimeout(() => {
        global.gui.setData('mouseMove/setEnebled', false);
        global.gui.close();
        global.customCamera.switchOff(0);
        opened = false;
        global.showCursor(false)

        if (currentAuto.entity == null) return;
        currentAuto.entity.destroy();
        currentAuto.entity = null;
    }, 500);
    return true;
}