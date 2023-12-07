<template>
  <div class="rent-vehicle">
    <div class="rent-vehicle-title">{{loc('Rent_vehicle_0')}}</div>
    <div class="rent-vehicle__body">
      <div class="rent-vehicle__nav">
        <div
          :class="[{ active: cat === currentFilter }, 'rent-vehicle__nav-item']"
          v-for="cat in currentCategories"
          :key="cat"
          @click="setCurrentFilter(cat)"
        >
          <div 
            class="rent-vehicle__nav-item__icon"
            :style="{backgroundImage: `url(/img/rentVehicle/${getCategory(cat).Icon}.png)`}"
          ></div>
          <div class="rent-vehicle__nav-item__text">{{loc(getCategory(cat).Name)}}</div>
        </div>
      </div>
      <div class="rent-vehicle__body-scroll">
        <RentVehicleItem 
          v-for="item in filteredList"
          :key="item.id" 
          :item="item"
          :currentChoice="currentChoice"
          :category="currentFilter"
          @selectChoise="selectChoise"
        />
      </div>
    </div>
  </div>
</template>

<script>
import categories from '../../configs/vehicleRent/categories'
import { mapState, mapGetters } from 'vuex'
import RentVehicleItem from './RentVehicleItem'
export default {
  name: 'RentVehicle',
  components: {
    RentVehicleItem
  },
  data() {
    return {
      currentChoice: null,
      currentFilter: null
    }
  },
  computed: {
    ...mapState('rentVehicle', ['categoriesPoint']),
    ...mapGetters('localization', ['loc']),
    filteredList() {
      if (this.getCategory(this.currentFilter))
        return this.getCategory(this.currentFilter).Cars;
      return [];
    },
    currentCategories() {
      return this.categoriesPoint.slice().filter(item => categories[item] !== undefined)
    }
  },
  mounted() {
    this.currentFilter = this.categoriesPoint[0]
  },
  methods: {
    setCurrentFilter(value) {
      if (this.currentFilter !== value) {
        this.currentFilter = value
        this.currentChoice = null
      }
    },
    selectChoise(model) {
      this.currentChoice = model
    },
    getCategory(key) {
      return categories[key];
    }
  }
}
</script>

<style lang="scss">
.rent-vehicle{
  width: 100vw;
  height: 100vh;
  position: absolute;
  display: flex;
  flex-flow: column;
  align-items: center;
  justify-content: center;
  left: 0;
  top: 0;
  background: url('/img/rentVehicle/bg.png'), radial-gradient(44.58% 79.26% at 50% 50%, rgba(21, 23, 28, 0) 0%, rgba(11, 13, 18, 0.8) 100%);
  background-size: cover;
  background-position: center;
  color: #fff;
  text-transform: uppercase;
  padding-top: 9rem;
  &:before{
    content: '';
    width: 30rem;
    height: 30rem;
    position: absolute;
    top: -6rem;
    background: rgba(91, 191, 243, 0.4);
    filter: blur(7rem);
    border-radius: 50%;
    pointer-events: none;
    z-index: 0;
  }
  &-title{
    font-size: 8.1rem;
    line-height: 9.52rem;
    letter-spacing: 0.05em;
    z-index: 1;
    position: absolute;
    top: 1.45rem;
  }
  &__nav{
    display: flex;
    flex-flow: column;
    position: absolute;
    left: -.1rem;
    top: 0;
    transform: translateX(-100%);
    &-item{
      width: 6rem;
      height: 6rem;
      margin-bottom: .1rem;
      background: rgba(255, 255, 255, 0.1);
      display: flex;
      flex-flow: column;
      align-items: center;
      justify-content: center;
      transition: .2s;
      &:hover, &.active{
        background: #B6D300;
        box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
      }
      &:last-child{
        margin-bottom: 0;
      }
      &__icon{
        width: 2rem;
        height: 2rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        margin-bottom: .5rem;
      }
      &__text{
        font-size: .9rem;
      }
    }
  }
  &__body{
    height: calc(100vh - (9rem));
    width: 60rem;
    background: linear-gradient(180deg, #0A0A0A 0%, rgba(10, 10, 10, 0) 100%);
    position: relative;
    padding-right: 3rem;
    &-scroll{
      padding: 3rem 1.5rem .5rem 3rem;
      margin-right: -1.5rem;
      height: 100%;
      display: grid;
      grid-template-columns: repeat(4, 1fr);
      grid-auto-rows: 13.7rem;
      grid-row-gap: 2rem;
      grid-column-gap: .6rem;
      overflow-y: auto;
      &::-webkit-scrollbar{
        width: .3rem;
        background: transparent;
        &-thumb{
          background: #B6D300;
          box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
          border-radius: 1px;
        }
      }
    }
  }
}
</style>
