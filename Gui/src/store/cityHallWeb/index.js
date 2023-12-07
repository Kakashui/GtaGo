import vote from './vote'
import letter from './letter'
import complaint from './complaint'
import recording from './recording'
import notary from './notary'
import jobs from './jobs'
import nameEdit from './nameEdit'
import licenses from './licenses'
import certificate from './certificate'
import donations from './donations'
import events from './events'
import news from './news'
import community from './community'

export default {
  namespaced: true,
  state: {
    isRegistered: true,
    name: 'Alexey Pupkin',
    day: 10,
    donationConsent: true,
    tax: 0,
    socialStatus: [
      {
        desc: 'Статус',
        values: [
          'Безработный',
          'В браке с Sandexa_Domina'
        ]
      },
      {
        desc: 'Судимость',
        values: [
          '25.02.2021 - 25.02.2021, убийство',
          '25.02.2021 - 25.02.2021, кража',
          '25.02.2020 - 25.02.2020, убийство',
          '25.02.2019 - 25.02.2019, убийство',
        ]
      },
      {
        desc: 'Информация о занятости',
        values: [
          '25.02.2021 - 25.02.2021, охотник',
          '25.02.2021 - 25.02.2021, дальнобойщик',
        ]
      },
      {
        desc: 'Лицензии',
        values: [
          'Категория A, 25.02.2021 - 25.02.2021',
          'Категория B, 25.02.2021 - 25.02.2021',
          'Категория C, 25.02.2021 - 25.02.2021',
          'Категория D, 25.02.2021 - 25.02.2021',
        ]
      },
    ],
    month: {
      number: 9,
      desc: 'Сентябрь'
    },
    year: 1993,
    cardId: 12345678,
    modalData: {
      show: false,
      title: 'Необходимый заголовок',
      desc: 'Какое-то описание'
    }
  },
  mutations: {
    setName: function(state, value){
      state.name = value;
    },
    setUuid: function(state, value){
      state.cardId = value;
    },
    setSocialStatus: function(state, data){
      state.socialStatus = data;
    },
    closeModal: function(state){
      state.modalData.show = false
      state.modalData.title = null
      state.modalData.desc = null
    },
    openModal: function(state, data){
      state.modalData.title = data.title
      state.modalData.desc = data.desc
      state.modalData.show = true
    }
  },
  actions: {
  },
  modules: {
    vote,
    letter,
    complaint,
    recording,
    notary,
    jobs,
    nameEdit,
    licenses,
    certificate,
    donations,
    events,
    news,
    community
  }
}
  