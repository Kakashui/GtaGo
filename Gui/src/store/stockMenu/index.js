export default {
  namespaced: true,

  state: {
    // items: [
      // { type: 'money', title: 'gui_768', count: 0, countType: '$' },
      // { type: 'medkits', title: 'gui_769', count: 0, countType: 'gui_773' },
      // { type: 'drugs', title: 'gui_770', count: 0, countType: 'gui_773' },
      // { type: 'mats', title: 'gui_771', count: 0, countType: 'gui_773' },
      // { type: 'weapons', title: 'gui_772', count: 0, countType: 'gui_773' }
    // ],
    // input: 'money',
    // type: null,
    // show: true
    items: [
      { typeKey: 0, typeTitle: 'money', title: 'gui_768', count: 999999999 },
      { typeKey: 1, typeTitle: 'medkits', title: 'gui_769', count: 10000 },
      { typeKey: 2, typeTitle: 'drugs', title: 'gui_770', count: 10000 },
      { typeKey: 3, typeTitle: 'mats', title: 'gui_771', count: 10000 },
      { typeKey: 4, typeTitle: 'weapons', title: 'gui_772', count: 10000 }
    ],
    type: 0,
    input: ''
  },

  mutations: {
    setInput (state, input) {
      state.input = input
    },

    setType (state, type) {
      state.type = type
    },

    setShow (state, payload) {
      state.show = payload
    },

    resetState (state) {
      // state.type = null
      state.input = ''
    },

    setCount(state, payload) {
      payload.forEach((count, index) => {
        state.items[index].count = count;
      })
    },
    
    incType(state) {
      state.type++
    },

    decType(state) {
      state.type--
    }
  }
}
