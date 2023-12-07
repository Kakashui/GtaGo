let opened = false;

function quitGame() {    
    if(!opened) return;
    global.gui.close();
    opened = false;
}

function openGame(oreJSON, fuelJSON, exp){
    if (opened || gui.isOpened()) return;
    let oreArray = JSON.parse(oreJSON)
    let ore = {};
    oreArray.forEach(oreElement => {
        if (!ore[oreElement.Name])
            ore[oreElement.Name] = oreElement.Count;
        else
        ore[oreElement.Name] += oreElement.Count;
    });
    let fuelArray = JSON.parse(fuelJSON)
    let fuel = {};
    fuelArray.forEach(fuelElement => {
        if (!fuel[fuelElement.Name])
        fuel[fuelElement.Name] = fuelElement.Count;
        else
        fuel[fuelElement.Name] += fuelElement.Count;
    });
    global.gui.setData("gameMetalPlant/setData", JSON.stringify({ore, fuel, exp}));
    global.gui.openPage("GameMetalPlant");
    opened = true;
}

function startGame(){
    global.gui.setData("gameMetalPlant/updateGameBegineState", true);
}

function updateGameResult(diamond, metals){
    global.gui.setData("gameMetalPlant/updateGameResult", JSON.stringify({state: diamond ? 3 : 2, metals}));
}

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        quitGame();
});

mp.events.add("mg:metalplant:game:open", openGame);
mp.events.add("mg:metalplant:quit", quitGame);
mp.events.add("mg:metalplant:start", startGame);
mp.events.add("mg:metalplant:result", updateGameResult);