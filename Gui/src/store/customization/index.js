import categories from './categories'

export default {
    namespaced: true,
    state: {
        categories,
        firstName: "",
        lastName: "",
        gender: true,
        firstCreate: true,
        age: {
            tag: "ageing",
            min: 18,
            max: 75,
            step: 1,
            value:18
        },
        similar: {
            tag: "similar",
            min: 0,
            max: 1,
            step: .05,
            value: .5
        },
        skin: {
            tag: "skin",
            min: 0,
            max: 1,
            step: .05,
            value: .5
        }
    },
    mutations: {
        updateFirstName(state, value){
            state.firstName = value;
        },
        updateLastName(state, value){
            state.lastName = value;
        },
        updateAge(state, value){
            state.age = value;
        },
        updateSimilar(state, value){
            state.similar = value;
            window.mp.trigger("customization:update", "similar", state.similar);
        },
        updateSkin(state, value){
            state.skin = value;
            window.mp.trigger("customization:update", "skin", state.skin);
        },
        changeGender(state, gender){
            if(state.gender === gender) return;
            window.mp.trigger("customization:gender:change", gender);
            state.gender = gender;
        },
        resetGender(state){
            state.gender = true;
        },
        isNotFirstCreate(state, notFirstCreate){
            state.firstCreate = !notFirstCreate
        }
    },
    getters:{
        categories(state){
            return state.categories.filter(c=> state.firstCreate ? true : c.tab !== "Clothes");
        }
    },
    actions: {
    },
    modules: {
    }
}