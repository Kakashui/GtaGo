const maxComponentId = 11;
const maxDrawableId = 499;
const maxTextureId = 25;

const result = {};
function parse() {
    for (let cId = 0; cId <= maxComponentId; cId++) {
        result[cId] = {};
        for (let dId = 0; dId <= maxDrawableId; dId++) {
            if(mp.players.local.isComponentVariationValid(cId, dId, 0)){
                result[cId][dId] = [];
                for (let tId = 0; tId <= maxTextureId; tId++) {                
                    if(mp.players.local.isComponentVariationValid(cId, dId, tId))
                        result[cId][dId].push(tId)
                    else 
                        break;
                }
            }  
        }
    }
    mp.events.callRemote("mlbr:cloth:valid:save", JSON.stringify(result))
}

mp.events.add("parse:clothes:valid", parse)