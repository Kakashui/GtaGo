<template>
  <div class="timer">
    <div class="timer__text">{{ prettyTime }}</div>
  </div>
</template>

<script>
export default {
  name: 'HeaderTimer',

  data: function() {
    return {
      timer: null,
      currentTime: null
    }
  },

  computed: {
    prettyTime () {
      let time = this.currentTime / 60
      let minutes = parseInt(time)
      let secondes = Math.round((time - minutes) * 60)

      if (minutes < 10) {
        minutes = '0'+minutes
      }
      if (secondes < 10) {
        secondes = '0'+secondes
      }

      return `${minutes}:${secondes}`
		}
  },


  methods: {
    startWorkTimer: function() {
      this.timer = setInterval(() => {
        this.currentTime++
      }, 1000)
    },

    stopWorkTimer: function() {
      clearInterval(this.timer)
      this.timer = null
      this.currentTime = null
    }
  },

  mounted: function() {
    this.startWorkTimer()
  },

  beforeDestroy: function() {
    this.stopWorkTimer()
  }
}
</script>

<style lang="scss" scoped>
.timer {
  color: rgba(255, 255, 255, 0.5);
  font-size: 0.9rem;
  line-height: 0.7rem;
  display: inline-block;
}
</style>
