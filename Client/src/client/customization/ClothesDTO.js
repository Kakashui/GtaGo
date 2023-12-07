const ClothesModel = require('./ClothesModel');

const maleBody = {
    "0": 0,
    "5": 5,
    "9": 0,
    "14": 12,
    "15": 15
}
const femaleBody  = {
    "0": 0,
    "5": 4,
    "9": 0,
    "14": 14,
    "15": 15
}
//items: [0,5,9,14],
module.exports = class ClotesDTO{
    constructor(top, pants, shoes) {
        this.gender = true;
        this.Shirt = parseInt(top);
        this.Pants = parseInt(pants);
        this.Shoes = parseInt(shoes);
        this.defaultShirt = parseInt(top);
        this.defaultPants = parseInt(pants);
        this.defaultShoes = parseInt(shoes);
    }

    changeGender(gender){
        this.gender = gender;
    }

    updateShirt(value){
        this.Shirt = value;
        this.setSelected();
    }

    updatePants(value){
        this.Pants = value;
        this.setSelected();
    }

    updateShoes(value){
        this.Shoes = value;
        this.setSelected();
    }

    setDefault(){
        global.setClothing(mp.players.local, 11, this.defaultShirt, 0, 0);
        global.setClothing(mp.players.local, 4, this.defaultPants, 0, 0);
        global.setClothing(mp.players.local, 6, this.defaultShoes, 0, 0);
        global.setClothing(mp.players.local, 3, 15, 0, 0);
        global.setClothing(mp.players.local, 8, 15, 0, 0);
    }

    setSelected(){
        global.setClothing(mp.players.local, 11, this.Shirt, 0, 0);
        global.setClothing(mp.players.local, 4, this.Pants, 0, 0);
        global.setClothing(mp.players.local, 6, this.Shoes, 0, 0);
        global.setClothing(mp.players.local, 3, this.gender ? maleBody[this.Shirt] : femaleBody[this.Shirt], 0, 0);
        global.setClothing(mp.players.local, 8, 15, 0, 0);
    }
    update(isCloth){
        if(isCloth)
            this.setSelected();
        else
            this.setDefault();
    }
}