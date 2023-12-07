mp.events.addPhone({
    "phone::msg::setGeoposition": (x, y) => {
        mp.game.ui.setNewWaypoint(x, y);
    }
});