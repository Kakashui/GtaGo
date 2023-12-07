export default {
  namespaced: true,
  state: {
    transfersList: [
    ]
  },
  mutations: {
    setTransfersList: function(state, data){
      state.transfersList = data;
    },
    updateTransfersList: function(state, data){
      const index = state.transfersList.findIndex(item => item.ID == data.ID)
      if (index >= 0)
        state.transfersList[index] = data;
      else 
        state.transfersList.push(data);
    },
    deleteTransfersList: function(state, id){
      const index = state.transfersList.findIndex(item => item.ID == id)
      if (index >= 0)
        state.transfersList.splice(index, 1)
    }
  },
  actions: {
  },
  modules: {
  }
}
  