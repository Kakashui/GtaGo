const islandCenter = new mp.Vector3(4664.171, -5098.138, 13.43878);
const dist = 1500.01;
let onIsland = (mp.game.gameplay.getDistanceBetweenCoords(islandCenter.x, islandCenter.y, islandCenter.z, mp.players.local.x, mp.players.local.y, mp.players.local.z, false) < dist);
setInterval(() => {
    var pos = mp.players.local.position;
    if(mp.game.gameplay.getDistanceBetweenCoords(islandCenter.x, islandCenter.y, islandCenter.z, pos.x, pos.y, pos.z, false) < dist){
        if(!onIsland){
            onIsland = true;
            mp.game.invoke("0x9A9D1BA639675CF1", "HeistIsland", true);
            mp.game.invoke("0x5E1460624D194A38", true);
        }        
    }else{
        if(onIsland){
            onIsland = false;
            mp.game.invoke("0x9A9D1BA639675CF1", "HeistIsland", false);
            mp.game.invoke("0x5E1460624D194A38", false);
        }
        
    }
}, 1500)