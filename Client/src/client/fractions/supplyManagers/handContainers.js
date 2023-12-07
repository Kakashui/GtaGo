let isHaveContainer = false;

const controlsToDrop = [
    { group: 0, control: 21 },
    { group: 0, control: 22 },
    { group: 0, control: 24 },
    { group: 0, control: 25 },
    { group: 0, control: 37 },
    { group: 0, control: 36 },
    { group: 0, control: 44 },
    { group: 0, control: 45 }
];
mp.events.add({
    "materialsSupply:pickContainer": () => {
        isHaveContainer = true;
        global.controlsManager.canSprintJump(true, true);
    },

    "materialsSupply:takeContainer": () => {
        isHaveContainer = false;
        global.controlsManager.canSprintJump(false, false);
    },

    "render": () => {
        if (!isHaveContainer) return;

        controlsToDrop.forEach((c) => {
            if (mp.game.controls.isControlJustPressed(c.group, c.control) || mp.game.controls.isControlPressed(c.group, c.control)) {
                
                mp.events.callRemote('materialsSupply:dropContainer');
            }
        });
    },
});