<template>
  <div class="speedometer">
    <div class="speedometer__substrate"></div>
    <div class="speedometer__main">
      <svg class="circle--inner" xmlns="http://www.w3.org/2000/svg" version="1.1">
        <defs>
          <radialGradient id="gradient">
            <stop offset="50%" stop-color="white" stop-opacity=".8" />
            <stop offset="92%" stop-color="white" stop-opacity="0.26" />
            <stop offset="94%" stop-color="white" stop-opacity="0.24" />
            <stop offset="96%" stop-color="white" stop-opacity="0.16" />
            <stop offset="98%" stop-color="white" stop-opacity="0.08" />
            <stop offset="100%" stop-color="white" stop-opacity="0.02" />
          </radialGradient>
        </defs>
        <circle :stroke-dasharray="appWidthUnit * 23.55" :cx="appWidthUnit * 6.5" :cy="appWidthUnit * 6.5" :r="appWidthUnit * 5" stroke="url('#gradient')" :stroke-width="appWidthUnit / 2" fill="none" />
      </svg>
      <svg class="circle--outer">
        <filter id="neon">
          <feDropShadow dx="0" dy="0" stdDeviation="1.2" flood-color="#B6D300"/>
        </filter>
        <circle :stroke-dasharray="appWidthUnit * 34.55" :stroke-dashoffset="appWidthUnit * 8.65" :cx="appWidthUnit * 6.5" :cy="appWidthUnit * 6.5" :r="appWidthUnit * 5.5" stroke="rgba(255, 255, 255, 0.35)" :stroke-width="appWidthUnit / 1.5" fill="none" />
        <circle :stroke-dasharray="appWidthUnit * 34.55" :stroke-dashoffset="speedValue" :cx="appWidthUnit * 6.5" :cy="appWidthUnit * 6.5" :r="appWidthUnit * 5.5" stroke="#B6D300" filter="url(#neon)" :stroke-width="appWidthUnit / 1.5" fill="none" />
      </svg>
      <div class="speed">
        <span class="speed__title">km/h</span>
        <div class="speed__values">
          <span
            :class="[{ transparent: index === 2 && curSpeed === 0 || index === 0 && curSpeed < 10 || index === 1 && curSpeed < 10 || index === 0 && curSpeed < 100 }, 'item']"
            v-for="(item, index) in speedFormat"
            :key="index"
          >{{ item }}</span>
        </div>
      </div>
      <div class="fuel">
        <div class="fuel__bar">
          <div class="static"></div>
          <div class="dynamic" :style="{ width: fuelLevel + '%', background: fuelBarColor, 'box-shadow': `0 0 .1rem 0 ${fuelBarColor}` }"></div>
        </div>
        <div class="fuel__value">
          <span class="current">{{curFuel}}</span>
          <span class="maximum">/{{maxFuel}}</span>
        </div>
      </div>
      <img
        src="img/hud/speedometer/engine.svg"
        alt="engine"
        :class="[{ transparent: !engine}, 'icon', 'engine']"
      >
      <img
        src="img/hud/speedometer/doors.svg"
        alt="doors"
        :class="[{ transparent: !doors}, 'icon', 'doors']"
      >
      <img
        src="img/hud/speedometer/belt.svg"
        alt="belt"
        :class="[{ transparent: !belt}, 'icon', 'belt']"
      >
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'


