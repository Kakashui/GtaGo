<template>
  <div class="user-card">
    <div class="user-card__block-white">
      <div class="header">
        <button class="header__btn" @click="$emit('close')">
          <img
            src="img/hud/smartphone/messengerTab/chatSection/chevronLeftDark.svg"
            class="icon"
          >
          <span class="text">{{loc('phone:back')}}</span>
        </button>
      </div>
      <div class="avatar">
        <div class="avatar__indicator"></div>
      </div>
      <span class="username">{{ currentProfile.nickname }}</span>
      <span class="indicator">{{ currentProfile.isOnline ? 'Online' : 'Offline' }}</span>
      <button class="btn" @click="setCurrentTab({ key: currentTab.key, section: 'ViewedProfileSection', data: { location: currentTab } })">{{loc('phone:chat:ucard:2')}}</button>
    </div>
    <div class="user-card__title">{{loc('phone:chat:ucard:3')}}</div>
    <div class="user-card__actions">
      <button
        :style="{ color: button.color }"
        class="btn"
        v-for="(button, index) in actions"
        :key="index"
        @click="button.event"
      >{{ loc(button.text) }}</button>
    </div>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'

export default {
  name: 'PopupUserCard',

  props: {
    currentProfile: {
      type: Object,
      require: true
    }
  },

  data: function() {
    return {
      actions: [
        { text: 'phone:chat:ucard:4', event: this.giveAdmin, color: '#007AFF' },
        { text: 'phone:chat:ucard:5', event: this.kick, color: '#FF3B30' },
        { text: 'phone:chat:ucard:6', event: this.toBlackList, color: '#FF3B30' },
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    giveAdmin: function() {
      console.log(this.currentProfile.id, 'give')
    },

    kick: function() {
      console.log(this.currentProfile.id, 'kick')
    },

    toBlackList: function() {
      console.log(this.currentProfile.id, 'black')
    }
  }
}
</script>

<style lang="scss" scoped>
@keyframes shake {
  25% {
    transform: translateX(0.5rem);
  }
  50% {
    transform: translateX(0);
  }
  75% {
    transform: translateX(0.25rem);
  }
}
.user-card {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 31rem;
  background: #F3F6FA;
  border-radius: 0.6rem 0.6rem 0 0;
  overflow: hidden;
  &__block-white {
    padding: 1rem 0;
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    background: #fff;
    .header {
      position: absolute;
      width: 100%;
      padding: 0 1rem;
      &__btn {
        background: transparent;
        display: flex;
        align-items: center;
        &:hover .icon {
          animation: shake 0.4s;
        }
        .icon {
          width: 0.6rem;
          height: 1rem;
        }
        .text{
          font-size: .7rem;
          line-height: 1rem;
          color: #4E6ED9;
          margin-left: .4rem;
        }
      }
    }
    .avatar {
      width: 4rem;
      height: 4rem;
      display: block;
      background: center / cover no-repeat url("/img/hud/smartphone/messengerTab/chatSection/canalOptions/avatar.png");
      position: relative;
      margin: 0.75rem 0 0.75rem 0;
      &__indicator {
        position: absolute;
        top: 0.15rem;
        right: 0.15rem;
        width: 0.8rem;
        height: 0.8rem;
        border: 0.1rem solid #fff;
        background: #4BDB4B;
        border-radius: 50%;
      }
    }
    .username {
      line-height: 1.15rem;
      color: #151515;
      margin: 0 0 0.5rem 0;
    }
    .indicator {
      font-size: 0.7rem;
      line-height: 0.8rem;
      color: #AAAAAA;
      margin: 0 0 1.25rem 0;
    }
    .btn {
      width: 13.2rem;
      height: 2rem;
      border-radius: 5rem;
      background: #4E6ED9;
      color: #fff;
      font-size: 0.7rem;
    }
  }
  &__title {
    padding: 1.25rem 0 0.5rem 1rem;
    color: #8A8A8E;
    font-size: 0.7rem;
    line-height: 0.8rem;
    font-weight: 300;
  }
  &__actions {
    display: flex;
    flex-direction: column;
    padding: 0 0 0 1rem;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    box-shadow: 0px 0.2rem 0.5rem rgba(236, 234, 244, 0.5);
    background: #fff;
    .btn {
      background: transparent;
      text-align: left;
      height: 2rem;
      font-size: 0.7rem;
      &:hover {
        text-decoration: underline;
      }
      &:not(:last-child) {
        border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      }
    }
  }
}
</style>
