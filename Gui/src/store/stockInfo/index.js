export default {
  namespaced: true,

  state: {
    data: {
      title: 'Cityhall',
      medkits: 100,
      medkitsMax: 500,
      drugs: 100,
      drugsMax: 500,
      money: 100,
      moneyMax: 500,
      mats: 100,
      matsMax: 500,
    }
  },

  mutations: {
    setData(state, data) {
      state.data = data;
    }
  }
}
