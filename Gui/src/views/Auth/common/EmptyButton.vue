<template>
  <button
    :style="styles"
    class="btn"
    @click="$emit('event')"
  >{{ text }}</button>
</template>

<script>
export default {
  name: 'EmptyButton',

  props: ['text', 'styles']
}
</script>

<style lang="scss" scoped>
.btn {
 text-transform: uppercase;
  font-size: 0.9rem;
  color: #FFFFFF;
  position: relative;
  width: fit-content;
  background: transparent;
  display: block;
  &:hover::after {
    transform: translate(0, -50%) scale(1);
  }
  &:after {
    content: "";
    display: block;
    transform: translate(0, -50%) scale(0);
    top: 100%;
    left: 50%;
    width: 100%;
    height: 0.1rem;
    background: rgba(255, 255, 255, 0.2);
    transition: all 0.15s;
  }
}
</style>
