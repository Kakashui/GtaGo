export default {
  namespaced: true,
  state: {
    taxiData: {
      location: '3517 W. Gray St. Utica',
      pricePerKm: 100,
      specifyLocation: {
        name: '3517 W. Gray St. Utica, Pennsylvania ...',
        position: { x: 0, y: 0, z: 0 }
      },
      price: 1200,
      driver: {
        name: null,
        car: null,
        phoneNumber: 999999
      }
    },
    taxiJobData: {
      preloader: true,
      stateWork: 'not working',
      isProfileStatsCached: false,
      profile: {
        TotalSumForDay: 0,
        TotalSumForMonth: 0,
        TotalSum: 0,
        TotalTripsForDay: 0,
        TotalTripsForMonth: 0,
        TotalTrips: 0
      },
      orders: [
        // {
        //   ID: 0,
        //   Address: '3517 W. Gray St. Utica, Pennsylvania ...',
        //   Destination: { x: 128, y: 922, z: 200 }
        // }
      ],
      currentOrder: {
        location: null,
        distance: null,
        sum: 2000,
        isCardPayment: true,
        /** Enum:
         * accepted - взят водителем, водитель направляется к клиенту
         * inProgress - клиент сел к водителю
         */
        status: 'accepted'
      }
    },
  },
  mutations: {

    taxi_setSpecifyLocation: function (state, location) {
      state.taxiData.specifyLocation = location;
    },

    taxi_setDriverData: function (state, { Name, Car, Phone }) {
      state.taxiData.driver = { name: Name, car: Car, phoneNumber: Phone };
    },

    taxi_setPricePerKm: function (state, price) {
      state.taxiData.pricePerKm = price;
    },


    
    taxijob_setStateWork: function (state, value) {
      state.taxiJobData.stateWork = value;
    },
    taxijob_setStats: function (state, data) {
      state.taxiJobData.isProfileStatsCached = true;
      state.taxiJobData.profile = data;
    },

    taxijob_addOrderToStats: function (state, orderSum) {
      state.taxiJobData.profile.TotalSumForDay += orderSum;
      state.taxiJobData.profile.TotalSumForMonth += orderSum;
      state.taxiJobData.profile.TotalSum += orderSum;
      state.taxiJobData.profile.TotalTripsForDay += 1;
      state.taxiJobData.profile.TotalTripsForMonth += 1;
      state.taxiJobData.profile.TotalTrips += 1;
    },

    taxijob_addOrder: function (state, order) {
      const existingOrder = state.taxiJobData.orders.find(o => o.ID === order.ID);
      if (existingOrder)
        return;
      state.taxiJobData.orders.push(order);
    },

    taxijob_setOrders: function (state, orders) {
      state.taxiJobData.orders = orders;
    },

    taxijob_removeOrder: function (state, orderID) {
      const orderIdx = state.taxiJobData.orders.findIndex(o => o.ID === orderID);
      if (orderIdx === -1)
        return;
      state.taxiJobData.orders.splice(orderIdx, 1);
    }
  },

  actions: {
    taxi_setSearch({ state, dispatch }, data) {
      if (!data)
        return;
      state.taxiData.driver.name = null;
      state.taxiData.driver.car = null;
      state.taxiData.driver.phoneNumber = null;
      dispatch('smartphone/setTabOrBack', {
        key: 'TaxiTab',
        section: 'SearchSection',
        data: null
      }, { root: true });
    },

    taxi_setNotify({ dispatch }, data) {
      if (!data)
        return;
      dispatch('smartphone/setTabOrBack', {
        key: 'TaxiTab',
        section: 'NotifySection',
        data: null
      }, { root: true });
    },

    taxi_goHome({ dispatch }, data) {
      if (!data)
        return;
      dispatch('smartphone/setTabOrBack', {
        key: 'TaxiTab',
        section: 'GeneralSection',
        data: null
      }, { root: true })
    },

    taxijob_setCurrentOrder({ state, dispatch }, { status, location, sum, isCardPayment }) {
      state.taxiJobData.currentOrder = { status, location, sum, isCardPayment };
      dispatch('smartphone/setTabOrBack', {
        key: 'TaxiJobTab',
        section: 'NotifySection',
        data: null
      }, { root: true });
    },

    taxijob_sendToOrders({ dispatch, commit }, data) {
      if (!data)
        return;
      dispatch('smartphone/setTabOrBack', {
        key: 'TaxiJobTab',
        section: 'OrdersSection',
        data: null
      }, { root: true })
      commit('taxijob_setStateWork', 'working')
    },
  }
}
