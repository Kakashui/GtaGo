export default {
    namespaced: true,

    state: {
        number: '999999',
        firstname: 'Elon',
        lastname: 'Musk',
        date: '9999.99.99',
        gender: 'Mele',
        member: '-',
        work: 'SpaceX',
        partner: 'Lady'
    },

    mutations: {
        setPassportData: function(state, json) {
            var data = JSON.parse(json)
            state.number = data[0]
            state.firstname = data[1]
            state.lastname = data[2]
            state.date = data[3]
            state.gender = data[4]
            state.member = data[5]
            state.work = data[6]
            state.partner = data[7]
        }
    }
}