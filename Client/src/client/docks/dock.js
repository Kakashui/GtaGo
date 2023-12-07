var opened = false;
mp.events.add("dockOrder:openPage", (data) => {
    global.gui.setData('portOrders/pushData', data);
    opened = global.gui.openPage('PortOrders');
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened) {
        global.gui.close();
        opened = false;
    }
});

mp.events.add("portOrders:buy", (data) => {
    mp.events.callRemote("dockOrder:playerOrdered", data); 
    global.gui.close();
});
