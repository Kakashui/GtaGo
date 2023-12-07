<template>
  <div v-show="dataLoaded" class="frisk-content">
    <div class="header">
      <div style="display: block; width: -webkit-fill-available;">
        <div class="header__info">
          <div class="info-name">{{ playerName }}
          </div>
          <div class="info-id">ID: {{ playerId }}</div>
        </div>
        <div class="green-border"></div>
      </div>
      <div class="header__close">
        <svg @click="exit" @mouseenter="closeHovered = true" @mouseleave="closeHovered = false" class="btn-close" width="22" height="22" viewBox="0 0 22 22" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path class="btn-close-hover" d="M19.75 2.25L2.25 19.75M19.75 19.75L2.25 2.25L19.75 19.75Z" :stroke=" closeHovered ? '#DCFF00' : 'white'" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
      </div>
    </div>
    <div class="content">
      <div class="content__text">{{ items.length > 0 ? loc('founded-illegal') : loc('not-founded-illegal') }}</div>
      <div v-if="items.length > 0" class="item-block" :style="{ 'grid-row-gap': fixGrid ? '0vh' : '1.8518518518518519vh' }">
        <div 
            class="item-block__item" 
            v-for="(item, index) in items" 
            :key="index"
            :style="{ background: item.id != '-1' ? 'radial-gradient(80.56% 80.56% at 50% 76.11%, rgba(255, 255, 255, 0.15) 0%, rgba(255, 255, 255, 0) 100%)' : 'rgba(255, 255, 255, 0.03)' }"
        >
          <div class="item-bg" :style="{ 'background-image': item.id != '-1' ? `url(${getImage(item.id)})` : 'none' }">
            <div class="item-count" v-if="isStackable(item.id)">x{{ item.count }}</div>
          </div>
        </div>
      </div>
      <div class="buttons">
        <div v-if="dataLoaded && showButton" @click="takeAll" class="buttons__btn-take">{{ loc('btn-take-illegal') }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import configs from '../../configs/inventory/configs'

import { mapGetters, mapState } from 'vuex'

export default {
  name: 'FriskInterface',

  components: {
    
  },

  data: function() {
    return {
      showButton: false,
      fixGrid: true,
      closeHovered: false
    }
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('friskInterface', [
      'items',
      'playerName',
      'playerId',
      'dataLoaded'
    ])
  },

  mounted() {
    console.log(this.items.length + ' ' + this.items.length % 8)
    if(this.items.length <= 24) {
      if(this.items.length > 0) {
        this.showButton = true;
      }
      while(this.items.length != 24) {
        this.items.push( { id: '-1', count: '-1'} )
      }
    }
    else {
      this.showButton = true;
      this.fixGrid = false
    }
    if(this.items.length % 8 != 0) {
      while(this.items.length % 8 != 0) {
        this.items.push( { id: '-1', count: '-1'} )
      }
    }

  },

  methods: {
    exit() {
      window.mp.trigger("friskInterface:close")
    },
    takeAll() {
      window.mp.trigger("friskInterface:takeAll")
    },
    getImage(id) {
      return `/img/inventory/items/${configs[id].Image}.png`
    },
    isStackable(id) {
      if(id == '-1') return false;
      return configs[id].Stackable
    }
  }
}
</script>

<style lang="scss" scoped>

.frisk-content {
  width: 92.5925925925926vh;
  height: 71.20370370370371vh;

  background: #121217;
  background-image: url(/img/friskInterface/bg.png);
  background-repeat: no-repeat;
  background-position-x: right;
  background-position-y: bottom;
  background-size: 20vh;
  filter: drop-shadow(0px 0px 250px rgba(128, 183, 200, 0.2));

  padding: 6.481481481481482vh;
  .header {
    display: flex;
    .green-border {
      width: 10.462962962962964vh;
      height: 0.37037037037037035vh;

      background: linear-gradient(90deg, #DCFF00 0%, rgba(0, 0, 0, 0) 100%);

      margin-top: 1vh;
    }
    &__info {
      display: flex;
      .info-name {
        font-style: normal;
        font-weight: normal;
        font-size: 5.555555555555555vh;

        color: #FFFFFF;

        line-height: 5vh;
      }
      .info-id {
        font-style: normal;
        font-weight: normal;
        font-size: 2.037037037037037vh;

        color: #606060;

        align-self: flex-end;

        margin-left: 1vh;
      }
    }
    &__close {
      width: -webkit-fill-available;
      text-align: right;
      font-size: 1vh;
      .btn-close {
        width: 2vh;
        height: 2vh;

        .btn-close-hover {
          transition: all 0.2s ease-in-out;
        }
      }
    }
  }
  .content {
    &__text {
      margin-top: 7.407407407407407vh;
      font-style: normal;
      font-weight: normal;
      font-size: 3.7037037037037037vh;

      color: #FFFFFF;
    }
    .buttons {
      margin-top: 3.7037037037037037vh;
      &__btn-take {
        transition: all 0.1s ease-in-out;
        width: 33.888888888888886vh;
        height: 6.203703703703703vh;

        background: #C1E704;

        text-transform: uppercase;
        font-style: normal;
        font-weight: bold;
        font-size: 2.2222222222222223vh;
        text-align: center;
        line-height: 6.2vh;
        color: #FFFFFF;

        &:hover {
          box-shadow: 0px 0px 16px #DCFF00;
        }
      }
    }
    
    .item-block {
      min-height: 30vh;
      max-height: 30vh;
      min-width: 79.62962962962963vh;
      max-width: 79.62962962962963vh;
      margin-top: 0.9259259259259259vh;
      overflow: overlay;
      display: grid;
      grid-template-columns: repeat(8, 10.185185185185185vh);
      flex-wrap: wrap;
      &::-webkit-scrollbar {
        display: none;
      }
      &__item {
        width: 8.333333333333334vh;
        height: 8.333333333333334vh;
        border: 2px solid rgba(255, 255, 255, 0.1);
        img{
            width: 70%;
            height: 70%;
            -webkit-user-drag: none;
            -o-user-drag: none;
            pointer-events: none;
        }
        .item-bg {
          height: inherit;
          padding-left: 0.9259259259vh;
          background-repeat: no-repeat;
          background-position: center;
          background-size: 5.555555555555555vh;
        }
        .item-count{
          font-style: normal;
          font-weight: normal;
          font-size: 1.6666666666666667vh;
          color: #fff;
        }
      }
    }
  }
}

</style>