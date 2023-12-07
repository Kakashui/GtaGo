
let opened = false;
function openBar(discounts) {
    global.gui.setData("bar/updateDiscounts", JSON.stringify(discounts));
    opened = global.gui.openPage("Bar");
}

function closeBar() {
    global.gui.close();
    opened = false;
}

function onEscape() {
    if(opened)
        closeBar();
}

mp.events.add("alco:bar:open", openBar);
mp.events.add("alco:bar:close", closeBar);
mp.keys.bind(global.Keys.Key_ESCAPE, false, onEscape);