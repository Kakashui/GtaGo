<template>
  <div class="diamond-page">
    <div class="container">
      <div class="head">
        <div class="logo"></div>
        <div class="btn" @click="setCurrentPage('ViewPage')">{{loc('carDealership_0')}}</div>
      </div>
      <div class="img"></div>
      <div class="footer">
        <div class="vector"></div>
        <div class="info">
          <div class="info-col">
            <div class="info-col__number">1</div>
            <div class="info-col__text">{{loc('carDealership_23')}}</div>
          </div>
          <div class="info-col">
            <div class="info-col__number">2</div>
            <div class="info-col__text">{{loc('carDealership_24')}}</div>
          </div>
          <div class="info-col">
            <div class="info-col__number">3</div>
            <div class="info-col__text">{{loc('carDealership_25')}}</div>
          </div>
          <div class="info-col">
            <div class="info-col__number">4</div>
            <div class="info-col__text">{{loc('carDealership_26')}}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'DiamondPage',

  methods: {
    ...mapMutations('carDealership', ['setCurrentPage']),
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
    transform: translateX(-100%) scale(0);
  }
  to {
    transform: translateX(0%) scale(1);
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
  .diamond-page{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-repeat: no-repeat;
    background-image: url('/img/carDealership/startPage/diamond.png');
    color: #FFFFFF;
    text-transform: uppercase;
    text-transform: uppercase;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 4rem 5rem 5rem 5rem;
    .container{
      padding: 0;
      width: 86rem;
      height: 44.65rem;
      display: flex;
      flex-flow: column;
      .head{
        display: flex;
        align-items: flex-end;
        justify-content: space-between;
        width: 100%;
        margin-bottom: 1.5rem;
        animation: view-mirror .7s;
        .logo{
          width: 58.9rem;
          height: 3.85rem;
          background-image: url('/img/carDealership/diamond-logo.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: left;
        }
        .btn{
          background-image: url('/img/carDealership/diamond-btn.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          font-style: normal;
          font-weight: bold;
          font-size: 1.2rem;
          color: rgba(255, 255, 255, 0.86);
          height: 3.3rem;
          width: 10.2rem;
          display: flex;
          align-items: center;
          justify-content: center;
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
            background-image: url('/img/carDealership/diamond-btn-arr.svg');
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
          }
        }
      }
      .img{
        height: 29.9rem;
        width: 100%;
        background-image: url('/img/carDealership/diamond-img.png');
        background-size: cover;
        background-repeat: no-repeat;
        animation: identifier 1s;
      }
      .footer{
        width: 100%;
        display: flex;
        align-items: flex-start;
        margin-top: 2rem;
        padding-top: 2rem;
        border-top: .1rem solid rgba(255, 255, 255, 0.3);
        .vector{
          width: 18.8rem;
          min-width: 18.8rem;
          height: 3.9rem;
          background-image: url('/img/carDealership/diamond-vector.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          margin-right: 2rem;
          animation: view 1s;
        }
        .info{
          display: flex;
          align-items: flex-start;
          &-col{
            display: flex;
            align-items: flex-start;
            margin-right: 1.2rem;
            animation: view 1s;
            &:last-child{
              margin-right: 0;
            }
            &__number{
              font-weight: bold;
              font-size: 4.5rem;
              line-height: 4.5rem;
              color: rgba(255, 255, 255, 0.3);
              margin-right: .6rem;
            }
            &__text{
              font-size: 1rem;
              line-height: 1.2rem;
            }
          }
        }
      }
    }

  }
</style>
