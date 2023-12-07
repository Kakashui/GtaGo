import battlePage from './modules/battle-page';
import controlPage from './modules/control-page';
import eventsPage from './modules/events-page';
import membersPage from './modules/members-page';
import propertyPage from './modules/property-page';
import ratingPage from './modules/rating-page';

export default {
  namespaced: true,
  state: {
    navData: {
      show: true
    },
    currentPage: 'InfoPage',
    accountType: 'Premium',
    isLeader: true,
    currentRankId: 1,
    currentMemberId: 2,
    inFamily: true,
    infoPage: {
      nation: 'Русские',
      income: 1250,
      members: 272,
      bankBalance: 9999999,
      houseType: 'Premium',
      houseAdress: 'Квартира, д. 23.',
      biography: 'Наша семья начала свое развитие в 2020 году. Мы славимся грузоперевозками и завоеваниями бизнесов. В этом же году у нас появился первый дом и 3 автомобиля. Самым прибыльным бизнесом нашей семьи является казино. Соответственно цель нашей семьи - защищать этот бизнес от нападений всеми силами.',
      onlineMembers: 43,
      familyRank: 20353,
      familyId: 20,
      businessControl: 25,
      tabooList: [
        {
          text: 'строго запрещается воровать деньги'
        },
        {
          text: 'строго запрещается убивать членов семьи и очень длинное табу '
        },
        {
          text: 'строго запрещается убивать членов семьи '
        }
      ],
      rulesList: [
        {
          text: 'всегда докладывать о происшествиях'
        },
        {
          text: 'носить одежду семьи'
        },
        {
          text: 'пополнять ежедневно общий банк'
        }
      ],
    },
    membersOnMap: true,
  },

  mutations: {
    setCurrentMemberId: function(state, value) {
      state.currentMemberId = value
    },
    hideBattlePage: function(state) {
      state.battlePageState = false
    },
    setCurrentPage: function(state, value) {
      state.currentPage = value;
    },
    toggleNav: function(state, value) {
      state.navData.show = value;
    },
    setInfoPage: function(state, data) {
      state.infoPage = data;
      state.infoPage.tabooList = state.infoPage.tabooList.map(item => ({text: item}));
      state.infoPage.rulesList = state.infoPage.rulesList.map(item => ({text: item}));
    },
    setRank: function(state, value) {
      state.currentRankId = value;
      state.isLeader = value == 0;
    },
    setNation: function(state, value) {
      state.infoPage.nation = value;
    },
    setBiography: function(state, value) {
      state.infoPage.biography = value;
    },
    setRules: function(state, value) {
      state.infoPage.tabooList = value.taboo.map(item => ({text: item}));
      state.infoPage.rulesList = value.rules.map(item => ({text: item}));
    },
    setMembersOnMap: function(state, value) {
      state.membersOnMap = value;
    },
    setBankBalance: function(state, value) {
      state.infoPage.bankBalance = value;
    },
    setInFamily: function(state, value) {
      state.inFamily = value;
    }
    
  },
  modules: {
    battlePage,
    controlPage,
    eventsPage,
    membersPage,
    propertyPage,
    ratingPage
  }
}
