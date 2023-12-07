const moveSettings = {
    size: {
        x: 60,
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
            value: 0,
            min: -0.6,
            max: 2.6,
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
            min: .5,
            max: 15,
            step: .5,
            invert: false,
            enabled: true,
            callback: "camSetDist"
        }
    ]
}
let tuningTypes = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 46, 48]; //этот тюнинг проверям для каждой машине
let tuningTypesAll = [11, 18, 22, 23, 51, 53, 55, 1000, 1001, 1003]; //этот тюнинг есть у всех авто
let tuningTypesAllMoto = [11, 18, 22, 23, 24, 51, 53, 1000, 1003]; //этот тюнинг есть у всех мото
let fixTuningCar = [
    { model: "impaler3", fixmod: [5, 9, 7, 10, 41, 43] },
    { model: "issi3", fixmod: [9, 7, 26, 35, 41, 43] },
    { model: "issi4", fixmod: [9, 7, 10, 26, 35, 41, 43] },
    { model: "issi5", fixmod: [9, 7, 10, 26, 35, 41, 43] },
    { model: "issi6", fixmod: [9, 7, 10, 26, 35, 41, 43] },
    { model: "revolter", fixmod: [10] },
]
let percentCarPrice = 5000000;  //для расчета коэффициента зависимости стоимости детали от цены на авто
let config = [];  //доступный тюнинг на авто
let modelCar = ""; //Машина, в которой находится игрок
let modelPrice = 20000000; //цена текущей машины
let priceOnePart = 100; //цена за материал на текущем LSCustom
let opened = false;
let vehicleComponents = null; //установленный на авто тюнинг
let currVehClass = -1;  //класс авто
let cntTurboStage = 1;
let disableMods = [];

//Текущая ветка
let currentCategory = "";
let currentComponent = "";
let currentWheelType = "";

//установленные компоненты
let currComp = -1;
let currMod = -1;
let currWheelType = -1;
let currPrice = 0;

//установленный цвет
let currentPearl = -1;
let currentMod1 = 0;
let currentMod2 = 0;
let currentFirstColor = {
    r: 0, g: 0, b: 0
};
let currentSecColor = {
    r: 0, g: 0, b: 0
};

//установленный неон
let currentNeon = {
    r: 0, g: 0, b: 0, a: 0
};
let currentNeon2 = {
    r: 0, g: 0, b: 0, a: 0
};
//установленный цвет дыма шин
let currentTyreSmokeColor = {
    r: 0, g: 0, b: 0, a: 0
};

let tuningRanks = [
    '0',
    'I',
    'II',
    'III',
    'IV',
    'V',
    'VI',
    'VII',
    'VIII',
    'IX',
    'X',
]


let fullTuning = [
    { id: 0, title: "client_71", key: "Spoilers", parent: "body" },
    { id: 1, title: "client_72", key: "FrontBumper", parent: "body" },
    { id: 2, title: "client_73", key: "RearBumper", parent: "body" },
    { id: 3, title: "client_74", key: "SideSkirt", parent: "body" },
    { id: 4, title: "client_75", key: "Exhaust", parent: "body" },
    { id: 5, title: "client_76", key: "Frame", parent: "body" },
    { id: 6, title: "client_77", key: "Grille", parent: "body" },
    { id: 7, title: "client_78", key: "Hood", parent: "body" },
    { id: 8, title: "client_79", key: "Fender", parent: "body" },
    { id: 9, title: "client_80", key: "RightFender", parent: "body" },
    { id: 10, title: "client_81", key: "Roof", parent: "body" },
    { id: 11, title: "client_82", key: "Engine", parent: "power" },
    { id: 12, title: "client_83", key: "Brakes", parent: "power" },
    { id: 13, title: "client_84", key: "Transmission", parent: "power" },
    { id: 14, title: "client_85", key: "Horns", parent: "painting" },
    { id: 15, title: "client_86", key: "Suspension", parent: "body" },
    { id: 16, title: "client_87", key: "Armor", parent: "body" },
    { id: 22, title: "client_88", key: "Xenon", parent: "painting" },
    { id: 23, title: "client_89", key: "FrontWheels", parent: "body" },
    { id: 24, title: "client_90", key: "BackWheels", parent: "body" },
    { id: 25, title: "client_91", key: "Plateholders", parent: "body" },
    { id: 26, title: "client_92", key: "VanityPlates", parent: "body" },
    { id: 27, title: "client_93", key: "TrimDesign", parent: "body" },
    { id: 28, title: "client_94", key: "Ornaments", parent: "body" },
    { id: 29, title: "client_95", key: "Cabin", parent: "body" },
    { id: 30, title: "client_96", key: "DialDesign", parent: "body" },
    { id: 31, title: "client_97", key: "DoorDesign", parent: "body" },
    { id: 32, title: "client_98", key: "Seats", parent: "body" },
    { id: 33, title: "client_99", key: "SteeringWheel", parent: "body" },
    { id: 34, title: "client_100", key: "ShiftLever", parent: "body" },
    { id: 35, title: "client_101", key: "Plaques", parent: "body" },
    { id: 36, title: "client_102", key: "Speakers", parent: "body" },
    { id: 37, title: "client_103", key: "Trunk", parent: "body" },
    { id: 38, title: "client_104", key: "Hydraulics", parent: "body" },
    { id: 39, title: "client_105", key: "EngineBlock", parent: "body" },
    { id: 40, title: "client_106", key: "AirFilter", parent: "body" },
    { id: 41, title: "client_107", key: "Struts", parent: "body" },
    { id: 42, title: "client_108", key: "ArchCover", parent: "painting" },
    { id: 43, title: "client_109", key: "Aerials", parent: "body" },
    { id: 44, title: "client_110", key: "Trim", parent: "body" },
    { id: 46, title: "client_111", key: "WindowsTypes", parent: "body" },
    { id: 48, title: "client_112", key: "Livery", parent: "painting" },
    { id: 51, title: "client_113", key: "WheelsColor", parent: "painting" },
    { id: 53, title: "client_114", key: "NumberType", parent: "body" },
    { id: 54, title: "client_115", key: "PearlColor", parent: "painting" },
    { id: 55, title: "client_116", key: "WindowToning", parent: "body" },
    { id: 1000, title: "client_117", key: "Color", parent: "painting" },
    { id: 1002, title: "client_118", key: "SecColor", parent: "painting" },
    { id: 1001, title: "client_119", key: "Neon", parent: "painting" },
    { id: 1003, title: "client_169", key: "TyreSmokeColor", parent: "painting" },
    { id: 18, title: "client_120", key: "Turbo", parent: "power" },
];

