module.exports = class FaceFeaturesDTO{
    constructor(overlayId, value) {
        this.OverlayId = parseInt(overlayId);
        this.Value = parseFloat(value);
    }

    setValue(value){
        this.Value = parseFloat(value);
        this.apply();
    }

    apply(){
        mp.players.local.setFaceFeature(this.OverlayId, this.Value);
    }
}