export default {
  namespaced: true,
  state: {
    businessData: {
      // name: 'General Electric',
      // description: 'Fuel Station',
      // id: 122,
      // price: 12000000,
      // owner: 'Gov',
      // overseer: 'No',
      // purchaseable: false
      // type: 1
    }
  },

  mutations: {
    setBusinessData: function(state, data) {
      state.businessData = data
    }
  }
}
