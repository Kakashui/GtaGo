export default {
  namespaced: true,

  state: {
    show: false,
    Body: {
      Header: 'Вы действительно хотите купить этот предмет? Вы действительно хотите купить этот предмет?',
      Buttons: [
        { Name: 'Подтвердить', Icon: 'confirm' },
        { Name: 'Отменить', Icon: 'cancel' },
        // { Name: 'В гараж', Icon: undefined }
      ]
    }
  },

  mutations: {
    setBody: function(state, payload) {
      state.Body = payload
    },

    setShow: function(state, payload) {
      state.show = payload
    }
  }
}
