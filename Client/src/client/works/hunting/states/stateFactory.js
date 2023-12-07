const StandState = require('./standState');
const WalkState = require('./walkState');
const DeadState = require('./deadState');
const SpawnState = require('./spawnState');

function getState(state, animal) {
    switch (state) {
        case -1:
            return new SpawnState(animal);
        case 0:
            return new StandState(animal);
        case 1:
            return new WalkState(animal);
        case 2:
            return new DeadState(animal);
    }
};

module.exports.getState = getState;