<template>
  <div class="truckers-menu">
    <div class="truckers-menu__title">{{truckersData.rankName}}</div>
    <div class="truckers-menu__level-state">
      <div class="icon">
        <div class="value">{{truckersData.level}}</div>
      </div>
      <div class="state-info">
        <div class="progress-wrap">
          <div class="progress-back"></div>
          <div class="progress-line" v-bind:style="{width: currentProgressBar}"></div>
        </div>
        <div class="desc">{{loc('TruckersMenu_1')}} <span>{{truckersData.currentExp}}</span> / <span>{{truckersData.requiredExp}}</span></div>
      </div>
    </div>
    <div class="truckers-menu__cars">
      <div
        v-for="item in this.truckersData.trucks"
        :key="item.key"
        :class="[{available: item.available}, 'truckers-menu__cars-item']"
      >
        <div class="info">
          <div class="info__name">{{item.name}}</div>
          <div class="info__cost">
            <div class="value">${{item.rentCost}}</div>
            <span>{{loc('TruckersMenu_2')}}</span>
          </div>
          <div class="info__cost">
            <div class="value">${{item.payment}}</div>
            <span>{{loc('TruckersMenu_7')}}</span>
          </div>
        </div>
        <div class="info-img" :style="{backgroundImage: 'url(/img/truckersMenu/items/trucker-car-' + item.id + '.png)'}"></div>
        <div
          v-if="item.available" 
          class="btn-rent"
          @click="setTruck(item.id)"
        >{{loc('TruckersMenu_3')}}</div>
        <div 
          v-else 
          class="unavailable"
        >
          <div class="info info-level">
            <div class="desc">{{loc('TruckersMenu_4')}}</div>
            <div class="value">{{item.availableLevel}} {{loc('TruckersMenu_5')}}</div>
          </div>
          <div class="info info-reward" v-if="item.reward">
            <div class="desc">{{loc('TruckersMenu_6')}}</div>
            <div class="value">+{{item.reward}}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
export default {
  name: 'TruckersMenu',

  computed: {
    ...mapState('truckersMenu', ['truckersData']),
    ...mapGetters('localization', ['loc']),
    currentProgressBar: function() {
      let currentValue = this.truckersData.currentExp 
      let maxValue = this.truckersData.requiredExp
      let percent = (currentValue/maxValue)*100
      return percent + '%'
    }
  },

  methods: {
    setTruck: function(id) {
      window.mp.trigger('truckersMenu:setTruck', id)
    }
  }
}
</script>

