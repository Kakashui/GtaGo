export default {
  namespaced: true,
  state: {
    time: 0,
    length: 0,
    interval: -1
  },
  mutations: {    
    setTimer(state, time){
      state.length = time;
      state.time = time;
      if(state.interval === -1){
        state.interval = setInterval(()=>{
          state.time--;
          if(state.time < 1){
            clearInterval(state.interval);
            state.interval = -1;
          }
        }, 1000);
      }      
    },
    resetTimer(state){
      if(state.interval !== -1){
        clearInterval(state.interval);
        state.interval = -1;
      }
      state.length = 0;
      state.time = 0;
    }
  },
  actions: {
  },
  modules: {
  }
}
  