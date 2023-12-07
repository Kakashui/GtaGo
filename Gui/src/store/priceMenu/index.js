export default {
  namespaced: true,

  state: {
    show: true,
    priceCategories: [
      { cat: "car", show: true},
    ],
    priceList: {
      "car::gtrr35":15000,
      "car::chirongo":150000,
      "car::ffgo":150000,
      "car::elegy":1500,
      "car::rsgo":15000,
      "car::goniva":150000,
      "car::gosian":150000,
      "car::bmw5":1500,
    }
  },

  mutations: {
    setPriceCategories (state, value) {
      state.priceCategories = [];
      value.forEach(category => {
        state.priceCategories.push({ cat: category, show: true})
      });
    },
    setPriceList (state, value) {
      state.priceList = value
    },
    updatePriceItem (state, value) {
      if (value)
        state.priceList[value.Name] = value.Price;
    },

    setCategoryShow(state, data) {
      const index = state.priceCategories.findIndex(item => item.cat == data.cat);
      if (index >= 0 )
        state.priceCategories.splice(index, 1, data);
    }
  }
}
