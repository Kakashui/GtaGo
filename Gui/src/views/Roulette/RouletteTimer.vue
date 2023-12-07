<template>
  <div class="roulette-timer">
    <div class="roulette-timer-title">{{loc('Roulette_9')}}</div>
    <div class="roulette-timer-value">{{secondsToTime(currentTime)}}</div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'RouletteTimer',
  data: function() {
    return {
      timer: null,
      currentTime: null,
    }
  },
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('roulette', ['seconds'])
  },
  methods: {
    startTimer() {
      this.timer = setInterval(() => {
        this.currentTime--
      }, 1000)
    },
    stopTimer() {
      clearTimeout(this.timer)
      window.mp.trigger('Roulette:stopTimer')
    },
    secondsToTime: function (secs){
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
    this.currentTime = this.seconds
    this.startTimer()
  },
  destroyed() {
    this.stopTimer()
  }
}
</script>

<style lang="scss" scoped>
.roulette-timer {
  position: absolute;
  top: 0;
  left: 50%;
  transform: translateX(-50%);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-end;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  background-image: url('/img/roulette/timer-bg.png');
  width: 10.85rem;
  height: 10.85rem;
  padding-bottom: 1.95rem;
  &:before{
    content: '';
    width: 1.5rem;
    height: 1.5rem;
    position: absolute;
    top: 2rem;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/roulette/timer-decorate.png');
  }
  &-title{
    font-size: 1rem;
    line-height: 1.15rem;
    text-align: center;
    letter-spacing: 0.04em;
    color: rgba(255, 255, 255, 0.6);
  }
  &-value{
    font-size: 3rem;
    line-height: 3.45rem;
    text-align: center;
    text-shadow: 0 .1rem 1.5rem #222222;
  }
}
</style>
