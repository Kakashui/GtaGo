function showMessage(title, subtitle) {
    global.gui.setData("hudQuestMessage/setMessageData", JSON.stringify({title, subtitle}));
    global.gui.setData("hudQuestMessage/setMesageVisible", true);
}

function hideMessage() {
    global.gui.setData("hudQuestMessage/setMesageVisible", false)
}

mp.events.add("questmsg:show", showMessage);
mp.events.add("questmsg:hide", hideMessage);