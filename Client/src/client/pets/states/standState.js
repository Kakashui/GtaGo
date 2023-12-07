const BaseState = require('./baseState');

class StandState extends BaseState {
    stateEntryHandler() {
        this.pet.ped.clearTasksImmediately();
        
        if (mp.players.local.remoteId === this.controllerId) {
            this.pet.setPosition(this.pet.ped.getCoords(true));
        }
    }
}

module.exports = StandState;