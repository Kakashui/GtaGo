import family from './modules/family.js'
import organization from './modules/organization.js'
import business from './modules/business.js'
import credit from './modules/credit.js'
import deposit from './modules/deposit.js'

export default {
  namespaced: true,
  state: {
    account: 456940,
    cardNumber: 8885553535,
    houseBalance: 200000,
    houseBalanceMax: 300000,
    transfersPerDay: 100000,
    dailyTransferLimit: 1000000,
    transactionsList: [
      {
        value: 11000,
        comment: 'The Walt',
        directFrom: true,
        tax: 100,
        date: 10002,
      },
      {
        value: 1200000,
        comment: 'The Walt Disney Company',
        directFrom: false,
        tax: 100,
        date: 10001,
      },
      {
        value: 12001000,
        comment: 'The Company',
        directFrom: true,
        tax: 100,
        date: 10002,
      },
      {
        value: 100000,
        comment: 'Disney Company',
        directFrom: false,
        tax: 0,
        date: 10006,
      },
      {
        value: 90000,
        comment: 'The Disney Company',
        directFrom: true,
        tax: 100,
        date: 10005,
      },
      {
        value: 6000,
        comment: 'The Walt Disney',
        directFrom: true,
        tax: 100,
        date: 10004,
      },
      {
        value: 93000,
        comment: 'The Walt Company',
        directFrom: true,
        tax: 100,
        date: 10003,
      },
      {
        value: 472000,
        comment: 'Walt Disney Company',
        directFrom: false,
        tax: 100,
        date: 10002,
      },
      {
        value: 10480000,
        comment: 'Company',
        directFrom: false,
        tax: 100,
        date: 10001,
      },
    ],
    finesList: [
      {
        id: 0,
        reason: 'bank:menu:1',
        cost: 9999,
        date: '24.07.2021',
      },
    ]
  },
  mutations: {
    setData: function (state, data) {
      state.account = data.account;
      state.cardNumber = data.cardNumber;
      state.houseBalance = data.houseBalance;
      state.houseBalanceMax = data.houseBalanceMax;
      state.transactionsList = data.transactionsList;
      state.finesList = data.finesList;
      state.transfersPerDay = data.transfersPerDay;
      state.dailyTransferLimit = data.dailyTransferLimit;
    },
    updateBalanceWithTransfer: function (state, value) {
      state.transfersPerDay = value
    },
    updateHomeBalance: function (state, value) {
      state.houseBalance = value
    }
  },
  actions: {
  },
  modules: {
    family,
    organization,
    business,
    credit,
    deposit,
  }
}
