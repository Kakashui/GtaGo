<template>
  <div class="casual-page">
    <div class="container">
      <div class="info">{{loc('carDealership_27')}}</div>
      <div class="block-link" @click="setCurrentPage('ViewPage')">
        <div class="text">{{loc('carDealership_0')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'CasualPage',

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
    transform: scale(0);
  }
  to {
    transform: scale(1);
  }
}
@keyframes view {
  from {
    transform: translateX(-50%) translateY(100%);
  }
  to {
    transform: translateX(-50%) translateY(5%);
  }
}
  .casual-page{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url('/img/carDealership/startPage/casual.png');
    color: #FFFFFF;
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    padding: 3rem;
    text-transform: uppercase;
    text-transform: uppercase;
    .container{
      width: 100%;
      height: 100%;
      border-top: 1px solid rgba(255,255,255, .2);
      border-left: 1px solid rgba(255,255,255, .2);
      border-right: 1px solid rgba(255,255,255, .2);
      display: flex;
      align-items: center;
      justify-content: center;
      .info{
        position: absolute;
        bottom: 34.7rem;
        max-width: 25.6rem;
        font-style: normal;
        font-weight: normal;
        font-size: 1.1rem;
        line-height: 1.3rem;
        text-align: center;
        z-index: 1;
        animation: identifier 1s;
      }
      .block-link{
        position: absolute;
        bottom: 0;
        left: 50%;
        transform: translateX(-50%) translateY(5%);
        width: 3.5rem; 
        height: 7.25rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center bottom;
        background-image: url('/img/carDealership/link-motor.svg');
        display: flex;
        align-items: center;
        justify-content: center;
        animation: view 1s;
        transition: .2s;
        &:hover{
          transform: translateX(-50%) translateY(0%);
          transition: .2s;
        }
        .text{
          font-weight: bold;
          font-size: 1.2rem;
          line-height: 1.2rem;
          position: absolute;
          top: 0;
          white-space: pre;
        }
      }
    }
  }
</style>
