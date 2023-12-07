export default {
  namespaced: true,
  state: {
    items: [
      {
        name: 'HuntingShop_8',
        key: 'huntingrifle',
        price: 2200,
        btnText: 'HuntingShop_3'
      },
      {
        name: 'HuntingShop_7',
        key: 'huntingAmmo',
        price: 13,
        amount: 30,
        btnText: 'HuntingShop_3'
      },
      {
        name: 'HuntingShop_6',
        key: 'huntingKnife',
        price: 2200,
        btnText: 'HuntingShop_3'
      },
      {
        name: 'HuntingShop_5',
        key: 'skin',
        price: 2,
        amount: 2,
        btnText: 'HuntingShop_4'
      }
    ]
  },

  mutations: {
    setData: function(state, data) {
      const newItems = [];

      data.forEach(element => {
        const item = state.items.find(el => el.key === element.key);

        const newItem = {
          name: item.name,
          btnText: item.btnText,
          ...element
        };

        newItems.push(newItem);
      });

      state.items = newItems;
    }
  }
}