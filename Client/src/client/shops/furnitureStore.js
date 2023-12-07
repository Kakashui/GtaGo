let opened = false;
mp.events.add("furnitureStore:open", (price) => {        
    opened = global.gui.openPage("FurnitureShop");
    global.gui.setData('furnitureShop/setPricePart', JSON.stringify(price))
});

mp.events.add("furnitureShop:buy", (data) => {
    // [{key: item.key, count: item.count}, {key: item.key, count: item.count}]
    closeMenu();
    mp.events.callRemote('furnitureStore:playerBought', data)
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function() {
    if(opened)
        closeMenu();
});

function closeMenu(){
    opened = false
    global.gui.close()
}