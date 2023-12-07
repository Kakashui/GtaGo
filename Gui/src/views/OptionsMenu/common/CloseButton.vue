<template>
  <div
    :style="styles"
    class="btn"
    @click="$emit('close')"
  ></div>
</template>

<script>
export default {
  name: 'CloseButton',

  props: {
    styles: Object
  }
}
</script>

<style lang="scss" scoped>
.btn {
  width: 2.5rem;
  height: 2.5rem;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.1);
  &::after {
    content: "";
    width: 1rem;
    height: 1rem;
    background: center / cover no-repeat url("/img/optionsMenu/cross.svg");
  }
}
</style>