<style lang="scss" scoped>
.truckers-menu{
  display: flex;
  flex-flow: column;
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
  text-transform: uppercase;
  color: #fff;
  text-transform: uppercase;
  background-image: url('/img/truckersMenu/bg.png');
  background-size: cover;
  padding: 2.8rem 0 2.8rem 2.6rem;
  &__title{
    font-size: 2.4rem;
    line-height: 2.9rem;
    margin-bottom: 1rem;
  }
  &__level-state{
    display: flex;
    align-items: center;
    .icon{
      margin-right: 1.25rem;
      width: 1.75rem;
      height: 2.5rem;
      background-image: url('/img/truckersMenu/level-icon.svg');
      background-size: contain;
      background-position: center;
      background-repeat: no-repeat;
      padding-top: .25rem;
      display: flex;
      align-items: flex-start;
      justify-content: center;
      .value{
        font-weight: bold;
        font-size: 1.55rem;
        line-height: 1.55rem;
        letter-spacing: 0.05em;
        color: #2A3202;
      }
    }
    .state-info{
      display: flex;
      flex-flow: column;
      align-items: center;
      .progress-wrap{
        margin-bottom: .8rem;
        display: flex;
        align-items: center;
        justify-content: flex-start;
        width: 12.5rem;
        height: .5rem;
        position: relative;
        .progress-back, .progress-line{
          height: 100%;
          position: absolute;
          top: 0;
          left: 0;
          overflow: hidden;
        }
        .progress-back{
          border: 1px solid rgba(255, 255, 255, 0.2);
          width: 100%;
        }
        .progress-line{
          background: linear-gradient(270deg, #B6D300 0%, rgba(182, 211, 0, 0) 100%);
          display: flex;
          align-items: center;
          justify-content: center;
          &:after{
            content: '';
            position: absolute;
            right: 0;
            width: 2.25rem;
            height: .9rem;
            background: radial-gradient(103.64% 63.89% at 81.11% 50%, #FFFFFF 0%, rgba(255, 255, 255, 0) 74.48%);
          }
        }
      }
      .desc{
        font-size: .9rem;
        line-height: 1.1rem;
        letter-spacing: 0.15em;
        color: #FFFFFF;

      }
    }
  }
  &__cars{
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    padding-right: 1.75rem;
    grid-auto-rows: 10rem;
    margin-top: 2.5rem;
    grid-row-gap: .8rem;
    grid-column-gap: 1.2rem;
    overflow-y: auto;
    padding-bottom: 1.75rem;
    margin-bottom: -1.75rem;
    margin-right: .5rem;
    &::-webkit-scrollbar {
      width: .3rem;
    }
    &::-webkit-scrollbar-track {
      background: radial-gradient(50% 50% at 50% 50%, rgba(255, 255, 255, 0.65) 0%, rgba(255, 255, 255, 0) 100%);
      width: 1px;
    }
    &::-webkit-scrollbar-thumb{
      background: #E5E5E5;
      border-radius: 1rem;
      width: 1px;
    }
    &-item{
      display: flex;
      align-items: flex-start;
      justify-content: space-between;
      padding: 1.5rem 2rem;
      background: rgba(0, 0, 0, 0.45);
      border: 1px solid rgba(255, 255, 255, 0.15);
      box-shadow: 0 .6rem 1.1rem rgba(0, 0, 0, 0.35);
      border-radius: .5rem;
      position: relative;
      .btn-rent{
        display: flex;
        position: absolute;
        opacity: 0;
        left: 50%;
        transform: translateX(-50%);
        align-self: center;
        align-items: center;
        justify-content: center;
        background: rgba(193, 231, 4, 0.15);
        border: 1px solid #C1E704;
        border-radius: .5rem;
        transition: .2s;
        z-index: 2;
        padding: 1rem 2.3rem;
        font-weight: bold;
        font-size: 1.05rem;
        letter-spacing: 0.05em;
        color: #FFFFFF;
        &:hover{
          background: rgba(193, 231, 4, 0.35);
          transition: .2s;
        }
      }
      .unavailable{
        width: 100%;
        height: 100%;
        background: radial-gradient(50% 50% at 50% 50%, rgba(0, 0, 0, 0) 50.52%, #000000 100%), rgba(0, 0, 0, 0.75);
        border: 1px solid rgba(255, 255, 255, 0.15);
        box-sizing: border-box;
        border-radius: .5rem;
        position: absolute;
        top: 0;
        left: 0;
        display: flex;
        align-items: center;
        justify-content: center;
        white-space: pre;
        font-size: 1.2rem;
        line-height: 1.95rem;
        letter-spacing: 0.05em;
        color: #FFFFFF;
        .info{
          display: flex;
          flex-flow: column;
          align-items: flex-start;
          justify-content: center;
          margin-right: 1.4rem;
          &:last-child{
            margin-right: 0;
          }
          .desc{
            font-size: .9rem;
          }
          .value{
            font-size: 1.75rem;
            position: relative;
            display: flex;
            align-items: center;
            justify-content: center;
            &:before{
              content: '';
              background-size: contain;
              background-position: center;
              background-repeat: no-repeat;
              position: absolute;
              height: 1.3rem;
              margin-bottom: .2rem;
            }
          }
          &.info-level{
            .value{
              color: rgba(240,44,44,1);
              padding-left: 1.5rem;
              &:before{
                margin-right: .9rem;
                width: .95rem;
                background-image: url('/img/truckersMenu/locked.svg');
                left: 0;
              }
            }
          }
          &.info-reward{
            padding-left: 1.5rem;
            position: relative;
            &:before{
              position: absolute;
              content: '';
              width: 1px;
              height: 4rem;
              background: rgba(255, 255, 255, 0.15);
              left: 0;
            }
            .value{
              color: rgba(255,242,5,1);
              &:before{
                width: 1.3rem;
                background-image: url('/img/truckersMenu/forward.svg');
                right: 0;
                transform: translateX(1.8rem);
              }
            }
          }
        }
      }
      &.available{
        &:hover{
          .btn-rent{
            opacity: 1;
            transition: .3s;
          }
          &:after{
            content: '';
            z-index: 1;
            width: 100%;
            height: 100%;
            position: absolute;
            top: 0;
            left: 0;
            background: rgba(0, 0, 0, 0.65);
            border: .1rem solid #C1E704;
            box-sizing: border-box;
            box-shadow: 0 .6rem 1.1rem rgba(0, 0, 0, 0.35);
            border-radius: .5rem;
            transition: .3s;
          }
        }
      }
      .info{
        display: flex;
        flex-flow: column;
        height: 100%;
        justify-content: space-between;
        &__name{
          font-size: 1.5rem;
          line-height: 1.8rem;
          letter-spacing: 0.05em;
        }
        &__cost{
          font-size: 1.3rem;
          line-height: 1.3rem;
          letter-spacing: 0.05em;
          color: #C1E704;
          display: flex;
          align-items: center;
          span{
            font-family: BPG Arial Caps;
            font-size: .8rem;
            line-height: .95rem;
            color: rgba(255, 255, 255, 0.65);
            margin-left: .4rem;
            text-transform: lowercase;
          }
        }
      }
      .info-img{
        width: 9.4rem;
        height: 6.45rem;
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
      }
    }
  }
}
</style>
