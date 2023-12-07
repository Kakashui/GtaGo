global.interiorId = 0;

setInterval(
    ()=>{
        const pos = mp.players.local.position;
        const interior = mp.game.interior.getInteriorAtCoords(pos.x, pos.y, pos.z);
        if(interior !== global.interiorId){
            mp.events.call("onChangeInteriors", interior, global.interiorId);
            global.interiorId = interior;
        }
    }
,  500);