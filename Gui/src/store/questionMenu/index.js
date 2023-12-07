export default {
  namespaced: true,

  state: {
    title: ''
  },

  mutations: {
    setTitle: function (state, text) {
      state.title = text
    }
  }
}
