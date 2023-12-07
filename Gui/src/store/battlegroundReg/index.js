
export default {
  namespaced: true,

  state: {
    isReg: false,
    peopleCount: 29,
    peopleCapacity: 50
  },

  mutations: {
    setData: function(state, data){
      state.isReg = data.isReg;
      state.peopleCount = data.peopleCount;
    }
  },
  actions: {
  },
  modules: {
  }
}