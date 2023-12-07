<template>
  <div :class="[{ active: isSelect }, 'main__item']" @click="toggleIsSelect">
    <span class="item__title">{{ loc(item.title) }}:</span>
    <div class="item__value">
      <span class="value__text">{{ item.currentValue }}</span>
      <svg fill="#FFF" :class="[{ active: isSelect }, 'value__icon']" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
        viewBox="0 0 451.847 451.847" style="enable-background:new 0 0 451.847 451.847;"
        xml:space="preserve">
      <g>
        <path d="M225.923,354.706c-8.098,0-16.195-3.092-22.369-9.263L9.27,151.157c-12.359-12.359-12.359-32.397,0-44.751
          c12.354-12.354,32.388-12.354,44.748,0l171.905,171.915l171.906-171.909c12.359-12.354,32.391-12.354,44.744,0
          c12.365,12.354,12.365,32.392,0,44.751L248.292,345.449C242.115,351.621,234.018,354.706,225.923,354.706z"/>
      </g>
      </svg>
    </div>
    <transition name="slide-fade">
      <div class="item__select" v-if="isSelect && item.id === 0 && currentPlayers == 2 && (currentMode === 'Gun game' || currentMode === 'Team fight' || currentMode === 'Death match')">
        <span
          class="select__item"
          v-for="(value, index) in item.values[2]"
          :key="index"
          @click="setCurrentValue({ id: item.id, value })"
        >{{ value }}</span>
      </div>
      <div class="item__select" v-else-if="isSelect && item.id === 0 && (currentMode === 'Gun game' || currentMode === 'Death match')">
        <span
          class="select__item"
          v-for="(value, index) in item.values[0]"
          :key="index"
          @click="setCurrentValue({ id: item.id, value })"
        >{{ value }}</span>
      </div>
      <div class="item__select" v-else-if="isSelect && item.id === 0 && currentMode === 'Team fight'">
        <span
          class="select__item"
          v-for="(value, index) in item.values[1]"
          :key="index"
          @click="setCurrentValue({ id: item.id, value })"
        >{{ value }}</span>
      </div>
      <div class="item__select" v-else-if="isSelect">
        <span
          class="select__item"
          v-for="(value, index) in item.values"
          :key="index"
          @click="setCurrentValue({ id: item.id, value })"
        >{{ value }}</span>
      </div>
    </transition>
  </div>
</template>

<script>
import { mapState, mapGetters, mapMutations } from 'vuex'

export default {
  name: 'FiltersItems',

  props: [
    'item',
    'currentMode',
    'currentPlayers'
  ],

  data: function () {
    return {
      isSelect: false
    }
  },

  computed: {
    ...mapState('arenaMenu', ['filters']),

    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('arenaMenu', ['setFiltersCurrentValue']),

    ...mapMutations('sounds', ['play']),

    setCurrentValue: function (value) {
      const mapsArray = this.filters[0].values

      if ((this.item.id == 1 && value.value == 2) && (this.currentMode === 'Team fight' || this.currentMode === 'Gun game' || this.currentMode === 'Death match')) {
        this.setFiltersCurrentValue({ id:0, value: mapsArray[2][0] })
      } else if ((this.item.id == 4 && value.value === 'Team fight' && this.currentPlayers != 2) || ((this.item.id == 1 && value.value > 2) && this.currentMode === 'Team fight')) {
        this.setFiltersCurrentValue({ id:0, value: mapsArray[1][0] })
      } else if ((this.item.id == 4 && value.value === 'Gun game' && this.currentPlayers != 2) || ((this.item.id == 1 && value.value > 2) && this.currentMode === 'Gun game')) {
        this.setFiltersCurrentValue({ id:0, value: mapsArray[0][0] })
      }
      else if ((this.item.id == 4 && value.value === 'Death match' && this.currentPlayers != 2) || ((this.item.id == 1 && value.value > 2) && this.currentMode === 'Death match')) {
          this.setFiltersCurrentValue({ id:0, value: mapsArray[0][0] })
      }

      this.play({ name: 'switch', volume: 0.1 })
      this.setFiltersCurrentValue(value)
    },

    setIsSelect: function (value) {
      this.isSelect = value
    },

    toggleIsSelect: function () {
      this.isSelect = !this.isSelect
      this.play({ name: 'switch', volume: 0.1 })
    }
  }
}
</script>

<style lang="scss" scoped>
.main__item {
  display: flex;
  align-items: center;
  font-size: 1.15rem;
  letter-spacing: 1px;
  margin: .2rem 0 0 0;
  padding-top: 1.1rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.2);
  width: 100%;
  position: relative;
  //transition: .15s all ease;
  font-size: .9rem;
  &.active {
    background-color: rgba(255, 255, 255, 0.4);
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }
  &:hover {
    //background: rgba(255, 255, 255, 0.5);
  }
  &:first-child {
    margin: 0;
  }
  .item__title {
    color: #FFF;
    text-transform: uppercase;
  }
  .item__value {
    display: flex;
    align-items: center;
    padding: 0 0 0 .5rem;
    width: 100%;
    .value__text {
      font-weight: bold;
      color: #B6D300;
      text-transform: uppercase;
    }
    .value__icon {
      width: 1rem;
      margin: 0 0 0 auto;
      transition: .2s all ease;
      &.active {
        transform: rotate(180deg);
      }
    }
  }
  .item__select {
    position: absolute;
    top: 105%;
    right: 0;
    width: 100%;
    display: flex;
    flex-direction: column;
    z-index: 1;
    font-weight: bold;
    text-transform: uppercase;
    background-color: rgba(255, 255, 255, 0.4);
    //background-color: #FFF;
    
    border-radius: .75rem;
    border-top-left-radius: 0;
    border-top-right-radius: 0;
    color: #FFF;
    overflow: hidden;
    .select__item {
      padding: .6rem 0;
      text-align: center;
      transition: all .15s ease;
      &:hover {
        background-color:  rgba(255, 255, 255, 0.5);
        color: #B6D300;
      }
    }
  }
}
.slide-fade-enter-active, .slide-fade-leave-active {
  transition: all .3s ease;
}

.slide-fade-enter, .slide-fade-leave-to {
  transform: translateY(-20px);
  opacity: 0;
}
</style>
