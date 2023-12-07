mp.events.add('bizsetts:changeOrderPrice', (value, productName) => {
    mp.events.callRemote('bizsetts::changeOrderPrice', value, productName);
});

mp.events.add('bizsetts:changeMaxPrice', (value, productName) => {
    mp.events.callRemote('bizsetts::changeMaxPrice', value, productName);
});

mp.events.add('bizsetts:changeMinPrice', (value, productName) => {
    mp.events.callRemote('bizsetts::changeMinPrice', value, productName);
});

mp.events.add('bizsetts:changeStockCapacity', (value, productName) => {
    mp.events.callRemote('bizsetts::changeStockCapacity', value, productName);
});

mp.events.add('bizsetts:delete', (productName) => {
    mp.events.callRemote('bizsetts::delete', productName);
});

mp.events.add('bizsetts:addnew', () => {
    mp.events.callRemote('bizsetts::addnew');
});

mp.events.add('bizsetts:close', () => {
    global.gui.close();
});

mp.events.add('bizsetts:open', (data) => {
    global.gui.setData(`businessMenu/setData`, data);
    global.gui.openPage('BusinessMenu');
});

mp.events.add('bizsetts:updateData', (data) => {
    global.gui.setData(`businessMenu/setData`, data);
});

