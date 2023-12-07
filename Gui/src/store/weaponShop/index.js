import WeaponModel from './WeaponModel'

export default {
    namespaced: true,
    state: {
        weapons: [],
        loadingWeapon: true,
    },
    mutations:{
        setWeapons(state, weapons){
            state.weapons = [];
            weapons.forEach(weapon => {
                state.weapons.push(new WeaponModel(weapon))
            });
            state.loadingWeapon = false;
        },
        loadWeapon(state, {id, slot, compIndex, model}){
            //state.loadingWeapon = true;
            window.mp.trigger("cef:wshop:update:model", id, slot, compIndex, model);
        },
        weaponLoaded(state){
            state.loadingWeapon = false;
        },
        close(state){
            state.weapons = [];
            state.loadWeapon = true;
        }
    }
}
  