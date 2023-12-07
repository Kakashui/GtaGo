const BaseState = require('./baseState');

const CALM_RANGE = 40;
const ONE_ITERATION_MOVE_DIST = 50;

class WalkState extends BaseState {
    calculateBehavior() {
        if (!mp.peds.exists(this.animal.ped)) {
            return;
        }
        const ped = mp.peds.atHandle(this.animal.ped.handle);
        const badPedPos = ped.getCoords(true);
        const pedPos = new mp.Vector3(badPedPos.x, badPedPos.y, badPedPos.z);
        
        const directionsFromPlayers = getDirectonsFromPlayers(pedPos);

        if (directionsFromPlayers.length == 0) {
            this.animal.setState(0);
            return;
        }
        
        const finalCoords = getFinalCoords(directionsFromPlayers, pedPos);

        // чтобы животное не выходило за пределы охотничьих угодий,
        // ибо всё сломается к хуям
        // (TODO: изменять направление движение туши по окружности)
        const currentHuntingGround = global.hunting.getCurrentHuntingGround();
        if (currentHuntingGround) {
            const centerPos = currentHuntingGround.CenterPosition;
            const isFinalCoordsOnGround = (Math.pow((finalCoords.x - centerPos.x), 2) + Math.pow((finalCoords.y - centerPos.y), 2) <= Math.pow(currentHuntingGround.Range, 2));
            if (!isFinalCoordsOnGround) {
                return;
            }
        }
        
        this.animal.updatePosition(finalCoords);
    }
}

function getDirectonsFromPlayers(pedPos) {
    const directionsFromPlayers = [];
    
    mp.players.forEachInStreamRange((player) => {
        const plPos = new mp.Vector3(player.position.x, player.position.y, player.position.z);

        const dist = mp.game.system.vdist(pedPos.x, pedPos.y, pedPos.z, plPos.x, plPos.y, plPos.z);
        if (dist < CALM_RANGE) {
            const vec = pedPos.subtract(plPos);
            directionsFromPlayers.push(vec);
        }
    });

    return directionsFromPlayers;
}

function getFinalCoords(directionsFromPlayers, pedPos) {
    // TODO: Сделать невозможным выход за зону охотничьих угодий
    
    const finalDirection = getCorrectDirection(directionsFromPlayers); 
    
    const finalCoords = pedPos.add(finalDirection.multiply(ONE_ITERATION_MOVE_DIST));
    const testGroundZ = 1000;
    finalCoords.z = mp.game.gameplay.getGroundZFor3dCoord(finalCoords.x, finalCoords.y, testGroundZ, 0.0, false);

    return finalCoords;
}

function getCorrectDirection(directionsFromPlayers) {
    let finalDirection = new mp.Vector3(0, 0, 0);
    directionsFromPlayers.forEach((vec) => {
        finalDirection = finalDirection.add(vec);
    });

    finalDirection.z = 0;
    finalDirection = finalDirection.unit();

    return finalDirection;
}

module.exports = WalkState;