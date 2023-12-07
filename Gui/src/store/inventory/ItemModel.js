import configs from '../../configs/inventory/configs';
import costumes from './costumes';
import itemTypes from './itemTypes';

export default class Item {
    constructor(item) {
        this.update(item);
    }

    update(item) {
        this.id = item[0];
        this.count = item[1];
        this.index = item[2];
        this.promo = item[3] == 1;
        //window.console.log(`id: ${this.id} conf: ${configs[this.id]}`);
        this.config = configs[this.id];
        if(this.config === undefined){
            this.id = 194;
            this.config = configs[this.id];
        }
        switch (itemTypes[this.config.Type]) {
            case "Weapon":
                this.serial = item[4];
                break;
            case "Backpack":
            case "KeyRing":
                this.inventoryId = item[4];
                this.weight = item[5];
                break;
            case "Animal":
                this.isActive = item[4];
                break;
            case "CarKey":
                this.vehNumber = `${String.fromCharCode(item[4])}${String.fromCharCode(item[5])}${String.fromCharCode(item[6])}${String.fromCharCode(item[7])}${String.fromCharCode(item[8])}${String.fromCharCode(item[9])}${String.fromCharCode(item[10])}${String.fromCharCode(item[11])}`;
                break;
            case "ItemBox":
                this.storedName = configs[item[4]].DisplayName;
                this.countStored = item[5];
                this.serial = item[6];
                break;
            case "Clothes":
                this.gender = item[4];
                this.drawable = item[5];
                this.texture = item[6];
                break;
            case "Costume":
                this.costumeName = costumes[item[5]];
                break;
            case "Cage":
                this.countFish = `${item[4]}/${this.config.Capacity}`;
                break;
            case "WateringCan":
                this.water = item[4]/1000;
                this.weight = item[5];
                break;
            default:
                break;
        }
    }

    isBackpack() {
        return itemTypes[this.config.Type] == "Backpack";
    }
    isKeyRing() {
        return itemTypes[this.config.Type] == "KeyRing";
    }

    isStackable() {
        return this.config.Stackable;
    }

    isWeapon() {
        return itemTypes[this.config.Type] == "Weapon";
    }

    isClothes() {
        return itemTypes[this.config.Type] == "Clothes";
    }

    isMask(){
        return itemTypes[this.config.Type] == "Clothes" && this.id == 1;
    }

    isCostume() {
        return itemTypes[this.config.Type] == "Costume";
    }

    isProps() {
        return itemTypes[this.config.Type] == "Props";
    }

    isEquipable() {
        return this.isWeapon() || this.isEquipClothes();
    }

    isEquipClothes() {
        return this.isClothes() || this.isProps() || this.isBackpack() || this.isCostume();
    }

    isUsable() {
        return this.config.CanUse
    }

    isType(type) {
        return this.config.Type == type
    }

    isSlotValid(slot) {
        if (!this.config.Slots) return false;
        return (this.config.Slots.find(s => s == slot) !== undefined);
    }

    getWeight() {
        return this.weight || (this.config.Weight * this.count)
    }

    getTypeName() {
        return `inv_type_${itemTypes[this.config.Type].toLowerCase()}`
    }

    getImage() {
        // if(this.isClothes())
        //     return `/img/inventory/items/cl_${this.id}_${this.drawable}_${this.texture}.png`
        // else 
            return `/img/inventory/items/${this.config.Image}.png`
    }

    getDisplayName() {
        return this.config.DisplayName;
    }

    isItemBox() {
        return itemTypes[this.config.Type] == "ItemBox";
    }

    getCountItemInOneReceive() {
        if (!this.config.CountGetItem)
            return 1;
        return this.config.CountGetItem;
    }
}