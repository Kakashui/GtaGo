<template>
  <label :style="styles" class="label">
    <input class="label__checkbox" type="checkbox" :checked="value" @change="$emit('input', $event.target.checked)">
    <!-- <div class="label__text">{{ text }}</div> -->
    <slot></slot>
  </label>
</template>

<script>
export default {
  name: 'ActionCheckbox',

  props: ['text', 'value', 'styles']
}
</script>

<style lang="scss" scoped>
.label {
  display: flex;
  align-items: center;
  &__checkbox {
    visibility: hidden;
    width: 1.5rem;
    height: 1.5rem;
    margin: 0 0.75rem 0 0;
    &::after {
      content: "";
      border: 1px solid rgba(255, 255, 255, 0.2);
      border-radius: 0.3rem;
      visibility: visible;
      display: inline-block;
      width: 1.5rem;
      height: 1.5rem;
    }
    &:checked::after {
      background: center / 0.9rem no-repeat url("/img/auth/icons/check.svg"), #fff;
    }
  }
}
</style>
