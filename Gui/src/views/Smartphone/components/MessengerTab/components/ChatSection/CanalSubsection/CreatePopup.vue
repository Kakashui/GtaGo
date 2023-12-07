<template>
  <div class="create-popup">
    <button class="btn-cancel" @click="close">{{loc('phone:cancel')}}</button>
    <span class="title">{{loc('phone:chat:crate:2')}}</span>
    <p class="desc">{{loc('phone:chat:crate:3')}}</p>
    <div class="actions">
      <input
        type="text"
        class="actions__input"
        v-model="imgUrl"
        :placeholder="loc('phone:chat:crate:plh:1')"
      >
      <input
        type="text"
        class="actions__input"
        v-model="title"
        :placeholder="loc('phone:chat:crate:plh:2')"
      >
      <textarea
        class="actions__textarea"
        :placeholder="loc('phone:chat:crate:plh:3')"
        maxlength="500"
        v-model="description"
        @input="resize"
        ref="textarea"
      ></textarea>
    </div>
    <button class="btn-publish" @click="create">{{loc('phone:chat:crate:4')}}</button>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'CreatePopup',

  data: function() {
    return {
      imgUrl: '',
      title: '',
      description: '',
    }
  },

  computed: {
    textareaStyle: function() {
      return { 'min-height': this.textareaHeight }
    },
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    resize: function() {
      this.$refs.textarea.style.height = 'auto'
      this.$refs.textarea.style.height = `${this.$refs.textarea.scrollHeight}px`
    },

    close: function() {
      this.$emit('closeCreation')
    },

    create: function() {
      this.$emit('create', this.imgUrl, this.title, this.description);
    }
  },

  mounted: function() {
    this.$refs.textarea.style.height = `${this.$refs.textarea.scrollHeight}px`
  }
}
</script>

<style lang="scss" scoped>
.create-popup {
  background: #FAFAFA;
  border-top-right-radius: 0.6rem;
  border-top-left-radius: 0.6rem;
  height: 26.5rem;
  width: 100%;
  position: absolute;
  bottom: 0;
  left: 0;
  padding: 1rem 0 1.5rem 0;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  &::after {
    content: '';
    position: absolute;
    top: 0;
    right: 0;
    height: 4rem;
    width: 3.35rem;
    background: center / contain no-repeat url("/img/hud/smartphone/messengerTab/chatSection/megaphone.png");
  }
  .btn-cancel {
    color: #4E6ED9;
    font-size: 0.7rem;
    line-height: 0.8rem;
    background: transparent;
    margin: 0 0 0 1rem;
    &:hover {
      text-decoration: underline;
    }
  }
  .title {
    font-size: 1.3rem;
    line-height: 1.5rem;
    color: #020202;
    font-weight: bold;
    margin: 0.7rem 0 0 1rem;
  }
  .desc {
    color: rgba(2, 2, 2, 0.4);
    font-size: 0.7rem;
    line-height: 0.9rem;
    margin: 0.25rem 0 0 1rem;
  }
  .actions {
    background: rgba(195, 195, 195, 0.1);
    width: 100%;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0 0 0 1rem;
    margin: 1rem 0 0 0;
    display: flex;
    flex-direction: column;
    &__input {
      background: transparent;
      width: 100%;
      height: 2.35rem;
      font-size: 0.7rem;
      color: #020202;
      &:nth-child(2) {
        border-top: 1px solid rgba(195, 195, 195, 0.35);
        border-bottom: 1px solid rgba(195, 195, 195, 0.35);
      }
      &::placeholder {
        color: rgba(2, 2, 2, 0.4);
      }
    }
    &__textarea {
      resize: none;
      width: 13.7rem;
      background: transparent;
      border: none;
      outline: none;
      font-size: 0.7rem;
      line-height: 1rem;
      overflow-y: auto;
      max-height: 6.5rem;
      color: #020202;
      padding: 1rem 0.5rem 1rem 0;
      &::placeholder {
        color: rgba(2, 2, 2, 0.4);
      }
      &::-webkit-scrollbar-track {
        background-color: transparent;
        margin: 1rem 0;
      }
      &::-webkit-scrollbar {
        width: 0.1rem;
        background-color: transparent;
      }
      &::-webkit-scrollbar-thumb {
        background: rgba(0, 0, 0, 0.3);
        border-radius: 0.1rem;
      }
    }
  }
  .btn-publish {
    margin: auto auto 0 auto;
    background: #4E6ED9;
    border-radius: 5rem;
    color: #fff;
    font-size: 0.7rem;
    height: 2rem;
    width: 13.2rem;
    transition: all 0.2s;
    &:hover {
      box-shadow: 0px 0.5rem 1.5rem rgba(78, 110, 216, 0.3);
      transform: translate(0, -5%);
    }
  }
}
</style>
