export default {
  namespaced: true,
  state: {
    categoriesPoint: [0, 1, 2]
  },
  mutations: {
    setCategories: function(state, data) {
      state.categoriesPoint = data;
    }
  },
  actions: {
  },
  modules: {
  }
}
  