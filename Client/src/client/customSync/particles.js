function requestNamedPtfxAsset(ptfxAssetName) {
    mp.game.streaming.requestNamedPtfxAsset(ptfxAssetName);
    mp.game.wait(0);
    mp.game.graphics.setPtfxAssetNextCall(ptfxAssetName);
}

mp.events.add({
    "particles:playAtPosition": (position, assetName, effectName, scale, ms) => {
        if (!global.loggedin)
            return;                
        requestNamedPtfxAsset(assetName);        
        const fxHandle = mp.game.graphics.startParticleFxLoopedAtCoord(effectName, position.x, position.y, position.z, 0, 0, 0, scale, false, false, false, true);        
        setTimeout(() => {
            mp.game.graphics.removeParticleFx(fxHandle, false);
        }, ms);
    }
});