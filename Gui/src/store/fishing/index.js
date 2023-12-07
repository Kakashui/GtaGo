export default {
    namespaced: true,
    state: {
        mgDifficult: 0,
        miniGame: false,
        showAction: false,
        cageWeight: 20
    },
    mutations: { 
      setData (state, {name, data}) {
        if(state[name])state[name] = data;
      },
      showPage(state, page){
        state.currentPage = page;
      },
      closePage(state){
        state.currentPage = '';
        window.mp.trigger("guiClose")
      },
      setShowAction (state, show){
        state.showAction = show;
      },
      setCageWeight(state, val){
        state.cageWeight = val;
      },  
      startMinigame (state, difficult) {
        state.mgDifficult = difficult
        state.miniGame = true
      },
      miniGameEnd (state, status) {
        window.mp.trigger("cef_endMiniGame", status)
        state.miniGame = false;
      },

    },
    actions: {
      
    },
    modules: {
    }
}
  