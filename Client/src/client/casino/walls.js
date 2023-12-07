
let renderTarget = null;

const SCREEN_DIAMONDS = "CASINO_DIA_PL";
const SCREEN_SKULLS = "CASINO_HLW_PL";
const SCREEN_SNOW = "CASINO_SNWFLK_PL";
const SCREEN_WIN = "CASINO_WIN_PL";

const targetName = "casinoscreen_01";
let targetModel = mp.game.joaat('vw_vwint01_video_overlay');
const textureDict = "Prop_Screen_Vinewood";
const textureName = "BG_Wall_Colour_4x4";
let loaded = false;

async function loadWalls(){
    if(loaded) return;
    if(!mp.game.graphics.hasStreamedTextureDictLoaded(textureDict)){
        mp.game.graphics.requestStreamedTextureDict(textureDict, false);

        while (!mp.game.graphics.hasStreamedTextureDictLoaded(textureDict)) {
            await mp.game.waitAsync(0); 
        }
    }

    mp.game.ui.registerNamedRendertarget(targetName, false);
    mp.game.ui.linkNamedRendertarget(targetModel);

    //  SET_TV_CHANNEL_PLAYLIST
    mp.game.invoke("0xF7B38B8305F1FE8B", 0, SCREEN_DIAMONDS, true)

    mp.game.graphics.setTvAudioFrontend(true);
    mp.game.graphics.setTvVolume(-100);
    mp.game.graphics.setTvChannel(0);

    renderTarget = mp.game.ui.getNamedRendertargetRenderId(targetName);

    loaded = true;
}

function unloadWalls(){    
    if(!loaded || renderTarget === null) return;
    mp.game.ui.releaseNamedRendertarget(targetName);
    mp.game.ui.isNamedRendertargetRegistered(targetName);
    mp.game.graphics.setStreamedTextureDictAsNoLongerNeeded(textureDict);
    mp.game.graphics.setTvChannel(-1);
    loaded = false;
}

mp.events.add("onChangeInteriors", (newId, oldId)=>{
    if(newId === 275201){
        if(!loaded)
            loadWalls();
    }else if(oldId === 275201 && loaded){
        unloadWalls();
    }
})

mp.events.add('render', function () {
    if (loaded === true) {
        mp.game.ui.setTextRenderId(renderTarget);            
        mp.game.invoke("0x61BB1D9B3A95D802", 4) //  SET_SCRIPT_GFX_DRAW_ORDER
        mp.game.invoke("0xC6372ECD45D73BCD", true); //  SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU
        //  _DRAW_INTERACTIVE_SPRITE
        mp.game.invoke('0x2BC54A8188768488', textureDict, textureName, 0.25, 0.5, 0.5, 1.0, 0.0, 255, 255, 255, 255);
        mp.game.graphics.drawTvChannel(0.5, 0.5, 1.0, 1.0, 0.0, 255, 255, 255, 255);
        mp.game.ui.setTextRenderId(1);
    }
});