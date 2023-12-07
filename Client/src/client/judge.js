mp.events.add('openjudgemenu', () => {
    global.gui.openPage("JudgeMenu")
});

mp.events.add('judje:close', () => {
    gui.close();
});

mp.events.add('judje:arrest', (name, time, message) => {
    mp.events.callRemote('judgearest', name, time, message);
    gui.close();
});