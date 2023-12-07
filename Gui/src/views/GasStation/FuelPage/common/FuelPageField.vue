<template>
  <div class="field">
    <input
      v-if="text === 'L'"
      type="number"
      class="field__input"
      :value="value" 
      min="0"
      :max="maxLiters - currentLiters"
      @input="$emit('update', $event.target.value)"
    >
    <input
      v-else
      type="number"
      class="field__input"
      :value="value" 
      min="0"
      :max="maxLiters - currentLiters"
      @input="$emit('update', $event.target.value)"
    >
    <div class="field__text">{{ text }}</div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
  name: 'FuelPageField',

  props: [
    'text',
    'value',
  ],

  computed: {
    ...mapState('gasStation', ['maxLiters', 'currentLiters', 'currentFuel'])
  },

  model: {
    prop: 'value',
    event: 'update'
  }
}
</script>

<style lang="scss" scoped>
.field {
  display: flex;
  align-items: center;
  &__input {
    background: transparent;
    height: 2.15rem;
    width: 5.65rem;
    border: 1px solid rgba(255, 255, 255, 0.6);
    border-radius: 0.1rem;
    font-size: 1.1rem;
    font-weight: bold;
   text-transform: uppercase;
    color: #FFFFFF;
    margin: 0 0.5rem 0 0;
    padding: 0 0.6rem;
    &::-webkit-outer-spin-button,
    &::-webkit-inner-spin-button {
      -webkit-appearance: none;
      margin: 0;
    }
  }
  &__text {
    font-size: 1.1rem;
    line-height: 1.1rem;
    font-weight: bold;
    color: #FFFFFF;
  }
}
</style>
