function openCashBox() {
    global.gui.openPage('ChipsTradeMenu');
}

mp.events.add('openCasinoOwnerMenu', (name, stateShare, casinoShare, amount) => {
    global.gui.setData('casinoOwner/setData', 
        JSON.stringify({ name: name, stateShare: stateShare, casinoShare: casinoShare, amount: amount }));
    global.gui.openPage('CasinoOwner');
})
mp.events.add('casinoOwner:closeInterface', (currentTax) => {
    mp.events.callRemote("casino:setTax", currentTax)
    global.gui.close();
})
mp.events.add('casinoOwner:update', (amount) => {
    global.gui.setData('casinoOwner/setAmount', amount);
})


mp.events.add('openCashbox', (playerbank) => {   
    global.gui.setData('chipsTradeMenu/setBank', playerbank);
    openCashBox();
})

mp.events.add('playerBoughtChips', (black, red, blue, green, yellow) => {
    mp.events.callRemote('player:boughtChips', black, red, blue, green, yellow);
    global.gui.close();
})

mp.events.add('playerSoldChips', () => {
    mp.events.callRemote('player:soldChips');
    global.gui.close();
})

mp.events.add('showCasinoHud', (json) => {
    if (currInteriorId === 275201)
        global.gui.setData("hud/setIsChipsList", true)
    else
        global.gui.setData("hud/setIsChipsList", false)

    let data = JSON.parse(json);
    //JSON.stringify(data);    
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "red", value: data.Red}))
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "black", value: data.Black}))
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "blue", value: data.Blue}))
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "green", value: data.Green}))
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "yellow", value: data.Yellow}))
})


mp.events.add('hideCasinoHud', () => {
    global.gui.setData("hud/setIsChipsList", false)
})

mp.events.add('roulette:updatePlayerBank', (totalBank, chipsData) => {
    global.gui.setData('casino/setData', JSON.stringify({playerBalance:totalBank}));
    let dto = JSON.parse(chipsData);
    JSON.stringify(dto);

    global.gui.setData("hud/setChipValue", JSON.stringify({type: "black", value: dto.Black}))
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "green", value: dto.Green}))
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "red", value: dto.Red}))
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "blue", value: dto.Blue}))
    global.gui.setData("hud/setChipValue", JSON.stringify({type: "yellow", value: dto.Yellow}))
})

mp.events.add('roulette:clearStats', () => {
    global.gui.setData('casino/resetStats');    
})

mp.events.add('roulette:sentStats', (statsDto) => {
    let data = JSON.parse(statsDto);
    global.gui.setData('casino/addStat', JSON.stringify( {
        value: data.WinNumber,
        color: global.colors[data.WinNumber + 1][1], 
        result: data.Result, 
        win: data.Winning
    }));
})
mp.events.add('roulettes:stats:update', statsDto => {
    const newData = [];
    statsDto.forEach(data=>{
        newData.push({
            throwValue: data.WinNumber,
            throwValueColor: global.colors[data.WinNumber + 1][1],
            win: data.Result === "win",
            gain: data.Winning
        })
    })
    global.gui.setData('roulette/updateStateList', JSON.stringify(newData));
})

let currInteriorId = -1;

mp.events.add("onChangeInteriors", (newId, oldId)=>{
    currInteriorId = newId;
    if(newId === 275201)
        global.gui.setData("hud/setIsChipsList", true)
    else if(oldId === 275201)
        global.gui.setData("hud/setIsChipsList", false)    
})