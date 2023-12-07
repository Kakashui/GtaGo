<template>
  <div class="options-subsection">
    <div class="options-subsection__header">
      <div class="block-top">
        <back-button :isText="true" :location="{ key: currentTab.key, section: 'ChatSection', subsection: 'GroupSubsection', data: { location: { key: currentTab.key, section: currentTab.section, subsection: currentTab.subsection }} }"/>
        <button class="block-top__btn" v-if="messengerData.CurrentChat.CanChangingGroupProfile" @click="isPopup = true">{{loc('phone:group:subj:1')}}</button>
      </div>
      <div class="block-bottom">
        <div class="block-bottom__avatar" :style="{ background: messengerData.CurrentChat.IconColors }"></div>
        <span class="block-bottom__title">{{ messengerData.CurrentChat.Name }}</span>
        <div class="block-bottom__info">
          <span class="participants">{{loc(`phone:group:subj:2@${messengerData.CurrentChat.SubscribersCount}`)}}</span>
        </div>
        <div class="block-bottom__actions">
          <div
            v-for="(item, index) in navButtons"
            :key="index"
            class="item"
            @click="item.event"
          >
            <div :style="{ 'background-image': `url(img/hud/smartphone/messengerTab/chatSection/groupOptions/${item.icon}.svg)`}" class="item__icon"></div>
            <span class="item__text">{{ loc(item.title) }}</span>
          </div>
        </div>
      </div>
    </div>
    <div v-bar>
      <div class="options-subsection__main">
        <div class="options-subsection__info">
          <div class="link">
            <div class="link__text">{{loc('phone:group:subj:3')}}</div>
            <div class="link__action" @click="copyUrl">{{ getChatInviteUrl() }}</div>
          </div>
          <div class="desc">
            <div class="desc__text">{{loc('phone:group:subj:4')}}</div>
            <div :class="[{ full: isFullDescription }, 'desc__value']">{{ messengerData.CurrentChat.Description }}</div>
            <button
              class="desc__btn-more"
              v-if="!isFullDescription"
              @click="isFullDescription = true"
            >{{loc('phone:group:subj:5')}}</button>
          </div>
        </div>
        <div class="options-subsection__actions">
          <div 
            class="item"
            v-for="item in getAvailableButtons()"
            :key="item.id"
            @click="setCurrentTab({key: currentTab.key, section: currentTab.section, subsection: item.subsection })"
          >
            {{loc(item.text)}}
          </div>
        </div>
      </div>
    </div>
    <transition name="smartphone-fade">
      <div class="layer" v-if="isInvite || isPopup"></div>
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <edit-popup v-if="isPopup" @close="isPopup = false" />
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <popup-invite v-if="isInvite" @close="isInvite = false" />
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import BackButton from '../common/BackButton'
import EditPopup from './EditPopup'
import PopupInvite from './PopupInvite'
import { getInviteUrl } from "../../common/invitesUrlConverter";

export default {
  name: 'GroupOptionsSubsection',

  components: {
    BackButton,
    EditPopup,
    PopupInvite
  },

  data: function() {
    return {
      isPopup: false,
      isInvite: false,
      buttons: [
        { icon: 'add', title: 'phone:group:subj:6', event: this.openInvite },
        { icon: 'mute', title: 'phone:group:subj:7', event: this.toggleMuteChat },
        { icon: 'exit', title: 'phone:group:subj:8', event: this.exitChat },
      ],
      navigationsItems: [
        {
          text: 'phone:group:subj:9',
          subsection: 'GroupControlSubsection',
          permissionName: 'CanPurposingAdmins'
        },
        {
          text: 'phone:group:subj:10',
          subsection: 'GroupBlacklistSubsection',
          permissionName: 'CanBlockingAccounts'
        }
      ],
      isFullDescription: false
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
    navButtons: function() {
      if (this.messengerData.CurrentChat.AdminLvl === 2) {
        const exitButton = this.buttons.find(b => b.icon === 'exit');
        exitButton.title = 'phone:group:subj:11';
        exitButton.event = this.deleteChat;
      }

      const muteButton = this.buttons.find(b => b.icon === 'mute' || b.icon === 'unmute');
      muteButton.icon = (this.messengerData.CurrentChat.IsMuted) ? 'mute' : 'unmute';

      return this.buttons;
    }
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),

    getChatInviteUrl: function() {
      const code = this.messengerData.CurrentChat.InviteCode;
      return getInviteUrl(code);
    },

    copyUrl: function() {
      const url = this.getChatInviteUrl();
      this.$clipboard(url);
    },

    openInvite: function() {
      this.isInvite = true
    },

    getAvailableButtons: function() {
      return this.navigationsItems.filter(b => !b.permissionName || this.messengerData.CurrentChat[b.permissionName]);
    },

    exitChat: function() {
      window.mp.triggerServer('phone::msg::leaveChat', this.messengerData.CurrentChat.Id);
    },

    deleteChat: function() {
      window.mp.triggerServer('phone::msg::deleteChat', this.messengerData.CurrentChat.Id);
    },

    toggleMuteChat: function() {
      window.mp.triggerServer('phone::msg::muteChat', this.messengerData.CurrentChat.Id, !this.messengerData.CurrentChat.IsMuted);
    }
  }
}
</script>

