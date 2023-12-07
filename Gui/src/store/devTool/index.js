import animations from './animations'

export default {
    namespaced: true,
    state: {
        currentPage: -1,
        console:{
            history: [],
            execstring: '',
            messages: [],
            show: false
        },
        anim:{
            currentDict: 0,
            currentAnim: 0,
            flags: [
                false,false,false,false,false,false,false,false,
                false,false,false,false,false,false,false,false
            ],
            onServer: false,
            disCA: false,
            playerId: -1,
            start: 0
        },
        vehicleEdit:{
            Model: 283828823,
            ModelName: 'bugattigo',
            DisplayName: 'Bugatti Chiron',
            Slots: 40,
            MaxWeight: 100,
            FuelConsumption: 2,
            MaxFuel: 200
        },
        menus: [
            "Animations",
            "VehicleEdit"
        ],
        animations
    },
    mutations: {
        addConsoleMessage(state, msg){
            state.console.messages.push(msg);
        },
        consoleVisible(state, val){
            state.console.show = val;
        },
        setBoneCoord(state, data){
            state.boneCoord = data;
        },
        selectMenu(state, index){
            state.currentPage = index;
        },
        selectDict(state, index){
            state.anim.currentAnim = 0;
            state.anim.currentDict = index;
        },
        selectAnim(state, index){
            state.anim.currentAnim = index;
        },
        changeAnimFlag(state, val){
            state.anim.flag = val;
        },

        setVehicleEdit(state, data) {
            state.vehicleEdit = data;
        }
    },
    actions: {
    },
    modules: {
    }
}
  