const waeaponConfigs = require('./weaponConfigs'); 

module.exports = class WeaponData{
    constructor(data) {
        const config = waeaponConfigs[data[0]];
        this.weaponHash = config.Hash;
        this.ammoType = config.AmmoType;
        this.components = [];
        this.damage = config.Damage || 15;
        for (let index = 1; index < 7; index++) {
            const component = config.Components[index][data[index]] || 0;
            this.components.push(component)
        }
        this.ammo = data[7] || 0;
    }
}