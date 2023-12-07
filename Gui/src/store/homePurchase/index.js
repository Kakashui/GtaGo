export default {
  namespaced: true,

  state: {
    homeData: {
      Owner: "Государство",
      Class: "Premium",
      Roommates: 0,
      GarageSpace: 10,
      Price: 12000000,
      IsSelled: false,
      ID: 123,
      IsLocked: true,
      IsTarget: false,
      CanEnter: false
    }
  },

  mutations: {
    setHomeData: function(state, data) {
      state.homeData = data
    }
  }
}
