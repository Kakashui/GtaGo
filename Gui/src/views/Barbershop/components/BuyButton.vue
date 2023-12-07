<template>
  <div class="button-buy" @click="onClick">
    <div class="buy__logo">
      <img src="/img/barbershop/dollar.svg" class="logo__icon">
    </div>
    <div class="buy__main">
      <span class="main__text">{{loc('barber_12')}}</span>
    </div>
  </div>
</template>

<script>
import {mapGetters} from 'vuex'

export default {
  name: 'BuyButton',
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  methods:{
    onClick(){
      this.$emit("onClick")
    }
  }
}
</script>

<style lang="scss" scoped>
.button-buy {
  position: relative;
  width: 13rem;
  transition: transform .15s;
  &:hover {
    transform: scale(1.05);
  }
  .buy__logo {
    position: absolute;
    z-index: 4;
    background: linear-gradient(270deg, #71B020 0%, #B3D028 135.51%);
    width: 3.8rem;
    height: 3.8rem;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    top: 50%;
    left: 1.9rem;
    transform: translate(-50%, -50%);
    &::after {
      position: absolute;
      content: '';
      transform: translate(-50%, -50%);
      left: 50%;
      top: 50%;
      width: 108%;
      height: 108%;
      border-radius: 50%;
      border: 1px solid #c9e731;
    }
    .logo__icon {
      width: 2.5rem;
      height: 2.5rem;
    }
  }
  .buy__main {
    position: absolute;
    z-index: 3;
    background: linear-gradient(270deg, #71B020 0%, #B3D028 135.51%);
    border-radius: .6rem;
    width: 9.2rem;
    height: 3.5rem;
    transform: translate(-50%, -50%) skew(15deg);
    top: 50%;
    left: 6.4rem;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 0 0 0 1.8rem;
    .main__text {
      display: inline-block;
      transform: skew(-15deg);
      color: #fff;
    }
  }
}
</style>
