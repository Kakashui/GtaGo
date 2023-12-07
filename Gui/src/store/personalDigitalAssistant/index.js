import dataBase from './dataBase';

export default {
  namespaced: true,
  state: {
    // 0 - police
    // 1 - fbr
    // 2 - military
    type: 1,
    myId: 1245,
    currentPage: "HelpPage",
    helpList: [
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: [
          {
            name: 'Alexey Vsevolodov',
            id: 1245
          },
          {
            name: 'Alexey Vsevolodov',
            id: 1235
          },
          {
            name: 'Alexey Vsevolodov',
            id: 145
          },
        ]
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: [
          {
            name: 'Alexey Vsevolodov',
            id: 1245
          }
        ]
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: [
          {
            name: 'Alexey Vsevolodov',
            id: 1245
          }
        ]
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: []
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: [
          {
            name: 'Alexey Vsevolodov',
            id: 1245
          }
        ]
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: [
          {
            name: 'Alexey Vsevolodov',
            id: 1245
          }
        ]
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: []
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: []
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: []
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: []
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: []
      },
      {
        time: '20:30',
        code: 'КОД-1',
        name: 'Alexey Vsevolodov',
        distance: '33000',
        helpers: []
      }
    ],
    searchHuman: {
      name: 'Alexey Vsevolodov',
      passport: 33333333,
      sex: 'М',
      number: '333 333 3333',
      wantedLevel: 1,
      licenses: 'A B C D E F',
      id: 1
    },
    searchCar: {
      name: 'Alexey Vsevolodov',
      number: '333 333 3333',
      carModel: 'Lamborgini',
      wantedLevel: 4,
      key: 1
    },
    humanListWanted: [
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 1
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 5,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 2
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 3
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 4
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 5
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 6
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 7
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 8
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 9
      },
      {
        name: 'Alexey Vsevolodov',
        passport: 33333333,
        sex: 'М',
        number: '333 333 3333',
        wantedLevel: 1,
        licenses: 'A B C D E F',
        reason: 'test reason',
        id: 10
      }
    ],
    carListWanted: [
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 5,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 4,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 4,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 1,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 4,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 4,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 4,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 4,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 4,
        reason: 'test reason',
        key: 1
      },
      {
        name: 'Alexey Vsevolodov',
        number: '333 333 3333',
        carModel: 'Lamborgini',
        wantedLevel: 4,
        reason: 'test reason',
        key: 1
      },
    ]
  },
  mutations: {
    setCurrentPage: function(state, page) {
      state.currentPage = page
    },
    setType: function(state, value) {
      state.type = value
    },
    setHelpList: function(state, value) {
      state.helpList = value
    },
    setSearchHuman: function(state, value) {
      state.searchHuman = value
    },
    setSearchCar: function(state, value) {
      state.searchCar = value
    },
    setHumanListWanted: function(state, value) {
      state.humanListWanted = value
    },
    setCarListWanted: function(state, value) {
      state.carListWanted = value
    },
    addIntoHelpList: function(state, value) {
      state.helpList.push(value);
    },
    removeFromHelpList: function(state, value) {
      let index = state.helpList.findIndex(item => item.id == value);
      if (index != -1)
        state.helpList.splice(index, 1);
    },
    updatePlayerWantedLVL: function(state, value) {
      if (state.searchHuman.passport == value.passport){
        state.searchHuman.wantedLevel = value.wantedLevel;
      }
      let findPlayer = state.humanListWanted.findIndex(item => item.passport == value.passport);
      if (findPlayer != -1)
      {
        if (value.wantedLevel > 0)
          state.humanListWanted[findPlayer].wantedLevel = value.wantedLevel;
        else 
          state.humanListWanted.splice(findPlayer, 1);
      }
      else
        state.humanListWanted.push(value);
    },
    updateVehicleWantedLVL: function(state, value) {
      if (state.searchCar.key == value.key){
        state.searchCar.wantedLevel = value.wantedLevel;
      }
      let findVehicle = state.carListWanted.findIndex(item => item.key == value.key);
      if (findVehicle != -1)
      {
        if (value.wantedLevel > 0)
          state.carListWanted[findVehicle].wantedLevel = value.wantedLevel
        else 
          state.carListWanted.splice(findVehicle, 1);
      }
      else
        state.carListWanted.pust(value);
    },
    updateHelpers: function(state, value) {
      let findHelp = state.helpList.findIndex(item => item.id == value.id);
      if (findHelp != -1)
        state.helpList[findHelp].helpers = value.helpers
    }
  },
  actions: {
  },
  modules: {
    dataBase
  }
}