let typePower = {
    0: "Lower",
    1: "Street",
    2: "Sport",
    3: "Race"
}

let xenonColor = {
    0: "client_121", //белый
    1: "client_122", //светло синий
    2: "client_123", //голубой
    3: "client_124", //зеленый
    4: "client_125", //лаймовый
    5: "client_126", //желтый
    6: "client_127", //оранжевый
    7: "client_128", //темно оранжевый
    8: "client_129", //красный
    9: "client_130", //розовый
    10: "client_131", //пурпурный
    11: "client_132", //фиолетовый
    12: "client_133", //синий
}
let wheelTypes = [
    { title: "Sport", key: 0, image: "Sport" },
    { title: "Muscle", key: 1, image: "Muscle" },
    { title: "Lowrider", key: 2, image: "Lowrider" },
    { title: "SUV", key: 3, image: "SUV" },
    { title: "Offroad", key: 4, image: "Offroad" },
    { title: "Tuner", key: 5, image: "Tuner" },
    { title: "Exclusive", key: 7, image: "Exclusive" },
    { title: "Benny Original", key: 8, image: "Benny1" },
    { title: "Benny Bespoke", key: 9, image: "Benny2" },
    { title: "F1", key: 10, image: "F1" },
]
let motoWhell = {
    title: "Motorcycles", key: 6, image: "Motorcycles"
}
let toning = [
    { title: "Stock", key: -1 },
    { title: "client_134", key: 1 }, //PURE_BLACK
    { title: "client_135", key: 2 }, //DARKSMOKE
    { title: "client_136", key: 3 }, //LIGHTSMOKE
    { title: "client_137", key: 5 }, //GREEN
]

let colorTypeItems = [
    { id: 0, title: 'client_138' },
    { id: 1, title: 'client_139' },
    { id: 3, title: 'client_140' },
    { id: 4, title: 'client_141' },
    { id: 5, title: 'client_142' }
]
//#region start

function GetTuningPars() {
    config = [];
    tuningTypes.forEach(index => {
        let count = mp.players.local.vehicle.getNumMods(index)
        if (count > 0)
            config[index] = count;
    });
}

mp.events.add('lsCustom:openTun', (pricePart, carModel, modelPr, components, vehclass, turboStage) => {

    modelCar = carModel; //Машина, в которой находится игрок
    modelPrice = modelPr; //цена текущей машины
    priceOnePart = pricePart; //цена за материал на текущем LSCustom
    cntTurboStage = turboStage; //количество уровней турбо на авто
    currVehClass = vehclass;
    GetTuningPars(); //загружаем конфиг
    vehicleComponents = JSON.parse(components);
    opened = global.gui.openPage('CarTunningMenu');
    if(!opened) return;
    global.gui.setData('carTunningMenu/setCurrentTab', JSON.stringify("FirstTab"));
    global.gui.setData('carTunningMenu/setSliderColors', JSON.stringify(global.RageColorsList));
    global.gui.setData('carTunningMenu/setColorTypeItems', JSON.stringify(colorTypeItems.map(item => ({ id: item.id, title: item.title}))));
    global.gui.setData('carTunningMenu/setSpecificationMaxValues', JSON.stringify({
        speed: 300,
        braking: 350,
        acceleration: 300,
        traction: 100
    }));
    global.gui.setData('carTunningMenu/setThirdTabData', JSON.stringify({ parent: "WheelsColor", items: null })); //сбрасываем третью страницу, чтоб предыдущий цвет в палитре не кэшировался
    DropState();
    //настройки камеры
    const pos = mp.players.local.position;
    global.customCamera.setPos(new mp.Vector3(pos.x, pos.y, pos.z + .6));
    global.customCamera.setPoint(new mp.Vector3(pos.x, pos.y, pos.z + .6));
    global.customCamera.moveCamZ(0);
    global.customCamera.setDist(4);
    global.customCamera.moveAngleX(160);
    global.customCamera.switchOn(0);
    global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
    global.gui.setData('mouseMove/setEnebled', true);

    global.gui.setData('carTunningMenu/setCurrentTab', JSON.stringify("FirstTab"));
    setTimeout(function () {
        global.showCursor(true);
    }, 100);
    InitPowerMenu();
    disableMods = fixTuningCar.find(item => mp.game.joaat(item.model) == mp.players.local.vehicle.model);

});

