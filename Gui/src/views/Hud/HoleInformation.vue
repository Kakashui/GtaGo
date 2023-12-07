<template>
  <div class="hole-information">
    <div 
      class="hole-information__title"
      v-if="holeInformation.itemInfo.name !== null"
    >
      <div 
        class="hole-information__title-icon"
        :style="{backgroundImage: `url(/img/inventory/items/${holeInformation.itemInfo.icon}.png)`}"
      ></div>
      <div class="hole-information__title__info">
        <div class="hole-information__title__info-descmy" v-if="holeInformation.itemInfo.isMy">{{loc('Hole_information_0')}}</div>
        <div class="hole-information__title__info-desc" v-else>{{loc('Hole_information_12')}}</div>
        <div class="hole-information__title__info-name">{{loc(holeInformation.itemInfo.name)}}</div>
      </div>
    </div>
    <div class="hole-information__list">
      <div 
        class="hole-information__list-item is-watered"
        v-if="holeInformation.itemInfo.isWatered !== null"
      >
        <div class="hole-information__list-item-desc">{{loc('Hole_information_1')}}</div>
        <div class="hole-information__list-item-value">{{holeInformation.itemInfo.isWatered ? loc('Hole_information_6') : loc('Hole_information_7')}}</div>
      </div>
      <div 
        class="hole-information__list-item fertilizer"
        v-if="holeInformation.itemInfo.fertilizer !== null"
      >
        <div class="hole-information__list-item-desc">{{loc('Hole_information_2')}}</div>
        <div class="hole-information__list-item-value">{{loc(holeInformation.itemInfo.fertilizer)}}</div>
      </div>
      <div 
        class="hole-information__list-item hole"
        v-if="holeInformation.itemInfo.hole !== null"
      >
        <div class="hole-information__list-item-desc">{{loc('Hole_information_3')}}</div>
        <div class="hole-information__list-item-value">{{loc(holeInformation.itemInfo.hole)}}</div>
      </div>
    </div>
    <div 
      :class="[{'withers' : holeInformation.stateInfo.isWithers}, 'hole-information__state']"
      v-if="holeInformation.stateInfo.show"
    >
      <div 
        class="hole-information__state-desc"
        v-if="holeInformation.stateInfo.value > 0"
      >{{holeInformation.stateInfo.isWithers ? loc('Hole_information_8') : loc('Hole_information_9')}} {{loc('Hole_information_4')}} <span>{{holeInformation.stateInfo.value}} {{loc('Hole_information_5')}}</span></div>
      <div 
        class="hole-information__state-value"
        v-else
      >{{holeInformation.stateInfo.isWithers ? loc('Hole_information_10') : loc('Hole_information_11')}}</div>
      <div 
        class="hole-information__state-progress"
        :style="{width: `${progressWidth}%`}"
      ></div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'HoleInformation',
  computed: {
    ...mapGetters('localization', ['loc']),
    ...mapState('hud', ['holeInformation']),
    progressWidth() {
      return (this.holeInformation.stateInfo.value/this.holeInformation.stateInfo.maxValue) * 100
    }
  },
}
</script>

<style lang="scss">
.hole-information{
  display: flex;
  flex-direction: column;
  position: absolute;
  right: 1.5rem;
  bottom: 0;
  width: 15rem;
  height: 15.75rem;
  background: radial-gradient(73.17% 64.56% at 50% 0%, rgba(41, 255, 242, 0.4) 0%, rgba(41, 255, 242, 0) 100%), linear-gradient(180deg, rgba(13, 39, 38, 0.8) 64.56%, rgba(27, 63, 61, 0) 120.88%);
  padding: 5.5rem 1.5rem 0 1.5rem;
  text-transform: uppercase;
  color: #FFFFFF;
  animation: holeShow .7s;
  &__title{
    display: flex;
    align-items: center;
    position: absolute;
    left: 1.5rem;
    top: 1.5rem;
    &-icon{
      width: 3rem;
      height: 3rem;
      background-size: contain;
      background-repeat: no-repeat;
      background-position: center;
      margin-right: .6rem;
    }
    &__info{
      display: flex;
      flex-direction: column;
      &-descmy{
        font-size: 1rem;
        line-height: 1.2rem;
        color: #B6D300;
        text-shadow: 0 0 1.5rem #B6D300;
      }
      &-desc{
        font-size: 1rem;
        line-height: 1.2rem;
        color: #d33100;
        text-shadow: 0 0 1.5rem #d33100;
      }
      &-name{
        font-weight: bold;
        font-size: 1.5rem;
        line-height: 1.5rem;
      }
    }
  }
  &__list{
    display: flex;
    flex-direction: column;
    &-item{
      display: flex;
      align-items: flex-start;
      margin-left: 1.5rem;
      border-bottom: 1px solid rgba(255, 255, 255, 0.2);
      padding-bottom: .25rem;
      font-size: 1rem;
      line-height: 1rem;
      margin-bottom: .8rem;
      position: relative;
      &:before{
        content: '';
        width: 1rem;
        height: 1rem;
        margin-left: -.5rem;
        margin-top: .1rem;
        transform: translateX(-100%);
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        box-sizing: border-box;
      }
      &-desc{
        color: rgba(255, 255, 255, 0.7);
        margin-right: .4rem;
      }
      &-value{
        font-weight: bold;
      }
      &.is-watered{
        &:before{
          background-image: url('/img/holeInformation/drop.png');
        }
      }
      &.fertilizer{
        &:before{
          background-image: url('/img/holeInformation/shopping-basket.png');
        }
      }
      &.hole{
        &:before{
          border-radius: 50%;
          border: .355rem solid #fff;
        }
      }
    }
  }
  &__state{
    width: 100%;
    height: 2rem;
    display: flex;
    align-items: center;
    justify-content: flex-start;
    position: absolute;
    left: 0;
    bottom: 0;
    background-color: rgba(13, 39, 38, .8);
    color: #29FFF2;
    padding: 0 1.5rem;
    font-size: 1rem;
    line-height: 1.2rem;
    &.withers{
      background-color: rgba(58, 24, 21, 0.8);
      color: #EE443A;
      .hole-information__state-value{
        text-shadow: .5rem 0 2rem #EE443A;
      }
      .hole-information__state-progress{
        background-color: #EE443A;
      }
    }
    &-desc{
      color: rgba(255, 255, 255, 0.7);
      white-space: pre;
      span{
        color: #fff;
      }
    }
    &-value{
      width: 100%;
      text-align: center;
      font-weight: bold;
    }
    &-progress{
      background-color: #29FFF2;
      height: .3rem;
      position: absolute;
      left: 0;
      bottom: 0;
    }
  }
}
@keyframes holeShow {
  from {
    transform: translateY(100%);
  }
  to {
    transform: translateY(0);
  }
}
</style>
