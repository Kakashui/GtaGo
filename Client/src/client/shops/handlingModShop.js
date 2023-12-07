const keyList = require('./../vehiclesync/handlingKeys');


let itemValues = {
    [1]: {//Масса
        onlyAdmin: true,
        value: {
            min: 0,
            max: 20000,
            step: 10,
            current: 1500
        },
    },
    [2]: {//Коэф. аэродинамического сопротивления
        onlyAdmin: true,
        value: {
            min: 0,
            max: 120,
            step: 0.01,
            current: 1
        },
    },
    [6]: {//Привод тс
        onlyAdmin: false,
        value: {
            min: 0,
            max: 1,
            step: 0.01,
            current: 0.5
        },
    },
    [7]: {//Количество передач
        onlyAdmin: true,
        value: {
            min: 1,
            max: 16,
            step: 1,
            current: 8
        },
    },
    [8]: {//множитель тягового усиления
        onlyAdmin: true,
        value: {
            min: 0.01,
            max: 2,
            step: 0.01,
            current: 1
        },
    },
    [9]: {//Множитель инерции
        onlyAdmin: true,
        value: {
            min: 0.01,
            max: 5,
            step: 0.01,
            current: 1
        },
    },
    [10]: {//множитель сцепления при смещении вверх;
        onlyAdmin: true,
        value: {
            min: 0.1,
            max: 10,
            step: 0.1,
            current: 5
        },
    },
    [11]: {//множитель сцепления при смещении вниз;
        onlyAdmin: true,
        value: {
            min: 0.1,
            max: 10,
            step: 0.1,
            current: 6
        },
    },
    [12]: {//теоретическая максимальная скорость
        onlyAdmin: true,
        value: {
            min: 0,
            max: 500,
            step: 1,
            current: 200
        },
    },
    [13]: {//множитель силы торможения
        onlyAdmin: false,
        value: {
            min: 0.0,
            max: 5,
            step: 0.1,
            current: 2
        },
        max: 2,
    },
    [14]: {//смещение тормозов
        onlyAdmin: false,
        value: {
            min: 0.01,
            max: 1,
            step: 0.01,
            current: 0.65
        },
    },
    [15]: {//мощность ручного тормоза
        onlyAdmin: false,
        value: {
            min: 0.1,
            max: 5,
            step: 0.1,
            current: 1
        },
        max: 2,
    },
    [16]: {//Угол поворота руля
        onlyAdmin: false,
        value: {
            min: 0.01,
            max: 2,
            step: 0.01,
            current: 1
        },
        max: 1.5,
    },
    [17]: {//сцепление колес на повороте
        onlyAdmin: false,
        value: {
            min: 0.1,
            max: 10,
            step: 0.1,
            current: 4
        },
        max: 5,
    },
    [18]: {//пробуксовка колес
        onlyAdmin: false,
        value: {
            min: 0.1,
            max: 10,
            step: 0.1,
            current: 4
        },
        max: 5,
    },
    [19]: {//боковая сила сцепления
        onlyAdmin: false,
        value: {
            min: 0.01,
            max: 2,
            step: 0.01,
            current: 1
        },
    },
    [20]: {//расстояние от земли, при котором транспортное средство теряет сцепление
        onlyAdmin: true,
        value: {
            min: 0.01,
            max: 1,
            step: 0.01,
            current: 0.25
        },
    },
    [21]: {//параметр, отвечающий за силу сцепления транспортного средства при низкой скорости (значение 0.0, //исходная сила сцепления);
        onlyAdmin: true,
        value: {
            min: 0.01,
            max: 1,
            step: 0.01,
            current: 0.5
        },
    },
    [23]: {//параметр, отвечающий за распределение тяги между передними и задними колесами (от 0.01 до 0.99);
        onlyAdmin: true,
        value: {
            min: 0.01,
            max: 1,
            step: 0.01,
            current: 0.5
        },
    },
    [24]: {//сцепления шины с дорогой;
        onlyAdmin: false,
        value: {
            min: 0.1,
            max: 2,
            step: 0.1,
            current: 1
        },
    },
    [25]: {//сила подвески
        onlyAdmin: false,
        value: {
            min: 0.01,
            max: 10,
            step: 0.01,
            current: 1
        },
        max: 5,
    },
    [26]: {//вибрация подвески;
        onlyAdmin: false,
        value: {
            min: 0.01,
            max: 5,
            step: 0.01,
            current: 0.1
        },
        max: 1,
    },
    [27]: {//вибрация подвески на высоких скоростях
        onlyAdmin: false,
        value: {
            min: 0.01,
            max: 5,
            step: 0.01,
            current: 0.1
        },
        max: 1,
    },
    [28]: {//максимальная высота кузова транспортного средства;
        onlyAdmin: true,
        value: {
            min: 0.001,
            max: 0.5,
            step: 0.001,
            current: 1
        },
    },
    [29]: {//минимальная высота кузова транспортного средства;
        onlyAdmin: true,
        value: {
            min: -0.5,
            max: 0.5,
            step: 0.01,
            current: -0.1
        },
    },
    [30]: {//высота подвески;
        onlyAdmin: false,
        value: {
            min: -1,
            max: 2,
            step: 0.01,
            current: 0.1
        },
        min: 0,
        max: 0.4,
    },
    [31]: {//смещение подвески вперед
        onlyAdmin: true,
        value: {
            min: 0.1,
            max: 2,
            step: 0.1,
            current: 1
        },
    },
    [32]: {//сила стабилизатора подвески;
        onlyAdmin: false,
        value: {
            min: 0.01,
            max: 5,
            step: 0.01,
            current: 1
        },
    },
    [33]: {//смещение силы стабилизатора подвески вперед
        onlyAdmin: true,
        value: {
            min: 0.01,
            max: 5,
            step: 0.01,
            current: 1
        },
    },
    [34]: {//высота центра крена (перед)
        onlyAdmin: true,
        value: {
            min: 0.01,
            max: 0.5,
            step: 0.01,
            current: 1
        },
    },
    [35]: {//высота центра крена (зад)
        onlyAdmin: true,
        value: {
            min: 0.01,
            max: 0.5,
            step: 0.01,
            current: 0.2
        },
    },
    [999]: {
        onlyAdmin: false,
        value: {
            current: false,
        },
    },
};

