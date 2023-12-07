<template>
  <div class="ferrari-page">
    <div class="info">
      <div class="info__logo"></div>
      <div class="info__text-wrap">
        <div class="text">{{loc('carDealership_20')}}</div>
        <div class="text">{{loc('carDealership_21')}}</div>
        <div class="text">{{loc('carDealership_22')}}</div>
      </div>
    </div>
    <div class="block-link" @click="setCurrentPage('ViewPage')">
      <div class="text">discover.</div>
    </div>
    <div class="slogan">
      <div class="slogan__text">POWER, PERFORMANCE AND BEAUTY</div>
      <div class="slogan__logo"></div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'FerrariPage',

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
    transform: translateX(-50%) scale(0);
  }
  to {
    transform: translateX(-50%) scale(1);
  }
}
.ferrari-page{
  width: 100%;
  height: 100%;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url('/img/carDealership/startPage/ferrari.png');
  padding: 2rem;
  display: flex;
  align-items: flex-start;
  justify-content: flex-start;
  color: #FFFFFF;
  .info{
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    animation: identifier linear 1s;
    &__logo{
      margin-right: 1rem;
      height: 5.7rem;
      width: 29.6rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-image: url('/img/carDealership/ferrari-logo.png');
      background-position: center;
    }
    &__text-wrap{
      display: flex;
      flex-flow: column;
      max-width: 39.85rem; 
      .text{
        font-family: Montserrat;
        font-style: normal;
        font-weight: 500;
        font-size: 1.2rem;
        line-height: 1.45rem;
      }
    }
  }
  .block-link{
    display: flex;
    align-items: center;
    width: 10.9rem;
    height: 10.9rem;
    border-radius: 50%;
    border: .1rem solid #fff;
    position: absolute;
    bottom: 2rem;
    left: 50%;
    transform: translateX(-50%);
    animation: view .7s;
    transition: .2s;
    &:hover{
      transform: translateX(-50%) scale(1.1);
      transition: .2s;
    }
    .text{
      font-size: 2.5rem;
      line-height: 3.05rem;
      text-transform: lowercase;
      position: absolute;
      right: 0;
      transform: translateX(50%);
    }
  }
  .slogan{
    display: flex;
    align-items: center;
    position: absolute;
    right: 2rem;
    bottom: 2rem;
    animation: identifier 1s;
    &__text{
      font-family: Montserrat;
      font-style: normal;
      font-weight: 500;
      font-size: 1.2rem;
      line-height: 1.45rem;
      text-align: right;
      text-transform: lowercase;
      margin-right: 1rem;
      max-width: 12.4rem;
    }
    &__logo{
      background-image: url('/img/carDealership/ferrari.png');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      width: 5rem;
      height: 7.1rem;
    }
  }
}
</style>
