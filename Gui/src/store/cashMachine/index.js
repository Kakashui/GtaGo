export default {
  namespaced: true,

  state: {
    currentPage: { number: 1, key: 3, title: 'dw'},
    username: 'Alexander Nabokov',
    bizName:'wdqwed'
  },

  mutations: {
    updateData(state, data){
      state.username = data[0].replace('_', ' ');
      state.bizName = data[1];
    },
    updateProp(state, {name, val}){
      if(state[name]) state[name] = val;
    },

    setCurrentPage: function (state, value) {
      state.currentPage = value
    },

    setUsername: function (state, name) {
      state.username = name
    },

    setUserId: function (state, value) {
      state.userId = value
    }
  }
}
