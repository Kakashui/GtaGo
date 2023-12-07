<template>
  <div class="poker__player-other" v-if="player.show">
    <div class="player__info">
      <div v-if="player.isWinner" :class="{ active: winCount }" class="player__win-count">
        <span>+ $ {{ winCount.toLocaleString() }}</span>
      </div>
      <div class="info__avatar" :class="{ active: player.id === activePlayer }">
        <div
          v-if="player.avatar"
          :style="{backgroundImage: `url(${player.avatar})`}"
          alt="avatar"
          class="avatar__img"
        ></div>
        <div
          v-else
          :style="{backgroundImage: `url(/img/poker/avatar.png)`}"
          alt="avatar"
          class="avatar__img"
        ></div>
        <div 
          v-if="player.isVoise"
          class="player__voise"
        ></div>
        <svg 
            viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg"
            v-if="player.id === activePlayer && currentMoveTime !== 0"
        >
          <circle
            :stroke-dasharray="calcCircumference" 
            :stroke-dashoffset="circleValue" 
          ></circle>
        </svg>
      </div>
      <div class="info_description">
        <span class="description__name">{{ player.name }}</span>
        <span class="description__chips">{{player.chipsCount.toLocaleString()}}</span>
        <span class="description__state">{{ loc(player.state) }}</span>
      </div>
      <div
        v-if="player.currentRate"
        class="player__rate"
        :class="{ done: ratesDone }"
      >
        <img src="img/poker/chip_black.png" alt="chip">
        <span v-if="!ratesDone">{{ player.currentRate.toLocaleString() }}</span>
      </div>
    </div>
    <div class="player__cards" v-if="player.cards.length">
      <other-player-card
        v-for="(card, index) in player.cards"
        :key="index"
        :card="card"
        :winCombinations="player.winCombinations"
        :index="index"
        :cardsDisabled="player.cardsDisabled"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'
import OtherPlayerCard from './OtherPlayerCard'

