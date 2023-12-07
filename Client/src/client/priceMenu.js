let opened = false;

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});

mp.events.add('priceMenu:open', (priceList, categories) => {
    if (opened)
    {
        CloseMenu();
        return;
    }
    global.gui.setData('priceMenu/setPriceList', priceList);
    global.gui.setData('priceMenu/setPriceCategories', categories);
    opened = global.gui.openPage('PriceMenu');
});

mp.events.add('priceMenu:close', () => {
    CloseMenu();
});

function CloseMenu() {
    global.gui.close();
    opened = false;
}