export default {
  namespaced: true,

  state: {
    currentEventId: 0,
    events: [
      {
        eventId: 0,
        name: "Каскадерские гонки",
        locationName: "Парк на окраине",
        players: 200,
        maxPlayers: 400,
        startTime: '19:00',//время старта
        description: "asdfasda",
        bestTime: '07:32',//лучшее время за день в сек
        registered: true, 
        countStartTime: '20 мин',
        isStarted: false
      },
      {
        eventId: 1,
        name: "Каскадерские гонки",
        locationName: "Парк на окраине",
        players: 200,
        maxPlayers: 400,
        startTime: '19:00',//время старта
        description: "Равным образом начало  работы по формированию позиции представляет собой интересный эксперимент проверки существенных финансовых и административных условий. Повседневная практика показывает, что реализация намеченных плановых заданий требуют определения и уточнения системы обучения кадров, соответствует насущным потребностям. отношении поставленных задач.",
        registered: false, 
        countStartTime: '20 мин',
        isStarted: false
      },
      {
        eventId: 2,
        name: "Каскадерские гонки",
        locationName: "Парк на окраине",
        players: 200,
        maxPlayers: 400,
        startTime: '19:00',//время старта
        description: "Равным образом начало повседневной  позиции представляет собой интересный эксперимент проверки  финансовых и административных условий. Повседневная практика показывает, что реализация намеченных плановых заданий требуют определения и уточнения системы обучения кадров, соответствует насущным потребностям. отношении поставленных задач.",
        bestTime: '07:32',//лучшее время за день в сек
        registered: false, 
        countStartTime: '20 мин',
        isStarted: false
      },
    ],
    notify: false,
  },

  mutations: {
    setEvents(state, payload) {
      state.events = payload
    },

    setCurrentEventId(state, id) {
      state.currentEventId = id
    },
    
    setCurrentEventsState(state, payload) {
      console.log('payload' + JSON.stringify(payload))
      payload.forEach(e => {
        const element = state.events.find(s => s.eventId === e.eventId)
        if (element) {
          element.players = e.players
          element.countStartTime = e.countStartTime
          element.bestTime = e.bestTime
          element.startTime = e.startTime
        }
      })
    },
    
    setPlayersCount(state, payload){
      const element = state.events.find(s => s.eventId === payload.id)
      element.players = payload.players
    },
    
    setRegistered(state, { id, registered }){
      console.log('registered' + JSON.stringify(id))
      const element = state.events.find(s => s.eventId === id)
      if (element) {
        element.registered = registered 
      }
    }
  }
}