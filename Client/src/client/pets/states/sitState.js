const BaseState = require('./baseState');

const animationsByModel = {
    [1125994524]: { dict: 'creatures@pug@amb@world_dog_sitting@base', name: 'base' },
    [2910340283]: { dict: 'creatures@pug@amb@world_dog_sitting@base', name: 'base' },
    [1126154828]: { dict: 'creatures@dog@move', name: 'sit_loop' },
    [1832265812]: { dict: 'creatures@pug@amb@world_dog_sitting@base', name: 'base' },
    [1318032802]: { dict: 'creatures@dog@move', name: 'sit_loop' },
    [351016938]: { dict: 'creatures@dog@move', name: 'sit_loop' },
}

class SitState extends BaseState {
    stateEntryHandler() {
        const ped = this.pet.ped;

        if (mp.players.local.remoteId === this.controllerId) {
            this.pet.setPosition(ped.getCoords(true));
        }

        const model = ped.getModel();
        let animation = animationsByModel[model];

        if (!animation) {
            animation = { dict: 'creatures@dog@move', name: 'sit_loop' }
        }
        if(!mp.game.streaming.doesAnimDictExist(animation.dict)) return;
        mp.game.streaming.requestAnimDict(animation.dict);
        for (let index = 0;!mp.game.streaming.hasAnimDictLoaded(animation.dict) && index < 250; index++) {
            mp.game.wait(0);
        };
        
        ped.taskPlayAnim(animation.dict, animation.name, 8.0, 1.0, -1, 1, 1.0, false, false, false);
    }
}

module.exports = SitState;