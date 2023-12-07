export default {
    namespaced: true,
    state: {
      key: 'lspd',
      firstName: 'Sandrexa',
      lastName: 'Domina',
      id: 123456,
      post: 'Main'
    },
    mutations: {
      setData:function(state, data){
        state.key = data.key;
        state.firstName = data.firstName;
        state.lastName = data.lastName;
        state.id = data.id;
        state.post = data.post;
      }
    },
    actions: {
    },
    modules: {
    }
}
  