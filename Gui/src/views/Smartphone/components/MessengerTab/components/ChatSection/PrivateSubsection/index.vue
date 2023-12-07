<template>
  <div class="private-subsection">
    <preloader-circle v-if="messengerData.CurrentChat.IsLoading" />
    <div v-else class="private-subsection__content">
      <div class="private-subsection__header">
        <back-button :location="{ key: currentTab.key, section: 'GeneralSection' }" @extraClick="closeCurrentChat" />
        <div class="avatar">
          <div class="avatar__indicator" v-if="messengerData.CurrentChat.IsOnline"></div>
        </div>
        <div class="info">
          <span class="info__name">{{ messengerData.CurrentChat.Name }}</span>
          <span class="info__desc">{{ messengerData.CurrentChat.IsOnline ? 'Online' : 'Offline' }}</span>
        </div>
        <button class="btn btn-phone"></button>
        <button class="btn btn-ellipsis" @click="msg_openAccount(messengerData.CurrentChat.AccountId)"></button>
      </div>
      <div v-bar>
        <div class="private-subsection__main" ref="messagesContainer">
          <message-block
            v-for="(block, index) in currentMessageBlocks"
            :key="index"
            :block="block"
          />
        </div>
      </div>
      <div :class="[{ more: additionalData.show }, 'private-subsection__footer']">
        <div class="main">
          <chat-input
            :value="inputModel"
            @input="inputModel = $event"
            @send="sendMessage"
          />
          <button
            v-if="!additionalData.show"
            class="btn clip"
            @click="openAdditional('choice')"
          ></button>
          <button
            v-else
            class="btn cross"
            @click="closeAdditional"
          ></button>
        </div>
        <div class="additional" v-if="additionalData.show">
          <div class="additional__list" v-if="additionalData.screen ==='choice'">
            <div
              v-for="(item, index) in additionalItems"
              :key="index"
              class="item"
              @click="item.event(item.key)"
            >
              <div class="item__icon">
                <img :src="`img/hud/smartphone/messengerTab/chatSection/${item.icon}.svg`" class="img">
              </div>
              <span class="item__text">{{ loc(item.text) }}</span>
            </div>
          </div>
          <div class="additional__image-block" v-else-if="additionalData.screen ==='picture'">
            <div class="img">
              <img
                :src="additionalData.imgUrl || 'img/hud/smartphone/messengerTab/chatSection/imageBg.png'"
                @error="(event) => {event.target.src='img/hud/smartphone/messengerTab/chatSection/imageBg.png'}"
                class="img__main"
              >
            </div>
            <div class="actions">
              <input
                type="text"
                :placeholder="loc('phone:group:main:plh')"
                class="actions__field"
                v-model.trim="additionalData.imgUrl"
                maxlength="130"
              >
              <button class="actions__btn" @click="sendMessage"></button>
            </div>
          </div>
        </div>
      </div>
      <transition name="smartphone-fade">
        <div class="layer" v-if="isJoinPopup"></div>
      </transition>
      <transition name="smartphone-bottom-to-top-slide">
        <join-chat-popup v-if="isJoinPopup" v-bind:inviteCode="inviteCode" @close="isJoinPopup = false" />
      </transition>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapState, mapActions, mapGetters } from 'vuex'
import BackButton from '../common/BackButton'
import ChatInput from '../common/ChatInput'
import MessageBlock from './MessagesBlock'
import { getTextWithTag } from "../common/textEmojiConverter";
import PreloaderCircle from '../../PreloaderCircle'
import JoinChatPopup from '../common/JoinChatPopup';

