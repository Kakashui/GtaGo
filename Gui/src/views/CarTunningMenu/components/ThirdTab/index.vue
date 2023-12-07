<template>
  <div class="car-tunning__third">
    <span class="title">{{ loc(thirdTabData.title) }}</span>
    <button-back :tab="'SecondTab'" />
    <div v-if="secondTabData.parent === 'painting'" class="third__palettes">
      <color-picker
        v-if="thirdTabData.parent == 'Color'"
        :title="loc('car_tunning_3')"
        :type="'SecColor'"
        :initialColor="thirdTabData.colorPickerAdditionalColor"
      />
      <color-picker
        v-if="thirdTabData.parent == 'Color'"
        :title="loc('car_tunning_4')"
        :type="'Color'"
        :initialColor="thirdTabData.colorPickerMainColor"
      />
      <color-picker
        v-if="thirdTabData.parent == 'Neon'"
        :title="loc('car_tunning_3')"
        :type="'Neon2'"
        :initialColor="thirdTabData.colorPickerNeon2Color"
      />
      <color-picker
        v-if="thirdTabData.parent == 'Neon'"
        :title="loc('car_tunning_4')"
        :type="'Neon'"
        :initialColor="thirdTabData.colorPickerNeonColor"
      />
      <color-picker
        v-if="thirdTabData.parent == 'TyreSmokeColor'"
        :title="loc('car_tunning_4')"
        :type="'TyreSmokeColor'"
        :initialColor="thirdTabData.colorPickerTyreSmokeColor"
      />
      <color-palette v-if="thirdTabData.parent == 'WheelsColor'" :title="loc('car_tunning_5')" />
      <color-palette v-if="thirdTabData.parent == 'Color'" :title="loc('car_tunning_6')" />
    </div>
    <specifications v-if="specifications && secondTabData.parent === 'power'" :specifications="specifications" />
    <div v-if="thirdTabData.parent !== 'FrontWheels' && thirdTabData.parent !== 'BackWheels'" class="third__price">
      <span class="price__title">{{loc('car_tunning_13')}}</span>
      <span class="price__value">${{ currentItemPrice }}</span>
      <button
        @mouseenter="play({ name: 'hover', volume: .4 })"
        class="price__btn"
        :disabled="!isBuyEnable"
        @click="buy"
      >{{loc('car_tunning_14')}}</button>
    </div>
    <div class="third__list" :style="{ left: appWidth, height: appWidth }">
      <list-item
        v-for="item in thirdTabData.items"
        :key="item.key"
        :item="item"
        :parent="thirdTabData.parent"
      />
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapGetters } from 'vuex'

import ListItem from './ListItem'
import ButtonBack from '../ButtonBack'
import ColorPicker from '../ColorPicker'
import ColorPalette from './ColorPalette'
import Specifications from '../SecondTab/Specifications'

export default {
  name: 'ThirdTab',

  components: {
    ListItem,
    ButtonBack,
    ColorPicker,
    ColorPalette,
    Specifications
  },

  computed: {
    ...mapState('carTunningMenu', [
      'thirdTabData',
      'secondTabData',
      'specifications',
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
.car-tunning__third {
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
  .third__palettes {
    position: absolute;
    top: 7.65rem;
    right: 2.15rem;
    display: flex;
    justify-content: flex-end;
  }
  .third__price {
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
  .third__list {
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
