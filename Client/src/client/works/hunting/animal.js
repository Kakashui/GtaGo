const { getState } = require('./states/stateFactory');

class Animal {
    constructor(id, model, position, state, isController) {
        this.id = id;
        this.model = model;
        this.position = position;
        this.isControlled = isController;

        this.lastDamageReceived = 0;
        const ped = mp.peds.newValid(model, position, 0, 0);
        if(ped == null) return;
        this.ped = ped;
        
        this.ped.freezePosition(false);
        this.ped.setInvincible(false);
        this.setState(state, false);

    }

    setState(newStateNum, updateServer = true) {
        this.stateNum = newStateNum;
        this.state = getState(newStateNum, this);
        this.state.handleEntryState();

        if (this.isControlled && updateServer) {
            mp.events.callRemote('hunting:updateAnimalState', this.id, newStateNum);
        }
    }

    destroy() {
        this.ped.destroy();
    }

    updatePosition(newCoords) {
        if(this.ped === undefined || !this.ped.doesExist()) return;
        this.position = newCoords;
        this.ped.taskGoStraightToCoord(newCoords.x, newCoords.y, newCoords.z, 3.0, -1, this.ped.getHeading(), 0);
        
        if (this.isControlled) {
            mp.events.callRemote('hunting:updateAnimal', this.id, newCoords.x, newCoords.y, newCoords.z);
        }
    }
    
    setPosition(newCoords) {
        if(this.ped === undefined || !this.ped.doesExist()) return;
        this.position = newCoords;
        this.ped.setCoordsNoOffset(newCoords.x, newCoords.y, newCoords.z, false, false, false);

        if (this.isControlled) {
            mp.events.callRemote('hunting:setAnimalPosition', this.id, newCoords.x, newCoords.y, newCoords.z);
        }
    }

    getCoords() {
        if (this.ped) {
            return this.ped.getCoords(true);
        }
        else {
            return this.position;
        }
    }
    
    handleShoot(fromPosition) {
        // dead state
        if (this.stateNum === 2) {
            return;
        }
        
        if (this.isControlled) {
            const maxDamageDist = 100;
            const maxHp = 100;
            
            if (this.lastDamageReceived > Date.now()) {
                return;
            }
            this.lastDamageReceived = Date.now() + 3000;
            
            const pedPos = this.getCoords();
            const dist = mp.game.system.vdist(pedPos.x, pedPos.y, pedPos.z, fromPosition.x, fromPosition.y, fromPosition.z);

            if (dist < maxDamageDist) {
                const hp = (1 - (dist / maxDamageDist)) * maxHp;
                mp.events.callRemote('hunting:decreaseHp', this.id, hp);
            }

            const coordsToRun = getCoordsToRun(pedPos, fromPosition);
            this.updatePosition(coordsToRun);
        }
        else {
            mp.events.callRemote('hunting:shootAnimal', this.id, fromPosition.x, fromPosition.y, fromPosition.z);
        }
    }
}

const AFTER_SHOOT_DIST = 50;

function getCoordsToRun(pedPos, fromPosition) {
    const pos = new mp.Vector3(pedPos.x, pedPos.y, pedPos.z);
    const directionToRun = pos.subtract(new mp.Vector3(fromPosition.x, fromPosition.y, fromPosition.z)).unit();
    directionToRun.z = 0;

    const coords = pos.add(directionToRun.multiply(AFTER_SHOOT_DIST));    
    coords.z = mp.game.gameplay.getGroundZFor3dCoord(coords.x, coords.y, 1000, 0.0, false);
    return coords;
}

module.exports = Animal;