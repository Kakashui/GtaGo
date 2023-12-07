<template>
  <img
    :src="`img/poker/card_${card}.png`"
    :alt="card"
    class="general__card"
    :class="{ active: index === winItem, disabled: cardsDisabled }"
  >
</template>

<script>
export default {
  name: 'CurrentPlayerCard',

  props: [
    'card',
    'index',
    'winCombinations',
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
.general__card {
  width: 4.5rem;
  height: 6.5rem;
  margin: 0 .5rem 0 0;
  border-radius: .1rem;
  &.active {
    transform: scale(1.2)
  }
  &.disabled {
    filter: grayscale(1) opacity(.5);
  }
}
</style>
