<template>
  <div class="add-popup">
    <div class="add-popup__header">
      <button class="close" @click="$emit('close')">{{loc('phone:cancel')}}</button>
      <span class="title">{{loc('phone:contact:add:2')}}</span>
      <p class="desc">{{loc('phone:contact:add:5')}}</p>
    </div>
    <div class="add-popup__actions">
      <input
        type="text"
        class="item nickname"
        v-model="nickname"
        :placeholder="loc('phone:contact:add:plh')"
      >
      <span class="item username">@{{ messengerData.viewedProfile.Username }}</span>
    </div>
    <button class="add-popup__btn" @click="add">{{loc('phone:contact:add:4')}}</button>
  </div>
</template>

<script>
import { mapGetters, mapState } from 'vuex';

export default {
  name: 'AddPopup',

  data: function() {
    return {
      nickname: 'long long nickname'
    }
  },

  computed: {
    ...mapState('smartphone/messagePage', ['messengerData']),
    ...mapGetters('localization', ['loc'])
  },

  methods: {
    add: function() {
      this.$emit('close');
      window.mp.triggerServer('phone::msg::addcontact', this.messengerData.viewedProfile.Username, this.nickname);
    }
  }
}
</script>

<style lang="scss" scoped>
.add-popup {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 20.5rem;
  background: 100% 0 / 3.35rem 4rem no-repeat url("/img/hud/smartphone/messengerTab/userBgIcon.png"), #FAFAFA;
  border-radius: 0.6rem 0.6rem 0 0;
  display: flex;
  flex-direction: column;
  &__header {
    display: flex;
    flex-direction: column;
    padding: 1rem;
    .close {
      align-self: flex-start;
      font-size: 0.7rem;
      line-height: 16px;
      margin: 0 0 0.7rem 0;
      background: transparent;
      color: #4E6ED9;
      &:hover {
        text-decoration: underline;
      }
    }
    .title {
      font-size: 1.3rem;
      line-height: 1.5rem;
      font-weight: bold;
      color: #020202;
      margin: 0 0 0.25rem 0;
    }
    .desc {
      font-size: 0.7rem;
      line-height: 0.9rem;
      color: rgba(2, 2, 2, 0.4);
    }
  }
  &__actions {
    background: rgba(195, 195, 195, 0.1);
    display: flex;
    flex-direction: column;
    border-top: 1px solid rgba(195, 195, 195, 0.35);
    border-bottom: 1px solid rgba(195, 195, 195, 0.35);
    padding: 0 0 0 1rem;
    .item {
      height: 2.4rem;
      font-size: 0.7rem;
      &.nickname {
        color: #020202;
        border-bottom: 1px solid rgba(195, 195, 195, 0.35);
        background: transparent;
        &::placeholder {
          color: rgba(2, 2, 2, 0.4);
        }
      }
      &.username {
        font-size: 0.7rem;
        color: #4E6ED9;
        line-height: 2.4rem;
      }
    }
  }
  &__btn {
    height: 2rem;
    width: 13.2rem;
    box-shadow: 0px 10px 30px rgba(78, 110, 216, 0.3);
    background: #4E6ED9;
    color: #fff;
    font-size: 0.7rem;
    border-radius: 5rem;
    transition: all 0.15s;
    margin: auto auto 1.5rem auto;
    &:hover {
      box-shadow: none;
    }
  }
}
</style>
