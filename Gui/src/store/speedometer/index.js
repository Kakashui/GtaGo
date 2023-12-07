export default {
  namespaced: true,

  state: {
    maxSpeed: 400,
    curSpeed: 10,
    maxFuel: 40,
    curFuel: 20,
    inVeh: false,
    engine: false,
    doors: true,
    belt: false
  },

  mutations: {
    setCurSpeed: function (state, value) {
        state.curSpeed = value;
    },

    setBelt: function (state, value) {
      state.belt = value
    },

    setCurFuel: function (state, value) {
        if(value > state.maxFuel) return;
        state.curFuel = value
    },

    setMaxFuel: function (state, value) {
      state.maxFuel = value
    },

    setInVeh: function (state, value) {
      state.inVeh = value
    },

    setEngine: function (state, value) {
      state.engine = value
    },

    setDoors: function (state, value) {
      state.doors = value
    },
  }
}