export default {
  name: 'PrivateSubsection',

  components: {
    BackButton,
    ChatInput,
    MessageBlock,
    PreloaderCircle,
    JoinChatPopup
  },

  data: function() {
    return {
      currentMessageBlocks: null,
      isJoinPopup: false,
      inviteCode: null,
      inputModel: null,
      additionalData: {
        show: false,
        screen: null,
        imgUrl: null
      },
      additionalItems: [
        { icon: 'pictureIcon', text: 'phone:group:main:4', key: 'picture', event: this.setAdditionalDataScreen },
        { icon: 'pin', text: 'phone:group:main:5', key: null, event: this.sendGeoposition }
      ],
    }
  },

  computed: {
    ...mapState('smartphone', ['currentTab']),
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
  },

  watch: {
    messengerData: {
      handler: function() {
        this.setCurrentMessageBlocks();
      },
      deep: true
    } 
  },

  methods: {
    ...mapMutations('smartphone', ['setCurrentTab']),
    ...mapActions("smartphone/messagePage", ['msg_openAccount', 'msg_dropCurrentChat']),

    closeCurrentChat: function() {
      this.msg_dropCurrentChat();
    },

    setCurrentMessageBlocks: function() {
      const monthNames= ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
      const items = this.messengerData.CurrentChat.Items
      const sortItems = items.sort((a, b) => a.Time - b.Time)
      const groups = sortItems.reduce((groups, item) => {
        const date = new Date(item.Time);
        const dateStr = `${date.getUTCDate()} ${monthNames[date.getUTCMonth()]}`;
        
        if (!groups[dateStr]) {
          groups[dateStr] = []
        }

        groups[dateStr].push(item)
        
        return groups
      }, {})
      this.currentMessageBlocks = Object.keys(groups).map((date) => {
        return {
          date: `${date}`,
          items: groups[date]
        }
      })
    },

    openAdditional: function(screen) {
      this.additionalData.show = true
      this.additionalData.screen = screen
    },

    closeAdditional: function() {
      this.additionalData.show = false
      this.additionalData.screen = null
      this.additionalData.imgUrl = null
    },

    setAdditionalDataScreen: function(screen) {
      this.additionalData.screen = screen
    },

    sendMessage: function() {
      const data = { };
      
      if (this.inputModel) {
        data.text = this.inputModel;
      }

      if (this.additionalData.imgUrl) {
        data.attachment = { type: 2, photoUrl: this.additionalData.imgUrl};
      }

      this.inputModel = null
      this.closeAdditional()

      if (data.text && data.text.trim() === "" && !this.additionalData.imgUrl) return;
      
      this.sendMessageToServer(data);
    },

    sendGeoposition: function() {
      const data = {
        text: null,
        attachment: {
          type: 0
        }
      }

      this.closeAdditional()
      this.sendMessageToServer(data);
    },

    sendMessageToServer: function(messageData) {
      const text = JSON.stringify(messageData);
      const textWithTags = getTextWithTag(text);
      window.mp.__triggerServer('phone::msg::sendMessage', this.messengerData.CurrentChat.Id, textWithTags);
    }
  },

  created: function() {
    this.setCurrentMessageBlocks();
  },

  mounted: function() {
    if (this.$refs.messagesContainer) {
      const container = this.$refs.messagesContainer;
      container.scrollTop = container.scrollHeight;
    }

    this._clickInviteUrlHandler = (code) => {
      this.inviteCode = code;
      this.isJoinPopup = true;
    };
    
    this.$bus.$on('phone::msg::clickInviteUrl', this._clickInviteUrlHandler);
  },

  beforeDestroy: function() {
    this._clickInviteUrlHandler && this.$bus.$off('phone::msg::clickInviteUrl', this._clickInviteUrlHandler);
  },

  updated: function() {
    const container = this.$refs.messagesContainer;
    container.scrollTop = container.scrollHeight;
  }
}
</script>

