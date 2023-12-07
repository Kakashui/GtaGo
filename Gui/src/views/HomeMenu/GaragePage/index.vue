<template>
  <div class="garage-page">
    <Head :titleText="loc('home:garage:head')"/>
    <div class="header">
      <div class="header__info">
        <div class="info-item">
          <div class="value">{{totalVehicles}}</div>
          <div class="desc">{{loc('HomeMenu_12')}}</div>
        </div>
      </div>
    </div>
    <div class="scroll">
      <div class="body">
        <div
          class="body__item"
          v-for="item in typeOfGarages"
          :key="item.id"
        >
          <div class="desc"></div>
          <div class="title">{{loc(item.desc)}}</div>
          <div class="icon-done" v-if="item.type === currentGarage.type"></div>
          <div 
            class="btn btn-buy"
            @click="buyGarage(item.type)"
          >{{loc('HomeMenu_13')}} ${{item.cost}}</div>
          <div class="img"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import Head from '../components/Head'

export default {
  name: 'GaragePage',

  components: {
    Head
  },

  computed: {
    ...mapState('homeMenu', ['totalVehicles', 'typeOfGarages', 'currentGarage', 'houseId']),
    ...mapGetters('localization', ['loc']),
  },

  methods: {
    buyGarage: function(key) {
      window.mp.trigger('homeMenu:buyGarage', this.houseId, key);
    }
  }
}
</script>

