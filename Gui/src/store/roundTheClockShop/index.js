export default {
  namespaced: true,
  state: {
    money: 100,
    productsSell: [
      {
        name: 'shop24_4',
        cost: 100,
        image: 'baseball',
        product: "Bat",
        single: true
      },
      {
        name: 'shop24_5',
        cost: 50,
        image: 'beer',
        product: "Beer",
        single: true
      },
      {
        name: 'shop24_6',
        cost: 30,
        image: 'chips',
        product: "Crisps",
        single: true
      },
      {
        name: 'shop24_7',
        cost: 250,
        image: 'cigarettes',
        product: "Cigarettes",
        single: true
      },
      {
        name: 'shop24_8',
        cost: 10,
        image: 'housekeeper',
        product: "KeyRing",
        single: false
      },
      {
        name: 'shop24_9',
        cost: 500,
        image: 'crowbar',
        product: "Crowbar",
        single: true
      },
      {
        name: 'shop24_10',
        cost: 1000,
        image: 'fuel',
        product: "GasCan",
        single: false
      },
      {
        name: 'shop24_11',
        cost: 173,
        image: 'hummer',
        product: "Hammer",
        single: true
      },
      {
        name: 'shop24_12',
        cost: 645,
        image: 'lamp',
        product: "Flashlight",
        single: true
      },
      {
        name: 'shop24_13',
        cost: 3000,
        image: 'parachute',
        product: "Parachute",
        single: true
      },
      {
        name: 'shop24_14',
        cost: 250,
        image: 'pizza',
        product: "Pizza",
        single: false
      },
      {
        name: 'shop24_15',
        cost: 700,
        image: 'radio',
        product: "Radio",
        single: true
      },
      {
        name: 'shop24_16',
        cost: 70,
        image: 'sim',
        product: "SIM Card",
        single: true
      },
      {
        name: 'shop24_17',
        cost: 540,
        image: 'wrench',
        product: "Wrench",
        single: true
      },
      {
        name: 'shop24_21',
        cost: 540,
        image: 'healthkit',
        product: "HealthKit",
        single: true
      },
      {
        name: 'shop24_22',
        cost: 540,
        image: 'tablet',
        product: "Tablet",
        single: true
      },
      {
        name: 'item_guitar',
        cost: 1000,
        image: 'guitar',
        product: "Guitar",
        single: true
      },
      {
        name: 'item_microphone',
        cost: 1000,
        image: 'microphone',
        product: "Microphone",
        single: true
      },
      {
        name: 'item_camera',
        cost: 1000,
        image: 'camera',
        product: "Camera",
        single: true
      },
      {
        name: 'item_binoculars',
        cost: 1000,
        image: 'binoculars',
        product: "Binoculars",
        single: true
      },
      {
        name: "item_clipboard",
        cost: 1000,
        image: "clipboard",
        product: "Clipboard",
        single: true
      },
      {
        name: 'item_bong',
        cost: 1000,
        image: 'bong',
        product: "Bong",
        single: true
      },
      {
        name: 'item_lighter',
        cost: 1000,
        image: 'lighter',
        product: "Lighter",
        single: true
      },
      {
        name: 'item_bandage',
        cost: 1000,
        image: 'bandage',
        product: "Bandage",
        single: true
      },
      {
        name: 'item_detector',
        cost: 20000,
        image: 'detector',
        product: "Detector",
        single: true
      }
    ],
    basket: [],
  },
  mutations: {
    putInBasket: function (state, { name, cost, image, product }) {
      const basket = state.basket
      let newItemIndex = null
      for (let index = 0; index < basket.length; index++) {
        const item = basket[index]
        if (item.product === product) {
          newItemIndex = index
        }
      }
      if (newItemIndex === null) {
        basket.push({ name, cost, image, product, count: 1 })
      } else {
        const existingItem = basket[newItemIndex]
        existingItem.count++
      }
    },
    clearBasket(state){
      state.basket.splice(0,state.basket.length   );
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
    setData(state, data, money){
      state.money = money;     
      state.productsSell.forEach(item => {
        const index = data.findIndex(i=>i[0] == item.product);
        if(index === -1) item.cost = 0;
        else item.cost = data[index][1];
      });
    }
  },
  actions: {
  },
  modules: {
  }
}
