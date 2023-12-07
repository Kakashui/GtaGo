
export default {
  namespaced: true,

  state: {
    currentMatch: [
      {
        nickname: 'Sarexa_Domina',
        kills: 900000,
        place: 1
      },
      {
        nickname: 'Saa_Domina',
        kills: 700000,
        place: 2
      },
      {
        nickname: 'Sandrea_Domina',
        kills: 500000,
        place: 3
      },
      {
        nickname: 'Sa_Domina',
        kills: 390000,
        place: 4
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 300000,
        place: 5
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 200000,
        place: 6
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 10000,
        place: 7
      },
      {
        nickname: 'Pupkin_Vasya',
        kills: 1000,
        place: 8
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 99,
        place: 9
      },
    ],
    allTime: [
      {
        nickname: 'Sandrexa_Domina',
        kills: 9999990,
        place: 1
      },
      {
        nickname: 'Pupkin_Vasya',
        kills: 707700,
        place: 2
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 509990,
        place: 3
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 399900,
        place: 4
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 300000,
        place: 5
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 201200,
        place: 6
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 140800,
        place: 7
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 1001,
        place: 8
      },
      {
        nickname: 'Sandrexa_Domina',
        kills: 93,
        place: 9
      },
    ],
    listSearch: [
      {
        nickname: 'Sandrexa_Domina',
        kills: 93,
        place: 9
      }
    ]
  },

  mutations: {
    setData:function (state, data){
      state.currentMatch = data.currentMatch;
      state.allTime = data.allTime;
    },
    setListSearch: function (state, data) {
      state.listSearch = data;
    }
  },
  actions: {
  },
  modules: {
  }
}