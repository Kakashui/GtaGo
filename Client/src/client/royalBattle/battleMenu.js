let openedEnterMenu = false;
let openedStatsMenu = false;

//Enter menu
mp.events.add('royalBattle:openMenuEnterBattle', (isReg, playersCount) => {    
    openedEnterMenu = global.gui.openPage('BattlegroundReg');
    if (!openedEnterMenu) return;
    global.gui.setData('battlegroundReg/setData', JSON.stringify({isReg:isReg, peopleCount:playersCount}));    
})

mp.events.add('battlegroundReg:registerForBattle', () => {
    mp.events.callRemote('royalBattle:registerForBattle');
})

//Stats menu
mp.events.add('royalBattle:openBattleStats', (stats) => {
    openedStatsMenu = global.gui.openPage('BattlegroundStats');
    if (!openedStatsMenu) return;
    global.gui.setData('battlegroundStats/setData', stats);    
})

mp.events.add('royalBattle:sendSearchBattleStats', (stats) => {
    global.gui.setData('battlegroundStats/setListSearch', stats);
})

mp.events.add('battlegroundStats:setSearch', (value) => {
    mp.events.callRemote('royalBattle:searchStats', value);
})

//Close menus
mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (openedEnterMenu)
        closeEnterMenu();
    if (openedStatsMenu)
        closeStatsMenu();
});

mp.events.add('battlegroundReg:closeModal', () => {
    if (openedEnterMenu)
        closeEnterMenu();
})

function closeEnterMenu() {
    global.gui.close();
    openedEnterMenu = false;
}

function closeStatsMenu() {
    global.gui.close();
    openedStatsMenu = false;
}