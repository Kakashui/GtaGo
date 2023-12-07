export default {
  namespaced: true,
  state: {
    truckersData: {
      rankName: "Развозчик  мусора",
      level: 3,
      currentExp: 320,
      requiredExp: 350,
      trucks: [
        {
          id : 0,
          name: "truck 2000",
          rentCost: 1480, 
          available: true, 
          availableLevel: 3,
          reward: 30,
          payment: 100,
        },
        {
          id : 1,
          name: "truck 200",
          rentCost: 2910, 
          available: true, 
          availableLevel: 5,
          reward: 30,
          payment: 100,
        },
        {
          id : 2,
          name: "truck 3000",
          rentCost: 2370, 
          available: true, 
          availableLevel: 4,
          payment: 100,
        },
        {
          id : 3,
          name: "truck",
          rentCost: 900, 
          available: true, 
          availableLevel: 2,
          reward: 30,
          payment: 100,
        },
        {
          id : 4,
          name: "truck 10000",
          rentCost: 230, 
          available: true, 
          availableLevel: 1,
          reward: 30,
          payment: 100,
        },
        {
          id : 5,
          name: "truck 228",
          rentCost: 800, 
          available: true, 
          availableLevel: 2,
          reward: 30,
          payment: 100,
        },
        {
          id : 6,
          name: "truck 2351",
          rentCost: 100, 
          available: true, 
          availableLevel: 1,
          reward: 30,
          payment: 100,
        },
        {
          id : 7,
          name: "treck",
          rentCost: 1050, 
          available: true, 
          availableLevel: 2,
          reward: 30,
          payment: 100,
        },
        {
          id : 8,
          name: "truck",
          rentCost: 4000, 
          available: false, 
          availableLevel: 11,
          payment: 100,
        },
        {
          id : 9,
          name: "truck 123",
          rentCost: 200, 
          available: true, 
          availableLevel: 1,
          reward: 30,
          payment: 100,
        },
        {
          id : 10,
          name: "truck 999",
          rentCost: 1000, 
          available: true, 
          availableLevel: 2,
          reward: 30,
          payment: 100,
        },
        {
          id : 11,
          name: "truck 9000",
          rentCost: 2000, 
          available: true, 
          availableLevel: 3,
          reward: 30,
          payment: 100,
        },
        {
          id : 12,
          name: "truck 3000",
          rentCost: 9000, 
          available: false, 
          availableLevel: 9,
          reward: 30,
          payment: 100,
        }
      ]
    }
  },
  mutations: {
    setData: function(state, truckData) {
      state.truckersData = truckData
    }
  }
}