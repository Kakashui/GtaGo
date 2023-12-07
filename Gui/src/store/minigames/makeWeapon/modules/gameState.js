export default {
  namespaced: true,
  state: {
    currentGun: 1, // id текущего автомата
    currentDetail: 4, // id текущей детали
    gameProfit: 0, // общая сумма денег за игру
    gameProgress: [0, 0, 0, 0], // результаты игры
    gameProgressNum: 0, // результат сборки автомата в %
    gameBonus: 0, // бонус за скорость
    gameFinish: 0, // флаг для скрытия элементов и отображения анимации для собранного автомата
    dateTime: 0, // время последнего действия для расчёта коэффициента,
    isGameBegine: false,
    waitNewGame: false,
    bonusForTime: false,
    compleeted: 0,
    floodProtection: 0,
    waitServer: false
  },
  getters: {},
  mutations: {
    resetGame(state, newGun) {
      state.currentGun = newGun;
      state.gameBonus = 0;
      state.compleeted = 0;
    },
    updateGameBonus(state) {
      state.gameBonus++;
    },
    setGameProgress(state, payload) {
      state.gameProgress.splice(payload.index, 1, 1);
      state.gameProgressNum = payload.progressNum;
    },
    zeroingGameProgress(state, progress) {
      state.gameProgress = progress;
      state.gameProgressNum = 0;
    },
    setCurrentDetail(state, detailNum) {
      state.isGameBegine = true;
      state.currentDetail = detailNum;
    },
    refreshDateTime(state) {
      state.dateTime = Date.now() + 2000;
    },
    setGameBegine(state, data){
      state.isGameBegine = data;
    },
    setWaitNewGame(state, val){      
      state.waitServer = false;
      state.waitNewGame = val;
    },
    setBonusForTime(state, val){
      state.bonusForTime = val;
    },
    stopGame(state){      
      state.gameBonus = 0;
      state.compleeted = 0;
      state.isGameBegine = false;
      state.setWaitNewGame = false;
    },
    updateFloodProtection(state){
      state.floodProtection = Date.now() + 800;
    },
    updateResult(state, result){
      state.isGameBegine = false;
      if(result) state.compleeted++;
    }
  },
  actions: {
    newGame(ctx) {
      ctx.commit("setWaitNewGame", false);
      ctx.commit("gameMakeWeapon/pageState/setFinalDetail", false, {root: true});
      ctx.commit("gameMakeWeapon/pageState/setAnimationActive", false, {root: true});
      let min = Math.ceil(1);
      let max = Math.floor(9);
      let progress;
      let currentDetail;
      let newGun;
      let userLevel = ctx.rootGetters["gameMakeWeapon/userData/userLevel"];
      do {
        newGun = Math.floor(Math.random() * (max - min)) + min;
      } while (newGun == ctx.state.currentGun);

      if (userLevel == 1) {
        currentDetail = 3;
        progress = [1, 1, 0, 0];
      } else if (userLevel == 2) {
        currentDetail = 2;
        progress = [1, 0, 0, 0];
      } else if (userLevel == 3) {
        currentDetail = 1;
        progress = [0, 0, 0, 0]
      }
      ctx.commit('zeroingGameProgress', progress);
      ctx.commit('refreshDateTime');
      ctx.commit('setCurrentDetail', currentDetail);
      ctx.commit('resetGame', newGun);
      ctx.commit("setGameBegine", true);
    },
    updateGameBonus(ctx) {
      if(!ctx.state.setBonusForTime)
        ctx.commit("setBonusForTime", true);
      ctx.commit('updateGameBonus');
    },
    updateGameProgress(ctx, index) {
      let userLevel = ctx.rootGetters["gameMakeWeapon/userData/userLevel"];
      let progressNum = ctx.state.gameProgressNum;
      progressNum = progressNum + 100 / (userLevel + 1);
      ctx.commit('setGameProgress', {index, progressNum});
    },
    updateCurrentDetail(ctx) {
      let nextDetail = ctx.state.currentDetail + 1;
      ctx.commit('refreshDateTime');
      if (nextDetail > 4) {
        if( ctx.state.waitServer) return;
        ctx.state.waitServer = true;
        ctx.commit("gameMakeWeapon/pageState/setFinalDetail", true, {root: true});
        ctx.commit("gameMakeWeapon/pageState/setAnimationActive", true, {root: true});
        if(ctx.state.compleeted === 0){
          window.mp.triggerServer("mg:makeweapon:result", ctx.state.currentGun, 0, 0);
          ctx.commit("sounds/play", {"name": "makeWeaponlose", "volume": .03}, {root: true});         
        }else {
          window.mp.triggerServer("mg:makeweapon:result", ctx.state.currentGun, ctx.state.compleeted, ctx.state.gameBonus);
          ctx.commit("sounds/play", {"name": "makeWeaponWin", "volume": .03}, {root: true});         
        }
        if(process.env.NODE_ENV == 'development')
          ctx.commit("setWaitNewGame", true);
      } else {
        ctx.commit('setCurrentDetail', nextDetail);
      }
    }
  },
};
