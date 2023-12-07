export default {
  namespaced: true,
  state: {
    status: false,
    isShowConfirm: false,
    confirmed: false,
    phone: ""
  },
  mutations: {
    setStatus(state, status){
      state.status = status;
    },
    setConfirmInput(state, status){
      state.isShowConfirm = status;
    },
    setConfirmed(state, status){
      state.confirmed = status;
    },
    setPhone(state, phone){
      state.phone = phone;
    }
  },
  actions: {
  },
  modules: {
  }
}
  