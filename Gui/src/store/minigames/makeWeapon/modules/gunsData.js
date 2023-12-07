import guns from '../../../../configs/gameMakeWeapon/guns'

export default {
  namespaced: true,
  state: {
    guns,
    details: [
      { id: 1, name: 'mw:detail:1' },
      { id: 2, name: 'mw:detail:2' },
      { id: 3, name: 'mw:detail:3' },
      { id: 4, name: 'mw:detail:4' },
    ],
  },
  getters: {
    getGunByIndex: (state) => (id) => {
      return state.guns.find((gun) => gun.id === id);
    },
    getRandomGuns: () => (currentGun) => {
      let gunsArr = [1, 2, 3, 4, 5, 6, 7, 8];
      let shuffledGuns = gunsArr.sort(function() {
        return 0.5 - Math.random();
      });
      let selectedGuns = shuffledGuns.slice(0, 5);
      if (selectedGuns.toString().indexOf(currentGun, 0) == -1) {
        selectedGuns[1] = currentGun;
        selectedGuns = selectedGuns.sort(function() {
          return 0.5 - Math.random();
        });
      }
      return selectedGuns;
    },
  },
};
