<template>
  <div class="call-tab" :style="{ background: 'center / contain no-repeat url(' + `img/hud/smartphone/settingsTab/wallpaper/${configuration.Wallpaper}.png` + '), #000' }">
  <div class="call-tab__layer"></div>
  <div class="call-tab__main">
    <div class="header">
      <div class="header__destination">{{ callData.destination }}</div>
      <div class="header__text" v-if="callData.status === 'outcome' || callData.status === 'income'">{{ loc(callStatusText) }}</div>
      <div class="header__text" v-else-if="callData.status === 'drop'">{{ callData.dropReason }}</div>
      <header-timer v-else />
    </div>
    <div class="options" v-if="callData.status === 'outcome' || callData.status === 'active'">
      <div
        :class="[
          {
            disabled: item.id === 1 || item.id === 3 || item.id === 5,
            active: (item.id === 0 && !isMicrophone) || item.id === 2 && !isAudio,
          },
          'options__item'
        ]"
        v-for="item in buttons"
        :key="item.id"
      >
      <div class="icon" @click="item.event">
        <img
          :src="`img/hud/smartphone/callTab/${item.icon}.svg`"
          :alt="item.icon"
          class="icon__img"
        >
      </div>
      <span class="title">{{ loc(item.text) }}</span>
      </div>
    </div>
    <div class="actions">
      <div v-if="callData.status !== 'drop'" :class="[{ center: callData.status === 'outcome' || callData.status === 'active'}, 'actions__item']">
        <button class="btn decline" @click="dropCall">
          <img
            src="img/hud/smartphone/contactsTab/phone1.svg"
            alt="decline"
            class="btn__icon"
          >
        </button>
        <span class="title">{{ loc(dropBtnText) }}</span>
      </div>
      <div class="actions__item" v-if="callData.status === 'income'">
        <button class="btn accept" @click="takeCall">
          <img
            src="img/hud/smartphone/contactsTab/phone1.svg"
            alt="decline"
            class="btn__icon"
          >
        </button>
        <span class="title">{{loc('phone:call:accept')}}</span>
      </div>
    </div>
  </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

import HeaderTimer from './HeaderTimer'

export default {
  name: 'CallTab',

  components: { HeaderTimer },

  data: function() {
    return {
      isMicrophone: true,
      isAudio: true,
      buttons: [
        { id: 0, icon: 'microphone', text: 'phone:call:btn:1', event: this.toggleMicrophone },
        { id: 1, icon: 'keypad', text: 'phone:call:btn:2', event: () => undefined },
        { id: 2, icon: 'volume', text: 'phone:call:btn:3', event: this.toggleAudio },
        { id: 3, icon: 'plus', text: 'phone:call:btn:4', event: () => undefined },
        { id: 4, icon: 'blockUser', text: 'phone:call:btn:5', event: this.blockUser },
        { id: 5, icon: 'user', text: 'phone:call:btn:6', event: () => undefined }
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['configuration', 'callData']),
    ...mapGetters('localization', ['loc']),
    dropBtnText: function() {
      switch (this.callData.status) {
        case 'income':
          return 'phone:call:drop:1';
        case 'outcome':
          return 'phone:call:drop:2';
        default:
          return 'phone:call:drop:3';
      }
    },

    callStatusText: function() {
      switch (this.callData.status) {
        case 'income':
          return 'phone:call:stat:1';
        case 'outcome':
          return 'phone:call:stat:2';
        default:
          return '';
      }
    }
  },

  watch: {
    'callData.status': function(value) {
      if (value === 'drop') {
        setTimeout(() => {
          window.setData('smartphone/setCurrentTab', { key: 'DesktopTab', data: {theme: 'dark'} })
        }, 2000);
      }
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab', 'setColorTheme']),

    toggleMicrophone: function() {
      this.isMicrophone = !this.isMicrophone
    },

    toggleAudio: function() {
      this.isAudio = !this.isAudio
    },

    blockUser: function() {
      console.log('block user')
    },

    takeCall: function() {
      window.setData('smartphone/takeCall', this.callData.number);
    },

    dropCall: function() {
      if (this.callData.isFake)
        return;
      
      window.setData('smartphone/setCurrentTab', { key: 'DesktopTab', data: {theme: 'dark'} })
      window.mp.triggerServer('phone::calls::drop', this.callData.number);
      window.mp.trigger('phone::calls::phoneDown');
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes dotty {
  0%   { content: ''; }
  25%  { content: '.'; }
  50%  { content: '..'; }
  75%  { content: '...'; }
  100% { content: ''; }
}
.call-tab {
  height: 100%;
  position: relative;
  z-index: 3;
  &__layer {
    position: absolute;
    z-index: 1;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    backdrop-filter: blur(1.5rem);
  }
  &__main {
    position: absolute;
    z-index: 2;
    height: 100%;
    width: 100%;
    padding: 5rem 1.8rem 3.75rem 1.8rem;
    display: flex;
    flex-direction: column;
    // justify-content: space-between;
    .header {
      text-align: center;
      &__destination {
        font-size: 1.5rem;
        font-weight: 300;
        color: #fff;
        letter-spacing: 0.05rem;
        margin: 0 0 0.5rem 0;
      }
      &__text {
        color: rgba(255, 255, 255, 0.5);
        font-size: 0.7rem;
        line-height: 0.7rem;
        text-transform: uppercase;
        position: relative;
        display: inline-block;
        &::after {
          position: absolute;
          bottom: 0;
          left: 100%;
          display: inline-block;
          animation: dotty steps(1,end) 1s infinite;
          content: '';
        }
      }
    }
    .options {
      display: grid;
      grid-template-columns: repeat(3, 3.2rem);
      grid-gap: 0.8rem 1rem;
      margin: 4rem 0 0 0;
      &__item {
        display: flex;
        flex-direction: column;
        align-items: center;
        &.disabled {
          opacity: 0.4;
          .icon:hover {
            transform: none;
          }
        }
        &.active {
          .icon {
            background: #FFFFFF;
            &__img {
              filter: invert(1);
            }
          }
        }
        .icon {
          width: 3.2rem;
          height: 3.2rem;
          border-radius: 50%;
          background: rgba(255, 255, 255, 0.3);
          display: flex;
          align-items: center;
          justify-content: center;
          &:hover {
            transform: scale(1.05);
          }
          &__img {
            width: 1.3rem;
            height: 1.3rem;
          }
        }
        .title {
          font-size: 0.6rem;
          color: rgba(255, 255, 255, 0.8);
          margin: 0.2rem 0 0 0;
        }
      }
    }
    .actions {
      width: 100%;
      height: 4.5rem;
      margin: auto 0 0 0;
      position: relative;
      &__item {
        display: flex;
        align-self: start;
        flex-direction: column;
        align-items: center;
        transition: all 0.2s;
        position: absolute;
        top: 0;
        width: 3.2rem;
        &:first-child {
          left: 0;
        }
        &:last-child {
          right: 0;
        }
        &.center {
          transform: translate(4.2rem, 0);
        }
        .btn {
          width: 3.2rem;
          height: 3.2rem;
          display: flex;
          align-items: center;
          justify-content: center;
          border-radius: 50%;
          &:hover {
            transform: scale(1.05);
          }
          &.decline {
            background: #FE3D2E;
            .btn__icon {
              transform: rotate(135deg);
            }
          }
          &.accept {
            background: #34C85A;
          }
          &__icon {
            width: 1rem;
            height: 1rem;
          }
        }
        .title {
          font-size: 0.7rem;
          color: #fff;
          margin: 0.5rem 0 0 0;
        }
      }
    }
  }
}
</style>
