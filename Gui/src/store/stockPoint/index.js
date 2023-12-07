export default {
  namespaced: true,

  state: {
    isArmy: false,
    isMed: false
  },

  mutations: {
    setData: function (state, data) {
      state.isMed = data[0];
      state.isArmy = data[1];
    }
  }
}
