let cursorWasOpened = false;
let currentDialogCallback;

mp.events.add({
    "dialog:open": (header, buttons, callback) => {
        currentDialogCallback = callback;
        
        const dto = {
            Header: header,
            Buttons: buttons
        }
        
        global.gui.setData('dialogMenu/setBody', JSON.stringify(dto));
        global.gui.setData('dialogMenu/setShow', true);
        
        cursorWasOpened = mp.gui.cursor.visible;
        global.showCursor(true);
    },

    "dialog:close": () => {
        if (!cursorWasOpened) {
            global.showCursor(false)
        }

        global.gui.setData('dialogMenu/setShow', false);
    },

    // GUI EVENTS
    "dialog::buttonClick": (buttonIndex) => {
        currentDialogCallback(buttonIndex);
        mp.events.call('dialog:close');
    }
});