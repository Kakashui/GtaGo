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
      ]
    }
  },

  mutations: {
    setCarState: function(state, value) {
      state.carState = value;
    }
  }
}