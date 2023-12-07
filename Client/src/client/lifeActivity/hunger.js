let moveDisabled = false;

mp.events.add({
    "lifesystem:playStomachSound": () => {
        global.gui.playSound("stomach", .3, false);
    },

    "lifesystem:enableRagdoll": () => {
        mp.players.local.setToRagdoll(1000, 2000, 1, false, false, false);
    },

    "lifesystem:disableMoveOnTime": (time) => {
        moveDisabled = true;
        setTimeout(() => moveDisabled = false, time);
    },

    // RAGE EVENTS
    "render": () => {
        if (moveDisabled) {
            mp.game.controls.disableControlAction(0, 30, true);
            mp.game.controls.disableControlAction(0, 31, true);
            mp.game.controls.disableControlAction(0, 32, true);
            mp.game.controls.disableControlAction(0, 33, true);
            mp.game.controls.disableControlAction(0, 34, true);
            mp.game.controls.disableControlAction(0, 35, true);
        }
    }
});