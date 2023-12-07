//#region Server

mp.events.add('WORK::TECHNICIAN::DIAGNOSTIC::SERVER', () => {
    global.gui.openPage("WorkMiniGame");
})

mp.events.add('WORK::TECHNICIAN::DIAGNOSTIC::RESULT::VUE', (isSuccess) => {
    mp.events.callRemote("WORK::TECHNICIAN::DIAGNOSTIC::RESULT::CLIENT", isSuccess);
    global.gui.close();
})

//#endregion