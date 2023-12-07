mp.events.add("mmenu:frac:members:update", (members)=>{
    global.gui.setData("optionsMenu/setFractionMembers", members);
})

mp.events.add("mmenu:frac:biz:update", (biz)=>{
    global.gui.setData("optionsMenu/setFractionBusinesses", biz);
})

mp.events.add("mmenu:frac:data:update", (id, bank, lastHour, lastDay, canInvite, canKick, canRank, canWhithdraw, canAccess)=>{
    global.gui.setData("optionsMenu/setFractionData", JSON.stringify({id, bank, lastHour, lastDay, canInvite, canKick, canRank, canWhithdraw, canAccess}));
})

mp.events.add("mmenu:frac:access:set", (access)=>{
    global.gui.setData("optionsMenu/setFractionAccess", access);
})

mp.events.add("mmenu:frac:capt:attack", ()=>{
    global.gui.playSound("alarm", 0.1, false);
    global.gui.setData("optionsMenu/setAttack", 'true');
})
