<template>
    <div class="hud-state">
      <!-- <div class="hud-state__substrate--top"></div>
      <div class="hud-state__substrate--bottom"></div> -->
      <div class="hud-state__substrate--center" v-if="wantedLevels"></div>
      <div class="hud-state__status">
        <div class="player">
          <div class="player__online">
            <img src="img/hud/state/user.svg" class="icon">
            <span class="value">{{ online }}</span>
          </div>
          <div class="player__id">
            <span class="title">id:</span>
            <span class="value">{{ id }}</span>
          </div>
        </div>
        <img src="img/hud/state/logo.png" class="logo">
        <div class="date">
          <span class="date__number">{{ date }}</span>
          <div class="date__time">{{ time }}</div>
        </div>
      </div>
      <div class="hud-state__substatus">
        <div class="balance">
          <img src="img/hud/state/dollar.svg" class="balance__icon">
          <span class="balance__value">{{  money.toLocaleString() }}</span>
          <span class="balance__change"
                :style="{ 'color': `${moneyChange > 0 ? '#B6D300' : '#FFF'}` }"
                v-if="moneyChange !== 0">{{ moneyChange > 0 ? '+' + moneyChange.toLocaleString() : moneyChange.toLocaleString()}}</span>
        </div>
        <div class="separator"></div>
        <div class="voice">
          <div class="voice__main">
            <span class="desc">B</span>
            <img :src="voiceState === 0 ? mic ? 'img/hud/state/mic-on.svg' : 'img/hud/state/mic-off.svg' : mic ? 'img/hud/state/rad-on.svg' : 'img/hud/state/rad-off.svg'" class="icon">
          </div>
          <span class="voice__title">Voice</span>
        </div>
      </div>
      <div class="hud-state__prompt" v-if="promptData.show">
        <div :class="[{ cut: promptData.items.length > 1 }, 'main']">
          <div
            class="main__block"
            v-for="(item, index) in promptData.items"
            :key="index"
          >
            <span class="key">{{ loc(item.key) }}</span>
            <div class="desc">
              <span class="desc__title">{{ loc(item.text) }}</span>
              <span class="desc__main">{{ loc('bizhelp_0') }}</span>
            </div>
          </div>
        </div>
      </div>
      <div class="hud-state__reports" v-if="reportsCount">
        <div class="desc">Report</div>
        <div class="circle" :style="{ background: reportsCount >= 10 ? 'radial-gradient(50% 50% at 50% 50%, rgba(238, 68, 58, 0.5) 0%, rgba(238, 68, 58, 0) 100%)' : ''}">
          <span class="circle__value">{{ reportsCount }}</span>
          <span class="circle__prompt">F6</span>
          <svg version="1.1" xmlns="http://www.w3.org/2000/svg">
            <circle :cx="appWidthUnit * 3.25" :cy="appWidthUnit * 3.25" :r="appWidthUnit * 1.5" fill="none" stroke="rgba(255, 255, 255, 0.4)" :stroke-width="appWidthUnit * 0.15"/>
            <circle :stroke-dasharray="appWidthUnit * 10.05" :stroke-dashoffset="reportsFullness" :cx="appWidthUnit * 3.25" :cy="appWidthUnit * 3.25" :r="appWidthUnit * 1.5" fill="none" :stroke="reportsCircleColor" :stroke-width="appWidthUnit * 0.15"/>
          </svg>
        </div>
      </div>
      <div :class="[{ shifted: inVeh }, 'hud-state__ammo']" v-show="ammo > 0">
        <div class="main">
          <span class="main__title">{{ loc('hud_state_1') }}</span>
          <span class="main__value">{{ ammo }}</span>
        </div>
        <div class="separator"></div>
        <img src="img/hud/state/bullets.svg" class="icon">
      </div>
      <div class="hud-state__wanted" v-if="wantedLevels">
        <span class="title">{{loc('donincrease:2')}}</span>
        <div class="stars">
          <img
            v-for="(item, index) in wantedStars"
            :key="index"
            :src="item ? 'img/hud/state/star-fill.svg' : 'img/hud/state/star-empty.svg'"
            class="stars__item"
            alt="star"
          >
        </div>
      </div>
      <div class="hud-state__chips" v-if="isChipsList && chipsCheck">
        <div
          class="item"
          v-for="(item, key) in chipsList"
          :key="key"
          @click="pop(key)"
        >
          <span class="item__value">{{ item.toLocaleString() }}</span>
          <img
            :src="`img/hud/state/chip-${key}.svg`"
            alt="chip"
            class="item__icon"
          >
        </div>
      </div>
    </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex'

