<template>
  <div class="button__exit" @click="exit">
    <img src="/img/barbershop/log_off.svg" class="exit__icon">
  </div>
</template>

<script>
export default {
  name: 'ButtonExit',

  methods: {
    exit: function() {
      window.mp.trigger("closeBarber");
    }
  }
}
</script>

<style lang="scss" scoped>
.button__exit {
  width: 3.55rem;
  height: 3.55rem;
  background-color: #1C1B1B;
  border-radius: 50%;
  position: relative;
  transition: all .15s;
  .exit__icon {
    position: absolute;
    transform: translate(-50%, -50%);
    left: 45%;
    top: 50%;
    width: 1.5rem;
    height: auto;
  }
  &:hover {
    box-shadow: 0px 2px 4px 0 #000000;
  }
}
</style>
