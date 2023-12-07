<template>
  <div class="input">
    <div class="input__name">{{ loc(item.name) }}</div>
    <img
      v-if="item.icon"
      :src="`img/auth/icons/${item.icon}.svg`"
      class="input__icon"
    >
    <input
      :type="item.type"
      :placeholder="loc('auth:login:lbl:enter')"
      class="input__main"
      :value="value" 
      @input="$emit('update', $event.target.value)"
    >
  </div>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  name: 'InputLabel',

  props: ["value", 'item'],

  computed: {
    ...mapGetters('localization', ['loc'])
  },

  model: {
    prop: "value",
    event: "update"
  }
}
</script>

<style lang="scss" scoped>
.input {
  width: 25rem;
  height: 4rem;
  position: relative;
  &:not(:last-child) {
    margin: 0 0 1.95rem 0;
  }
  &__name {
    position: absolute;
    display: block;
    transform: translate(0, -50%);
    top: 0;
    left: 1.8rem;
    font-size: 0.9rem;
    color: #ffffff;
  }
  &__icon {
    width: 1.1rem;
    height: 1.1rem;
    position: absolute;
    transform: translate(0, -50%);
    top: 50%;
    right: 1.95rem;
  }
  &__main {
    width: 100%;
    height: 100%;
    border-radius: 0.4rem;
    background: transparent;
    padding: 0 3.5rem 0 2rem;
    font-size: 1.1rem;
    letter-spacing: 0.1em;
    font-family: "Arial";
    color: #ffffff;
    background: center / cover no-repeat url("/img/auth/inputBg1.png");
    &:focus {
      background: center / cover no-repeat url("/img/auth/inputBg2.png");
    }
    &::placeholder {
      color: rgba(255, 255, 255, 0.3);
      letter-spacing: 0.1em;
    }
  }
}
</style>
