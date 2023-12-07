const BaseState = require('./baseState');


const MIN_DIST_FOR_MOVE = 3;

class FollowState extends BaseState {
    stateEntryHandler() {
        this.pet.ped.clearTasksImmediately();
    }
    
    calculateBehavior() {
        const playerPos = mp.players.local.position;
        const petPos = this.pet.ped.getCoords(true);
        const distance = mp.game.system.vdist(playerPos.x, playerPos.y, playerPos.z, petPos.x, petPos.y, petPos.z);

        if (distance < MIN_DIST_FOR_MOVE) {
            return;
        }

        this.pet.moveTo(playerPos);
    }
}

module.exports = FollowState;