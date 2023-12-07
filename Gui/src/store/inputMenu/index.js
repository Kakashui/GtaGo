export default {
  namespaced: true,

  state: {
    title: 'Tittle',
    plHolder: 'placeholder',
    length: 99
  },

  mutations: {
    setData: function (state, data) {
      state.title = data.tittle;
      state.plHolder = data.plHolder;
      state.length = data.length;
    }
  }
}