export default {
  namespaced: true,
  state: {
    bankAccount: 100,
    balance: 0,

    homeBalance: 99000,
    maxHomeBalance: -1,

    homeRentBalance: -1,

    businessBalance: 0,
    maxBusinessBalance: -1,

    penaltySum: 1,
    historyItems: [
      {
        date: 1509560,
        value: 15000,
        recipient: 1503,
        directFrom: true,
        recipientType: 2,
        tax: 0,
        comment: 'Money_Transfer'

      }
    ]
  },
  mutations: {
    setBankAccount: function (state, value) {
      state.bankAccount = value
    },
    setBalance: function (state, value) {
      state.balance = value
    },

    setHomeBalance: function (state, value) {
      state.homeBalance = value
    },
    setMaxHomeBalance: function (state, value) {
      state.maxHomeBalance = value
    },

    setHomeRentBalance: function (state, value) {
      state.homeBalance = value
    },

    setBusinessBalance: function (state, value) {
      state.businessBalance = value
    },
    setMaxBusinessBalance: function (state, value) {
      state.maxBusinessBalance = value
    },

    setPenaltySum: function (state, value) {
      state.penaltySum = value
    },

    setHistoryItems: function (state, value) {
      state.historyItems = value
      state.historyItems.sort((a, b) => b.date - a.date);

    },
    addHistoryItems: function (state, value) {
      state.historyItems.unshift(value)
    },
  },

  actions: {
    setCompleateTrasfer({ dispatch }, data) {
      if (!data)
        return;

      dispatch('smartphone/setTabOrBack', {
        key: 'BankTab',
        section: 'TransferSection',
        subsection: 'NotifySubsection'
      }, { root: true });
    },
    setCompleateTaxPay({ dispatch }, data) {
      if (!data)
        return;

      dispatch('smartphone/setTabOrBack', {
        key: 'BankTab',
        section: 'ServicesSection',
        subsection: 'NotifySubsection'
      }, { root: true });
    },
  }
}
