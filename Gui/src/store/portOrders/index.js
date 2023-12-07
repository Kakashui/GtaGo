export default {
  namespaced: true,

  state: {
    userName: 'Alexey Volkov',
    buissName: 'Gun Shop',
    productsList: [
      {
        id: 0,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 2000000,
      },
      {
        id: 1,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 2525010,
      },
      {
        id: 2,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 2342550,
      },
      {
        id: 3,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 700,
      },
      {
        id: 4,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 4500000,
      },
      {
        id: 5,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 130000,
      },
      {
        id: 6,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 200400,
      },
      {
        id: 7,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 2053000,
      },
      {
        id: 8,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 2030000,
      },
      {
        id: 9,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 20100,
      },
      {
        id: 10,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 20000,
      },
      {
        id: 11,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 20000000,
      },
      {
        id: 12,
        name: 'Тунец свежий',
        desc: 'Небольшое описание товара',
        cost: 200000,
      }
    ],
    basket: [],
    basketTotal: 0,
    showModal: false,
    currentItem: null,
  },
  mutations: {
    setCurrentItem: function(state, { id, name, desc, cost, count }) {
      state.showModal = true
      state.currentItem = { id, name, desc, cost, count }
    },
    putInBasket: function (state, { id, count }) {
      const basket = state.basket
      let newItemIndex = null
      for (let index = 0; index < basket.length; index++) {
        const item = basket[index]
        if (item.id === id) {
          newItemIndex = index
        }
      }
      if (newItemIndex === null) {
        state.currentItem.count = Number(count)
        basket.push(state.currentItem)
      } else {
        const existingItem = basket[newItemIndex]
        existingItem.count += Number(count)
      }
      state.showModal = false
      state.currentItem = null
    },
    clearBasket(state){
      state.basket.splice(0,state.basket.length   );
    },
    deleteItem: function(state, {key}) {
      state.basket.splice(key, 1)
    },
    pushData(state, data){
      state.productsList = data.productsList;
      state.buissName = data.buissName;
      state.userName = data.userName;
    },
    setData(state, data, money){
      state.money = money;     
      state.productsList.forEach(item => {
        const index = data.findIndex(i=>i[0] == item.id);
        if(index === -1) item.cost = 0;
        else item.cost = data[index][1];
      });
    },
    closeModal(state) {
      state.showModal = !state.showModal;
    }
  },
  actions: {
  },
  modules: {
  }
}