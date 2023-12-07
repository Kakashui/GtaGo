<template>
  <div class="vehicle-repairs-services">
    <div class="vehicle-repairs-services__top">
      <div class="vehicle-repairs-services__car-info">
        <div class="desc">{{loc('vehicleRepairsServices_1')}}</div>
        <div class="car-name">{{this.vehicleData.name}}</div>
        <div class="state">
          <div
            class="state__svg-wrap"
            v-bind:class="{middle: this.vehicleData.StateEngine <= 40 && this.vehicleData.StateEngine > 20, fiew: this.vehicleData.StateEngine <= 20}"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="svg" viewBox="0 0 120 120" fill="none">
              <circle cx="60" cy="60" r="55" fill="none" stroke="rgba(255, 255, 255, 0.2)" stroke-width="3" />
              <circle cx="60" cy="60" r="54" fill="none" stroke="rgba(255, 255, 255, .8)" stroke-width="1" />
              <circle class="circle-value" :stroke-dasharray="circleCircumference" :stroke-dashoffset="circleValue" cx="60" cy="60" r="55" fill="none" stroke="#B6D300" stroke-width="4"/>
            </svg>
            <div class="value">{{this.vehicleData.StateEngine}}</div>
          </div>
          <div class="state-info">
            <div class="state__title">{{loc('vehicleRepairsServices_2')}}</div>
            <div class="state__desc">{{loc('vehicleRepairsServices_3')}}</div>
            <div class="state__value">{{vehicleData.Mileage}} {{loc('vehicleRepairsServices_15')}}</div>
          </div>
        </div>
      </div>
      <div class="vehicle-repairs-services__run">
        <div class="icon icon-oil"></div>
        <div class="info">
          <div class="desc">{{loc('vehicleRepairsServices_4')}}<br>{{loc('vehicleRepairsServices_5')}}</div>
          <div class="value">{{vehicleData.MileageToChangeOil}} {{loc('vehicleRepairsServices_15')}}</div>
        </div>
      </div>
      <div class="vehicle-repairs-services__run">
        <div class="icon icon-transmission"></div>
        <div class="info">
          <div class="desc">{{loc('vehicleRepairsServices_4')}}<br>{{loc('vehicleRepairsServices_6')}}</div>
          <div class="value">{{vehicleData.MileageToChangeOilTransm}} {{loc('vehicleRepairsServices_15')}}</div>
        </div>
      </div>
      <div class="vehicle-repairs-services__run">
        <div class="icon icon-disc-brake"></div>
        <div class="info">
          <div class="desc">{{loc('vehicleRepairsServices_4')}}<br>{{loc('vehicleRepairsServices_7')}}</div>
          <div class="value">{{vehicleData.MileageToChangeBrake}} {{loc('vehicleRepairsServices_15')}}</div>
        </div>
      </div>
    </div>
    <div class="vehicle-repairs-services__bottom">
      <div class="vehicle-repairs-services__repair repair-carcase" @click="repair('body')">
        <div class="vehicle-repairs-services__repair-title">{{loc('vehicleRepairsServices_8')}}</div>
        <div class="vehicle-repairs-services__repair-value">{{vehicleData.PriceRepair}}</div>
        <div class="vehicle-repairs-services__repair-img"></div>
        <div 
        class="vehicle-repairs-services__repair-state"
        >
          <div class="state-desc rotate">{{loc('vehicleRepairsServices_14')}}</div>
        </div>
      </div>
      <div class="vehicle-repairs-services__repair repair-engine" @click="repair('engine')">
        <div class="vehicle-repairs-services__repair-title">{{loc('vehicleRepairsServices_9')}}</div>
        <div class="vehicle-repairs-services__repair-value">{{vehicleData.PriceRepairEngine}}</div>
        <div class="vehicle-repairs-services__repair-img"></div>
        <div 
          class="vehicle-repairs-services__repair-state"
          v-bind:class="{middle: this.vehicleData.StateEngine <= 40 && this.vehicleData.StateEngine > 20, fiew: this.vehicleData.StateEngine <= 20}"
        >
          <div class="state-desc">{{loc('vehicleRepairsServices_13')}}</div>
          <div class="state-value">{{vehicleData.StateEngine}}</div>
        </div>
      </div>
      <div class="vehicle-repairs-services__repair repair-engine-oil" @click="repair('engineOil')">
        <div class="vehicle-repairs-services__repair-title">{{loc('vehicleRepairsServices_10')}}</div>
        <div class="vehicle-repairs-services__repair-value">{{vehicleData.PriceChangeOil}}</div>
        <div class="vehicle-repairs-services__repair-img"></div>
        <div 
          class="vehicle-repairs-services__repair-state"
          v-bind:class="{middle: this.vehicleData.StateEngineOil <= 40 && this.vehicleData.StateEngineOil > 20, fiew: this.vehicleData.StateEngineOil <= 20}"
        >
          <div class="state-desc">{{loc('vehicleRepairsServices_13')}}</div>
          <div class="state-value">{{vehicleData.StateEngineOil}}</div>
        </div>
      </div>
      <div class="vehicle-repairs-services__repair repair-transmission-oil" @click="repair('transmisOil')">
        <div class="vehicle-repairs-services__repair-title">{{loc('vehicleRepairsServices_11')}}</div>
        <div class="vehicle-repairs-services__repair-value">{{vehicleData.PriceChangeOilTransm}}</div>
        <div class="vehicle-repairs-services__repair-img"></div>
        <div 
          class="vehicle-repairs-services__repair-state"
          v-bind:class="{middle: this.vehicleData.StateTransmOil <= 40 && this.vehicleData.StateTransmOil > 20, fiew: this.vehicleData.StateTransmOil <= 20}"
        >
          <div class="state-desc">{{loc('vehicleRepairsServices_13')}}</div>
          <div class="state-value">{{vehicleData.StateTransmOil}}</div>
        </div>
      </div>
      <div class="vehicle-repairs-services__repair repair-disc-brake" @click="repair('break')">
        <div class="vehicle-repairs-services__repair-title">{{loc('vehicleRepairsServices_12')}}</div>
        <div class="vehicle-repairs-services__repair-value">{{vehicleData.PriceChangeBrake}}</div>
        <div class="vehicle-repairs-services__repair-img"></div>
        <div 
          class="vehicle-repairs-services__repair-state"
          v-bind:class="{middle: this.vehicleData.StateBrake <= 40 && this.vehicleData.StateBrake > 20, fiew: this.vehicleData.StateBrake <= 20}"
        >
          <div class="state-desc">{{loc('vehicleRepairsServices_13')}}</div>
          <div class="state-value">{{vehicleData.StateBrake}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  name: 'vehicleRepairsServices',

  computed: {
    ...mapState('vehicleRepairsServices', ['vehicleData']), 
    ...mapGetters('localization',['loc']),

    circleCircumference: function () {
      const maxCircleValue = 2 * Math.PI * 55
      return maxCircleValue
    },

    circleValue: function () {
      const maxCircleValue = 2 * Math.PI * 55
      const maxValue = 100
      const curCircleVal = maxCircleValue - (maxCircleValue / maxValue) * this.vehicleData.StateEngine
      console.log(this.vehicleData.StateEngine)
      return curCircleVal
    }
  },

  methods: {
    repair: function(state) {
      window.mp.trigger('repair:buyService', state)
    }
  }

}
</script>

