<template>
  <div class="phase-timer">
    <div class="phase-timer__desc">{{loc('PhaseTimer_0')}}</div>
    <div class="phase-timer__wrap">
      <div class="title">{{phaseTimer.title}}</div>
      <div class="timer">{{secondsToTime(currentTime)}}</div>
    </div>
    <div class="phase-timer__progress">
      <div 
        class="progress-line"
        :style="{width: currentProgressWidth + '%'}"
      ></div>
    </div>
    <div class="phase-timer__statistic">
      <div class="statistic-item">
        <div class="statistic-item__value">{{phaseTimer.alive}}</div>
        <div class="statistic-item__desc">Alive</div>
      </div>
      <div class="statistic-item">
        <div class="statistic-item__value">{{phaseTimer.kills}}</div>
        <div class="statistic-item__desc">Kills</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'PhaseTimer',

  computed: {
    ...mapState('hud', ['phaseTimer']),
    ...mapGetters('localization', ['loc']),
    currentProgressWidth: function() {
      return ((this.currentTime/this.phaseTimer.time) * 100)
    }
  },

  data: function() {
    return {
      currentTime: 500,
      timer: null,
    }
  },

  methods: {
    startTimer() {
      this.timer = setInterval(() => {
        this.currentTime--
      }, 1000)
    },
    stopTimer() {
      clearTimeout(this.timer)
    },
    secondsToTime: function (secs){
      // let hours = Math.floor(secs / (60 * 60))

      let divisor_for_minutes = secs % (60 * 60)
      let minutes = Math.floor(divisor_for_minutes / 60)

      let divisor_for_seconds = divisor_for_minutes % 60
      let seconds = Math.ceil(divisor_for_seconds)

      if (minutes < 10 && seconds < 10) {
        return '0' + minutes + ':0' + seconds 
      } else if (minutes < 10) {
        return '0' + minutes + ':' + seconds 
      } else if (seconds < 10) {
        return minutes + ':0' + seconds 
      } else {
        return minutes + ':' + seconds 
      }
    }
  },

  watch: {
    currentTime(time) {
      if (time === 0) {
        this.stopTimer()
      }
    }
  },

  mounted() {
    this.startTimer()
    this.currentTime = this.phaseTimer.time
  },

  destroyed() {
    this.stopTimer()
  },
}
</script>

<style lang="scss" scoped>
.phase-timer{
  width: 12.5rem;
  position: absolute;
  left: 2.5rem;
  bottom: 12.35rem;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  flex-flow: column;
  text-transform: uppercase;
  &:before{
    content: '';
    height: 3rem;
    width: 100%;
    background: #151720;
    filter: blur(6rem);
    position: absolute;
    bottom: 2.5rem;
    z-index: 1;
  }
  &>div{
    z-index: 2;
    color: #FFFFFF;
    text-shadow: 0 0 .5rem rgba(0, 0, 0, 0.15);
  }
  &__desc{
    font-style: normal;
    font-weight: normal;
    font-size: .9rem;
    line-height: 1.1rem;
    color: rgba(255, 255, 255, 0.7);
    margin-bottom: .25rem;
    text-align: left;
    width: fit-content
  }
  &__wrap{
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: .2rem;
    .title, .timer{
      white-space: pre;
    }
    .title{
      font-style: normal;
      font-weight: 300;
      font-size: 1.8rem;
      line-height: 1.8rem;
      letter-spacing: 0.08em;
      position: relative;
      display: flex;
      align-items: center;
      &:after{
        content: '';
        width: 1.4rem;
        height: .1rem;
        background: rgba(255, 255, 255, 0.2);
        border-radius: .5rem; 
        margin: 0 .75rem;
      }
    }
    .timer{
      font-weight: bold;
      font-size: 1.8rem;
      line-height: 1.8rem;
      text-align: center;
      letter-spacing: 0.01em;
    }
  }
  &__progress{
    width: 100%;
    height: .2rem;
    position: relative;
    background-color: rgba(255, 255, 255, 0.2);
    border-radius: .5rem;
    margin-bottom: .5rem;
    .progress-line{
      position: absolute;
      height: 100%;
      left: 0;
      top: 0;
      background: #A2BC1F;
      border-radius: .5rem;
      box-shadow: 0 .5rem 2rem #C1E704;
    }
  }
  &__statistic{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    .statistic-item{
      display: flex;
      align-items: center;
      width: 100%;
      margin-right: .5rem;
      background: rgba(0, 0, 0, 0.5);
      &:last-child{
        margin-right: 0;
      }
      &__value{
        display: flex;
        align-items: center;
        justify-content: center;
        width: 2rem;
        height: 2rem;
        background: rgba(0, 0, 0, 0.7);
        font-weight: bold;
        font-size: 1.5rem;
        line-height: 1.5rem;
      }
      &__desc{
        height: 100%;
        padding: 0 .9rem 0 .6rem;
        font-style: normal;
        font-weight: 300;
        font-size: 1.5rem;
        line-height: 1.5rem;
        letter-spacing: 0.01em;
      }
    }
  }
}
</style>
