<template>
  <label
    class="search-label"
    ref="label"
    :style="styles"
  >
    <input
      type="text"
      class="search-label__input"
      @focus="isFocus = true"
      @blur="isFocus = false"
      :value="value"
      @input="$emit('input', $event.target.value)"
    >
    <span v-show="!isFocus" class="search-label__placeholder">{{loc('phone:serch')}}</span>
  </label>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'SearchLabel',

  props: { value: String, styles: Object },
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  data: function() {
    return {
      isFocus: false
    }
  }
}
</script>

<style lang="scss" scoped>
.search-label {
  width: 13.7rem;
  height: 1.8rem;
  display: block;
  border-radius: 0.5rem;
  background: rgba(118, 118, 128, 0.12);
  overflow: hidden;
  position: relative;
  margin: 0.9rem auto 0 auto;
  &__input {
    background: transparent;
    text-align: center;
    width: 100%;
    height: 100%;
    font-size: 0.7rem;
  }
  &__placeholder {
    position: absolute;
    transform: translate(-50%, -50%);
    top: 50%;
    left: 50%;
    color: rgba(60, 60, 67, 0.6);
    font-size: 0.7rem;
    display: flex;
    align-items: center;
    &::before {
      content: '';
      display: block;
      width: 0.7rem;
      height: 0.7rem;
      background: center / contain no-repeat url("/img/hud/smartphone/messengerTab/magnifier.svg");
      margin: 0 0.5rem 0 0;
    }
  }
}
</style>
