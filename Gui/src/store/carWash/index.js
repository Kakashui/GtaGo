export default {
  namespaced: true,
  state: {
    washId: 12,
    servicesList: [
      {
        key: 'wash',
        text: 'car_wash_6',
        cost: 1000,
        img: 'external-sink.png',
      },
      // {
      //   key: 'internal-sink',
      //   text: 'Внутренняя мойка',
      //   cost: 20000,
      //   img: 'internal-sink.png',
      // },
      {
        key: 'wax',
        text: 'car_wash_7',
        cost: 5000,
        img: 'waxing.png',
      },
      {
        key: 'ceramic',
        text: 'car_wash_8',
        cost: 10000,
        img: 'ceramics.png',
      },
    ]
  },
  mutations: {
    setData: function (state, data) {
      state.washId = data.id
      state.servicesList.forEach(item => {
        switch (item.key) {
          case 'wash':
            item.cost = data.wash;
            break;
          case 'wax':
            item.cost = data.wax;
            break;
          case 'ceramic':
            item.cost = data.ceramic;
            break;
        }
      });
    }
  },
  actions: {
  },
  modules: {
  }
}
