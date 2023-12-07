let peds = [];
const cam = mp.cameras.new('default', new mp.Vector3(0, 0, 0), new mp.Vector3(0, 0, 0), 50);

mp.events.add('questPeds:load', (pedinfo) => {
    pedinfo = JSON.parse(pedinfo);

    mp.game.streaming.requestAnimDict("friends@frm@ig_1");
    mp.game.wait(0);
    
    pedinfo.forEach(ped => {
        let p = mp.peds.newValid(ped.hash, ped.position, ped.heading, ped.dimension);

        p.questId = ped.id;
        peds.push(p);  
        //if(p !== null) p.taskPlayAnim("friends@frm@ig_1", "greeting_idle_a", 8.0, 1.0, -1, 1, 1.0, false, false, false);
    });
});

mp.events.add('questPeds:interacted', (id) => {
    const ped = peds.find(p => p.questId === id);
    if (!ped) return;
    
    const pos = ped.getOffsetFromInWorldCoords(0, 1.5, 1)
    cam.setCoord(pos.x, pos.y, pos.z);
    //mp.game.invoke(0x4A5113B7E65C8C80, player.handle, speechName, speechParam)
    mp.game.invoke("0x8E04FEDD28D42462", ped.handle, "GENERIC_HOWS_IT_GOING", "Speech_Params_Standard", 0);
    cam.setActive(true);
    cam.pointAtPedBone(ped.handle, 12844, 0, 0, 0, false)
    mp.game.cam.renderScriptCams(true, true, 350, true, false);
});

mp.events.add('questPeds:openDialog', (dialogData) => {
    global.gui.setData('dialogWindow/setData', dialogData);
    opened = global.gui.openPage("DialogWindow");
});

mp.events.add('questPeds:closeDialog', (cameraOff) => {
    if (cameraOff) deleteCamera();
    opened = false;
    global.gui.close();
});

function deleteCamera(){
    if (!cam) return;
    mp.game.cam.renderScriptCams(false, true, 200, true, false);
    cam.setActive(false)
}

let opened = false;
mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened) {
        deleteCamera();
        global.gui.close();
        opened = false;
        mp.events.callRemote("dialogWindow:playerClosedDialog");        
    }
});

mp.events.add('dialogWindow:currentAnswer', (id) => {
    opened = false;
    mp.events.callRemote("dialogWindow:playerSelectedAnswer", id);
});