export default {
    namespaced: true,
    state: {
        personValue: '',
        personResult:{
            fName: {tittle:'cef_219', val:''},
            lName: {tittle:'cef_220', val:''},
            passport: {tittle:'cef_221', val:''},
            gender: {tittle:'cef_222', val:''},
            lvl: {tittle:'cef_223', val:''},
            lic: {tittle:'cef_224', val:''}
        },
        carValue: '',
        carResult:{
            model: {tittle:'cef_213', val:''},
            owner: {tittle:'cef_214', val:''}
        },
        list:[]
    },
    mutations: {
        setPersonValue(state, val){
            state.personValue = val;
        },
        setCarValue(state, val){
            state.carValue = val;
        },
        clearPerson(state){
            state.personResult.fName.val = '';
            state.personResult.lName.val = '';
            state.personResult.passport.val = '';
            state.personResult.gender.val = '';
            state.personResult.lvl.val = '';
            state.personResult.lic.val = '';
            state.personValue = '';
        },
        setPerson(state, data){
            state.personResult.fName.val = data[0];
            state.personResult.lName.val = data[1];
            state.personResult.passport.val = data[2];
            state.personResult.gender.val = data[3];
            state.personResult.lvl.val = data[4];
            state.personResult.lic.val = data[5];
        },
        setCar(state, data){
            state.carResult.model.val = data[0];
            state.carResult.owner.val = data[1];
        },
        setWantedList(state,data){
            state.list = data;
        },
        clearCar(state){
            state.carResult.model.val = '';
            state.carResult.owner.val = '';
            state.carValue = '';
        }
    },
    actions: {
    },
    modules: {
    }
}
  