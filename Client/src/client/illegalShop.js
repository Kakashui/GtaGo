let opened = false;
let productsName = {
    ['Cuffs']: 'gui_691',
    ['Hatchet']: 'gui_710',
    ['Knife']: 'gui_695',
    ['ArmyLockpick']: 'gui_689',
    ['Pocket']: 'gui_690',
    ['LowRod']: 'gui_716',
    ['MiddleRod']: 'gui_717',
    ['HightRod']: 'gui_718',
    ['LowFishingCage']: 'gui_720',
    ['MiddleFishingCage']: 'gui_721',
    ['HightFishingCage']: 'gui_722',
    ['FishingBait']: 'gui_723',
    ['Dynamite']: 'item_dynamite',
    ['Stetoskop']: 'item_stetoskop',
    ['Detector']: 'Детектор',//новый добавил я jordan
    
}




mp.events.add('illegalShop:open', (items) => {
    opened = global.gui.openPage('IllegalShop');
    if(!opened) return;
    let products = JSON.parse(items);
    let productsSell = [];
    Object.keys(products).forEach(key => {
        productsSell.push({
            name: productsName[key],
            cost: products[key],
            product: key,
            count: 1
        });
    });
    global.gui.setData('illegalShop/setProductsSell', JSON.stringify(productsSell));
    
});


mp.events.add('illegalShop:buy', (items) => {
    mp.events.callRemote('illegalShop:buyProduct', items);
});

mp.events.add('illegalShop:close', () => {
    ExitMenu();
});

function ExitMenu() {
    global.gui.close();
    mp.events.callRemote('illegalShop:closeMenu');
    opened = false;
    global.showCursor(false)
}

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        ExitMenu();
});