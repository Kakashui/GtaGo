export default {
  namespaced: true,
  state: {
    cageItems: {
      0:2,
      2:10,
      4:4,
      6:4,
      7:4,
      8:4,
      11:4,
    }
  },
  mutations: {
    setCageItems(state, items){
      state.cageItems = items;
    }
  },
  actions: {
  },
  modules: {
  }
}
  