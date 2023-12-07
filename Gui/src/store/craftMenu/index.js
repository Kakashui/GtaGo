import config from './config'

export default {
    namespaced: true,
    state: {
        materials: 0,
        items: [],
        merry:["SMG","PistolAmmo","ShotgunsAmmo","SMGAmmo","RiflesAmmo"],
        lost:["DoubleBarrelShotgun", "CompactRifle","PistolAmmo","ShotgunsAmmo","SMGAmmo","RiflesAmmo"],
        gang:["Pistol","SNSPistol","DoubleBarrelShotgun","SawnOffShotgun","MicroSMG","SMGMk2", "BodyArmor","PistolAmmo","ShotgunsAmmo","SMGAmmo"],
        mafia:["Pistol","Pistol50","Revolver","VintagePistol","PumpShotgun", "MiniSMG", "AssaultRifle","PistolAmmo","ShotgunsAmmo","SMGAmmo","RiflesAmmo"],
        config
    },
    mutations: {
        setData(state, {frac, mats}){
            state.materials = mats;
            state.items = [];
            let data = [];
            if(frac > 0 && frac < 6)
                data = state.gang;
            else if(frac > 9 && frac < 14)
                data = state.mafia;
            else if (frac === 16)
                data = state.lost;
            else if (frac === 17)
                data = state.merry;
            data.forEach(item => {
                if(state.config[item])state.items.push(state.config[item]);
            });
        },
        updateMats(state, count){
            state.materials = count;
        }
    },
    actions: {
    },
    modules: {
    }
}
  