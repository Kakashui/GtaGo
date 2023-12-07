import itemsConfig from './items'
import rouletteConfig from './roulette'
import colors from './colors'
import coinKits from './coinkits'
import rarityPrices from './rarityPrices'

export function rouletteItemsById(id, gender){
    const rConfig = rouletteConfig[id];
    const filtred = [];
    rConfig.items.forEach(id => {
        const item = itemsConfig[id];
        if(item.category === 1){
            if(item.data.Gender === gender) 
                filtred.push(item);
        }
        else 
            filtred.push(item);
    });
    return filtred.map(item=>{
        if(item){
            if(!item.color)item.color = colors[item.rarity] || "#FFF";
            return item;
        }
    });
}

export function itemsByCategory (category){
    const filtred = Object.values(itemsConfig).filter(i=>i.category === category);
    return filtred.map(item=>{
        if(item){
            if(!item.color)item.color = colors[item.rarity] || "#FFF";
            return item;
        }
    });
}

export function itemById(id){
    const item = itemsConfig[id];
    if(item === undefined) return undefined;
    if(!item.color)item.color = colors[item.rarity] || "#FFF";
    return itemsConfig[id];
}

export function itemPriceById(id, newPrices){
    if(newPrices === undefined) 
        return itemsConfig[id].price
    else
        return newPrices[id] || itemsConfig[id].price;    
}

export function getRouletteConfig(){
    return rouletteConfig;
}

export function getCoinKitsConfig(){
    return coinKits;
}

export function getRarityPrice(rarity){
    return rarityPrices[rarity].Price || rarityPrices[0].Price
}