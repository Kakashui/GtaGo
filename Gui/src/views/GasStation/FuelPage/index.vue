<template>
  <div class="fuel-page">
    <div class="fuel-page__title">{{loc('gasStation_1')}}</div>
    <div class="fuel-page__fuel-list">
      <fuel-item 
        v-for="item in fuelTypes"
        :key="item.key"
        :item="item"
        @setPrice="setPrice"
      />
    </div>
    <div class="fuel-page__info">
      <div class="fuel-page__info-count">
        <div class="title">{{loc('gasStation_2')}}</div>
        <div class="desc">{{loc('gasStation_3')}}</div>
        <range-slider
          class="slider"
          min="0"
          :max="maxLiters - currentLiters"
          step="1"
          v-model.number="liters"
        />
        <div class="fuel-choice">
          <div class="inputs-list">
            <field v-model="liters" :text="'L'" />
            <field v-model="price" :text="'$'" />
          </div>
          <div class="btn btn-full" @click="setFullLiters">full</div>
        </div>
      </div>
      <div class="fuel-page__info-pay">
        <div class="info-block">
          <div
            class="desc"
            v-if="currentFuel.key === 'electro'"
          >{{loc('gasStation_4')}}</div>
          <div
            class="desc"
            v-else
          >{{loc('gasStation_5')}}</div>
          <div 
           :class="[{ electro: currentFuel.key === 'electro' }, { liters: currentFuel.key !== 'electro' }, 'value']"
          >{{currentLiters + liters}}/{{maxLiters}}</div>
        </div>
        <div class="info-block">
          <div class="desc">{{loc('gasStation_6')}}</div>
          <div class="value cash">{{totalCost}}</div>
        </div>
        <div
          class="btn btn-pay" 
          v-if="liters > 0"
          @click="goToPay"
        >{{loc('gasStation_7')}}</div>
        <div
          class="btn btn-locked"
          v-else
        >{{loc('gasStation_7')}}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapState } from 'vuex'

import RangeSlider from 'vue-range-slider'
import 'vue-range-slider/dist/vue-range-slider.css'

import FuelItem from './common/FuelItem'
import Field from './common/FuelPageField'

export default {
  name: "FuelPage",

  components: {
    FuelItem,
    Field,
    RangeSlider
  },

  data: function() {
    return {
      liters: null,
      price: null,
      test: 1
    }
  },

  computed: {
    ...mapState('gasStation', ['fuelTypes', 'currentFuel', 'currentLiters', 'maxLiters']),
    ...mapGetters('localization', ['loc']),
    totalCost: function() {
      return this.liters * this.currentFuel.cost
    },
  },

  methods: {
    ...mapMutations('gasStation', ['setCurrentPage', 'setCurrentFuel']),
    setPrice: function(value) {
      this.price = value
    },
    goToPay: function() {
      this.setCurrentPage({page: 'PayPage', data: this.liters})
    },
    setFullLiters: function() {
      this.liters = this.maxLiters - this.currentLiters
    }
  },

  watch: {
    liters: function(value) {
      this.price = value * this.currentFuel.cost
    },
    price: function(value) {
      this.liters = value / this.currentFuel.cost
    }
  },

  mounted: function() {
    const defaultValue = 0
    this.liters = defaultValue
  },

  created() {
    this.setCurrentFuel(this.fuelTypes[0])
  },
}
</script>

<style lang="scss" scoped>
.fuel-page{
  display: flex;
  flex-flow: column;
  padding-top: 1rem;
  &__title{
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.5rem;
    color: rgba(255, 255, 255, 0.7);
    margin-bottom: 1.75rem;
  }
  &__fuel-list{
    display: grid;
    grid-template-columns: repeat(5, 7.1rem);
    grid-gap: .8rem;
    grid-auto-rows: 7.95rem;
    margin-bottom: 4rem;

  }
  &__info{
    display: flex;
    justify-content: space-between;
    &-count, &-pay{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
    }
    &-count{
      width: 24.45rem;
      min-width: 24.45rem;
      padding-right: 4rem;
      border-right: 1px solid rgba(255, 255, 255, 0.3);
      margin-right: 2rem;
      .title{
        font-weight: bold;
        font-size: 2rem;
        line-height: 2rem;
        color: #FFFFFF;
        margin-bottom: .5rem;
      }
      .desc{
        font-weight: bold;
        font-size: 1rem;
        line-height: 1rem;
        color: rgba(255, 255, 255, 0.3);

      }
      .fuel-choice{
        display: flex;
        align-items: center;
        width: 100%;  
        justify-content: space-between;
        .inputs-list{
          display: flex;
          width: 100%;
          justify-content: space-between;
          margin-right: 2.5rem;
        }
        .btn-full{
          font-weight: bold;
          font-size: 1.1rem;
          color: #DCFF00;
          padding: 0 .8rem;
          height: 2.15rem;
          border-radius: .1rem;
          border: 1px solid #DCFF00;
          &:hover{
            transform: scale(1.1);
          }
          &:active{
            transform: scale(1.05);
          }
        }
      }
      .slider {
        width: 100%;
        margin: 1.9rem 0 1rem 0;
        height: 1rem;
      }
    }
    &-pay{
      width: 100%;
      .info-block{
        display: flex;
        flex-flow: column;
        margin-bottom: .6rem;
        .desc{
          font-size: 1.5rem;
          line-height: 1.5rem;
          color: rgba(255, 255, 255, 0.4);
          margin-bottom: .2rem;
          font-weight: 300;
        }
        .value{
          font-size: 1.5rem;
          line-height: 1.8rem;
          padding-left: 1.4rem;
          display: flex;
          align-items: flex-start;
          position: relative;
          &:before{
            content: '';
            width: 1.2rem;
            height: 1.2rem;
            position: absolute;
            left: 0;
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
          }
          &.cash{
            &:before{
              background-image: url('/img/gasStation/cash.svg');
            }
          }
          &.liters{
            &:before{
              background-image: url('/img/gasStation/drop.svg');
            }
          }
          &.electro{
            &:before{
              background-image: url('/img/gasStation/electro-icon.svg');
            }
          }
        }
      }
      .btn-pay, .btn-locked{
        margin-top: .4rem;
        border-radius: .1rem;
        padding: .5rem 2.3rem;
        font-size: 1.1rem;
      }
      .btn-pay{
        background: #C1E704;
        &:hover{
          box-shadow: 0 .1rem .8rem #DCFF00;
        }
      }
      .btn-locked{
        color: rgba(199, 199, 201, 0.3);
        background: rgba(255, 255, 255, 0.1);
      }
    }
  }
}
</style>

<style lang="scss">
.gas-station{
  .slider {
    width: 100%;
    margin: 1.9rem 0 1rem 0;
    height: 1rem;
    &.range-slider{
      padding: 0;
    }
    .range-slider-knob{
      width: 1rem;
      height: 1rem;
    }
    .range-slider-fill{
      background: #DCFF00;
      border: 1px solid #37393C;
      height: 100%;
      border-radius: 2rem 0 0 2rem;
    }
    .range-slider-rail{
      background: #292929;
      border: 1px solid #37393C;
      height: 100%;
      border-radius: 2rem;
    }
  }
}
</style>
