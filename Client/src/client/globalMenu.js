

global.goodScreenFadeOut = function(fadeoutTime, blackTime, fadeinTime) {
    mp.game.cam.doScreenFadeOut(fadeoutTime);

    setTimeout(() => {
        mp.game.cam.doScreenFadeIn(fadeinTime);
    }, blackTime);
}

global.input = {
    head: "",
    desc: "",
    len: "",
    cBack: "",
    set: function(h, d, l, c) {
        this.head = h, this.desc = d;
        this.len = l, this.cBack = c;
        if (gui.isOpened()) return;
        global.gui.setData('inputMenu/setData', JSON.stringify({tittle: this.head, plHolder: this.desc, length: this.len}))
    },
    open: function() {
        global.gui.openPage('InputMenu');
        //mp.events.call('startScreenEffect', "MenuMGHeistIn", 1, true);
    },
    close: function() {
        global.gui.close();
        //mp.events.call('stopScreenEffect', "MenuMGHeistIn");
    }
};

let maxAmount = 100000;
mp.events.add('input', (text) => {
    if (global.input.cBack == "") return;
    if (global.input.cBack == "setCruise") mp.events.call('setCruiseSpeed', text);
    else{
        if(global.input.cBack == "player_givemoney" && text > maxAmount) {
            mp.events.call('notify', 4, 9,`client_70@${maxAmount}`, 3000)
            return;
        }
        mp.events.callRemote('inputCallback', global.input.cBack, text);
        global.input.cBack = "";
        global.input.close();
    }
});
mp.events.add('openInput', (h, d, l, c) => {
    if (gui.isOpened()) return;
    input.set(h, d, l, c);
    input.open();
})
mp.events.add('closeInput', () => {
        input.close();
})
// // // // //
global.dialog = {
    queue:[],
    current: undefined,
    
    add(item){
        this.queue.push(item)
        if(this.current == undefined) this.check();
    },

    pressY(){
        if(this.current !== undefined) {
                mp.events.callRemote('dialogCallback', this.current.callback, true);            
            this.check();
        }
    }, 

    pressN(){
        if(this.current !== undefined) {
                mp.events.callRemote('dialogCallback', this.current.callback, false);            
            this.check();
        }
    }, 
    check: function() {
        this.current = this.queue.shift();

        if(this.current != undefined)
        {
            if (global.gui.isOpened()) {
                mp.events.call('dialog:open', this.current.question, [
                    { Name: 'dialog_0', Icon: 'confirm' },
                    { Name: 'dialog_1', Icon: 'cancel' }
                ], dialogCallback);
            }
            else {
                global.gui.setData("questionMenu/setTitle", `'${this.current.question}'`)
            }
        }
        else 
            global.gui.setData("questionMenu/setTitle", '')

        
    },

    close: function() {
        global.gui.close(!global.gui.isOpened())
    },
}

const dialogCallback = (buttonIndex) => {
    // confirm
    if (buttonIndex === 0) {
        global.dialog.pressY();
    }
    // decline
    else {
        global.dialog.pressN();
    }
};

mp.keys.bind(global.Keys.Key_Y, false, ()=>{global.dialog.pressY()})

mp.keys.bind(global.Keys.Key_N, false, ()=>{global.dialog.pressN()})
    
mp.events.add('openDialog', (c, q) => {
    global.dialog.add({
        callback: c,
        question: q
    })
})

// STOCK //
mp.events.add('openStock', (data) => {
    global.gui.setData('stockMenu/setCount', data);
    global.gui.openPage('StockMenu');
});
mp.events.add('stock:get', (type, amount) => {
    global.gui.close();

    if (type == 'weapons') {
        mp.events.callRemote('openWeaponStock');
    } else {
        if (isNaN(parseInt(amount))) return;
        mp.events.callRemote('stock::change', 'take_stock', type, amount);
    }
});
mp.events.add('stock:put', (type, amount) => {
    global.gui.close();

    if (type == 'weapons') {
        mp.events.callRemote('openWeaponStock');
    } else {
        if (isNaN(parseInt(amount))) return;
        mp.events.callRemote('stock::change', 'put_stock', type, amount);
    }
});



