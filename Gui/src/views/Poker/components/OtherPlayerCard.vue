<template>
  <img
    :src="`img/poker/card_${card}.png`"
    :alt="card"
    class="player__card"
    :class="{ active: index === winItem, disabled: cardsDisabled }"
  >
</template>

<script>
export default {
  name: 'OtherPlayerCard',

  props: [
    'card',
    'winCombinations',
    'index',
    'cardsDisabled'
  ],

  computed: {
    winItem: function () {
      const item = this.winCombinations.find(item => item === this.index)
      return item
    }
  }
}
</script>

<style lang="scss" scoped>
.player__card {
  width: 4.5rem;
  height: 6.5rem;
  margin: 0 0 0 .5rem;
  border-radius: .1rem;
  &:first-child {
    margin: 0;
  }
  &.active {
    transform: scale(1.2);
  }
  &.disabled {
    filter: grayscale(1) opacity(.5);
  }
}
</style>
