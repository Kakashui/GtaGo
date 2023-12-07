const BaseState = require('./baseState');

class HuntState extends BaseState {
    stateEntryHandler() {
        if (mp.players.local.remoteId === this.pet.controllerId) {
            const pos = mp.players.local.position;
            const nearestAnimal = global.hunting.getNearestAnimalCoords(pos);

            if (nearestAnimal == null) {
                mp.gui.notify(mp.gui.notifyType.ERROR, 'err_pets_0', 3000);
                
                this.pet.setState(1);
                return;
            }

            if (nearestAnimal.dist < 20) {
                mp.gui.notify(mp.gui.notifyType.ERROR, 'err_pets_1', 3000);
                this.pet.setState(1);
                return;
            }

            this.trackedAnimalCoords = nearestAnimal.coords;
            
        }
    }

    calculateBehavior() {
        if (this.trackedAnimalCoords) {
            this.pet.moveTo(this.trackedAnimalCoords, 1);
        }
    }
}

module.exports = HuntState;