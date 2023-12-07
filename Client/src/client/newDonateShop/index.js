let kassa =  null;
let show = false;


function redirectToPayment(link){
    if(!mp.gui.cursor.visible) 
        global.showCursor(true);
    if(kassa != null) kassa.destroy();
    kassa = mp.browsers.new(link);
}

function resultResponce(result){
    global.gui.dispatch("newDonateShop/roulette/responceStart", result)
}

function updateCoins(coins){
    global.gui.setData("newDonateShop/setCoins", coins);
}

function updatePrimeData(days){
    global.gui.setData("newDonateShop/prime/setData", days);
}

function openDonateMenu(){
    if(kassa != null) {
        kassa.destroy();
        kassa = null;
    }
    if(global.gui.isOpened() && !show) return;
    
    if(global.gui.isOpened())
        closeDonateMenu();
    else
    {
        if(!global.gui.openPage("NewDonateShop")) return;
        const gender = global.localplayer.getVariable("GENDER") ? true : false;
        global.gui.setData("newDonateShop/setGender", gender == true);
        mp.events.callRemote("dshop:coins:request");
        show = true;
    }
}

function closeDonateMenu(){
    global.gui.close();
    show = false;
}

function tryClothes(...args){
    args.forEach(cl=>{
        
    })
}

function onEscape(){
    if(kassa != null) {
        kassa.destroy();
        kassa = null;
        global.gui.close();
        show = false;
    }
}

function setInventory(items){
    global.gui.setData("newDonateShop/inventory/setItems", items);
}

function showWinNotification(name, id){
    name = name.replace(" ", "");
    global.gui.setData("hud/showWinNotification", JSON.stringify({name, id}));
}

function updateItemFromInventory(id, count, sell){    
    global.gui.setData("newDonateShop/inventory/updateItem", JSON.stringify({id, count, sell}));
}

function takeDonateItem(items) {
    global.gui.setData("newDonateShop/setTakeItems", items);
    global.gui.openPage("TakeDonateItem");
}

function updateExchangeCource(cources, currency, coinKits){
    global.gui.setData("newDonateShop/updateExchangeCource", JSON.stringify(cources))
    global.gui.setData("newDonateShop/updateCurrency", `'${currency}'`)
    global.gui.setData("newDonateShop/updateCoinKits", coinKits)
}

mp.events.add("tryClothes", tryClothes);
mp.events.add("dshop:roulette:result", resultResponce);
mp.events.add("dshop:roulette:notify", showWinNotification);
mp.events.add("dshop:coins:update", updateCoins);
mp.events.add("dshop:prime:update", updatePrimeData);
mp.events.add("dshop:inventory:set", setInventory);
mp.events.add("dshop:inventory:update", updateItemFromInventory);
mp.events.add("dshop:wallet:redirect", redirectToPayment);
mp.events.add("dshop:close", closeDonateMenu);
mp.events.add("dshop:open", openDonateMenu);
mp.events.add("dshop:take:item", takeDonateItem);
mp.events.add("dshop:cources:update", updateExchangeCource);

mp.keys.bind(global.Keys.Key_F9, false, openDonateMenu);
mp.keys.bind(global.Keys.Key_ESCAPE, false, onEscape);