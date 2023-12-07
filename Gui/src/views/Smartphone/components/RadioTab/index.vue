<template>
  <div class="radio-tab">
    <div class="radio-tab__disk">
      <img
        src="img/hud/smartphone/radioTab/diskInner.png"
        alt="inner"
        class="inner"
        :style="{transform : 'rotate(' + currentAngle + 'deg)'}"
        ref="diskInner"
      >
    </div>
    <div class="radio-tab__main">
      <div class="block-top">
        <span class="block-top__title">RadioGo</span>
        <button class="block-top__play" @click="togglePlaying">
          <img
            class="icon"
            :src="isPlaying ? 'img/hud/smartphone/radioTab/pause.svg' : 'img/hud/smartphone/radioTab/play.svg'"
            alt="play"
          >
        </button>
      </div>
      <div class="block-center">    
          <img
            class="block-center__left"
            :src="'img/hud/smartphone/radioTab/next.svg'"
            alt="prev"
            @click="prevIndex"
          >
        <p class="block-center__title">{{radioList[index].radioName}}</p>        
          <img
            class="block-center__right"
            :src="'img/hud/smartphone/radioTab/next.svg'"
            alt="next"
            @click="nextIndex"
          >
      </div>
      <div class="block-bottom">
        <input
          :class="[{ active: soundLevel === minSoundLevel }, 'block-bottom__btn']"
          type="image"
          src="img/hud/smartphone/radioTab/volumeOff.svg"
          @click="setVolume({value: 0})"
        >
        <input
          type="range"
          :style="{ background: 'linear-gradient(to right, #B6D300 0%, #B6D300 ' + this.soundLevel + '%, rgba(255, 255, 255, 0.2) ' + this.soundLevel + '%, rgba(255, 255, 255, 0.2) 100%)'}"
          class="block-bottom__slider"
          v-model.number="soundLevel"
          :max="maxSoundLevel"
          :min="minSoundLevel"
        >
        <input
          :class="[{ active: soundLevel === maxSoundLevel },'block-bottom__btn']"
          type="image"
          src="img/hud/smartphone/radioTab/volumeOn.svg"
          @click="setVolume({value: 100})"
        >
      </div>
    </div>
  </div>
</template>

<script>

import { mapMutations, mapState } from 'vuex'

export default {
  name: 'RadioTab',

  data: function() {
    return {
      maxSoundLevel: 100,
      minSoundLevel: 0,
      isPlaying: false,
      currentAngle: 0,
      // runner: null,
      request: null, 
      fps: 45
    }
  },

  computed: {
    ...mapState('smartphone/radioPage', ['radioList', 'index', 'volume']),
    soundLevel: {
      get: function() {
        return this.volume
      },
      set: function(value) {
        this.setVolume({value: value})
      }
    },
  },

  methods: {
    ...mapMutations('smartphone/radioPage', ['playRadio', 'stopRadio', 'setVolume', 'nextIndex', 'prevIndex']),
    animLoop: function() {
      // this.runner = setTimeout(() => {
        this.request = requestAnimationFrame(this.animLoop)
        this.currentAngle++
        // console.log(this.currentAngle)
      // }, 1000 / this.fps)
    },

    togglePlaying: function() {
      this.isPlaying = !this.isPlaying

      if(this.isPlaying) {
        this.animLoop();
        this.playRadio({index: 0});
      } else if (!this.isPlaying) {
        // clearTimeout(this.runner)
        cancelAnimationFrame(this.request)
        // this.runner = null
        this.request = null
        this.stopRadio(null);
      }
    }
  }
  
}
</script>

<style lang="scss" scoped>
.radio-tab {
  height: 100%;
  background: center / cover no-repeat url("/img/hud/smartphone/radioTab/bg.png"), #000;
  &__disk {
    width: 15.2rem;
    height: 18rem;
    background: center / cover no-repeat url("/img/hud/smartphone/radioTab/disk.png");
    position: relative;
    .inner {
      position: absolute;
      top: 2.2rem;
      left: 0.85rem;
      width: 8.9rem;
      height: 8.9rem;
    }
  }
  &__main {
    color: #fff;
    padding: 0 0.75rem;
    margin: 1rem 0 0 0;
    .block-top {
      display: flex;
      justify-content: space-between;
      align-items: center;
      &__title {
        font-size: 2.2rem;
        font-weight: 700;
      }
      &__play {
        background: center / cover no-repeat url("/img/common/textures/green_texture_long-112x300.jpg");
        width: 3.5rem;
        height: 3.5rem;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        .icon {
          width: 1rem;
          height: 1rem;
        }
      }
    }
    .block-center {
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin: 1.5rem 0 0 0;
      &__desc {
        font-size: 0.8rem;
        font-weight: 300;
        opacity: 0.5;
      }
      &__title {
        font-size: 0.9rem;
        font-weight: 500;
        margin: 0.25rem 0 0 0;
        width: 10rem;
      }
      &__left {
        transform: rotate(180deg);
        width: 1rem;
      }
      &__right {
        width: 1rem;
        
      }
    }
    .block-bottom {
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin: 3.1rem 0 0 0;
      &__btn {
        width: 0.9rem;
        height: 0.9rem;
        opacity: 0.5;
        &.active {
          opacity: 1;
        }
      }
      &__slider {
        -webkit-appearance: none;
        width: 74%;
        height: .2rem;
        outline: none;
        border-radius: .25rem;
        margin: 1rem 0;
        &::-webkit-slider-thumb {
          -webkit-appearance: none;
          width: 0.9rem;
          height: 0.9rem;
          background: #B6D300;
          border-radius: 1rem;
        }
        &::-webkit-slider-runnable-track {
          -webkit-appearance: none;
        }
        &::-moz-range-progress {
          background-color: #43e5f7; 
        }
      }
    }
  }
}
</style>
