mp.game.cam.renderScriptCams(false, true, 0, true, false);

mp.events.add('auth:startReg', (name) => {
    global.gui.setData("setLoadScreen", false)
    global.gui.setData("auth/setSocialClub", JSON.stringify({ name }));
    global.gui.setData("auth/setCurrentTab", JSON.stringify({ page: "CreateNewAccountTab" }));
    global.gui.close();
    global.gui.openPage("Auth");
});

let waitAutoAuthResponse = false;
mp.events.add('auth:startAuth', (login) => {
    global.gui.setData("setLoadScreen", false)
    global.gui.close();
    global.gui.setData("auth/setSocialClub", JSON.stringify({ name: login }));    
    global.gui.setData("auth/setCurrentTab", JSON.stringify({ page: "LoginTab" }));  
    global.gui.openPage("Auth");
});

mp.events.add('auth:startCreateCharacter', () => {
    global.gui.setData("setLoadScreen", false)
    global.gui.close();
    global.gui.setData("auth/setCurrentTab", JSON.stringify({ page: "Customization" }));
    global.gui.openPage("Auth");
});

mp.events.add('auth:character:select', (data, coins, slots) => {
    global.gui.setData("setLoadScreen", false)
    global.gui.close();
    global.gui.setData("characterSelect/setData", data);
    global.gui.setData("characterSelect/setCoins", coins);
    global.gui.setData("characterSelect/setSlots", slots);
    global.gui.openPage("CharacterSelect");
});

mp.events.add('auth:spawn:select', (data) => {
    global.gui.setData("setLoadScreen", false)
    const pos = JSON.parse(data[1]);
    const {streetName,crossingRoad} = mp.game.pathfind.getStreetNameAtCoord(pos.x, pos.y, pos.z, 0, 0);
    data[1] = mp.game.ui.getStreetNameFromHashKey(streetName);  
    global.gui.close();
    global.gui.setData("spawnSelect/setData", JSON.stringify(data));
    global.gui.openPage("SpawnSelect");
});

mp.events.add("auth:save:pass", (login, password, save) => {
    checkAuthStorage();
    if(save){
        if(mp.storage.data.auth.login !== login || mp.storage.data.auth.password !== password || !mp.storage.data.auth.save){
            mp.storage.data.auth.login=login;
            mp.storage.data.auth.password=password;
            mp.storage.data.auth.save = true;
            mp.storage.flush();
        }
    }else{
        if(mp.storage.data.auth.login !== '' || mp.storage.data.auth.password !== '' || mp.storage.data.auth.save){
            mp.storage.data.auth.login='';
            mp.storage.data.auth.password='';
            mp.storage.data.auth.save = false;
            mp.storage.flush();
        }
    }
})

mp.events.add('auth:charCreated', function (name, surname) {
    mp.events.callRemote("newchar", name, surname)
});

mp.events.add('auth:doSpawn', spawn);
function spawn() {
    try {
        global.gui.setData("setLoadScreen", true);
        mp.game.cam.doScreenFadeOut(0);
        global.gui.close();
        setTimeout(()=>{
            global.showHud(false);
        }, 10)
        
        setTimeout(()=>{
            global.gui.setData("setLoadScreen", false)
            mp.game.cam.doScreenFadeIn(1700);
            global.showHud(true);
            global.checkFarm();
            if (global.characterEditor && global.gui.curPage !== "Customization"){
                global.gui.close();
                global.gui.openPage("Customization");
            }
        }, 3000);

        setTimeout(()=>{
            if (global.characterEditor && global.gui.curPage !== "Customization"){
                global.gui.close();
                global.gui.openPage("Customization");
            }
        }, 6000)
    
        mp.discord.update(`Playing ELITE RP`, `eliterp.ru`);
        
        global.gui.stopSound();
        global.showHud(true);
        global.loggedin = true;
        mp.game.player.setHealthRechargeMultiplier(0);
        
        global.gui.setData("setBackground", "0");
        global.activateAntiCheat();
        setTimeout(() => {
            global.chw();
            mp.events.call("switchTime", 0);
            global.localplayer.freezePosition(false);
            global.gui.close();
        }, 500)
    } catch (e) {
        if(global.sendException)mp.serverLog(`authorization.spawn: ${e.name }\n${e.message}\n${e.stack}`);
    }
    
}

function checkAuthStorage() {
    if (!mp.storage.data.hasOwnProperty('auth')) {
        mp.storage.data.auth = {
            login: '',
            password: '',
            save: false
        };
    }
}