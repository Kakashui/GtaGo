let opened = false;
let lastcheck = Date.now();

mp.keys.bind(global.Keys.Key_OEM_7, false, function () {
    if (!opened)
        OpenMenu()
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});


mp.events.add('lsnews::exit', () => {
    CloseMenu();
});

function OpenMenu() {
    if (global.checkIsAnyActivity()) return;
    if (global.getVariable(mp.players.local, 'fraction', 0) != 15)
        return;
    if (Date.now() < lastcheck)
        return;
    lastcheck = Date.now() + 1000;
    mp.events.callRemote('lsnews::pressOpenMenu');
    global.gui.setData('news/setName', JSON.stringify(mp.players.local.name));
}

function CloseMenu() {
    global.gui.close();
    opened = false;
}

mp.events.add('lsnews::open', () => {
    if (global.checkIsAnyActivity()) return;
    opened = global.gui.openPage('News');
});