//#endregion

//#region Set SecondTab

//открывыаем список компонентов в категории
mp.events.add('lsCustom:openListCat', (parrent) => {

    let list = GetListCat(parrent);
    global.gui.setData('carTunningMenu/setSecondTabData', JSON.stringify(list));
    if (parrent == 'power')
        UpdatePowerMenu();
});

function GetListCat(currParent) {
    let currItems = fullTuning.filter(item =>
        item.parent == currParent &&
        CheckSolveMod(item.id) &&
        ((config[item.id] !== undefined && config[item.id] != null) ||
            (tuningTypesAll.includes(item.id) && currVehClass != 8) ||
            (tuningTypesAllMoto.includes(item.id) && currVehClass == 8)))
        .map(item => ({ title: item.title, key: item.key }));
    currentCategory = currParent;
    return { parent: currParent, items: currItems };
}

//#endregion

//#region Set ThirdTab

//открываем список модификаций в компоненте
mp.events.add('lsCustom:openListTun', (component) => {
    if (component == "Color" || component == "Neon" || component == "TyreSmokeColor") {
        global.gui.setData('carTunningMenu/setThirdCurrentMainColorType', JSON.stringify({ id: vehicleComponents.PaintTypePrim, title: colorTypeItems.find(item => item.id == vehicleComponents.PaintTypePrim).title}));
        global.gui.setData('carTunningMenu/setColorPickerMainColor', JSON.stringify(`${vehicleComponents.PrimColor.Red}, ${vehicleComponents.PrimColor.Green}, ${vehicleComponents.PrimColor.Blue}`));
        global.gui.setData('carTunningMenu/setThirdCurrentAdditionalColorType', JSON.stringify({ id: vehicleComponents.PaintTypeSec, title: colorTypeItems.find(item => item.id == vehicleComponents.PaintTypeSec) }));
        global.gui.setData('carTunningMenu/setColorPickerAdditionalColor', JSON.stringify(`${vehicleComponents.SecColor.Red}, ${vehicleComponents.SecColor.Green}, ${vehicleComponents.SecColor.Blue}`));
        
        let neonColor1 = vehicleComponents.NeonColors.length > 0 ? vehicleComponents.NeonColors[0] : {Red:0, Green:0, Blue:0, Alpha:0};
        let neonColor2 = vehicleComponents.NeonColors.length > 1 ? vehicleComponents.NeonColors[1] : {Red:0, Green:0, Blue:0, Alpha:0};
        global.gui.setData('carTunningMenu/setColorPickerNeonColor', JSON.stringify(`${neonColor1.Red}, ${neonColor1.Green}, ${neonColor1.Blue}`));
        global.gui.setData('carTunningMenu/setColorPickerNeon2Color', JSON.stringify(`${neonColor2.Red}, ${neonColor2.Green}, ${neonColor2.Blue}`));
        global.gui.setData('carTunningMenu/setColorPickerTyreSmokeColor', JSON.stringify(`${vehicleComponents.TyreSmokeColor.Red}, ${vehicleComponents.TyreSmokeColor.Green}, ${vehicleComponents.TyreSmokeColor.Blue}`));
    }
    let list = GetListTuning(component);
    global.gui.setData('carTunningMenu/setThirdTabData', JSON.stringify(list));
    let title = fullTuning.find(item => item.key == component).title;
    global.gui.setData('carTunningMenu/setThirdTabDataTitle', JSON.stringify(title));

    let compon = fullTuning.find(item => item.key == component);
    currComp = compon.id;
    currMod = GetNowSetMod(currentComponent);
    global.gui.setData('carTunningMenu/setCurrentSelectItem', JSON.stringify(currMod));
    switch (component) {
        case "Frame":
        case "Ornaments":
        case "Cabin":
        case "DialDesign":
        case "DoorDesign":
        case "Seats":
        case "SteeringWheel":
        case "ShiftLever":
        case "Speakers":
        case "Plaques":
            mp.players.local.vehicle.setDoorOpen(0, true, true); //двери
            mp.players.local.vehicle.setDoorOpen(1, true, true);
            break;
        case "EngineBlock":
        case "AirFilter":
        case "Struts":
            mp.players.local.vehicle.setDoorOpen(4, true, true); //капот
            break;
        case "Trunk":
        case "Hydraulics":
            mp.players.local.vehicle.setDoorOpen(5, true, true); //багажник
            break;
        case "Color":
            SetCurrentColor();
            break;
        case "Neon":
            SetCurrentNeon("Neon");
            SetCurrentNeon("Neon2");
            mp.players.local.vehicle.setEngineOn(true, true, !false);
            break;
        case "Xenon":
            mp.players.local.vehicle.setEngineOn(true, true, !false);
            mp.players.local.vehicle.setLights(2);
            break;
        case "TyreSmokeColor":
            mp.players.local.vehicle.setEngineOn(true, true, !false);
            mp.players.local.vehicle.setTyresCanBurst(false);
            mp.game.invoke('0xC429DCEEB339E129', mp.players.local.handle, mp.players.local.vehicle.handle, 30, 1000000);
            break;
    }
    if (currentCategory == 'power')
        UpdatePowerMenu();
    SetPrice(0);
});

