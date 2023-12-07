<template>
  <transition name="fade">
    <div :class="[{ dark: colorTheme.header === 'dark' }, { light: colorTheme.header === 'light' }, 'notification']">
      <div class="main">
        <div class="main__info">
          <p class="username">{{ notificationData.name }}</p>
          <p class="desc">{{loc('phone:notify:1')}}</p>
        </div>
        <div class="main__actions">
          <button class="btn decline" @click="dropCall">
            <img
              src="img/hud/smartphone/contactsTab/phone1.svg"
              alt="decline"
              class="btn__icon"
            >
          </button>
          <button class="btn accept" @click="takeCall">
            <img
              src="img/hud/smartphone/contactsTab/phone1.svg"
              alt="accept"
              class="btn__icon"
            >
          </button>
        </div>
      </div>
      <button @click="setNotificationData(null)" class="close"></button>
    </div>
  </transition>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'NotificationBlock',

  computed: {
    ...mapState('smartphone', ['colorTheme', 'notificationData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setNotificationData']),

    takeCall: function() {
      window.setData('smartphone/takeCall', this.notificationData.number);
    },

    dropCall: function() {
      window.mp.triggerServer('phone::calls::drop', this.notificationData.number);
      window.mp.trigger('phone::calls::phoneDown');
      this.setNotificationData(null)
    }
  }
}
</script>

<style lang="scss" scoped>
.notification {
  position: absolute;
  width: 14.2rem;
  height: 4.5rem;
  border-radius: 0.65rem;
  top: 1.75rem;
  right: 0.5rem;
  padding: 1rem 0.6rem 0.5rem 1rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  z-index: 2;
  &.light {
    background: #FFFFFF;
    box-shadow: 0px 0.2rem 1rem rgba(0, 0, 0, 0.1);
    .main {
      &__info {
        .username {
          color: #000000;
        }
        .desc {
          color: rgba(0, 0, 0, 0.5);
        }
      }
    }
    .close {
      background: rgba(0, 0, 0, 0.3);
    }
  }
  &.dark {
    background: rgba(30, 30, 30, 0.95);
    .main {
      &__info {
        .username {
          color: #fff;
        }
        .desc {
          color: rgba(255, 255, 255, 0.5);
        }
      }
    }
    .close {
      background: rgba(255, 255, 255, 0.3);
    }
  }
  .main {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    &__info {
      .username {
        font-size: 0.8rem;
        
      }
      .desc {
        font-size: 0.7rem;
        
        margin: 0.25rem 0 0 0;
      }
    }
    &__actions {
      display: flex;
      .btn {
        width: 1.5rem;
        height: 1.5rem;
        border-radius: 50%;
        &:hover {
          transform: scale(1.05);
        }
        &__icon {
          width: 0.65rem;
          height: 0.65rem;
        }
        &.decline {
          background: #FF3B30;
          .btn__icon {
            transform: rotate(135deg);
          }
        }
        &.accept {
          background: #34C759;
          margin: 0 0 0 0.4rem;
        }
      }
    }
  }
  .close {
    mask: url('/img/hud/smartphone/arrowLeft.svg') no-repeat center / contain;
    width: 0.6rem;
    height: 1rem;
    transform: rotate(90deg);
  }
}
.fade-enter-active, .fade-leave-active {
  transition: all .2s;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
  transform: translateY(-0.2rem);
}
</style>