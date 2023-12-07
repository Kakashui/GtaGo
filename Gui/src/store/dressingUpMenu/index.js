export default {
    namespaced: true,
  
    state: {
      show: true,
      dresses: [
        {id: 0, label: 'FBI'},
        {id: 1, label: 'Пожарные'},
        {id: 2, label: 'Полиция'},
        {id: 3, label: 'МЧС'},
        {id: 4, label: 'Минздрав'},
        {id: 5, label: 'Спецназ'}
      ]
    },
  
    mutations: {
      setDresses: function (state, value) {
        state.dresses = value
      },
      
    }
  }
