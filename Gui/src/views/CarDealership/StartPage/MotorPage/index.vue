<template>
  <div class="motor-page">
    <div class="url">motorcycle.store</div>
    <div class="info">
      <div class="info__title">Sander’s</div>
      <div class="info__desc">Motorcycle</div>
    </div>
    <div class="block-link" @click="setCurrentPage('ViewPage')">
      <div class="text">{{loc('carDealership_0')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'MotorPage',

  methods: {
    ...mapMutations('carDealership', ['setCurrentPage'])
  },

  computed: {
    ...mapGetters('localization', ['loc'])
  }
}
</script>

<style lang="scss" scoped>
@keyframes identifier {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
@keyframes view {
  from {
    transform: translateY(100%);
  }
  to {
    transform: translateY(10%);
  }
}
.motor-page{
  width: 100%;
  height: 100%;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url('/img/carDealership/startPage/motor.png');
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: space-between;
  padding-top: 2rem;
  .url{
    font-family: Roboto;
    font-style: normal;
    font-weight: normal;
    font-size: 1rem;
    line-height: 1.15rem;
    color: #FFFFFF;
    animation: identifier linear 1.5s;
  }
  .info{
    display: flex;
    flex-flow: column;
    align-items: center;
    position: absolute;
    text-transform: uppercase;
    color: #FFFFFF;
    text-transform: uppercase;
    align-self: center;
    top: 50%;
    transform: translateY(-50%);
    animation: identifier linear 1.5s;
    &__title{
      font-size: 5rem;
      line-height: 6rem;
    }
    &__desc{
      margin-top: .5rem;
      font-weight: bold;
      font-size: 10rem;
      line-height: 10rem;
    }
  }
  .block-link{
    width: 3.5rem;
    height: 5.45rem;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/carDealership/link-motor.svg');
    transform: translateY(10%);
    animation: view linear 1s;
    transition: .2s;
    &:hover{
      transform: translateY(5%);
      transition: .2s;
    }
    .text{
      font-family: Roboto;
      font-style: normal;
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.15rem;
      color: #A09E9E;
      padding-bottom: .5rem;
      position: absolute;
      top: 0;
      transform: translateY(-100%);
      white-space: pre;
    }
  }
}
</style>
