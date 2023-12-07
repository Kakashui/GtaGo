const KeyAction = require('./KeyAction');
global.keyActions = {};
const listActions = [];
let newActions = [
    {
        name: "reduce",
        key: 53,
    }
];
if(mp.storage.data.actions === undefined){
    mp.storage.data.actions = {
        "microphone": 66,
        "inventory": 9,
        "belt": 74,
        "lock": 76,
        "engine": 50,
        //"reports": new KeyAction(117),
        //"lock": new KeyAction(76)
    }
    mp.storage.flush();
}
newActions.forEach(action => {
    if (!mp.storage.data.actions[action.name])
        mp.storage.data.actions[action.name] = action.key
});
mp.storage.flush();

for (const key in mp.storage.data.actions) {
    const val = mp.storage.data.actions[key];
    global.keyActions[key] = new KeyAction(val);
    listActions.push(global.keyActions[key]);
    global.gui.setData("optionsMenu/updateActionKey", JSON.stringify({key, val}));
}

mp.events.add("cef:mmenu:action:key:bind", (key, val)=>{
    if(!val || val == "" || mp.storage.data.actions[key] === undefined || mp.storage.data.actions[key] == val) return;
    mp.storage.data.actions[key] = val;
    mp.storage.flush();
    global.keyActions[key].changeKey(val);
    global.gui.setData("optionsMenu/updateActionKey", JSON.stringify({key, val}));
})

mp.events.add("render", ()=>{
    listActions.forEach(action => {
        action.tick()
    });
})