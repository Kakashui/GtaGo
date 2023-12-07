let opened = false;

mp.events.add('government:openVoteMenu', (uuid, data, socialStatus) => {
    
    if (!global.loggedin) return;
    if (global.checkIsAnyActivity()) return;
    if (opened) return;
    global.gui.setData('cityHallWeb/vote/setState', data);
    global.gui.setData('cityHallWeb/setName', JSON.stringify(mp.players.local.name.replace('_', ' ')));
    global.gui.setData('cityHallWeb/setSocialStatus', socialStatus);
    global.gui.setData('cityHallWeb/setUuid', uuid);
    
    global.gui.openPage('CityHallWeb');
    opened = true;
});

mp.events.add('government:closeMenu', () => {
    if (opened)
        CloseMenu();
});


mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});
function CloseMenu() {
    global.gui.close();
    opened = false;
}