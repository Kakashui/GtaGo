<template>
  <div class="capture">
    <transition name="capture-state" appear>
      <div class="capture__state" v-if="isCaptureItems">
        <div
          v-for="(team, index) in captureTeams"
          :key="index"
          :class="[{ reversed: index === 0 }, 'side']"
        >
          <img
            :src="`img/hud/captureUI/${team.key}.png`"
            :alt="team.key"
            class="side__icon"
          >
          <div class="side__desc">
            <h6 class="title">{{ team.title }}</h6>
            <span class="count">{{ team.players }}</span>
          </div>
        </div>
        <div class="timer">
          <span class="timer__value">{{ formatTimeLeft(captureTimeCurrent) }}</span>
          <svg class="timer__svg" xmlns="http://www.w3.org/2000/svg">
            <circle class="circle" :stroke-dasharray="appWidthUnit * 12.55" :cx="appWidthUnit * 2.5" :cy="appWidthUnit * 2.5" :r="appWidthUnit * 2" fill="none" stroke="rgba(255, 255, 255, 0.3)" :stroke-width="appWidthUnit / 4" />
            <circle class="circle" :stroke-dasharray="appWidthUnit * 12.55" :stroke-dashoffset="circleValue" :cx="appWidthUnit * 2.5" :cy="appWidthUnit * 2.5" :r="appWidthUnit * 2" fill="none" stroke="rgba(255, 255, 255, .8)" :stroke-width="appWidthUnit / 4" />
          </svg>
        </div>
      </div>
    </transition>
    <transition-group
      tag="ul"
      name="list"
      class="capture__log"
      v-if="isCaptureLog"
      appear
    >
      <li
        v-for="(item, index) in captureLog"
        :key="`key_${index}`"
        class="item"
      >
        <span v-if="item.killerName" :style="{ color: colors[item.killerFraction] }" class="item__killer">{{ item.killerName }}</span>
        <img
          :src="`img/hud/captureUI/weapons/${item.weaponId}.png`"
          :alt="item.weaponId"
          class="item__icon"
        >
        <span class="item__deceased" :style="{ color: colors[item.deceasedFraction] }">{{ item.deceasedName }}</span>
      </li>
    </transition-group>
  </div>
</template>

<script>
import { mapState } from 'vuex'

export default {
  name: 'Capture',

  data: function() {
    return {
      colors: {
        neutral: '#B7B7B7',
        attackers: '#ff2400',
        defenders: '#03c03c',
        ballas: '#7A34CF',
        bloods: '#EE443A',
        vagos: '#C8AE00',
        families: '#30B02B',
        marabunta: '#1180C1'
      }
    }
  },

  computed: {
    ...mapState('hud', [
      'captureTeams',
      'captureLog',
      'isCaptureItems',
      'isCaptureLog',
      'captureTimeLimit',
      'captureTimeCurrent',
      'captureTimer'
    ]),

    appWidthUnit: function () {
      const appWidth = document.getElementById('app').offsetWidth / 100
      return  Math.ceil(appWidth)
    },

    circleValue: function () {
      const maxCircleValue = this.appWidthUnit * 12.55
      const maxCircleValuePer = (maxCircleValue / 100) / this.captureTimeLimit * 100
      const curCircleVal = maxCircleValue - (maxCircleValuePer * this.captureTimeCurrent)
      
      return curCircleVal
    }
  },

  methods: {
    formatTimeLeft: function(time) {
      let minutes = Math.floor(time / 60)
      let seconds = time % 60;
      
      if (seconds < 10) {
        seconds = `0${seconds}`
      }
      if (minutes < 10) {
        minutes = `0${minutes}`
      }

      return `${minutes}:${seconds}`
    }
  }
}
</script>

<style lang="scss" scoped>
.capture {
  width: 100%;
  height: 100%;
  position: relative;
  text-transform: uppercase;
  &__state {
    transform: translate(-50%, 0);
    position: absolute;
    top: 0;
    left: 50%;
    width: 50rem;
    height: 10rem;
    background: center / contain no-repeat url('/img/hud/captureUI/state-substrate.png'), radial-gradient(54% 147.27% at 50% -47.27%, rgba(233, 21, 21, 0.5) 0%, rgba(233, 21, 21, 0) 100%);
    filter: drop-shadow(0px 4px 4px rgba(0, 0, 0, 0.25));
    display: flex;
    // justify-content: center;
    padding: .75rem 0 0 0;
    .side {
      display: flex;
      align-items: flex-start;
      width: 50%;
      padding: 0 0 0 3.8rem;
      &__icon {
        width: 4rem;
        height: 4rem;
      }
      &__desc {
        color: #fff;
        margin: .7rem 0 0 1rem;
        .title {
          letter-spacing: .1em;
          font-size: 0.9rem;
          text-transform: uppercase;
        }
        .count {
          font-size: 2rem;
        }
      }
      &.reversed {
        flex-direction: row-reverse;
        padding: 0 3.8rem 0 0;
        .side__desc {
          margin: .7rem 1rem 0 0;
          text-align: right;
        }
      }
    }
    .timer {
      position: absolute;
      transform: translate(-50%, 0);
      top: 0;
      left: 50%;
      width: 5rem;
      height: 5rem;
      &__value {
        position: absolute;
        transform: translate(-50%, -50%);
        top: 50%;
        left: 50%;
        color: #fff;
        letter-spacing: 0.1em;
      }
      &__svg {
        width: 100%;
        height: 100%;
        transform: rotate(90deg) scale(-1, 1);
      }
    }
  }
  &__log {
    transform: translate(0 -50%);
    position: absolute;
    top: 50%;
    right: 0;
    margin: 0 1.5rem;
    list-style: none;
    text-transform: uppercase;
    .item {
      display: flex;
      align-items: center;
      justify-content: flex-end;
      letter-spacing: .05rem;
      &__icon {
        margin: 0 .75rem;
        width: 2.5rem;
        height: 2.5rem;
      }
    }
  }
}
.list-enter-active, .list-leave-active, .capture-state-enter-active, .capture-state-leave-active {
  transition: all .4s;
}
.list-enter {
  opacity: 0;
  position: absolute;
  transform: translateX(1rem);
}
.list-leave-to {
  opacity: 0;
  position: absolute;
  transform: translateX(-1rem);
}
.capture-state-enter {
  opacity: 0;
  transform: translate(-50%, -1rem);
}
.capture-state-leave-to {
  opacity: 0;
  transform: translate(-50%, -1rem);
}
</style>
