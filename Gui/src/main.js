import Vue from 'vue'
import App from './App.vue'
import store from './store'

import VueVirtualScroller from 'vue-virtual-scroller'
import 'vue-virtual-scroller/dist/vue-virtual-scroller.css'

import Vuebar from 'vuebar'
import Clipboard from 'v-clipboard'

Vue.use(Vuebar)
Vue.use(Clipboard)
Vue.use(VueVirtualScroller)

Vue.config.productionTip = false;

if(window.mp == undefined){
  window.mp = {
    trigger(...args){
      window.console.log(`Event name: ${args[0]}`)
      let params = "";
      for (let index = 1; index < args.length; index++) {
        params += `${args[index]},`;        
      }
      window.console.log(`Params: ${params}`);
    },
    invoke(...args){
      window.console.log(`Event name: ${args[0]}`)
      let params = "";
      for (let index = 1; index < args.length; index++) {
        params += `${args[index]},`;        
      }
      window.console.log(`Params: ${params}`);
    }
  }
}

window.mp.triggerServer = (...args)=>{
  window.mp.trigger('efwd', ...args);
};

const antiFloodTimeMs = 200;

const lastEventSendingTime = {};

/**
 * .triggerServer, with antiflood
 */
window.mp.__triggerServer = (...args)=>{
  const eventName = args[0];
  
  if (lastEventSendingTime[eventName] 
    && new Date().getTime() - lastEventSendingTime[eventName] < antiFloodTimeMs) {
    return;
  }

  lastEventSendingTime[eventName] = new Date().getTime();
  window.mp.triggerServer(...args);
};

Object.defineProperty(Vue.prototype, "$bus", {
  get: function() {
    return this.$root.bus;
  }
});

new Vue({
  data: {
    bus: new Vue({})
  },
  store,
  render: h => h(App)
}).$mount('#app')

if(window.mp.events)
{
	let api = {"chat:push": window.chatAPI.push, "chat:clear": window.chatAPI.clear, "chat:activate": window.chatAPI.activate, "chat:show": window.chatAPI.show}; 

	for(let fn in api)
	{
		window.mp.events.add(fn, api[fn]);
	}
}