<template>
  <div class="lambo-page">
    <div class="info">
      <div class="info__logo"></div>
      <div class="info__text">{{loc('carDealership_17')}}</div>
    </div>
    <div class="logo"></div>
    <div class="block-link" @click="setCurrentPage('ViewPage')">
      <div class="text">discover.</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'LamboPage',

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
    opacity: 0;
    transform: translateX(-50%) translateY(100%);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) translateY(0);
  }
}
.lambo-page{
  width: 100%;
  height: 100%;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url('/img/carDealership/startPage/lambo.png');
  padding: .7rem 2rem 0 2.5rem;
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  justify-content: flex-start;
  color: #FFFFFF;
  font-family: Arial;
  .info{
    display: flex;
    flex-flow: column;
    width: 45.5rem;
    animation: identifier 1s;
    &__logo{
      width: 100%;
      height: 9.5rem;
      background-image: url('/img/carDealership/Lamborghini.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
    }
    &__text{
      padding-left: 1.5rem;
      font-style: normal;
      font-weight: normal;
      font-size: 1.2rem;
      line-height: 1.4rem;
    }
  }
  .logo{
    background-image: url('/img/carDealership/lambo-logo.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: center;
    width: 4.5rem;
    height: 5.1rem;
    position: absolute;
    top: 2rem;
    right: 2rem;
    animation: identifier 1s;
  }
  .block-link{
    position: absolute;
    bottom: 6.1rem;
    width: 10.9rem;
    height: 10.9rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 50%;
    border: .1rem solid #fff;
    transform: translateX(-50%);
    left: 50%;
    animation: view 1s;
    transition: .2s;
    &:hover{
      transform: translateX(-50%) scale(1.1);
      transition: .2s;
    }
    .text{
      position: absolute;
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.85rem;
      text-transform: lowercase;
      right: 0;
      transform: translateX(50%);
    }
  }

}
</style>
