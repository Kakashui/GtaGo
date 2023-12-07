<template>
  <div class="profile-tab">
    <div class="profile-tab__header">
      <button class="back-button" @click="setTab({ key: 'GeneralTab' })">
        <img
          src="img/hud/smartphone/messengerTab/chatSection/chevronLeft.svg"
          alt="chevron"
          class="back-button__icon"
        >
        <span class="back-button__text" >{{loc('phone:back')}}</span>
      </button>
      <span class="title">{{loc('phone:group:profile:1')}}</span>
    </div>
    <div class="profile-tab__info">
      <div class="avatar">
        <div class="avatar__indicator" v-if="messengerData.CanalProfile.IsOnline"></div>
      </div>
      <span class="username">{{ messengerData.CanalProfile.DisplayedName }}</span>
      <span class="status">{{ currentStatus }}</span>
    </div>
    <div class="profile-tab__abilities">
      <span class="title">{{loc('phone:group:profile:2')}}</span>
      <div class="list">
        <abilities-item
          v-for="{ hasPermission, key, title } in permissions"
          :key="key"
          :item="{ value: hasPermission, title: title, key: key }"
        />
      </div>
    </div>
    <!-- <button class="profile-tab__btn profile-tab__btn-add">{{loc('phone:group:profile:3')}}</button> -->
    <button class="profile-tab__btn profile-tab__btn-remove" @click="isPopup = true">{{loc('phone:group:profile:4')}}</button>
    <transition name="smartphone-fade">
      <div class="profile-tab__layer" v-if="isPopup"></div>
    </transition>
    <transition name="smartphone-scale-in">
      <div class="profile-tab__popup" v-if="isPopup">
        <span class="title">{{loc('phone:group:profile:5')}}</span>
        <p class="desc">{{loc('phone:group:profile:6')}}</p>
        <button class="btn-confirm" @click="removeAdmin">{{loc('phone:group:profile:7')}}</button>
        <button class="btn-cancel" @click="isPopup = false">{{loc('phone:cancel')}}</button>
      </div>
    </transition>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'
import AbilitiesItem from './AbilitiesItem'

export default {
  name: 'ProfileTab',

  components: { AbilitiesItem },

  props: {
    setTab: Function
  },

  data: function() {
    return {
      isPopup: false,
      abilityTitles: {
        1: 'phone:group:profile:9',
        3: 'phone:group:profile:10',
        4: 'phone:group:profile:11'
      }
    }
  },

  methods: {
    removeAdmin: function() {
      window.mp.triggerServer('phone::msg::manage::removeAdmin', this.messengerData.CurrentChat.Id, this.messengerData.CanalProfile.Id);
      
      this.isPopup = false;
      this.setTab({ key: 'GeneralTab' });
    }
  },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
    permissions: function() {
      const groupPermissions = [1,3,4]
      const intPermissions = this.messengerData.CanalProfile.Permissions;

      return groupPermissions
        .map((key) => {
          return {
            hasPermission: intPermissions.includes(key),
            title: this.abilityTitles[key],
            key: key
          };
        });
    },

    currentStatus: function() {
      return this.messengerData.CanalProfile.IsOnline ? 'Online' : 'Offline'
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
.profile-tab {
  width: 100%;
  height: 100%;
  padding: 2.5rem 0 0 0;
  display: flex;
  flex-direction: column;
  background: #FAFAFA;
  position: relative;
  &__header {
    padding: 0 0.75rem;
    position: relative;
    display: flex;
    align-items: center;
    .back-button {
      background: transparent;
      display: flex;
      align-items: center;
      &:hover {
        .back-button__icon {
          animation: shake 0.4s;
        }
      }
      &__icon {
        width: 0.6rem;
        height: 1rem;
      }
      &__text{
        font-size: .7rem;
        line-height: 1rem;
        color: #007AFF;
        margin-left: .4rem;
      }
    }
    .title {
      position: absolute;
      transform: translate(-50%, 0);
      left: 50%;
      font-size: 0.7rem;
    }
  }
  &__info {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 1rem 0;
    background: #FFFFFF;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    margin: 1rem 0 0 0;
    .avatar {
      width: 4rem;
      height: 4rem;
      position: relative;
      background: center / cover no-repeat url("/img/hud/smartphone/messengerTab/chatSection/canalOptions/avatar.png");
      &__indicator {
        width: 0.8rem;
        height: 0.8rem;
        border: 0.1rem solid #FFFFFF;
        background: #4BDB4B;
        position: absolute;
        top: 0.15rem;
        right: 0.15rem;
        border-radius: 50%;
      }
    }
    .username {
      line-height: 1.15rem;
      color: #151515;
      margin: 0.75rem 0 0 0;
    }
    .status {
      margin: 0.25rem 0 0 0;
      font-size: 0.7rem;
      line-height: 0.8rem;
      color: #AAAAAA;
    }
  }
  &__abilities {
    display: flex;
    flex-direction: column;
    margin: 1.25rem 0 0 0;
    .title {
      font-size: 0.7rem;
      font-weight: 300;
      line-height: 0.8rem;
      color: #8A8A8E;
      padding: 0 0 0.5rem 0.75rem;
    }
    .list {
      display: flex;
      flex-direction: column;
      border-top: 1px solid rgba(195, 195, 195, 0.35);
      border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      padding: 0 0 0 0.75rem;
      background: #FFFFFF;
    }
  }
  &__btn {
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    background: #fff;
    height: 2.2rem;
    
    margin: 0.9rem 0 0 0;
    font-size: 0.7rem;
    &-add {
      color: #4E6ED9;
    }
    &-remove {
      color: #FF3B30;
    }
  }
  &__layer {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.7)
  }
  &__popup {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    margin: auto;
    width: 14.2rem;
    height: 13.05rem;
    display: flex;
    flex-direction: column;
    align-items: center;
    background: #FAFAFA;
    border-radius: 0.6rem;
    padding: 1.75rem 1.5rem 1.5rem;
    .title {
      line-height: 1.15rem;
      color: #020202;
      text-align: center;
      font-weight: 500;
    }
    .desc {
      font-size: 0.7rem;
      text-align: center;
      line-height: 0.9rem;
      color: rgba(2, 2, 2, 0.4);
      margin: 0.4rem 0 0 0;
    }
    .btn-confirm {
      width: 100%;
      height: 2rem;
      border-radius: 5rem;
      background: #4E6ED9;
      font-size: 0.7rem;
      color: #fff;
      transition: all 0.2s;
      margin: 1.5rem 0 0 0;
      &:hover {
        box-shadow: 0px 1rem 1.5rem rgba(78, 110, 216, 0.3);
      }
    }
    .btn-cancel {
      background: transparent;
      font-size: 0.7rem;
      line-height: 0.8rem;
      color: #4E6ED9;
      transition: all 0.2s;
      margin: 1rem 0 0 0;
      &:hover {
        color: #1C3CA7;
      }
    }
  }
}
</style>
