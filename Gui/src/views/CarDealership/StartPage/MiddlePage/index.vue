<template>
  <div class="middle-page">
    <div class="title">middle class cars</div>
    <div class="desc">{{loc('carDealership_14')}}</div>
    <div class="info">
      <div class="info__value">150+</div>
      <div class="info__text">{{loc('carDealership_15')}}</div>
    </div>
    <div class="block-link" @click="setCurrentPage('ViewPage')">
      <div class="text">{{loc('carDealership_0')}}</div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'MiddlesPage',

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
    transform: translateY(0);
  }
}
  .middle-page{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-repeat: no-repeat;
    background-image: url('/img/carDealership/startPage/middle.png');
    color: #FFFFFF;
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    justify-content: space-between;
    text-transform: uppercase;
    text-transform: uppercase;
    padding: 9rem 4rem 6rem 4rem;
    .title{
      font-weight: bold;
      font-size: 8rem;
      line-height: 8rem;
      max-width: 44rem;
      animation: identifier linear 1s;
    }
    .desc{
      font-weight: bold;
      font-size: 2rem;
      line-height: 2rem;
      max-width: 35.15rem;
      animation: identifier linear 1s;
    }
    .info{
      width: 6rem;
      height: 23.45rem;
      position: absolute;
      right: 4rem;
      bottom: 2.8rem;
      -webkit-writing-mode: vertical-rl; 
      writing-mode:tb-rl;
      display: flex;
      align-items: flex-start;
      animation: view linear 1s;
      &__value{
        font-weight: bold;
        font-size: 6rem;
        line-height: 6rem;
        letter-spacing: -0.045em;

      }
      &__text{
        font-size: 1.2rem;
        line-height: 124%;
        border-top: .1rem solid #fff;
        margin-top: 1.1rem;
        padding-top: .8rem;
        padding-right: 1.5rem;
      }
    }
    .block-link{
      display: flex;
      align-items: center;
      justify-content: center;
      width: 4.15rem;
      height: 4.15rem;
      position: absolute;
      bottom: 6rem;
      left: 50%;
      transform: translateX(-50%);
      border-radius: 50%;
      background-color: #D6FF00;
      transition: .2s;
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      background-image: url('/img/carDealership/link-middle.svg');
      transition: .2s;
      animation: identifier linear 1s;
      &:hover{
        box-shadow: 0 0 1.5rem rgba(193, 231, 4, 0.7);
        transition: .2s;
      }
      .text{
        font-weight: bold;
        font-size: 1.2rem;
        line-height: 1.2rem;
        color: #D6FF00;
        white-space: pre;
        padding-top: .8rem;
        position: absolute;
        top: -.8rem;
        transform: translateY(-100%);
      }
    }
  }
</style>
