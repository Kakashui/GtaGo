let opened = false;
let currPoint = -1;

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});

mp.events.add('vehTrade::exit', () => {
    CloseMenu();
});


function CloseMenu() {
    global.gui.close();
    opened = false;
}

mp.events.add('vehTrade::openSellMenu', (idPoint, vehicleName) => {
    if (global.checkIsAnyActivity()) return;
    currPoint = idPoint;
    opened = global.gui.openPage('SettingAmountCarSales');
    global.gui.setData('settingAmountCarSales/setCarModel', JSON.stringify(vehicleName))
});

mp.events.add('vehTrade:acceptSell', (price) => {
    CloseMenu()
    mp.events.callRemote('vehTrade:setVehicleOnPoint', currPoint, price)
});

mp.events.add('vehTrade:cancelSell', () => {
    CloseMenu()
});




mp.events.add('vehTrade::openBuyMenu', (idPoint, price, number) => {
    if (global.checkIsAnyActivity()) return;
    currPoint = idPoint;
    opened = global.gui.openPage('SellCar');
    global.gui.setData('sellCar/setData', JSON.stringify({currentPrice: price, registrationNumber: number}))
});


mp.events.add('sellCar:acceptBuyCar', () => {
    CloseMenu()
    mp.events.callRemote('vehicleTrade:buyVehicle', currPoint)
});

mp.events.add('sellCar:closeBuyCar', () => {
    CloseMenu()
});
