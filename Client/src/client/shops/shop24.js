mp.events.add("shop24:open", (data, money)=>{
    global.gui.setData("roundTheClockShop/setData", data, money);
    global.gui.openPage("RoundTheClockShop");
});

mp.events.add("shop24:buy", (data)=>{
    global.gui.close();
    mp.events.callRemote("shop24:buy", data);
});

mp.events.add("shop24:close", ()=>{
    global.gui.close();
});