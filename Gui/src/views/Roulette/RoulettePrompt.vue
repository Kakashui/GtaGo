<template>
  <div class="roulette-prompt">
    <div class="roulette-prompt-item">{{loc('Roulette_4')}}</div>
    <div class="roulette-prompt-item">{{loc('Roulette_5')}}</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'RoulettePrompt',
  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.roulette-prompt {
  display: flex;
  align-items: center;
  justify-content: center;
  position: absolute;
  right: 3rem;
  top: 2rem;
  pointer-events: none;
  &-item {
    display: flex;
    align-items: center;
    justify-content: center;
    margin-right: 2rem;
    position: relative;
    font-size: 0.9rem;
    line-height: 130%;
    letter-spacing: 0.04em;
    &:before {
      content: "";
      width: 1.7rem;
      height: 1.7rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      margin-right: 0.5rem;
    }
    &:nth-child(1) {
      &:before {
        background-image: url("/img/roulette/mouse-left.png");
      }
    }
    &:nth-child(2) {
      &:before {
        background-image: url("/img/roulette/mouse-right.png");
      }
    }
    &:last-child {
      margin-right: 0;
    }
  }
}
</style>
