import Inventory from './InventoryModel';
import Item from './ItemModel';
import itemTypes from './itemTypes';
import ClothesSlots from './ClothesSlots';
import clothingNames from './clothingNames';

export default {
    namespaced: true,
    state: {
        equip: {
            clothes: {
              "1": null,
              "2": null,
              "3": null,
              "4": null,
              "5": null,
              "6": null,
              "7": null,
              "8": null,
              "9": null,
              "10": null,
              "11": null,
              "12": null,
              "13": null,
              "14": null,
              "15": null
            },
            weapons: {
                "1": null,
                "2": null,
                "3": null,
                "4": null
              }
        },
        itemTypes,
        personalId: -1,
        stock:{
            id: -1,
            type: -1
        },
        nearItems:{
            id: 0,
            size: 2,
            items: []
        },
        dynamicType: "Equip",
        inventories:{
            
        },
        otherInventories:{},
        fastAccessButtons: undefined,
        clothingNames,
        ClothesSlots,
        checkInventory: undefined
    },

    getters: {
        getClothingName: state => data => {
            if(data[0] == undefined || data[1] == undefined || data[2] == undefined ) return `Type ${type}`;
            //window.console.log(data)
            const gender = data[0];
            const type = data[1];
            const variation = data[2];
            try {
                return state.clothingNames[gender][type][variation] || "Hot Sale New";
            } catch (error) {
                return `Type ${type}`;
            }
        },
        getById: state => id =>{
            if(state.inventories[id] == undefined){
                state.inventories[id] = new Inventory([], -1, 2);
                if(id != 0) window.mp.triggerServer("inv:get:byid", id);
            } 
            return state.inventories[id];
        }
    },
    mutations: {
        //checkinvenntory 
        updateCheckInventory(state, {equip, items}){
            if(!equip || !items) return;
            window.console.log(JSON.stringify(equip))
            window.console.log(JSON.stringify(items))
            const clothes = {};
            const weapons = {};
            equip[0].forEach(c => {
                clothes[c[0]] = c[1] == -1 ? null : new Item(c[1]);
            });
            equip[1].forEach(w => {
                weapons[w[0]] = w[1] == -1 ? null : new Item(w[1]);
            });
            state.checkInventory = {
                equip: {clothes, weapons},
                inventory: new Inventory(items, 40000, 36)
            };
        },
        closeCheckInventory(state){
            state.checkInventory = undefined;
        },
        //inventories
        update(state, {id, items, maxWeight, size}){
            state.inventories[id] = new Inventory(items, maxWeight, size);
            state.inventories = {...state.inventories};
        },

        updateItem(state, {id, item}){
            if(state.inventories[id] == undefined) window.mp.triggerServer("inv:get:byid", id);
            else{
                state.inventories[id].updateItem(item);
            }
        },        
        setPersonalId(state, id) {
            state.personalId = id;
        },
        setStock(state, stock) {
            state.stock = stock;
        },
        close(state){
            state.stockId = -1;
            state.dynamicType = "Equip";
            state.otherInventories = {};
        },
        closeOther(state ,inventoryId){
            if(inventoryId == undefined) return;
            if(state.otherInventories[inventoryId] === undefined) return;
            delete state.otherInventories[inventoryId];
            state.otherInventories = {...state.otherInventories};
        },
        openOther(state ,item){
            if(item.inventoryId == undefined) return;
            if(state.otherInventories[item.inventoryId] !== undefined) return;
            if(state.inventories[item.inventoryId] == undefined) {
                state.inventories[item.inventoryId] = new Inventory([], 0, 0);
                window.mp.triggerServer("inv:get:byid", item.inventoryId);
            }
            state.otherInventories[item.inventoryId] = item.getDisplayName();
            state.otherInventories = {...state.otherInventories};
        },
        //equip
        updateEquip(state, {type, slot, item}){
            if(type == 1){
                state.equip.weapons[slot] = item == null ? null : new Item(item)
            }else{
                state.equip.clothes[slot] = item == null ? null : new Item(item)
            }
        },

        clearEquip(state){
            for (const key in state.equip.clothes) {
                state.equip.clothes[key] = null;
            }
            for (const key in state.equip.weapons) {
                state.equip.weapons[key] = null;
            }
        },

        setDynamic(state, dynamicType){
            state.dynamicType = dynamicType;
        },

        //fast access
        setFastAccessButton(state, {key, from}){
            if(state.fastAccessButtons[key] === undefined) return;
            if(from[0] == "inv"){
                if(from[1] != state.personalId) return;
                const item = state.inventories[state.personalId].items.find(i=>i.index == from[2]);
                if(!item || !item.isUsable()) return;
                state.fastAccessButtons[key] = {type: "inv", id: item.id};
            }else if(from[0] == "eq"){
                if(from[1] != 1) return;
                state.fastAccessButtons[key] = {type: "eq", id: from[2]};
            } else if(from[0] == "access"){
                const temp = state.fastAccessButtons[from[1]];
                if(temp === undefined || state.fastAccessButtons[key] === undefined) return;
                state.fastAccessButtons[from[1]] = state.fastAccessButtons[key];
                state.fastAccessButtons[key] = temp;
                window.mp.trigger("cef:access:setButton", from[1], JSON.stringify(state.fastAccessButtons[from[1]]));
            }
            window.mp.trigger("cef:access:setButton", key, JSON.stringify(state.fastAccessButtons[key]));    
        },

        resetFastAccessButton(state, key){
            if(state.fastAccessButtons[key] === undefined) return;
            state.fastAccessButtons[key] = null;
            window.mp.trigger("cef:access:setButton", key, JSON.stringify(state.fastAccessButtons[key]));  
        },

        setFastAccessData(state, buttons){
            state.fastAccessButtons = buttons;            
        }
    },
}
  