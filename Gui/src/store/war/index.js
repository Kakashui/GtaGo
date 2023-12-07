export default {
  namespaced: true,
  state: {
    currentOrg: 'The Walt Disney Company',
    title: "war_2",
    totalList: [
      {
        FamilyName: 'keki',
        Kills: 15,
        Points: 1234,
      },
      {
        FamilyName: 'teki',
        Kills: 6,
        Points: 1234,
      },
      {
        FamilyName: 'deki',
        Kills: 0,
        Points: 1234,
      },
      {
        FamilyName: 'kedki',
        Kills: 999999996,
        Points: 1234,
      },
      {
        FamilyName: 'kgeki',
        Kills: 520756,
        Points: 1234,
      },
      {
        FamilyName: 'adawe',
        Kills: 56,
        Points: 124,
      },
      {
        FamilyName: 'meki',
        Kills: 13413,
        Points: 1234,
      },
      {
        FamilyName: 'gomokeki',
        Kills: 9999421243,
        Points: 1234,
      },
      {
        FamilyName: 'gomoseki',
        Kills: 5456,
        Points: 1234,
      },
    ],
    teamList: [
      {
        playerName: 'kedki',
        Kills: 999996,
      },
      {
        playerName: 'kgeki',
        Kills: 520756,
      },
      {
        playerName: 'adawe',
        Kills: 56,
      },
      {
        playerName: 'meki',
        Kills: 13413,
      },
      {
        playerName: 'gomokeki',
        Kills: 99421243,
      },
      {
        playerName: 'gomoseki',
        Kills: 5456,
      },
    ]
  },
  mutations: {
    setTotalList(state, data) {
      state.totalList = data;
    },
    setTittle(state, type){
      if(type == 1)
        state.title = "war_2";
      else if(type == 2)
        state.title = "war_12";
    },
    setTeamList(state, data) {
      state.teamList = data;
    },
    updateTeamList(state, value) {
      const index = state.teamList.findIndex(item => item.UUID == value.UUID);
      if (index >= 0)
        state.teamList[index] = value;
      else
        state.teamList.push(value);
    }
  },
  actions: {
  },
  modules: {
  }
}
  