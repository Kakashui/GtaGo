export default {
  namespaced: true,
  state: {
    profileVisibility: true, // открыто ли слева окно профиля
    finalDetail: false, // флаг для обозначения, является текущая деталь финальной
    animationActive: false, // запущена ли фоновая анимация,
    helpVisibility: false
  },
  getters: {
  },
  mutations: {
    setProfileVisibility(state, val) {
      state.profileVisibility = val;
    },
    setHelpVisibility(state, val) {
      state.helpVisibility = val;
    },
    setFinalDetail(state, val) {
      state.finalDetail = val;
    },
    setAnimationActive(state, val) {
      state.animationActive = val;
    }
  }
}