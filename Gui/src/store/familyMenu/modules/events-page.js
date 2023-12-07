export default {
  namespaced: true,
  state: {
    showBattleResults: false,
    notification: false,
    notificationMessage: '',
    notificationResult: '',
    showNavElem: 'PrivateEvents',
    prevNavElem: 'PrivateEvents',
    businessList: [
      {
        name: 'Магазин мебели',
        id: 987654321,
        famOwner: 'Akorluv',
        income: 2000
      },
      {
        name: 'Торговый центр с заправкой',
        id: 123456789,
        famOwner: 'Groove',
        income: 555
      },
      {
        name: 'Казино',
        id: 489,
        famOwner: 'Akorluv',
        income: 10000
      },
      {
        name: 'Торговый центр рядом с парком',
        id: 1233,
        famOwner: 'Akorluv',
        income: 200000
      },
      {
        name: 'Парикмахерская',
        id: 6789,
        famOwner: 'Akorluv',
        income: 444444
      },
      {
        name: 'Прачечная',
        id: 126789,
        famOwner: 'Akorluv',
        income: 824641
      },
      {
        name: 'Булошная',
        id: 1234567,
        famOwner: 'Akorluv',
        income: 494617
      },
      {
        name: 'Магазин алкоголя',
        id: 12345,
        famOwner: 'Akorluv',
        income: 438764
      },
      {
        name: 'Бар',
        id: 129,
        famOwner: 'Akorluv',
        income: 4372
      },
      {
        name: 'Магазин продуктов',
        id: 99999,
        famOwner: 'Akorluv',
        income: 20000000
      },
    ],
    placesList: [
      {
        name: 'Place_ElBurroHeights',
        img: 'elBurroHeights',
        id: 1,
        active: true,
      },
      {
        name: 'Place_ElysianIsland',
        img: 'elysianIsland',
        id: 2,
        active: false,
      },
      {
        name: 'Place_GrandSenoraDesert',
        img: 'grandSenoraDesert',
        id: 3,
        active: false,
      },
      {
        name: 'Place_TheCayoPerico',
        img: 'thecayoperico',
        id: 4,
        active: false,
      },
      {
        name: 'Place_StabCity',
        img: 'stabcity',
        id: 5,
        active: true,
      },
      {
        name: 'Place_Galilee',
        img: 'galilee',
        id: 6,
        active: true,
      },
      {
        name: 'Place_RedwoodLights',
        img: 'redwoodlights',
        id: 7,
        active: true,
      },
    ],
    weaponsList: [
      {
        name: 'familyMenu_Handguns',
        img: 'handguns',
        id: 1
      },
      {
        name: 'familyMenu_SubmachineGuns',
        img: 'submachine',
        id: 2
      },
      {
        name: 'familyMenu_Shotguns',
        img: 'shotguns',
        id: 3
      },
      {
        name: 'familyMenu_AssaultRifles',
        img: 'assaultrifles',
        id: 4
      },
      {
        name: 'familyMenu_LightMachineGuns',
        img: 'lightmachineguns',
        id: 5
      },
    ],
    globalEvents: [
      {
        ID: 0,
        Date: 1600000000,
        Location: 1,
        WinnerFamilyName: '',
        IsFinished: false,
        Description: 'Описание',
        Rewards: 'Награды',
        KillLog: [
          {
            Name: 'Slade_Wilson',
            UUID: 1,
            Kills: 20,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Ivan_Ivanovich',
            UUID: 2,
            Kills: 10,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Petr_Petrovich',
            UUID: 3,
            Kills: 12,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Sidor_Sidorovich',
            UUID: 4,
            Kills: 15,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
        ],
      },
      {
        ID: 0,
        Date: 1600000000,
        Location: 1,
        WinnerFamilyName: '',
        IsFinished: false,
        Description: 'Описание',
        Rewards: 'Награды',
        KillLog: [
          {
            Name: 'Slade_Wilson',
            UUID: 1,
            Kills: 20,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Ivan_Ivanovich',
            UUID: 2,
            Kills: 10,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Petr_Petrovich',
            UUID: 3,
            Kills: 12,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Sidor_Sidorovich',
            UUID: 4,
            Kills: 15,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
        ],
      },
      {
        ID: 0,
        Date: 1600000000,
        Location: 1,
        WinnerFamilyName: '',
        IsFinished: false,
        Description: 'Описание',
        Rewards: 'Награды',
        KillLog: [
          {
            Name: 'Slade_Wilson',
            UUID: 1,
            Kills: 20,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Ivan_Ivanovich',
            UUID: 2,
            Kills: 10,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Petr_Petrovich',
            UUID: 3,
            Kills: 12,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Sidor_Sidorovich',
            UUID: 4,
            Kills: 15,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
        ],
      },
      {
        ID: 0,
        Date: 1600000000,
        Location: 1,
        WinnerFamilyName: 'SladeGang',
        IsFinished: true,
        Description: 'Описание',
        Rewards: 'Награды',
        KillLog: [
          {
            Name: 'Slade_Wilson',
            UUID: 1,
            Kills: 20,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Ivan_Ivanovich',
            UUID: 2,
            Kills: 10,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Petr_Petrovich',
            UUID: 3,
            Kills: 12,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
          {
            Name: 'Sidor_Sidorovich',
            UUID: 4,
            Kills: 15,
            FamilyId: 1,
            FamilyName: 'SladeGang'
          },
        ],
      },
    ],
    currentEvent: {
      ID: 0,
      Date: 1600000000,
      Location: 1,
      WinnerFamilyName: '',
      IsFinished: false,
      Description: 'Описание',
      Rewards: 'Награды',
      KillLog: [
        {
          Name: 'Slade_Wilson',
          UUID: 529132,
          Kills: 1000,
          FamilyId: 1,
          FamilyName: 'SladeGang'
        },
      ],
    }
  },

  mutations: {
    toggleNavElem(state, value) {
      state.showNavElem = null
      setTimeout(() => {
        state.showNavElem = value
      }, 300);
    },
    setBusinessList(state, data) {
      state.businessList = data;
    },
    updateBusinessFamilyPatronage(state, data) {
      let index = state.businessList.findIndex(item => item.id == data.biz);
      if (index > -1) {
        state.businessList[index].famOwner = data.familyName;
      }
    },
    setNotificationMessage(state, data) {
      state.notificationMessage = data.message;
      state.notificationResult = data.result;
      state.notification = true;
    },
    toggleBattleResults(state, value) {
      if (value) {
        state.prevNavElem = state.showNavElem
        state.showNavElem = 'BattleResults'
        state.currentEvent = value
        setTimeout(() => {
          state.showBattleResults = true
        }, 300);
      }
      else {
        state.showNavElem = state.prevNavElem
        state.showBattleResults = false
      }
    },

    
    loadGlobalEvents(state, data) {
      state.globalEvents = data;
    },
    updateGlobalEvents(state, data) {
      let index = state.globalEvents.findIndex(item => item.ID == data.ID);
      if (index > -1) {
        state.globalEvents[index] = data;
      }
      else {
        state.globalEvents.push(data);
      }
    }
  },
  getters: {
    setNotificationValue: state => value => {
      state.notification = value;
    }
  }
}
