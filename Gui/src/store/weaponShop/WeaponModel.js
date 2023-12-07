import configs from '../../configs/inventory/configs';
import itemTypes from '../inventory/itemTypes';

export default class WeaponModel{
    constructor(item) {
        this.id = item[0];
        this.price = item[1];
        this.config = configs[this.id];
    }

    getName(){
        return this.config.DisplayName
    }

    getComponents(){
        if(this.config.Components == undefined) return null;
        else return this.config.Components;
    }

    getImage(){
        return `/img/inventory/items/${this.config.Image}.png`
    }

    getModel(){
        //window.console.log(`hash: ${+this.config.ModelHash}`)
        return +this.config.ModelHash;
    }

    isAmmo(){
        return itemTypes[this.config.Type] == "Ammo";
    }

    isWeapon(){
        return itemTypes[this.config.Type] == "Weapon";
    }

    isBodyArmor(){
        return itemTypes[this.config.Type] == "Clothes";
    }
}