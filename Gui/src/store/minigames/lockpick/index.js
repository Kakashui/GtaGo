export default {
  namespaced: true,
  state: {
    callback: "noCallback"
  },
  mutations: {
    setCallback(state, {callback}){
      state.callback = callback;
    }
  },
  actions: {
  },
  modules: {
  }
}
  