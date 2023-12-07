<template>
  <div class="timer-to-capture" v-if="untilCaptureTimerEnabled">
    <div class="timer-to-capture__circle">
      <svg class="svg" :viewBox="'0 0 ' + appWidthUnit * 5 +' '+ appWidthUnit * 5" xmlns="http://www.w3.org/2000/svg">
        <circle class="circle" :stroke-dasharray="appWidthUnit * 15.06" :cx="appWidthUnit * 2.5" :cy="appWidthUnit * 2.5" :r="appWidthUnit * 2.3" fill="none" stroke="rgba(255, 255, 255, 0.3)" :stroke-width="appWidthUnit / 5" />
        <circle class="circle" :stroke-dasharray="appWidthUnit * 15.06" :stroke-dashoffset="circleValue" :cx="appWidthUnit * 2.5" :cy="appWidthUnit * 2.5" :r="appWidthUnit * 2.3" fill="none" stroke="rgba(255, 255, 255, .8)" :stroke-width="appWidthUnit / 5" />
      </svg>
    </div>
    <div class="timer-to-capture__info">
      <div class="title">{{loc(untilCaptureTimerMessage)}}</div>
      <div class="value">{{ formatTimeLeft(untilCaptureTimerCurrentTime) }}</div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from "vuex";

export default {
  name: 'TimeToCapture',

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('hud', ['untilCaptureTimerMaxTime', 'untilCaptureTimerCurrentTime', 'untilCaptureTimerEnabled', 'untilCaptureTimerMessage']),
    appWidthUnit: function () {
      const appWidth = document.getElementById('app').offsetWidth / 100
      return  Math.ceil(appWidth)
    },

    circleValue: function () {
      const maxCircleValue = this.appWidthUnit * 15.06
      const maxCircleValuePer = (maxCircleValue / 100) / this.untilCaptureTimerMaxTime * 100
      const curCircleVal = maxCircleValue - (maxCircleValuePer * this.untilCaptureTimerCurrentTime)
      
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
  .timer-to-capture{
    display: flex;
    position: absolute;
    align-items: center;
    right: .3rem;
    bottom: 14.55rem;
    text-transform: uppercase;
    padding-right: .6rem;
    &__circle{
      width: 3.5rem;
      height: 3.5rem;
      display: flex;
      align-items: center;
      justify-content: center;
      position: relative;
      margin-right: .75rem;
      &:before, &:after{ 
        content: '';
        position: absolute;
      }
      &:before{
        width: 100%;
        height: 100%;
        background: #EE443A;
        filter: blur(1.5rem);
        border-radius: 50%;
      }
      &:after{
        width: 1.5rem;
        height: 1.5rem;
        background-image: url('/img/hud/timeToCaptureIcon.svg');
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
      }
      .svg {
        width: 100%;
        height: 100%;
        transform: rotate(90deg) scale(-1, 1);
      }
    }
    &__info{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      .title{
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.1em;
        text-transform: uppercase;
        color: rgba(255, 255, 255, 0.7);
      }
      .value{
        font-weight: 300;
        font-size: 2.5rem;
        line-height: 2.5rem;
        letter-spacing: 0.1em;
        text-transform: uppercase;
        color: #FFFFFF;
      }
    }
  }
</style>
