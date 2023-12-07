import configs from './config'
export default {
  namespaced: true,

  state: {
    configs,
    price: 2
  },

  mutations: {
    setPrice(state, price){
      state.price = price;
    }
  }
}
