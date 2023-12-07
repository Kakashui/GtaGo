const Pet = require("./pet");

let sendingExcept = false;

const loadedPets = [];
let controlledPet = null;
let requestedControlledPetDelete = false;

global.isHandlePet = (handle) => {
    return controlledPet && controlledPet.ped.handle === handle; 
};

global.changeControlledPetState = (newState) => {
    if (controlledPet) {
        controlledPet.setState(newState);
    }
};

mp.events.add({
    // SERVER EVENTS
    "pets:setControlledPet": (data) => {
        const { ID, Model, Position, ControllerId, State } = JSON.parse(data);
        controlledPet = new Pet(ID, Model, Position, ControllerId, State);
    },

    "pets:unloadControlledPed": (isRemovedByClient) => {
        if (isRemovedByClient) {
            if (requestedControlledPetDelete) {
                return;
            }
            mp.events.callRemote('pets:destroyPet');
            requestedControlledPetDelete = true;
            return;
        }
        
        if (controlledPet) {
            controlledPet.destroy();
            controlledPet = null;
        }

        requestedControlledPetDelete = false;
    },

    "pets:loadPet": (data) => {
        const { ID, Model, Position, ControllerId, State } = JSON.parse(data);
        if (!loadedPets.find(p => p.id === ID)) {
            const pet = new Pet(ID, Model, Position, ControllerId, State);
            loadedPets.push(pet);
        }
    },

    "pets:unloadPet": (petId) => {
        const idx = loadedPets.findIndex(p => p.id === petId);
        if (loadedPets[idx]) {
            loadedPets[idx].destroy();
            loadedPets.splice(idx, 1);
        }
    },

    "pets:move": (petId, position, speed) => {
        const pet = loadedPets.find(p => p.id === petId);
        if (!pet) {
            return;
        }
        pet.moveTo(position, speed);
    },

    "pets:setPosition": (petId, position, speed) => {
        const pet = loadedPets.find(p => p.id === petId);
        if (!pet) {
            return;
        }
        pet.moveTo(position, speed);
    },

    "pets:setState": (petId, state) => {
        const pet = loadedPets.find(p => p.id === petId);
        pet.setState(state);
    },

    // RAGE EVENTS
    "entityStreamIn": (entity) => {
        try {
            if (entity && entity.type === 'player') {
                const petId = entity.getVariable('pets:id');
                if (petId) {
                    mp.events.callRemote('pets:loadPet', petId);
                }
            }
        } catch (e) {
            if(global.sendException) mp.serverLog(`pets.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);            
        }
    },

    "entityStreamOut": (entity) => {
        try {            
            if (entity && entity.type === 'player') {
                const pet = loadedPets.find(p => p.controllerId === entity.remoteId);
                if (pet) {
                    mp.events.callRemote('pets:unloadPet', pet.id);
                    mp.events.call('pets:unloadPet', pet.id);
                }
            }
        } catch (e) {
            if(global.sendException) mp.serverLog(`pets.entityStreamOut: ${e.name}\n${e.message}\n${e.stack}`);             
        }
    },
});

const STREAM_RANGE = 250;
setInterval(() => {
    try {
        if (controlledPet != null && controlledPet.state) {
            if (controlledPet.ped) {
                const playerPos = mp.players.local.position;
                const petPos = controlledPet.ped.getCoords(true);
                const distance = mp.game.system.vdist(playerPos.x, playerPos.y, playerPos.z, petPos.x, petPos.y, petPos.z);
    
                if (distance > STREAM_RANGE) {
                    mp.events.call('pets:unloadControlledPed', true);
                }
            }
            
            controlledPet.state.calculateBehavior();
        }
    }
    catch (e) {
        if (global.sendException && !sendingExcept) {
            sendingExcept = true;
            mp.serverLog(`setInterval: ${e.name}\n${e.message}\n${e.stack}`);    
        }
    }
}, 1000);