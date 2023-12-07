import chipBlack from '../../../public/img/chipsTradeMenu/chip-black.png'
import chipBlue from '../../../public/img/chipsTradeMenu/chip-blue.png'
import chipGreen from '../../../public/img/chipsTradeMenu/chip-green.png'
import chipRed from '../../../public/img/chipsTradeMenu/chip-red.png'
import chipYellow from '../../../public/img/chipsTradeMenu/chip-yellow.png'

export default {
  namespaced: true,
  state: {
    chips: [
      { id: 0, icon: chipBlack, quantity: 0, price: 500 },
      { id: 1, icon: chipRed, quantity: 0, price: 1000 },
      { id: 2, icon: chipBlue, quantity: 0, price: 2000 },
      { id: 3, icon: chipGreen, quantity: 0, price: 5000 },
      { id: 4, icon: chipYellow, quantity: 0, price: 10000 }
    ],
    curProductId: null,
    playerBank: 1000000,
  },

  mutations: {
    addChip: function (state, id) {
      state.chips[id].quantity++
    },
    
    resetAllChips(state){
      for (const key in state.chips) {
        state.chips[key].quantity = 0;
      }
    },

    setBank(state, bank){
      state.playerBank = bank;
    },

    removeChip: function (state, id) {
      if(state.chips[id].quantity - 1  < 0)state.chips[id].quantity = 0;
      else state.chips[id].quantity--
    },

    setCurrentProduct: function (state, value) {
      state.curProductId = value
    }
  }
}