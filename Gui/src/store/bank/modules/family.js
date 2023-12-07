export default {
  namespaced: true,
  state: {
    familyName: 'Черные псы с гетто',
    familyBalance: 19382450,
    houseTaxBalance: 100, 
    houseTaxBalanceMax: 100, 
    transfersList: [
      {
        recipient: 'Marvin Testovich',
        comment: 'Пополнил баланс',
        // withdrawal replenishment bonus work
        value: 1200000,
        tax: 100,
        directFrom: false,
        date: 10001
      },
      {
        recipient: 'Marvin Testovich',
        comment: 'Снял со счета',
        value: 13800,
        tax: 100,
        directFrom: true,
        date: 10002
      },
      {
        recipient: 'Marvin Testovich',
        comment: 'Money_Work',
        value: 936100,
        tax: 0,
        directFrom: false,
        date: 10003
      },
      {
        recipient: 'Marvin Testovich',
        comment: 'Money_Work',
        value: 7037100,
        tax: 0,
        directFrom: true,
        date: 10004
      }
    ],
    staffList: [
      {
        uuid: 1,
        name: 'Esther Govard',
        money: 500,
        date: '06:08:2021'
      },
      {
        uuid: 2,
        name: 'Esther Howard',
        money: 500,
        date: '06:08:2021'
      },
      {
        uuid: 3,
        name: 'Gomo Doward',
        money: 1000,
        date: '06:08:2021'
      },
      {
        uuid: 4,
        name: 'Sek Soward',
        money: 5000,
        date: '06:08:2021'
      },
      {
        uuid: 5,
        name: 'Kek Howard',
        money: 5000,
        date: '06:08:2021'
      },
      {
        uuid: 6,
        name: 'Zek Howard',
        money: 1000,
        date: '06:08:2021'
      },
    ]
  },
  mutations: {
    setData: function (state, data) {
      if (data) {
        state.familyName = data.name
        state.familyBalance = data.balance
        state.transfersList = data.transfersList
        state.houseTaxBalance = data.houseTaxBalance
        state.houseTaxBalanceMax = data.houseTaxBalanceMax
        state.staffList = data.staffList;
      }
      else {
        state.familyName = null;
      }

    },
    updateFamilyBalance: function(state, value) {
      state.familyBalance = value;
    },
    updateFamilyHomeBalance: function(state, value) {
      state.houseTaxBalance = value;
    },
    updateStaffList: function(state, data) {
      const index = state.staffList.findIndex(item => item.uuid == data.uuid)
      if (index >= 0)
        state.staffList.splice(index, 1, data);
      else
        state.staffList.push(data)
    }
  },
  actions: {
  },
  modules: {
  }
}
