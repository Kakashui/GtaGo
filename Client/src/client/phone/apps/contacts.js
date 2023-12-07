mp.events.add({
    "phone:contacts:load": (contacts) => {
        global.gui.setData('smartphone/addContacts', contacts);
    },
    
    "phone:contacts:applyEdit": (contact) => {
        global.gui.setData('smartphone/editContact', contact);
    }
});