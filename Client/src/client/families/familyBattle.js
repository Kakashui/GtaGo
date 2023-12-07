let opened = false;
let showWar = false;

let blip = null;

let zone = null;

mp.events.add('familyMp:showWarKey', (value, position, range, isLegal, time) => {
    showWar = value;
    global.gui.setData('hud/setWarZoneShow', value);
    if (!value)
        CloseMenu();
    UpdateBlip(value, position, range, isLegal);
    UpdateZone(value, position, range);
    time = Math.round(time);
    if (time > 0)
        global.gui.setData("timerTemo/setTimer", time);
    else
        global.gui.setData("timerTemo/resetTimer");
})

function UpdateBlip(value, position, range, isLegal) {

    if (blip) {
        mp.game.ui.removeBlip(blip);
        blip = null;
    }
    if (value) {
        blip = mp.game.ui.addBlipForRadius(position.x, position.y, position.z, range);
        mp.game.invoke(global.getNative("SET_BLIP_SPRITE"), blip, 9);
        mp.game.invoke(global.getNative("SET_BLIP_ALPHA"), blip, 120);
        mp.game.invoke(global.getNative("SET_BLIP_COLOUR"), blip, isLegal ? 2 : 1);
    }
}

function UpdateZone(value, position, range) {

    if (zone != null) {
        global.deleteZone(zone);
        zone = null;
    }
    if (value) {
        zone = global.newZone(500, position, range, position, range, Date.now(), 0, 0, 0, 9999)
    }
}

mp.keys.bind(global.Keys.Key_CAPITAL, false, () => {
    if (!showWar)
        return;
    if (showWar && !opened)
        OpenMenu();
    else if (opened)
        CloseMenu();
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});


mp.events.add('familyMp::statMenu::exit', () => {
    CloseMenu();
});

function OpenMenu() {
    if (global.checkIsAnyActivity()) return;
    opened = global.gui.openPage('War');
}

function CloseMenu() {
    global.gui.close();
    opened = false;
}