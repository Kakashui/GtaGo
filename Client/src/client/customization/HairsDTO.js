module.exports = class HairsDTO{
    constructor(hair, color1, color2) {
        this.Id = parseInt(hair);
        this.Color1 = parseInt(color1);
        this.Color2 = parseInt(color2);
    }

    setHair(id){
        this.Id = parseInt(id);
        this.apply();
    }

    setColor1(id){
        this.Color1 = parseInt(id);
        this.apply();
    }

    setColor2(id){
        this.Color2 = parseInt(id);
        this.apply();
    }

    apply(){
        global.setClothing(mp.players.local, 2, this.Id, 0, 0);
        mp.players.local.setHairColor(this.Color1, this.Color2);
    }
}