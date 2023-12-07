export default {
  namespaced: true,
  state: {
    showRoulette: false,
    seconds: 20,
    timerIsOn: false,
    animList: [
      ['Bad 2', 'reaction_bad_var_02'],
      ['Terrible 1', 'reaction_terrible_var_01'],
      ['Good 1', 'reaction_good_var_01'],
      ['Terrible 4', 'reaction_terrible_var_04'],
    ],
    // если statsList.length > 6, то показываться будут последние 6 объектов
    statsList: [],
    betResult: {
      show: false,
      isWin: true,
      // color: black || red (любое другое значение - ноль)
      color: 'black',
      values: [10, 25, 29],
      cash: 1000,
    }
  },
  mutations: {
    updateStateList(state, list){
      state.statsList = list;
    }
  },
  actions: {
  },
  modules: {
  }
}
