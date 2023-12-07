<template>
  <div class="actions__range">
    <span class="range__title">{{loc('barber_3')}}</span>
    <input
      class="range__input"
      v-if="currentColors !== undefined"
      :max="currentColors"
      min="0"
      step = "1"
      v-model="currentIndex"
      type="range"
    >
  </div>
</template>

<script>
import {mapGetters} from 'vuex'

export default {
  name: 'CurrentTypeRange',

  props: ['currentColors', 'value'],

  computed: {
    ...mapGetters('localization', ['loc']),
    currentIndex: {
      get: function() {
        return this.value.color
      },
      set: function(value) {
        this.value.color = value;
        this.$emit("onChange");
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.actions__range {
  margin: 1.5rem 0 0 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  background: linear-gradient(90deg, rgba(26, 26, 26, 0.46) 0%, #1E1E1E 49.48%, rgba(26, 26, 26, 0.43) 100%);
  border-radius: .6rem;
  padding: .5rem 0;
  .range__title {
    color:#ADADAD;
    font-weight: bold;
    font-size: .8rem;
    letter-spacing: .05rem;
    text-transform: uppercase;
  }
  .range__input {
    appearance: none;
    width: 74%;
    height: .3rem;
    outline: none;
    border-radius: .25rem;
    margin: 1rem 0;
    &::-webkit-slider-thumb {
      appearance: none;
      width: 1.1rem;
      height: 1.1rem;
      background-color: #71B020;
      box-shadow: .1rem -.1rem .2rem 0 rgba(#000, .8);
      border: 1px solid white;
      border-radius: 1rem;
    }
  }
}
</style>
