export default {
  namespaced: true,

  state: {
    show: true,
    businessData: {}
  },

  mutations: {
    setShow (state, payload) {
      state.show = payload
    },

    setData (state, payload) {
      state.businessData = payload
    }
  }
}
