import Item from './ItemModel';

export default class Inventory{
    constructor(items, maxWeight, size){
        this.active = true;
        this.maxWeight = maxWeight;
        this.size = size;
        this.items = [];
        items.forEach(item => {
            this.items.push(new Item(item));
        });
    }   

    updateItem(item){
        const index = this.items.findIndex(i=>i.index == item[2]);
        if(index === -1) this.items.push(new Item(item));
        else if(item[1] > 0) this.items[index].update(item);
        else this.items.splice(index, 1);
    }
}