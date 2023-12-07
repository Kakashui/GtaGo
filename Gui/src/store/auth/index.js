export default {
  namespaced: true,
  state: {
    currentTab: 'LoginTab',
    socialClub: 'no account',
    soundOnStart: true,
    isRememberPass: false,
    auth: {
      login: "",
      password: ""
    }
  },

  mutations: {
    setCurrentTab: function(state, { page }) {
      state.currentTab = page
    },
    updateSoundState(state, toggle){
      state.soundOnStart = toggle;
    },
    setSocialClub: function(state, { name }) {
      state.socialClub = name
    },
    
    setIsRememberPass: function(state, payload) {
      state.isRememberPass = payload
    },

    setPass(state, {login, password, save}){
      state.auth.login = login;
      state.auth.password = password;
      state.isRememberPass = save;
    }
  }
}
