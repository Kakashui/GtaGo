const dialogCallback = (buttonIndex) => {
    mp.events.callRemote('dialog:buttonClick', buttonIndex);
};

mp.events.add({
    "dialogServer:open": (header, buttons) => {
        buttons = JSON.parse(buttons);
        mp.events.call('dialog:open', header, buttons, dialogCallback);
    }
});