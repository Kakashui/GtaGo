export default {
  namespaced: true,
  state: {
    members: [
      {
        nickname: 'Kek_Kekovich',
        id: 2,
        online: false,
        rank: 0,
        up: 10,
        down: 5,
        rating: 327
      },
      {
        nickname: 'Kek_Lekovich',
        id: 352,
        online: false,
        rank: 0,
        up: 10,
        down: 5,
        rating: 4285
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 0,
        online: true,
        rank: 1,
        up: 10,
        down: 5,
        rating: 11
      },
      {
        nickname: 'Roman_Ivanov',
        id: 1,
        online: true,
        rank: 2,
        up: 10,
        down: 5,
        rating: 0
      },
      {
        nickname: 'Eric_Davidovich',
        id: 3,
        online: true,
        rank: 4,
        up: 10,
        down: 5,
        rating: 4544
      },
      {
        nickname: 'Lol_Lolovich',
        id: 4,
        online: true,
        rank: 3,
        up: 10,
        down: 5,
        rating: 9762
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 5,
        online: false,
        rank: 6,
        up: 10,
        down: 5,
        rating: 7817
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 6,
        online: false,
        rank: 1,
        up: 10,
        down: 5,
        rating: 4528
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 7,
        online: true,
        rank: 1,
        up: 10,
        down: 5,
        rating: 4204
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 8,
        online: true,
        rank: 1,
        up: 10,
        down: 5,
        rating: 1954
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 9,
        online: true,
        rank: 1,
        up: 10,
        down: 5,
        rating: 2592
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 10,
        online: true,
        rank: 1,
        up: 10,
        down: 5,
        rating: 9247
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 11,
        online: true,
        rank: 1,
        up: 10,
        down: 5,
        rating: 6198
      },
      {
        nickname: 'Alexey_Chevcov',
        id: 12,
        online: true,
        rank: 1,
        up: 10,
        down: 5,
        rating: 837
      },
    ]
  },

  mutations: {
    setMembers: function (state, value) {
      state.members = value;
      state.members.sort((a, b) => {
        if (a.online && !b.online)
          return -1;
        else if (b.online && !a.online)
          return 1;
        else if (a.rank - b.rank != 0)
          return a.rank - b.rank;
        else if (a.nickname < b.nickname)
          return -1
          else return 1;
      });
    },
    updateMember: function (state, value) {
      const index = state.members.findIndex(item => item.id == value.id)
      if (index > -1)
        state.members[index] = value;
      else {
        state.members.push(value);
      }
      state.members.sort((a, b) => {
        if (a.online && !b.online)
          return -1;
        else if (b.online && !a.online)
          return 1;
        else if (a.rank - b.rank != 0)
          return a.rank - b.rank;
        else if (a.nickname < b.nickname)
          return -1
          else return 1;
      });
    },
    removeMember: function (state, memberId) {
      const index = state.members.findIndex(item => item.id == memberId)
      if (index > -1)
        state.members.splice(index, 1);
    }
  }
}
