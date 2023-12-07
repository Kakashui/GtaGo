<template>
  <div class="stock-info">
    <div class="stock-info__menu">
      <div class="header">
        <div class="descr">{{loc('fracinfo_1')}}</div>
        <div class="title">{{this.data.title}}</div>
        <div class="btn-close" @click="close"></div>
      </div>
      <div class="body">
        <div class="body__info meds">
          <div class="title">{{loc('fracinfo_2')}}</div>
          <div class="value">{{this.data.medkits}} / {{this.data.medkitsMax}}</div>
        </div>
        <div class="body__info drugs">
          <div class="title">{{loc('fracinfo_3')}}</div>
          <div class="value">{{this.data.drugs}} / {{this.data.drugsMax}}</div>
        </div>
        <div class="body__info cash">
          <div class="title">{{loc('fracinfo_4')}}</div>
          <div class="value">{{this.data.money}}</div>
        </div>
        <div class="body__info mats">
          <div class="title">{{loc('fracinfo_5')}}</div>
          <div class="value">{{this.data.mats}} / {{this.data.matsMax}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'StockInfo',

  computed: {
    ...mapState('stockInfo', ['data']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    close: function() {
      window.mp.trigger("stockInfo::close")
    }
  }
}
</script>

<style lang="scss" scoped>
  .stock-info{
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 100%;
    &__menu{
      display: flex;
      flex-flow: column;
      width: 30rem;
      .header{
        display: flex;
        flex-flow: column;
        width: 100%;
        position: relative;
        background-image: url('/img/stockInfo/header-bg.png');
        background-size: cover;
        padding: 2rem;
        .descr{
          font-weight: bold;
          font-size: .8rem;
          line-height: .95rem;
          letter-spacing: 0.02em;
          text-transform: uppercase;
          color: #FFFFFF;
          margin-bottom: .1rem;
        }
        .title{
          font-weight: 900;
          font-size: 2rem;
          line-height: 2.35rem;
          letter-spacing: 0.02em;
          text-transform: uppercase;
          color: #FFFFFF;
        }
        .btn-close{
          position: absolute;
          right: 1.1rem;
          top: 1rem;
          width: 1rem;
          height: 1rem;
          background-image: url('/img/stockInfo/btn-close.svg');
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          transition: .2s;
          &:hover{
            transform: scale(1.1);
            transition: .2s;
          }
        }
      }
      .body{
        display: flex;
        flex-flow: column;
        width: 100%;
        height: 28.85rem;
        min-height: 28.85rem;
        justify-content: flex-end;
        align-items: flex-start;
        padding: 2.5rem;
        background-image: url('/img/stockInfo/body-bg.png');
        background-size: cover;
        background-position: center;
        &__info{
          display: flex;
          flex-flow: column;
          align-items: flex-start;
          justify-content: center;
          width: 100%;
          position: relative;
          padding: .5rem 0 .5rem 2.8rem;
          margin-bottom: .65rem;
          &:before{
            content: '';
            width: 2rem;
            height: 2rem;
            background-position: center;
            background-size: contain;
            background-repeat: no-repeat;
            position: absolute;
            left: 0;
          }
          &.meds{
            &:before{
              background-image: url('/img/stockInfo/meds.svg');
            }
          }
           &.drugs{
            &:before{
              background-image: url('/img/stockInfo/drugs.svg');
            }
          }
           &.cash{
            &:before{
              background-image: url('/img/stockInfo/cash.svg');
            }
          }
           &.mats{
            &:before{
              background-image: url('/img/stockInfo/mats.svg');
            }
          }
          .title{
            font-weight: normal;
            font-size: .7rem;
            line-height: .8rem;
            letter-spacing: 0.02em;
            text-transform: uppercase;
            color: rgba(255, 255, 255, 0.5);
            margin-bottom: .25rem;
          }
          .value{
            font-weight: bold;
            font-size: 1rem;
            line-height: 1.15rem;
            letter-spacing: 0.02em;
            text-transform: uppercase;
            color: #FFFFFF;
          }
          &:after{
            content: '';
            width: 20.25rem;
            height: .1rem;
            background: linear-gradient(90deg, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%);
            position: absolute;
            left: 0;
            bottom: 0;
          }
        }
      }
    }

  }
</style>
