let showVehicleTrunk = false;

let pos;
let boneName;
mp.events.add('SET_TO_VEH_TRUNK', (vehicle) => {
    if(vehicle === undefined) return;
    let trunkBoneIndex = vehicle.getBoneIndexByName('seat_dside_r');
    if (trunkBoneIndex === -1) trunkBoneIndex = vehicle.getBoneIndexByName('seat_dside_f');

    let vehZRotation = vehicle.getHeading();
    mp.gui.chat.push(`vehzrotation ` + vehZRotation);
    mp.players.local.attachTo(vehicle.handle, trunkBoneIndex, 0.25, -1.3, 0.3, 0, 0, vehZRotation + 90, false, true, false, true, 0, false);
});

mp.events.add('UNSET_FROM_TRUNK', () => {
    mp.players.local.detach(false, false);
});

mp.events.add('checkBone', (entity, bone) => {
    let boneIndex = entity.getBoneIndexByName(bone);
    pos = entity.getWorldPositionOfBone(boneIndex);
    boneName = bone;

    showVehicleTrunk = true;
});

mp.events.add('render', () => {
    if (!showVehicleTrunk) return;

    mp.game.graphics.drawText(boneName, [pos.x, pos.y, pos.z], {
        scale: [0.2, 0.2],
        outline: true,
        color: [255, 255, 255, 255],
        font: 4
    });
});