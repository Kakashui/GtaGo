class BaseState {
    constructor (animal) {
        this.animal = animal;
    }

    calculateBehavior() { }

    handleEntryState() { }
}

module.exports = BaseState;