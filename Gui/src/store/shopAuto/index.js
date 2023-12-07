export default {
    namespaced: true,
    state: {        
        vip: false,
        models: ["16challenger","teslaM3","teslaMX"],
        colors: [
            {name: "black", val: "rgb(0, 0, 0)"},
            {name: "white", val: "rgb(225, 225, 225)"},
            {name: "red", val: "rgb(230, 0, 0)"},
            {name: "orange", val: "rgb(255, 115, 0)"},
            {name: "yellow", val: "rgb(240, 240, 0)"},
            {name: "green", val: "rgb(0, 230, 0)"},
            {name: "lightBlue", val: "rgb(0, 205, 255)"},
            {name: "blue", val: "rgb(0, 0, 230)"},
            {name: "purple", val: "rgb(190, 60, 165)"}
        ],
        prices: [19,199,1999],
        displayNames: ["Challenger","Tesla Model 3","Tesla Model X"]
    },
    mutations: {
        setData(state, {vip,models,autoColors,prices,displayNames}){
            state.vip = vip;
            state.models = JSON.parse(models);
            state.colors = autoColors;
            state.prices = JSON.parse(prices);
            state.displayNames = JSON.parse(displayNames);
        }
    },
    actions: {
    },
    modules: {
    }
}
  