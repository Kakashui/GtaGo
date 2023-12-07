
export default {
  namespaced: true,

  state: {
    money: 100,
    items: [
      {
        name: 'HotDog',
        unicName: 'HotDog',
        cost: 200,
        single: true
      },
      {
        name: 'Burger',
        unicName: 'Burger',
        cost: 100,
        single: true
      },
      {
        name: 'eCola',
        unicName: 'eCola',
        cost: 120,
        single: true
      },
      {
        name: 'Sprunk',
        unicName: 'Sprunk',
        cost: 110,
        single: false
      },
      {
        name: 'Steak',
        unicName: 'Steak',
        cost: 500,
        single: true
      },
      {
        name: 'Sandwich',
        unicName: 'Sandwich',
        cost: 90,
        single: true
      },
    ],
    shoppingCart: [],
  },

  mutations: {
    putInShoppingCart: function (state, { name, cost, unicName }) {
      const shoppingCart = state.shoppingCart
      let newItemIndex = null
      for (let index = 0; index < shoppingCart.length; index++) {
        const item = shoppingCart[index]
        if (item.unicName === unicName) {
          newItemIndex = index
        }
      }
      if (newItemIndex === null) {
        shoppingCart.push({ name, cost, unicName, count: 1 })
      } else {
        const existingItem = shoppingCart[newItemIndex]
        existingItem.count++
      }
    },
    clearShoppingCart(state){
      state.shoppingCart.splice(0,state.shoppingCart.length   );
    },
    deleteItem: function(state, {key}) {
      state.shoppingCart.splice(key, 1)
    },
    changeInQuantityUp: function(state, {key}) {
      state.shoppingCart[key].count++
    },
    changeInQuantityDown: function(state, {key}) {
      console.log(key)
      if (state.shoppingCart[key].count > 1) {
        state.shoppingCart[key].count--
      } else {
        state.shoppingCart.splice(key, 1)
      }
    },
    setData(state, { ItemCostByName, Money }){
      state.money = Money;
      
      state.items.forEach(item => {
        const cost = ItemCostByName[item.unicName];
        item.cost = cost;
      });
    }
  },
  actions: {
  },
  modules: {
  }
}