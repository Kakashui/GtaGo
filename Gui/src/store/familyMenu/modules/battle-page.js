export default {
  namespaced: true,
  state: {
    pendingWars: [
      { id: 1, status: 0, completed: false, modal: false, incoming: false, won: true, bizName: 'Литейный завод', enemyName: 'Lobster Gang', date: '20.20.2020', time: '23:00', weaponName: 'АК-47', placeName: 'Остров Пасхи', comment: null },
      { id: 2, status: 1, completed: false, modal: false, incoming: true, won: true, bizName: 'Молочный завод', enemyName: 'Crabster Gang', date: '21.21.2020', time: '23:30', weaponName: 'М4А1', placeName: 'Остров Нового Года', comment: null },
      { id: 3, status: 1, completed: false, modal: false, incoming: true, won: true, bizName: 'Молочный завод', enemyName: 'Crabster Gang', date: '21.21.2020', time: '23:30', weaponName: 'М4А1', placeName: 'Остров Нового Года', comment: null }
    ],
    completedWars: [
      { id: 4, status: 4, completed: true, modal: false, incoming: false, won: true, bizName: 'Шоколадный завод', enemyName: 'Gangster Gang', date: '22.22.2020', time: '21:00', weaponName: 'АК-47', placeName: 'Остров Пасхи', comment: null  },
      { id: 5, status: 4, completed: true, modal: false, incoming: true, won: false, bizName: 'Шоколадный завод', enemyName: 'Gangster Gang', date: '22.22.2020', time: '21:00', weaponName: 'АК-47', placeName: 'Остров Пасхи', comment: null  },
      { id: 6, status: 4, completed: true, modal: false, incoming: false, won: true, bizName: 'Шоколадный завод', enemyName: 'Gangster Gang', date: '22.22.2020', time: '21:00', weaponName: 'АК-47', placeName: 'Остров Пасхи', comment: null  },
      { id: 7, status: 5, completed: true, modal: false, incoming: true, won: false, bizName: 'Шоколадный завод', enemyName: 'Gangster Gang', date: '22.22.2023', time: '21:10', weaponName: 'М4А1', placeName: 'Остров Пасхи', comment: null  },
      { id: 8, status: 5, completed: true, modal: false, incoming: false, won: true, bizName: 'Шоколадный завод', enemyName: 'Gangster Gang', date: '22.22.2021', time: '21:20', weaponName: 'М4А1', placeName: 'Остров Пасхи', comment: null  },
      { id: 9, status: 5, completed: true, modal: false, incoming: true, won: true, bizName: 'Мрачный завод', enemyName: 'Useless Gang', date: '23.23.2020', time: '20:30', weaponName: 'М4А1', placeName: 'Остров Нового Года', comment: null }
    ],
    unAcceptedWars: [
      { id: 10, status: 0, completed: false, modal: true, incoming: false, won: true, bizName: 'Первый завод', enemyName: 'Lobster Gang', date: '20.20.2020', time: '23:00', weaponName: 'АК-47', placeName: 'Остров Пасхи', comment: 'Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи!'  },
      { id: 11, status: 0, completed: false, modal: true, incoming: false, won: true, bizName: 'Второй завод', enemyName: 'Crabster Gang', date: '20.20.2020', time: '23:00', weaponName: 'АК-47', placeName: 'Остров Пасхи', comment: 'Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи!!'  },
      { id: 12, status: 0, completed: false, modal: true, incoming: false, won: true, bizName: 'Третий завод', enemyName: 'Noobster Gang', date: '20.20.2020', time: '23:00', weaponName: 'АК-47', placeName: 'Остров Пасхи', comment: 'Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи Остров Пасхи!!!'  }
    ]
  },

  mutations: {
    loadBattles: function(state, data) {
      state.pendingWars = data.filter(item => item.completed == false);
      state.completedWars = data.filter(item => item.completed == true);
      state.unAcceptedWars = data.filter(item => item.modal == true);
    },    
    updateBattleData: function(state, data) {
      let index = state.pendingWars.findIndex(item => item.id == data.id);
      if (index > -1)
      {
        if (data.completed)
          state.pendingWars.splice(index, 1);
        else
          state.pendingWars[index] = data;
      }
      else
      {
        if (!data.completed)
          state.pendingWars.push(data);
      }
      
      index = state.completedWars.findIndex(item => item.id == data.id);
      if (index > -1)
      {
        if (!data.completed)
          state.completedWars.splice(index, 1);
        else
          state.completedWars[index] = data;
      }
      else
      {
        if (data.completed)
          state.completedWars.push(data);
      }     
      index = state.unAcceptedWars.findIndex(item => item.id == data.id);
      if (index > -1)
      {
        if (!data.modal)
          state.unAcceptedWars.splice(index, 1);
        else
          state.unAcceptedWars[index] = data;
      }
      else
      {
        if (data.modal)
          state.unAcceptedWars.push(data);
      }
    },
  }
}
