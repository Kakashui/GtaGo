import configs from './config'

export default {
  namespaced: true,

  state: {
    currentTypeId: 0,
    currentColorId: 0,
    currentItemId: 0,
    configs,
    gender: 1,
    price: 120,
    types: [
      {key: 'headdress', config: configs.hats},
      {key: 'hoody', config: configs.tops},
      {key: 'underwears', config: configs.underwears},
      {key: 'pants', config: configs.legs},
      {key: 'footwear', config: configs.feets},
      {key: 'gloves', config: configs.gloves},
      {key: 'asseccories', config: configs.wathces},
      {key: 'glasses', config: configs.glasses},
      {key: 'jewerly', config: configs.clothesJewerly}
    ]
  },

  mutations: {
    setCurrentTypeId: function (state, value) {
      state.currentTypeId = value
    },

    setData: function (state, {gender, price}) {
      state.gender = gender;
      state.price = price;
    },

    setCurrentColorId: function (state, value) {
      state.currentColorId = value
    },

    setCurrentItemId: function (state, value) {
      state.currentItemId = value
    }
  }
}
