<template>
  <transition name="fade">
    <img
      :src="`img/poker/card_${card}.png`"
      :alt="card"
      class="distribution__card"
      :class="{ active: index === winItem }"
    >
  </transition>
</template>

<script>
import { mapState } from 'vuex'

export default {
  name: 'DistributionCard',

  props: ['card', 'index'],

  computed: {
    ...mapState('poker', ['cardsDistributionCombinations']),

    winItem: function () {
      const item = this.cardsDistributionCombinations.find(item => item === this.index)
      return item
    }
  },

  mounted: function () {
    const audio = document.getElementById('cardSound')
    audio.play()
  }
}
</script>

<style lang="scss" scoped>
.distribution__card {
  width: 6.1rem;
  height: 8.85rem;
  margin-right: 1rem;
  border-radius: .1rem;
  transition: all .2s ease;
  &:last-child {
    margin-right: 0;
  }
  &.active {
    transform: scale(1.2)
  }
}
.fade-enter-active, .fade-leave-active {
  transition: all .5s;
}
.fade-enter, .fade-leave-to{
  opacity: 0;
  transform: translateY(-1.5rem);
}
</style>
