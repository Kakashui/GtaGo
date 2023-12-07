export default {
  namespaced: true,

  state: {

    gender: true,
    price: 100,
    hairs: {
      male:{
        min: 0,
        max: 512,
        step: 1,
        exclude:[23],
        price: 3000,
        colors: 63,
        hair: 74
      },
      female:{
        min: 0,
        max: 534,
        step: 1,
        exclude:[24],
        price: 3000,
        colors: 63,
        hair: 78
      }
    },
    eyebrows: {
        min: 0,
        max: 33,
        step: 1,
        exclude:[],
        price: 500,
        colors: 63
    },
    torso: {
        min: -1,
        max: 16,
        step: 1,
        exclude:[],
        price: 1000,
        colors: 63
    },
    lenses: {
        min: 0,
        max: 31,
        step: 1,
        exclude:[],
        price: 5000
    },
    pomade: {
        min: -1,
        max: 9,
        step: 1,
        exclude:[],
        price: 500,
        colors: 63
    },
    blush: {
        min: -1,
        max: 6,
        step: 1,
        exclude:[],
        price: 1500,
        colors: 63
    },
    shadows: {
        min: -1,
        max: 71,
        step: 1,
        exclude:[],
        price: 10000,
        colors: 63
    },
    beard:{
      min: -1,
      max: 28,
      step: 1,
      exclude:[],
      price: 5000,
      colors: 63
    }
  },
  mutations:{
    setData(state, {gender, price}){
      state.gender = gender;
      state.price = price;
    }
  },
  getters:{
    config: state=>key=>{
      switch (key) {
        case "hairstyle": return state.gender ? state.hairs.male : state.hairs.female;
        case "eyebrows": return state.eyebrows;
        case "torso": return state.torso;
        case "lenses": return state.lenses;
        case "pomade": return state.pomade;
        case "blush": return state.blush;
        case "shadows": return state.shadows;      
        case "beard": return state.beard;   
        default: return {}
      }
    }
  }
}
