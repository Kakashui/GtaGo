<template>
  <div class="car-tunning__first">
    <span class="title">{{loc('car_tunning_12')}}</span>
    <button class="btn">
      <img
        class="btn__icon"
        src="/img/carTunningMenu/arrow_left.svg"
        alt="arrow"
      >
      <span class="btn__text" @click="exitTunning">{{loc('car_tunning_11')}}</span>
    </button>
    <div class="list" :style="{ left: appWidth, height: appWidth }">
      <list-item
        v-for="item in fistTabItems"
        :key="item.key"
        :item="item"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

import ListItem from './GeneralListItem'

export default {
  name: 'GeneralTab',

  components: {
    ListItem
  },

  computed: {
    ...mapState('carTunningMenu', ['fistTabItems']),
    ...mapGetters('localization', ['loc']),
    appWidth: function () {
      return document.getElementById('app').offsetWidth + 'px'
    },
  },

  methods: {
    exitTunning: function() {
      window.mp.trigger('lsCustom:exitTun');
    }
  }
}
</script>

<style lang="scss" scoped>
.car-tunning__first {
  width: 100%;
  height: 100%;
  position: relative;
  // position: absolute;
  // bottom: 0.5rem;
  // left: 2.8rem;
  // display: flex;
  // flex-direction: column;
  .title {
    font-size: 2.05rem;
    line-height: 2.45rem;
    color: #FFFFFF;
    position: absolute;
    bottom: 13.25rem;
    left: 2.8rem;
  }
  .btn {
    display: flex;
    align-items: center;
    background: transparent;
    color: #FFFFFF;
   text-transform: uppercase;
    position: absolute;
    bottom: 10.85rem;
    left: 2.4rem;
    padding: 0.4rem;
    border-radius: 0.4rem;
    &:hover {
      background: rgba($color: #000000, $alpha: 0.3);
    }
    &__icon {
      width: 0.9rem;
      height: 0.9rem;
      margin: 0 0.5rem 0 0;
    }
    &__text {
      font-weight: bold;
      text-transform: uppercase;
      letter-spacing: 0.05em;
    }
  }
  .list {
    position: absolute;
    bottom: 10.5rem;
    // left: 96rem;
    width: 10rem;
    // height: 96rem;
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
