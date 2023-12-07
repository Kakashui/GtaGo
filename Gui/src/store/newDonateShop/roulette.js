//import {getRouletteConfig} from '../../configs/newDonateShop'
export default {
    namespaced: true,
    state: {
        isRun: false,
        isStop: false,
        isWin: false,
        result: -1,
        sound: true
    },
    mutations: {        
        setResult(state, result){
            state.result = result;
        },
        reset(state){
            state.isRun = false;
            state.isStop = false;
            state.result = -1;
        },
        switchSound(state){
            state.sound = !state.sound;
        },
        win(state){
            state.isRun = false;
            state.isStop = false;
            state.isWin = true;
            state.result = -1;
        },
        start(state, result){            
            state.isRun = true;  
            state.isStop = false;
            state.isWin = false;
            state.result = result;
        },
        fastStop(state){            
            state.isRun = false;  
            state.isStop = false;
            state.isWin = true;
            state.result = -1;
        },
        stop(state){            
            state.isStop = true;
        }
    },
    actions: {
        requestStart(ctx, type){
            ctx.commit("newDonateShop/setBegineAction", true, {root: true});
            ctx.commit("reset");            
            window.mp.triggerServer("dshop:roulette:start", type)
            if(process.env.NODE_ENV == 'development')ctx.dispatch('responceStart', 26002);
        },
        responceStart(ctx, result){
            if(result < 0){
                window.setData('notifyList/notify', {type: 1, position:2, message: 'ndsh:roulette:error', time: 3000});
                ctx.commit("reset");
            }else{
                if(ctx.state.sound){
                    ctx.commit("sounds/play", { name: `choice`, volume: 0.5 }, {root: true});
                    ctx.commit("sounds/play", { name: `roulette_back`, volume: 0.03, loop: true }, {root: true});
                }
                ctx.commit("start", result);
            }
            //ctx.commit("newDonateShop/setBegineAction", false, {root: true});
        },
        requestFastStop(ctx, sell){
            ctx.commit("sounds/stop", null, {root: true});
            ctx.commit("newDonateShop/setBegineAction", false, {root: true});
            ctx.commit("newDonateShop/inventory/addItem", {id: ctx.state.result, sell}, {root: true});
            ctx.commit("fastStop");
            if(ctx.state.sound)
                ctx.commit("sounds/play", { name: `roulette_win`, volume: 0.1 }, {root: true});
        },
        requestStop(ctx){
            ctx.commit("stop");
        },
        requestWin(ctx, sell){            
            ctx.commit("newDonateShop/inventory/addItem", {id: ctx.state.result, sell}, {root: true});
            ctx.commit("win");
            ctx.commit("newDonateShop/setBegineAction", false, {root: true});
            ctx.commit("sounds/stop", null, {root: true});    
            if(ctx.state.sound)
                ctx.commit("sounds/play", { name: `roulette_win`, volume: 0.1 }, {root: true});
        }
    },
    modules: {
    },
    getters:{        
    }
}
  