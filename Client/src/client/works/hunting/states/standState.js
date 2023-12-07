const BaseState = require('./baseState');

/**
 * Минимальное расстояние до игроков, на котором животное будет оставаться на месте.
 */
const CALM_RANGE = 35;
/**
 * Минимальное расстояние до игроков в Stealth Mode, на котором животное будет оставаться на месте
 */
const CALM_RANGE_STEAL = 20;

class StandState extends BaseState {
    calculateBehavior() {
        if (!mp.peds.exists(this.animal.ped)) {
            return;
        }
        
        const ped = mp.peds.atHandle(this.animal.ped.handle);
        if(!ped) return;
        const pos1 = ped.getCoords(true);
        
        mp.players.forEachInStreamRange((player) => {
            const pos2 = player.position;

            const dist = mp.game.system.vdist(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);
            const minDist = player.getStealthMovement() ? CALM_RANGE_STEAL : CALM_RANGE;
            
            if (dist < minDist) {
                this.animal.setState(1); // set state to 'WALK'
                return;
            }
        });
    }
}

module.exports = StandState;