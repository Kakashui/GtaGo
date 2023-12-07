class BaseState {
    constructor (pet) {
        this.pet = pet;
    }

    calculateBehavior() {
        // nothing
    }

    stateEntryHandler() {

    }
}

module.exports = BaseState;