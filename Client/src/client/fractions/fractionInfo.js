let isStockInfoOpenned = false;

mp.events.add({
    "fraction:openStockInfo": (data) => {
        global.gui.setData("stockInfo/setData", data);
        isStockInfoOpenned = global.gui.openPage("StockInfo");
    }
});

mp.events.add("stockInfo::close", () => {        
    if (!isStockInfoOpenned) return;
    global.gui.close();
    isStockInfoOpenned = false;
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function() {
    if (!isStockInfoOpenned) return;
    
    global.gui.close();
    global.showCursor(false);

    isStockInfoOpenned = false;
});