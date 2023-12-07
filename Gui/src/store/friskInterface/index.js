export default {
  namespaced: true,

  state: {
    dataLoaded: false,
    playerName: null,
    playerId: null,
    items: []
  },

  mutations: {
    setData: function(state, data) {
      state.items = JSON.parse(data.items); 
      state.playerName = data.name;
      state.playerId = data.id
      state.dataLoaded = true;     
    }
  }
}
