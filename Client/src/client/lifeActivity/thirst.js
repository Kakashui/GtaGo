let endlessStamina = true;

mp.events.add({
    "lifesystem:toggleStaminaDecreased": (toggle) => {
        togle = toggle == true;
        const staminaParam = toggle ? 10 : 100;
        endlessStamina = !toggle;

        mp.game.stats.statSetInt(mp.game.joaat("SP0_STAMINA"), staminaParam, false);
    },

    // RAGE EVENTS
    "render": () => {
        if (endlessStamina) {
            if  (global.localplayer.isSprinting() || global.localplayer.isOnAnyBike()) mp.game.player.restoreStamina(100);
        }
    }
});