<style lang="scss" scoped>
.options-subsection {
  display: flex;
  flex-direction: column;
  &__header {
    padding: 3rem 0 1rem 0;
    background: #fff;
    position: relative;
    border-bottom: 1px solid #E9ECF0;
    .block-top {
      position: absolute;
      transform: translate(0, -50%);
      top: 3rem;
      left: 0;
      display: flex;
      width: 100%;
      justify-content: space-between;
      padding: 0 0.75rem;
      &__btn {
        background: transparent;
        font-size: 0.7rem;
        line-height: 0.8rem;
        color: #4E6ED9;
        &:hover {
          text-decoration: underline;
        }
      }
    }
    .block-bottom {
      display: flex;
      flex-direction: column;
      align-items: center;
      &__avatar {
        width: 4.5rem;
        height: 4.5rem;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 50%;
        margin: 0 0 0.75rem 0;
        &::after {
          content: "";
          width: 2rem;
          height: 2rem;
          background: center / contain no-repeat url("/img/hud/smartphone/messengerTab/chatSection/groupOptions/users.png");
        }
      }
      &__title {
        font-size: 0.8rem;
        line-height: 0.95rem;
        color: #020202;
        margin: 0 0 0.1rem 0;
      }
      &__info {
        font-size: 0.7rem;
        line-height: 0.8rem;
        color: #AAAAAA;
        margin: 0 0 0.75rem 0;
      }
      &__actions {
        display: flex;
        justify-content: space-between;
        width: 100%;
        padding: 0 2rem;
        .item {
          display: flex;
          flex-direction: column;
          align-items: center;
          &:hover {
            .item__text {
              text-decoration: underline;
            }
          }
          &:last-child {
            .item__icon {
              background-color: #FF3B30;
            }
            .item__text {
              color: #FF3B30;
            }
          }
          &__icon {
            width: 2rem;
            height: 2rem;
            border-radius: 50%;
            background-color: #4E6ED9;
            box-shadow: 0px 0.2rem 0.5rem rgba(236, 234, 244, 0.5);
            margin: 0 0 0.3rem 0;
            background-position: center;
            background-size: 0.8rem;
            background-repeat: no-repeat;
          }
          &__text {
            color: #4E6ED9;
            font-size: 0.55rem;
            line-height: 0.65rem;
          }
        }
      }
    }
  }
  &__main {
    height: 18.2rem !important;
    display: flex;
    flex-direction: column;
    padding: 0.5rem 0 0 0;
  }
  &__info {
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0.75rem 0 0.75rem 1rem;
    margin: 0 0 0.5rem 0;
    display: flex;
    flex-direction: column;
    background: #FFFFFF;
    .link {
      display: flex;
      flex-direction: column;
      padding: 0 0 0.7rem 0;
      border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      &__text {
        font-size: 0.65rem;
        line-height: 0.75rem;
        color: rgba(2, 2, 2, 0.4);
        margin: 0 0 0.15rem 0;
      }
      &__action {
        font-size: 0.7rem;
        line-height: 0.8rem;
        color: #4E6ED9;
        &:hover {
          text-decoration: underline;
        }
      }
    }
    .desc {
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      padding: 0.6rem 0 0 0;
      &__text {
        font-size: 0.65rem;
        line-height: 0.75rem;
        color: rgba(2, 2, 2, 0.4);
        margin: 0 0 0.15rem 0;
      }
      
      &__value {
        margin: 0 0 0.25rem 0;
        font-size: 0.7rem;
        line-height: 0.9rem;
        color: #454545;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
        overflow: hidden;
        text-overflow: ellipsis;
        &.full {
          display: block;
        }
      }
      &__btn-more {
        background: transparent;
        font-size: 0.7rem;
        line-height: 0.8rem;
        color: #4E6ED9;
        &:hover {
          text-decoration: underline;
        }
      }
    }
  }
  &__actions {
    display: flex;
    flex-direction: column;
    padding: 0 0 0 1rem;
    background: #fff;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    .item {
      @keyframes shake {
        25% {
          transform: translateX(-0.5rem);
        }
        50% {
          transform: translateX(0);
        }
        75% {
          transform: translateX(-0.25rem);
        }
      }
      font-weight: normal;
      font-size: .7rem;
      color: #020202;
      height: 2rem;
      display: flex;
      align-items: center;
      justify-content: flex-start;
      position: relative;
      &:not(:last-child) {
        border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      }
      &:after{
        content: '';
        width: .4rem;
        height: .7rem;
        position: absolute;
        background-image: url('/img/hud/smartphone/messengerTab/chatSection/canalOptions/arr-right.svg');
        background-size: contain;
        background-position: center;
        background-repeat: no-repeat;
        position: absolute;
        right: .75rem;
      }
      &:hover::after {
        animation: shake 0.4s;
      }
      &.delete{
        color: #FF3B30;
        &:after{
          content: none;
      }
    }
    }
  }
  .layer {
    position: absolute;
    top: 0;
    display: flex;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.7);
  }
}
</style>
