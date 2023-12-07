import tattoos from '../../configs/tattooShop/config'

export default {
  namespaced: true,
  state: {
    tattoos,
    categoryItemsCount: 0,
    price: 120,
    gender: true,
    currentTattoos: [
      {"Collection": 3093597567, "Overlay": 1904300216},
      {"Collection": 3093597567, "Overlay": 2397965201},
      {"Collection": 3093597567, "Overlay": 3661636376},
      {"Collection": 3093597567, "Overlay": 3986835932},
      {"Collection": 3093597567, "Overlay": 929455459},
      {"Collection": 3093597567, "Overlay": 2978173115}
    ],
    cameraPositions:[

    ]
  },
  getters:{
    cameraPosition: () => slot =>{
      switch (slot) {
        case 0: case 1: case 2: case 4: case 5:
          return {angle: 90, pointZ:.6, cameraZ: .6};
        case 3:
          return {angle: -90, pointZ:.6, cameraZ: .6};
        case 6: case 7: case 8: case 13: case 14:
          return {angle: 90, pointZ:.35, cameraZ: .4};
        case 9: case 10: case 11: case 12:
          return {angle: -90, pointZ:.35, cameraZ: .6};
        case 15: 
          return {angle: 190, pointZ:-.35, cameraZ: .6};
        case 16: case 17: 
          return {angle: 190, pointZ:.1, cameraZ: .6};
        case 18: 
          return {angle: 10, pointZ:-.35, cameraZ: .6};
        case 19: case 20: 
          return {angle: 10, pointZ:.1, cameraZ: .6};
        case 21:case 23: 
          return {angle: 90, pointZ:-.55, cameraZ: -.05};
        case 22: case 24: 
          return {angle: 90, pointZ:-.25, cameraZ: -.05};
        default:
          return {angle: 0, pointZ: 0, cameraZ: 0};
      }
    }
  },

  mutations: {
    setData: function(state, {price, gender}) {
      state.price = price;
      state.gender = gender;
    },
    setGender(state, gender){
      state.gender = (gender == true);
    },
    updateTattooList(state, list){
      state.currentTattoos = list;
    },
    setCategoryItemsCount: function(state, payload) {
      state.categoryItemsCount = payload
    }
  }
}
