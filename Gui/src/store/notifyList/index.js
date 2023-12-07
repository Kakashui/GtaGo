export default {
  namespaced: true,
  
  state: {
    items: [],
    waitingItems: [],
    counter: 0,
    types: [
      {
        title: 'info',
        color: '10, 210, 223'
        // color: '#0AD2DF'
      },
      {
        title: 'error',
        color: '233, 21, 21'
        // color: '#E91515'
      },
      {
        title: 'success',
        color: '182, 211, 0'
        // color: '#B6D300'
      },
      {
        title: 'info',
        color: '10, 210, 223'
        // color: '#0AD2DF'
      },
      {
        title: 'warning',
        color: '255, 201, 60'
        // color: '#0AD2DF'
      }
    ]
  },

  mutations: {
    notify: function (state, { type, position, message, time }) {
      
      const items = state.items

      const item = {
        type: state.types[type],
        position,
        message,
        time,
        id: state.counter++
      }

      if (items.length >= 1) {
        state.waitingItems.push(item)
      } else {
        items.push(item)
      }
    },

    addItem: function (state, item) {
      state.items.push(item)
    },

    removeItem: function (state, item) {
      state.items.splice(item, 1)
    },

    removeWaitingItem: function (state, item) {
      state.waitingItems.splice(item, 1)
    }
  }
}
