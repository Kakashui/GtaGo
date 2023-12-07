export default {
  namespaced: true,
  state: {
    factor: 1,
    profit: 12000,
    owner: 0,
    ownerType: 1,

    enterpricesList: [
      {
        id: 0,
        key: "DavisQuartz",
        name: 'склад зерна',
        img: 'enterprice-1.png',
        captureisInProgress: false,
        orgId: 2,
        date: '10.08.2020 12:00'
      },
      {
        id: 1,
        name: 'склад великов',
        key: "DavisQuartz",
        img: 'enterprice-2.png',
        captureisInProgress: true,
        orgId: 4,
        date: '10.08.2020 12:00'
      },
      {
        id: 2,
        name: 'свалка',
        key: "DavisQuartz",
        img: 'enterprice-3.png',
        captureisInProgress: false,
        orgId: null,
        date: '10.08.2020 12:00'
      },
      {
        id: 3,
        name: 'ресторан на берегу',
        key: "DavisQuartz",
        img: 'enterprice-4.png',
        captureisInProgress: false,
        orgId: 20,
        date: null
      },
      {
        id: 4,
        name: 'склад с алиэксперсс',
        key: "DavisQuartz",
        img: 'enterprice-5.png',
        captureisInProgress: false,
        orgId: 21,
        date: null
      },
      {
        id: 5,
        name: 'урна',
        key: "DavisQuartz",
        img: 'enterprice-2.png',
        captureisInProgress: false,
        orgId: 24,
        date: null
      },
      {
        id: 6,
        name: 'помойка',
        key: "DavisQuartz",
        img: 'enterprice-2.png',
        captureisInProgress: false,
        orgId: 25,
        date: null
      },
      {
        id: 7,
        name: 'шаверма',
        key: "DavisQuartz",
        img: 'enterprice-4.png',
        captureisInProgress: false,
        orgId: 8,
        date: '10.08.2020 12:00'
      },
      {
        id: 8,
        name: 'склад резиновых женщин',
        key: "DavisQuartz",
        img: 'enterprice-1.png',
        captureisInProgress: false,
        orgId: 11,
        date: '10.08.2020 12:00'
      },

    ],

    fractionNames: {
      [1]: 'FAMILY',
      [2]:'BALLAS',
      [3]:'VAGOS',
      [4]:'MARABUNTA',
      [5]:'BLOOD',
      [16]: 'THELOST'

    }
  },
  mutations: {
    setEnterpricesList: function (state, data) {
      state.enterpricesList = data.companies;
      state.profit = data.profit
    },
    updateEnterpricesList: function (state, value) {
      const index = state.enterpricesList.findIndex(item => item.id == value.id)
      if (index >= 0) {
        state.enterpricesList[index] = value;
      }
      else {
        state.enterpricesList.push(value);
      }
    },
    setOwner: function (state, data) {
      state.owner = data.owner;
      state.ownerType = data.ownerType;
    }
  },
  actions: {
  },
  modules: {
  }
}
