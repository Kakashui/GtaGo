export default {
  namespaced: true,
  state: {
    currentPage: 'FurniturePage',
    houseId: 142,
    houseType: 1,// 0 - personal, 1 - family
    houseCost: 2222,
    houseLocked: false,
    paidBefore: '11-11-2020',
    furnitureList: [
      {
        id: 0,
        name: 'какой-то диван', 
        placed: true, 
        key: 'sofa-m',
      },
      {
        id: 1,
        name: 'какой-то диван', 
        placed: false, 
        key: 'sofa-m' 
      },
      {
        id: 2,
        name: 'какой-то диван', 
        placed: false, 
        key: 'sofa-m' 
      },
      {
        id: 3,
        name: 'какой-то диван', 
        placed: true, 
        key: 'sofa-m' 
      },
      {
        id: 4,
        name: 'какой-то диван', 
        placed: false, 
        key: 'sofa-m' 
      },
      {
        id: 5,
        name: 'какой-то диван', 
        placed: false, 
        key: 'sofa-m' 
      },
    ],
    interiorList: [
      { 
        name: 'интерьер', 
        bought: false,
        key: 1
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 2
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 3
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 4
      },
      { 
        name: 'интерьер', 
        bought: true,
        key: 5
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 6
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 7
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 8
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 9
      },
      { 
        name: 'интерьер', 
        bought: true,
        key: 10
      },
      { 
        name: 'интерьер', 
        bought: true,
        key: 11
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 12
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 13
      },
      { 
        name: 'интерьер', 
        bought: true,
        key: 14
      },
      { 
        name: 'интерьер', 
        bought: true,
        key: 15
      },
      { 
        name: 'интерьер', 
        bought: true,
        key: 16
      },
      { 
        name: 'интерьер', 
        bought: false,
        key: 17
      },
    ],
    currentInteriorId: 1,
    totalVehicles: 2,
    occupiers: [// Жильцы
      { 
        name: 'Vladimir Putin', 
        garageAccess: false, 
        safeAccess: true 
      },
    ],
    rentCost: 125,
    currentGarage: {
      type: 'premium',
      placesCount: 5
    },
    typeOfGarages: [
      {
        type: 'standart',
        desc: '1 место',
        placesCount: 1,
        cost: 0,
      },
      {
        type: 'vip',
        desc: '3 места',
        placesCount: 3,
        cost: 55000,
      },
      {
        type: 'premium',
        desc: '5 мест',
        placesCount: 5,
        cost: 100000,
      },
      {
        type: 'radiant',
        desc: '7 мест',
        placesCount: 7,
        cost: 300000,
      },
      {
        type: 'maxquality',
        desc: '15 мест',
        placesCount: 15,
        cost: 1100000,
      }
    ]
  },
  mutations: {
    setCurrentPage: function (state, page) {
      state.currentPage = page
    },
    setHouseLocked: function(state) {
      state.houseLocked = !state.houseLocked
    },
    uninstallFurniture: function(state, index) {
      state.furnitureList.find(e => e.id === index).placed = false
      window.mp.trigger("homeMenu:uninstallFurniture", state.houseId, index)
    },
    uninstallAllFurniture: function(state) {
      state.furnitureList.forEach(element => element.placed = false);
      window.mp.trigger("homeMenu:uninstallAllFurniture", state.houseId)
    },
    setSafe: function(state, {item, uuid}) {
      const index = state.occupiers.findIndex(item => item.uuid == uuid)
      state.occupiers[index].safeAccess = item
      window.mp.triggerServer("house:changeAccess", state.houseId, 0, item, uuid)
    },
    setGarage: function(state, {item, uuid}) {
      const index = state.occupiers.findIndex(item => item.uuid == uuid)
      state.occupiers[index].garageAccess = item
      window.mp.triggerServer("house:changeAccess", state.houseId, 1, item, uuid)
    },
    deleteOccupier: function(state, uuid) {
      const index = state.occupiers.findIndex(item => item.uuid == uuid)
      state.occupiers.splice(index, 1)
      window.mp.triggerServer("house:occupierDeleted", state.houseId, uuid)
    },
    deleteAllOccupiers: function(state) {
      window.mp.triggerServer("house:allOccupiersDeleted", state.houseId)
      state.occupiers = []
    },
    setRentCost: function(state, rentCost) {
      state.rentCost = rentCost
    },
    setFullState: function(state, payload) {
      state.rentCost = payload.rentCost
      state.houseId = payload.houseId
      state.houseType = payload.houseType
      state.houseCost = payload.houseCost
      state.houseLocked = payload.houseLocked
      state.paidBefore = payload.paidBefore
      state.furnitureList = payload.furnitureList
      state.interiorList = payload.interiorList
      state.currentInteriorId = payload.currentInteriorId
      state.totalVehicles = payload.totalVehicles
      state.occupiers = payload.occupiers
      state.typeOfGarages = payload.typeOfGarages
      state.currentGarage = payload.currentGarage
    }
  }
}