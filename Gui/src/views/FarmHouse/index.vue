<template>
  <div class="farm-house">
    <div class="farm-house-cereals"></div>
    <Board class="farm-house__board"/>
    <div class="farm-house-cereals farm-house-cereals--front"></div>
  </div>
</template>

<script>
import Board from './Board'
export default {
  name: 'FarmHouse',

  components: {
    Board
  }
}
</script>

<style lang="scss" scoped>
.farm-house{
  width: 100%;
  height: 100%;
  background-image: url('/img/farmHouse/bg.png');
  background-size: cover;
  background-position: center;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  &__board{
    z-index: 3;
  }
  &:after{
    content: '';
    z-index: 2;
    width: 100%;
    height: 100%;
    position: absolute;
    left: 0;
    top: 0;
    background: radial-gradient(50% 50% at 50% 50%, rgba(18, 19, 23, 0) 0%, rgba(21, 23, 29, 0.6) 100%);
  }
  &-cereals{
    width: 20.6rem;
    height: 20.6rem;
    z-index: 1;
    position: absolute;
    right: 2.24rem;
    bottom: -4rem;
    transform-origin: 100% 100%;
    animation: rotateCereal 15s infinite;
    pointer-events: none;
    &:after{
      content: '';
      width: 100%;
      height: 100%;
      background-image: url('/img/farmHouse/cereals.png');
      background-position: center;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      transform-origin: 50% 50%;
      transform: rotate(-170deg);
      position: absolute;
      left: 0;
      top: 0;
    }
    &--front{
      z-index: 4;
      right: 4.88rem;
      bottom: -10.8rem;
      animation: rotateCereal 40s infinite;
    }
  }
}
@keyframes rotateCereal{
  0% {
    transform: rotate(0deg);
  }
  20% {
    transform: rotate(8deg);
  }
  40% {
    transform: rotate(3deg);
  }
  60% {
    transform: rotate(11deg);
  }
  80% {
    transform: rotate(5deg);
  }
}
</style>
