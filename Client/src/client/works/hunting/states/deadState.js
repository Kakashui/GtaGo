const BaseState = require('./baseState');

class DeadState extends BaseState {
    handleEntryState() {
        if(this.animal && this.animal.ped)
            this.animal.ped.applyDamageTo(1000, true);
    }
}

module.exports = DeadState;