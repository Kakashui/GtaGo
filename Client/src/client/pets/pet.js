const { getState } = require('./states/stateFactory');

class Pet {
    constructor(id, model, position, controllerId, state) {
        this.id = id;
        this.model = model;
        this.position = position;
        this.controllerId = controllerId;
        const ped = mp.peds.newValid(model, position, 0, 0);
        if(ped == null ) return;
        this.ped = ped;
        let index = 0;
        while (this.ped.handle == 0 || index++ <= 250)
            mp.game.wait(0);

        this.ped.freezePosition(false);
        this.setState(state);
    }

    destroy() {
        this.ped.destroy();
    }

    setState(state) {
        this.state = getState(state, this);
        this.state.stateEntryHandler();
        
        if (mp.players.local.remoteId === this.controllerId) {
            mp.events.callRemote('pets:syncState', this.id, state);
        }
    }

    moveTo(position, speed = 3.0) {
        if(this.ped === undefined) return;
        this.position = position;

        const heading = this.ped.getHeading();

        this.ped.taskGoStraightToCoord(position.x, position.y, position.z, speed, -1, heading, 0);

        if (mp.players.local.remoteId === this.controllerId) {
            mp.events.callRemote('pets:moveToPosition', this.id, position.x, position.y, position.z, speed);
        }
    }

    async setPosition(position) {
        if(this.ped !== undefined) this.ped.setCoordsNoOffset(position.x, position.y, position.z, false, false, false);
        if (mp.players.local.remoteId === this.controllerId) {
            mp.events.callRemote('pets:setPosition', this.id, position.x, position.y, position.z);
        }
    }
}

module.exports = Pet;
