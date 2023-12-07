export default {
    namespaced: true,
    state: {
        enabled: false,        
        icons:["cam", "point", "scroll", "zoom"],
        move:{
            size: {
                x: 35,
                y: 35
            },
            // position: {
            //     left: '85%',
            //     top: '78%',
            // },
            showIcons:[true, true, false, true],
            values:[
                {//LEFT X
                    value: 120,
                    min: 30,
                    max: 210,
                    step: 5,
                    invert: true,
                    enabled: true,
                    callback: "camMoveAngleX"
                },
                {//LEFT Y
                    value: .5,
                    min: -.5,
                    max: 1.5,
                    step: .05,
                    invert: false,
                    enabled: true,
                    callback: "camMoveCamZ"
                },
                {//RIGHT X
                    value: 2,
                    min: 1,
                    max: 3,
                    step: .1,
                    invert: false,
                    enabled: true,
                    callback: "camSetDist"
                },
                {//RIGHT Y
                    value: 2,
                    min: 1,
                    max: 3,
                    step: .1,
                    invert: true,
                    enabled: true,
                    callback: "camSetDist"
                },
                { //WHEELE
                    value: 1,
                    min: .5,
                    max: 3,
                    step: .5,
                    invert: false,
                    enabled: true,
                    callback: "camSetDist"
                }
            ]
        }
    },
    mutations: {
        setSettings(state, settings){
            state.move = settings;
        },
        setEnebled(state, enabled){
            state.enabled = enabled;
        }
    },
    actions: {
    },
    modules: {
    }
}
  