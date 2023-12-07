export default {
  namespaced: true,
  state: {
    bizName: 'hair on hawick №120000',
    bizProfit: 9137401,
    bizBalance: 82172430,
    bizTaxBalance: 200000,
    bizTaxMax: 300000,
    accessList: {
      topUp: true,
      withdraw: true,
      bonus: false,
      payForBiz: true,
    },
    isCredit: true,
    currentCredit: {
      isTaken: false,
      rate: 0.14,
      approvedAmount: 1000000,
      date: '21.04.2021 21:45'
    },
    transfersList: [
      {
        name: 'Marvin Testovich',
        comment: 'Money_AtmIn',
        // withdrawal replenishment bonus tax
        value: 1200000,
        directFrom: false,
        recipient: 0,
        tax: 100,
        date: 1000
      },
    ],
    staffList: [
      {
        id: 1,
        name: 'Esther Govard',
        payment: 'Последняя выплата: 19.06.3030',
      },
      {
        id: 2,
        name: 'Esther Howard',
        payment: 'Последняя выплата: 22.06.3030',
      },
      {
        id: 3,
        name: 'Gomo Doward',
        payment: 'Последняя выплата: 15.06.3030',
      },
      {
        id: 4,
        name: 'Sek Soward',
        payment: 'Последняя выплата: 26.06.3030',
      },
      {
        id: 5,
        name: 'Kek Howard',
        payment: 'Последняя выплата: 20.06.3030',
      },
      {
        id: 6,
        name: 'Zek Howard',
        payment: 'Последняя выплата: 21.06.3099',
      },
      {
        id: 7,
        name: 'Gek Howard',
        payment: 'Последняя выплата: 21.06.3099',
      },
      {
        id: 8,
        name: 'Pek Far',
        payment: 'Последняя выплата: 21.06.3099',
      },
      {
        id: 9,
        name: 'Lek Howard',
        payment: 'Последняя выплата: 21.06.3099',
      },
    ]
  },
  mutations: {
    setData: function (state, data) {
      if (data)
      {
        state.bizName = data.bizName;
        state.bizBalance = data.bizBalance;
        state.bizTaxBalance = data.bizTaxBalance;
        state.bizTaxMax = data.bizTaxMax;
        state.isCredit = data.isCredit;
        state.transfersList = data.transfersList;
      }
      else
        state.bizName = null;
    },
    updateBizBalance: function (state, value) {
      state.bizBalance = value;
    },
    updateBizTaxBalance: function (state, value) {
      state.bizTaxBalance = value;
    }
  },
  actions: {
  },
  modules: {
  }
}