<style lang="scss" scoped>
.vehicle-repairs-services{
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  flex-flow: column;
  justify-content: space-between;
  text-transform: uppercase;
  &__top, &__bottom{
    display: flex;
  }
  &__top{
    align-items: flex-start;
    justify-content: flex-start;
    background: radial-gradient(75.9% 46.81% at 50% -7.08%, rgba(53, 59, 73, 0.9) 0%, rgba(53, 59, 73, 0) 100%);
    padding-right: 3.25rem;
  }
  &__bottom{
    align-items: center;
    justify-content: space-between;
    background: linear-gradient(180deg, rgba(0, 0, 0, 0) 0%, rgba(0, 0, 0, 0.4) 100%);
    padding: 0 3.25rem 2.25rem 2.5rem;
  }
  &__car-info{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    margin-right: 1rem;
    width: 24.2rem;
    min-width: 24.2rem;
    padding: 2.5rem 0 0 2.5rem;
    background: radial-gradient(69.62% 69.62% at 0% 0%, rgba(16, 23, 41, 0.9) 0%, rgba(53, 59, 73, 0) 100%);
    .desc{
      font-size: .9rem;
      line-height: 1.1rem;
      letter-spacing: 0.03em;
      color: rgba(255, 255, 255, 0.5);
    }
    .car-name{
      font-weight: normal;
      font-size: 2.5rem;
      line-height: 3rem;
      letter-spacing: 0.03em;
      color: #FFFFFF;
    }
    .state{
      display: flex;
      align-items: center;
      &-info{
        display: flex;
        flex-flow: column;
      }
      &__svg-wrap{
        margin-right: 1.4rem;
        width: 6rem;
        min-width: 6rem;
        height: 6rem;
        position: relative;
        display: flex;
        align-items: center;
        justify-content: center;
        &.middle{
          &:before{
            background: rgba(211, 178, 0, 0.3);
          }
          .svg{
            .circle-value{
              stroke: #D3B200;
            }
          }
        }
        &.fiew{
          &:before{
            background: rgba(233, 21, 21, 0.3);
          }
          .svg{
            .circle-value{
              stroke: #E91515;
            }
          }
        }
        &:before{
          content: '';
          background: rgba(156, 211, 0, 0.3);
          filter: blur(1rem);
          width: 5rem;
          height: 5rem;
          position: absolute;
        }
        .value{
          font-weight: 300;
          font-size: 2rem;
          line-height: 2rem;
          text-align: center;
          letter-spacing: 0.03em;
          color: #FFFFFF;
          position: absolute;
          &:after{
            content: '%';
          }
        }
        .svg{
          width: 100%;
          height: 100%;
          transform-origin: 50% 50%;
          transform: rotate(-90deg);
        }
      }
      &__title{
        font-size: 1rem;
        line-height: 1.2rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
      }
      &__desc{
        font-size: .9rem;
        line-height: 1.1rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.5);
      }
      &__value{
        font-size: 1.3rem;
        line-height: 1.55rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
      }
    }
  }
  &__run{
    display: flex;
    align-items: flex-start;
    justify-content: center;
    padding-top: 1.5rem;
    margin-right: 3.5rem;
    .icon{
      width: 4rem;
      min-width: 4rem;
      height: 4rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-size: contain;
      &.icon-oil{
        background-image: url('/img/vehicleRepairsServices/oil.svg');
      }
      &.icon-transmission{
        background-image: url('/img/vehicleRepairsServices/transmission.svg');
      }
      &.icon-disc-brake{
        background-image: url('/img/vehicleRepairsServices/disc-brake.svg');
      }
    }
    .info{
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      margin-left: .8rem;
      .desc{
        font-weight: normal;
        font-size: .9rem;
        line-height: 1.1rem;
        letter-spacing: 0.03em;
        color: rgba(255, 255, 255, 0.5);
      }
      .value{
        font-weight: 300;
        font-size: 2.3rem;
        line-height: 2.5rem;
        letter-spacing: 0.03em;
        color: #FFFFFF;
        text-transform: uppercase;
      }
    }
  }
  &__repair{
    display: flex;
    flex-flow: column;
    align-items: flex-start;
    background: rgba(0, 3, 7, 0.75);
    padding: 1.75rem;
    min-width: 16.75rem;
    position: relative;
    min-height: 9.25rem;
    background-size: contain;
    background-position: right bottom;
    background-repeat: no-repeat;
    transition: .2s;
    &:hover{
      transform: translateY(-1rem);
      transition: .2s;
      .vehicle-repairs-services__repair-state{
        bottom: 0;
        right: 0;
        transition: .2s;
      }
    }
    &.repair{
      &-carcase{
        background-image: url('/img/vehicleRepairsServices/carcase.png');
      }
      &-engine{
        background-image: url('/img/vehicleRepairsServices/engine.png');
      }
      &-engine-oil{
        background-image: url('/img/vehicleRepairsServices/engine-oil.png');
      }
      &-transmission-oil{
        background-image: url('/img/vehicleRepairsServices/transmission-oil.png');
      }
      &-disc-brake{
        background-image: url('/img/vehicleRepairsServices/disc-brake.png');
      }
    }
    &-title{
      font-weight: normal;
      font-size: 1.6rem;
      line-height: 1.9rem;
      letter-spacing: 0.03em;
      color: #FFFFFF;
      margin-bottom: .5rem;
      max-width: 12rem;
      min-height: 3.7rem;
    }
    &-value{
      font-weight: normal;
      font-size: 1rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      color: #B6D300;
      position: relative;
      &:before{
        content: '$ ';
        white-space: pre;
      }
    }
    &-state{
      width: 7rem;
      height: 7rem;
      position: absolute;
      bottom: -.25rem;
      right: -.25rem;
      display: flex;
      flex-flow: column;
      align-items: flex-end;
      justify-content: flex-end;
      padding: 0 .5rem .5rem 0;
      transition: .2s;
      &.fiew{
        &:before{
          border-right: 3.5rem solid #E91515;
          border-bottom: 3.5rem solid #E91515;
        }
      }
      &.middle{
        &:before{
          border-right: 3.5rem solid #F2CD07;
          border-bottom: 3.5rem solid #F2CD07;
        }
      }
      &:before{
        position: absolute;
        right: 0;
        bottom: 0;
        content: '';
        width: 0;
        height: 0;
        border: 3.5rem solid transparent;
        border-right: 3.5rem solid #B6D300;
        border-bottom: 3.5rem solid #B6D300;
        z-index: 0;
      }
      .state-desc, .state-value{
        z-index: 1;
        color: #FFFFFF;
        letter-spacing: 0.03em;
      }
      .state-desc{
        display: flex;
        font-size: 1rem;
        line-height: 1.2rem;
        &.rotate{
          transform: rotate(-45deg);
          margin-bottom: 1rem;
        }
      }
      .state-value{
        font-size: 2rem;
        line-height: 2.4rem;
        font-weight: 300;
        &:after{
          content: '%';
        }
      }
    }
  }
}
</style>