function GetListTuning(component) {
    let listTuning = [];
    let currComp = fullTuning.find(item => item.key == component);
    listTuning.push({ title: "Stock", key: -1, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[1] });
    if (component == "Brakes" || component == "Transmission") {
        for (var i = 0; i < 3; i++) {
            listTuning.push({ title: `${typePower[i + 1]} ${component}`, key: i, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[i + 2] });
        }
    }
    else if (component == "Engine") {
        for (var i = 0; i < 4; i++) {
            listTuning.push({ title: `Stage${i + 1} ${component}`, key: i, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[i + 2] });
        }
    }
    else if (component == "Suspension") {
        for (var i = 0; i < 4; i++) {
            listTuning.push({ title: `${currComp.title}@${typePower[i]}`, key: i, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[i + 2] });
        }
    }
    else if (component == "Armor") {
        for (var i = 0; i < 5; i++) {
            listTuning.push({ title: `${currComp.title}@${(i + 1) * 20}%`, key: i, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[i + 2] });
        }
    }
    else if (component == "Turbo") {
        for (var i = 0; i < cntTurboStage; i++)
            listTuning.push({ title: `${currComp.title}@Stage${i + 1}`, key: i, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[i + 2] });
    }
    else if (component == "Xenon") {
        for (var i = 0; i < 13; i++) {
            listTuning.push({ title: `${xenonColor[i]}@${currComp.title}`, key: i, image: `thirdTab/${component}/Color${i}.png`, rank: tuningRanks[2] });
        }
    }
    else if (component == "NumberType") {
        listTuning = [];
        for (var i = 0; i < 6; i++) {
            listTuning.push({ title: `${currComp.title}@${i + 1}`, key: i, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[2] });
        }
    }
    else if (component == "WindowToning") {
        listTuning = [];
        toning.forEach(index => {
            listTuning.push({ title: index.title, key: index.key, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[2] });
        });
    }
    else if (component == "FrontWheels" || component == "BackWheels") {
        listTuning = [];

        if (currVehClass == 8) {
            listTuning.push({ title: motoWhell.title, key: motoWhell.key, image: `thirdTab/${component}/${motoWhell.image}.png`, rank: undefined });
        }
        else {
            wheelTypes.forEach(index => {
                listTuning.push({ title: index.title, key: index.key, image: `thirdTab/${component}/${index.image}.png`, rank: undefined });
            });
        }
    }
    //все остальные
    else if (config[currComp.id] != undefined && config[currComp.id] != null) {
        for (var i = 0; i < config[currComp.id]; i++) {
            listTuning.push({ title: `${currComp.title}@${i + 1}`, key: i, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[2] });
        }
    }
    //цвета
    else if (currComp.title != undefined && currComp.title != null) {
        listTuning = [];
        listTuning.push({ title:currComp.title, key: 0, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[1] });
        if (component == "Color")
            listTuning.push({ title: "Stock Pearl", key: 1, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[1] });
        if (component == "Neon")
            listTuning.push({ title: "Stock Neon", key: 1, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[1] });
        if (component == "TyreSmokeColor")
            listTuning.push({ title: "Stock Smoke Color", key: 1, image: `secondTab/${currentCategory}/${component}.png`, rank: tuningRanks[1] });
    }
    currentComponent = component;
    return { parent: component, items: listTuning };
}

function SetCurrentColor() {
    currentPearl = GetNowSetMod("PearlColor")
    currentMod1 = vehicleComponents.PaintTypePrim;
    currentMod2 = vehicleComponents.PaintTypeSec;
    currentFirstColor = {
        r: vehicleComponents.PrimColor.Red, g: vehicleComponents.PrimColor.Green, b: vehicleComponents.PrimColor.Blue
    };
    currentSecColor = {
        r: vehicleComponents.SecColor.Red, g: vehicleComponents.SecColor.Green, b: vehicleComponents.SecColor.Blue
    };
};

function SetCurrentNeon(type) {
    try {
        let neonColor = vehicleComponents.NeonColors;
        if (type == 'Neon')
            if (neonColor.length > 0)
                currentNeon = {
                    r: neonColor[0].Red, g: neonColor[0].Green, b: neonColor[0].Blue, a: neonColor[0].Alpha
                };
            else
                currentNeon = {
                    r: 0, g: 0, b: 0, a: 0
                };
        if (type == 'Neon2')
            if (neonColor.length > 1)
                currentNeon2 = {
                    r: neonColor[1].Red, g: neonColor[1].Green, b: neonColor[1].Blue, a: neonColor[1].Alpha
                };
            else
                currentNeon2 = {
                    r: 0, g: 0, b: 0, a: 0
                };
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in lscustoms.SetCurrentNeon: ${e.name}\n${e.message}\n${e.stack}`);
    }
};

function SetCurrentTyreSmokeColor() {
    try {
        currentTyreSmokeColor = {
            r: vehicleComponents.TyreSmokeColor.Red, g: vehicleComponents.TyreSmokeColor.Green, b: vehicleComponents.TyreSmokeColor.Blue, a: vehicleComponents.TyreSmokeColor.Alpha
        };
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in lscustoms.SetCurrentTyreSmokeColor: ${e.name}\n${e.message}\n${e.stack}`);
    }
};

//#endregion

//#region Set WheelTab

