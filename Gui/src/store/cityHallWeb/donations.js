export default {
  namespaced: true,
  state: {
    list: [
    ]
  },
  mutations:{
    setDonationList: function(state, value){
      state.list = value;
    }
  }
}
