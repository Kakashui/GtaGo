let spamProtection = 0;
let spamProtectionTime = 1000;

function isSpam(){
    if(spamProtection > Date.now()) return true;
    spamProtection = Date.now() + spamProtectionTime;
    return false;
}

function open(dynamic){
    if (!global.isFight && (mp.players.local.dimension == 11333377 || mp.players.local.dimension == 1337))
        return;
    if(global.playerInventory.id === -1){
        mp.events.callRemote("inv:get:personal")
        return;
    }
    if(global.gui.isOpened()) return;
    mp.events.callRemote("inv:open");
    global.gui.setData("inventory/setDynamic", `'${dynamic}'`);
    global.gui.openInventory();
    global.inventoryOpened = true;
}

function close(){
    global.gui.closeInventory();
    mp.events.callRemote("inv:close", global.stockId);
    global.gui.setData("inventory/close");
    global.stockId = -1;
    global.inventoryOpened = false;
}

function onKeyPres(){
    if(mp.keys.isDown(17) || mp.keys.isDown(18)) return;
    if (global.inventoryOpened) close();
    else {
        if(isSpam()) return;
        if (
            !global.loggedin|| 
            global.chatActive || 
            global.editing || 
            global.cuffed || 
            global.localplayer.getVariable('InDeath') == true || 
            global.IsPlayingDM == true
        ) return;
        open("Equip");
    }
}

mp.events.add("gui:ready", ()=>{
    global.keyActions["inventory"].subscribe(onKeyPres, true);
})

mp.events.add("gui:inv:close", ()=>{
    close();
})

mp.events.addDataHandler("InDeath", (entity, isDeath) => {
    if (entity === mp.players.local && global.inventoryOpened && isDeath == true)
        close();
});

// mp.keys.bind(global.Keys.Key_TAB, false, function () { 
//     if (global.inventoryOpened) close();
//     else {
//         if(isSpam()) return;
//         if (
//             !global.loggedin|| 
//             global.chatActive || 
//             global.editing || 
//             global.cuffed || 
//             global.localplayer.getVariable('InDeath') == true || 
//             global.IsPlayingDM == true
//         ) return;
//         open("Equip");
//     }
// });

mp.keys.bind(global.Keys.Key_ESCAPE, false, function() {
    if (global.inventoryOpened) {
        mp.game.ui.setPauseMenuActive(false);
        close();
    }
});

global.playerInventory = {
    id: -1, 
    items: []
};

global.playerEquip = {
    clothes: {
      "1": null,
      "2": null,
      "3": null,
      "4": null,
      "5": null,
      "6": null,
      "7": null,
      "8": null,
      "9": null,
      "11": null,
      "10": null,
      "12": null,
      "13": null,
      "14": null
    },
    weapons: {
        "1": null,
        "2": null,
        "3": null,
        "4": null
      }
};

global.stockId = -1;
//personal
mp.events.add("inv:set:personal", (id)=>{
    global.playerInventory.id = id;
    global.gui.setData('inventory/setPersonalId', `'${id}'`);
});

//open close
mp.events.add("cef:inv:close", ()=>{
    close();
})

mp.events.add("inv:open:stock", (id, type)=>{
    global.stockId = id;
    global.gui.setData("inventory/setStock", JSON.stringify({id, type}));
    open("Stock");
})

//equips
mp.events.add("inv:clear:equip", ()=>{
    global.gui.setData('inventory/clearEquip');
})

mp.events.add("inv:update:equip", (type, slot, item)=>{
    if(type == 1){
        global.playerEquip.weapons[slot] = item == null ? null : {id: item[0], count: 1, index: slot}
    }else{
        global.playerEquip.clothes[slot] = item == null ? null : {id: item[0], count: 1, index: slot}
    }
    global.gui.setData('inventory/updateEquip', JSON.stringify({type, slot, item}));
})

mp.events.add("inv:update:item", (id, item)=>{
    //mp.serverLog(`inv:update:item ${id} ${JSON.stringify(item)}`);
  
    if(global.playerInventory.id == id ){
        const index = global.playerInventory.items.findIndex(i=>i.index == item[2]);
        if(index == -1){
            if(item[1] > 0) 
                global.playerInventory.items.push({id: item[0], count: item[1], index: item[2]});
        }else{
            if(item[1] > 0)
                global.playerInventory.items[index].count = item[1];
            else
                global.playerInventory.items.splice(index, 1);
        }
    }
    global.gui.setData('inventory/updateItem', JSON.stringify({id, item}));
})

mp.events.add("inv:update", updateInventory)

let waitInventoryReady = 10;
function updateInventory(id, items, maxWeight, size){
    if(global.playerInventory.id == -1 && waitInventoryReady > 0){
        waitInventoryReady--;
        setTimeout(()=>{
            updateInventory(id, items, maxWeight, size);
        }, 1000)
        return;
    }

    if(global.playerInventory.id == id ){
        global.playerInventory.items = [];        
        global.playerInventory.maxWeight = maxWeight;
        global.playerInventory.size = size;
        items.forEach(item => {
            global.playerInventory.items.push({
                id: item[0],
                count: item[1],
                index: item[2]
            })

        });
    }
    //mp.serverLog(`inv:update2: ${items} ${typeof(items)}`);
    global.gui.setData('inventory/update', JSON.stringify({id, items, maxWeight, size}));
}


mp.events.add("inv:update:near", (items)=>{   
    global.gui.setData('inventory/update', JSON.stringify({id:0, items, maxWeight: -1, size: 2}));
})

function placeOnGround(obj){
    const position = obj.getCoords(true);
    const z = mp.game.gameplay.getGroundZFor3dCoord(position.x, position.y, position.z, 0.0, false);
    obj.setCoordsNoOffset(position.x, position.y, z + .05, true, true, true);
}

mp.events.add("inv:use:item", (index)=>{
    if(global.inAction) return;
    if(mp.players.local.hasOwnProperty("attachedWeapons") && mp.players.local.attachedWeapons["weapon:current"]){
        mp.events.call('notify', 1, 9, "act:canc:w", 3000);
        return;
    } 
    mp.events.callRemote("inv:use:item", index);
})

// mp.events.addDataHandler("data:object:id", (obj, value)=>{
//     if(obj.getType() != 3 || obj.handle == 0) return;
//     placeOnGround(obj);
// })

// mp.events.add("entityStreamIn", (obj)=>{
//     if(obj.getType() != 3) return;
//     placeOnGround(obj);
// })
