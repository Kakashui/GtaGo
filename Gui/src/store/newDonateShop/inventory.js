import {itemById} from '../../configs/newDonateShop'
export default {
    namespaced: true,
    state: {
        items:[],
        newItem:null,
        newIds: 0
    },
    mutations: {
        updateItem(state, {id, count, sell}){
            if(count > 0){
                const exists = state.items.find(i=>i.id === id && i.sell === sell); 
                if(exists){
                    exists.count = count;
                }else{
                    state.items.push({ id, count, sell})
                }
            }else{
                const index = state.items.findIndex(i=>i.id === id && i.sell === sell)
                if(index !== -1)
                    state.items.splice(index, 1);
            }
        },
        addItem(state, {id, sell}){
            const item = itemById(id);
            if(item === undefined) return;
            if(Array.isArray(item.data.Items)){
                item.data.Items.forEach(cId => {
                    const cItem = itemById(cId);
                    const exists = state.items.find(i=>i.id === cId && i.sell === true);
                    if(exists){
                        exists.count += cItem.count;                
                        state.items = [...state.items];
                    }else{
                        state.items.push({ id: cId, count: cItem.count, sell});
                    }   
                });
            }else{
                const exists = state.items.find(i=>i.id === id && i.sell === true);
                if(exists){
                    exists.count += item.count;                
                    state.items = [...state.items];
                }else{
                    state.items.push({ id, count: item.count, sell});
                }          
            }
            state.newItem = item;
        },
        setItems(state, items){
            state.items = items;
        },
        deleteNewItem(state){
            state.newItem = null
        }
    },
    actions: {
    },
    modules: {
    },
    getters:{        
    }
}
  