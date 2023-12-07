<template>
  <div class="fishing-store">
    <div 
      class="fishing-store__btn-exit"
      @click="exitMenu"
    >{{loc('Fishing_store_0')}}</div>
    <div class="fishing-store__list">
      <div 
        class="fishing-store__list-item"
        v-for="(item, key) in cageItems"
        :key="key"
      >
        <div>
          <div class="fishing-store__list-item-title">{{getName(key)}}</div>
          <div class="fishing-store__list-item-cost">$ {{getPrice(key)}}</div>
          <div
            class="fishing-store__list-item-btn"
            @click="sellFish(key)"
          >{{loc('Fishing_store_1')}}</div>
        </div>
        <div>
          <div class="fishing-store__list-item__img">
            <img 
              class="fishing-store__list-item__img-fish"
              :src="`/img/fish/${key}.png`"
            />
            <div class="fishing-store__list-item-count">{{item}} {{loc('gui_773')}}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import config from '../../configs/fishing/fishShop'

export default {
  name: 'FishingStore',
  computed: {
    ...mapState('fishingStore', ['cageItems']),
    ...mapGetters('localization', ['loc'])
  },
  methods: {
    exitMenu() {
      window.mp.trigger("guiClose")
    },
    sellFish(key) {
      window.mp.triggerServer('fshCellFish', key);
    },
     getName(key){
      return this.loc(config[key].name);
    },
    getPrice(key){
      return config[key].price.toLocaleString();
    }
  }
}
</script>

<style lang="scss">
.fishing-store{
  width: 27.25rem;
  height: 100vh;
  position: absolute;
  right: 0;
  top: 0;
  background: linear-gradient(270deg, rgba(255, 255, 255, 0.26) 0%, rgba(255, 255, 255, 0) 100%);
  padding: 12.7rem 1rem 0 0;
  text-transform: uppercase;
  color: #fff;
  font-weight: bold;
  &:before, &:after{
    content: '';
    position: absolute;
    background-size: contain;
    background-repeat: no-repeat;
    pointer-events: none;
  }
  &:before{
    width: 21.25rem;
    height: 18rem;
    top: 0;
    right: 0;
    background-position: right top;
    background-image: url('/img/fishingStore/decorate.png');
    z-index: 0;
    mix-blend-mode: multiply;
  }
  &:after{
    width: 9.6rem;
    height: 8.2rem;
    top: 2rem;
    right: 3.5rem;
    background-position: center;
    background-image: url('/img/fishingStore/logo.png');
    z-index: 1;
  }
  &__btn-exit{
    display: flex;
    align-items: center;
    justify-content: flex-end;
    font-size: 1rem;
    margin-top: 1rem;
    z-index: 2;
    text-shadow: -.1rem .1rem .2rem rgba(#000,.3);
    &:before{
      content: '';
      width: .85rem;
      height: .7rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      background-image: url('/img/fishingStore/arrow.png');
      margin-right: .85rem;
      transition: .3s;
    }
    &:hover{
      &:before{
        transform: translateX(-30%);
      }
    }
  }
  &__list{
    height: calc(100vh - (17rem));
    overflow-y: auto;
    padding-right: 2.5rem;
    margin-top: 3rem;
    &::-webkit-scrollbar{
      background: transparent;
      width: .3rem;
      &-thumb{
        background: rgba(255, 255, 255, 0.4);
        border-radius: .1rem;
      }
    }
    &-item{
      display: flex;
      align-items: center;
      justify-content: flex-end;
      letter-spacing: 0.05em;
      margin-bottom: 2.5rem;
      &>div{
        display: flex;
        flex-flow: column;
        align-items: flex-end;
        justify-content: center;
        &:nth-child(1){
          margin-right: 2.5rem;
        }
      }
      &-title{
        font-size: 1.2rem;
        line-height: 1.2rem;
        margin-bottom: .45rem;
        text-shadow: -.1rem .1rem .2rem rgba(#000,.3);
      }
      &-cost{
        font-size: 1rem;
        line-height: 1rem;
        color: #C1E704;
        margin-bottom: .85rem;
        text-shadow: -.1rem .1rem .2rem rgba(#000,.3);
      }
      &-btn{
        font-size: .8rem;
        padding: 1rem 2rem;
        background: #3C6091;
        transition: .2s;
        &:hover{
          transform: scale(1.1);
        }
        &:active{
          transform: scale(1.05);
        }
      }
      &__img{
        display: flex;
        align-items: center;
        justify-content: center;
        width: 7.5rem;
        height: 7.5rem;
        min-width: 7.5rem;
        position: relative;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/fishingStore/item-bg.png');
        img{
          width: 10rem;
          position: absolute;
          transform-origin: 50% 50%;
          transform: rotate(-40deg);
        }
      }
      &-count{
        width: 2.5rem;
        height: 2.5rem;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 50%;
        color: #000000;
        background: url(/img/fishingStore/item-bg.png) no-repeat center;
        background-size: contain;
        
        //background: #A3A4B8;        
        font-size: .8rem;
        text-transform: none;
        position: absolute;
        left: -.6rem;
        bottom: .55rem;
        border: .15rem solid #333;
        box-shadow: 0 0 0 .2rem rgba(#fff, .5);
      }
    }
  }
}
</style>
