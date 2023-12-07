mp.phone.setTexting = () => {
    if (mp.phone.fuckingPedCanOpenPhone(false))
    {
        if (!mp.players.local.isInAnyVehicle(true) && !mp.players.local.isGettingIntoAVehicle())
            mp.events.callRemote('phone:getHeld');
    }
};

mp.phone.takeDown = () => {
    mp.events.callRemote('phone:offHeld');
};

mp.phone.setSpeaking = () => {
    mp.events.callRemote('phone:setSpeaking');
};