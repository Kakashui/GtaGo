const keyList = require('./handlingKeys')


global.reduce = false;
let handlingData = {};
// let keyList = [

//     'handlingName', //параметр, отвечающий за название транспортного средства в игре (макс. 14 прописных букв, может также содержать цифры);
//     'fMass', //параметр, отвечающий за массу транспортного средства в килограммах. (от 0.0 до 10000.0 и более);
//     'fInitialDragCoeff', //начальный коэффициент аэродинамического сопротивления (от 0 до 120);
//     'fPercentSubmerged', //процент от высоты транспортного средства в воде, при котором оно начинает «плыть», погружаясь затем в воду. (любое значение с плавающей точкой, например 0.85 - равно 85 %).
//     'vecCentreOfMassOffset', //координаты x, y, z, отвечающие за вектор центра массы (от -10.0 до 10.0);x - положительное значение смещает центр тяжести вправо.y - положительное значение смещает центр тяжести вперед.z - положительное значение смещает центр тяжести вверх.
//     'vecInertiaMultiplier', //координаты x, y, z, отвечающие за множитель вектора инерции (от -10. до 10.0);


//     //TransmissionParams
//     'fDriveBiasFront', //параметр, отвечающий за привод транспортного средства. Так, значение 0.0 определяет заднеприводный транспорт; 1.0', //переднеприводный; 0.5', //полноприводный транспорт (на 4 колеса).
//     'nInitialDriveGears', //параметр, отвечающий за количество передач (от 1 до 16);
//     'fInitialDriveForce', //множитель тягового усиления (ускорение) транспортного средства (от 0.01 до 2.0. Значение 1.0 не влияет на множитель.);
//     'fDriveInertia', //множитель, отвечающий за инерцию двигателя (за то, как быстро набираются обороты, от 0.01 до 2.0. Значение 1.0 не влияет на множитель.);
//     'fClutchChangeRateScaleUpShift', //множитель сцепления при смещении вверх;
//     'fClutchChangeRateScaleDownShift', //множитель сцепления при смещении вниз;
//     'fInitialDriveMaxFlatVel', //теоретическая максимальная скорость транспортного средства в км/ч (от 0.00 до 500.0 и более. Изменение данного значения не гарантирует достижение этой скорости); Умножьте значение из файла на 0.82 чтобы получить скорость в миля/час Умножьте значение из файла на 1.32 чтобы получить скорость в км/час
//     'fBrakeForce', //множитель силы торможения транспортного средства (от 0.01 до 2.0. Значение 1.0 не влияет на множитель);
//     'fBrakeBiasFront', //параметр, отвечающий за равномерное распределение силы торможения между передней и задней осей (от 0.0 до 1.0. Значение 0.0 увеличивает силу торможения только для задней оси; 1.0', //для передней оси; 0.5', //равномерно. В жизни, автомобили как правила имеют более высокую силу торможения передней оси, например 0.65);
//     'fHandBrakeForce', //параметр, отвечающий за мощность ручного тормоза транспортного средства;
//     'fSteeringLock', //множитель максимального угла поворота руля (от 0.01 до 2.0. Значение в радиусе 0.1', //0.2 сымитирует транспорт с удлиненной колесной базой. Высокое значение данного параметра приведет к заносу);


//     'fTractionCurveMax', //параметр, отвечающий за сцепление колес на повороте;
//     'fTractionCurveMin', //параметр, отвечающий за пробуксовку колес;
//     'fTractionCurveLateral', //параметр, отвечающий за боковую силу сцепления;
//     'fTractionSpringDeltaMax', //параметр, отвечающий за расстояние от земли, при котором транспортное средство теряет сцепление;
//     'fLowSpeedTractionLossMult', //параметр, отвечающий за силу сцепления транспортного средства при низкой скорости (значение 0.0', //исходная сила сцепления);
//     'fCamberStiffnesss', //параметр, отвечающий за угол развала колес у мотоциклов;
//     'fTractionBiasFront', //параметр, отвечающий за распределение тяги между передними и задними колесами (от 0.01 до 0.99);
//     'fTractionLossMult', //параметр, отвечающий за потерю сцепления шины с дорогой;

//     'fSuspensionForce', //(1 / сила * количество колес)', //нижний предел силы при полном выдвижении (сила подвески);
//     'fSuspensionCompDamp', //параметр, отвечающий за силу и интенсивность вибрации подвески;
//     'fSuspensionReboundDamp', //параметр, отвечающий за силу и интенсивность вибрации подвески на высоких скоростях;
//     'fSuspensionUpperLimit', //(визуально) максимальная высота кузова транспортного средства;
//     'fSuspensionLowerLimit', //(визуально) минимальная высота кузова транспортного средства;
//     'fSuspensionRaise', //параметр, отвечающий за высоту подвески;
//     'fSuspensionBiasFront', //параметр, отвечающий за смещение подвески вперед (большое значение подходит для грузовиков);
//     'fAntiRollBarForce', //параметр, отвечающий за силу стабилизатора подвески;
//     'fAntiRollBarBiasFront', //параметр, отвечающий за смещение силы стабилизатора подвески вперед;
//     'fRollCentreHeightFront', //параметр, отвечающий за высоту центра крена;
//     'fRollCentreHeightRear', //параметр, отвечающий за высоту центра крена;