// SM DATA //
mp.events.add('policeg', () => {
    let data = [
        "Nightstick",
        "Pistol",
        "CombatPDW",
        "PumpShotgun",
        "StunGun",
        "Broni",
        "Medikamenti",
        "Pistol Ammo x12",
        "SMG Ammo x30",
        "Shotguns Ammo x6",
        "Radio Set"
    ];
    openSM(4, JSON.stringify(data));
});
mp.events.add('fbiguns', () => {
    let data = [
        "StunGun",
        "Combat Pistol",
        "Combat PDW",
        "Carbine Rifle",
        "Heavy Sniper",
        "Broni",
        "Medikamentebi",
        "Pistol Ammo x12",
        "SMG Ammo x30",
        "Rifles Ammo x30",
        "Sniper Ammo x5",
        "Sashvi",
        "Radio Set"
    ];
    openSM(3, JSON.stringify(data));
});
mp.events.add('govguns', () => {
    let data = [
        "Tazer",
        "Pistol",
        "Advanced Rifle",
        "Gusenberg Sweeper",
        "Broni",
        "Medikamentebi",
        "Pistol Ammo x12",
        "SMG Ammo x30",
        "Rifles Ammo x30",
        "Radio Set"
    ];
    openSM(6, JSON.stringify(data));
});
mp.events.add('armyguns', () => {
    let data = [
        "Pistol",
        "carbine",
        "CombatMG",
        "Broni",
        "Medikamentebi",
        "Pistol Ammo x12",
        "Rifles Ammo x30",
        "SMG Ammo x100",
        "Radio Set"
    ];
    openSM(7, JSON.stringify(data));
});
mp.events.add('refereeg', () => {
    let data = [
        "Tazer",
        "Pistol",
        "Advanced Rifle",
        "Broni",
        "Medikamentebi",
        "Pistol Ammo x12",
        "SMG Ammo x30",
        "Rifles Ammo x30",
        "Radio Set"
    ];
    openSM(10, JSON.stringify(data));
});
mp.events.add('mavrshop', () => {
    let data = [
        ["client_45", "1000$"],
        ["client_46", "1000$"],
        ["client_47", "2000$"],
        ["client_48", "5000$"]
    ];
    openSM(2, JSON.stringify(data));
});
mp.events.add('gangmis', () => {
    let data = [
        "client_49",
        "client_50",
    ];
    openSM(8, JSON.stringify(data));
});
mp.events.add('mafiamis', () => {
    let data = [
        "client_51",
        "client_52",
        "client_53",
    ];
    openSM(9, JSON.stringify(data));
});
mp.events.add('shop', (json) => {
    let data = JSON.parse(json);
    openSM(1, JSON.stringify(data));
})

// PETROL //
mp.events.add('gasStation:buyFuel', (key, liters, playmentType) => {
    mp.events.callRemote('gasStation:buypetrol', key, liters, playmentType);
    global.gui.close();
});

mp.events.add('openPetrol', (price) => {
    if(!mp.players.local.vehicle) return;
    let data = { cur: mp.players.local.vehicle.getVariable('PETROL'), max: mp.players.local.vehicle.getVariable('MAXPETROL') };
    global.gui.setData('gasStation/setData', JSON.stringify(data));
    global.gui.setData('gasStation/setCurrentPage', JSON.stringify(
        {
            page: 'FuelPage',
            data: null
        }));
    global.gui.setData('gasStation/setFuelTypes', JSON.stringify([
        {
            key: 'standart',
            title: 'Standart',
            cost: price
        }
    ]));

    global.gui.openPage('GasStation');
});

mp.events.add('gasStation:close', () => {
    global.gui.close();
});


// PLAYERLIST //
let pliststate = false;

mp.keys.bind(global.Keys.Key_F8, false, function() { // F8
    if  (global.getVariable(mp.players.local, 'ALVL', 0) >= 2) {
        if (pliststate) closePlayerList();
        else openPlayerList();
    }
});

function openPlayerList() {
    if (gui.isOpened()) return;
    let list =[];
    mp.players.forEach(player => 
	{
        let item = [];
		let uuid = player.getVariable('C_ID');
		if (uuid >= 0) item.push(uuid);
		else item.push(-1);
        item.push(player.name);
        item.push(global.getVariable(player, 'C_LVL', 0));
        item.push(global.getVariable(player, 'Ping', -1));
        list.push(item);
    });
    global.gui.setData('playerList/setData', JSON.stringify(list))
    global.gui.openPage("PlayerList");
    pliststate = true;
}

function closePlayerList() {
    pliststate = false;
    global.gui.close();
}
// MATS //

mp.events.add('matsOpen', (isArmy, isMed) => {
    global.gui.setData('stockPoint/setData', JSON.stringify([isMed, isArmy]));
    global.gui.openPage('StockPoint');
});

mp.events.add('matsL', (type) => {
    global.gui.close();

    switch (type) {
        case 0:
            global.input.set("client_58", "client_59", 4, "loadmats");
            global.input.open();
            break;
        case 1:
            global.input.set("client_60", "client_61", 4, "loaddrugs");
            global.input.open();
            break;
        case 2:
            global.input.set("client_62", "client_63", 4, "loadmedkits");
            global.input.open();
            break;
    }
});

mp.events.add('matsU', (type) => {
    global.gui.close();

    switch (type) {
        case 0:
            global.input.set("client_64", "client_59", 4, "unloadmats");
            global.input.open();
            break;
        case 1:
            global.input.set("client_65", "client_61", 4, "unloaddrugs");
            global.input.open();
            break;
        case 2:
            global.input.set("client_66", "client_63", 4, "unloadmedkits");
            global.input.open();
            break;
    }
});

    
    // BODY CUSTOM //
function getCameraOffset(pos, angle, dist) {
    angle = angle * 0.0174533;
    pos.y = pos.y + dist * Math.sin(angle);
    pos.x = pos.x + dist * Math.cos(angle);
    return pos;
}

// WEAPON CRAFT //
mp.events.add('openWCraft', (frac, mats) => {
    global.gui.setData('craftMenu/setData', JSON.stringify({frac, mats}));
    global.gui.openPage("CraftMenu");
})

mp.events.add('updateWCraft', (mats) => {
    global.gui.setData('craftMenu/updateMats',  mats);
})