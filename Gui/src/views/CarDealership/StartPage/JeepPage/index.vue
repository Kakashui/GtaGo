<template>
  <div class="jeep-page">
    <div class="container">
      <div class="img"></div>
      <div class="info">
        <div class="logo"></div>
        <div class="text-block">
          <div class="text">{{loc('carDealership_18')}}</div>
          <div class="text">{{loc('carDealership_19')}}</div>
        </div>
        <div class="btn" @click="setCurrentPage('ViewPage')">{{loc('carDealership_0')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'JeepPage',

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
    transform: translateX(-100%);
  }
  to {
    transform: translateX(0%);
  }
}
@keyframes view-mirror {
  from {
    transform: translateX(100%);
  }
  to {
    transform: translateX(0%);
  }
}
  .jeep-page{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-repeat: no-repeat;
    background-image: url('/img/carDealership/startPage/jeep.png');
    color: #FFFFFF;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 0 7.15rem 0 5rem;
    &:before, &:after{
      content: '';
      background-size: contain;
      background-repeat: no-repeat;
      position: absolute;
      right: 0;
      animation: view-mirror linear 1s;
    }
    &:before{
      background-position: right top;
      width: 6.75rem;
      height: 8.5rem;
      background-image: url('/img/carDealership/angle-top.svg');
      top: 0;
    }
    &:after{
      background-position: right bottom;
      width: 9.85rem;
      height: 13.05rem;
      background-image: url('/img/carDealership/angle-bottom.svg');
      bottom: 0;
    }
    .container{
      width: 83.65rem;
      height: 40.55rem;
      background-image: url('/img/carDealership/jeep-bg.svg');
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center left;
      display: flex;
      justify-content: space-between;
      padding: 0;
      animation: view linear 1s;
      .img{
        width: 46.75rem;
        min-width: 46.75rem;
        height: 100%;
        background-image: url('/img/carDealership/jeep-img.png');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center left;
        position: relative;
        &:before, &:after{
          content: '';
          position: absolute;
          width: 2.9rem;
          height: 1.2rem;
          background-image: url('/img/carDealership/jeep-pros.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
        }
        &:before{
          left: 0;
          top: -.5rem;
          transform: translateY(-100%);
        }
        &:after{
          bottom: -.5rem;
          left: 29.7rem;
          transform: translateY(100%);
        }
      }
      .info{
        margin-left: 2.15rem;
        display: flex;
        flex-flow: column;
        align-items: flex-start;
        width: 34.75rem;
        .logo{
          width: 100%;
          height: 10rem;
          background-image: url('/img/carDealership/jeep-logo.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          margin-bottom: 1.6rem;
        }
        .text-block{
          width: 100%;
          display: flex;
          flex-flow: column;
          font-family: Arial;
          font-style: normal;
          font-weight: normal;
          font-size: 1.1rem;
          line-height: 1.25rem;
          padding-bottom: 1.5rem;
          margin-bottom: 1.5rem;
          border-bottom: .1rem solid rgba(255, 255, 255, 0.2);
          .text{
            width: 100%;
            &:last-child{
              margin-top: 1.25rem;
            }
          }
        }
        .btn{
          display: flex;
          align-items: center;
          justify-content: center;
          width: 7.8rem;
          height: 3.2rem;
          background-image: url('/img/carDealership/jeep-btn.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          text-transform: uppercase;
          font-style: normal;
          font-weight: normal;
          font-size: 1.2rem;
          line-height: 1.45rem;
          color: #00A3FF;
          transition: .2s;
          &:hover{
            transform: scale(1.1);
            transition: .2s;
          }
          &:after{
            content: '';
            width: 1rem;
            height: .45rem;
            margin-left: .35rem;
            background-image: url('/img/carDealership/jeep-btn-arr.svg');
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
          }
        }
      }
    }
  }
</style>
