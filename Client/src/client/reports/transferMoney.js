let opened = false;

mp.keys.bind(global.Keys.Key_F7, false, function () {
    if (opened)
        CloseMenu();
    else
        OpenMenu()
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});

mp.events.add('transfer:closemenu', () => {
    CloseMenu();
});

function OpenMenu() {
    if (!global.loggedin|| global.getVariable(mp.players.local, 'ALVL', 0) < 1) return;
    if (global.checkIsAnyActivity()) return;
    opened = global.gui.openPage('TransfersConfirmation');
}

function CloseMenu() {
    global.gui.close();
    opened = false;
}
