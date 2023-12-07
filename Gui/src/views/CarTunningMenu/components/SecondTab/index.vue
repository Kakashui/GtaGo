<template>
  <div class="car-tunning__second">
    <specifications v-if="specifications && secondTabData.parent === 'power'" />
    <span class="title">{{ secondTabData.title }}</span>
    <button-back :tab="'FirstTab'" />
    <div class="second__list" :style="{ left: appWidth, height: appWidth }">
      <list-item
        v-for="item in secondTabData.items"
        :key="item.key"
        :item="item"
        :parent="secondTabData.parent"
      /> 
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'

import Specifications from './Specifications'
import ListItem from './Listitem'
import ButtonBack from '../ButtonBack'

export default {
  name: 'SecondaryTab',

  components: {
    ListItem,
    Specifications,
    ButtonBack
  },

  computed: {
    ...mapState('carTunningMenu', ['secondTabData', 'specifications']),

    appWidth: function () {
      return document.getElementById('app').offsetWidth + 'px'
    },
  }
}
</script>

<style lang="scss" scoped>
.car-tunning__second {
  width: 100%;
  height: 100%;
  display: flex;
  position: relative;
  .title {
    position: absolute;
    left: 2.8rem;
    bottom: 13.25rem;
    font-size: 2.05rem;
    color: #fff;
  }
  .second__list {
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