export default {
  name: 'OtherPlayer',

  components: { OtherPlayerCard },

  props: ['player'],
  data() {
    return { 
      timerOtherPlayer: null,
      currentTime: null,
    }
  },  
  computed: {
    ...mapState('poker', [
      'activePlayer',
      'currentMoveTime',
      'ratesDone',
      'winCount'
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
    startTimer() {
      this.currentTime = this.currentMoveTime
      this.timerOtherPlayer = setInterval(() => {
        this.currentTime--
      }, 1000)
    },
    stopTimer() {
      clearInterval(this.timerOtherPlayer)
      this.timerOtherPlayer = null;
    },
  },
  watch: {
    currentTime(val) {
      if (val <= 0 && this.timerOtherPlayer != null) {
        this.stopTimer()
      }
    },
    currentMoveTime(val) {
      if (val === 60) {
        if (this.timerOtherPlayer === null)
          this.startTimer()
        else        
          this.currentTime = this.currentMoveTime
      }
      else if (val <= 0 && this.timerOtherPlayer != null) {
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

</style>

<style lang="scss" scoped>
.poker__player-other {
  display: flex;
  align-items: flex-start;
  flex-direction: column;
  position: absolute;
  z-index: 2;
  .player__win-count {
    position: absolute;
    transition: all 1s ease;
    opacity: 0;
    &.active {
      opacity: 1;
    }
    span {
      font-weight: bold;
      font-size: 1.5rem;
      letter-spacing: 0.04em;
      color: #FFC117;
    }
  }
  .player__rate {
    color: #e5e5e5;
    position: absolute;
    display: flex;
    align-items: center;
    transition: all .7s ease;
    padding: .7rem 1rem;
    background: rgba(255, 255, 255, 0.1);
    border-radius: 3rem;
    &.done {
      opacity: 0;
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
  .player__info {
    display: flex;
    align-items: center;
    justify-content: flex-start;
    position: relative;
    height: 5.65rem;
    background: rgba(255, 255, 255, 0.06);
    border-radius: 100px;
    padding: .65rem;
    min-width: 18.4rem;
    .info__winner {
      width: 2rem;
    }
    .info__avatar {
      position: relative;
      display: flex;
      justify-content: center;
      align-items: center;
      width: 4.3rem;
      min-height: 4.3rem;
      margin-right: 1rem;
      .avatar__img {
        width: 4.3rem;
        height: 4.3rem;
        border-radius: 50%;
        overflow: hidden;
        background-size: cover;
        background-repeat: no-repeat;
        background-position: center;
        background-color: #000;
      }
      svg{
        position: absolute;
        width: 4.3rem;
        height: 4.3rem;
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
    .info_description {
      height: 100%;
      padding: .2rem;
      color: #e5e5e5;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      justify-content: space-between;
      .description__state {
        color: rgba(255, 255, 255, 0.4);
      }
      .description__name{
        letter-spacing: 0.04em;
      }
      .description__chips{
        position: relative;
        padding-left: 1.4rem;
        display: flex;
        align-items: center;
        &:before{
          content: '';
          position: absolute;
          left: 0;
          width: 1rem;
          min-width: 1rem;
          height: 1rem;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          background-image: url('/img/poker/chip_black.png');
        }
      }
    }
  }
  .player__cards {
    display: flex;
    margin: 1rem 0;
    &.player__card {
      &.active {
        transform: scale(1.2);
      }
    }
  }
  .player__voise{
    position: absolute;
    right: -.25rem;
    top: 0;
    width: 1.6rem;
    height: 1.6rem;
    background: #27B1FF;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    &:before{
      content: '';
      width: .8rem;
      height: .8rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/poker/voise-icon.png');
    }
  }
  &.player-1 {
    bottom: 0;
    left: 0;
    margin: 0 0 9.25rem 1.2rem;
    flex-direction: column-reverse;
    .player__win-count.active {
      transform: translateY(-100%);
      top: -1.2rem;
      right: 0;
    }
    .player__rate {
      left: 13.5rem;
      top: -1rem;
      transform: translateY(-100%);
      &.done {
        top: -60%;
        left: 125%;
      }
    }
  }
  &.player-2 {
    top: 0;
    left: 0;
    margin: 1.2rem 0 0 1.2rem;
    .player__win-count.active {
      transform: translateY(100%);
      bottom: -1.2rem;
      right: 0;
    }
    .player__rate {
      left: 13.5rem;
      bottom: -1rem;
      transform: translateY(100%);
      &.done {
        bottom: -60%;
        left: 140%;
      }
    } 
    .player__cards .player__card  {
      &.active {
        transform: scale(1.2);
      }
    }
  }
  &.player-3 {
    top: 0;
    left: 50%;
    margin: .8rem 0 0 0;
    transform: translate(-50%, 0);
    .player__win-count.active {
      transform: translateY(100%);
      bottom: -1.2rem;
      right: 0;
    }
    .player__rate {
      left: 13.5rem;
      bottom: -1rem;
      transform: translateY(100%);
      &.done {
        bottom: -119%;
        left: 35%;
      }
    }
  }
  &.player-4 {
    top: 0;
    right: 0;
    margin: 1.2rem 1.2rem 0 0;
    align-items: flex-end;
    .player__win-count.active {
      transform: translateY(100%);
      bottom: -1.2rem;
      left: 0;
    }
    .player__rate {
      right: 13.5rem;
      bottom: -1rem;
      transform: translateY(100%);
      &.done {
        bottom: -125%;
        left: -140%;
      }
    }
  }
  &.player-5 {
    bottom: 0;
    right: 0;
    margin: 0 1.2rem 9.25rem 0;
    align-items: flex-end;
    flex-direction: column-reverse;
    .player__win-count.active {
      transform: translateY(-100%);
      top: -1.2rem;
      left: 0;
    }
    .player__rate {
      right: 13.5rem;
      top: -1rem;
      transform: translateY(-100%);
      &.done {
        top: -84%;
        left: -110%;
      }
    }
  }
}
</style>
