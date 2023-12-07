let opened = false;
let currentId = -1;
mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});

mp.events.add('vehicleRent:openMenu', (id, categories) => {
    if (!global.loggedin) return;
    if (opened || global.checkIsAnyActivity()) return;
    currentId = id;
    global.gui.setData('rentVehicle/setCategories', categories);
    opened = global.gui.openPage('RentVehicle');
});

function CloseMenu() {
    global.gui.close();
    opened = false;
}



mp.events.add('vehicleRent:acceptRent', (model, category, payment) => {    
    mp.events.callRemote('vehicleRent:tryRent', currentId, model, category, payment)
    if (opened)
        CloseMenu();
});



let peds = [];
mp.events.add('vehicleRent:loadPeds', (pedsJSON) => {
    pedinfo = JSON.parse(pedsJSON);

    pedinfo.forEach(ped => {
        let p = mp.peds.newValid(579932932, ped.position, ped.heading, 0);
        let obj = { ped: p, id: ped.id };
        peds.push(obj);
        //if(p !== null) p.taskPlayAnim("friends@frm@ig_1", "greeting_idle_a", 8.0, 1.0, -1, 1, 1.0, false, false, false);
    });
});

mp.events.add('vehicleRent:updatePeds', (pedJSON) => {
    ped = JSON.parse(pedJSON);
    const index = peds.findIndex(item => item.id == ped.id)
    if (index >= 0) {
        peds[index].ped.destroy();
        peds[index].ped = mp.peds.newValid(579932932, ped.position, ped.heading, 0);
    }
    else {
        let p = mp.peds.newValid(579932932, ped.position, ped.heading, 0);
        let obj = { ped: p, id: ped.id };
        peds.push(obj);
    }
});

mp.events.add('vehicleRent:deletePed', (id) => {
    const index = peds.findIndex(item => item.id == id)
    if (index >= 0) {
        peds[index].ped.destroy();
        peds.splice(index, 1);
    }
});