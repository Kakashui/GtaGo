<template>
  <div class="canal-options-subsection">
    <div class="canal-options-subsection__header">
      <div class="actions">
        <back-button :isText="true" :location="{ key: currentTab.key, section: 'ChatSection', subsection: 'CanalSubsection' }"/>
        <div class="btn-edit" v-if="messengerData.CurrentChat.CanChangingGroupProfile" @click="isPopup = true">{{loc('phone:chat:opt:1')}}</div>
      </div>
      <div class="avatar" v-bind:style="{ background: messengerData.CurrentChat.IconColors }"></div>
      <div class="canal-name">{{ messengerData.CurrentChat.Name }}</div>
      <div class="canal-subscribers">{{loc(`phone:chat:opt:1:1@${messengerData.CurrentChat.SubscribersCount}`)}}</div>
    </div>
    <div v-bar>
      <div class="main">
        <div class="canal-options-subsection__body-section">
          <div class="canal-options-subsection__body-section__info">
            <div class="title">{{loc('phone:chat:opt:2')}}</div>
            <div class="value value-url" @click="copyUrl">{{ getChatInviteUrl() }}</div>
          </div>
          <div class="canal-options-subsection__body-section__info">
            <div class="title">{{loc('phone:chat:opt:3')}}</div>
            <div :class="[{ full: isFullDescription }, 'value']">{{ messengerData.CurrentChat.Description }}</div>
            <button
              class="btn-more"
              v-if="!isFullDescription"
              @click="isFullDescription = true"
            >{{loc('phone:chat:opt:4')}}</button>
          </div>
        </div>
        <div class="canal-options-subsection__body-section">
          <div 
            class="canal-options-subsection__body-section__interaction"
            v-for="item in getAvailableButtons()"
            :key="item.id"
            @click="setCurrentTab({key: currentTab.key, section: currentTab.section, subsection: item.subsection })"
          >
            <div class="btn">{{loc(item.text)}}</div>
          </div>
          <div class="canal-options-subsection__body-section__interaction">
            <!-- Admin lvl 2 is Owner -->
            <div v-if="messengerData.CurrentChat.AdminLvl === 2" class="btn delete" @click="deleteChannel">{{loc('phone:chat:opt:5')}}</div>
            <div v-else class="btn delete" @click="leaveChannel">{{loc('phone:chat:opt:6')}}</div>
          </div>
        </div>
      </div>
    </div>
    <transition name="smartphone-fade">
      <div class="layer" v-if="isPopup"></div>
    </transition>
    <transition name="smartphone-bottom-to-top-slide">
      <edit-popup v-if="isPopup" @close="isPopup = false" @save="saveChatChanges" />
    </transition>
  </div>
</template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
import BackButton from '../../common/BackButton'
import EditPopup from './EditPopup'
import { getInviteUrl } from "../../common/invitesUrlConverter";

export default {
  name: 'CanalOptionsSubsection',

  components: {
    BackButton,
    EditPopup,
  },

  data: function() {
    return {
      isPopup: false,
      isFullDescription: false,
      buttons: [
        {
          text: 'phone:chat:opt:7',
          subsection: 'CanalControlSubsection',
          permissionName: 'CanPurposingAdmins'
        },
        // {
        //   text: 'phone:chat:opt:8',
        //   subsection: 'CanalSubscribersSubsection'
        // },
        {
          text: 'phone:chat:opt:9',
          subsection: 'CanalBlacklistSubsection',
          permissionName: 'CanBlockingAccounts'
        }
      ]
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
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

    getAvailableButtons: function() {
      return this.buttons.filter(b => !b.permissionName || this.messengerData.CurrentChat[b.permissionName]);
    },

    saveChatChanges: function(title, description, avatar) {
      this.isPopup = false;
      window.mp.triggerServer('phone::msg::updateChat', this.messengerData.CurrentChat.Id, title, description, avatar);
    },

    deleteChannel: function() {
      window.mp.triggerServer('phone::msg::deleteChat', this.messengerData.CurrentChat.Id);
    },

    leaveChannel: function() {
      window.mp.triggerServer('phone::msg::leaveChat', this.messengerData.CurrentChat.Id);
    }
  }
}
</script>

<style lang="scss" scoped>
.canal-options-subsection{
  display: flex;
  flex-flow: column;
  &__header{
    display: flex;
    flex-flow: column;
    align-items: center;
    justify-content: space-between;
    background-color: #fff;
    padding: 3rem 0 .9rem 0;
    width: 100%;
    position: relative;
    height: 11rem;
    border-bottom: 1px solid #E9ECF0;
    .actions {
      position: absolute;
      display: flex;
      justify-content: space-between;
      padding: 0 0.75rem;
      width: 100%;
      transform: translate(0, -50%);
      top: 3rem;
      .btn-edit{
        font-weight: normal;
        font-size: .7rem;
        color: #4E6ED9;
        line-height: 1.3rem;
        &:hover {
          text-decoration: underline;
        }
      }
    }
    .avatar{
      width: 4.5rem;
      height: 4.5rem;
      min-height: 4.5rem;
      border-radius: 50%;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      &:after{
        content: '';
        position: absolute;
        width: 2rem;
        height: 2rem;
        background: center/contain no-repeat url("/img/hud/smartphone/messengerTab/megaphone.svg")
      }
    }
    .canal-name{
      margin-top: .75rem;
      margin-bottom: .1rem;
      font-size: .8rem;
      line-height: .95rem;
      text-align: center;
      color: #020202;
    }
    .canal-subscribers{
      font-size: .7rem;
      line-height: .8rem;
      color: #AAAAAA;
    }
  }
  .main {
    height: 22rem !important;
  }
  &__body-section{
    display: flex;
    flex-flow: column;
    width: 100%;
    background: #FFFFFF;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    box-shadow: 0px 4px 10px rgba(236, 234, 244, 0.5);
    margin: 0.75rem 0 0 0;
    &__info, &__interaction{
      margin-left: 1rem;
      padding: .6rem 0;
      border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      &:last-child{
        border-bottom: 0;
      }
    }
    &__info{
      display: flex;
      flex-flow: column;
      align-content: flex-start;
      .title{
        font-weight: normal;
        font-size: .65rem;
        line-height: .75rem;
        color: rgba(2, 2, 2, 0.4);
        margin-bottom: .15rem;
      }
      .value{
        font-weight: normal;
        font-size: .7rem;
        line-height: .9rem;
        color: #454545;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
        overflow: hidden;
        text-overflow: ellipsis;
        &.full {
          display: block;
        }
        &.value-url{
          color: #4E6ED9;
        }
      }
      .btn-more {
        align-self: flex-start;
        background: transparent;
        color: #4E6ED9;
        font-size: 0.7rem;
        line-height: 0.8rem;
        margin: 0.25rem 0 0 0;
        &:hover {
          text-decoration: underline;
        }
      }
    }
    &__interaction{
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
      &:hover {
        .btn::after {
          animation: shake 0.4s;
        }
      }
      .btn{
        font-weight: normal;
        font-size: .7rem;
        line-height: .8rem;
        color: #020202;
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: flex-start;
        position: relative;
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
        &.delete{
          color: #FF3B30;
          &:after{
            content: none;
          }
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
