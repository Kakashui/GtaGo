export default {
  namespaced: true,

  state: {
    cardsDistribution: ['SS14', 'SS12', 'SS9', 'HH14', 'back'],
    cardsDistributionCombinations: [],
    otherPlayers: [
      { id: 1, show: true, avatar: '/img/poker/avatar.png', name: 'FirstuseruseruserFirstuseruseruser user', currentRate: 374823, chipsCount: 3000000000, bank: 4200, state: 'Уравнял(а)', cards: ['back', 'SS14'], winCombinations: [], isWinner: false, cardsDisabled: false, isVoise: false},
      { id: 2, show: true, avatar: '/img/poker/avatar.png', name: 'SecondSecondFirstuseruseruser user', currentRate: 999, chipsCount: 40000000, bank: 4200, state: 'Думает', cards: ['back', 'SS14'], winCombinations: [], isWinner: false, cardsDisabled: false, isVoise: false},
      { id: 3, show: true, avatar: '/img/poker/avatar.png', name: 'Thrid useruseruser', currentRate: 100, chipsCount: 5000000000, bank: 4200, state: 'Уравнял(а)', cards: ['back', 'SS14'], winCombinations: [], isWinner: false, cardsDisabled: false, isVoise: false},
      { id: 4, show: true, avatar: 'https://img.redbull.com/images/c_crop,x_0,y_0,h_1822,w_3239/c_fill,w_890,h_593/q_auto,f_auto/redbullcom/2017/03/24/1331850781287_2/how-long-will-it-take-you-to-get-these-cars', name: 'Fourth useruseruser', currentRate: 50, chipsCount: 60000, bank: 4200, state: 'Уравнял(а)', cards: ['back', 'SS14'], winCombinations: [], isWinner: false, cardsDisabled: false, isVoise: false},
      { id: 5, show: true, avatar: '/img/poker/avatar.png', name: 'Fifth useruseruser', currentRate: 100, chipsCount: 7000000000, bank: 4200, state: 'Уравнял(а)', cards: ['DD10', 'SS14'], winCombinations: [], isWinner: false, cardsDisabled: false, isVoise: false}
    ],
    activePlayer: 0,
    currentPlayer: { id: 0, avatar: 'https://image.api.playstation.com/vulcan/img/rnd/202102/0422/TkQfZ0v0NF1p8YHovQPIYtr5.png', name: 'Current user', currentRate: 1, bank: 4200, chipsCount: 99990, state: 'Уравнял(а)', cards: ['SS14', 'SS12'], winCombinations: [], isWinner: true, cardsDisabled: false, isVoise: true },
    currentPlayerMove: true,
    currentPlayerMoveValues: {
      checkValue: 400,
      minValue: 200,
      stepValue: 100
    },
    bankAvailable: true,
    currentPlayerRaiseInput: 0,
    currentMoveTime: 60,
    ratesDone: false,
    allBets: [],
    allBank: [],
    minChipsValue: null,
    winCount: 0,
    autoCheck: false,
    grantAutoCheck: true,
    rangeSlider: {
      listValue: [],
      posHandle: 0,
      currentIndex: 0,
      lastValue: 0,
      sliderWrap: null,
      coodrLeftWrap: 0
    }
  },

  mutations: {
    setCurrentPlayerData: function (state, data) {
      state.currentPlayer.bank = data.currentPlayer.bank;
      state.currentPlayer.chipsCount = data.currentPlayer.chipsCount;
      state.currentPlayer.cards = data.currentPlayer.cards;
      state.currentPlayer.winCombinations = data.currentPlayer.winCombinations;
      state.currentPlayer.state = data.currentPlayer.state;
      state.currentPlayer.isWinner = data.currentPlayer.isWinner;
      state.currentPlayer.cardsDisabled = data.currentPlayer.cardsDisabled;
      state.currentPlayer.avatar = data.currentPlayer.avatar;
      
      state.bankAvailable = data.bankAvailable;
      state.activePlayer = data.activePlayer;
      state.grantAutoCheck = data.grantAutoCheck;

      
      state.cardsDistribution = data.cardsDistribution;
      state.cardsDistributionCombinations = data.cardsDistributionCombinations;
    },

    setOtherPlayerData(state, data) {
      const rightPlayer = state.otherPlayers.find(item => item.id === data.id)
      rightPlayer.name = data.name;
      rightPlayer.chipsCount = data.chipsCount;
      rightPlayer.state = data.state;
      rightPlayer.cards = data.cards;
      rightPlayer.winCombinations = data.winCombinations;
      rightPlayer.isWinner = data.isWinner;
      rightPlayer.avatar = data.avatar;
    },

    hideUser: function (state, { id, show }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.show = show
    },

    setOtherPlayerName(state, { id, name }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.name = name
    },

    setOtherPlayerBank(state, { id, bank }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.bank = bank
    },

    setOtherPlayerChipsCount(state, { id, value }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.chipsCount = value
    },

    setActivePlayer: function (state, value) {
      state.activePlayer = value
    },

    setOtherPlayerWinCombinations: function (state, { id, combinations }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.winCombinations = combinations
    },

    setCurrentPlayerWinCombinations: function (state, combinations) {
      state.currentPlayer.winCombinations = combinations
    },

    setCardsDistributionCombinations: function (state, combinations) {
      state.cardsDistributionCombinations = combinations
    },

    setOtherPlayerState: function (state, { id, value }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.state = value
    },

    setCurrentPlayerState: function (state, value) {
      state.currentPlayer.state = value
    },

    setUserCards: function (state, { id, cards }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.cards = cards
    },

    setCardsDistribution: function (state, cards) {
      state.cardsDistribution = cards
    },

    setCurrentPlayerMove: function (state, value) {

      if (value && value !== state.currentPlayerMove) {
        const audio = document.getElementById('turnSound')

        audio.volume = 0.6
        audio.play()
      }
      state.currentPlayerMove = value
    },

    setCurrentPlayerChipsCount: function (state, value) {
      state.currentPlayer.chipsCount = value
    },

    setbankAvailable: function (state, value) {
      state.bankAvailable = value
    },

    setCurrentMoveTime: function (state, value) {
      state.currentMoveTime = value
    },

    setAllBets: function (state, value) {
      state.allBets = value
    },

    setRatesDone: function (state, value) {
      const audio = document.getElementById('ratesDoneSound')

      audio.volume = 0.6
      state.ratesDone = value
      if (value) {
        setTimeout(() => {
          state.allBets = state.allBank
          audio.play()
        }, 1000)
      }
    },

    setCurrentPlayerMoveValues: function (state, { checkValue, minValue, stepValue }) {
      const values = state.currentPlayerMoveValues

      values.checkValue = checkValue
      values.minValue = minValue
      values.stepValue = stepValue
      state.currentPlayerRaiseInput = minValue
    },

    setOtherPlayerCurrentRate: function (state, { id, value }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      const audio = document.getElementById('individualBitSound')

      audio.volume = 0.6
      if (value && value !== rightPlayer.currentRate) audio.play()
      rightPlayer.currentRate = value
    },

    setCurrentPlayerCurrentRate: function (state, value) {
      const audio = document.getElementById('individualBitSound')

      audio.volume = 0.6
      if (value && value !== state.currentPlayer.currentRate) audio.play()
      state.currentPlayer.currentRate = value
    },

    incrementPlayerRaiseInput: function (state) {
      if (state.currentPlayerRaiseInput + state.currentPlayerMoveValues.stepValue <= state.currentPlayer.chipsCount) {
        state.currentPlayerRaiseInput += state.currentPlayerMoveValues.stepValue
      }
    },

    decrementPlayerRaiseInput: function (state) {
      if (state.currentPlayerRaiseInput - state.currentPlayerMoveValues.stepValue >= state.currentPlayerMoveValues.minValue) {
        state.currentPlayerRaiseInput -= state.currentPlayerMoveValues.stepValue
      }
    },

    setCurrentPlayerCards: function (state, value) {
      state.currentPlayer.cards = value
    },

    setCurrentPlayerBank: function (state, value) {
      state.currentPlayer.bank = value
    },

    setAllBank: function (state, value) {
      state.allBank = value
    },

    setMinChipsValue (state, value) {
      state.minChipsValue = value
    },

    setOtherPlayerIsWinner: function (state, { id, value }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.isWinner = value
    },

    setCurrentPlayerIsWinner: function (state, value) {
      state.currentPlayer.isWinner = value
    },

    setWinCount: function (state, value) {
      state.winCount = value
      const audio = document.getElementById('winningSound')

      if (value && state.currentPlayer.isWinner) {
        audio.volume = 0.6
        audio.play()
      }
    },

    toggleAutoCheck: function (state, value) {
      state.autoCheck = value
      window.mp.trigger('poker:autoCheck', value)
    },

    setGrantAutoCheck: function (state, value) {
      state.grantAutoCheck = value
    },

    setAutoCheck (state, value) {
      state.autoCheck = value
    },
    setCurrentPlayerRaiseInput: function (state, value ) {
      state.currentPlayerRaiseInput = value
    },

    setOtherPlayerCardsDisabled: function (state, { id, value }) {
      const rightPlayer = state.otherPlayers.find(item => item.id === id)
      rightPlayer.cardsDisabled = value
    },

    setCurrentPlayerCardsDisabled: function (state, value) {
      state.currentPlayer.cardsDisabled = value
    },

    // RANGE SLIDER
    resetRangeSlider: function (state) {
      state.rangeSlider.listValue = [],
      state.rangeSlider.posHandle = 0,
      state.rangeSlider.currentIndex = 0,
      state.rangeSlider.lastValue = 0,
      state.rangeSlider.sliderWrap = null,
      state.rangeSlider.coodrLeftWrap = 0
    },
    
    initRangeSlider: function (state) {
      let min = state.currentPlayerMoveValues.minValue
      let max = state.currentPlayer.chipsCount
      let step = state.currentPlayerMoveValues.stepValue
      let listValue = state.rangeSlider.listValue
      let rangeSlider = state.rangeSlider

      for (let i = min; i <= max; i += step) {
        listValue.push({
          value: i
        })
      }
      let elWrap = document.getElementById('sliderWrap')
      rangeSlider.sliderWrap = elWrap
      rangeSlider.coodrLeftWrap = rangeSlider.sliderWrap.getBoundingClientRect().left
      let period = (elWrap.offsetWidth / (listValue.length))
      let periodStart = 0
      let periodEnd = period
      listValue.forEach((item, index)=> {
        item.period_start = periodStart
        item.period_end = periodEnd
        periodStart += period
        periodEnd += period
        if (item.value == this.value) {
          rangeSlider.posHandle = item.period_start
          rangeSlider.currentIndex = index
        }
      })
      rangeSlider.lastValue = state.currentPlayerMoveValues.minValue
    },

    setRangeSliderCurrentIndex: function (state, value) {
      state.rangeSlider.currentIndex = value
    },

    setRangeSliderPosHandle: function (state, value)  {
      state.rangeSlider.posHandle = value
    },

    setRangeSliderLastValue: function (state, value) {
      state.rangeSlider.lastValue = value
    }
    // RANGE SLIDER END
  }
}
