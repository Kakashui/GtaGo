export default {
  namespaced: true,

  state: {
    currentPage: {
      page: 'FuelPage',
      data: null
    },
    currentLiters: 28,
    maxLiters: 40,
    currentFuel: null,
    fuelTypes: [
      {
        key: 'standart',
        title: 'Standart',
        cost: 2,
      },
      {
        key: 'standartPlus',
        title: 'Standart+',
        cost: 3,
      },
      {
        key: 'diesel',
        title: 'Diesel',
        cost: 4,
      },
      {
        key: 'deluxe',
        title: 'Deluxe',
        cost: 10,
      },
      {
        key: 'electro',
        title: 'Electro',
        cost: 18,
      }
    ],
  },

  mutations: {
    setCurrentFuel: function(state, key) {
      state.currentFuel = key
    },

    setCurrentPage: function(state, {page, data}) {
      state.currentPage.page = page
      state.currentPage.data = data
    },

    setData: function(state, value) {
      state.currentLiters = value.cur
      state.maxLiters = value.max
    },

    setFuelTypes: function(state, value) {
      state.fuelTypes = value
    }
  }
}
