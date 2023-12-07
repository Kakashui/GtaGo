import Element from './Element'
import itemConfigs from '../../../configs/inventory/configs'
import lvlConfig from '../../../configs/gameMetalPlant/levels'

export default {
  namespaced: true,
  state: {
    currantState: "Inventory",
    preparation: false,
    gameBegine: false,
    furnaceCoolingBegine: false,    
    elements: {},
    antiFlood: 0,
    validElementIcon: new Element("valid", 1, 0),
    invalidElementIcon: new Element("invalid", 1, 0),
    itemConfigs,
    oreTime:{
      245: 100 / 2 / 20, //100 / 2 / sek; - becouse interval = 500ms
      246: 100 / 2 / 30,
      247: 100 / 2 / 45
    },
    gameResult: {
      state: 0, //0 - furnace cooled 1 - loose (invalid elements) 2 - win 3 - win with diamond
      metals: []
    },
    game:{
      valid: 0,
      invalid: 0,
      condition: 0
    },
    settings:{
        columnsCount: 3,
        stepOnRow: 20,
        minSpeed: 1,
        maxSpeed: 3,
        bodyHeight: 0,
        tempElementValue: 30,
        baseElements: 30,
        addElementsOnLevel: 15,
        hornStep: 10
    },
    furnace: {
      ore: null,
      fuel: null,
      temperature: 0
    },
    inventory:{
      ore: {256: 2, 257: 3, 255: 4},
      fuel: {245: 3, 246: 2, 247: 1}
    },
    playerExp: 1
  },
  mutations: {
    setData(state, {ore, fuel, exp}){
      state.gameResult = {state:0, metals: []};
      state.elements = {}
      state.currantState = "Inventory";
      state.preparation = false;
      state.gameBegine = false;
      state.furnaceCoolingBegine = false;
      state.furnace.ore = null;
      state.furnace.fuel = null;
      state.furnace.temperature = 0;
      state.furnace.temperature = 0;
      state.game = {valid: 0,invalid: 0,condition: 0};
      state.inventory = {ore, fuel};
      state.playerExp = exp;

    },
    loadOreToFurnace(state, ore){
      state.furnace.ore = ore;
    },
    loadFuelToFurnace(state, fuel){
      state.furnace.fuel = fuel;
    },
    updateFuranceTemperature(state, temp){
      state.furnace.temperature = Math.min(temp, 100);
    },
    updatePreparationState(state, val){
      state.preparation = val;
    },
    updateGameBegineState(state, val){
      state.gameBegine = val;     
    },
    updateCoolingBegineState(state, val){
      state.furnaceCoolingBegine = val;
    },
    updateGameResult(state, gameResult){
      state.gameResult = gameResult
    },
    updateCurrentState(state, page){
      state.currantState = page
    },
    updatePlayerExp(state, exp){
      state.playerExp = exp
    },
    addValid(state){
      state.game.valid++;
    },
    addInvalid(state){
      state.game.invalid++;
    },
    setGameCondition(state, condition){
      state.game.condition = condition
    }
  },
  actions: {
    hornAction(ctx){
      if(ctx.state.antiFlood > Date.now()) return;
      ctx.state.antiFlood = Date.now() + 250;
      if(ctx.state.furnace.ore === null) {
        ctx.commit('notifyList/notify', { type: 1, position: 0, message: "mplant:action:error:ore", time: 1000 }, {root: true})
        return;
      }
      if(ctx.state.furnace.fuel === null) {
        ctx.commit('notifyList/notify', { type: 1, position: 0, message: "mplant:action:error:fuel", time: 1000 }, {root: true})
        return;
      }
      ctx.commit("sounds/play", { name: `metalPlant_meh`, volume: 0.1 }, {root: true});
      if(!ctx.state.preparation && !ctx.state.gameBegine && ctx.state.furnace.temperature + ctx.state.settings.hornStep >= 100){
        ctx.commit("updateCurrentState","Game");
        if(!ctx.state.gameBegine)
          ctx.commit('updatePreparationState', true)
        if(process.env.NODE_ENV == 'development')
          ctx.dispatch('updateGameBegineState', true)
        else
          window.mp.triggerServer("game:mplant:start", ctx.state.furnace.ore, ctx.state.furnace.fuel);
      }
      ctx.commit('updateFuranceTemperature', ctx.state.furnace.temperature + (ctx.state.gameBegine ? ctx.state.settings.hornStep : 40));
      if(!ctx.state.furnaceCoolingBegine)
        ctx.dispatch("furnaceCoolingStart");
    },
    startGame(ctx){
      ctx.commit('updatePreparationState', false)
    },
    gameOver(ctx, result){
      ctx.dispatch("updateGameBegineState", false);
      ctx.commit("updatePreparationState", false);
      ctx.commit("updateGameResult", {state: result, metals: []})
      ctx.commit("updateCurrentState","GameOver");
    },
    gameWin(ctx, validPercent){
      if(process.env.NODE_ENV == 'development'){
        const metals = [];
        const metalOnLvl = ctx.getters.playerLvl + 1;
        let perfect = 0;
        for (let index = 0; index < metalOnLvl; index++) {
            if(validPercent > 99){
              metals.push(true);
              perfect++;
            }
            else{
                validPercent -= 10;
                const isPerfect = Math.random() * 100 < validPercent;
                metals.push(isPerfect);
                if(isPerfect)
                  perfect++;
                // else
                //   common++;
            }
        }
        const diamond = perfect > 0 ? Math.random() * 100 < perfect * 2 : false;
        ctx.commit("updateGameResult", {state: diamond ? 3 : 2, metals})
      }else {
        window.mp.triggerServer("game:mplant:win", validPercent);
      }        

      ctx.commit("updateCurrentState","GameWin");      
      ctx.dispatch("updateGameBegineState", false);
      ctx.commit("updatePreparationState", false);
    },
    updateGameBegineState(ctx, val){
      ctx.commit("updateGameBegineState", val);
      if(val)
        ctx.commit("sounds/play", { name: `metalPlant_flame`, volume: 0.05, loop: true }, {root: true});
      else      
        ctx.commit("sounds/stop", null, {root: true});
    },
    furnaceCoolingStart(ctx){
      ctx.commit("updateCoolingBegineState", true);
      const interval = setInterval(()=>{        
        if(ctx.state.preparation) return;
        if(!ctx.state.furnaceCoolingBegine){
          clearInterval(interval);
          return;
        }
        const nextValue = ctx.state.furnace.temperature - ctx.state.oreTime[ctx.state.furnace.fuel];        
        if(nextValue < 5){          
          ctx.commit("updateFuranceTemperature", 0);
          clearInterval(interval);
          ctx.commit("updateCoolingBegineState", false);
          if(ctx.state.gameBegine)
            ctx.dispatch("gameOver", 0);
        }else 
          ctx.commit("updateFuranceTemperature", nextValue);
      }, 500)
    },
    tempElementAction(ctx, value){
      ctx.commit('updateFuranceTemperature', ctx.state.furnace.temperature + value);
    },
    createElements(ctx){
      ctx.commit('setGameCondition', ctx.state.settings.baseElements +  ctx.state.settings.addElementsOnLevel * ctx.getters.playerLvl);
      //ctx.state.elements = {};
      const elements = [];
      for (let index = 0; index < ctx.state.game.condition; index++) {
          elements.push(new Element("valid"));
          elements.push(new Element("invalid"));
      }
      for (let index = 0; index < Math.floor(ctx.state.game.condition / 100 * 6); index++) {
          elements.push(new Element("tempUp"));
          elements.push(new Element("tempDown"));
      }
      for (let i = elements.length - 1; i > 0; i--) {
        let j = Math.floor(Math.random() * (i + 1)); 
        [elements[i], elements[j]] = [elements[j], elements[i]];
      }
      let currentColumn = 0;
      let row = 0
      const columnWidth = Math.floor(95 / ctx.state.settings.columnsCount);

      const elObj = {};
      for (let index = 0; index < elements.length; index++) {
          const el = elements[index];
          if(currentColumn + 1 < ctx.state.settings.columnsCount){
              currentColumn++
          }else{
              currentColumn = 0;
              row++;
          }
          const x = Math.floor(columnWidth * Math.random() + columnWidth * currentColumn);
          const y = row * ctx.state.settings.stepOnRow * -1;
          el.initPositions(x, y);
          el.initSpeed(ctx.state.settings.minSpeed, ctx.state.settings.maxSpeed);
          elObj[index] = el;
      }
      ctx.state.elements = {...elObj}
    }
  },
  modules: {
  },
  getters:{
    furnaceTempearature(state){
      return state.furnace.temperature;
    },
    allValidElements(){
      const elements = [];
      for (let index = 0; index < 3; index++) {
        elements.push(new Element("valid", 1, index))        
      }
      return elements;
    },
    allInvalidElements(){
      const elements = [];
      for (let index = 0; index < 4; index++) {
        elements.push(new Element("invalid", 1, index))        
      }
      return elements;
    },
    tempUpElement(){
      return new Element("tempUp", 1)
    },
    tempDownElement(){
      return new Element("tempDown", 1)
    },
    playerLvl(state){
      let level = 1;
      for (let index = lvlConfig.length - 1; index >= 0; index--) {
        if(lvlConfig[index].exp < state.playerExp) {
          level = lvlConfig[index].level;
          break;
        }
      } 
      return level;
    } 
  }
}
  