<template>
  <div class="mercedes-page">
    <div class="title">
      <div>Mercedes-</div>
      <div>benz</div>
    </div>
    <div class="info">{{loc('carDealership_16')}}</div>
    <div class="slogan">The best or nothing</div>
    <div class="block-link" @click="setCurrentPage('ViewPage')">
      <div class="text">discover.</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'MercedesPage',

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
    transform: translateY(100%) translateX(100%);
  }
  to {
    opacity: 1;
    transform: translateY(0) translateX(0);
  }
}
.mercedes-page{
  width: 100%;
  height: 100%;
  background-size: cover;
  background-repeat: no-repeat;
  background-image: url('/img/carDealership/startPage/mercedes.png');
  color: #FFFFFF;
  display: flex;
  align-items: flex-start;
  justify-content: flex-start;
  .title{
    display: flex;
    flex-flow: column;
    align-items: flex-end;
    -webkit-writing-mode: vertical-rl; 
    writing-mode:tb-rl;
    transform: rotate(180deg);
    height: 100%;
    padding-left: 2rem;
    animation: identifier linear 1s;
    &>div{
      display: flex;
      justify-content: flex-end;
      padding: 2rem 0;
      text-align: right;
      height: 100%;
      border-left: 1px solid rgba(255, 255, 255, 0.2);
      font-family: Playfair Display;
      font-style: normal;
      font-weight: bold;
      font-size: 10rem;
      line-height: 13.35rem;
    }
  }
  .info{
    font-family: Roboto;
    font-style: normal;
    font-weight: normal;
    font-size: 1.2rem;
    line-height: 1.4rem;
    margin: 2rem 0 0 1rem;
    max-width: 27.55rem;
    animation: identifier linear 1s;
  }
  .slogan{
    font-family: Playfair Display;
    font-style: normal;
    font-weight: normal;
    font-size: 1.2rem;
    line-height: 1.6rem;
    position: absolute;
    right: 2rem;
    top: 2rem;
    padding-right: 6rem;
    display: flex;
    align-items: center;
    justify-content: center;
    height: 5rem;
    animation: identifier linear 1s;
    &:after{
      content: '';
      width: 5rem;
      height: 5rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-image: url('/img/carDealership/mercedes-logo.png');
      background-position: center;
      position: absolute;
      right: 0;
    }
  }
  .block-link{
    position: absolute;
    bottom: 3.85rem;
    right: 18.2rem;
    width: 10.9rem;
    height: 10.9rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border: .1rem solid #fff;
    border-radius: 50%;
    animation: view 1.5s;
    transition: .2s;
    &:hover{
      transform: scale(1.1);
      transition: .2s;
    }
    .text{
      font-family: Arial;
      font-style: normal;
      font-weight: bold;
      font-size: 2.5rem;
      line-height: 2.85rem;
      text-transform: lowercase;
      position: absolute;
      right: 0;
      transform: translateX(50%);
    }
  }
}
</style>