//получаем список дисков заданного типа
mp.events.add('lsCustom:chooseWheelType', (wheel) => {
    let list = GetWheelList(wheel);
    global.gui.setData('carTunningMenu/setFourthTabData', JSON.stringify(list));
    let wheelType = wheelTypes.find(item => item.key == wheel);
    if (currVehClass == 8)
        wheelType = motoWhell;
    global.gui.setData('carTunningMenu/setFourthTabDataTitle', JSON.stringify(wheelType.title));
    if ((GetNowSetMod("WheelsType") == currWheelType) || (GetNowSetMod(currentComponent) == -1))
        global.gui.setData('carTunningMenu/setCurrentSelectItem', JSON.stringify(GetNowSetMod(currentComponent)));
});

function GetWheelList(wheel) {
    mp.players.local.vehicle.setWheelType(wheel);
    currWheelType = wheel;
    let listWheels = [];
    let wheelType = wheelTypes.find(item => item.key == wheel);
    if (currVehClass == 8)
        wheelType = motoWhell;
    keys = Object.keys(global.tuningWheels[wheel]);
    listWheels.push({ title: "Stock", key: -1, image: `thirdTab/${currentComponent}/${wheelType.image}.png`, rank: tuningRanks[1] });
    for (var i = 1; i < keys.length; i++) {
        listWheels.push({ title: `${wheelType.title} ${i}`, key: i - 1, image: `thirdTab/${currentComponent}/${wheelType.image}.png`, rank: tuningRanks[2] });
    }
    currentWheelType = wheelType.title;
    return { parent: currentComponent, items: listWheels };
}

//#endregion

//#region ChangeMod

//смена детали
mp.events.add('lsCustom:clickTun', (comp, variant) => {
    let compon = fullTuning.find(item => item.key == comp);
    switch (comp) {
        case "Color":
            if (variant == 1) {
                currentPearl = -1;
                UpdateColor();
            }
            break;
        case "Neon":
            if (variant == 1) {
                currentNeon = {
                    r: 0, g: 0, b: 0, a: 0
                };
                currentNeon2 = {
                    r: 0, g: 0, b: 0, a: 0
                };
                UpdateNeon();
            }
            break;
        case "TyreSmokeColor":
            if (variant == 1) {
                currentTyreSmokeColor = {
                    r: 0, g: 0, b: 0, a: 0
                };
                UpdateTyreSmokeColor();
            }
            break;
        case "WheelsColor":
            break;
        default:
            if (compon != undefined && compon != null)
                ChangeModClick(comp, variant);
            break;
    }
    if (currentCategory == 'power')
        UpdatePowerMenu();
    global.gui.setData('carTunningMenu/setCurrentSelectItem', JSON.stringify(variant));
});

function ChangeModClick(comp, variant) {
    let compon = fullTuning.find(item => item.key == comp);
    switch (comp) {
        case "Horns":
            mp.players.local.vehicle.setMod(compon.id, variant);
            mp.players.local.vehicle.startHorn(2000, 0, true);
            break;
        case "WindowToning":
            mp.players.local.vehicle.setWindowTint(variant);
            break;
        case "NumberType":
            mp.players.local.vehicle.setNumberPlateTextIndex(variant);
            break;
        case "Xenon":
            if (variant >= 0) {
                mp.players.local.vehicle.setMod(22, 0);
                mp.game.invoke('0xE41033B25D003A07', mp.players.local.vehicle.handle, parseInt(variant));
            }
            else
                mp.players.local.vehicle.setMod(22, -1);
            break;
        default:
            if (compon != undefined && compon != null) {
                mp.players.local.vehicle.setMod(compon.id, variant);
            }
            break;
    }
    currComp = compon.id;
    currMod = variant;
    updatePrice();
}


//смена цвета в палитре
mp.events.add('lsCustom:changeColor', (r, g, b, type) => {
    if (type == "Color") {
        currentFirstColor = {
            r: r, g: g, b: b
        };
        UpdateColor()
    }
    else if (type == "SecColor") {
        currentSecColor = {
            r: r, g: g, b: b
        };
        UpdateColor()
    }
    else if (type == "Neon") {
        currentNeon = {
            r: r, g: g, b: b, a: 1
        };
        UpdateNeon();
    }
    else if (type == "Neon2") {
        currentNeon2 = {
            r: r, g: g, b: b, a: 1
        };
        UpdateNeon();
    }
    else if (type == "TyreSmokeColor") {
        if (r == 0 && g == 0 && b == 0)
            r = 1;
        currentTyreSmokeColor = {
            r: r, g: g, b: b, a: 1
        };
        UpdateTyreSmokeColor();
    }
});

//скрытие палитры
mp.events.add('lsCustom:changeToggleIsPicker', (isPicker, type) => {
    switch (type) {
        case "Color":
            if (!isPicker) {
                currentMod1 = vehicleComponents.PaintTypePrim;
                currentFirstColor = {
                    r: vehicleComponents.PrimColor.Red, g: vehicleComponents.PrimColor.Green, b: vehicleComponents.PrimColor.Blue
                };
                UpdateColor();
            }
            break;
        case "SecColor":
            if (!isPicker) {
                currentMod2 = vehicleComponents.PaintTypeSec;
                currentSecColor = {
                    r: vehicleComponents.SecColor.Red, g: vehicleComponents.SecColor.Green, b: vehicleComponents.SecColor.Blue
                };
                UpdateColor();
            }
            break;
        case "Neon":
        case "Neon2":
            if (!isPicker) {
                SetCurrentNeon(type);
                UpdateNeon();
            }
            break;
        case "TyreSmokeColor":
            if (!isPicker) {
                SetCurrentTyreSmokeColor();
                UpdateTyreSmokeColor();
            }
            break;
    }
});

