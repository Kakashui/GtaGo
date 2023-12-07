export default {
  namespaced: true,
  state: {
    carState: {
      carName: 'Mercesed-AMG G63',
      miliage: 100.64,
      generalState: 100,
      price: 1000000,
      owner: 'Charles Gonzales',
      mechState: [
        {
          name: 'engine',
          unicName: 'engine',
          currentLevel: 3,
          maxLevel: 5,
          typeLevel: 'stage 3'
        },
        {
          name: 'turbine',
          unicName: 'turbine',
          currentLevel: 1,
          maxLevel: 4,
          typeLevel: 'stock'
        },
        {
          name: 'transmission',
          unicName: 'transmission',
          currentLevel: 5,
          maxLevel: 5,
          typeLevel: 'max stage'
        },
        {
          name: 'brakes',
          unicName: 'brakes',
          currentLevel: 2,
          maxLevel: 5,
          typeLevel: 'stage 2'
        }
      ],
    },

    registrationNumber: 'V25JIN',
    dateOfIssue: '12.03.2021',
    currentPrice: 1450000
  },
  mutations: {
    setCarState:function(state, data) {
      state.carState = data
    },
    setData:function(state, data) {
      state.registrationNumber = data.registrationNumber
      // state.dateOfIssue = data.dateOfIssuer
      state.currentPrice = data.currentPrice
    }
  },
  actions: {
  },
  modules: {
  }
}