export default {
  methods: {
    pop(bet){
        switch(bet){
            case 'black':
                if (this.chipsList.black <= 0){
                    return;
                }
                break;
            case 'red':
                if (this.chipsList.red <= 0){
                    return;
                }
                break;
            case 'blue':
                if (this.chipsList.blue <= 0){
                    return;
                }
                break;
            case 'green':
                if (this.chipsList.green <= 0){
                    return;
                }
                break;
            case 'yellow':
                if (this.chipsList.yellow <= 0){
                    return;
                }
                break;
        }
        window.mp.trigger('updateChipCost', bet);
    }
  },
  computed: {
    ...mapState('hud', [
      'time',
      'date',
      'id',
      'online',
      'money',
      'voiceState',
      'mic',
      'ammo',
      'mammo',
      'wantedLevels',
      'chipsList',
      'isChipsList',
      'promptData',
      'moneyChange',
      'reportsCount'
    ]),
    ...mapState('speedometer', ['inVeh']),
    ...mapGetters('localization', ['loc']),

    wantedStars: function() {
      const array = []

      for (let i = 1; i <= 5; i++) {
        if (i <= this.wantedLevels) {
          array.push(1)
        } else {
          array.push(0)
        }
      }

      return array
    },

    chipsCheck: function() {
      const values = Object.values(this.chipsList).some(elem => elem > 0)
      return values
    },

    appWidthUnit: function () {
      const appWidth = document.getElementById('app').offsetWidth / 100
      return  Math.ceil(appWidth)
    },
    
    reportsCircleColor: function () {
      const alertValue = 5
      const criticalValue = 9
      
      if (this.reportsCount <= alertValue) {
        return '#B6D300'
      } else if (this.reportsCount < criticalValue) {
        return '#D3B200'
      } else {
        return '#EE443A'
      }
    },

    reportsFullness: function() {
      if (this.reportsCount > 10) return minCircleValue
      const criticalValue = 10
      const maxCircleValue = this.appWidthUnit * 10.05
      const minCircleValue = this.appWidthUnit * 0.6
      const maxCircleValuePer = ((maxCircleValue - minCircleValue) / 10) / criticalValue * 10
      const curCircleVal = maxCircleValue - (maxCircleValuePer * this.reportsCount)
      
      return curCircleVal
    }
  }
}
</script>


<style lang="scss" scoped>
@mixin position($right, $top) {
  position: absolute;
  top: $top;
  right: $right;
}

