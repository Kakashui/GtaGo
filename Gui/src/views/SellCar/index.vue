<template>
  <div class="sell-car">
    <div class="sell-car__wrap">
      <BtnClose
        @closeBuyCar="closeBuyCar"
      />
      <div class="sell-car__block-label"><span>{{loc('sellCar_0')}}</span></div>
      <div class="sell-car__body">
        <div class="sell-car-title">{{loc('sellCar_1')}}</div>
        <div class="sell-car__info">
          <div class="sell-car__info_block">
            <div class="sell-car__info-desc">{{loc('sellCar_2')}}</div>
            <div class="sell-car-name">{{carState.carName}}</div>
          </div>
          <div class="sell-car__info_row">
            <div class="sell-car__info_block">
              <div class="sell-car__info-desc">{{loc('sellCar_3')}}</div>
              <div class="sell-car__info-label">{{carState.miliage}} {{loc('sellCar_4')}}</div>
            </div>
            <div class="sell-car__info_block">
              <div class="sell-car__info-desc">{{loc('sellCar_5')}}</div>
              <div class="sell-car__info-label">{{registrationNumber}}</div>
            </div>
          </div>
          <div class="sell-car__info_row">
            <!-- <div class="sell-car__info_block">
              <div class="sell-car__info-desc">{{loc('sellCar_6')}}</div>
              <div class="sell-car__info-label">{{dateOfIssue}}</div>
            </div> -->
            <div class="sell-car__info_block">
              <div class="sell-car__info-desc">{{loc('sellCar_7')}}</div>
              <div class="sell-car__info-label">${{carState.price}}</div>
            </div>
            <div class="sell-car__info_block">
              <div class="sell-car__info-desc">{{loc('sellCar_8')}}</div>
              <div class="sell-car__info-label">{{carState.owner}}</div>
            </div>
          </div>
          <div class="sell-car__info_block">
            <div class="sell-car__info-desc">{{loc('sellCar_9')}}</div>
            <div class="sell-car__info-label sell-car__info-label--cost">${{currentPrice}}</div>
          </div>
          <div class="sell-car__btns">
            <div 
              class="sell-car-btn"
              @click="acceptBuyCar"
            >{{loc('sellCar_10')}}</div>
            <div 
              class="sell-car-btn"
              @click="closeBuyCar"
            >{{loc('sellCar_11')}}</div>
          </div>
        </div>
        <div class="sell-car__params">
          <div class="sell-car__state">
            <div class="sell-car__state-desc">{{loc('sellCar_12')}}</div>
            <div class="sell-car__state-value">{{carState.generalState}}%</div>
          </div>
          <ParamItem
            v-for="(item, index) in carState.mechState"
            :key="index"
            :item="item"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import BtnClose from './BtnClose'
import ParamItem from './ParamItem'
export default {
  name: 'SellCar',

  components: {
    BtnClose,
    ParamItem
  },

  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('sellCar', [
      'registrationNumber', 
      'dateOfIssue', 
      'currentPrice', 
      'carState'
    ])
  },

  methods: {
    closeBuyCar: function() {
      window.mp.trigger('sellCar:closeBuyCar')
    },
    acceptBuyCar: function() {
      window.mp.trigger('sellCar:acceptBuyCar')
    }
  }
}
</script>

