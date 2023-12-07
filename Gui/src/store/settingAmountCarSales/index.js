export default {
  namespaced: true,
  state: {
    carModel: 'Mercedes-AMG G63'
  },
  mutations: {
    setCarModel: function(state, value) {
      state.carModel = value
    }
  },
  actions: {
  },
  modules: {
  }
}
