<template>
  <button :class="[ `${button.class}`, 'main-button']" @click="button.event">
    <span class="main-button__title">{{ loc(button.text) }}</span>
  </button>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'MainButton',

  props: {
    button: Object
  },
   computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
.main-button {
  height: 2.4rem;
  border-radius: 0.1rem;
  font-size: 0.75rem;
  font-weight: 500;
  display: flex;
  align-items: center;
  justify-content: center;
  letter-spacing: 0.02em;
  color: #fff;
  transition: all 0.15s;
  &.act {
    background: #A2BC1F;
    width: 10.05rem;
    &:hover {
      box-shadow: 0px 1rem 2rem rgba(193, 231, 4, 0.3);
    }
  }
  &.close {
    background: rgba(255, 255, 255, 0.25);
    width: 4rem;
    &:hover {
      box-shadow: 0px 1rem 2rem rgba(255, 255, 255, 0.3);
    }
  }
}
</style>
