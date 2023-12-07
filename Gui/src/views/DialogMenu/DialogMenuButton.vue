<template>
  <button :class="[{ column: button.isColumn }, 'dialog-menu-button']" @click="click(button.index)">
    <img
      :src="`img/dialogMenu/${button.icon}.svg`"
      class="dialog-menu-button__icon"
      v-if="button.icon"
    >
    <span class="dialog-menu-button__text">{{ loc(button.text) }}</span>
  </button>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  
  name: 'DialogMenuButton',

  props: { button: Object },

  methods: {
    click: function(index) {
      window.mp.trigger('dialog::buttonClick', index)
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
  }
}
</script>

<style lang="scss" scoped>
.dialog-menu-button {
  flex: 0 1 9rem;
  height: 3.5rem;
  max-height: 3.5rem;
  display: flex;
  align-items: center;
  position: relative;
  background: rgba(255, 255, 255, 0.05);
  border: 0.1rem solid rgba(255, 255, 255, 0.1);
  border-radius: 0.2rem;
  transition: all 0.15s;
  &.column {
    margin: 0.75rem 0 0 0;
    justify-content: center;
    .dialog-menu-button__text {
      margin: 0;
    }
    &:first-child {
      margin: 0;
    }
  }
  &:hover {
    background: rgba(255, 255, 255, 0.15);
    .dialog-menu-button__icon {
      opacity: 1;
    }
  }
  &:first-child {
    background: #B6D300;
    box-shadow: 0px 0.5rem 2.5rem rgba(168, 195, 2, 0.6);
    border-color: transparent;
    .dialog-menu-button__icon {
      opacity: 1;
    }
    &:hover {
      background: #A2BF00;
      box-shadow: none;
    }
  }
  &__icon {
    position: absolute;
    left: 1.1rem;
    width: 1.3rem;
    height: 1.3rem;
    opacity: 0.3;
    transition: all 0.15s;
  }
  &__text {
    text-transform: uppercase;
    font-weight: bold;
    letter-spacing: 0.1em;
    color: #FFFFFF;
    font-size: 0.8rem;
    text-transform: uppercase;
    background: transparent;
    margin: 0 0 0 3.15rem;
  }
}
</style>
