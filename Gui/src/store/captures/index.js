export default {
  namespaced: true,

  state: {
    capturing: {
      attackStatus: 'we', // us
      myGangName: 'Bloods',
      enemy: 'Ballas',
    },
    gangList: [
      {
        nickname: 'Savannah Nguyen',
        rang: 'Лидер',
        status: false
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'Annette Black',
        rang: 'Gangster',
        status: true
      },
      {
        nickname: 'Kathryn Murphy',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'Cameron Williamson',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'Eleanor Pena',
        rang: 'Gangster',
        status: true
      },
      {
        nickname: 'Guy Hawkins',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'Brooklyn Simmons',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'Annette Black',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'Kathryn Murphy',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'William Smith',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'John Vegas',
        rang: 'Gangster',
        status: false
      },
      {
        nickname: 'Tyler Brand',
        rang: 'Gangster',
        status: false
      },
    ],
    teamList: [
      {
        nickname: 'Kristin Watson',
        rang: 'Gangster'
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster'
      },
      {
        nickname: 'Annette Black',
        rang: 'Gangster'
      },
      {
        nickname: 'Kathryn Murphy',
        rang: 'Gangster'
      },
      {
        nickname: 'Cameron Williamson',
        rang: 'Gangster'
      },
      {
        nickname: 'Eleanor Pena',
        rang: 'Gangster'
      },
      {
        nickname: 'Guy Hawkins',
        rang: 'Gangster'
      },
      {
        nickname: 'Brooklyn Simmons',
        rang: 'Gangster'
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster'
      },
      {
        nickname: 'Annette Black',
        rang: 'Gangster'
      },
      {
        nickname: 'Kathryn Murphy',
        rang: 'Gangster'
      },
      {
        nickname: 'William Smith',
        rang: 'Gangster'
      },
      {
        nickname: 'John Vegas',
        rang: 'Gangster'
      },
      {
        nickname: 'Tyler Brand',
        rang: 'Gangster'
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster'
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster'
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster'
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster'
      },
      {
        nickname: 'Jenny Wilson',
        rang: 'Gangster'
      },
    ]
  },

  mutations: {
    inviteMember: function (state, { key, nickname, rang }) {
      const newObj = { nickname, rang }
      state.gangList.splice(key, 1)
      state.teamList.push(newObj)
    },
    kickMember: function (state, { key, nickname, rang }) {
      const status = false
      const newObj = { nickname, rang, status }
      state.teamList.splice(key, 1)
      state.gangList.push(newObj)
    },
    setCapturing: function (state, value) {
      state.capturing = value
    },
    setGangList: function (state, value) {
      state.gangList = value
    },
    setTeamList: function (state, value) {
      state.teamList = value
    },
    setPlayerState: function (state, value) {
      if (value.status == 2) {
        let memberIndex = state.gangList.findIndex(item => item.nickname == value.nickname)
        if (memberIndex > -1)
          state.gangList.splice(memberIndex, 1);

        if (state.teamList.findIndex(item => item.nickname == value.nickname) == -1)
          state.teamList.push({ nickname: value.nickname, rang: value.rang })
      }
      else {
        let memberIndex = state.teamList.findIndex(item => item.nickname == value.nickname);
        if (memberIndex > -1)
          state.teamList.splice(memberIndex, 1);

        if (state.gangList.findIndex(item => item.nickname == value.nickname) == -1)
          state.gangList.push({ nickname: value.nickname, rang: value.rang, status: value.status == 1 });
        else
          state.gangList[state.gangList.findIndex(item => item.nickname == value.nickname)].status = value.status == 1;
      }
    },
    setDeletePlayer: function (state, nickname) {      
      let memberIndex = state.gangList.findIndex(item => item.nickname == nickname)
      if (memberIndex > -1)
        state.gangList.splice(memberIndex, 1);

      memberIndex = state.teamList.findIndex(item => item.nickname == nickname);
      if (memberIndex > -1)
        state.teamList.splice(memberIndex, 1);
    },

  }
}