//смена цвета ползунком
mp.events.add('lsCustom:changeColorSlider', (color) => {
    switch (currentComponent) {
        case "Color":
            currentPearl = color;
            UpdateColor();
            break;
        case "WheelsColor":
            mp.players.local.vehicle.setExtraColours(GetNowSetMod("PearlColor") == -1 ? 0 : GetNowSetMod("PearlColor"), color);
            currComp = 51;
            currMod = color;
            updatePrice();
            break;
    }
});

//скрытие ползунка
mp.events.add('lsCustom:changeToggleIsSlider', (isSlider) => {
    switch (currentComponent) {
        case "Color":
            currentPearl = GetNowSetMod("PearlColor");
            UpdateColor();
            break;
        case "WheelsColor":
            mp.players.local.vehicle.setExtraColours(GetNowSetMod("PearlColor") == -1 ? 0 : GetNowSetMod("PearlColor"), GetNowSetMod("WheelsColor") == -1 ? 0 : GetNowSetMod("WheelsColor"));
            currComp = 51;
            currMod = GetNowSetMod("WheelsColor");
            updatePrice();
            break;
    }
});


//смена типа покраски
mp.events.add('lsCustom:changePaintType', (paintType, type) => {
    if (type == 'Color') {
        currentMod1 = paintType;
    }
    else if (type == 'SecColor') {
        currentMod2 = paintType;
    }
    UpdateColor();
});


//обновление цвета
function UpdateColor() {
    mp.players.local.vehicle.setModColor1(currentMod1, 0, 0);
    mp.players.local.vehicle.setModColor2(currentMod2, 0);
    mp.players.local.vehicle.setCustomPrimaryColour(currentFirstColor.r, currentFirstColor.g, currentFirstColor.b);
    mp.players.local.vehicle.setCustomSecondaryColour(currentSecColor.r, currentSecColor.g, currentSecColor.b);
    mp.players.local.vehicle.setExtraColours(currentPearl == -1 ? 0 : currentPearl, GetNowSetMod("WheelsColor") == -1 ? 0 : GetNowSetMod("WheelsColor"));
    updatePrice();
};

//обновление цвета неона
function UpdateNeon() {
    let vehicle = mp.players.local.vehicle;
    let neonColor = [];
    if (currentNeon.a > 0)
        neonColor.push([currentNeon.r, currentNeon.g, currentNeon.b, currentNeon.a ]);
    if (currentNeon2.a > 0) {
        if (currentNeon.a <= 0)
            neonColor.push([0, 0, 0, 0 ]);
        neonColor.push([currentNeon2.r, currentNeon2.g, currentNeon2.b,  currentNeon2.a]);
    }
    global.VehicleNeons.delete(vehicle);
    if (neonColor.length > 0) {
        vehicle.setNeonLightEnabled(0, true);
        vehicle.setNeonLightEnabled(1, true);
        vehicle.setNeonLightEnabled(2, true);
        vehicle.setNeonLightEnabled(3, true);
        vehicle.setNeonLightsColour(neonColor[0][0], neonColor[0][1], neonColor[0][2]);
        if (neonColor.length > 1)
            global.VehicleNeons.set(vehicle, { colors: neonColor, currColor: 0 });
    }
    else {
        vehicle.setNeonLightEnabled(0, false);
        vehicle.setNeonLightEnabled(1, false);
        vehicle.setNeonLightEnabled(2, false);
        vehicle.setNeonLightEnabled(3, false);
    }
    updatePrice();
};

//обновление цвета дыма шин
function UpdateTyreSmokeColor() {
    mp.players.local.vehicle.toggleMod(20, true);
    if (currentTyreSmokeColor.a > 0) {
        mp.players.local.vehicle.setTyreSmokeColor(currentTyreSmokeColor.r, currentTyreSmokeColor.g, currentTyreSmokeColor.b);
    }
    else {
        mp.players.local.vehicle.setTyreSmokeColor(240, 240, 240);
    }


    updatePrice();
};

function InitPowerMenu() {
    let currPowers = GetNowSetMod("Engine") + GetNowSetMod("Turbo") + 2;
    let lscSpeed = mp.game.vehicle.getVehicleModelMaxSpeed(mp.players.local.vehicle.model) / 1.2 + currPowers * 2;
    let lscBrakes = mp.players.local.vehicle.getMaxBraking() * 100;
    let lscBoost = (mp.players.local.vehicle.getAcceleration() + currPowers * 0.1) * 100;
    let lscClutch = mp.players.local.vehicle.getMaxTraction() * 10;
    global.gui.setData('carTunningMenu/setSpecificationsValues', JSON.stringify({
        title: modelCar,
        speed: Math.floor(lscSpeed * 10) / 10,
        braking: Math.floor(lscBrakes * 10) / 10,
        acceleration: Math.floor(lscBoost * 10) / 10,
        traction: Math.floor(lscClutch * 10) / 10
    }));
}

function UpdatePowerMenu() {
    let currPowers = (currentComponent == "Engine" ? currMod : GetNowSetMod("Engine")) + (currentComponent == "Turbo" ? currMod : GetNowSetMod("Turbo")) + 2;
    let lscSpeed = mp.game.vehicle.getVehicleModelMaxSpeed(mp.players.local.vehicle.model) / 1.2 + currPowers * 2;
    let lscBrakes = mp.players.local.vehicle.getMaxBraking() * 100;
    let lscBoost = (mp.players.local.vehicle.getAcceleration() + currPowers * 0.1) * 100;
    let lscClutch = mp.players.local.vehicle.getMaxTraction() * 10;
    global.gui.setData('carTunningMenu/setSpecificationsPossibleValues', JSON.stringify({
        speed: Math.floor(lscSpeed * 10) / 10,
        braking: Math.floor(lscBrakes * 10) / 10,
        acceleration: Math.floor(lscBoost * 10) / 10,
        traction: Math.floor(lscClutch * 10) / 10
    }));
}
//#region  Update price

