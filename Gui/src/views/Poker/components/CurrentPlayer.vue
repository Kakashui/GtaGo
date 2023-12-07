<template>
  <div class="poker__player-current">
    <div class="player__general">
      <div
        v-if="currentPlayer.currentRate"
        class="player__rate"
        :class="{ done: ratesDone }"
      >
        <img src="img/poker/chip_black.png" alt="chip">
        <span>{{ currentPlayer.currentRate.toLocaleString() }}</span>
      </div>
      <div class="general__info">
        <div 
          class="poker__player-option"
          @click="setOption"
        ></div>
        <div class="info__avatar" :class="{ active: currentPlayer.id === activePlayer }">
          <div
            v-if="currentPlayer.avatar"
            :style="{backgroundImage: `url(${currentPlayer.avatar})`}"
            alt="avatar"
            class="avatar__img"
          ></div>
          <div
            v-else
            :style="{backgroundImage: `url(/img/poker/avatar.png)`}"
            alt="avatar"
            class="avatar__img"
          ></div>
          <svg 
            viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"
            v-if="currentPlayer.id === activePlayer && currentMoveTime !== 0"
          >
            <circle
              :stroke-dasharray="calcCircumference" 
              :stroke-dashoffset="circleValue" 
            ></circle>
          </svg>
        </div>
      </div>
      <div class="general__cards">
        <current-player-card
          v-for="(card, index) in currentPlayer.cards"
          :key="index"
          :card="card"
          :index="index"
          :winCombinations="currentPlayer.winCombinations"
          :cardsDisabled="currentPlayer.cardsDisabled"
        />
      </div>
      <div class="chips-info" :class="{ active: currentPlayer.chipsCount <= minChipsValue }" >
        <span class="chips-info__title">{{loc('poker_4')}}</span>
        <span class="chips-info__count">$ {{ currentPlayer.chipsCount.toLocaleString()}}</span>
        <div 
          class="chips-info__btn"
          @click="addChips"
        >{{loc('poker_5')}}</div>
      </div>
      <div class="info__state">
        <span v-if="currentPlayer.state">{{ loc(currentPlayer.state) }}</span>
        <span 
          v-if="currentPlayer.isWinner" 
          class="win-count"
        >+ $ {{ winCount.toLocaleString() }}</span>
      </div>
      
    </div>
    <current-player-menu 
      :currentPlayer="currentPlayer" 
      @setExitWindow="$emit('setExitWindow', true)"
    />
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import CurrentPlayerMenu from './CurrentPlayerMenu'
import CurrentPlayerCard from './CurrentPlayerCard'

export default {
  name: 'CurrentPlayer',

  components: { CurrentPlayerMenu, CurrentPlayerCard },
  data() {
    return { 
      timer: null,
      currentTime: null,
    }
  },  

  computed: {
    ...mapState('poker', [
      'currentPlayer',
      'activePlayer',
      'currentMoveTime',
      'ratesDone',
      'winCount',
      'minChipsValue'
    ]),
    ...mapGetters('localization',['loc']),
    calcCircumference: function () {
      let number = 2 * Math.PI * 48
      return  number
    },
    circleValue: function () {
      const maxCircleValue = this.calcCircumference
      if (this.currentMoveTime <= 0 || this.currentTime <= 0)
        return maxCircleValue;
      const maxCircleValuePer = (maxCircleValue / this.currentMoveTime) * this.currentTime
      
      return maxCircleValue - maxCircleValuePer
    },
  },
  methods: {
    addChips: function () {
      this.$emit('setSaleOfChips', true)
    },
    setOption: function () {
      this.$emit('setAvatarOption', true)
    },
    startTimer() {
      this.currentTime = this.currentMoveTime
      this.timer = setInterval(() => {
        this.currentTime--
      }, 1000)
    },
    stopTimer() {
      clearInterval(this.timer)
      this.timer = null;
    },
  },
  watch: {
    currentTime(val) {
      if (val <= 0 && this.timer != null) {
        this.stopTimer()
      }
    },
    currentMoveTime(val) {
      if (val === 60) {
        if (this.timer === null)
          this.startTimer()
        else        
          this.currentTime = this.currentMoveTime
      }
      else if (val <= 0 && this.timer != null) {
        this.stopTimer()
      }    
    }
  },
  mounted() {
    if (this.currentMoveTime === 60) {
      this.startTimer()
    }
  },
  destroyed() {
    this.stopTimer()
  },
}
</script>

