export default {
  namespaced: true,

  state: {
    inputItems: ['-', '-', '-', '-', '-', '-'],
    keyboardValues: ['1', '2', '3', '4', '5', '6', '7', '8', '9'],
    isMuted: false,
    isConnected: false,
  },
  
  mutations: {
    appendInputItem: function (state, { index, value }) {
      state.inputItems.splice(index, 1, value)
    },

    removeInputFirstItem: function (state) {
      state.inputItems.shift()
    },

    setIsMuted: function (state, value) {
      state.isMuted = value
    },
    
    setIsConnected: function (state, value) {
      state.isConnected = value
    },

    resetInputItems: function (state) {
      state.inputItems = ['-', '-', '-', '-', '-', '-']
    }
  }
}