<style lang="scss" scoped>
.private-subsection {
  height: 100%;
  width: 100%;
  position: relative;
  &__content {
    height: 100%;
    width: 100%;
    background: center / cover no-repeat url("/img/hud/smartphone/messengerTab/chatSection/bg.png");
    display: flex;
    flex-direction: column;
  }
  &__header {
    height: 5rem;
    background: #fff;
    border-bottom: 1px solid #E9ECF0;
    display: flex;
    padding: 2.25rem 0.75rem 0.75rem 0.75rem;
    display: flex;
    align-items: center;
    .avatar {
      width: 2rem;
      height: 2rem;
      background: center / contain no-repeat url("/img/hud/smartphone/messengerTab/user.svg");
      position: relative;
      margin: 0 0 0 0.65rem;
      &__indicator {
        position: absolute;
        top: 1px;
        right: 1px;
        width: 0.6rem;
        height: 0.6rem;
        border-radius: 50%;
        border: 0.1rem solid #fff;
        background: #4BDB4B;
      }
    }
    .info {
      display: flex;
      flex-direction: column;
      margin: 0 0 0 0.5rem;
      &__name {
        font-size: 0.7rem;
        line-height: 0.8rem;
        font-weight: 500;
        color: #020202;
        width: 6.9rem;
        text-overflow: ellipsis;
        overflow: hidden;
        white-space: nowrap;
      }
      &__desc {
        font-size: 0.65rem;
        line-height: 16px;
        color: #9E9EA2;
        margin: 0.1rem 0 0 0;
      }
    }
    .btn {
      width: 1rem;
      height: 1rem;
      background: #A7B7EC;
      margin: 0 0 0 0.5rem;
      &:hover {
        background: #4E6ED9;
      }
      &-phone {
        mask: url("/img/hud/smartphone/messengerTab/chatSection/phone.svg") no-repeat center / contain;
        background: #fff;

        &:hover {
          background: #fff;
        }
      }
      &-ellipsis {
        mask: url("/img/hud/smartphone/messengerTab/chatSection/ellipsis.svg") no-repeat center / contain;
      }
    }
  }
  &__main {
    height: 24rem !important;
    width: 100%;
    position: relative;
  }
  &__footer {
    display: flex;
    flex-direction: column;
    width: 100%;
    background: #fff;
    border-top: 1px solid #F0F0F0;
    &.more {
      height: 26.95rem !important;
      .main {
        padding: 0.75rem 0.75rem 0.75rem 0.75rem;
      }
    }
    .main {
      padding: 0.75rem 0.75rem 1.75rem 0.75rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
      .btn {
        width: 1rem;
        height: 1rem;
        background: #C4C4C4;
        &.clip {
          mask: url("/img/hud/smartphone/messengerTab/chatSection/clip.svg") no-repeat center / contain;
        }
        &.cross {
          mask: url("/img/hud/smartphone/messengerTab/chatSection/cross.svg") no-repeat center / contain;
        }
        &:hover {
          background: #2E4CB0;
        }
      }
    }
    .additional {
      border-top: 1px solid #F0F0F0;
      padding: 0 0 1.5rem 0;
      &__list {
        display: flex;
        flex-direction: column;
        padding: 0 0 0 0.75rem;
        .item {
          display: flex;
          align-items: center;
          height: 2.5rem;
          &:not(:last-child) {
            border-bottom: 1px solid #F0F0F0;
          }
          &__icon {
            width: 1.5rem;
            height: 1.5rem;
            border-radius: 50%;
            background: #4E6ED9;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 0.75rem 0 0;
            .img {
              width: 0.8rem;
              height: 0.8rem;
            }
          }
          &__text {
            font-size: 0.7rem;
            color: #464646;
          }
        }
      }
      &__image-block {
        display: flex;
        flex-direction: column;
        padding: 0.75rem 0.75rem 0 0.75rem;
        .img {
          height: 7.35rem;
          display: flex;
          align-items: center;
          justify-content: center;
          margin: 0 0 0.75rem 0;
          &__main {
            max-width: 100%;
            max-height: 100%;
          }
        }
        .actions {
          display: flex;
          align-items: center;
          &__field {
            height: 2rem;
            width: 11.6rem;
            border-radius: 5rem;
            background: #F3F4F6;
            color: #464646;
            font-size: 0.7rem;
            padding: 0 0.75rem;
            margin: 0 0.7rem 0 0;
            &::placeholder {
              color: rgba(2, 2, 2, 0.4);
            }
          }
          &__btn {
            width: 1.4rem;
            height: 1.4rem;
            background: #C4C4C4;
            mask: center / cover no-repeat url("/img/hud/smartphone/messengerTab/chatSection/airplane.svg");
            transition: all 0.15s;
            &:hover {
              background: #4E6ED9;
            }
          }
        }
      }
    }
  }
  .layer {
    position: absolute;
    width: 100%;
    height: 33rem;
    background: rgba(0, 0, 0, 0.7);
    top: 0;
    left: 0;
    z-index: 43;
  }
}
</style>
