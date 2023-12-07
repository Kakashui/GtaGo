import data from '../../configs/furniture/data'

export default {
  namespaced: true,
  state: {
    categories: data.categories,
    productsList: data.products,
    basket: [],
    currentItem: null,
    pricePart: 100,
  },

  mutations: {
    setPricePart: function(state, value) {
      state.pricePart = value
    },
    putInBasket: function (state, { name, cost, key }) {
      const basket = state.basket
      let newItemIndex = null
      for (let index = 0; index < basket.length; index++) {
        const item = basket[index]
        if (item.key === key) {
          newItemIndex = index
        }
      }
      if (newItemIndex === null) {
        basket.push({ name, cost, key, count: 1 })
      }
    },
    changeInQuantityUp: function(state, {key}) {
      const basket = state.basket
      for (let index = 0; index < basket.length; index++) {
        const item = basket[index]
        if (item.key === key) {
          item.count++
        }
      }
    },
    changeInQuantityDown: function(state, {key}) {
      const basket = state.basket
      for (let index = 0; index < basket.length; index++) {
        const item = basket[index]
        if (item.key === key && item.count > 1) {
          item.count--
        } 
      }
      for (let index = 0; index < basket.length; index++) {
        const item = basket[index]
        if (item.key === key && item.count === 1) {
          basket.splice(index, 1)
        }
      }
    },
    setCurrentItem: function (state, {key, name, cost, desc}) {
      state.currentItem = {key, name, cost, desc}
    },
    deleteItem: function(state, {key}) {
      state.basket.splice(key, 1)
    },
    clearBasket(state){
      state.basket.splice(0, state.basket.length);
    },   
  }
}