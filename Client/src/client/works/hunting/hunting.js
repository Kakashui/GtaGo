const Animal = require('./animal.js');

//#region Global fucntions
global.hunting = { };
global.hunting.getCurrentHuntingGround = () => {
    return currentHuntingGround; 
};
global.hunting.getNearestAnimalCoords = (coordToTest) => {
    if (!isPlayerOnHuntingGround) {
        return null;
    }

    let nearestAnimal;

    animals.forEach((animal) => {
        let animalCoords = animal.getCoords();
        const distance = mp.game.system.vdist(animalCoords.x, animalCoords.y, animalCoords.z, coordToTest.x, coordToTest.y, coordToTest.z);
        
        if (nearestAnimal == null) {
            nearestAnimal = { coords: animalCoords, dist: distance };
        }
        else if (distance < nearestAnimal.dist) {
            nearestAnimal = { coords: animalCoords, dist: distance };
        }
    });
    
    if (nearestAnimal)
    {
        return nearestAnimal;
    }
    else {
        return null;
    }
};
//#endregion Global functions

//#region Admin command
let adminHuntingVision = false;
mp.events.add({
    "hunting:toggleAdminVision": () => {
        adminHuntingVision = !adminHuntingVision;
    },

    "render": () => {
        if (!isPlayerOnHuntingGround || !adminHuntingVision) {
            return;
        }

        const posPl = mp.players.local.position;

        animals.forEach((animal) => {
            if (animal.ped) {
                const pos = animal.ped.getCoords(true);
                mp.game.graphics.drawLine(pos.x, pos.y, pos.z, posPl.x, posPl.y, posPl.z, 255, 0, 0, 255);
            }
        });
    }
});
//#endregion Admin command

const animals = [];
const currentHuntingGround = {
    CenterPosition: new mp.Vector3(0, 0, 0),
    Range: 200
};
let isPlayerOnHuntingGround = false;

mp.events.add({
    // SERVER EVENTS
    "hunting:loadAnimal": (data) => {
        data = JSON.parse(data);
        const animal = new Animal(data.ID, data.Model, data.Position, data.State, data.IsController);
        animals.push(animal);
    },

    "hunting:unloadAnimal": (animalId) => {
        const animal = animals.find(a => a.id === animalId);
        
        if (!animal) {
            return;
        }
        
        const idx = animals.findIndex(a => a == animal);
        animals.splice(idx, 1);

        animal.destroy();
    },

    "hunting:setState": (animalId, newState) => {
        const animal = animals.find(a => a.id === animalId);
        if (animal) {
            animal.setState(newState);
        }
    },

    "hunting:updateAnimal": (animalId, newCoords) => {
        const animal = animals.find(a => a.id === animalId);
        if (animal) {
            animal.updatePosition(newCoords);
        }        
    },

    "hunting:setAnimalPosition": (animalId, newCoords) => {
        const animal = animals.find(a => a.id === animalId);
        if (animal) {
            animal.setPosition(newCoords);
        }        
    },

    "hunting:setController": (animalId) => {
        const animal = animals.find(a => a.id === animalId);
        if (animal) {
            animal.isControlled = true;
        }
    },

    "hunting:handleShoot": (animalId, fromPosition) => {
        const animal = animals.find(a => a.id === animalId);
        if (animal) {
            animal.handleShoot(fromPosition);
        }
    },

    "hunting:handleAnimalDeath": (animalId) => {
        const animal = animals.find(a => a.id === animalId);
        if (animal) {
            animal.setState(2);
            animal.setPosition(animal.getCoords());
        }
    },

    "hunting:setOnHuntingGround": (isOnGround, centerPosition, range) => {
        isPlayerOnHuntingGround = isOnGround;

        if (isPlayerOnHuntingGround) {
            currentHuntingGround.CenterPosition = centerPosition;
            currentHuntingGround.Range = range;
        }
    },

    // RAGE EVENTS
    "playerWeaponShot": (targetPosition, targetEntity) => {
        if (!isPlayerOnHuntingGround) {
            return;
        }

        const sniperRifles = [-1466123874,2828843422];
        if (sniperRifles.indexOf(global.getCurrentPlayerWeapon()) == -1) {
            return;
        }
        
        const animal = getAnimalOnPosition(targetPosition);
        if (animal) {
            animal.handleShoot(mp.players.local.position);
        }
    }
});

const sizeByPedModel = {
    [-832573324]: 1.2,
    [1682622302]: 0.7,
    [-664053099]: 1.5
};
const defaultPedModelSize = 1;
function getAnimalOnPosition(pos) {
    if (!pos) {
        return null;
    }

    const animal = animals.find((a) => {
        const aPos = a.getCoords();
        if (aPos == null) {
            return false;
        }

        const modelSize = (sizeByPedModel[a.model]) ? sizeByPedModel[a.model] : defaultPedModelSize;
        
        const dist = mp.game.system.vdist(pos.x, pos.z, pos.y, aPos.x, aPos.z, aPos.y);
        return dist <= modelSize;
    });

    return animal;
}

let animalIdInShortRange = -1;
setInterval(() => {
    if (!isPlayerOnHuntingGround) {
        return;
    }
    
    animalIdInShortRange = -1;
    animals.forEach((animal) => {
        if (animal.state && animal.isControlled) {
            animal.state.calculateBehavior();
        }

        if (animal.stateNum === 2) {
            const playerPos = mp.players.local.position;
            const animalPos = animal.getCoords();
            const dist = mp.game.system.vdist(playerPos.x, playerPos.z, playerPos.y, animalPos.x, animalPos.z, animalPos.y);

            if (dist < 2) {
                animalIdInShortRange = animal.id;
            }
        }
    });

    if (animalIdInShortRange !== -1) {
        global.gui.setData('hud/setPromptData', JSON.stringify({ show: true, items: [{ key: 'E', text: 'interact_30'}] }));
    }
    else {
        global.gui.setData('hud/setPromptData', JSON.stringify({ show: false, items: [] }));
    }
}, 1000);

let antiFloodCheck = 0;
mp.keys.bind(global.Keys.Key_E, false, () => {
    if (!isPlayerOnHuntingGround || global.checkIsAnyActivity() || Date.now() < antiFloodCheck || global.inAction || global.cuffed) return;
    antiFloodCheck = Date.now() + 1000;
    
    if (animalIdInShortRange === -1) {
        return;
    }

    mp.events.callRemote('hunting:hautAnimal', animalIdInShortRange);
});