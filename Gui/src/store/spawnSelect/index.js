export default {
  namespaced: true,

  state: {
    spawns: ['Название семьи','Адрес последнего захода','','Русская мафия']
  },

  mutations: {
    setData(state, data){
      state.spawns = data
    }
  }
}
