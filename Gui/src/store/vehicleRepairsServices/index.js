export default {
  namespaced: true,
  state: {
    vehicleData: {
      name: 'Mercesed-AMG G63',
      Mileage: 100.64,
      PriceRepair: 1000,

      StateEngine: 39,
      PriceRepairEngine: 1000,

      MileageToChangeOil: 150,
      StateEngineOil: 19,
      PriceChangeOil: 1000,

      MileageToChangeOilTransm: 150,
      StateTransmOil: 100,
      PriceChangeOilTransm: 1000,

      MileageToChangeBrake: 150,
      StateBrake: 20,
      PriceChangeBrake: 1000,
    }
  },

  mutations: {
    setAutoState: function(state, value) {
      state.vehicleData.name = value.name
      state.vehicleData.Mileage = value.Mileage
    },
    setPriceRepair: function(state, value) {
      state.vehicleData.PriceRepair = value
    },
    setEngineState: function(state, value) {
      state.vehicleData.StateEngine = value.StateEngine
      state.vehicleData.PriceRepairEngine = value.PriceRepairEngine
    },
    setEngineOilState: function(state, value) {
      state.vehicleData.MileageToChangeOil = value.Mileage
      state.vehicleData.StateEngineOil = value.State
      state.vehicleData.PriceChangeOil = value.Price
    },
    setTransmOilState: function(state, value) {
      state.vehicleData.MileageToChangeOilTransm = value.Mileage
      state.vehicleData.StateTransmOil = value.State
      state.vehicleData.PriceChangeOilTransm = value.Price
    },
    setBrakeState: function(state, value) {
      state.vehicleData.MileageToChangeBrake = value.Mileage
      state.vehicleData.StateBrake = value.State
      state.vehicleData.PriceChangeBrake = value.Price
    }
  }
}