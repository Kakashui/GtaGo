import traders from '../../configs/weedfarm/traders.js'
import maxOnStady from '../../configs/weedfarm/maxOnStady.js'

export default {
  namespaced: true,
  state: {
    currentPage: "ControlBoard",
    profit: 20000,
    ownerName: 'Sandrex_Domina',
    systems:{
      irrigation: 100,
      light: 10,
      dyring: 50,
      ventilation: 70,
    }, 
    stady:{
      onGrowing: 0, 
      onDrying: 0, 
      onPacking: 0, 
      onDelivery: 2,
    },
    onDeliveryJob: false,
    maxOnStady,
    traders,
    components: [2,3,5]
  },
  mutations: {
    updateOnDeliveryJob(state, onJob){
      state.onDeliveryJob = onJob;
    },
    updateFarmData(state, {components, onGrowing, onDrying, onPacking, onDelivery, irrigation, light, drying, ventilation}){
      state.components = components;
      state.stady = {onGrowing, onDrying, onPacking, onDelivery};
      state.systems = {irrigation, light, drying, ventilation};
      state.currentPage = components.length < Object.keys(state.traders).length ? "Components" : "ControlBoard";
    },
    updateSystems(state, {key, value}){
      state.systems[key] = value;
    },
    updateStady(state, {key, value}){
      state.stady[key] = value;
    },
    updateProfit(state, amount){
      state.profit = amount;
    },
    updateComponents(state, component){
      state.components.push(component);
    }
  },
  actions: {
  },
  modules: {
  }
}
  