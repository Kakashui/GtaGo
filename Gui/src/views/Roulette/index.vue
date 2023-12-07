<template>
  <div class="roulette">
    <RoulettePrompt/>
    <RouletteStatistic/>
    <RouletteAnimations/>
    <RouletteTimer v-if="timerIsOn"/>
    <RouletteBetResult v-if="betResult.show"/>
    <div class="roulette__interaction">
      <div class="roulette__interaction-title">{{loc('Roulette_0')}}</div>
      <div class="roulette__interaction-chips">
        <RouletteChip
          v-for="(item, key) in chipsList"
          :key="key"
          :color="key"
          :count="item"
        />
      </div>
      <!-- <div class="roulette__interaction-btns">
        <div 
          class="roulette__interaction-btn repeat"
          @click="repeatBet"
        >{{loc('Roulette_1')}}</div>
        <div 
          class="roulette__interaction-btn double"
          @click="doubleBet"
        >{{loc('Roulette_2')}}</div>
      </div> -->
    </div>
    <div 
      class="roulette__interaction-btn exit"
      @click="exit"
    >{{loc('Roulette_3')}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from "vuex"

import RoulettePrompt from './RoulettePrompt'
import RouletteStatistic from './RouletteStatistic'
import RouletteAnimations from './RouletteAnimations'
import RouletteChip from './RouletteChip'
import RouletteTimer from './RouletteTimer'
import RouletteBetResult from './RouletteBetResult'

export default {
  components: {
    RoulettePrompt,
    RouletteStatistic,
    RouletteAnimations,
    RouletteChip,
    RouletteTimer,
    RouletteBetResult
  },
  name: "Roulette",
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState("roulette", [
      "playerBalance",
      "betsValue",
      "betIndex",
      "seconds",
      "currentStage",
      "showHud",
      "timerIsOn",
      "betResult"
    ]),
    ...mapState("hud", ["chipsList"])
  },
  methods: {
    exit() {
      window.mp.trigger("roulette:exit:pressed")
    },
    repeatBet() {
      window.mp.trigger("Roulette:repeatBet")
    }, 
    doubleBet() {
      window.mp.trigger("Roulette:doubleBet")
    }
  },
  beforeDestroy() {
    clearInterval(this.timer);
  },
};
</script>

<style lang="scss" scoped>
.roulette {
  width: 100%;
  height: 100vh;
  position: absolute;
  left: 0;
  top: 0;
  color: #fff;
  font-weight: bold;
  z-index: 2;
  background: radial-gradient(102.09% 85.32% at 65.03% 55.79%, rgba(16, 23, 43, 0) 28.17%, rgba(16, 23, 43, 0.74) 64.91%);
  div{
    box-sizing: border-box;
  }
  &:before, &:after{
    content: '';
    position: absolute;
    pointer-events: none;
    background-repeat: no-repeat;
  }
  &:before {
    height: 9.25rem;
    width: 100%;
    left: 0;
    bottom: 0;
    background-size: cover;
    background-position: center top;
    background-image: url("/img/roulette/shadow.png");
  }
  &:after {
    width: 12.45rem;
    height: 3.225rem;
    left: 3.1rem;
    bottom: 3.8rem;
    background-size: contain;
    background-image: url('/img/roulette/logo.png');
  }
  &__interaction{
    display: flex;
    align-items: center;
    position: absolute;
    bottom: 2.6rem;
    left: 50%;
    transform: translateX(-50%);
    margin-left: 3rem;
    &-title{
      font-size: .9rem;
      line-height: 1.05rem;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.9);
      position: absolute;
      left: 0;
      top: -1rem;
      transform: translateY(-100%);
    }
    &-chips{
      display: flex;
      align-items: center;
      margin-right: 2.3rem;
    }
    &-btns{
      display: flex;
      align-items: center;
      position: relative;
      padding-left: 1.2rem;
      &>div{
        margin-right: 1.2rem;
        &:last-child{
          margin-right: 0;
        }
      }
      &:before{
        content: '';
        height: 3.5rem;
        width: .1rem;
        background: rgba(255, 255, 255, 0.3);
        position: absolute;
        left: 0;
      }
    }
    &-btn{
      background: rgba(255, 255, 255, 0.1);
      border-radius: .1rem;
      height: 3rem;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 0 1.2rem;
      font-size: .9rem;
      letter-spacing: 0.1em;
      transition: .3s;
      &:hover{
        background: rgba(255, 255, 255, 0.2);
      }
      &:active{
        transform: scale(.95);
      }
      &:before, &:after{
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
      }
      &:before{
        margin-right: .5rem;
      }
      &:after{
        margin-left: .5rem;
      }
      &.repeat{
        &:before{
          content: '';
          width: 1.1rem;
          height: 1.1rem;
          background-image: url('/img/roulette/repeat-icon.png');
        }
      }
      &.double{
        &:before{
          content: 'x2';
          text-shadow: 0 .2rem 1rem #F23838;
          font-size: 1.1rem;
          color: #F23838;
        }
      }
      &.exit{
        position: absolute;
        bottom: 2.6rem;
        right: 3rem;
        &:after{
          content: '';
          width: 1rem;
          height: 1rem;
          background-image: url('/img/roulette/arrow-back.png');
        }
      }
    }
  }
}
</style>
