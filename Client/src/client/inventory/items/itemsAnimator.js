let objToAttach;

mp.events.add('inv:useAnim', () => {
    mp.events.call('weapon:hideCurrentWeapon', global.localplayer)

    if (!mp.players.local.isInAnyVehicle(true)){
        objToAttach = mp.objects.new(mp.game.joaat('prop_cs_burger_01'), global.localplayer.position);

        objToAttach.attachTo(global.localplayer.handle,
            global.localplayer.getBoneIndexByName('IK_R_Hand'),
            0.1, -0.015, -0.07,
            40, -20, 110,
            false, false, false, false, 2, true);
    }

    setTimeout(() => {
        mp.events.callRemote('invSc:remove')
        if (!mp.players.local.isInAnyVehicle(true)) {
            objToAttach.destroy();
            objToAttach = null;
        }
        mp.events.call('weapon:showCurrentWeapon', global.localplayer)
    }, 5000)
})
