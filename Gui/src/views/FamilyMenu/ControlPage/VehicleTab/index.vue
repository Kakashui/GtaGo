<template>
  <div class="vehicle-tab">
    <div class="title">{{loc('familyMenu_78')}}</div>
    <div class="vehicle-list">
      <car-item
        v-for="item in cars"
        :key="item.id"
        :item="item"
        :cars="cars"
        :currentCar="currentCar"
        @setCurrentCar="manageCar"
      />
    </div>
    <div class="hexagon-wrap">
      <div 
        class="hexagon item"
        v-for="item in currentVehicleOptions"
        :key="item.id"
        @click="selectCarOption(item.key)"
      >
        <div class="info">
          <div 
            class="icon"
            :style="{backgroundImage: 'url(/img/familyMenu/vehicleOptions/icon-' + item.key + '.svg)'}"
          ></div>
          <div class="text">{{loc(item.text)}}</div>
        </div>
      </div>
      <div 
        class="hexagon"
        v-for="item in decoreHexagone"
        :key="item.id"
      >
      </div>
    </div>
    <div 
      class="modal-wrap"
      v-if="currentCar"
    >
      <div 
        class="btn btn-close"
        @click="closeModal"
      >
        <div class="icon"></div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import CarItem from './CarItem'
export default {
  name: 'VehicleTab',

  components: {
    CarItem
  },

  data: function() {
    return {
      vehicleOptionsData: [
        {
          key: 0,
          text: 'familyMenu_104',
          minRank: 1
        },
        {
          key: 1,
          text: 'familyMenu_105',
          minRank: 1
        },
        {
          key: 2,
          text: 'familyMenu_106',
          minRank: 3
        },
        {
          key: 3,
          text: 'Gps',
          minRank: 0
        },
        {
          key: 4,
          text: 'familyMenu_107',
          minRank: 3
        },
      ],
      currentVehicleOptions: [],
      currentCar: null
    }
  },

  methods: {
    setCurrentCar: function(value) {
      this.currentCar = value
    },
    closeModal: function() {
      this.setCurrentCar(null)
      this.currentVehicleOptions = []
    },
    manageCar: function(key) {
      this.cars.forEach(element => {
        if (element.key === key) {
          this.setCurrentCar(element)
        }
      })
      this.currentVehicleOptions = this.vehicleOptionsData.filter(item => item.minRank <= this.currentCar.access || this.isLeader)
    },
    selectCarOption: function(key) {
      window.mp.trigger('familyMenu:selectCarOption', key, this.currentCar.key)
    }
  },

  computed: {
    ...mapState('familyMenu', ['currentRankId', 'isLeader']),
    ...mapState('familyMenu/controlPage', ['ranksList']),
    ...mapGetters('localization', ['loc']),
    decoreHexagone: function() { 
      return 9 - this.currentVehicleOptions.length
    },

    currentRank: function() {
      let currentRank = this.ranksList.find(item => item.rankId == this.currentRankId || this.isLeader);
      return currentRank
    },
    cars: function() {
      let carsFromRank = this.currentRank.accessCars.filter(car => car.access >= 0 || this.isLeader)
      return carsFromRank
    }
  },
}
</script>

<style lang="scss" scoped>
@keyframes isShow{
  from{
    transform: translateX(100%);
    opacity: 0;
  }
  to{
    transform: translateX(0);
    opacity: 1;
  }
}

@for $i from 1 through 9 {
  .vehicle-tab .hexagon-wrap .hexagon:nth-child(#{$i}) {
    $length: .1*$i;  
    animation: isShow #{$length}s;
  }
}

