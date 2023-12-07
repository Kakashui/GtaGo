export default {
  namespaced: true,
  state: {
    nickname: 'Marvin McKinney',
    taxGames: 20,
    currentTax: 0,
    bank: 1000000
  },
  mutations: {
    setData(state, { name, stateShare, casinoShare, amount }) {
      state.nickname = name
      state.taxGames = stateShare
      state.currentTax = casinoShare
      state.bank = amount
    },
    setAmount(state, amount) {
      state.bank = amount
    }
  },
  actions: {
  },
  modules: {
  }
}
