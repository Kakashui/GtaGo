let opened = false;

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});


mp.events.add('bankMenu:open', (bankDto, depositsDto, businessDto, creditDto, familyDto, fractionDto) => {
    if (opened)
    {
        CloseMenu();
        return;
    }
    global.gui.setData('bank/setData', bankDto);
    global.gui.setData('bank/deposit/setDepositsList', depositsDto);
    global.gui.setData('bank/business/setData', businessDto);
    global.gui.setData('bank/credit/setData', creditDto);
    global.gui.setData('bank/family/setData', familyDto);
    global.gui.setData('bank/organization/setData', fractionDto);
    opened = global.gui.openPage('Bank');
});
mp.events.add('bankMenu:exit', () => {
    CloseMenu();
});

function OpenMenu() {
}

function CloseMenu() {
    global.gui.close();
    opened = false;
}