<template>
  <div class="garage-page">
    <div class="container">
      <div class="logo"></div>
      <div class="block-link" @click="setCurrentPage('ViewPage')">
        <div class="text">{{loc('carDealership_0')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'GaragePage',

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
    transform: translateY(-100%);
  }
  to {
    transform: translateY(0%);
  }
}
  .garage-page{
    width: 100%;
    height: 100%;
    color: #FFFFFF;
    display: flex;
    padding: 3.5rem;
    background: #000;
    .container{
      width: 100%;
      height: 100%;
      background-size: cover;
      background-repeat: no-repeat;
      background-position: top;
      background-image: url('/img/carDealership/startPage/garage.png');
      position: relative;
      animation: view .5s;
      overflow: hidden;
      .logo{
        width: 100%;
        height: 50%;
        position: absolute;
        top: 0;
        left: 0;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: top;
        background-image: url('/img/carDealership/garage-logo.svg');
      }
      .block-link{
        position: absolute;
        bottom: 0;
        left: 50%;
        transform: translateX(-50%) translateY(10%);
        width: 3.5rem; 
        height: 7.25rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center bottom;
        background-image: url('/img/carDealership/link-garage.svg');
        display: flex;
        align-items: center;
        justify-content: center;
        animation: identifier 1s;
        transition: .2s;
        padding-top: 2rem;
        &:hover{
          transform: translateX(-50%) translateY(0%);
          transition: .2s;
        }
        .text{
          text-transform: uppercase;
          font-style: normal;
          font-weight: bold;
          font-size: 1.2rem;
          color: #FCC916;
          position: absolute;
          top: 0;
          text-transform: uppercase;
          white-space: pre;
        }
      }
    }
  }
</style>
