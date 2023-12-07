<template>
  <div class="car-tunning">
    <div class="car-tunning__balance">
      <div class="icon"></div>
      <span class="value">{{money.toLocaleString()}}</span>
    </div>
    <div class="car-tunning__actions">
      <button @click="toggleSound(randomBgSound)" class="volume">
        <img
          v-if="!isBgSoundPlaying"
          class="volume__icon"
          src="/img/carTunningMenu/mute.svg"
          alt="mute"
        >
        <img
          v-else-if="isBgSoundPlaying"
          class="volume__icon"
          src="/img/carTunningMenu/volume.svg"
          alt="mute"
        >
      </button>
      <button class="exit" @click="exitTunning">
        <img
          class="exit__icon"
          src="/img/carTunningMenu/close.svg"
          alt="close"
          @click="exitTunning()"
          @mouseenter="play({ name: 'hover' , volume: .4 })"
        >
      </button>
    </div>
    <component :is="currentTab"/>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

import FirstTab from './components/FirstTab'
import SecondTab from './components/SecondTab'
import ThirdTab from './components/ThirdTab'
import FourthTab from './components/FourthTab'

export default {
  name: 'CarTunningMenu',

  components: {
    FirstTab,
    SecondTab,
    ThirdTab,
    FourthTab
  },

  data: function() {
    return {
      isBgSoundPlaying: true
    }
  },

  computed: {
    ...mapState('carTunningMenu', ['currentTab']),
    ...mapState('hud', ['money']),

    randomBgSound: function() {
      const items = ['tunningBg1', 'tunningBg2', 'tunningBg3', 'tunningBg4', 'tunningBg5', 'tunningBg6', 'tunningBg7', 'tunningBg8', 'tunningBg9', 'tunningBg10', 'tunningBg11', 'tunningBg12', 'tunningBg13']
      return items[Math.floor(Math.random() * items.length)]
    }
  },

  methods: {
    ...mapMutations('carTunningMenu', ['setCurrentTab']),
    ...mapMutations('sounds', ['play', 'stop']),

    exitTunning: function() {
        window.mp.trigger('lsCustom:exitTun');
    },

    toggleSound: function(sound) {
      if (this.isBgSoundPlaying) {
        this.stop(sound)
        this.isBgSoundPlaying = false;
      } else {
        this.play({ name: sound, volume: .04, loop: true })
        this.isBgSoundPlaying = true;
      }
    }
  },

  mounted: function() {
    this.play({ name: this.randomBgSound, volume: .04, loop: true })
  },

  beforeDestroy: function () {
    this.stop(this.randomBgSound)
    this.isBgSoundPlaying = false
  }
}
</script>

<style lang="scss" scoped>
.car-tunning {
  width: 100%;
  height: 100%;
  position: relative;
 text-transform: uppercase;
  &__balance {
    position: absolute;
    top: 2.8rem;
    left: 2.8rem;
    display: flex;
    .icon {
      width: 1.4rem;
      height: 1.4rem;
      border-radius: 50%;
      border: 0.1rem solid rgba(193, 231, 4, 0.65);
      display: flex;
      align-items: center;
      justify-content: center;
      margin: 0 0.6rem 0 0;
      &::before {
        content: "";
        background: center / contain no-repeat url("/img/carTunningMenu/dollar1.svg");
        height: 0.85rem;
        width: 0.35rem;
      }
    }
    .value {
      color: #C1E704;
      font-size: 1.55rem;
      line-height: 1.55rem;
      letter-spacing: 0.05em;
    }
  }
  &__actions {
    position: absolute;
    top: 2.5rem;
    right: 1.45rem;
    z-index: 99;
    display: flex;
    align-items: center;
    .volume {
      width: 2.3rem;
      height: 2.3rem;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      background: transparent;
      margin: 0 1.7rem 0 0;
      &:hover {
        background: rgba($color: #000000, $alpha: 0.3);
      }
      &__icon {
        width: 90%;
        height: 90%;
      }
    }
    .exit {
      width: 2.5rem;
      height: 2.5rem;
      background: transparent;
      border-radius: 50%;
      &:hover {
        background: rgba($color: #000000, $alpha: 0.3);
      }
      &__icon {
        width: 100%;
        height: 100%;
      }
    }
  }
}
</style>