<style lang="scss" scoped>
.poker__player-current {
  display: flex;
  flex-direction: column;
  align-items: center;
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translate(-50%, 0);
  width: 100%;
  .poker__player-option{
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    left: -.2rem;
    top: 0;
    width: 2rem;
    height: 2rem;
    background: #A3BC22;
    transform-origin: 50% 50%;
    border-radius: 50%;
    transition: .3s;
    z-index: 1;
    &:hover{
      transform: rotate(45deg);
    }
    &:before{
      content: '';
      width: 1.2rem;
      height: 1.2rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/poker/option-icon.png');
    }
  }
  .player__rate {
    position: absolute;
    transition: all .7s ease;
    display: flex;
    align-items: center;
    top: -1rem;
    left: 20.85rem;
    padding: .7rem 1rem;
    background: rgba(255, 255, 255, 0.1);
    border-radius: 3rem;
    transform: translateY(-100%);
    &.done {
      opacity: 0;
      top: -35%;
      left: 50%;
    }
    img {
      width: 1.2rem;
      height: 1.2rem;
      margin-right: .5rem;
    }
    span {
      font-weight: bold;
      font-size: 1rem;
      line-height: 1.15rem;
      color: #FFFFFF;
    }
  }
  .player__general {
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0 0 1.4rem 0;
    position: relative;
    .general__cards {
      display: flex;
      align-self: flex-end;
    }
    .chips-info{
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      margin-left: .5rem;
      &__title{
        font-weight: bold;
        font-size: .9rem;
        line-height: 1.05rem;
        text-align: center;
        color: rgba(255, 255, 255, 0.8);
        display: flex;
        flex-direction: column;
        align-items: center;
        margin-bottom: .5rem;
        &:after{
          content: '';
          width: 2.8rem;
          height: 1.4rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/poker/chips-decorate.png');
          margin-top: .5rem;
        }
      }
      &__count{
        font-weight: bold;
        font-size: 1rem;
        letter-spacing: 0.04em;
        color: #FFFFFF;
        margin-bottom: .3rem;
      }
      &__btn{
        border: 1px solid #C1E704;
        border-radius: 2rem;
        height: 1.6rem;
        width: 4.8rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 1rem;
        font-weight: bold;
        font-size: .8rem;
        text-transform: lowercase;
        color: #C1E704;
        opacity: .7;
        transition: .3s;
        &:hover{
          opacity: 1;
        }
      }
    }
    .general__info {
      display: flex;
      flex-direction: column;
      align-items: center;
      .info__winner {
        width: 2rem;
      }
      .info__avatar {
        width: 5.3rem;
        height: 5.3rem;
        position: relative;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 50%;
        border: .2rem solid rgba(255, 255, 255, 0.2);
        margin-right: 1rem;
        box-sizing: border-box;
        .avatar__img {
          width: 100%;
          height: 100%;
          overflow: hidden;
          border-radius: 50%;
          background-size: cover;
          background-repeat: no-repeat;
          background-position: center;
        }
        svg{
          position: absolute;
          width: 5.3rem;
          height: 5.3rem;
          circle{
            fill: transparent;
            cx: 50%;
            cy: 50%;
            r: 48%;
            stroke-width: 2%;
            stroke: rgba(0, 255, 0, 1)!important;
            transform-origin: 50% 50%;
            transform: rotate(-90deg);
          }
        }
      }
    }
    .info__state {
      position: absolute;
      top: -1.2rem;
      transform: translateY(-100%); 
      font-weight: bold;
      font-size: 1.5rem;
      letter-spacing: 0.04em;
      color: rgba(255, 255, 255, 0.5);
      display: flex;
      flex-direction: column;
      .win-count{
        font-weight: bold;
        font-size: 1.5rem;
        line-height: 1.7rem;
        letter-spacing: 0.04em;
        color: #FFC117;
        margin-top: .25rem;
      }
    }
  }
}
</style>