//     'fCollisionDamageMult', //множитель урона от столкновения (от 0.0 до 10.0. Значение 0.0 означает нулевой урон, 10.0', //десятикратный урон);
//     'fWeaponDamageMult', //множитель урона от оружия (от 0.0 до 10.0. Значение 0.0 означает нулевой урон, 10.0', //десятикратный урон);
//     'fDeformationDamageMult', //множитель урона от деформации (от 0.0 до 10.0. Значение 0.0 означает нулевой урон, 10.0', //десятикратный урон с последующей поломкой);
//     'fEngineDamageMult', //множитель урона, получаемого двигателем. Может привести к взрыву или выходу из строя;
//     'fPetrolTankVolume', //параметр, отвечающий за количество топлива вытыкаемого после выстрела по бензобаку;
//     'fOilVolume', //параметр, отвечающий за уровень масла/черный дым?;



//     'fSeatOffsetDistX', //координаты расстояния от двери до сидения;
//     'fSeatOffsetDistY', //координаты расстояния от двери до сидения;
//     'fSeatOffsetDistZ', //координаты расстояния от двери до сидения;
//     'nMonetaryValue', //стоимость транспортного средства;
//     'strModelFlags', //флаги модели. Самая правая значащая цифра является первой:
// ];

global.getHandling = (vehicle, key) => {
    return Math.round(parseFloat(vehicle.getHandling(key)) * 1000) / 1000;
}

global.setHandling = (vehicle, key, value) => {
    if (value !== undefined && value != null)
        vehicle.setHandling(key, value);
}

global.getSharedHandling = (vehicle, keyID) => {
    let value = vehicle.getVariable(`veh:handl:${keyID}`)
    InitDefaultValue(vehicle, keyList[keyID]);
    if ((typeof value) === 'undefined' || value == undefined || value == null)
        return global.GetDefaultHandling(vehicle, keyList[keyID])
    else
        return Math.round(parseFloat(value.replace(',', '.')) * 1000) / 1000;
}

function InitDefaultValue(vehicle, key) {
    if (!handlingData[vehicle.model]) {
        handlingData[vehicle.model] = {};
        handlingData[vehicle.model][key] = global.getHandling(vehicle, key);
    }
    else if (handlingData[vehicle.model][key] === undefined) {
        handlingData[vehicle.model][key] = global.getHandling(vehicle, key);
    }
}

global.GetDefaultHandling = (vehicle, key) => {
    InitDefaultValue(vehicle, key);
    return handlingData[vehicle.model][key]
}

function CheckExistVehicle(vehicle) {
    return (vehicle && vehicle.type === 'vehicle' && vehicle.handle !== 0);
}

Object.keys(keyList).forEach(keyID => {
    if (keyID != 999) {
        mp.events.addDataHandler(`veh:handl:${keyID}`, (vehicle, value) => AddDataHandler(keyID, vehicle));
    }
});

function AddDataHandler(keyID, vehicle) {
    if (!CheckExistVehicle(vehicle))
        return;
    global.setHandling(
        vehicle,
        keyList[keyID],
        global.getSharedHandling(vehicle, keyID)
    );
}

global.VehicleSetSharedDataHandlingMods = (vehicle) => {
    Object.keys(keyList).forEach(keyID => {
        if (keyID != 999) {
            global.setHandling(
                vehicle,
                keyList[keyID],
                global.getSharedHandling(vehicle, keyID)
            );
        }
    });
}

mp.events.add("veh:setHandling", (key, value) => {
    let getHandling = global.getHandling(mp.players.local.vehicle, key);
    let getDefaultHandling = global.GetDefaultHandling(mp.players.local.vehicle, key);
    mp.gui.chat.push(`${key} - get: ${getHandling}, getDefault: ${getDefaultHandling}`);
    mp.players.local.vehicle.setHandling(key, value);
    getHandling = global.getHandling(mp.players.local.vehicle, key);
    getDefaultHandling = global.GetDefaultHandling(mp.players.local.vehicle, key);
    mp.gui.chat.push(`${key} - set: ${value}, get: ${getHandling}, getDefault: ${getDefaultHandling}`);
});

mp.events.add("veh:checkHandling", (key) => {
    let getHandling = global.getHandling(mp.players.local.vehicle, key);
    let getDefaultHandling = global.GetDefaultHandling(mp.players.local.vehicle, key);
    mp.gui.chat.push(`${key} - get: ${getHandling}, getDefault: ${getDefaultHandling}`);
});


mp.events.add("gui:ready", () => {
    global.keyActions["reduce"].subscribe(ChangeReduce, true);
})

// mp.keys.bind(global.Keys.Key_6, false, ChangeReduce);

function ChangeReduce() {
    if (!global.localplayer.isInAnyVehicle(true)) return;
    if (!global.loggedin || global.chatActive || global.editing || global.lastCheck > Date.now()) return;
    if (!global.getVariable(mp.players.local.vehicle, `veh:handl:999`, false) && !global.shopReduce) {
        mp.events.call('notify', 1, 9, `handling:reduce:dontSystem`, 1500);
        return;
    }
    global.reduce = !global.reduce;
    mp.players.local.vehicle.setReduceGrip(global.reduce);
    mp.events.call('notify', 2, 9, `handling:reduce:${global.reduce}`, 1500);
}