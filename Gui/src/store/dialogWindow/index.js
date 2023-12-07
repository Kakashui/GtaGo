export default {
  namespaced: true,
  state: {
    dialogData: {
      name: "john down",
      desc: "фермер",
      text: "Доброго дня, сынок! Заскакивай ко мне на рандеву. Диалог и разговор — принципиально разные вещи, пусть и выглядят похожими.",
      answers: [
        {
          id: 0,
          text: "Мне нужен паспорт"
        },
        {
          id: 1,
          text: "где можно заработать денег?"
        },
        {
          id: 2,
          text: "начальные квесты"
        },
        {
          id: 3,
          text: "до встречи"
        }
      ]
    }
  },
  mutations: {
    setData(state, data) {
      state.dialogData = data;
    }
  }
}