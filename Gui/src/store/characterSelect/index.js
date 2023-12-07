export default {
    namespaced: true,
    state: {
        coin: 99000,
        slotsCount: 1, //кол-во доступных слотов (ИМЕННО ПУСТЫХ, не учитывая созданных персонажей)
        slots:[
            {                
                name: "Elon_Musk2",
                gender: 1, // 1 - м, 0 - ж
                level: 7,
                frac: "SpaceX", 
                cash: 100398, 
                bank: 500, 
                stats: [
                    {
                        desc: 'Голод',
                        value: 40
                    },
                    {
                        desc: 'Сон',
                        value: 28
                    },
                    {
                        desc: 'Жажда',
                        value: 19
                    },
                    {
                        desc: 'Счастье',
                        value: 99
                    },
                ],
                ban:{
                    admin: "Admin name",
                    time: '2011-01-26 13:51:50Z',
                    banTime: '2011-01-21 13:51:50Z',
                    reason: 'Клонирование Илона Маска, придумай другой никнейм, биографию, проходи заново регистрацию.',
                }
            },
            {
                name: "Elena_Musk",
                gender: 0,
                level: 37,
                frac: "SpaceXX", 
                cash: 10000398, 
                bank: 50000000, 
                stats: [
                    {
                        desc: 'Голод',
                        value: 50
                    },
                    {
                        desc: 'Сон',
                        value: 79
                    },
                    {
                        desc: 'Жажда',
                        value: 81
                    },
                    {
                        desc: 'Счастье',
                        value: 99
                    },
                ],
                ban: null,
            },
            null,            
            // null,
            // null,

        ],
        newChar:{
            firstName: "",
            lastName: ""
        }
    },
    mutations: {
        setData(state, data){
            state.slots = data;
        },
        setCoins(state, coin){
            state.coin = coin;
        },
        setSlots(state, slots){
            state.slotsCount = slots;
        }
    },
    actions: {
    },
    modules: {
    }
}
  