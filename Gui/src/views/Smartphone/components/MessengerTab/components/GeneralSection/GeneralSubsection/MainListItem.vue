<template>
  <div class="main-list-item" @click="msg_openChat(item.Id)">
    <div class="main-list-item__avatar" v-if="item.Type === 0">
      <img
        src='img/hud/smartphone/messengerTab/user.svg'
        :alt="item.Type"
        class="icon"
      >
      <div v-if="item.IsOnline" class="indicator"></div>
    </div>
    <div v-else
        :style="{ background: item.IconColors, '--bgImg': getChatImageByType(item.Type) }"
        class="main-list-item__avatar_group"
    ></div>
    <div class="main-list-item__main">
      <div class="block-left">
        <span class="title">{{ item.Name }}</span>
        <span class="message">{{ message }}</span>
      </div>
      <div class="block-right">
        <span class="date">{{ currentTime }}</span>
        <span class="unread" v-if="item.NonReadMessagesCount">{{ item.NonReadMessagesCount }}</span>
      </div>
    </div>
  </div>
</template>

<script>
import { getTextWithEmoji } from "../../ChatSection/common/textEmojiConverter";
import { mapActions } from 'vuex'

export default {
  name: 'MainLIstItem',

  props: { item: Object },

  data: function() {
    return {
      subsections: [
        'PrivateSubsection',
        'GroupSubsection',
        'CanalSubsection'
      ]
    }
  },

  computed: {
    message: function() {
      return this.item?.LastMessage?.Text ? this.getTextWithEmoji(this.item.LastMessage.Text) : '';
    },

    currentTime: function() {
      const options = { timeZone: 'UTC', hour12: false, hour: 'numeric', minute: 'numeric'}
      const date = new Intl.DateTimeFormat(undefined, options).format(this.item.ActionTime)

      return date
    },

    currentSubsection: function() {
      return this.subsections[this.item.Type]
    }
  },

  methods: {
    ...mapActions('smartphone/messagePage', ['msg_openChat']),
    
    getChatImageByType: function(type) {
      switch (type) {
        case 1:
          return 'center / cover no-repeat url("/img/hud/smartphone/messengerTab/chatSection/groupAvatar.svg")';
        case 2:
          return 'center / cover no-repeat url("/img/hud/smartphone/messengerTab/megaphone.svg")';
      }
    },

    getTextWithEmoji: function(text) {
      return getTextWithEmoji(text);
    }
  }
}
</script>

<style lang="scss" scoped>
.main-list-item {
  display: flex;
  align-items: center;
  position: relative;
  height: 3.5rem;
  &__avatar {
    margin: 0 0 0 0.75rem;
    flex: 0 0 2.5rem;
    height: 2.5rem;
    position: relative;
    .icon {
      width: 100%;
      height: 100%;
    }
    .indicator {
      position: absolute;
      width: 0.6rem;
      height: 0.6rem;
      border: 0.1rem solid #fff;
      background: #4BDB4B;
      border-radius: 50%;
      right: 1px;
      top: 1px;
    }
  }
  &__avatar_group {
      margin: 0 0 0 0.75rem;
      flex: 0 0 2.5rem;
      height: 2.5rem;
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 50%;
      &::after {
        content: "";
        width: 0.9rem;
        height: 0.9rem;
        background: var(--bgImg);
      }
    }
  &__main {
    flex: auto;
    height: 100%;
    margin: 0 0 0 0.6rem;
    padding: 0.45rem 0.75rem 0.45rem 0;
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    display: flex;
    justify-content: space-between;
    .block-left {
      display: flex;
      flex-direction: column;
      justify-content: center;
      .title {
        font-size: 0.7rem;
        font-weight: 500;
        line-height: 0.8rem;
        color: #020202;
      }
      .message {
        margin: 0.25rem 0 0 0;
        font-size: 0.65rem;
        line-height: 0.8rem;
        color: #9E9EA2;
        display: -webkit-box;
        width: 9rem;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;  
        overflow: hidden;
      }
    }
    .block-right {
      display: flex;
      flex-direction: column;
      align-items: center;
      font-size: 0.65rem;
      line-height: 0.75rem;
      .date {
        color: #9E9EA2;
      }
      .unread {
        margin: 0.5rem 0 0 0;
        background: #007AFF;
        font-weight: 500;
        text-align: center;
        border-radius: 5rem;
        padding: 0 0.25rem;
        height: 1rem;
        line-height: 1rem;
        min-width: 1rem;
        color: #fff;
      }
    }
  }
}
</style>
