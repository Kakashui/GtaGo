module.exports = class ParentsDTO{
    constructor(father, mother, similarity, skin) {
        this.Father = parseInt(father);
        this.Mother = parseInt(mother);
        this.Skin = parseFloat(skin);
        this.Similarity = parseFloat(similarity);
    }

    updateSimilarity(value){
        this.Similarity = parseFloat(value);
        this.apply();
    }

    updateSkin(value){
        this.Skin = parseFloat(value);
        this.apply();
    }

    setFather(value){
        this.Father = parseInt(value);
        this.apply();
    }

    setMother(value){
        this.Mother = parseInt(value);
        this.apply();
    }

    apply(){
        mp.players.local.setHeadBlendData(
            this.Mother,
            this.Father,
            0,
    
            this.Mother,
            this.Father,
            0,
    
            this.Similarity,
            this.Skin,
            0.0,
    
            true
        );
    }
}