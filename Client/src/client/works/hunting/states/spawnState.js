const BaseState = require('./baseState');

class SpawnState extends BaseState {
    handleEntryState() {
        if (this.animal.isControlled) {
            this.loadAnimal();
        }
    }

    loadAnimal() {
        try {
            const pos = this.animal.position;
            pos.z = 0;            
            for (let index = 0;pos.z === 0 && index < 250; index++) {
                mp.game.wait(0);
                pos.z = mp.game.gameplay.getGroundZFor3dCoord(pos.x, pos.y, 1000, 0.0, false);
            };
            mp.game.wait(0);
            
            this.animal.setPosition(pos);
            this.animal.setState(0);
        }
        catch (e) { mp.gui.chat.push(`ERROR: loadAnimal = ${e}`) }
    }
}

module.exports = SpawnState;