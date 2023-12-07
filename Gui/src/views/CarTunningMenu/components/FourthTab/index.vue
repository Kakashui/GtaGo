<template>
  <div class="car-tunning__fourth">
    <span class="title">{{ loc(fourthTabData.title) }}</span>
    <button-back :tab="'ThirdTab'" />
    <div class="fourth__price">
      <span class="price__title">{{ loc('car_tunning_2') }}</span>
      <span class="price__value">${{ currentItemPrice }}</span>
      <button
        @mouseenter="play({ name: 'hover', volume: .4 })"
        class="price__btn"
        :disabled="!isBuyEnable"
        @click="buy"
      >{{ loc('car_tunning_10') }}</button>
    </div>
    <div class="fourth__list" :style="{ left: appWidth, height: appWidth }">
      <list-item
        v-for="item in fourthTabData.items"
        :key="item.key"
        :item="item"
        :parent="fourthTabData.parent"
      />
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'

import ListItem from './ListItem'
import ButtonBack from '../ButtonBack'

export default {
  name: 'FourthTab',

  components: { ListItem, ButtonBack },

  computed: {
    ...mapState('carTunningMenu', [
      'fourthTabData', 
      'isBuyEnable',
      'currentItemPrice'
      ]),
    ...mapGetters('localization', ['loc']),

    appWidth: function () {
      return document.getElementById('app').offsetWidth + 'px'
    },
  },
  
  methods: {
    ...mapMutations('sounds', ['play']),
    buy: function() {
      window.mp.trigger('lsCustom:buyTuning')
    }
  }
}
</script>

<style lang="scss" scoped>
.car-tunning__fourth {
  width: 100%;
  height: 100%;
  position: relative;
  .title {
    position: absolute;
    left: 2.8rem;
    bottom: 13.25rem;
    font-size: 2.05rem;
    color: #fff;
  }
  .fourth__price {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    position: absolute;
    top: 21.95rem;
    right: 2.15rem;
    .price__title {
      font-size: 1.05rem;
      font-weight: bold;
      color: rgba(255, 255, 255, 0.65);
      letter-spacing: 0.15em;
      margin: 0 0 0.4rem 0;
    }
    .price__value {
      font-size: 2.4rem;
      line-height: 2.9rem;
      letter-spacing: 0.05em;
      color: #C1E704;
      margin: 0 0 0.7rem 0;
      
    }
    .price__btn {
     text-transform: uppercase;
      letter-spacing: 0.05em;
      margin: 0 0 0 .5rem;
      width: 5.6rem;
      height: 2rem;
      border-radius: 11.1rem;
      background: #C1E704;
      font-size: 0.9rem;
      color: #1F2500;
      transition: all 0.15s;
      font-weight: bold;
      &:hover {
        background: #1F2500;
        color: #C1E704;
      }
      &:disabled {
        background: #4E4E4E;
        color: #757575;
      }
    }
  }
  .fourth__list {
    position: absolute;
    bottom: 10.5rem;
    // left: 74.4rem;
    width: 10rem;
    // height: 72rem;
    overflow-y: auto;
    overflow-x: hidden;
    transform: rotate(90deg) scale(1, -1);
    transform-origin: bottom left;
    &::-webkit-scrollbar-track {
      background-color: transparent;
      margin: 1.5rem 0;
    }
    &::-webkit-scrollbar {
      width: 0.2rem;
    }
    &::-webkit-scrollbar-thumb {
      background-color: #FFFFFF;
      border-radius: 0.2rem;
    }
  }
}
</style>