global.shopReduce = false;

let openedMenu = false;
let inShop = false;

function GetItemByKey(vehicle, isAdmin, keyID) {
    if (keyID == 999)
        return {
            onlyAdmin: itemValues[keyID].onlyAdmin,
            defaultValue: false,
            value: {
                current: global.getVariable(vehicle, `veh:handl:999`, false)
            }
        }
    else {
        let def = global.GetDefaultHandling(vehicle, keyList[keyID]);
        let currVal = global.getSharedHandling(vehicle, keyID);
        let result = {
            onlyAdmin: itemValues[keyID].onlyAdmin,
            value: {
                min: itemValues[keyID].value.min,
                max: itemValues[keyID].value.max,
                step: itemValues[keyID].value.step,
                current: currVal
            },
            defaultValue: def,
        }
        if (!isAdmin && !result.onlyAdmin) {
            if (itemValues[keyID].max != undefined)
                result.value.max = itemValues[keyID].max;

            if (itemValues[keyID].min != undefined)
                result.value.min = itemValues[keyID].min;
        }

        if (result.value.current > result.value.max)
            result.value.max = result.value.current

        if (result.value.current < result.value.min)
            result.value.min = result.value.current
        return result;
    }
}


mp.events.add("handlingShop:openMenu", (pricePart, priceCar, isAdmin) => {
    let vehicle = mp.players.local.vehicle;
    let currentHandlings = {};
    Object.keys(keyList).forEach(keyID => {
        currentHandlings[keyID] = GetItemByKey(vehicle, isAdmin, keyID);
    });

    global.gui.setData('handlingModShop/setData', JSON.stringify(
        {
            items: currentHandlings,
            isAdmin: isAdmin,
            priceCar: priceCar,
            pricePart: pricePart,
        }));
    global.gui.setData('handlingModShop/showMenu');
    openedMenu = true;
    opened = global.gui.openPage('HandlingModShop');
    inShop = true;
    global.shopReduce = false;
});

mp.events.add("hmodshop:value:change", (keyID, value) => {
    if (keyID == 999)
        global.shopReduce = true;
    else
        mp.players.local.vehicle.setHandling(keyList[keyID], value);
});


mp.keys.bind(global.Keys.Key_F2, false, function () {
    if (inShop) {
        if (openedMenu) {
            global.gui.setData('handlingModShop/hideMenu');
            openedMenu = false;
            global.showCursor(false)
        }
        else {
            global.gui.setData('handlingModShop/showMenu');
            openedMenu = true;
            global.showCursor(true)
        }
    }
});

mp.events.add('hmodshop:exit', () => {
    if (inShop)
        ExitMenu();
});

function ExitMenu() {
    mp.events.callRemote('handlingShop:closeMenu');
    global.gui.close();
    openedMenu = false;
    inShop = false;
    global.VehicleSetSharedDataHandlingMods(mp.players.local.vehicle);
    global.shopReduce = false;
    global.reduce = false;
    mp.players.local.vehicle.setReduceGrip(global.reduce);
}

mp.events.add("VehStream_PlayerExitVehicleAttempt", (vehicle, engState) => {
    if (inShop)
        ExitMenu();
});


mp.events.add("handlingShop:update", (keyID, value) => {
    if (value !== null)
        global.gui.setData('handlingModShop/updateData', JSON.stringify({ key: keyID, current: keyID == 999 ? value : Math.round(value * 1000) / 1000 }));
    else
        global.gui.setData('handlingModShop/updateData', JSON.stringify({ key: keyID, current: keyID == 999 ? false : global.GetDefaultHandling(mp.players.local.vehicle, keyList[keyID]) }));
});

// F - seats in driver
mp.events.add('render', () => {
    if (inShop)
        mp.game.controls.disableControlAction(0, 75, true);
});