export default {
    namespaced: true,
    state: {
        floors:[ 
            "1",
            "2",
            "3"
        ]
    },
    mutations: {
        set(state, data){
            state.floors = data;
        },
        reset(state){
            state.floors = [];
        }
    },
    actions: {
    },
    modules: {
    }
}
  