<style lang="scss" scoped>
.sell-car{
  width: 100%;
  height: 100vh;
  position: absolute;
  top: 0;
  left: 0;
  text-transform: uppercase;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #FFFFFF;
  background: radial-gradient(44.58% 79.26% at 50% 50%, rgba(21, 23, 28, 0) 0%, rgba(11, 13, 18, 0.8) 100%);
  &__block-label{
    position: absolute;
    top: -1.8rem;
    background: #B6D300;
    box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
    border-radius: 1px;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 18rem;
    height: 4rem;
    transform: skewX(-15deg);
    font-weight: bold;
    font-size: 1.5rem;
    span{
      transform: skewX(15deg);
    }
  }
  &__wrap, &__body{
    display: flex;
    position: relative;
  }
  &__wrap{
    width: 60rem;
    height: 43.5rem;
    background: #0A0A0A;
    justify-content: center;
    &:before{
      content: '';
      width: 30rem;
      height: 30rem;
      position: absolute;
      top: 0;
      transform: translateY(-66%);
      background: rgba(255, 255, 255, 0.15);
      filter: blur(5rem);
    }
  }
  &__body{
    height: 100%;
    width: 100%;
    overflow: hidden;
    padding: 10rem 4rem 5.75rem 4rem;
    justify-content: space-between;
  }
  &-title{
    font-weight: normal;
    font-size: 8.1rem;
    line-height: 9.7rem;
    letter-spacing: 0.03em;
    color: rgba(255, 255, 255, 0.12);
    white-space: pre;
    position: absolute;
    left: -.35rem;
    top: -2rem;
  }
  &-name{
    font-weight: normal;
    font-size: 3rem;
    line-height: 3.6rem;
    letter-spacing: 0.03em;
  }
  &__info{
    display: flex;
    flex-direction: column;
    &_block{
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      letter-spacing: 0.03em;
      margin-bottom: 1.75rem;
    }
    &_row{
      display: flex;
      align-items: center;
      &>div{
        &:nth-child(1) {
          min-width: 9.5rem;
          margin-right: .8rem;
        }
        &:last-child{
          margin-right: 0;
        }
      }
    }
    &-desc{
      font-size: .9rem;
      line-height: 1.1rem;
      color: rgba(255, 255, 255, 0.5);
      margin-bottom: .25rem;
    }
    &-label{
      font-size: 1.3rem;
      line-height: 1.55rem;
      &--cost{
        font-size: 2.5rem;
        line-height: 2.2rem;
        display: flex;
        align-items: flex-start;
        position: relative;
        &:before{
          content: '';
          width: 2rem;
          height: 2rem;
          margin-right: .5rem;
          background-size: contain;
          background-position: center;
          background-repeat: no-repeat;
          background-image: url('/img/sellCar/icon-money.svg');
        }
      }
    }
  }
  &__btns{
    display: flex;
    align-items: center;
  }
  &-btn{
    width: 10rem;
    height: 3.2rem;
    display: flex;
    align-items: center;
    justify-content: center;
    border: .1rem solid rgba(255, 255, 255, 0.2);
    background: transparent;
    border-radius: 1px;
    font-weight: bold;
    font-size: 1rem;
    margin-right: 1rem;
    transition: .3s;
    &:last-child{
      margin-right: 0;
    }
    &:hover{
      background: #B6D300;
      box-shadow: 0 .5rem 4rem rgba(168, 195, 2, 0.6);
      border-color: #B6D300;
    }
  }
  &__state{
    display: flex;
    align-items: center;
    margin-bottom: 1.5rem;
    position: relative;
    &-desc{
      font-size: .9rem;
      line-height: 1.2rem;
      letter-spacing: 0.03em;
      max-width: 5.5rem;
    }
    &-value{
      font-size: 3rem;
      line-height: 3.6rem;
      letter-spacing: 0.03em;
      color: #B6D300;
      text-shadow: 0 .5rem 2.5rem rgba(168, 195, 2, 0.6);
    }
    &:before{
      content: '';
      width: .7rem;
      height: .7rem;
      background: #FFFFFF;
      border-radius: 1px;
      position: absolute;
      left: -2.25rem;
      top: 50%;
      transform: translateX(-50%) translateY(-50%) rotate(45deg);
    }
  }
  &__params{
    display: flex;
    flex-direction: column;
    position: relative;
    padding-left: 2.25rem;
    border-left: 1px solid rgba(255, 255, 255, 0.1);
    height: fit-content;
    &:before{
      content: 'tuning';
      position: absolute;
      left: -.89rem;
      bottom: .6rem;
      -webkit-writing-mode: vertical-rl; 
      writing-mode: tb-rl;
      text-transform: uppercase;
      font-style: normal;
      font-weight: normal;
      font-size: 6rem;
      line-height: 6rem;
      letter-spacing: 0.18em;
      color: rgba(255, 255, 255, 0.1);
      transform: rotate(180deg);
    }
  }
}
</style>
