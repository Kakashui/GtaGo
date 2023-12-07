export default {
  namespaced: true,

  state: {
    clothesList: [
      {
        name: 'Torsos',
        type: 'clothes',
        key: 3,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Legs',
        type: 'clothes',
        key: 4,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Bags',
        type: 'clothes',
        key: 5,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Shoes',
        type: 'clothes',
        key: 6,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Accessories',
        type: 'clothes',
        key: 7,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Undershirts',
        type: 'clothes',
        key: 8,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Decals',
        type: 'clothes',
        key: 10,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Top',
        type: 'clothes',
        key: 11,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Hats',
        type: 'props',
        key: 0,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Glasses',
        type: 'props',
        key: 1,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Ears',
        type: 'props',
        key: 2,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Wathes',
        type: 'props',
        key: 6,
        drawable: 15,
        texture: 0,
      },
      {
        name: 'Bracelets',
        type: 'props',
        key: 7,
        drawable: 15,
        texture: 0,
      },
    ],
    armorItem: {
      viewArmor: true,
      type: 0,
      armType: 0,
      drawable: 0,
      texture: 0,
    }
  },

  mutations: {
    setData(state, data) {
      state.clothesList.forEach(cloth => {
        if (cloth.type == 'clothes') {
          cloth.drawable = data[cloth.key];
        }
        else {
          cloth.drawable = -1;
        }
        cloth.texture = 0;
      });
      state.armorItem.viewArmor = false;
      state.armorItem.type = 0;
      state.armorItem.armType = 12;
      state.armorItem.drawable = 504;
      state.armorItem.texture = 0;
    },
    setDrawable(state, data) {
      const index = state.clothesList.findIndex(item => item.key == data.key && item.type == data.type);
      if (index >= 0) {
        state.clothesList[index].drawable = data.drawable;
        window.mp.trigger('costumeMenu:setClothes', state.clothesList[index].type, state.clothesList[index].key, state.clothesList[index].drawable, state.clothesList[index].texture);
      }
    },
    setTexture(state, data) {
      const index = state.clothesList.findIndex(item => item.key == data.key && item.type == data.type);
      if (index >= 0) {
        state.clothesList[index].texture = data.texture;
        window.mp.trigger('costumeMenu:setClothes', state.clothesList[index].type, state.clothesList[index].key, state.clothesList[index].drawable, state.clothesList[index].texture);
      }
    },
    setArmType(state, value) {
      state.armorItem.armType = value
      window.mp.trigger('costumeMenu:setArmor', state.armorItem.armType, 0, state.armorItem.viewArmor);
    },
    setArmor(state, data) {
      state.armorItem.drawable = data.drawable;
      state.armorItem.texture = data.texture;
      window.mp.trigger('costumeMenu:setArmor', state.armorItem.drawable, state.armorItem.texture, state.armorItem.viewArmor);
    },
    setTypeArmor(state, value) {
      state.armorItem.type = value;
      if (state.armorItem.type == 0) {
        window.mp.trigger('costumeMenu:setArmor', state.armorItem.armType, 0, state.armorItem.viewArmor);
      }
      else {
        window.mp.trigger('costumeMenu:setArmor', state.armorItem.drawable, state.armorItem.texture, state.armorItem.viewArmor);
      }
    },
    setViewArmor(state, value) {
      state.armorItem.viewArmor = value;
      if (state.armorItem.type == 0) {
        window.mp.trigger('costumeMenu:setArmor', state.armorItem.armType, 0, state.armorItem.viewArmor);
      }
      else {
        window.mp.trigger('costumeMenu:setArmor', state.armorItem.drawable, state.armorItem.texture, state.armorItem.viewArmor);
      }
    }
  }
}
