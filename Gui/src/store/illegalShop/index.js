export default {
  namespaced: true,
  state: {
    money: 999999,
    productsSell: [
      {
        name: 'Топор',
        cost: 100,
        product: "axe",
        count: 1
      },
      {
        name: 'Мешок',
        cost: 50,
        product: "mesh",
        count: 1
      },
      {
        name: 'Динамит',
        cost: 30,
        product: "dynamite",
        count: 1
      },
      {
        name: 'Нож',
        cost: 250,
        product: "knife",
        count: 1
      },
      {
        name: 'Отмычка',
        cost: 10,
        product: "lockpick",
        count: 1
      },
      {
        name: 'Металло-детектор',
        cost: 500,
        product: "metal-detector",
        count: 1
      },
      {
        name: 'Кирка',
        cost: 1000,
        product: "pickaxe",
        count: 1
      },
      {
        name: 'Ремкомплект',
        cost: 173,
        product: "repair-kit",
        count: 1
      },
      {
        name: 'Стяжка',
        cost: 645,
        product: "screed",
        count: 1
      },
      {
        name: 'Стетоскоп',
        cost: 645,
        product: "stetoskop",
        count: 1
      },
      
    ],
    basket: [],
  },
  mutations: {
    putInBasket: function (state, { name, cost, count, product }) {
      const basket = state.basket
      let newItemIndex = null
      for (let index = 0; index < basket.length; index++) {
        const item = basket[index]
        if (item.product === product) {
          newItemIndex = index
        }
      }
      if (newItemIndex === null) {
        basket.push({ name, cost, count, product })
      } else {
        const existingItem = basket[newItemIndex]
        existingItem.count += Number(count)
      }
    },
    deleteItem: function(state, {key}) {
      state.basket.splice(key, 1)
    },
    changeInQuantityUp: function(state, {key}) {
      state.basket[key].count++
    },
    changeInQuantityDown: function(state, {key}) {
      console.log(key)
      if (state.basket[key].count > 1) {
        state.basket[key].count--
      } else {
        state.basket.splice(key, 1)
      }
    },
    setProductsSell: function(state, value) {
      state.productsSell = value;
    }
  },
  actions: {
  },
  modules: {
  }
}
