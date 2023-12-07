<template>
  <div class="edit-popup">
    <div class="edit-popup__header">
      <div class="btn btn-cancel" @click="$emit('close')">{{loc('phone:cancel')}}</div>
      <div class="btn btn-delete" @click="$emit('delete')">{{loc('phone:chat:crate:5')}}</div>
    </div>
    <span class="edit-popup__title">{{loc('phone:chat:crate:6')}}</span>
    <div class="edit-popup__actions">
      <input
        type="text"
        class="input"
        :placeholder="loc('phone:chat:crate:plh:1')"
        v-model="imgUrl"
      >
      <input
        type="text"
        class="input"
        :placeholder="loc('phone:chat:crate:plh:2')"
        v-model="title"
      >
      <textarea
        class="textarea"
        :placeholder="loc('phone:chat:crate:plh:3')"
        maxlength="500"
        v-model="description"
        @input="resize"
        ref="textarea"
      ></textarea>
    </div>
    <button class="edit-popup__btn-publish" @click="confirm">{{loc('phone:chat:crate:7')}}</button>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex'

export default {
  name: 'EditPopup',

  props: {
    currentId: Number
  },

  data: function() {
    return {
      imgUrl: '',
      title: '',
      description: '',
    }
  },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc']),
    textareaStyle: function() {
      return { 'min-height': this.textareaHeight }
    }
  },

  methods: {
    resize: function() {
      this.$refs.textarea.style.height = 'auto'
      this.$refs.textarea.style.height = `${this.$refs.textarea.scrollHeight}px`
    },

    setTextFields: function() {
      const rightItem = this.messengerData.CurrentChat.Items.find(item => item.Id === this.currentId)

      this.imgUrl = rightItem.ImgUrl
      this.title = rightItem.Title
      this.description = rightItem.Text
    },

    confirm: function() {
      this.$emit('confirm', this.imgUrl, this.title, this.description);
    }
  },

  mounted: function() {
    this.resize()
  },

  created: function() {
    this.setTextFields()
  }
}
</script>

<style lang="scss" scoped>
.edit-popup {
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
  &__header {
    display: flex;
    justify-content: space-between;
    width: 100%;
    padding: 0 1rem;
    .btn {
      font-size: 0.7rem;
      line-height: 0.8rem;
      background: transparent;
      &:hover {
        text-decoration: underline;
      }
      &-cancel {
        color: #4E6ED9;
      }
      &-delete {
        color: #FF3B30;
      }
    }
  }
  &__title {
    font-size: 1.3rem;
    line-height: 1.5rem;
    color: #020202;
    font-weight: bold;
    margin: 0.7rem 0 0 1rem;
  }
  &__actions {
    background: rgba(195, 195, 195, 0.1);
    width: 100%;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0 0 0 1rem;
    margin: 1rem 0 0 0;
    display: flex;
    flex-direction: column;
    .input {
      background: transparent;
      width: 100%;
      padding: 0 1rem 0 0;
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
    .textarea {
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
  &__btn-publish {
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
