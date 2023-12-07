
// ELEVATOR //
let lift = 0;

function closeLift() {
    global.gui.close();
}
mp.events.add('openliftmenu', (liftId, floors) => {
    lift = liftId;
    global.gui.setData("elevator/set", floors)
    global.gui.openPage("Elevator")
})
mp.events.add('lift', (act, data) => {
    switch (act) {
        case "stop":
            closeLift();
            break;
        case "start":
            mp.events.callRemote('lift:callBack', lift, data);
            closeLift();
            break;
    }
});