<style lang="scss" scoped>
.garage-page{
  width: 100%;
  padding: 2.85rem 2.85rem 0 2.85rem;
  display: flex;
  flex-flow: column;
  height: 100%;
  position: relative;
  &:after{
    content: '';
    width: 100%;
    height: 38rem;
    position: absolute;
    right: 0;
    bottom: 0;
    background-image: url('/img/homeMenu/garage-bg.png');
    background-position: bottom right;
    background-size: cover;
    background-repeat: no-repeat;
    z-index: 4;
  }
  .header{
    display: flex;
    align-items: flex-start;
    margin: 2rem 0;
    position: relative;
    &__info{
      display: flex;
      .info-item{
        position: relative;
        padding: 0 2rem;
        display: flex;
        flex-flow: column;
        position: relative;
        &:first-child{
          padding-left: 0;
        }
        &:last-child{
          &:after{
            content: none;
          }
        }
        &:after{
          content: '';
          background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.45) 0%, rgba(255, 255, 255, 0) 100%);
          height: 100%;
          width: 1px;
          position: absolute;
          right: 0;
          top: 0;
        }
        .value{
          font-size: 3.6rem;
          line-height: 4.3rem;
        }
        .desc{
          font-size: 1.2rem;
          line-height: 1.45rem;
          color: rgba(255, 255, 255, 0.45);
          margin-top: .3rem;
        }
      }
    }
    &__nav{
      &-item{
        margin-right: .9rem;
        &:last-child{
          margin-right: 0;
        }
        &.nav-edit{
          &:before{
            background-image: url('/img/homeMenu/icon-edit.svg');
          }
        }
        &.nav-remove{
          &:before{
            background-image: url('/img/homeMenu/icon-remove.svg');
          }
        }
      }
    }
  }
  .scroll{
    width: 100%;
    display: flex;
    flex-flow: column;
    overflow-y: auto;
    height: 100%;
    padding-bottom: 5.85rem;
    z-index: 5;
    &::-webkit-scrollbar{
      width: .3rem;
      background: transparent;
    }
    &::-webkit-scrollbar-thumb{
      background: #A9ABAE;
      border-radius: 1rem;
    }
  }
  .body{
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    grid-auto-rows: 9.8rem;
    grid-gap: 2.2rem;
    width: 100%;
    padding-right: 4rem;
    &__item{
      display: flex;
      flex-flow: column;
      justify-content: flex-start;
      align-items: flex-start;
      padding: 1.6rem 1.45rem;
      background: rgba(255, 255, 255, 0.05);
      position: relative;
      transition: .2s;
      &>div{
        z-index: 3;
      }
      &.item-lock{
        &:before{
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(255, 64, 64, 0.15) 0%, rgba(255, 64, 64, 0) 100%), rgba(255, 255, 255, 0.05);
        }
        &:after{ 
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(255, 64, 64, 0.45) 0%, rgba(255, 64, 64, 0) 100%), rgba(255, 255, 255, 0.05);
        }
      }
      &.item-unlock{
        &:before{
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(193, 231, 4, 0.15) 0%, rgba(193, 231, 4, 0) 100%), rgba(255, 255, 255, 0.05);
        }
        &:after{ 
          background: radial-gradient(66.58% 66.58% at 50% 26.79%, rgba(193, 231, 4, 0.35) 0%, rgba(193, 231, 4, 0) 100%), rgba(255, 255, 255, 0.05);
        }
      }
      &.item-colored{
        .title{
          color: #C1E704;
        }
      }
      .desc{
        font-size: 1.05rem;
        line-height: 1.25rem;
        white-space: pre;
      }
      .title{
        font-size: 2.05rem;
        line-height: 2.45rem;
        color: #C1E704;
      }
      .cost{
        margin-top: 1.15rem;
        display: flex;
        align-items: center;
        padding-left: 1.4rem;
        font-size: 1.05rem;
        line-height: 1.25rem;
        letter-spacing: 0.05em;
        color: #C1E704;
        position: relative;
        &:before{
          content: '';
          width: 1rem;
          height: 1rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          background-image: url('/img/homeMenu/cost-icon.svg');
          position: absolute;
          left: 0;
        }
      }
      .icon-done{
        display: flex;
        justify-content: center;
        align-items: center;
        width: 1.2rem;
        height: 1.4rem;
        position: absolute;
        top: 1.5rem;
        right: 1.25rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/homeMenu/done.svg');
        &:after{
          content: '';
          position: absolute;
          width: 50%;
          height: 50%;
          background-color: rgba(193, 231, 4, 0.65);
          filter: blur(.3rem);
        }
      }
      .btn{
        position: absolute;
        left: 1.5rem;
        bottom: 1.5rem;
        width: 2.35rem;
        height: 2.35rem;
        opacity: 0;
        padding: .85rem;
        white-space: pre;
        font-weight: bold;
        font-size: 1.05rem;
        color: #2B3400;
        min-width: 10.5rem;
        background: #C1E704;
        transition: .2s;
      }
      .img{
        background-size: cover;
        background-repeat: no-repeat;
        background-position: bottom;
        background-image: url('/img/homeMenu/garage-item-bg.svg');
        position: absolute;
        height: 100%;
        width: 100%;
        bottom: 0;
        right: 0;
        z-index: 2;
      }
      &:before, &:after{
        content: '';
        width: 100%;
        height: 100%;
        position: absolute;
        border: 1px solid;
        border-image-slice: 1;
        top: 0;
        left: 0;
        z-index: 1;
        box-sizing: border-box;
        transition: .2s;
      }
      &:before{
        border-image-source: linear-gradient(to bottom, rgba(255,255,255, .15), rgba(255,255,255,0));
        opacity: 1;
      }
      &:after{ 
        background: radial-gradient(75.92% 75.92% at 50% 12.89%, rgba(255, 255, 255, 0.25) 0%, rgba(255, 255, 255, 0) 100%), rgba(255, 255, 255, 0.05);
        border-image-source: linear-gradient(to bottom, rgba(255,255,255, .65), rgba(255,255,255,0));
        opacity: 0;
      }
      &:hover{
        .btn{
          opacity: 1;
          transition: .2s;
        }
        &:before, &:after{
          transition: .2s;
        }
        &:before{
          opacity: 0;
        }
        &:after{
          opacity: 1;
        }
      }
    }
  }
}
</style>