function GetPriceByMod(component, modelPrice, variant, wheelType = -2) {

    if ((global.tuningPartsPrice == undefined || global.tuningPartsPrice[component] == undefined) && component != "FrontWheels" && component != "BackWheels")
        return 0;
    if (variant == -1)
        return global.tuningPartsPrice[component].Stock;
    let price;
    if (component == "FrontWheels" || component == "BackWheels")
        price = global.tuningWheels[wheelType][variant];
    else
        price = global.tuningPartsPrice[component].Modifire;
    let modelKoef = modelPrice / percentCarPrice * global.tuningPartsPrice[component].KoefCar + 1;
    return Math.floor((price + Math.floor(global.tuningPartsPrice[component].Step * variant)) * modelKoef);
};

function updatePrice() {
    try {
        currPrice = 0;
        switch (currentComponent) {
            case "Color":
                if (vehicleComponents.PaintTypePrim != currentMod1 || vehicleComponents.PrimColor.Red != currentFirstColor.r || vehicleComponents.PrimColor.Green != currentFirstColor.g || vehicleComponents.PrimColor.Blue != currentFirstColor.b)
                    currPrice += Math.floor(global.tuningColorPrice[currentMod1].Stock * (modelPrice / percentCarPrice * global.tuningColorPrice[currentMod1].KoefCar + 1));
                if (vehicleComponents.PaintTypeSec != currentMod2 || vehicleComponents.SecColor.Red != currentSecColor.r || vehicleComponents.SecColor.Green != currentSecColor.g || vehicleComponents.SecColor.Blue != currentSecColor.b)
                    currPrice += Math.floor(global.tuningColorPrice[currentMod2].Stock * (modelPrice / percentCarPrice * global.tuningColorPrice[currentMod2].KoefCar + 1));
                if (currentPearl != GetNowSetMod("PearlColor")) {
                    if (currentPearl == -1)
                        currPrice += global.tuningColorPrice[2].Stock;
                    else
                        currPrice += global.tuningColorPrice[2].Modifire;
                }
                break;
            case "Neon":
                let colors = vehicleComponents.NeonColors;
                if (colors.length <= 0 || (colors[0].Red != currentNeon.r || colors[0].Green != currentNeon.g || colors[0].Blue != currentNeon.b || colors[0].Alpha != currentNeon.a))
                    if (currentNeon.a <= 0)
                        currPrice += global.tuningColorPrice[6].Stock * (modelPrice / percentCarPrice * global.tuningColorPrice[6].KoefCar + 1);
                    else
                        currPrice += global.tuningColorPrice[6].Modifire * (modelPrice / percentCarPrice * global.tuningColorPrice[6].KoefCar + 1);
                if (colors.length <= 1 || (colors[1].Red != currentNeon2.r || colors[1].Green != currentNeon2.g || colors[1].Blue != currentNeon2.b || colors[1].Alpha != currentNeon2.a))
                    if (currentNeon2.a <= 0)
                        currPrice += global.tuningColorPrice[8].Stock * (modelPrice / percentCarPrice * global.tuningColorPrice[8].KoefCar + 1);
                    else
                        currPrice += global.tuningColorPrice[8].Modifire * (modelPrice / percentCarPrice * global.tuningColorPrice[8].KoefCar + 1);
                break;
            case "TyreSmokeColor":
                if (vehicleComponents.TyreSmokeColor.Red != currentTyreSmokeColor.r || vehicleComponents.TyreSmokeColor.Green != currentTyreSmokeColor.g || vehicleComponents.TyreSmokeColor.Blue != currentTyreSmokeColor.b || vehicleComponents.TyreSmokeColor.Alpha != currentTyreSmokeColor.a)
                    if (currentTyreSmokeColor.a <= 0)
                        currPrice += global.tuningColorPrice[7].Stock * (modelPrice / percentCarPrice * global.tuningColorPrice[7].KoefCar + 1);
                    else
                        currPrice += global.tuningColorPrice[7].Modifire * (modelPrice / percentCarPrice * global.tuningColorPrice[7].KoefCar + 1);
                break;
            case "FrontWheels":
            case "BackWheels":
                if ((GetNowSetMod(currentComponent) == currMod && GetNowSetMod("WheelsType") == currWheelType) || (GetNowSetMod(currentComponent) == currMod && currMod == -1))
                    currPrice = 0;
                else
                    currPrice = GetPriceByMod(currentComponent, modelPrice, currMod, currWheelType);
                break;
            default:
                if (GetNowSetMod(currentComponent) == currMod)
                    currPrice = 0;
                else
                    currPrice = GetPriceByMod(currentComponent, modelPrice, currMod, currWheelType);
                break;
        }
        SetPrice(currPrice);
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in lscustoms.updatePrice: ${e.name}\n${e.message}\n${e.stack}`);
    }
};

function SetPrice(price) {
    price *= priceOnePart;
    global.gui.setData('carTunningMenu/setCurrentItemPrice', JSON.stringify(price));
    if (price > 0)
        global.gui.setData('carTunningMenu/setIsBuyEnable', JSON.stringify(true));
    else
        global.gui.setData('carTunningMenu/setIsBuyEnable', JSON.stringify(false));
}

//#endregion


//#endregion

//#region Set BackTab

mp.events.add('lsCustom:backPage', (page) => {
    DropState();
    let compon = fullTuning.find(item => item.key == currentComponent);
    if (page == "SecondTab" && currentComponent != "FrontWheels" && currentComponent != "BackWheels") {
        //откатываем тюнинг
        switch (currentComponent) {
            case "Color":
                SetCurrentColor();
                UpdateColor();
                break;
            case "Neon":
                SetCurrentNeon("Neon");
                SetCurrentNeon("Neon2");
                UpdateNeon();
                break;
            case "TyreSmokeColor":
                SetCurrentTyreSmokeColor();
                UpdateTyreSmokeColor();
                break;
            case "WheelsColor":
                mp.players.local.vehicle.setExtraColours(GetNowSetMod("PearlColor") == -1 ? 0 : GetNowSetMod("PearlColor"), GetNowSetMod(currentComponent));
                break;
            case "WindowToning":
                mp.players.local.vehicle.setWindowTint(GetNowSetMod(currentComponent));
                break;
            case "NumberType":
                mp.players.local.vehicle.setNumberPlateTextIndex(GetNowSetMod(currentComponent));
                break;
            default:
                mp.players.local.vehicle.setMod(compon.id, GetNowSetMod(currentComponent));
                break;
        }
        currentComponent = "";
        global.gui.setData('carTunningMenu/setThirdTabData', JSON.stringify({ parent: "WheelsColor", items: null }));
    }
    else if (page == "ThirdTab") {
        //откатываем тюнинг колес
        mp.players.local.vehicle.setWheelType(GetNowSetMod("WheelsType"));
        mp.players.local.vehicle.setMod(compon.id, GetNowSetMod(currentComponent));
    }
    else if (page == "FirstTab") {
        currentCategory = "";
    }

    if (currentCategory == 'power')
        UpdatePowerMenu();

    global.gui.setData('carTunningMenu/setCurrentSelectItem', JSON.stringify(-2));
});

function DropState() {
    mp.players.local.clearTasks();
    mp.players.local.vehicle.setDoorShut(0, false);
    mp.players.local.vehicle.setDoorShut(1, false);
    mp.players.local.vehicle.setDoorShut(4, false);
    mp.players.local.vehicle.setDoorShut(5, false);
    mp.players.local.vehicle.setEngineOn(false, true, !false);
    mp.players.local.vehicle.setLights(0);
    mp.players.local.vehicle.setTyresCanBurst(true);
    currComp = -1;
    currMod = -1;
    currWheelType = -1;
}

//#endregion



//#region Buy tuning

mp.events.add('lsCustom:buyTuning', () => {
    if (currPrice == 0)
        return;
    switch (currentComponent) {
        case "Color":
            mp.events.callRemote('lsCustom:buyColor', currentFirstColor.r, currentFirstColor.g, currentFirstColor.b, currentSecColor.r, currentSecColor.g, currentSecColor.b, currentMod1, currentMod2, currentPearl);
            break;
        case "Neon":
            mp.events.callRemote('lsCustom:buyNeon', currentNeon.r, currentNeon.g, currentNeon.b, currentNeon.a, 'neon', currentNeon2.r, currentNeon2.g, currentNeon2.b, currentNeon2.a);
            break;
        case "TyreSmokeColor":
            mp.events.callRemote('lsCustom:buyNeon', currentTyreSmokeColor.r, currentTyreSmokeColor.g, currentTyreSmokeColor.b, currentTyreSmokeColor.a, 'tyresmoke');
            break;
        default:
            mp.events.callRemote('lsCustom:buyModTuning', currComp, currMod, currWheelType);
            break;
    }
});



//#endregion


//#region Function

function GetNowSetMod(tuningName) {
    keys = Object.keys(vehicleComponents.Components);
    let nowSetMod = keys.find(item => item == tuningName);
    if ((nowSetMod === undefined || nowSetMod == null))
        return -1;
    else
        return vehicleComponents.Components[nowSetMod];
}

function CheckSolveMod(mod) {
    if (disableMods === undefined)
        return true;
    else {
        if (disableMods.fixmod.includes(mod))
            return false;
        else return true;
    }
}

//#endregion



mp.events.add('tuningUpd', function (components) {
    vehicleComponents = JSON.parse(components);
    switch (currentComponent) {
        case "Color":
            SetCurrentColor();
            break;
        case "Neon":
            SetCurrentNeon("Neon");
            SetCurrentNeon("Neon2");
            break;
        case "TyreSmokeColor":
            SetCurrentTyreSmokeColor();
            break;
        default:
            currComp = 0;
            currMod = 0;
            break;
    }
    SetPrice(0);
});


mp.events.add('lsCustom:exitTun', () => {
    ExitTuning();
});

function ExitTuning() {
    global.gui.setData('mouseMove/setEnebled', false);
    global.gui.close();
    mp.events.callRemote('lsCustom:exitTuning');
    global.customCamera.switchOff(0);
    opened = false;
    DropState();
    global.showCursor(false)
}

// mp.keys.bind(global.Keys.Key_F7, false, function () {
//     if (opened)
//         ExitTuning();
// });
mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        ExitTuning();
});
