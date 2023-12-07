<template>
  <div class="smartphone">
    <div class="smartphone__close">
      <div class="btn" @click="hideSmartphone"></div>
      <div class="text">{{loc('phone:back:hint')}}</div>
    </div>
    <div class="smartphone__main">
      <div class="content" :style="{ background: 'center / contain no-repeat url(' + `img/hud/smartphone/settingsTab/wallpaper/${configuration.Wallpaper}.png` + '), #000' }">
        <img
          src="img/hud/smartphone/fringe.png"
          alt="fringe"
          class="content__fringe"
        >
        <div :class="[{ dark: colorTheme.header === 'dark', light: colorTheme.header === 'light' }, 'content__header']">
          <span class="time">{{ time }}</span>
          <img
            src="img/hud/smartphone/status.png"
            alt="status"
            class="status"
          >
        </div>
        <notification-block v-if="notificationData" />
        <transition-group tag="div" class="content__apps" name="slide-fade">
          <radio-tab :key="0" v-show="currentTab.key === 'RadioTab'" />
          <component :key="1" v-if="currentTab.key !== 'RadioTab'" :is="currentTab.key" />
        </transition-group>
        <button
          v-if="currentTab.key !== 'DesktopTab'"
          :class="[{ dark: colorTheme.button === 'dark', light: colorTheme.button === 'light', none: colorTheme.button === 'none' }, 'content__home']"
          @click="setCurrentTab({ key: 'DesktopTab' })"
        ></button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

import DesktopTab from './components/DesktopTab'
import SettingsTab from './components/SettingsTab'
import ContactsTab from './components/ContactsTab'
import CallTab from './components/CallTab'
import ForbesTab from './components/ForbesTab'
import BankTab from './components/BankTab'
import RadioTab from './components/RadioTab'
import TaxiTab from './components/TaxiTab'
import TaxiJobTab from './components/TaxiJobTab'
import MapTab from './components/MapTab'
import AppStoreTab from './components/AppStoreTab'
import MessengerTab from './components/MessengerTab'
import WeatherTab from './components/WeatherTab'
import NewsTab from './components/NewsTab'
// import BrowserTab from './components/BrowserTab'
import NotificationBlock from './components/NotificationBlock'

export default {
  name: 'Smartphone',

  components: {
    DesktopTab,
    SettingsTab,
    ContactsTab,
    CallTab,
    ForbesTab,
    BankTab,
    NotificationBlock,
    TaxiTab,
    TaxiJobTab,
    RadioTab,
    AppStoreTab,
    MessengerTab,
    // BrowserTab,
    MapTab,
    WeatherTab,
    NewsTab
  },

  computed: {
    ...mapState('smartphone', [
      'currentTab', 
      'notificationData',
      'colorTheme',
      'configuration'
    ]),

    ...mapState('hud', ['time']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    hideSmartphone: function() {
      window.mp.trigger('phone::hide');
    }
  }
}
</script>

<style lang="scss" scoped>
.smartphone {
  display: flex;
  position: absolute;
  bottom: 1.5rem;
  right: 1.5rem;
  z-index: 2;
  &__close {
    display: flex;
    justify-content: center;
    align-items: center;
    position: absolute;
    width: 1.8rem;
    height: 1.8rem;
    bottom: 0;
    transform: translateX(calc(-100% - .75rem));
    .btn, .text{
      position: absolute;
    }
    .btn{
      width: 1.8rem;
      height: 1.8rem;
      border-radius: 50%;
      background: rgba(255, 255, 255, 0.4);
      display: flex;
      align-items: center;
      justify-content: center;
      &:after{
        content: '';
        width: .7rem;
        height: .7rem;
        background-image: url('/img/hud/smartphone/arrow.svg');
        background-size: contain;
        background-position: center;
        position: absolute;
      }
      &:hover{
        background: rgba(255, 255, 255, 0.2);
        transition: .3s;
      }
    }
    .text{
      white-space: pre;
      text-transform: uppercase;
      color: rgba(255, 255, 255, 0.5);
      letter-spacing: .1rem;
      font-weight: bold;
     text-transform: uppercase;
      width: 1px;
      height: 1rem;
      top: 0;
      transform: rotate(-90deg) translateX(100%);
      padding-left: .75rem;
      display: flex;
      align-items: center;
      justify-content: flex-start;
    }
  }
  &__main {
    width: 17.5rem;
    height: 35rem;
    background: center / contain no-repeat url("/img/hud/smartphone/shell.png");
    display: flex;
    align-items: center;
    justify-content: center;
    .content {
      width: 15.2rem;
      height: 33rem;
      position: relative;
      overflow: hidden;
      border-radius: 1.6rem;
      &__fringe {
        position: absolute;
        top: 0;
        left: 50%;
        transform: translate(-50%, 0);
        width: 7.8rem;
        height: 1.25rem;
        z-index: 4;
      }
      &__header {
        position: absolute;
        width: 100%;
        height: 1.25rem;
        align-items: flex-end;
        display: flex;
        justify-content: space-between;
        padding: 0 .7rem 0 1.3rem;
        z-index: 4;
        transition: all 0.4s;
        &.light {
          filter: invert(1);
        }
        &.dark {
          filter: none;
        }
        .time {
          font-size: .7rem;
          font-weight: bold;
          line-height: .6rem;
        }
        .status {
          width: 2.8rem;
          height: .85rem;
        }
      }
      &__apps {
        height: 100%;
        width: 100%;
      }
      &__home {
        position: absolute;
        transform: translate(-50%, 0);
        bottom: .5rem;
        left: 50%;
        background: #000;
        width: 6.7rem;
        height: .25rem;
        border-radius: 1rem;
        opacity: 1;
        z-index: 2;
        &.dark {
          background: #000;
        }
        &.light {
          background: #fff;
        }
        &.none {
          display: none;
        }
      }
    }
  }
}
.slide-fade-enter-active, .slide-fade-leave-active {
  transition: all .2s;
}
.slide-fade-enter, .slide-fade-leave-to {
  opacity: 0;
  position: absolute;
  transform: scale(0.5);
}

</style>

<style lang="scss">
.smartphone-bottom-to-top-slide-enter-active, .smartphone-bottom-to-top-slide-leave-active {
  transition: all .2s;
}
.smartphone-bottom-to-top-slide-enter, .smartphone-bottom-to-top-slide-leave-to {
  opacity: 0;
}
.smartphone-bottom-to-top-slide-enter {
  transform: translate(0, 100%);
}
.smartphone-bottom-to-top-slide-leave-to {
  transform: translate(0, 50%);
}
.smartphone-fade-enter-active, .smartphone-fade-leave-active {
  transition: all 0.3s ease-out;
}
.smartphone-fade-enter, .smartphone-fade-leave-to {
  opacity: 0;
}
.smartphone-scale-in-enter-active, .smartphone-scale-in-leave-active {
  transition: all 0.2s ease-in-out;
}
.smartphone-scale-in-enter, .smartphone-scale-in-leave-to {
  transform: scale(0.8);
  opacity: 0;
}
</style>
