<template>
  <transition name="fade-in">
    <div class="search-section">
      <div class="search-section__title" v-if="taxiData.driver.name === null">{{loc('phone:taxi:search:1')}}</div>
      <div class="search-section__title" v-else>{{loc('phone:taxi:search:2')}}</div>
      <div class="search-section__desc" v-if="taxiData.driver.name === null">{{loc('phone:taxi:search:3')}}</div>
      <div class="search-section__desc" v-else>{{loc('phone:taxi:search:4')}}</div>
      <div class="search-section__radar">
        <div :class="[{ 'stop-anim' : taxiData.driver.name !== null }, 'radar-animation']">
          <div class="driver-block">
            <div class="info">
              <div class="driver-name">{{ taxiData.driver.name }}</div>
              <div class="car-model"><span class="desc">Car: </span>{{ taxiData.driver.car }}</div>
            </div>
          </div>
          <div class="car-served"></div>
          <div class="central-radar stoped"></div>
          <div class="animated-block">
            <div class="car" id="car-1"></div>
            <div class="car" id="car-2"></div>
            <div class="car" id="car-3"></div>
            <div class="central-radar"></div>
          </div>
        </div>
        <div class="timer"><span class="desc">{{loc('phone:taxi:search:5')}}</span>{{ correctTime }}</div>
      </div>
      <div 
        class="btn btn-call"
        v-if=" taxiData.driver.name !== null"
        @click="callDriver"
      >{{loc('phone:taxi:search:6')}}</div>
      <div 
      :class="[{ 'btn-cancel' : taxiData.driver.name !== null }, 'btn']"
      @click="cancel"
      >{{loc('phone:taxi:search:7')}}</div>
    </div>
  </transition>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'SearchSection',

  data: function() {
    return {
      time: 0,
      timer: null
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/taxiPage', ['taxiData']),
    ...mapGetters('localization', ['loc']),

    correctTime: function() {
      const minutes = Math.floor(this.time / 60);
      const seconds = Math.floor(this.time % 60);

      return `${this.twoDigit(minutes)}:${this.twoDigit(seconds)}`;
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    callDriver: function() {
      window.setData('smartphone/setCallData', { destination: this.taxiData.driver.phoneNumber, status: 'outcome' })
      window.setData('smartphone/setCurrentTab', { key: 'CallTab' })
    },

    cancel: function() {
      this.setCurrentTab({ key: this.currentTab.key, section: 'GeneralSection'})
      window.mp.triggerServer('phone::taxi::cancel');
    },

    twoDigit: (n) => n >= 10 ? n.toString() : `0${n}` 
  },

  mounted: function() {
    this.imer = setInterval(() => {
      this.time++;
    }, 1000);
  },

  beforeDestroy: function() {
    clearInterval(this.timer);
  }
}
</script>

<style lang="scss" scoped>
@keyframes fade {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
.search-section{
  padding: 3.5rem 1.25rem 2.25rem 1.25rem;
  display: flex;
  flex-flow: column;
  align-items: center;
  position: relative;
  &__title{
    text-transform: uppercase;
    font-style: normal;
    font-weight: bold;
    font-size: 1.5rem;
    line-height: 1.5rem;
    text-align: center;
    letter-spacing: 0.04em;
    text-transform: uppercase;
    color: #282828;
  }
  &__desc{
    font-size: .65rem;
    line-height: .75rem;
    text-align: center;
    color: rgba(0, 0, 0, 0.4);
    margin-top: .25rem;
    min-height: 1.5rem;
    margin-bottom: 3rem;
  }
  &__radar{
    width: 13rem;
    height: 13rem;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    margin-bottom: 1.5rem;
    .radar-animation{
      width: 100%;
      height: 100%;
      background-image: url('/img/hud/smartphone/taxiTab/radar-anim-bg.svg');
      background-size: contain;
      display: flex;
      align-items: center;
      justify-content: center;
      position: relative;
      .central-radar{
        background-image: url('/img/hud/smartphone/taxiTab/radar-img.svg');
        transition: 1s;
        &.stoped{
          opacity: 0;
          animation: none;
        }
      }
      .car, .central-radar{
        width: 100%;
        height: 100%;
        position: absolute;
        transform-origin: 50% 50%;
        background-size: contain;
        background-position: center;
        transition: 1s;
      }
      .driver-block, .car-served{
        opacity: 0;
        transition: .5s;
        position: absolute;
        top: 0;
      }
      .driver-block{
        width: 100%;
        height: 3.75rem;
        background: #FFFFFF;
        box-shadow: 0 .5rem 1.5rem rgba(177, 188, 203, 0.5);
        border-radius: 5rem;
        background-position: center;
        display: flex;
        align-items: center;
        justify-content: center;
        transform: translateY(-150%);
        z-index: 1;
        &:before, &:after{
          content: '';
          position: absolute;
        }
        &:before{
          width: 2.75rem;
          height: 2.75rem;
          background-image: url('/img/hud/smartphone/taxiTab/driver-avatar.png');
          background-size: contain;
          left: .5rem;
          top: .5rem;
        }
        &:after{
          border: .5rem solid transparent;
          border-top: .5rem solid #fff;
          bottom: 0;
          transform: translateY(92%);
        }
        .info{
          display: flex;
          flex-flow: column;
          align-items: flex-start;
          justify-content: center;
          margin-left: .4rem;
          .driver-name, .car-model{
            font-size: .65rem;
            line-height: .75rem;
            color: #000000;
          }
          .driver-name{
            margin-bottom: .4rem;
          }
          .car-model{
            position: relative;
            .desc{
              white-space: pre;
              color: rgba(0, 0, 0, 0.4);
            }
          }
        }
      }
      .car-served{
        width: 2.5rem;
        height: 2.5rem;
        background-image: url('/img/hud/smartphone/taxiTab/car-served.svg');
        background-size: contain;
        z-index: 2;
        transform: translateY(-200%);
      }
      .animated-block{
        position: relative;
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        transition: 1s;
        .car{
          &#car-1{
            background-image: url('/img/hud/smartphone/taxiTab/car-1.svg');
            animation: 4s linear infinite car-1;
          }
          &#car-2{
            background-image: url('/img/hud/smartphone/taxiTab/car-2.svg');
            animation: 4s linear infinite car-2;
          }
          &#car-3{
            background-image: url('/img/hud/smartphone/taxiTab/car-3.svg');
            animation: 4s linear infinite car-3;
          }
        }
        .central-radar{
          animation: 3s linear infinite radar;
        }
      }
      &.stop-anim{
        .driver-block, .car-served, .central-radar.stoped{
          opacity: 1;
        }
        .driver-block{
          transform: translateY(-50%);
        }
        .car-served{
          transform: translateY(100%);
        }
        .animated-block{
          opacity: 0;
          .car#car-1, .car#car-2, .car#car-3, .central-radar{
            animation-play-state: paused;
          }
        }
      }
    }
    .timer{
      position: absolute;
      display: flex;
      align-items: center;
      justify-content: center;
      bottom: 0;
      font-size: 2rem;
      line-height: 2.35rem;
      font-weight: 300;
      color: #000000;
      .desc{
        position: absolute;
        font-size: .65rem;
        font-weight: normal;
        line-height: .75rem;
        color: rgba(0, 0, 0, 0.4);
        top: -0.25rem;
        transform: translateY(-100%);
      }
    }
  }
  .btn{
    width: 100%;
    height: 2.5rem;
    min-height: 2.5rem;
    background: #F1C115;
    box-shadow: 0rem .5rem 1.5rem rgba(244, 193, 11, 0.3);
    border-radius: .5rem;
    margin-bottom: .5rem;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: .7rem;
    line-height: .8rem;
    color: #FFFFFF;
    position: relative;
    &.btn-call{
      &:before{
        content: '';
        position: absolute;
        width: 1rem;
        height: 1rem;
        left: .75rem;
        background-image: url('/img/hud/smartphone/taxiTab/call-icon.svg');
        background-size: contain;
        background-position: center;
      }
    }
    &.btn-cancel{
      background: #F5F5F5;
      color: #000000;
      box-shadow: none;
    }
  }

  @keyframes car-1 {
    0% {
      transform: rotate(0deg);
      opacity: 1;
    }
    
    43%{
      transform: rotate(-155deg);
      opacity: 1;
    }

    53%{
      opacity: 0;
      transform: rotate(-191deg);
    }

    90%{
      opacity: 0;
      transform: rotate(-324deg);
    }

    100% {
      transform: rotate(-360deg);
    }
  }
  @keyframes car-2 {
    0% {
      transform: rotate(0deg);
      opacity: 1;
    }
    
    5%{
      transform: rotate(18deg);
      opacity: 1;
    }

    15%{
      opacity: 0;
      transform: rotate(54deg);
    }
    
    44%{
      opacity: 0;
      transform: rotate(158deg);
    }

    54%{
      opacity: 1;
      transform: rotate(195deg);
    }

    100% {
      transform: rotate(360deg);
    }
  }
  @keyframes car-3 {
    0% {
      transform: rotate(0deg);
      opacity: 1;
    }
    
    8%{
      transform: rotate(-29deg);
      opacity: 1;
    }

    18%{
      opacity: 0;
      transform: rotate(-64deg);
    }
    
    51%{
      opacity: 0;
      transform: rotate(-185deg);
    }

    61%{
      opacity: 1;
      transform: rotate(-219.6deg);
    }

    100% {
      transform: rotate(-360deg);
    }
  }
  @keyframes radar {
    0%{
      transform: rotate(0deg);
    }
    25%{
      transform: rotate(-45deg);
    }
    75%{
      transform: rotate(45deg);
    }
    100%{
      transform: rotate(0deg);
    }
  }
  
}
.fade-in-enter-active {
  animation: fade 0.4s;
}
</style>
