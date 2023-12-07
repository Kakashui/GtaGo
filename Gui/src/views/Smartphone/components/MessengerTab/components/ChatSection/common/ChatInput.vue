<template>
  <div class="chat-input">
    <input
      type="text"
      :placeholder="loc('phone:chat:plh:input')"
      class="chat-input__field"
      :value="value"
      @input="$emit('input', $event.target.value)"
      @keydown.enter="$emit('send')"
      maxlength="150"
      ref="field"
    >
    <button class="chat-input__btn" @click="isEmojiList = !isEmojiList"></button>
    <transition name="emoji">
      <div class="chat-input__emoji" v-show="isEmojiList">
        <div
          v-for="(item, index) in emojiList"
          :key="index"
          class="item"
          @click="addEmoji(item)"
        >{{ item }}</div>
      </div>
    </transition>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'ChatInput',
  computed: {
    ...mapGetters('localization', ['loc'])
  },
  props: { 
    value: String
  },

  data: function() {
    return {
      isEmojiList: false,
      emojiList: [
        '😃', '😁', '😆', '😅', '🤣', '😂', '🙂',
        '🙃', '😉', '😊', '😇', '😍', '🤩', '😘',
        '🤗', '🤑', '🤪', '😜', '🤐', '🤔', '🤫',
        '😏', '🤢', '🤮', '🥶', '💩', '💋', '❤️'
      ]
    }
  },

  methods: {
    addEmoji: function(emoji) {
      this.$emit('input', (this.value ?? '') + emoji)
      this.$refs.field.focus()
      this.isEmojiList = false
    }
  }
}
</script>

<style lang="scss" scoped>
.chat-input {
  width: 11.7rem;
  height: 2rem;
  position: relative;
  &__field {
    width: 100%;
    height: 100%;
    border-radius: 5rem;
    font-size: 0.65rem;
    color: #464646;
    background: #F3F4F6;
    padding: 0 2.2rem 0 0.75rem;
    &::placeholder {
      color: #6F798C;
    }
  }
  &__btn {
    mask: url("/img/hud/smartphone/messengerTab/chatSection/smile.svg") no-repeat center / contain;
    width: 1rem;
    height: 1rem;
    background: #4E6ED9;
    position: absolute;
    transform: translate(0, -50%);
    top: 50%;
    right: 0.6rem;
    &:hover {
      background: #2E4CB0;
    }
  }
  &__emoji {
    padding: 0.75rem;
    background: #fff;
    box-shadow: 0px 0.5rem 1.5rem rgba(177, 188, 203, 0.5);
    border-radius: 0.5rem;
    position: absolute;
    bottom: calc(100% + 0.25rem);
    display: grid;
    grid-template-columns: repeat(7, 1.4rem);
    grid-auto-rows: 1.4rem;
    grid-gap: .4rem;
  }
}
.emoji-enter-active, .emoji-leave-active {
  transition: all 0.2s;
}
.emoji-enter, .emoji-leave-to {
  transform: scale(0.5);
  opacity: 0;
}
</style>
