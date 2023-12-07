<template>
  <div class="work-timer">
    <span class="work-timer__text">{{ prettyTime }}</span>
  </div>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'WorkTimer',

  data: function() {
    return {
      timer: null
    }
  },

  computed: {
    ...mapState('hud', ['workTime']),

    prettyTime () {
      let time = this.workTime / 60
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
    ...mapMutations('hud', ['startWorkTimer'])
  }
}
</script>

<style lang="scss" scoped>
.work-timer {
  position: absolute;
  left: 1.5rem;
  bottom: 11.5rem;
  &__text {
    color: #fff;
    font-size: 1.4rem;
    text-shadow: 0 0 3px #000;
    font-weight: bold;
    letter-spacing: .09rem;
  }
}
</style>