.vehicle-tab{
  display: flex;
  flex-flow: column;
  align-items: flex-start;
  .title{
    padding-bottom: .2rem;
    position: relative;
    font-weight: bold;
    font-size: 2rem;
    line-height: 2rem;
    margin-bottom: .6rem;
    &:before{
      content: '';
      width: 9rem;
      height: .1rem;
      background: linear-gradient(270deg, rgba(220, 255, 0, 0) 4.75%, #DCFF00 100%);
      position: absolute;
      left: 0;
      bottom: 0;
    }
  }
  .vehicle-list{
    display: flex;
    flex-flow: column;
    width: calc(100% + (2rem) + (2.2rem));
    overflow-y: auto;
    padding-right: 2rem;
    padding-left: 2.2rem;
    margin-left: -2.2rem;
    max-height: calc(100vh - (11.2rem));
    &::-webkit-scrollbar{
      width: 0;
      background-color: transparent;
      &-thumb{
        width: 0;
        background-color: 0;
      }
    }
  }
  .hexagon-wrap{
    width: 34.45rem;
    position: absolute;
    right: -10.4rem;
    top: 9.2rem;
    display: flex;
    .hexagon {
      position: absolute;
      width: 8.9rem;
      height: 5.125rem;
      background-color: transparent;
      border-left: 1px solid #2D3236;
      border-right: 1px solid #2D3236;
      display: flex;
      align-items: center;
      justify-content: center;
      transition: .2s;
      &.item{
        background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.05) 0%, rgba(255, 255, 255, 0) 100%);
        z-index: 5;
        &:hover{
          border-color: #555b5f;
          transition: .2s;
          &:before,&:after{
            border-color: #555b5f;
            transition: .2s;
          }
        }
      }
      &:before,
      &:after {
        content: "";
        position: absolute;
        z-index: 1;
        width: inherit;
        height: inherit;
        background-color: transparent;  
        box-sizing: border-box;
        border-left: 1px solid #2D3236;
        border-right: 1px solid #2D3236;
        transition: .2s;
        background: inherit;
      }
      &:before {
        transform: rotate(-60deg);
      }
      &:after {
        transform: rotate(60deg);
      }
      &:nth-child(1){
        top: 0;
      }
      &:nth-child(2), &:nth-child(3){
        top: 8.3rem;
      }
      &:nth-child(4), &:nth-child(5){
        top: 16.6rem;
      }
      &:nth-child(6), &:nth-child(7){
        top: 24.9rem;
      }
      &:nth-child(8), &:nth-child(9){
        top: 33.2rem;
      }
      &:nth-child(2){
        left: 4.85rem;
      }
      &:nth-child(7){
        left: 24.25rem;
      }
      &:nth-child(9){
        left: 29.1rem;
      }
      &:nth-child(1), &:nth-child(4){
        left: 9.7rem;
      }
      &:nth-child(3), &:nth-child(6){
        left: 14.55rem;
      }
      &:nth-child(5), &:nth-child(8){
        left: 19.4rem;
      }
      .info {
        position: absolute;
        z-index: 10;
        display: flex;
        flex-flow: column;
        align-items: center;
        justify-content: center;
        .icon{
          width: 2rem;
          height: 2rem;
          background-size: contain;
          background-repeat: no-repeat;
          background-position: center;
          margin-bottom: .8rem;
        }
        .text{
          font-size: 1.2rem;
          text-align: center;
        }
      }
    }
  }
  .modal-wrap{
    .btn-close{
      width: 3.1rem;
      height: 1.75rem;
      position: absolute;
      top: 10.5rem;
      right: 15rem;
      background-color: rgb(56, 56, 56);
      display: flex;
      align-items: center;
      justify-content: center;
      &:hover{
        transform: scale(1.05);
      }
      .icon{
        width: 1.8rem;
        height: 1.8rem;
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        background-image: url('/img/familyMenu/btn-close.svg');
        z-index: 2;
      }
      &:before, &:after{
        content: "";
        position: absolute;
        z-index: 1;
        width: inherit;
        height: inherit;
        background-color: inherit;  
        box-sizing: border-box;
        transition: .2s;
        background: inherit;
      }
      &:before {
        transform: rotate(-60deg);
      }
      &:after {
        transform: rotate(60deg);
      }
    }
  }
}
</style>
