import levels from '../../../../configs/gameMakeWeapon/levels'

export default {
  namespaced: true,
  state: {
    userName: '',
    userExperience: 0,
    //userProgress: 0, // прогресс до следующего уровна (%)
    userGain: 0,
    details: 0,
    levels
  },
  getters: {
    userLevel: (state) => {
      let level = 1;
      for (let index = state.levels.length - 1; index >= 0; index--) {
        if(state.levels[index].exp < state.userExperience) {
          level = state.levels[index].level;
          break;
        }
      } 
      return level;
    }
  },
  mutations: {
    updateUserExperience(state, experience) {
      state.userExperience = experience;
    },
    updateUserGain(state, gain) {
      state.userGain = +gain;
    },
    addUserGain(state, gain) {
      state.userGain += +gain;
    },
    setDetails(state, details){
      state.details = details;
    },
    setName(state, name){
      state.userName = name;
    }
  },
};
