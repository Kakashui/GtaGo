<template>
  <!-- <button 
  class="button__back" 
    @mouseenter="play({ name: 'hover' , volume: .4 })"
    @click="getBack(tab)"
  >
    <img
      class="back__icon"
      src="/img/carTunningMenu/arrow_left.svg"
      alt="arrow"
    >
    <span class="back__title">{{ loc('car_tunning_1') }}</span>
  </button> -->
  <button
    class="btn"
    @mouseenter="play({ name: 'hover' , volume: .4 })"
    @click="getBack(tab)"
  >
    <img
      class="btn__icon"
      src="/img/carTunningMenu/arrow_left.svg"
      alt="arrow"
    >
    <span class="btn__text">{{ loc('car_tunning_1') }}</span>
  </button>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'

export default {
  name: 'ButtonBack',

  props: ['tab'],

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('carTunningMenu', ['setCurrentTab']),
    ...mapMutations('sounds', ['play']),

    getBack(tab) {
      this.setCurrentTab(tab);
      window.mp.trigger('lsCustom:backPage', tab);
    }
    
  }
}
</script>

<style lang="scss" scoped>
.btn {
  display: flex;
  align-items: center;
  background: transparent;
  color: #FFFFFF;
 text-transform: uppercase;
  position: absolute;
  bottom: 10.85rem;
  left: 2.4rem;
  padding: 0.4rem;
  border-radius: 0.4rem;
  &:hover {
    background: rgba($color: #000000, $alpha: 0.3);
  }
  &__icon {
    width: 0.9rem;
    height: 0.9rem;
    margin: 0 0.5rem 0 0;
  }
  &__text {
    font-weight: bold;
    text-transform: uppercase;
    letter-spacing: 0.05em;
  }
}
</style>
