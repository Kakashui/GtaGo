export default {
  namespaced: true,
  state: {
    show: false,
    progress: 0,
    level: 0,
    money: 0
  },
  mutations: {
    show(state){
      state.show = true;
    },
    hide(state){
      state.progress = 0;
      state.level = 0;
      state.money = 0;
      state.show = false;
    },
    update(state, {progress, level, money}){
      state.progress = progress;
      state.level = level;
      state.money = money;
    }
  },
  actions: {
  },
  modules: {
  }
}
  