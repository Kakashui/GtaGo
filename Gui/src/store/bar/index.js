import productsList from '../../configs/bar/products'
export default {
  namespaced: true,
  
  state: {
    productsList
  },
  mutations:{
    updateDiscounts(state, discounts){
      for (const key in discounts) {
        if (state.productsList[key]) {
            state.productsList[key].sale = discounts[key];
        }
      }
    }
  }
}
