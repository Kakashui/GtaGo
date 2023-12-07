export default {
  namespaced: true,
  state: {
    contractConsent: true,
    fastMoney: true,
    currentCreditOptionsResult: {
      rate: 0,
      calculatedAmount: 0
    },
    // selectedCreditProperty: null, (если не выбрано имущество под залог)
    selectedCreditProperty: null,
    creditsList: [
      {
        id: 31234,
        amount: 12000000,
        propertyId: 14,
        payment: 10000,
        payedAmount: 0,
        datePayment: '12.08.2021', 
        leftPayment: 5,
        paymentsList: [
          {
            id: 134162,
            payment: 19414,
            amount: 1200000,
            date: '20.08.2021'
          },
          {
            id: 134163,
            payment: 19416,
            amount: 1148000,
            date: '20.08.2021'
          },
          {
            id: 134164,
            payment: 19416,
            amount: 1043000,
            date: '20.08.2021'
          },
          {
            id: 134165,
            payment: 19416,
            amount: 940050,
            date: '20.08.2021'
          },
          {
            id: 134166,
            payment: 19416,
            amount: 814000,
            date: '20.08.2021'
          },
        ]
      },
      {
        id: 52134,
        amount: 91000000,
        propertyId: 410,
        payment: 10000,
        payedAmount: 0,
        datePayment: '12.08.2021', 
        leftPayment: 5,
        paymentsList: [
          {
            id: 371163,
            payment: 194164,
            amount: 1200000,
            date: '20.08.2021'
          }
        ]
      },
      {
        id: 93654,
        amount: 649000,
        propertyId: 0,
        payment: 10000,
        payedAmount: 0,
        datePayment: '12.08.2021', 
        leftPayment: 5,
        paymentsList: [
          {
            id: 378164,
            payment: 194164,
            amount: 1200000,
            date: '20.08.2021'
          }
        ]
      },
    ],
    propertyList: [
      {
        type: 'biz',
        name: 'Кабак на Groove Street',
        id: 0,
        familyProperty: true,
        price: 100,
        isPledged: false,
      },
      {
        type: 'vehicle',
        name: 'Mercedes-benz g63',
        id: 13,
        familyProperty: false,
        price: 100,
        isPledged: false,
      },
      {
        type: 'vehicle',
        name: 'Mercedes-benz g63',
        id: 14,
        familyProperty: true,
        price: 10000000,
        isPledged: false,
      },
      {
        type: 'house',
        name: 'Вилла на побережье',
        id: 410,
        familyProperty: true,
        price: 900000000,
        isPledged: false,
      },
      {
        type: 'house',
        name: 'Вилла на побережье',
        id: 411,
        familyProperty: true,
        price: 900000000,
        isPledged: false,
      },
      {
        type: 'house',
        name: 'Вилла на побережье',
        id: 412,
        familyProperty: true,
        price: 900000000,
        isPledged: false,
      },
      {
        type: 'house',
        name: 'Вилла на побережье',
        id: 413,
        familyProperty: true,
        price: 900000000,
        isPledged: true,
      },
      {
        type: 'house',
        name: 'Вилла на побережье',
        id: 414,
        familyProperty: true,
        price: 900000000,
        isPledged: true,
      },
    ],
    creditOptionsData: {
      minAmount: 100000,
      maxAmount: 1000000,
      minDays: 10,
      maxDays: 90,
    },
  },
  mutations: {
    setData: function (state, data) {
      state.fastMoney = data.fastMoney;
      state.creditsList = data.creditsList;
      state.propertyList = data.propertyList;
      state.currentCreditOptionsResult = {
        rate: 0,
        calculatedAmount: 0
      }
      state.selectedCreditProperty = null;
    },
    updateCredit: function (state, data) {
      const index = state.creditsList.findIndex(item => item.id == data.id);
      if (index >= 0 )
        state.creditsList.splice(index, 1, data);
      else
        state.creditsList.push(data);
    },
    deleteCredit: function (state, id) {
      const index = state.creditsList.findIndex(item => item.id == id);
      if (index >= 0 )
        state.creditsList.splice(index, 1);
    },
    setCurrentCreditOptionsResult: function(state, data) {
      state.currentCreditOptionsResult = data
    },
    setSelectedCreditProperty: function(state, data) {
      if (data.price <= 0)
        return;
      state.selectedCreditProperty = data;
      let price = Math.round(data.price * 0.4);
      state.creditOptionsData.maxAmount = price
      state.creditOptionsData.minAmount = price > 100000 ? 100000 : 1000;
    },
    setContractConsent: function(state, value) {
      state.contractConsent = value;
    },
    clearCurrentCreditOptionsResult: function(state) {
      state.currentCreditOptionsResult = {
        rate: 0,
        calculatedAmount: 0
      }
    },
    setMaxAmountCredit: function(state, data) {
      state.creditOptionsData.maxAmount = data;
      state.creditOptionsData.minAmount = data > 100000 ? 100000 : 1000;
    }
  },
  actions: {
  },
  modules: {
  }
}
