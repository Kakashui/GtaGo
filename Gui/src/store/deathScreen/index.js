import hints from '../../configs/deathScreen'
export default {
    namespaced: true,
    state: {
        active: false,
        time: 0,
        startTime: 0,
        interval: undefined,
        medicsBtn: false,
        deathBtn: false,
        medics: 0,
        hints
    },
    mutations: {       
        setTime(state, time){
            state.time = time;
            state.startTime = time;
            state.active = true;
            if(!state.interval) {                
                state.interval = setInterval(()=>{
                    if(state.time < 1) {
                        state.interval = clearInterval(state.interval);
                        state.medicsBtn = false;
                        state.deathBtn = true;
                    }
                    else state.time--;
                },1000)
            }
        },
        showBtns(state, medics){
            state.deathBtn = false;
            state.medicsBtn = (medics >= 0);
            state.medics = medics;
            state.active = true;
        },
        updateTime(state, time){
            state.startTime += time * 60;
            state.time += time * 60;
            state.medicsBtn = false;
        },
        close(state){ 
            state.time = 0;
            state.startTime = 0;
            state.medics = 0;
            state.deathBtn = false;
            state.medicsBtn = false;
            state.active = false;
        }
    },
    actions: {
    },
    modules: {
    }
}
  