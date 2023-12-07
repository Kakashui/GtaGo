export default {
  namespaced: true,
  state: {
    show: false,
    title: "Title Title Title Title Title Title Title Title Title",
    subtitle: "subtitle subtitle subtitle subtitle subtitle subtitle subtitle subtitle subtitle subtitle subtitle subtitle",
    background: "linear-gradient(91.37deg, #B6D300 0%, rgba(182, 211, 0, 0) 98.83%)"
  },
  mutations: {
    setMessageData(state, {title, subtitle}){
      state.title = title;
      state.subtitle = subtitle;
    },
    setMesageVisible(state, isShow){
      state.show = isShow;
    }
  },
  actions: {
  },
  modules: {
  }
}
  