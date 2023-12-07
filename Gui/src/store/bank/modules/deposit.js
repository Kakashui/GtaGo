export default {
  namespaced: true,
  state: {
    currentDepositsList: [
      {
        id: 1,
        balance: 100000,
        profit: 10000,
        time: '35 часов',
        depositTypes: 1,
      },
      {
        id: 2,
        balance: 14000,
        profit: 9000,
        time: '15 часов',
        depositTypes: 2,
      },
    ],
  },
  mutations: {
    setDepositsList: function (state, data) {
      state.currentDepositsList = data
    },
    updateDeposit: function (state, data) {
      const index = state.currentDepositsList.findIndex(item => item.id == data.id);
      if (index >= 0 )
        state.currentDepositsList.splice(index, 1, data);
      else
        state.currentDepositsList.push(data);
    },
    deleteDeposit: function (state, id) {
      const index = state.currentDepositsList.findIndex(item => item.id == id);
      if (index >= 0 )
        state.currentDepositsList.splice(index, 1);
    }
  },
  actions: {
  },
  modules: {
  }
}