export default {
  name: 'Speedometer',
  data() {
    return {
      appWidthUnit: 23
    }
  },
  computed: {
    ...mapState('speedometer', [
      'curSpeed',
      'maxSpeed',
      'maxFuel',
      'curFuel',
      'inVeh',
      'engine',
      'doors',
      'belt'
    ]),

    speedValue: function () {
      if (this.curSpeed > 400) return this.appWidthUnit * 8.65
      const maxCircleValue = this.appWidthUnit * 34.55
      const maxCircleValuePer = ((maxCircleValue - this.appWidthUnit * 8.65) / 100) / this.maxSpeed * 100
      const curCircleVal = maxCircleValue - (maxCircleValuePer * this.curSpeed)
      
      return curCircleVal
    },

    speedFormat: function() {
      let speed = String(this.curSpeed)

      if (this.curSpeed < 10) {
        speed = '00' + speed
      } else if (this.curSpeed < 100) {
        speed = '0' + speed
      }

      return speed.split('')
    },

    fuelLevel: function () {
      return this.curFuel / this.maxFuel * 100
    },
    
    fuelBarColor: function () {
      if (this.fuelLevel <= 10) {
        return '#EC502D'
      } else if (this.fuelLevel <= 55) {
        return '#ECAB2D'
      } else {
        return '#C4F53C'
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@mixin position($bottom, $right) {
  position: absolute;
  bottom: $bottom;
  right: $right;
}
@mixin center() {
  position: absolute;
  transform: translate(-50%, -50%);
  left: 50%;
  top: 50%;
  width: 100%;
  height: 100%;
}
@keyframes slide {
  0% {
    transform: translateX(100%);
    opacity: 0;
  }
  100% {
    transform: translateX(0);
    opacity: 1;
  }
}
.speedometer {
  width: 300px;
  height: 300px;
  animation: slide .15s;
  @include position(.3rem, .3rem);
  &__substrate {
    z-index: 1;
    @include center();
    background: radial-gradient(34.29% 34.29% at 50% 50%, rgba(0, 0, 0, 0.4) 0%, rgba(0, 0, 0, 0) 100%);
  }
  &__main {
    z-index: 2;
    @include center();
    .circle--inner {
      @include center();
      circle {
        // stroke-dasharray: 628.3184;
        // stroke-dasharray: 471;
        transform: rotate(90deg);
        transform-origin: 50% 50%;
      }
    }
    .circle--outer {
      @include center();
      circle {
        // stroke-dasharray: 691;
        // stroke-dashoffset: 173;
        transform: rotate(90deg);
        transform-origin: 50% 50%;
        &:last-child {
          transition: stroke-dashoffset .5s;
        }
      }
    }
    .speed {
      transform: translate(-50%, -50%);
      position: absolute;
      left: 50%;
      top: 44%;
     text-transform: uppercase;
      display: flex;
      flex-direction: column;
      align-items: center;
      &__title {
        color: rgba(255, 255, 255, 0.5);
        font-size: .85rem;
        letter-spacing: .05rem;
      }
      &__values {
        display: inline-block;
        line-height: 3.2rem;
        .item {
          font-size: 3.5rem;
          color: rgba(255, 255, 255, 1);
          &.transparent {
            color: rgba(255, 255, 255, 0.5);
          }
        }
      }
    }
    .fuel {
      transform: translate(-50%, -50%);
      position: absolute;
      left: 50%;
      top: 65%;
     text-transform: uppercase;
      display: flex;
      flex-direction: column;
      align-items: center;
      &__bar {
        width: 110px;
        height: 5px;
        border-radius: 10px;
        position: relative;
        .static {
          position: absolute;
          width: 100%;
          height: 100%;
          background-color: rgba(255, 255, 255, 0.2);
          border-radius: 1rem;
        }
        .dynamic {
          position: absolute;
          height: 100%;
          border-radius: 1rem;
          transition: all .5s;
        }
      }
      &__value {
        margin: 4px 0 0 0;
        color: #fff;
        .maximum {
          color: rgba(255, 255, 255, 0.65);
          font-size: .9rem;
          letter-spacing: .05rem;
        }
      }
    }
    .icon {
      position: absolute;
      width: 1.3rem;
      height: 1.3rem;
      &.transparent {
        opacity: 0.4;
      }
      &.engine {
        top: 86%;
        left: 58%;
      }
      &.doors {
        top: 78%;
        left: 78%;
      }
      &.belt {
        top: 58%;
        left: 86%;
      }
    }
  }
}
</style>
