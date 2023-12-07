<template>
  <transition name="fade">
    <div class="overlay"></div>
  </transition>
</template>

<script>
export default {
  name: 'DialogMenuOverlay'
}
</script>

<style lang="scss" scoped>
.overlay {
  position: absolute;
  width: 100%;
  height: 100%;
  background: radial-gradient(28.13% 50% at 50% 50%, rgba(2, 3, 16, 0.5) 0%, rgba(2, 3, 16, 0.85) 100%);
  z-index: 98;
}
.fade-enter-active, .fade-leave-active {
  transition: opacity .4s;
}
.fade-enter, .fade-leave-to /* .fade-leave-active  to version 2.1.8 */ {
  opacity: 0;
}
</style>
