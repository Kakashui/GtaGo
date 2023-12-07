export default {
    namespaced: true,
    state: {
        val: 36,
        off: 2
    },
    mutations: {
        setData(state, {val, off}){
            state.val = val;
            state.off = off;
        }
    },
    actions: {
    },
    modules: {
    }
}
  