.hud-state {
  position: absolute;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
 text-transform: uppercase;
  font-weight: 400;
  letter-spacing: .07rem;
  // &__substrate--top {
  //   @include position(0, 0);
  //   width: 30rem;
  //   height: 30rem;
  //   background: linear-gradient(225deg, rgba(0, 0, 0, 0.5) 0%, rgba(0, 0, 0, 0) 52.36%);
  //   z-index: 1;
  // }
  // &__substrate--bottom {
  //   position: absolute;
  //   right: 0;
  //   bottom: 0;
  //   width: 17.5rem;
  //   height: 17.5rem;
  //   background: linear-gradient(315deg, rgba(0, 0, 0, 0.5) 0%, rgba(0, 0, 0, 0) 52.36%);
  //   z-index: 1;
  // }
  &__substrate--center {
    z-index: 1;
    transform: translate(-50%, -50%);
    position: absolute;
    width: 24rem;
    height: 9rem;
    top: 4.5rem;
    left: 50%;
    background: radial-gradient(54% 147.27% at 50% -47.27%, rgba(168, 195, 2, 0.5) 0%, rgba(168, 195, 2, 0) 100%);
  }
  &__status {
    z-index: 2;
    @include position(1.5rem, 1.6rem);
    display: flex;
    align-items: center;
    color: #fff;
    .player {
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      font-weight: 700;
      &__online {
        .icon {
          width: .8rem;
          height: auto;
        }
        .value {
          font-size: 1.7rem;
        }
      }
      &__id {
        text-transform: uppercase;
      }
    }
    .logo {
      margin: 0 1.5rem;
      width: 3.5rem;
      height: auto;
    }
    .date {
      font-weight: 700;
      &__number {
        color: rgba(255, 255, 255, 0.8);
      }
      &__time {
        font-size: 2rem;
        line-height: 1.6rem;
      }
    }
  }
  &__substatus {
    z-index: 2;
    @include position(1rem, 9.4rem);
    display: flex;
    align-items: center;
    .balance {
      display: flex;
      align-items: center;
      font-weight: 700;
      &__icon {
        width: 1.4rem;
        height: 1.4rem;
      }
      &__value {
        color: #fff;
        font-size: 1.8rem;
        margin: 0 0 0 .2rem;
      }
      &__change {
        position: absolute;
        right: 6.5rem;
        top: 2rem;
      }
    }
    .separator {
      height: 1.9rem;
      width: .1rem;
      background-color: rgba(255, 255, 255, 0.5);
      margin: 0 1rem;
    }
    .voice {
      display: flex;
      align-items: center;
      &__main {
        position: relative;
        width: 2.2rem;
        height: 2.2rem;
        background-color: #B6D300;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        .desc {
          position: absolute;
          top: -.2rem;
          left: -.2rem;
          background-color: #fff;
          color: #000;
          width: 1rem;
          height: 1rem;
          font-weight: bold;
          font-size: .8rem;
          display: flex;
          align-items: center;
          justify-content: center;
          border-radius: 50%;
        }
        .icon {
          max-width: 60%;
          height: auto;
        }
      }
      &__title {
        transform: rotate(180deg);
        text-transform: uppercase;
        font-weight: bold;
        color: rgba(255, 255, 255, 0.5);
        letter-spacing: 0.1rem;
        -webkit-writing-mode: vertical-rl; 
        writing-mode:tb-rl;
      }
    }
  }
  &__prompt {
    position: absolute;
    right: 1.5rem;
    color: #fff;
    top: 13rem;
    font-weight: 700;
    .main {
      min-width: 15rem;
      &.cut {
        .main__block {
          &:last-child {
            &::after {
              display: none;
            }
          }
        }
      }
      &__block {
        display: flex;
        align-items: center;
        padding: 0.4rem 0;
        position: relative;
        &:first-child {
          .key {
            background: #B6D300;
            box-shadow: 0 0px 2rem 0 #A8C302;
          }
        }
        &::after {
          content: '';
          position: absolute;
          bottom: 0;
          left: 0;
          background: linear-gradient(90deg, rgba(255, 255, 255, 0.7) 0%, rgba(255, 255, 255, 0) 100%);
          height: 0.1rem;
          width: 100%;
        }
        .key {
          width: 2rem;
          height: 2rem;
          font-size: 1.3rem;
          text-align: center;
          border-radius: 0.2rem;
          line-height: 2rem;
          border: 0.1rem solid rgba(255, 255, 255, 0.3);
        }
        .desc {
          text-transform: uppercase;
          margin: 0 0 0 0.6rem;
          letter-spacing: 0.15em;
          display: flex;
          flex-direction: column;
          &__main {
            color: rgba(255, 255, 255, 0.5);
            font-size: 0.8rem;
          }
        }
      }
    }
  }
  &__reports {
    position: absolute;
    right: 16.4rem;
    top: 0.5rem;
    .desc {
      transform: translate(-50%, -50%) rotate(270deg);
      position: absolute;
      letter-spacing: 0.1rem;
      color: rgba(255, 255, 255, 0.5);
      text-transform: uppercase;
      left: 90%;
      top: 50%;
    }
    .circle {
      width: 6.5rem;
      height: 6.5rem;
      position: relative;
      &__value {
        position: absolute;
        transform: translate(-50%, -50%);
        top: 50%;
        left: 50%;
        font-size: 1.5rem;
        color: #fff;
      }
      &__prompt {
        font-size: 0.7rem;
        background: #fff;
        color: #000;
        display: block;
        text-align: center;
        border-radius: 50%;
        width: 0.9rem;
        height: 0.9rem;
        top: 27%;
        left: 26%;
        position: absolute;
        z-index: 1;
      }
      svg {
        width: 100%;
        height: 100%;
        transform: rotate(270deg);
        circle:last-child {
          transition: stroke-dashoffset .5s;
        }
      }
    }
  }
  &__ammo {
    z-index: 2;
    position: absolute;
    bottom: 1rem;
    right: 1.6rem;
    display: flex;
    align-items: flex-end;
    transition: transform .15s;
    &.shifted {
      transform: translateX(-13rem);
    }
    .main {
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      &__title {
        text-transform: uppercase;
        font-size: .9rem;
        color: rgba(255, 255, 255, 0.5);
      }
      &__value {
        font-size: 1.5rem;
        font-weight: bold;
        color: #fff;
        line-height: 1.3rem;
        letter-spacing: .1rem;
      }
    }
    .separator {
      height: 2.2rem;
      width: .1rem;
      background-color: rgba(255, 255, 255, 0.5);
      margin: 0 1rem;
    }
    .icon {
      width: 2.25rem;
      height: auto;
    }
  }
  &__wanted {
    z-index: 2;
    transform: translate(-50%, 0);
    position: absolute;
    left: 50%;
    top: .8rem;
    display: flex;
    flex-direction: column;
    align-items: center;
    .title {
      font-weight: bold;
      text-transform: uppercase;
      color: #fff;
      letter-spacing: .1rem;
      font-size: .9rem;
    }
    .stars {
      display: flex;
      justify-content: space-between;
      width: 12rem;
      margin: .4rem 0 0 0;
      &__item {
        width: 2rem;
        height: auto;
      }
    }
  }
  &__chips {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    @include position(1rem, 50%);
    transform: translate(0, -50%);
    z-index: 2;
    .item {
      display: flex;
      align-items: center;
      justify-content: flex-end;
      margin: .5rem 0 0 0;
      width: 100%;
      &:first-child {
        margin: 0;
      }
      &__value {
        background-color: rgba(0, 0, 0, 0.2);
        color: #fff;
        font-family: "Roboto", "BPG Arial Caps";
        font-style: italic;
        font-size: .7rem;
        padding: 0 .4rem;
        border-radius: 1rem;
        flex: auto;
        text-align: right;
      }
      &__icon {
        width: 1rem;
        height: 1rem;
        margin: 0 0 0 .3rem;
      }
    }
  }
}
</style>